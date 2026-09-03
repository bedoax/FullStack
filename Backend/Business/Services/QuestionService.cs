using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.Option;
using Models.DTOs.Pagination;
using Models.DTOs.Question;

namespace Business.Services
{
   

    public class QuestionService : IQuestionService
    {
        private IUnitOfWork _unitOfWork;
        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateQuestion(CreateQuestionDto questionCreateDto)
        {

            await _unitOfWork.Questions.AddQuestion(questionCreateDto);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteQuestion(int id,int userId,string roleName)
        {
            await _unitOfWork.Questions.DeleteQuestion(id,userId,roleName);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<QuestionDetailsDto> GetQuestionById(int id)
        {
            return _unitOfWork.Questions.GetQuestionById(id);
        }

        public Task<IEnumerable<OptionForStudentDto>> GetQuestionOptions(int questionId)
        {
            return _unitOfWork.Options.GetQuestionOptions(questionId);
        }

        public Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByDifficulty(QuestionByDifficultAndCountDto questionByDifficult)
        {
            return _unitOfWork.Questions.GetQuestionsByDifficulty(questionByDifficult);
        }

        public Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopic(int topicId)
        {
            return _unitOfWork.Questions.GetQuestionsByTopic(topicId);
        }

        public Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopicAndDifficulty(QuestionByTopicAndDifficultyDto questionByTopicAndDifficulty)
        {
            return _unitOfWork.Questions.GetQuestionsByTopicAndDifficulty(questionByTopicAndDifficulty);
        }

        public Task<IEnumerable<QuestionDetailsDto>> GetQuestionsWithOptionsByQuizId(int quizId)
        {
            return _unitOfWork.Questions.GetQuestionsWithOptionsByQuizId(quizId);
        }

        public async Task UpdateQuestion(QuestionUpdateDto questionUpdateDto,string rolename)
        {
            await _unitOfWork.Questions.UpdateQuestion(questionUpdateDto,rolename);
            await _unitOfWork.SaveChangesAsync();
        }
        public Task<QuestionStatisticsDto?> GetQuestionStatistics(int teacherId, int questionId)
        {
            return _unitOfWork.Questions.GetQuestionStatistics(teacherId,questionId);
        }
        public Task<PaginatedResult<QuestionDto>> GetTeacherQuestionsNotInQuizAsync(int teacherId, int quizId,int? topicId, int page, int pageSize)
        {
            return _unitOfWork.QuizQuestions.GetTeacherQuestionsNotInQuizAsync(teacherId, quizId, topicId,page, pageSize);
        }
        public Task<PaginatedResult<QuestionDto>> GetAllQuestions(int page, int size)
        {
            return _unitOfWork.Questions.GetAllQuestions(page, size);
        }
    }
}
