using Business.Helper;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Quiz;
using Models.DTOs.QuizQuestionDto;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        /*
         POST /quizzes
PUT /quizzes/{id}
DELETE /quizzes/{id}
GET /quizzes
GET /quizzes/{id}
         
         */
        private IQuizService _quizService;
        private IQuestionService _questionService;
        public QuizzesController(IQuizService quizService, IQuestionService questionService)
        {
            _quizService = quizService;
            _questionService = questionService;
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{quizId}/questions-with-answers")]
        public async Task<IActionResult> GetAllQuestionsOfTheQuiz(int quizId)
        {
            var questions = await _questionService.GetQuestionsWithOptionsByQuizId(quizId);
            if(!questions.Any())
                return NotFound();
            return Ok(questions);
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("{quizId}/questions")]
        public async Task<IActionResult> GetQuestionsByQuizForStudent(int quizId)
        {
            var questions = await _quizService.GetQuizQuestionsForStudent(quizId);
            if(!questions.Any())
                return NotFound();
            return Ok(questions);
        }
        



        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateDto quizDto)
        {
            var userId = User.GetUserId();
            var createQuiz = new CreateQuizDto
            {
                Description = quizDto.Description,
                MaxAttempts = quizDto.MaxAttempts,
                PassPercentage = quizDto.PassPercentage,
                TeacherId = userId,
                IsPublished = quizDto.IsPublished,
                Title = quizDto.Title,
                DurationInMinutes = quizDto.DurationInMinutes,
                AvailableFrom = quizDto.AvailableFrom,
                AvailableTo = quizDto.AvailableTo
            };
            await _quizService.CreateQuiz(createQuiz);
                return Created();

        }



        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPost("{quizId}/add-questions")]
        public async Task<IActionResult> AddQuestionsToQuiz(int quizId, [FromBody] List<int> questionIds)
        {
            var userId = User.GetUserId();
            var quizQuestionDto = new CreateQuizQuestionsDto
            {
                QuizId = quizId,
                QuestionIds = questionIds,
            };
            await _quizService.AddQuestionsToQuiz(userId, quizQuestionDto);
            return Created();
        }


        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpPost("{quizId}/add-random-questions")]
        public async Task<IActionResult> AddRandomQuestions(int quizId,[FromQuery] int count,[FromQuery] int? topicId = null)
        {
            if (count <= 0)
                return BadRequest("Count must be greater than zero.");

            var teacherId = User.GetUserId();

            var addedCount = await _quizService.AddRandomQuestionsToQuizAsync(teacherId, quizId, topicId, count);

            return Ok(new { addedCount, message = $"{addedCount} questions added successfully." });
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("published")]
        public async Task<IActionResult> GetAllQuizzes(CancellationToken ct)
        {
            var quizzes = await _quizService.GetAllQuizs(ct);
            return Ok(quizzes);
        }


        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyQuizzes(CancellationToken ct)
        {
            var userId = User.GetUserId();
            var quizzes = await _quizService.GetMyQuizzes(userId,ct);
            return Ok(quizzes);
        }



        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("drafted")]
        public async Task<IActionResult> GetAllDraftQuizzes(CancellationToken ct)
        {
            var quizzes = await _quizService.GetAllDraftQuizs(ct);
            return Ok(quizzes);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("{id}/published")]
        public async Task<IActionResult> GetPublishQuizById(int id)
        {
            var quiz = await _quizService.GetPublishedQuizById(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("{id}/drafted")]
        public async Task<IActionResult> GetDraftQuizById(int id)
        {
            var quiz = await _quizService.GetDraftQuizById(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] QuizUpdateDto quizDto)
        {
            var userId = User.GetUserId();
            var userRoleName = User.GetUserRole();
                await _quizService.UpdateQuiz(id, quizDto, userId,userRoleName);
                return NoContent();
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var userId = User.GetUserId();
            var userRoleName = User.GetUserRole();
                await _quizService.DeleteQuiz(id, userId,userRoleName);
                return NoContent();
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("{quizId}/leaderboard")]
        public async Task<IActionResult> leaderboardByQuizId(int quizId,CancellationToken ct)
        {
            // add pagination later
            var result = await _quizService.LeaderboardByQuizId(quizId,ct);
            return Ok(result);
        }
    }
    /*
     GetAllQuizzes
GetMyQuizzes
GetAllDraftQuizzes
GetAllQuestionsOfTheQuiz
LeaderboardByQuizId
     
     
     */
}
