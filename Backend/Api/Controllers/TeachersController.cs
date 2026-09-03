using Business.Helper;
using Business.Interfaces;
using Business.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        /*
         GET /api/teacher/quizzes
        GET /api/teacher/questions
        GET /api/teacher/quizzes/{quizId}/attempts
        GET /api/teacher/quizzes/{quizId}/statistics
         */
        private ITeacherService _teacherSerivce;
        private IQuestionService _questionSerivce;
        public TeachersController(ITeacherService teacherSerivce,IQuestionService questionService)
        {
            _teacherSerivce = teacherSerivce;
            _questionSerivce = questionService;
        }


        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("quizzes")]
        public async Task<IActionResult> GetMyQuizzes(CancellationToken ct)
        {
            var teacherId = User.GetUserId();
            var quizzes = await _teacherSerivce.GetMyQuizzes(teacherId,ct);
            return Ok(quizzes);
        }
        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("questions/not-in-quiz")]
        public async Task<IActionResult> GetTeacherQuestionsNotInQuiz([FromQuery] int quizId, [FromQuery] int page = 1, [FromQuery] int? topicId = null, [FromQuery] int size = 10)
        {
            var teacherId = User.GetUserId();

            var questions = await _questionSerivce.GetTeacherQuestionsNotInQuizAsync(teacherId, quizId, topicId, page, size);

            return Ok(questions);
        }

        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("quizzes/{quizId}/attempts")]
        public async Task<IActionResult> GetMyAttemptsByQuizId(int quizId)
        {
            var teacherId = User.GetUserId();
            var attempts = await _teacherSerivce.GetMyAttemptsByQuizId(teacherId, quizId);
            return Ok(attempts);
        }

        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("quizzes/{quizId}/statistics")]
        public async Task<IActionResult> GetStaticsByQuizId(int quizId)
        {
            var teacherId = User.GetUserId();
            var statistics = await _teacherSerivce.GetStaticsByQuizId(teacherId, quizId);
            return Ok(statistics);
        }

        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("quizzes/{quizId}/students")]
        public async Task<IActionResult> GetStudentsOfQuiz(int quizId)
        {
            var teacherId = User.GetUserId();
            var studentsQuiz = await _teacherSerivce.GetStudentsOfQuiz(teacherId, quizId);
            return Ok(studentsQuiz);
        }

        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("questions")]
        public async Task<IActionResult> GetMyQuestions([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var teacherId = User.GetUserId();
            var questions = await _teacherSerivce.GetMyQuestions(page, size, teacherId);
            return Ok(questions);
        }


        // for testing later by admin
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{teacherId}/quizzes")]
        public async Task<IActionResult> GetMyQuizzes(int teacherId,CancellationToken ct)
        {
            var quizzes = await _teacherSerivce.GetMyQuizzes(teacherId,ct);
            return Ok(quizzes);
        }


        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{teacherId}/quizzes/{quizId}/attempts")]
        public async Task<IActionResult> GetMyAttemptsByQuizId(int teacherId, int quizId)
        {
            var attempts = await _teacherSerivce.GetMyAttemptsByQuizId(teacherId, quizId);
            return Ok(attempts);
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{teacherId}/quizzes/{quizId}/statistics")]
        public async Task<IActionResult> GetStaticsByQuizId(int teacherId, int quizId)
        {
            var statistics = await _teacherSerivce.GetStaticsByQuizId(teacherId, quizId);
            return Ok(statistics);
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{teacherId}/quizzes/{quizId}/students")]
        public async Task<IActionResult> GetStudentsOfQuiz(int teacherId, int quizId)
        {
            var studentsQuiz = await _teacherSerivce.GetStudentsOfQuiz(teacherId, quizId);
            return Ok(studentsQuiz);
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{teacherId}/questions")]
        public async Task<IActionResult> GetMyQuestions([FromQuery] int page = 1, [FromQuery] int size = 10, int teacherId = 0)
        {
            var questions = await _teacherSerivce.GetMyQuestions(page, size, teacherId);
            return Ok(questions);
        }

        [Authorize(Roles =RolesConstants.Teacher)]
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard(CancellationToken ct)
        {
            var userId = User.GetUserId();
            var result = await _teacherSerivce.TeacherDashboard(userId,ct);
            return Ok(result);
        }
        /*
         *  GET /api/teachers/dashboard
         * the output expected for get student dashborad 
         {
          "quizzes": 15,
          "questions": 240,
          "students": 180,
          "averagePassRate": 78
        }
         */
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{teacherId}/dashboard")]
        public async Task<IActionResult> Dashboard(int teacherId,CancellationToken ct)
        { 
            var result = await _teacherSerivce.TeacherDashboard(teacherId,ct);
            return Ok(result);
        }
    }
}
