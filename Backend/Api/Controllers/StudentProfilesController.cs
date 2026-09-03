using Business.Helper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.StudentProfile;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentProfilesController : ControllerBase
    {
        /*
         GET /student-profile/{userId}
GET /student-profile/{userId}/skill-score
GET /student-profile/{userId}/current-level
GET /student-profile/{userId}/attempts
         
         */
        private IStudentProfileService _studentProfileService;
        public StudentProfilesController(IStudentProfileService studentProfileService)
        {
            _studentProfileService = studentProfileService;
        }
        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.GetUserId();
            var profile = await _studentProfileService.GetProfile(userId);
            var userProfileDto = new StudentProfileDto
            {
                CurrentLevel = profile?.CurrentLevel,
                SkillScore = profile?.SkillScore,
                LastAssessmentDate = profile?.LastAssessmentDate,
                TotalAttempts = profile?.TotalAttempts,
                UserId = profile.UserId
            };
            return Ok(userProfileDto);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me/skill-score")]
        public async Task<IActionResult> GetUserSkillScore()
        {
            var userId = User.GetUserId();
            var Skillscore = await _studentProfileService.GetSkillScore(userId);
            return Ok(Skillscore);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me/current-level")]
        public async Task<IActionResult> GetUserCurrentLevel()
        {
            var userId = User.GetUserId();
            var CurrentLevel = await _studentProfileService.GetCurrentLevel(userId);
            return Ok(CurrentLevel);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me/attempts")]
        public async Task<IActionResult> GetUserAttempts()
        {
            var userId = User.GetUserId();
            var TotalAttempts = await _studentProfileService.GetTotalAttempts(userId);
            return Ok(TotalAttempts);
        }



        // for testing
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var profile = await _studentProfileService.GetProfile(userId);
            var userProfileDto = new StudentProfileDto
            {
                CurrentLevel = profile?.CurrentLevel,
                SkillScore = profile?.SkillScore,
                LastAssessmentDate = profile?.LastAssessmentDate,
                TotalAttempts = profile?.TotalAttempts,
                UserId = profile.UserId
            };
            return Ok(userProfileDto);
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}/skill-score")]
        public async Task<IActionResult> GetUserSkillScore(int userId)
        {
            var Skillscore = await _studentProfileService.GetSkillScore(userId);
            return Ok(Skillscore);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}/current-level")]
        public async Task<IActionResult> GetUserCurrentLevel(int userId)
        {
            var CurrentLevel = await _studentProfileService.GetCurrentLevel(userId);
            return Ok(CurrentLevel);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}/attempts")]
        public async Task<IActionResult> GetUserAttempts(int userId)
        {
            var TotalAttempts = await _studentProfileService.GetTotalAttempts(userId);
            return Ok(TotalAttempts);
        }

        /*
         * GET /api/studentprofiles/me/dashboard
         * the output expected for get student dashborad 
         {
             "skillScore": 85,
             "currentLevel": "Intermediate",
             "attempts": 22,
             "passed": 17,
             "weakTopics": [...]
         }
         
         */
        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me/dashboard")]
        public async Task<IActionResult> Dashboard(CancellationToken ct)
        {
            var userId = User.GetUserId();
            var dashboard = await _studentProfileService.Dashboard(userId,ct);
            return Ok(dashboard);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{userId}/dashboard")]
        public async Task<IActionResult> Dashboard(int userId,CancellationToken ct)
        {
            var dashboard = await _studentProfileService.Dashboard(userId,ct);
            return Ok(dashboard);
        }
    }
}
