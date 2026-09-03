using Models.DTOs.Attempt;
using Models.DTOs.AttemptAnswer;
using Models.DTOs.Quiz;
using Models.Entities;

namespace Business.Interfaces.Repository
{
    public interface IAttemptRepository
    {
        Task<AttemptDetailsDto> GetAttemptById(int attemptId);

        Task<IEnumerable<AttemptListDto>> GetUserAttempts(int userId,CancellationToken ct);
        Task<List<TeacherAttemptStudentDto>> GetMyAttemptsByQuizId(int teacherId, int quizId);
        Task<List<QuizStudentDto>> GetStudentsOfQuiz(int teacherId, int quizId);
        Task<AttemptDetailsDto> GetLastAttempt(int userId);
        Task<Attempt?> GetAttemptEntityWithQuiz(int attemptId);
        Task<int> GetAttemptsCount( int userId,int quizId);
        Task<IEnumerable<AttemptListDto>> GetAttemptsByQuiz(int quizId);
        Task<IEnumerable<UserQuizAttemptsDto>> GetAttemptsCountPerQuiz(int userId);
        Task<Attempt> AddAttempt( CreateAttemptDto attempt);
        Task<AttemptWithQuizDto?> GetAttemptWithQuizDetails(int AttemptId);
        Task<AttemptReviewDto> ReviewMyAttempt(int userId, int attemptId);

    }
}
