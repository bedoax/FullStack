using Business.Helper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Attempt;
using Models.DTOs.AttemptAnswer;
using Models.Entities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptsController : ControllerBase
    {
        /*
         POST /attempts/create
        POST /attempts/submit

        GET /attempts/{id}
            GET /attempts/user/{userId}
        GET /attempts/user/{userId}/overall-score
         */
        private IAttemptService _attemptService;

        public AttemptsController(IAttemptService attemptService)
        {
            _attemptService = attemptService;
        
        }
        // TEMP ENDPOINTS FOR TESTING BEFORE JWT AUTHORIZATION
        [HttpPost("submit-from-user")]
        public async Task<IActionResult> Submit(int userId, int attemptId, [FromBody] IEnumerable<AnswerDto> submitAttemptDto)
        {
            var submitAttempt = new SubmitAttemptDto
            {
                AttemptId = attemptId,
                UserId = userId,
                Answers = submitAttemptDto
            };
            await _attemptService.SubmitAttempt(submitAttempt);
            return NoContent();    
        }

        [HttpPost("create-from-user")]
        public async Task<IActionResult> CreateAttempt(int userId,int quizId)
        {
           
            if (quizId <= 0)
            {
                return BadRequest("Invalid quiz ID");
            }
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            var attemptCreateDto = new CreateAttemptDto
            {
                UserId = userId,
                QuizId = quizId
            };
            await _attemptService.CreateAttempt(attemptCreateDto);
            return Created();
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttemptById([FromRoute] int id)
        {
            var attempt = await _attemptService.GetAttemptWithQuizDetails(id);
            
            if(attempt == null)
                return NotFound();

            return Ok(attempt);
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAttemptsByUserId([FromRoute] int userId,CancellationToken ct)
        {
            var attempts = await _attemptService.GetUserAttempts(userId,ct);

            if (attempts == null || !attempts.Any())
                return NotFound();

            return Ok(attempts);
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}/overall-score")]
        public async Task<IActionResult> GetUserOverallScore([FromRoute] int userId)
        {
            var userScore = await _attemptService.GetUserOverallScore(userId);

            return Ok(userScore);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("user/{userId}/{attemptId}/review")]
        public async Task<IActionResult> ReviewMyAttemptById(int userId,int attemptId)
        {
            var result = await _attemptService.ReviewMyAttempt(attemptId, userId);
            return Ok(result);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpPost("{attemptId:int}/submit")]
        public async Task<IActionResult> SubmitAttempt( int attemptId, [FromBody] IEnumerable<AnswerDto> submitAttemptDto)
        {

            var userId = User.GetUserId();
            var submitAttempt = new SubmitAttemptDto
            {
                AttemptId = attemptId,
                UserId = userId,
                Answers = submitAttemptDto
            };
            await _attemptService.SubmitAttempt(submitAttempt);
            return Created();
        }
        
        [Authorize(Roles = RolesConstants.Student)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAttempt(int quizId)
        {
            var userId = User.GetUserId();
            if (quizId <= 0)
            {
                return BadRequest("Invalid quiz ID");
            }
            var attemptCreateDto = new Models.DTOs.Attempt.CreateAttemptDto
            {
                UserId = userId,
                QuizId = quizId
            };
           var result  = await _attemptService.CreateAttempt(attemptCreateDto);
            return Ok(result);
        }
        
        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("user/me")]
        public async Task<IActionResult> GetAttemptsByUserId(CancellationToken ct)
        {
            var userId = User.GetUserId();
            var attempts = await _attemptService.GetUserAttempts(userId,ct);
            if(attempts == null)
                return NotFound();
            return Ok(attempts);
        }
        
        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("user/me/{attemptId}/review")]
        public async Task<IActionResult> ReviewMyAttemptById(int attemptId)
        {
            var userId = User.GetUserId();
            var result = await _attemptService.ReviewMyAttempt(userId, attemptId);
            return Ok(result);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("user/me/overall-score")]
        public async Task<IActionResult> GetUserOverallScore()
        {
            var userId = User.GetUserId();
            var userScore = await _attemptService.GetUserOverallScore(userId);
            return Ok(userScore);
        }
    }
}
