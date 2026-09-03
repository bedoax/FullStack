using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Option;
using Models.DTOs.Pagination;
using Models.DTOs.Question;
using Models.DTOs.QuizQuestionDto;
using Models.Entities;
namespace Data.Repositories
{
    public class QuizQuestionsRepository : IQuizQuestionsRepository
    {
        private AppDbContext _context;
        public QuizQuestionsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddQuizQuestion(CreateQuizQuestionDto quizQuestion)
        {
            var validation = await _context.Quizzes
                .Where(q => q.Id == quizQuestion.QuizId)
                .Select(q => new
                {
                    QuizExists = true,
                    QuestionExists = _context.Questions.Any(ques => ques.Id == quizQuestion.QuestionId),
                    RelationExists = _context.QuizQuestions.Any(qq => qq.QuizId == quizQuestion.QuizId && qq.QuestionId == quizQuestion.QuestionId)
                })
                .FirstOrDefaultAsync();

            if (validation is null)
                throw new KeyNotFoundException("Quiz not found.");

            if (!validation.QuestionExists)
                throw new KeyNotFoundException("Question not found.");

            if (validation.RelationExists)
                throw new InvalidOperationException("Question already exists in quiz.");

            await _context.QuizQuestions.AddAsync(new QuizQuestion
            {
                QuizId = quizQuestion.QuizId,
                QuestionId = quizQuestion.QuestionId
            });
        }
        public async Task AddQuizQuestions(int userId, CreateQuizQuestionsDto quizQuestions)
        {
            var distinctQuestionIds = quizQuestions.QuestionIds.Distinct().ToList();

            if (!distinctQuestionIds.Any())
                return;

            var validation = await _context.Quizzes
                .Where(q => q.Id == quizQuestions.QuizId && q.CreatedByTeacherId == userId)
                .Select(q => new
                {
                    QuizExists = true,

                    ValidQuestionsCount = _context.Questions
                                                   .Count(ques =>
                                                       distinctQuestionIds.Contains(ques.Id) &&
                                                       ques.CreatedByTeacherId == userId),

                    ExistingRelationCount = _context.QuizQuestions
                        .Count(qq => qq.QuizId == quizQuestions.QuizId &&
                                     distinctQuestionIds.Contains(qq.QuestionId))
                })
                .FirstOrDefaultAsync();

            if (validation is null)
                throw new KeyNotFoundException("Quiz not found.");

            if (validation.ValidQuestionsCount != distinctQuestionIds.Count)
                throw new InvalidOperationException(
                    "One or more selected questions are invalid."
                );

            if (validation.ExistingRelationCount > 0)
                throw new InvalidOperationException(
                    "One or more questions are already added to this quiz."
                );

            var newQuizQuestions = distinctQuestionIds.Select(questionId => new QuizQuestion
            {
                QuizId = quizQuestions.QuizId,
                QuestionId = questionId
            });

            await _context.QuizQuestions.AddRangeAsync(newQuizQuestions);
        }

        public async Task<IEnumerable<QuestionDetailsDto>> GetQuizQuestions(int quizId)
        {
            if (quizId <= 0)
                throw new ArgumentOutOfRangeException(nameof(quizId));
            // add include to load the related question and options data in a single query to improve performance and reduce the number of database calls
            return await _context.QuizQuestions
                .AsNoTracking()
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
                            // see if you wanna return the answers with it or not
                            IsCorrect = o.IsCorrect
                        })
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<QuestionDetailsDtoForStudent>> GetQuizQuestionsForStudent(int quizId)
        {
            if (quizId <= 0)
                throw new ArgumentOutOfRangeException(nameof(quizId));
            // add include to load the related question and options data in a single query to improve performance and reduce the number of database calls
            return await _context.QuizQuestions
                .AsNoTracking()
                .Where(qq => qq.QuizId == quizId)
                .Select(qq => new QuestionDetailsDtoForStudent
                {
                    Id = qq.Question.Id,
                    TopicId = qq.Question.TopicId,
                    Content = qq.Question.Content,
                    Difficulty = qq.Question.Difficulty,
                    Points = qq.Question.Points,
                    Options = qq.Question.Options
                        .Select(o => new OptionForStudentDto
                        {
                            Id = o.Id,
                            Content = o.Content
                        })
                })
                .ToListAsync();
        }

        public async Task RemoveQuizQuestion(int quizId, int questionId)
        {
            var entity = await _context.QuizQuestions
                .FirstOrDefaultAsync(q =>
                    q.QuizId == quizId &&
                    q.QuestionId == questionId);

            if (entity == null)
                throw new KeyNotFoundException("Question not assigned to this quiz.");

            _context.QuizQuestions.Remove(entity);
     
        }
        public async Task<PaginatedResult<QuestionDto>> GetTeacherQuestionsNotInQuizAsync(int teacherId, int quizId, int? topicId,int page, int pageSize)
        {

            var query = _context.Questions
                .Where(q =>
                    q.CreatedByTeacherId == teacherId &&
                    !_context.QuizQuestions.Any(qq =>
                        qq.QuizId == quizId &&
                        qq.QuestionId == q.Id));
                

            if(topicId.HasValue)
                query = query.Where(q => q.TopicId == topicId.Value);

            var totalCount = await query.CountAsync();

            var questions = await query
                .OrderBy(q => q.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    TopicId = q.TopicId,
                    Content = q.Content,
                    Difficulty = q.Difficulty,
                    Points = q.Points,
                    TopicName = q.Topic.Name
                })
                .ToListAsync();


            return new PaginatedResult<QuestionDto>
            {
                Items = questions,
                TotalCount = totalCount,
                Page = page,
                Size = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)

            };
        }
        public async Task<int> AddRandomQuestionsToQuizAsync(int teacherId, int quizId, int? topicId, int count)
        {
            var query = _context.Questions
                .Where(q =>
                    q.CreatedByTeacherId == teacherId &&
                    !_context.QuizQuestions.Any(qq =>
                        qq.QuizId == quizId &&
                        qq.QuestionId == q.Id));

            if (topicId.HasValue)
            {
                query = query.Where(q => q.TopicId == topicId.Value);
            }

            var selectedQuestionIds = await query
                .OrderBy(q => Guid.NewGuid()) 
                .Take(count)
                .Select(q => q.Id)
                .ToListAsync();

            if (!selectedQuestionIds.Any())
                return 0;

            var quizQuestions = selectedQuestionIds.Select(qId => new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = qId
            });

            await _context.QuizQuestions.AddRangeAsync(quizQuestions);
            await _context.SaveChangesAsync();

            return selectedQuestionIds.Count;
        }

    }
}


