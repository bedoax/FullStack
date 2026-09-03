using Business.Helper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.StudentTopicPerformance;

namespace Api.Controllers
{
    [Route("api/performance")]
    [ApiController]
    public class StudentTopicPerformancesController : ControllerBase
    {
        private IStudentTopicPerformanceService _studentTopicPerformanceService;
        public StudentTopicPerformancesController(IStudentTopicPerformanceService studentTopicPerformanceService)
        {
            _studentTopicPerformanceService = studentTopicPerformanceService;
        }
        /*
         GET /performance/user/{userId}
GET /performance/user/{userId}/topic/{topicId}

         */
        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyPerformance()
        {
            var userId = User.GetUserId();
            var performance = await _studentTopicPerformanceService.GetPerformanceByUser(userId);
            return Ok(performance);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me/topic/{topicId}")]
        public async Task<IActionResult> GetMyPerformanceByTopic(int topicId)
        {
            var userId = User.GetUserId();
            var performance = await _studentTopicPerformanceService.GetPerformanceByTopic(userId, topicId);
            return Ok(performance);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me/weak-topics")]
        public async Task<IActionResult> WeakTopicsOfStudent()
        {
            int userId = User.GetUserId();
            var weakTopics = await _studentTopicPerformanceService.WeakTopicsOfStudent(userId);
            return Ok(weakTopics);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPerformanceByUser(int userId)
        {
            var performance = await _studentTopicPerformanceService.GetPerformanceByUser(userId);
            return Ok(performance);
        }


        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}/topic/{topicId}")]
        public async Task<IActionResult> GetPerformanceByUserAndTopic(int userId, int topicId)
        {
            var performance = await _studentTopicPerformanceService.GetPerformanceByTopic(userId, topicId);
            if(performance == null)
                return NotFound("Performance record not found for this user and topic.");
            return Ok(performance);
        }
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{userId}/weak-topics")]
        public async Task<IActionResult> WeakTopicsOfStudent(int userId)
        {
            var weakTopics = await _studentTopicPerformanceService.WeakTopicsOfStudent(userId);
            return Ok(weakTopics);
        }

    }
}
