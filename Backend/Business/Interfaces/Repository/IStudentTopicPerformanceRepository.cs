using Microsoft.EntityFrameworkCore;
using Models.DTOs.StudentTopicPerformance;
using Models.Entities;

namespace Business.Interfaces.Repository
{
    public interface IStudentTopicPerformanceRepository
    {
        Task<IEnumerable<StudentTopicPerformanceDto>>
            GetUserPerformance(
                int userId);

        Task<StudentTopicPerformanceDto>
            GetTopicPerformance(
                int userId,
                int topicId);

        Task AddPerformance(
            CreateStudentTopicPerformanceDto performance);

        Task UpdatePerformance(
            UpdateStudentTopicPerformanceDto performance);

        Task DeletePerformance(
            int userId,
            int topicId);
        Task<List<StudentTopicPerformance>> GetByUserIdAndTopicIds(int userId,IEnumerable<int> topicIds);
        Task AddRangeAsync(IEnumerable<StudentTopicPerformance> studentTopicPerformances);
        Task<IEnumerable<WeakTopicDto>> GetUserPerformanceWithTopicsName(int userId);
        Task<List<LeaderboardDto>> GetLeaderboardByTopicId(int topicId);
    }
}
