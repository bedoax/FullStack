using Business.Helper;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Topic;
using Models.Entities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly  ITopicService _ITopicService;
        private IQuestionService _questionService;
        private IStudentTopicPerformanceService _studentTopicPerformanceService;
        public TopicsController(ITopicService topicService, IQuestionService questionService, IStudentTopicPerformanceService studentTopicPerformanceService)
        {
            _ITopicService = topicService;
            _questionService = questionService;
            _studentTopicPerformanceService = studentTopicPerformanceService;
        }
        /*
         POST /topics
PUT /topics/{id}
DELETE /topics/{id}
GET /topics
GET /topics/{id}
         
         */
        //[Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{topicId}/questions")]
        public async Task<IActionResult> GetQuestionsOfTopic(int topicId)
        {
            var questions = await _questionService.GetQuestionsByTopic(topicId);
            return Ok(questions);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateDto topicDto)
        {
             await _ITopicService.CreateTopic(topicDto);
            return Created();
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _ITopicService.GetAllTopics();
            return Ok(topics);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopic(int id)
        {
            var topic = await _ITopicService.GetTopicById(id);
            if (topic == null)
                return NotFound();
            return Ok(topic);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] UpdateTopicRequest topicDto)
        {
            var topic = new TopicUpdateDto
            {
                Description = topicDto.Description,
                Id = id,
                Name = topicDto.Name
            };
            await _ITopicService.UpdateTopic(topic);
            return NoContent();
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            await _ITopicService.DeleteTopic(id);
            return NoContent();
        }
        [Authorize(Roles =RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("{topicId:int}/leaderboard")]
        public async Task<IActionResult> leaderboardByTopicId(int topicId)
        {
            // add pagination later
            var result = await _studentTopicPerformanceService.GetLeaderboardByTopicId(topicId);
            return Ok(result);
        }
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> TopicStaticsByTopicId(int id)
        {
            var result = await _ITopicService.TopicStatisticByTopicId(id);
            return Ok(result);
        }
    }
}
