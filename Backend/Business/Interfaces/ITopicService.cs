using Models.DTOs.Topic;


namespace Business.Interfaces
{
    public interface ITopicService
    {
        /*
         Create Topic
         Update Topic
         Delete Topic
         Get Topic By Id
         Get All Topics
         */
        Task CreateTopic(TopicCreateDto topicCreateDto);
        Task DeleteTopic(int topicId);
        Task UpdateTopic(TopicUpdateDto topicUpdateDto);
        Task<TopicDto> GetTopicById(int topicId);
        Task<IEnumerable<TopicDto>> GetAllTopics();
        Task<TopicStatisticsDto> TopicStatisticByTopicId(int topicId);
    }


}
