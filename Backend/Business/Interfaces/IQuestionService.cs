using Models.DTOs.Option;
using Models.DTOs.Pagination;
using Models.DTOs.Question;


namespace Business.Interfaces
{
    public interface IQuestionService
    {
        /*
         Create Question
         Update Question
         Delete Question
         Get Question Details
         Get Questions By Topic
         Get Questions By Difficulty
         Get Questions By Topic & Difficulty

        // maybe do internal service for this and integrate into question service 
        Add Option To Question
        Update Option
        Delete Option
        Get Question Options

         */
        Task CreateQuestion(CreateQuestionDto questionCreateDto);
        Task UpdateQuestion(QuestionUpdateDto question, string roleName);
        Task DeleteQuestion(int id,int userId,string RoleName);
        Task<QuestionDetailsDto> GetQuestionById(int id);
        Task<QuestionStatisticsDto?> GetQuestionStatistics(int teacherId, int questionId);
        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByDifficulty(QuestionByDifficultAndCountDto questionByDifficult);
        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopicAndDifficulty(QuestionByTopicAndDifficultyDto questionByTopicAndDifficulty);
        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsWithOptionsByQuizId(int quizId);
        Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopic(int topicId);
        Task<IEnumerable<OptionForStudentDto>> GetQuestionOptions(int questionId);
        Task<PaginatedResult<QuestionDto>> GetTeacherQuestionsNotInQuizAsync(int teacherId, int quizId, int? topicId, int page, int pageSize);
        Task<PaginatedResult<QuestionDto>> GetAllQuestions(int page, int size);

    }


}
