using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Topic;
using Models.Entities;
namespace Data.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly AppDbContext _context;
        public TopicRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTopic(TopicCreateDto topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            if (string.IsNullOrWhiteSpace(topic.Name))
                throw new ArgumentException("Topic name is required.");

            string normalizedName = topic.Name.Trim();

            bool exists = await _context.Topics
                .AsNoTracking()
                .AnyAsync(t => t.Name.ToLower() == normalizedName.ToLower());

            if (exists)
                throw new InvalidOperationException("Topic already exists.");

            var newTopic = new Topic
            {
                Name = normalizedName,
                Description = topic.Description?.Trim()
            };

            await _context.Topics.AddAsync(newTopic);
          
        }

        public async Task DeleteTopic(int topicId)
        {
            //change the remove method to soft delete by adding a IsDeleted property to the Topic entity and setting it to true instead of removing the topic from the database
            if (topicId <= 0)
                throw new ArgumentOutOfRangeException(nameof(topicId));

            var topic = await _context.Topics.FindAsync(topicId);

            if (topic == null)
                throw new KeyNotFoundException("Topic not found.");

            _context.Topics.Remove(topic);

           
        }

        public async Task<IEnumerable<TopicDto>> GetAllTopics()
        {
            // add cursor pagination to this method
            return await _context.Topics
                .Select(t => new TopicDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description
                })
                .ToListAsync();
        }

        public async Task<TopicDto?> GetTopicById(int topicId)
        {
            if (topicId <= 0)
                throw new ArgumentOutOfRangeException(nameof(topicId));

            var topic =  await _context.Topics
                .Select(t => new TopicDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description
                })
                .FirstOrDefaultAsync(t => t.Id == topicId);
            if (topic == null)
                throw new KeyNotFoundException("The Id Key not found");
            return topic;
        }

        public async Task UpdateTopic(TopicUpdateDto topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));


            if (string.IsNullOrWhiteSpace(topic.Name))
                throw new ArgumentException("Topic name is required.");

            var existingTopic = await _context.Topics
                .FirstOrDefaultAsync(t => t.Id == topic.Id);

            if (existingTopic == null)
                throw new KeyNotFoundException("Topic not found.");

            string normalizedName = topic.Name.Trim();

            bool duplicateExists = await _context.Topics
                .AsNoTracking()
                .AnyAsync(t =>
                    t.Id != existingTopic.Id &&
                    t.Name.ToLower() == normalizedName.ToLower());

            if (duplicateExists)
                throw new InvalidOperationException("Topic name already exists.");

            existingTopic.Name = normalizedName;
            existingTopic.Description = topic.Description?.Trim();

           
        }
        public async Task<TopicStatisticsDto> TopicStatisticByTopicId(int topicId)
        {
            var result =  _context.StudentTopicPerformances.Where(x => x.TopicId == topicId)
                                                          .GroupBy(x => x.TopicId)
                                                          .Select(g => new TopicStatisticsDto
                                                          {
                                                              Students = g.Count(),
                                                              AverageSuccessRate = g.Average(x=>x.SuccessRate ?? 0)
                                                          }).FirstOrDefault();
            if(result == null)
            {
                result = new TopicStatisticsDto
                {
                    AverageSuccessRate = 0,
                    Students = 0
                };
            }
            return result;
        }
    }
}


