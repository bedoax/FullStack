using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.StudentTopicPerformance;
using Models.Entities;
namespace Data.Repositories
{
    public class StudentTopicPerformanceRepository : IStudentTopicPerformanceRepository
    {
        private  AppDbContext _context;
        public StudentTopicPerformanceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddPerformance(CreateStudentTopicPerformanceDto dto)
        {
            var isExisting = await _context.StudentTopicPerformances
                .AnyAsync(p => p.UserId == dto.UserId && p.TopicId == dto.TopicId);
            if (isExisting)
                throw new InvalidOperationException("Performance record already exists for this user and topic.");

            var performance = new StudentTopicPerformance
            {
                UserId = dto.UserId,
                TopicId = dto.TopicId,
                CorrectAnswers = dto.CorrectAnswers,
                WrongAnswers = dto.WrongAnswers
            };
           await _context.StudentTopicPerformances.AddAsync(performance);
        }
        // the same also for DeltePerformance , use it when we will delete user and delete student
        // profile and delete all the performance records for this user, so we will not implement this method for now
        public async Task DeletePerformance(int userId, int topicId)
        {
            throw new NotImplementedException();
        }

        public async  Task<StudentTopicPerformanceDto> GetTopicPerformance(int userId, int topicId)
        {
            var performance = await _context.StudentTopicPerformances
                .Where(p => p.UserId == userId && p.TopicId == topicId)
                .FirstOrDefaultAsync();
            if (performance == null)
                return null;

            return new StudentTopicPerformanceDto
            {
                UserId = performance.UserId,
                TopicId = performance.TopicId,
                TopicName = performance.Topic.Name,
                CorrectAnswers = performance.CorrectAnswers,
                WrongAnswers = performance.WrongAnswers,
                SuccessRate = performance.SuccessRate,
                LastUpdated = performance.LastUpdated
            };
        }

        public async Task<IEnumerable<StudentTopicPerformanceDto>> GetUserPerformance(int userId)
        {
            return await _context.StudentTopicPerformances
                .Where(p => p.UserId == userId)
                .Select(p => new StudentTopicPerformanceDto
                {
                    UserId = p.UserId,
                    TopicId = p.TopicId,
                    TopicName = p.Topic.Name,
                    CorrectAnswers = p.CorrectAnswers,
                    WrongAnswers = p.WrongAnswers,
                    SuccessRate = p.SuccessRate,
                    LastUpdated = p.LastUpdated
                }).ToListAsync();
        }

        public  async Task UpdatePerformance(UpdateStudentTopicPerformanceDto performance)
        {
            // make summation  on service layer of old correct and new correct answers and same for wrong answers and then update the success rate and last updated date
            var rows =  await _context.StudentTopicPerformances
                .Where(p => p.UserId == performance.UserId && p.TopicId == performance.TopicId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.CorrectAnswers, performance.CorrectAnswers)
                    .SetProperty(p => p.WrongAnswers,  performance.WrongAnswers)
                    .SetProperty(p => p.SuccessRate, performance.SuccessRate)
                    .SetProperty(p => p.LastUpdated, performance.LastUpdated));
            if(rows == 0)
                throw new KeyNotFoundException("Performance record not found for this user and topic.");
        }
        public Task<List<StudentTopicPerformance>> GetByUserIdAndTopicIds( int userId, IEnumerable<int> topicIds)
        {
            return _context.StudentTopicPerformances
                .Where(x =>
                    x.UserId == userId &&
                    topicIds.Contains(x.TopicId))
                .ToListAsync();
        }
        public async Task AddRangeAsync(IEnumerable<StudentTopicPerformance> studentTopicPerformances)
        {
            await _context.StudentTopicPerformances.AddRangeAsync(studentTopicPerformances);
        }
        public async Task<IEnumerable<WeakTopicDto>> GetUserPerformanceWithTopicsName(int userId)
        {
            return  _context.StudentTopicPerformances
                          .Where(p => p.UserId == userId)
                          .Select(p => new WeakTopicDto
                          {
                               TopicName = p.Topic.Name,
                              TopicId = p.TopicId,
                              TotalQuestionsSolved = (int)(p.CorrectAnswers + p.WrongAnswers),
                              SuccessRate = (decimal) p.SuccessRate,
                          });
        }
        public async Task<List<LeaderboardDto>> GetLeaderboardByTopicId(int topicId)
        {
            var leaderboard = await _context.StudentTopicPerformances
                .Where(x => x.TopicId == topicId)
                .OrderByDescending(x => x.SuccessRate)
                .Select(x => new LeaderboardDto
                {
                    UserId = x.UserId,
                    Username = x.User.Username,
                    Score = x.SuccessRate ?? 0
                })
                .ToListAsync();

            return leaderboard;
        }
/*        private void RankingTheLeaderboard(List<LeaderboardDto>leaderboard)
        {
            int rank = 1;
            for (int i = 0; i < leaderboard.Count; i++)
            {
                if (i > 0 &&
                    leaderboard[i].Score < leaderboard[i - 1].Score)
                {
                    rank = i + 1;
                }

                leaderboard[i].Rank = rank;

            }
        }*/
    }
}


