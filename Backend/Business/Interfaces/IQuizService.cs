using Models.DTOs.Attempt;
using Models.DTOs.AttemptAnswer;
using Models.DTOs.Auth;
using Models.DTOs.Option;
using Models.DTOs.Question;
using Models.DTOs.Quiz;
using Models.DTOs.QuizQuestionDto;
using Models.DTOs.Role;
using Models.DTOs.StudentProfile;
using Models.DTOs.StudentTopicPerformance;
using Models.DTOs.Topic;
using Models.DTOs.User;
using Models.Entities;


namespace Business.Interfaces
{
    public interface IQuizService
    {
        /*
         Create Quiz
          Update Quiz
          Delete Quiz
          Get Quiz Details
          Get Quiz By Id
          Get All Quizzes

        Add Question To Quiz
        Remove Question From Quiz
        Get Quiz Questions

        // those to do after summtion of quiz 
        Calculate New Skill Score
        Determine Current Level
        Update Student Profile
        Update Topic Performance
        Detect Weak Topics
        Detect Strong Topics
         */
        Task CreateQuiz(CreateQuizDto quizCreateDto);
        Task UpdateQuiz(int id, QuizUpdateDto quiz, int TeacherId, string userRoleName);
        Task<IEnumerable<QuizDto>> GetAllQuizs(CancellationToken ct);
        Task<IEnumerable<QuizDto>> GetAllDraftQuizs(CancellationToken ct);
        Task<IEnumerable<StudentQuizDto>> GetMyQuizzes(int userId,CancellationToken ct);
        Task DeleteQuiz(int quizId, int userId, string userRoleName);
        Task<QuizDto?> GetPublishedQuizById(int id);
        Task<QuizDto?> GetDraftQuizById(int id);
        Task AddQuestionToQuiz(CreateQuizQuestionDto createQuizQuetion);
        Task AddQuestionsToQuiz(int userId, CreateQuizQuestionsDto createQuizQuestions);
        Task<int> AddRandomQuestionsToQuizAsync(int teacherId, int quizId, int? topicId, int count);
        Task RemoveQuestionFromQuiz(int quizId, int questionId);
         Task<IEnumerable<QuestionDetailsDto>> GetQuizQuestions(int quizId);
        Task<IEnumerable<QuestionDetailsDtoForStudent>> GetQuizQuestionsForStudent(int quizId);
        Task<IEnumerable<LeaderboardDto>> LeaderboardByQuizId(int quizId,CancellationToken ct);
    }


}
