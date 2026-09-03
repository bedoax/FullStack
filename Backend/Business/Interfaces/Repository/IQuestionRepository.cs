using Models.DTOs.Pagination;
using Models.DTOs.Question;

namespace Business.Interfaces.Repository
{
    public interface IQuestionRepository
    {
        Task<QuestionDetailsDto> GetQuestionById(
            int questionId);
        Task<PaginatedResult<QuestionDto>> GetMyQuestions(int page, int size, int teacherId);
        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopic(
            int topicId);

        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByDifficulty(
QuestionByDifficultAndCountDto dto);

        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopicAndDifficulty(
            QuestionByTopicAndDifficultyDto dto);
        Task<QuestionStatisticsDto?> GetQuestionStatistics(int teacherId,int questionId);
        Task AddQuestion(
            CreateQuestionDto question);

        Task UpdateQuestion(QuestionUpdateDto question, string roleName);

        Task DeleteQuestion(int questionId, int userId, string roleName);
        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsWithOptionsByQuizId(int quizId);
        Task<PaginatedResult<QuestionDto>> GetAllQuestions(int page, int size);
    }
}
