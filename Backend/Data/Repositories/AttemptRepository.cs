using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Attempt;
using Models.DTOs.AttemptAnswer;
using Models.DTOs.Option;
using Models.DTOs.Question;
using Models.DTOs.Quiz;
using Models.Entities;
namespace Data.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        private  AppDbContext _context;
        
        public AttemptRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Attempt> AddAttempt(CreateAttemptDto dto)
        {
            if (dto.UserId <= 0 || dto.QuizId <= 0)
                throw new ArgumentOutOfRangeException();

            bool userExists =
                await _context.Users.AnyAsync(u => u.Id == dto.UserId);

            if (!userExists)
                throw new KeyNotFoundException("User not found.");

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == dto.QuizId);

            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found.");

            if (!quiz.IsPublished)
                throw new InvalidOperationException("Quiz is not published.");
            if (quiz.AvailableFrom > DateTime.UtcNow)
                throw new InvalidOperationException("Quiz is not available yet.");

            if (quiz.AvailableTo < DateTime.UtcNow)
                throw new InvalidOperationException("Quiz is closed.");

            if (quiz.DurationMinutes <= 0)
                throw new InvalidOperationException("Quiz duration is invalid.");

            int lastAttemptNumber =
                await _context.Attempts
                    .Where(a =>
                        a.UserId == dto.UserId &&
                        a.QuizId == dto.QuizId)
                    .MaxAsync(a => (int?)a.AttemptNumber) ?? 0;

            if (lastAttemptNumber >= quiz.MaxAttempts)
                throw new InvalidOperationException(
                    "Maximum attempts reached.");


            var attemptStart = DateTime.UtcNow;
            var attempt = new Attempt
            {
                UserId = dto.UserId,
                QuizId = dto.QuizId,
                AttemptNumber = lastAttemptNumber + 1,
                StartedAt = attemptStart,
                EndsAt = attemptStart.AddMinutes(quiz.DurationMinutes),
                IsAutoSubmitted = false
                
            };

            await _context.Attempts.AddAsync(attempt);

            return attempt;

        }


        public async Task<List<QuizStudentDto>> GetStudentsOfQuiz(int teacherId, int quizId)
        {
            return await _context.Attempts
                .Where(a =>
                    a.QuizId == quizId &&
                    a.Quiz.CreatedByTeacherId == teacherId)
                .Select(a => new QuizStudentDto
                {
                    UserId = a.UserId,
                    Username = a.User.Username,
                    Score = a.Score,
                    Passed = a.Passed
                })
                .ToListAsync();
        }
        public async Task<List<TeacherAttemptStudentDto>> GetMyAttemptsByQuizId(int teacherId,int quizId)
        {
            return await _context.Attempts
                .Where(a =>
                    a.QuizId == quizId &&
                    a.Quiz.CreatedByTeacherId == teacherId)
                .Select(a=>new TeacherAttemptStudentDto
                {
                    AttemptNumber = a.AttemptNumber,
                    EndsAt = a.EndsAt,
                    IsAutoSubmitted = a.IsAutoSubmitted,
                    Score = a.Score,
                    StartedAt = a.StartedAt,
                    SubmittedAt = a.SubmittedAt,
                    Id = a.Id,
                    Passed = a.Passed,
                    Percentage = a.Percentage,
                    QuizId = a.QuizId,
                    UserId = a.UserId,
                    Username = a.User.Username,
                })
                .ToListAsync();
        }

        
        public async Task<AttemptReviewDto> ReviewMyAttempt(int userId,int attemptId)
        {
            // three way to solve this , first by the projection with joins all of them in one request 
            var result = await _context.Attempts
                .AsNoTracking()
                .Where(a =>
                    a.Id == attemptId &&
                    a.UserId == userId)
                .Select(a => new AttemptReviewDto
                {
                    
                    AttemptId = a.Id,

                    Score = a.Score ?? 0,

                    Percentage = a.Percentage ?? 0,

                    Questions = a.AttemptAnswers
                  
                        .Select(answer => new QuestionReviewDto
                        {
                            QuestionId = answer.QuestionId,

                            Content = answer.Question.Content,

                            IsCorrect = answer.IsCorrect ?? false,

                            Point = answer.EarnedPoints ?? 0,
                            SelectedAnswer = answer.Question.Options
                                          .Where(o => o.Id == answer.SelectedOptionId)
                                          .Select(o => o.Content)
                                          .FirstOrDefault()
,

                            CorrectAnswer =
                                      answer.Question.Options
                                          .Where(o => o.IsCorrect)
                                          .Select(o => o.Content)
                                          .FirstOrDefault(),

                            Options = answer.Question.Options
                                .Select(option => new OptionReviewDto
                                {
                                    OptionId = option.Id,

                                    Content = option.Content,

                                    IsCorrect = option.IsCorrect,

                                    IsSelectedByStudent =
                                        option.Id ==
                                        answer.SelectedOptionId
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();


            //good when object graph becomes very large
            //or when a single query causes huge joins/cartesian explosion
            /*            var attempt = await _context.Attempts
                      .AsNoTracking()
                      .Where(a =>
                          a.Id == attemptId &&
                          a.UserId == userId)
                      .Select(a => new
                      {
                          a.Id,
                          Score = a.Score ?? 0,
                          Percentage = a.Percentage ?? 0
                      })
                      .FirstOrDefaultAsync();

            var answers = await _context.AttemptAnswers
                             .AsNoTracking()
                             .Where(a => a.AttemptId == attemptId)
                             .Select(a => new
                             {
                                 a.QuestionId,
                                 a.SelectedOptionId,
                                 IsCorrect = a.IsCorrect ?? false,
                                 Point = a.EarnedPoints ?? 0,

                                 QuestionContent = a.Question.Content
                             })
                             .ToListAsync();


            var questionIds = answers
                             .Select(x => x.QuestionId)
                             .Distinct()
                             .ToList();

                                     var options = await _context.Options
                                         .AsNoTracking()
                                         .Where(o => questionIds.Contains(o.QuestionId))
                                         .Select(o => new
                                         {
                                             o.Id,
                                             o.QuestionId,
                                             o.Content,
                                             o.IsCorrect
                                         })
                                         .ToListAsync();

            var result = new AttemptReviewDto
            {
                             AttemptId = attempt.Id,
                              Score = attempt.Score,
                              Percentage = attempt.Percentage,

                              Questions = answers
                      .Select(answer => new QuestionReviewDto
                      {
                          QuestionId = answer.QuestionId,

                          Content = answer.QuestionContent,

                          IsCorrect = answer.IsCorrect,

                          Point = answer.Point,

                          Options = options
                              .Where(o =>
                                  o.QuestionId ==
                                  answer.QuestionId)
                              .Select(o => new OptionReviewDto
                              {
                                  OptionId = o.Id,

                                  Content = o.Content,

                                  IsCorrect = o.IsCorrect,

                                  IsSelectedByStudent =
                                      o.Id ==
                                      answer.SelectedOptionId
                              })
                              .ToList()
                      })
                      .ToList()
            };*/


            // For very large datasets, consider:
            // - Stored Procedures: maximum SQL performance, higher maintenance cost.
            // - CTEs: useful for complex reporting and ranking queries.
            // - Materialized Views / Precomputed Tables: fastest reads for analytics,
            //   but require data synchronization.
            // For the current solution, projection is the most maintainable solution for now to me.
            if (result == null)
            throw new KeyNotFoundException(
                    "Attempt not found.");

            return result;


        }
        public  async Task<AttemptDetailsDto?> GetAttemptById(int attemptId)
        {
            return await _context.Attempts
                     .AsNoTracking()
                     .Where(a => a.Id == attemptId)
                     .Select(a => new AttemptDetailsDto
                     {
                         Id = a.Id,
                         UserId = a.UserId,
                         QuizId = a.QuizId,
                         AttemptNumber = a.AttemptNumber,
                         Score = a.Score,
                         Percentage = a.Percentage,
                         StartedAt = a.StartedAt,
                         SubmittedAt = a.SubmittedAt,
                         Passed = a.Passed
                     })
                     .FirstOrDefaultAsync();
        }

        public async Task<int> GetAttemptsCount(int userId, int quizId)
        {
            return await _context.Attempts
                          .AsNoTracking()
                          .CountAsync(a => a.UserId == userId && a.QuizId == quizId);
        }

        public async Task<AttemptDetailsDto?> GetLastAttempt(int userId)
        {
            return await _context.Attempts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.StartedAt)
                .Select(a => new AttemptDetailsDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    QuizId = a.QuizId,
                    AttemptNumber = a.AttemptNumber,
                    Score = a.Score,
                    Percentage = a.Percentage,
                    StartedAt = a.StartedAt,
                    SubmittedAt = a.SubmittedAt,
                    Passed = a.Passed
                }).FirstOrDefaultAsync();
        }

        public  async Task<IEnumerable<AttemptListDto>> GetUserAttempts(int userId,CancellationToken ct)
        {
            return await _context.Attempts
                      .AsNoTracking()
                      .Where(a => a.UserId == userId)
                      .OrderByDescending(a => a.StartedAt)
                      .Select(a => new AttemptListDto
                      {
                          Id = a.Id,
                          QuizId = a.QuizId,
                          QuizTitle = a.Quiz.Title,
                          AttemptNumber = a.AttemptNumber,
                          Score = a.Score,
                          Percentage = a.Percentage,
                          Passed = a.Passed,
                          StartedAt = a.StartedAt,
                          SubmittedAt = a.SubmittedAt
                          
                      })
                      .ToListAsync(ct);
        }

        public async Task<IEnumerable<AttemptListDto>> GetAttemptsByQuiz(int quizId)
        {
            return await _context.Attempts
                .AsNoTracking()
                .Where(a => a.QuizId == quizId)
                .OrderByDescending(a => a.StartedAt)
                .Select(a => new AttemptListDto
                {
                    Id = a.Id,
                    QuizId = a.QuizId,
                    QuizTitle = a.Quiz.Title,
                    AttemptNumber = a.AttemptNumber,
                    Score = a.Score,
                    Percentage = a.Percentage,
                    Passed = a.Passed,
                    StartedAt = a.StartedAt,
                    SubmittedAt = a.SubmittedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<UserQuizAttemptsDto>>GetAttemptsCountPerQuiz(int userId)
        {
            return await _context.Attempts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .GroupBy(a => a.QuizId)
                .Select(g => new UserQuizAttemptsDto
                {
                    QuizId = g.Key,
                    AttemptsCount = g.Count()
                })
                .ToListAsync();
        }
        public async Task<AttemptWithQuizDto?> GetAttemptWithQuizDetails(int AttemptId)
        {
            return await _context.Attempts
                .Where(a => a.Id == AttemptId)
                .Include(a=>a.Quiz)
                .AsNoTracking()
                .Select(a => new AttemptWithQuizDto
                {
                  AttemptId = a.Id,
                  QuizId = a.QuizId,
                  QuizTitle = a.Quiz.Title,
                  PassPercentage = a.Quiz.PassPercentage,
                  MaxAttempts = a.Quiz.MaxAttempts,

                })
                .FirstOrDefaultAsync();
        }
        public Task<Attempt?> GetAttemptEntityWithQuiz(int attemptId)
        {
            return _context.Attempts
                .Include(x => x.Quiz)
                .FirstOrDefaultAsync(x => x.Id == attemptId);
        }

/*        public async Task SubmitAttempt(SubmitAttemptDto dto)
        {
            *//*            var attempt = await _context.Attempts
                            .Include(a => a.AttemptAnswers)
                            .ThenInclude(a => a.Question)
                            .FirstOrDefaultAsync(a => a.Id == attemptId && a.UserId == userId);

                        if (attempt == null)
                            throw new KeyNotFoundException("Attempt not found or unauthorized.");

                        var totalScore = attempt.AttemptAnswers.Sum(a => a.EarnedPoints);
                        var maxScore = attempt.AttemptAnswers.Sum(a => a.Question.Points ?? 0);

                        var percentage = maxScore == 0 ? 0 : (decimal)totalScore / maxScore * 100;

                        attempt.Score = totalScore;
                        attempt.Percentage = percentage;
                        attempt.Passed = percentage >= 60;
                        attempt.SubmittedAt = DateTime.UtcNow;*//*

        }*/
    }
}


