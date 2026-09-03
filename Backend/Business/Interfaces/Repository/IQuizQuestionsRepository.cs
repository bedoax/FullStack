using Models.DTOs.Pagination;
using Models.DTOs.Question;
using Models.DTOs.QuizQuestionDto;

namespace Business.Interfaces.Repository
{
    public interface IQuizQuestionsRepository
    {
        // dont forget to new entity QuizQuestion in Models/Entities folder
        Task AddQuizQuestion(
            CreateQuizQuestionDto quizQuestion);
        Task AddQuizQuestions(int userId, CreateQuizQuestionsDto quizQuestions);
        Task RemoveQuizQuestion(
            int quizId,
            int questionId);
        Task<PaginatedResult<QuestionDto>> GetTeacherQuestionsNotInQuizAsync(int teacherId, int quizId, int? topicId, int page, int pageSize);
        Task<IEnumerable<QuestionDetailsDto>>
            GetQuizQuestions(
                int quizId);
        Task<IEnumerable<QuestionDetailsDtoForStudent>> GetQuizQuestionsForStudent(int quizId);
        Task<int> AddRandomQuestionsToQuizAsync(int teacherId, int quizId, int? topicId, int count);
    }
}
