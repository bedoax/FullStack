using Business.Helper;
using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs.Quiz;
using Models.DTOs.StudentTopicPerformance;
using Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Data.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private AppDbContext _context;
        private readonly ILogger<QuizRepository> _logger;
        public QuizRepository(AppDbContext context, ILogger<QuizRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddQuiz(CreateQuizDto quiz)
        {
            var newQuiz = new Quiz
            {
                Title = quiz.Title.Trim(),
                Description = quiz.Description?.Trim(),
                MaxAttempts = quiz.MaxAttempts,
                PassPercentage = quiz.PassPercentage,
                CreatedAt = DateTime.UtcNow,
                CreatedByTeacherId = quiz.TeacherId,
                IsPublished = quiz.IsPublished,
                AvailableFrom = quiz.AvailableFrom,
                AvailableTo = quiz.AvailableTo,
                DurationMinutes = quiz.DurationInMinutes,
                
            };
            await _context.Quizzes.AddAsync(newQuiz);
        }
        public async Task DeleteQuiz(int quizId,int userId,string userRoleName)
        {
            // see if you will use soft delete or use redirct delete to remove the quiz and attempts related to it
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found.");

            bool hasAttempts = await _context.Attempts
                .AnyAsync(a => a.QuizId == quizId);

            if (hasAttempts)
                throw new InvalidOperationException(
                    "Cannot delete quiz that contains attempts.");

            if (quiz.CreatedByTeacherId != userId && userRoleName != RolesConstants.Admin)
                throw new UnauthorizedAccessException("Can not delete quiz of other Teacher");

            _context.Quizzes.Remove(quiz);

        }

        public async Task<IEnumerable<TeacherQuizDto>> GetMyQuizzes(int teacherId,CancellationToken ct)
        {
            return await _context.Quizzes
                .Where(q => q.CreatedByTeacherId == teacherId).Select(q=> new TeacherQuizDto
                {
                    AvailableFrom = q.AvailableFrom,
                    AvailableTo = q.AvailableTo,
                    CreatedAt = q.CreatedAt,
                    CreatedByTeacherId = q.CreatedByTeacherId,
                    Description = q.Description,
                    DurationMinutes = q.DurationMinutes,
                    Id = q.Id,
                    IsPublished = q.IsPublished,
                    MaxAttempts = q.MaxAttempts,
                    PassPercentage = q.PassPercentage,
                    Title = q.Title,
                })
                .ToListAsync(ct);

        }
        public async Task<IEnumerable<QuizDto>> GetAllQuizzes(CancellationToken ct)
        {
            // we need to do pagination here to avoid loading too many records at once, but for now we will return all quizzes
            return await _context.Quizzes
                .AsNoTracking()
                .Where(q=>q.IsPublished)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    MaxAttempts = q.MaxAttempts,
                    PassPercentage = q.PassPercentage,
                    CreatedAt = q.CreatedAt,
                    DurationMinutes = q.DurationMinutes, 
                    AvailableFrom = q.AvailableFrom,     
                    AvailableTo = q.AvailableTo,
                })
                .ToListAsync(ct);
        }
        public async Task<IEnumerable<QuizDto>> GetAllDraftQuizzes(CancellationToken ct)
        {
            // we need to do pagination here to avoid loading too many records at once, but for now we will return all quizzes
            return await _context.Quizzes
                .AsNoTracking()
                .Where(q => !q.IsPublished)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    MaxAttempts = q.MaxAttempts,
                    PassPercentage = q.PassPercentage,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync(ct);
        }
        /// <summary>
        /// Retrieves all programming quizzes attempted by the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose attempted quizzes are being fetched.</param>
        /// <returns>A collection of <see cref="QuizDto"/> representing the unique quizzes attempted by the user.</returns>
        public async Task<IEnumerable<QuizDto>> GetQuizzesByUserId(int userId)
        {
            // we need to do pagination here to avoid loading too many records at once, but for now we will return all quizzes attempted by the user
            return await _context.Attempts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Select(a => a.Quiz)
                .Distinct()
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    MaxAttempts = q.MaxAttempts,
                    PassPercentage = q.PassPercentage,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();
        }

        /*public async Task<IEnumerable<StudentQuizDto>> GetAllQuizzesByUserId(int userId)
        {
            var quizzes = await (

                             from quiz in _context.Quizzes

                             where quiz.IsPublished

                             join attempt in _context.Attempts.Where(a => a.UserId == userId)

                                 on quiz.Id equals attempt.QuizId

                                 into attempts

                             
                             select new StudentQuizDto
                             {
                                 Id = quiz.Id,

                                 Title = quiz.Title,

                                 Description = quiz.Description,

                                 MaxAttempts = quiz.MaxAttempts ?? 0,

                                 PassPercentage = quiz.PassPercentage ?? 0,
                                 DurationMinutes = quiz.DurationMinutes,

                                 AttemptsUsed = attempts.Count(),

                                 HasActiveAttempt = attempts.Any(a => a.SubmittedAt == null),

                                 EndsAt = attempts
                                     .Where(a => a.SubmittedAt == null)
                                     .Select(a => a.EndsAt)
                                     .FirstOrDefault(),

                                 Passed = attempts
                                     .Where(a => a.SubmittedAt != null)
                                     .OrderByDescending(a => a.AttemptNumber)
                                     .Select(a => a.Passed)
                                     .FirstOrDefault(),

                                 CanStart =
                                     attempts.Count() < (quiz.MaxAttempts ?? 0)
                                     &&
                                     !attempts.Any(a => a.SubmittedAt == null),

                                 IsAvailable =
                                     (!quiz.AvailableFrom.HasValue || quiz.AvailableFrom <= DateTime.UtcNow)
                                     &&
                                     (!quiz.AvailableTo.HasValue || quiz.AvailableTo >= DateTime.UtcNow)
                             }).ToListAsync();
            return quizzes;
        }*/

        public async Task<IEnumerable<StudentQuizDto>> GetAllQuizzesByUserId(int userId,CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var query =
                from quiz in _context.Quizzes

                join attempt in _context.Attempts
                    .Where(a => a.UserId == userId)
                    on quiz.Id equals attempt.QuizId
                    into attempts

                where quiz.IsPublished

                let attemptsUsed = attempts.Count()

                let activeAttempt = attempts
                    .Where(a =>
                        a.SubmittedAt == null &&
                        a.EndsAt > now)
                    .OrderByDescending(a => a.AttemptNumber)
                    .Select(a => new
                    {
                        a.Id,
                        a.QuizId,
                        a.EndsAt
                    })
                    .FirstOrDefault()

                let latestSubmittedAttempt = attempts
                    .Where(a => a.SubmittedAt != null)
                    .OrderByDescending(a => a.AttemptNumber)
                    .Select(a => (bool?)a.Passed)
                    .FirstOrDefault()

                select new StudentQuizDto
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Description = quiz.Description,

                    MaxAttempts = quiz.MaxAttempts ?? 0,
                    PassPercentage = quiz.PassPercentage ?? 0,
                    DurationMinutes = quiz.DurationMinutes,

                    AttemptsUsed = attemptsUsed,

                    HasActiveAttempt = activeAttempt != null,

                    ActiveAttemptId = activeAttempt != null
                        ? (int?)activeAttempt.Id
                        : null,
                    ActiveQuizId = activeAttempt != null ? (int?)activeAttempt.QuizId : null,
                    EndsAt = activeAttempt != null
                        ? activeAttempt.EndsAt
                        : null,

                    Passed = latestSubmittedAttempt,

                    CanStart =
                        attemptsUsed < (quiz.MaxAttempts ?? 0)
                        && activeAttempt == null,

                    IsAvailable =
                        (!quiz.AvailableFrom.HasValue ||
                         quiz.AvailableFrom <= now)
                        &&
                        (!quiz.AvailableTo.HasValue ||
                         quiz.AvailableTo >= now)
                };

            var sql = query.ToQueryString();

            _logger.LogInformation(
                "GetAllQuizzesByUserId SQL:\n{Sql}",
                sql);

            return await query.ToListAsync(ct);
