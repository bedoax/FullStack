using Models.DTOs.Topic;

namespace Business.Interfaces.Repository
{
    public interface ITopicRepository
    {
        Task<IEnumerable<TopicDto>> GetAllTopics();
        Task<TopicStatisticsDto> TopicStatisticByTopicId(int topicId);
        Task<TopicDto> GetTopicById(
            int topicId);

        Task AddTopic(
            TopicCreateDto topic);

        Task UpdateTopic(
            TopicUpdateDto topic);

        Task DeleteTopic(
            int topicId);
    }
}
