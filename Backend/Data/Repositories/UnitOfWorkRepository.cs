using Business.Interfaces.Repository;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public IQuizRepository Quizzes { get; }
    public IQuizQuestionsRepository QuizQuestions { get; }
    public IQuestionRepository Questions { get; }
    public IOptionRepository Options { get; }
    public IAttemptRepository Attempts { get; }
    public IAttemptAnswersRepository AttemptAnswers { get; }
    public IStudentProfileRepository StudentProfiles { get; }
    public IStudentTopicPerformanceRepository StudentTopicPerformances { get; }
    public ITopicRepository Topics { get; }
    public IRefreshTokenRepository RefreshTokenRepository { get; }
    public IOtpRepository OtpRepository { get; }

    public UnitOfWork(
            AppDbContext context,
            IUserRepository users,
            IRoleRepository roles,
            IRefreshTokenRepository refreshTokenRepository,
            IQuizRepository quizzes,
            IQuizQuestionsRepository quizQuestions,
            IQuestionRepository questions,
            IOptionRepository options,
            IOtpRepository otpRepository,
            IAttemptRepository attempts,
            IAttemptAnswersRepository attemptAnswers,
            IStudentProfileRepository studentProfiles,
            IStudentTopicPerformanceRepository studentTopicPerformances,
            ITopicRepository topics)
    {
        _context = context;

        Users = users;
        Roles = roles;
        RefreshTokenRepository = refreshTokenRepository;
        Quizzes = quizzes;
        QuizQuestions = quizQuestions;
        Questions = questions;
        Options = options;
        OtpRepository = otpRepository;
        Attempts = attempts;
        AttemptAnswers = attemptAnswers;
        StudentProfiles = studentProfiles;
        StudentTopicPerformances = studentTopicPerformances;
        Topics = topics;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if(_currentTransaction != null)
        {
            return _currentTransaction;
        }
        _currentTransaction = await _context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    public async Task CommitAsync()
    {
        try
        {

            if (_currentTransaction != null)
                await _currentTransaction.CommitAsync();
        }
        catch
        {
            if (_currentTransaction != null)
                await _currentTransaction.RollbackAsync();

            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
            }
        }
        finally
        {
            if( _currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}