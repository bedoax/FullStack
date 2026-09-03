using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.Attempt;
using Models.DTOs.AttemptAnswer;
using Models.DTOs.Question;
using Models.Entities;

namespace Business.Services
{
    public class AttemptService : IAttemptService
    {
        private IUnitOfWork _unitOfWork;
        private IStudentProfileService _studentProfileService;
        private IStudentTopicPerformanceService _studentTopicPerformanceService;
        public AttemptService(IUnitOfWork unitOfWork, IStudentProfileService studentProfileService, IStudentTopicPerformanceService studentTopicPerformanceService)
        {
            _unitOfWork = unitOfWork;
            _studentProfileService = studentProfileService;
            _studentTopicPerformanceService = studentTopicPerformanceService;
        }

        public async Task<AttemptCreatedDto> CreateAttempt(CreateAttemptDto dto)
        {
            var attempt = await _unitOfWork.Attempts.AddAttempt(dto);

            await _unitOfWork.SaveChangesAsync();

            return new AttemptCreatedDto
            {
                AttemptId = attempt.Id,
                QuizId = attempt.QuizId,
                StartedAt = attempt.StartedAt,
                EndsAt = attempt.EndsAt
            };
        }

        public Task<AttemptDetailsDto> GetAttemptById(int attemptId)
        {
            return _unitOfWork.Attempts.GetAttemptById(attemptId);
            
        }


        // see wich one you will use reposity or service 

        public Task<AttemptReviewDto> ReviewMyAttempt(int userId, int attemptId)
        {
            return _unitOfWork.Attempts.ReviewMyAttempt(userId, attemptId);
        }
        public Task<IEnumerable<AttemptListDto>> GetAttemptsByQuiz(int quizId)
        {
            return _unitOfWork.Attempts.GetAttemptsByQuiz(quizId);
        }

        public Task<int> GetAttemptsCount(int userId, int quizId)
        {
            return _unitOfWork.Attempts.GetAttemptsCount(userId, quizId);
        }

        public Task<IEnumerable<UserQuizAttemptsDto>> GetAttemptsCountPerQuiz(int userId)
        {
            return _unitOfWork.Attempts.GetAttemptsCountPerQuiz(userId);
        }
        // fix this soon
        public Task<AttemptWithQuizDto?> GetAttemptWithQuizDetails(int attemptId)
        {
            return _unitOfWork.Attempts.GetAttemptWithQuizDetails(attemptId);
        }

        public Task<AttemptDetailsDto> GetLastAttempt(int userId)
        {
            return _unitOfWork.Attempts.GetLastAttempt(userId);
        }

        public Task<IEnumerable<AttemptListDto>> GetUserAttempts(int userId,CancellationToken ct)
        {
            return _unitOfWork.Attempts.GetUserAttempts(userId,ct);
        }

        public Task<decimal> GetUserOverallScore(int userId)
        {
            return _studentProfileService.GetOverallScore(userId);
        }

