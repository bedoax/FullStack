using Business.Helper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Question;
using Models.DTOs.QuizQuestionDto;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        /*
         POST /questions
PUT /questions/{id}
DELETE /questions/{id}
GET /questions/{id}
GET /quizzes/{quizId}/questions
         
         */
        private IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateDto questionDto)
        {
            var userId = User.GetUserId();
            var createQuestion = new CreateQuestionDto
            {
                Content = questionDto.Content,
                Difficulty = questionDto.Difficulty,
                Points = questionDto.Points,
                TopicId = questionDto.TopicId,
                TeacherId = userId
            };
                await _questionService.CreateQuestion(createQuestion);
                return StatusCode(201);


        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpGet("AdminOrTeacher/{id:int}")]
        public async Task<IActionResult> GetQuestionById(int id)
        {

            var questions = await _questionService.GetQuestionById(id);

            if(questions == null)
                return NotFound();

            return Ok(questions);
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("difficulties/{difficulty}")]
        public async Task<IActionResult> GetQuestionsByDifficulty( string difficulty, int count = 10)
        {
            var dto = new QuestionByDifficultAndCountDto
            {
                Difficulty = difficulty,
                Count = count
            };
            var questions = await _questionService.GetQuestionsByDifficulty(dto);
            
            if(!questions.Any())
            return NotFound();

            return Ok(questions);
        }
        /*            [HttpGet("{id}")]
                public async Task<IActionResult> GetQuestionById(int id)
                {
                    var question = await _questionService.GetQuestionById(id);
                    return Ok(question);
                }*/
        
        [Authorize(Roles = RolesConstants.Student)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetQuestionByIdForStudent(int id)
        {
            var questions = await _questionService.GetQuestionOptions(id);

            if(!questions.Any())
                return NotFound();

            return Ok(questions);
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionRequest questionDto)
        {
            var userId = User.GetUserId();
            var userRoleName = User.GetUserRole();
            var questionToUpdate = new QuestionUpdateDto
            {
                Id = id,
                TopicId = questionDto.TopicId,
                Content = questionDto.Content,
                Difficulty = questionDto.Difficulty,
                Points = questionDto.Points,
                TeacherId = userId
            };

                await _questionService.UpdateQuestion(questionToUpdate, userRoleName);
                return NoContent();


        }

        [Authorize(Roles = RolesConstants.AdminOrTeacher)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {

            var userId = User.GetUserId();
            var roleName = User.GetUserRole();
                await _questionService.DeleteQuestion(id,userId,roleName);
                return NoContent();

        }

        [Authorize(Roles = RolesConstants.Teacher)]
        [HttpGet("{questionId}/statics")]
        public async Task<IActionResult>GetQuestionStatics(int questionId)
        {
            var userId = User.GetUserId();
            var result = await _questionService.GetQuestionStatistics(userId,questionId);
            return Ok(result);
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("all-questions")]
        public async Task<IActionResult> GetAllQuestions([FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var result =await _questionService.GetAllQuestions(page, pageSize);
            return Ok(result);
        }
        
        // u can make the same for admin 
        /*        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{questionId}/statics-admin")]
        public async Task<IActionResult> GetQuestionStaticsByAdmin(int questionId)
        {
            var adminRole = User.GetUserId();
            var result = await _questionService.GetQuestionStatistics(userId, RolesConstants.Admin,questionId);
            return Ok(result);
        }*/

    }
}
