using Business.Interfaces;
using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.StudentTopicPerformance;
using Models.Entities;


namespace Business.Services
{
    public class StudentTopicPerformanceService : IStudentTopicPerformanceService
    {
        private IUnitOfWork _unitOfWork;
        private const int MinQuestionsForEvaluation = 5;
        private const decimal WeakTopicThreshold = 60m;
        public StudentTopicPerformanceService(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }

/*        public async Task AddPerformance(CreateStudentTopicPerformanceDto performance)
        {
            await _unitOfWork.StudentTopicPerformances.AddPerformance(performance);
            await _unitOfWork.SaveChangesAsync();
        }*/

        public async Task DeletePerformance(int userId, int topicId)
        {
            // note delete student performannce repository not implemented
            await _unitOfWork.StudentTopicPerformances.DeletePerformance(userId, topicId);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<StudentTopicPerformanceDto> GetPerformanceByTopic(int userId, int topicId)
        {
            return _unitOfWork.StudentTopicPerformances.GetTopicPerformance(userId, topicId);
        }

        public Task<IEnumerable<StudentTopicPerformanceDto>> GetPerformanceByUser(int userId)
        {
           return  _unitOfWork.StudentTopicPerformances.GetUserPerformance(userId);
        }




        /*        public async Task UpdatePerformance(UpdateStudentTopicPerformanceDto performance)
                {
                    // here we will summation the old mistaks with new and new correct answers + old 
                    await _unitOfWork.StudentTopicPerformances.UpdatePerformance(performance);
                }*/

        public async Task UpdateAfterAttempt(int userId,Dictionary<int, (int Correct, int Wrong)> topicStats)
        {
            if (topicStats.Count == 0)
                return;

            var performances = await _unitOfWork.StudentTopicPerformances.GetByUserIdAndTopicIds(userId,topicStats.Keys);

            var performanceMap = performances.ToDictionary(x => x.TopicId);

            var newPerformances =new List<StudentTopicPerformance>();

            ApplyTopicPerformanceUpdates(userId, topicStats, performanceMap, newPerformances);

            if (newPerformances.Count > 0)
            {
                await _unitOfWork.StudentTopicPerformances.AddRangeAsync(newPerformances);
            }
        }
        private void ApplyTopicPerformanceUpdates(int  userId, Dictionary<int, (int Correct, int Wrong)> topicStats, Dictionary<int, StudentTopicPerformance> performanceMap, List<StudentTopicPerformance> newPerformances)
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
                    performance.LastUpdated = DateTime.UtcNow;
                }
                else
                {
                    var total = topic.Value.Correct + topic.Value.Wrong;
                    var newPerformance = new StudentTopicPerformance
                    {
                        TopicId = topic.Key,
                        UserId = userId,
                        CorrectAnswers = topic.Value.Correct,
                        WrongAnswers = topic.Value.Wrong,
                        SuccessRate = total == 0 ? 0: (decimal)topic.Value.Correct / total * 100,
                        LastUpdated = DateTime.UtcNow
                    };
                    newPerformances.Add(newPerformance);
                }
            }
        }
        public async Task<IEnumerable<WeakTopicDto>> WeakTopicsOfStudent(int userId)
        {
            var topicsPerformance = await _unitOfWork.StudentTopicPerformances.GetUserPerformanceWithTopicsName(userId);
            return topicsPerformance.Where(x=>x.TotalQuestionsSolved >= MinQuestionsForEvaluation && x.SuccessRate < WeakTopicThreshold)
                                              .OrderBy(x=>x.SuccessRate).ToList();
             
        }
        public  Task<List<LeaderboardDto>> GetLeaderboardByTopicId(int topicId)
        {
            return  _unitOfWork.StudentTopicPerformances.GetLeaderboardByTopicId(topicId);
        }
    }
}
