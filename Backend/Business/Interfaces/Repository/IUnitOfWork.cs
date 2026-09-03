using Microsoft.EntityFrameworkCore.Storage;
namespace Business.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }

        IQuizRepository Quizzes { get; }
        IQuizQuestionsRepository QuizQuestions { get; }

        IQuestionRepository Questions { get; }
        IOptionRepository Options { get; }

        IAttemptRepository Attempts { get; }
        IAttemptAnswersRepository AttemptAnswers { get; }

        IStudentProfileRepository StudentProfiles { get; }
        IStudentTopicPerformanceRepository StudentTopicPerformances { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        ITopicRepository Topics { get; }
        IOtpRepository OtpRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();
    }
}