        public async Task SubmitAttempt(SubmitAttemptDto dto)
        {
            /*
                ================================================================================
                SUBMIT ATTEMPT FLOW
                ================================================================================
                
                1. Validate Attempt
                   - Ensure attempt exists.
                   - Ensure attempt belongs to the current user.
                   - Ensure attempt has not already been submitted.
                
                2. Load Quiz Data
                   - Load quiz questions with their options.
                   - Build lookup structures (Dictionary / HashSet) for fast access.
                
                3. Validate Submission
                   - Ensure all quiz questions were answered.
                   - Ensure there are no missing or invalid question ids.
                
                4. Process Submitted Answers
                   - Validate question existence.
                   - Validate selected option existence.
                   - Determine whether answer is correct.
                   - Calculate earned points.
                   - Accumulate total score.
                   - Accumulate maximum possible score.
                   - Build topic statistics (correct/wrong per topic).
                   - Create AttemptAnswer entities.
                
                5. Persist Attempt Answers
                   - Add all AttemptAnswer records using AddRange.
                
                6. Calculate Attempt Result
                   - Calculate final score.
                   - Calculate percentage.
                   - Determine pass/fail status.
                   - Set submission timestamp.
                
                7. Update Student Profile
                   - Increment TotalAttempts.
                   - Update LastAssessmentDate.
                   - Update SkillScore using weighted average.
                   - Recalculate CurrentLevel.
                
                8. Update Topic Performances
                   - Load existing StudentTopicPerformance records for affected topics.
                   - Update CorrectAnswers, WrongAnswers and SuccessRate.
                   - Create new StudentTopicPerformance records for unseen topics.
                   - Insert new records using AddRange.
                
                9. Transaction Handling
                   - Save all changes once.
                   - Commit transaction if successful.
                   - Rollback transaction if any error occurs.
                
                10. Future Enhancements
                   - Detect weak topics.
                   - Detect strong topics.
                   - Generate personalized recommendations.
                   - Create adaptive quizzes.
                   - Use bulk operations for large datasets or use Stored Procedure to reduce the requests to 1 request only.
                   - Add caching where appropriate.
                
                Complexity:
                - Question lookup: O(1) via Dictionary.
                - Topic lookup: O(1) via Dictionary.
                - Submission validation: O(n).
                - Answer processing: O(n).
                - Overall complexity: O(n).
                ================================================================================
                */

            // 1. Get attempt with quiz info
            var attempt = await _unitOfWork.Attempts
                .GetAttemptEntityWithQuiz(dto.AttemptId);


            if (attempt == null)
                throw new KeyNotFoundException("Attempt not found");

            if (attempt.UserId != dto.UserId)
                throw new UnauthorizedAccessException();

            if (attempt.SubmittedAt != null)
                throw new InvalidOperationException("Attempt already submitted");



            // 2. Load full quiz questions + options (مرة واحدة فقط)
            var quizQuestions = await _unitOfWork.Questions
                .GetQuestionsWithOptionsByQuizId(attempt.QuizId);

            int totalScore = 0;
            int maxScore = 0;
            var topicStats = new Dictionary<int, (int Correct, int Wrong)>();
            //int correctAnswer = 0;


            var submittedQuestionIds = dto.Answers
                .Select(x => x.QuestionId)
                .Distinct()
                .ToList();

            var quizQuestionSet = quizQuestions
                .Select(x => x.Id)
                .ToHashSet();


            if (submittedQuestionIds.Count != quizQuestionSet.Count ||
                !submittedQuestionIds.All(quizQuestionSet.Contains))
            {
                throw new InvalidOperationException("Invalid submission");
            }
            if (dto.Answers.Count() != submittedQuestionIds.Count)
                throw new InvalidOperationException("Duplicate answers detected");



            var attemptAnswers = new List<AttemptAnswer>();
            var questionMap = quizQuestions
                    .ToDictionary(x => x.Id);
            // 3. Loop on submitted answers
            ProcessSubmittedAnswers(dto, questionMap, topicStats, ref totalScore, ref maxScore, attemptAnswers);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // we can use bulk insertion or stored procedure for reduce all of this operatinos to 1 request , but i will choose later between bulk update and insert agnist stored procedure
                await _unitOfWork.AttemptAnswers.AddRangeAsync(attemptAnswers);


                // 5. Update Attempt
                // those for update thte attempt after submition
                attempt.Score = totalScore;
                attempt.Percentage = maxScore == 0
                    ? 0
                    : (decimal)totalScore / maxScore * 100;

                attempt.Passed = attempt.Percentage >= (attempt.Quiz.PassPercentage ?? 60);
                attempt.SubmittedAt = DateTime.UtcNow;

                //total questions
                //var totalQuestions = quizQuestions.Count();
                //amount correct and wrong answers
                //int wrongAnswers = totalQuestions - correctAnswer;


                /*
                    - Delegate profile update to StudentProfileService.
                    - Increment TotalAttempts.
                    - Update LastAssessmentDate.
                    - Recalculate SkillScore.
                    - Recalculate CurrentLevel.

                 */

                await _studentProfileService.UpdateProfileAfterSubmission(dto.UserId, attempt.Percentage.Value);
                /*                var student = await _unitOfWork.StudentProfiles.GetByUserId(dto.UserId);
                                if (student == null)
                                    throw new KeyNotFoundException("Student profile not found");

                                // those for update the student profile after submition
                                student.TotalAttempts++;
                                student.LastAssessmentDate = DateTime.UtcNow;*/
                /*
                 Update Topic Performances
                 - Delegate topic performance update to StudentTopicPerformanceService.
                 - Update existing topic statistics.
                 - Create new topic performance records when needed.
                 - Recalculate SuccessRate. 

                 */

                await _studentTopicPerformanceService.UpdateAfterAttempt(dto.UserId, topicStats);
                /*                var performances = await _unitOfWork.StudentTopicPerformances.GetByUserIdAndTopicIds(dto.UserId, topicStats.Keys);
                                var performanceMap = performances
                                    .ToDictionary(x => x.TopicId);
                                var newPerformances = new List<StudentTopicPerformance>();

                                UpdateTopicPerformances(dto.UserId, topicStats, performanceMap, newPerformances);

                                await _unitOfWork.StudentTopicPerformances.AddRangeAsync(newPerformances);*/
                /*var studentPerformancesBasedOnTopic = await _unitOfWork.StudentTopicPerformances.GetUserPerformance(dto.UserId);
                foreach(var ansTopic in studentPerformancesBasedOnTopic)
                {

                    if (!topicStats.TryGetValue(ansTopic.TopicId, out var answersOfTopic))
                        continue;

                    ansTopic.CorrectAnswers += answersOfTopic.Correct;
                    ansTopic.WrongAnswers += answersOfTopic.Wrong;
                    var total = ansTopic.CorrectAnswers + ansTopic.WrongAnswers;
                    ansTopic.SuccessRate = total == 0 ?  0: ((decimal)ansTopic.CorrectAnswers / total) * 100;
                }*/


                /*                UpdateSkillScore(student, attempt.Percentage.Value);
                    UpdateStudentLevel(student);*/

                /*            var studentProfileAfterSubmition = new UpdateStudentProfileDto
                    {
                        LastAssessmentDate = DateTime.UtcNow,

                        UserId = dto.UserId,
                    };
                    await _unitOfWork.StudentProfiles.UpdateStudentProfileAfterSubmition(studentProfileAfterSubmition);*/
                /*            var studentProfilePerformance = new UpdateStudentTopicPerformanceDto
                    {
                        CorrectAnswers = correctAnswer,
                        WrongAnswers = wrongAnswers,
                    };
                    await _unitOfWork.StudentTopicPerformances.UpdatePerformance(studentProfilePerformance);*/




                // 6. Commit everything once
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        
        // this will be in internaly the service of student perforamnace based on topic
/*        private void UpdateTopicPerformances(int UserId, Dictionary<int, (int Correct, int Wrong)> topicStats, Dictionary<int, StudentTopicPerformance> performanceMap, List<StudentTopicPerformance> newPerformances)
        {
            foreach (var topic in topicStats)
            {
                if (performanceMap.TryGetValue(topic.Key, out var performance))
                {
                    performance.CorrectAnswers += topic.Value.Correct;
                    performance.WrongAnswers += topic.Value.Wrong;

                    var total =
                        performance.CorrectAnswers +
                        performance.WrongAnswers;

                    performance.SuccessRate =
                        total == 0
                            ? 0
                            : (decimal)performance.CorrectAnswers / total * 100;
                }
                else
                {
                    var total = topic.Value.Correct + topic.Value.Wrong;
                    var newPerformance = new StudentTopicPerformance
                    {
                        TopicId = topic.Key,
                        UserId = UserId,
                        CorrectAnswers = topic.Value.Correct,
                        WrongAnswers = topic.Value.Wrong,
                        SuccessRate = total == 0
        ? 0
                     : (decimal)topic.Value.Correct / total * 100
                    };
                    newPerformances.Add(newPerformance);
                }
            }
        }
*/        private void ProcessSubmittedAnswers(SubmitAttemptDto dto, Dictionary<int, QuestionDetailsDto> questionMap, Dictionary<int, (int Correct, int Wrong)> topicStats, ref int totalScore, ref int maxScore, List<AttemptAnswer> attemptAnswers)
        {
            foreach (var answer in dto.Answers)
            {
                // reduce time complexty  from N power 2 to N
                if (!questionMap.TryGetValue(answer.QuestionId, out var question))
                    throw new InvalidOperationException("Invalid question");

                var selectedOption = question.Options
                    .FirstOrDefault(o => o.Id == answer.SelectedOptionId);

                if (selectedOption == null)

                    throw new InvalidOperationException("Invalid option");



                bool isCorrect = selectedOption.IsCorrect;
                int earnedPoints = isCorrect ? (question.Points ?? 0) : 0;

                //correctAnswer += isCorrect ? 1 : 0;
                /*                    if (!topicStats.ContainsKey(question.TopicId))
                                        topicStats[question.TopicId] = (0, 0);

                                    var current = topicStats[question.TopicId];
                // this equal to the bottom |
                                            v
                 */
                topicStats.TryGetValue(question.TopicId, out var current);

                if (isCorrect)
                    topicStats[question.TopicId] =
                        (current.Correct + 1, current.Wrong);
                else
                    topicStats[question.TopicId] =
                        (current.Correct, current.Wrong + 1);


                totalScore += earnedPoints;
                maxScore += question.Points ?? 0;

                attemptAnswers.Add(new AttemptAnswer
                {
                    AttemptId = dto.AttemptId,
                    QuestionId = question.Id,
                    SelectedOptionId = selectedOption.Id,
                    IsCorrect = isCorrect,
                    EarnedPoints = earnedPoints
                });
            }
        }
        // this will be in student service
        /*        private void UpdateStudentLevel(StudentProfile student)
        {
            if (student.SkillScore >= 80)
                student.CurrentLevel = "Hard";
            else if (student.SkillScore >= 50)
                student.CurrentLevel = "Medium";
            else
                student.CurrentLevel = "Easy";
        }
        // and that too
        private void UpdateSkillScore(StudentProfile student, decimal percentage)
        {
            student.SkillScore =
                student.SkillScore == null
                    ? percentage
                    : student.SkillScore.Value * 0.8m + percentage * 0.2m;
        }
    }*/
    }
}
