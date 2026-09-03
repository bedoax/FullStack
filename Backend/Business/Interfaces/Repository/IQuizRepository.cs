using Models.DTOs.Quiz;
using Models.DTOs.StudentTopicPerformance;
using Models.Entities;

namespace Business.Interfaces.Repository
{
    public interface IQuizRepository
    {
        Task<IEnumerable<TeacherQuizDto>> GetMyQuizzes(int teacherId,CancellationToken ct);
        Task<QuizDto?> GetPublishedQuizById(int quizId);

        Task<QuizDto?> GetDraftQuizById(int quizId);
        Task<IEnumerable<QuizDto>> GetAllQuizzes(CancellationToken ct);

        Task<IEnumerable<QuizDto>> GetQuizzesByUserId(
            int userId);
        Task<TeacherDashboardDto> TeacherDashboard(int teacherId,CancellationToken ct);
        Task AddQuiz(
            CreateQuizDto quiz);
        Task<IEnumerable<StudentQuizDto>> GetAllQuizzesByUserId(int userId, CancellationToken ct);
        Task<IEnumerable<LeaderboardDto>> LeaderboardByQuizId(int quizId,CancellationToken ct);
        Task UpdateQuiz(int id, QuizUpdateDto quiz, int TeacherId, string userRoleName);
        Task<IEnumerable<QuizDto>> GetAllDraftQuizzes(CancellationToken ct);
        Task DeleteQuiz(int quizId, int userId, string userRoleName);
    }
}