/*        
 
 *        var quizzes = await _context
                .Set<StudentQuizDto>()
                .FromSqlInterpolated(
                    $"EXEC dbo.GetAllQuizzesByUserId {userId}, {DateTime.UtcNow}")
                .AsNoTracking()
                .ToListAsync();
            return quizzes;*/
        }


        public async Task<QuizDto?> GetPublishedQuizById(int quizId)
        {
            return await _context.Quizzes
                .AsNoTracking()
                .Where(q => q.Id == quizId && q.IsPublished)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    MaxAttempts = q.MaxAttempts,
                    PassPercentage = q.PassPercentage,
                    CreatedAt = q.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
        public async Task<QuizDto?> GetDraftQuizById(int quizId)
        {
            return await _context.Quizzes
                .AsNoTracking()
                .Where(q => q.Id == quizId && !q.IsPublished)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    MaxAttempts = q.MaxAttempts,
                    PassPercentage = q.PassPercentage,
                    CreatedAt = q.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateQuiz(int id,QuizUpdateDto quiz,int TeacherId,string userRoleName)
        {
            var existingQuiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existingQuiz == null)
                throw new KeyNotFoundException("Quiz not found.");

            if (existingQuiz.CreatedByTeacherId != TeacherId && userRoleName != RolesConstants.Admin)
                throw new UnauthorizedAccessException("Can not update quiz of other Teacher");

            if (!string.IsNullOrWhiteSpace(quiz.Title))
                existingQuiz.Title = quiz.Title.Trim();

            if (quiz.Description != null)
                existingQuiz.Description = quiz.Description.Trim();

            if (quiz.MaxAttempts.HasValue)
                existingQuiz.MaxAttempts = quiz.MaxAttempts.Value;

            if (quiz.PassPercentage.HasValue)
                existingQuiz.PassPercentage = quiz.PassPercentage.Value;

            if (quiz.AvailableFrom.HasValue)
                existingQuiz.AvailableFrom = quiz.AvailableFrom.Value;

            if (quiz.AvailableTo.HasValue)
                existingQuiz.AvailableTo = quiz.AvailableTo.Value;

            if (quiz.DurationInMinutes.HasValue)
                existingQuiz.DurationMinutes = quiz.DurationInMinutes.Value;

            if (quiz.IsPublished.HasValue)
                existingQuiz.IsPublished = quiz.IsPublished.Value;
        }

        // the hieghst students with their precentage number 
        public async Task<IEnumerable<LeaderboardDto>> LeaderboardByQuizId(int quizId,CancellationToken ct)
        {
            var studentQuiz = _context.Attempts.Where(q => q.QuizId == quizId).Select(q => new LeaderboardDto
            {
                Score = (decimal)q.Score,
                UserId = q.UserId,
                Username = q.User.Username
            }).OrderByDescending(q=>q.Score).ToListAsync(ct);
            return await studentQuiz;
        }
        public  Task<TeacherDashboardDto> TeacherDashboard(int teacherId,CancellationToken ct)
        {
            var result = _context.Quizzes.Where(x => x.CreatedByTeacherId == teacherId).GroupBy(x => 1).Select(_ => new TeacherDashboardDto
            {
                Quizzes = _context.Quizzes.Count(q=>q.CreatedByTeacherId == teacherId),
                Questions = _context.Questions.Count(q=>q.CreatedByTeacherId == teacherId),
                Students = _context.Attempts.Where(x=>x.Quiz.CreatedByTeacherId == teacherId).Select(x=>x.UserId).Distinct().Count(),
                AveragePassRate = _context.Attempts.Where(x=>x.Quiz.CreatedByTeacherId == teacherId).Average(x=>x.Percentage) ?? 0
            }).FirstOrDefaultAsync(ct);
            
            return result;
        }
    }
}


