using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.Topic;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public  class TopicsService : ITopicService
    {
        private IUnitOfWork _unitOfWork;
        
        public TopicsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTopic(TopicCreateDto topicCreateDto)
        {
             await _unitOfWork.Topics.AddTopic(topicCreateDto);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTopic(int topicId)
        {
            await _unitOfWork.Topics.DeleteTopic(topicId);
            await _unitOfWork.SaveChangesAsync();
        }

        public  Task<IEnumerable<TopicDto>> GetAllTopics()
        {
            return  _unitOfWork.Topics.GetAllTopics();
        }

        public Task<TopicDto> GetTopicById(int topicId)
        {
            return  _unitOfWork.Topics.GetTopicById(topicId);
        }

        public async Task UpdateTopic(TopicUpdateDto topicUpdateDto)
        {
            await _unitOfWork.Topics.UpdateTopic(topicUpdateDto);
            await _unitOfWork.SaveChangesAsync();
        }
        public  Task<TopicStatisticsDto> TopicStatisticByTopicId(int topicId)
        {
            return _unitOfWork.Topics.TopicStatisticByTopicId(topicId);
        }
    }
}
