using Business.Helper;
using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Option;
using Models.DTOs.Pagination;
using Models.DTOs.Question;
using Models.Entities;
using System.Data;
namespace Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        public  AppDbContext _context;
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddQuestion(CreateQuestionDto question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));


            bool topicExists = await _context.Topics
                .AnyAsync(t => t.Id == question.TopicId);

            if (!topicExists)
                throw new KeyNotFoundException("Topic not found.");

            var entity = new Question
            {
                TopicId = question.TopicId,
                Content = question.Content.Trim(),
                Difficulty = question.Difficulty?.Trim(),
                Points = question.Points,
                CreatedByTeacherId = question.TeacherId
            };

            await _context.Questions.AddAsync(entity);
    
        }

        public async Task DeleteQuestion(int questionId,int userId,string roleName)
        {
            // if there not any quiz that contains this question,
            // we can delete it, otherwise we will not delete it to avoid any issues with the quizzes that contain this question
            if (questionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(questionId));

            var question = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
                throw new KeyNotFoundException("Question not found.");

            bool isUsedInQuiz = await _context.QuizQuestions
                .AnyAsync(q => q.QuestionId == questionId);

            if (isUsedInQuiz)
                throw new InvalidOperationException(
                    "Cannot delete question assigned to quizzes.");
            if (question.CreatedByTeacherId != userId && roleName != RolesConstants.Admin)
                throw new UnauthorizedAccessException("can not delete Question of other Teachers");

            _context.Questions.Remove(question);
        }

        public async Task<QuestionDetailsDto?> GetQuestionById(int questionId)
        {
            if (questionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(questionId));

            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.Id == questionId)
                .Select(q => new QuestionDetailsDto
                {
                    Id = q.Id,
                    TopicId = q.TopicId,
                    Content = q.Content,
                    Difficulty = q.Difficulty,
                    Points = q.Points,

                    Options = q.Options.Select(o => new OptionDto
                    {
                        Id = o.Id,
                        Content = o.Content,
                        IsCorrect = o.IsCorrect
                    })
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByDifficulty(QuestionByDifficultAndCountDto dto)
        {
            return await _context.Questions
                          .AsNoTracking()
                          .Where(q => q.Difficulty == dto.Difficulty)
                          .Take(dto.Count)
                          .Select(q => new QuestionDetailsDto
                          {
                              Id = q.Id,
                              TopicId = q.TopicId,
                              Content = q.Content,
                              Difficulty = q.Difficulty,
                              Points = q.Points,

                              Options = q.Options.Select(o => new OptionDto
                              {
                                  Id = o.Id,
                                  Content = o.Content,
                                  IsCorrect = o.IsCorrect
                              })
                          })
                          .ToListAsync();

          
        }

        public async Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopic(int topicId)
        {
            if (topicId <= 0)
                throw new ArgumentOutOfRangeException(nameof(topicId));
            // add include to load the related options data in a single query to improve performance and reduce the number of database calls
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.TopicId == topicId)
                .Select(q => new QuestionDetailsDto
                {
                    Id = q.Id,
                    TopicId = q.TopicId,
                    Content = q.Content,
                    Difficulty = q.Difficulty,
                    Points = q.Points,

                    Options = q.Options.Select(o => new OptionDto
                    {
                        Id = o.Id,
                        Content = o.Content,
                        IsCorrect = o.IsCorrect
                    })
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<QuestionDetailsDto>> GetQuestionsByTopicAndDifficulty(QuestionByTopicAndDifficultyDto questionByTopicAndDifficulty)
        {

            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.TopicId == questionByTopicAndDifficulty.TopicId && q.Difficulty == questionByTopicAndDifficulty.Difficulty)
                .Take(questionByTopicAndDifficulty.Count)
                .Select(q => new QuestionDetailsDto
                {
                    Id = q.Id,
                    TopicId = q.TopicId,
                    Content = q.Content,
                    Difficulty = q.Difficulty,
                    Points = q.Points,
                    Options = q.Options.Select(
                        o=> new OptionDto
                        {
                            Id = o.Id,
                            Content = o.Content,
                            IsCorrect = o.IsCorrect
                        })
                })
                .ToListAsync();
        }

        public async Task UpdateQuestion(QuestionUpdateDto question,string roleName)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            var existing = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == question.Id);

            if (existing == null)
                throw new KeyNotFoundException("Question not found.");

            bool topicExists = await _context.Topics
                .AnyAsync(t => t.Id == question.TopicId);

            if (!topicExists)
                throw new KeyNotFoundException("Topic not found.");

            if (existing.CreatedByTeacherId != question.TeacherId && roleName != RolesConstants.Admin)
                throw new UnauthorizedAccessException("You cannot edit questions created by another teacher.");

            existing.TopicId = question.TopicId;
            existing.Content = question.Content.Trim();
            existing.Difficulty = question.Difficulty?.Trim();
            existing.Points = question.Points;

            
        }

        public async Task<IEnumerable<QuestionDetailsDto>> GetQuestionsWithOptionsByQuizId(int quizId)
        {
            return await _context.QuizQuestions
                .Include(q => q.Quiz)
                .Include(q => q.Question)
                .Where(qq => qq.QuizId == quizId)
                .Select(qq => new QuestionDetailsDto
                {
                    Id = qq.Question.Id,
                    TopicId = qq.Question.TopicId,
                    Content = qq.Question.Content,
                    Difficulty = qq.Question.Difficulty,
                    Points = qq.Question.Points,

                    Options = qq.Question.Options
                        .Select(o => new OptionDto
                        {
                            Id = o.Id,
                            Content = o.Content,
                            IsCorrect = o.IsCorrect
                        })
                })
                .ToListAsync();
        }
        public async Task<PaginatedResult<QuestionDto>> GetMyQuestions(int page,  int size, int teacherId)
        {
            var query = _context.Questions
                .AsNoTracking()
                .Where(q => q.CreatedByTeacherId == teacherId);

            var totalCount = await query.CountAsync();

            var questions = await query
              .OrderBy(q => q.Id)
                .Select(q => new QuestionDto
                {
                    Content = q.Content,
                    Difficulty = q.Difficulty,
                    Id = q.Id,
                    Points = q.Points,
                    TopicId = q.TopicId,
                    TopicName = q.Topic.Name
                })
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PaginatedResult<QuestionDto>
            {
                Items = questions,
                Page = page,
                Size = size,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / size)
            };
        }
        public async Task<PaginatedResult<QuestionDto>> GetAllQuestions(int page, int size)
        {
            var query = _context.Questions
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var questions = await query
              .OrderBy(q => q.Id)
                .Select(q => new QuestionDto
                {
                    Content = q.Content,
                    Difficulty = q.Difficulty,
                    Id = q.Id,
                    Points = q.Points,
                    TopicId = q.TopicId,
                    TopicName = q.Topic.Name
                })
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PaginatedResult<QuestionDto>
            {
                Items = questions,
                Page = page,
                Size = size,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / size)
            };
        }
        public async Task<QuestionStatisticsDto?> GetQuestionStatistics(int teacherId,int questionId)
        {
            var isQuestionCreatedByTeacher = await _context.Questions.AsNoTracking().AnyAsync(q => q.Id == questionId && q.CreatedByTeacherId == teacherId);
            
            if (!isQuestionCreatedByTeacher)
                throw new UnauthorizedAccessException("You can't Acssess the other question of other teacher");

            var result = await _context.AttemptAnswers
                .Where(a => a.QuestionId == questionId)
                .GroupBy(a => a.QuestionId)
                .Select(g => new QuestionStatisticsDto
                {
                    QuestionId = g.Key,
                    TimesAnswered = g.Count(),
                    CorrectAnswers = g.Count(x => x.IsCorrect == true),
                    SuccessRate =
                        g.Count() == 0
                            ? 0
                            : (decimal)g.Count(x => x.IsCorrect == true) * 100 / g.Count(), 
                   
                })
                .FirstOrDefaultAsync();
            return result ?? new QuestionStatisticsDto
            {
                QuestionId = questionId,
                TimesAnswered = 0,
                CorrectAnswers = 0,
                SuccessRate = 0
            };
        }

    }
}


