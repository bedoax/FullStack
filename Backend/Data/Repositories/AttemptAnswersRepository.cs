using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.AttemptAnswer;
using Models.Entities;
namespace Data.Repositories
{
    public class AttemptAnswersRepository : IAttemptAnswersRepository
    {
        private AppDbContext _context;
        public AttemptAnswersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAttemptAnswer(CreateAttemptAnswerDto dto)
        {
            /*            var attempt = await _context.Attempts
                            .FirstOrDefaultAsync(a => a.Id == dto.AttemptId);

                        if (attempt is null)
                            throw new KeyNotFoundException("Attempt not found.");

                        var question = await _context.Questions
                            .Include(q => q.Options)
                            .FirstOrDefaultAsync(q => q.Id == dto.QuestionId);

                        if (question is null)
                            throw new KeyNotFoundException("Question not found.");

                        var selectedOption = question.Options
                            .FirstOrDefault(o => o.Id == dto.SelectedOptionId);

                        if (selectedOption is null)
                            throw new KeyNotFoundException("Selected option not found.");

                        bool isCorrect = selectedOption.IsCorrect;
                        int earnedPoints = isCorrect ? (int)question.Points : 0;

                        var newAttemptAnswer = new AttemptAnswer
                        {
                            AttemptId = dto.AttemptId,
                            QuestionId = dto.QuestionId,
                            SelectedOptionId = dto.SelectedOptionId,
                            IsCorrect = isCorrect,
                            EarnedPoints = earnedPoints
                        };
                        _context.AttemptAnswers.Add(newAttemptAnswer);*/

            _context.AttemptAnswers.Add(new AttemptAnswer
            {
                AttemptId = dto.AttemptId,
                QuestionId = dto.QuestionId,
                SelectedOptionId = dto.SelectedOptionId,
                IsCorrect = dto.IsCorrect,
                EarnedPoints = dto.EarnedPoints
            });

        }

        public async Task<AttemptAnswerDto?> GetAttemptAnswerById(int id)
        {
                      return await _context.AttemptAnswers
                             .AsNoTracking()
                           .Where(a => a.Id == id)
                           .Select(a => new AttemptAnswerDto
                           {
                               Id = a.Id,
                               AttemptId = a.AttemptId,
                               QuestionId = a.QuestionId,
                               SelectedOptionId = a.SelectedOptionId,
                               IsCorrect = a.IsCorrect,
                               EarnedPoints = a.EarnedPoints
                           })
                           .FirstOrDefaultAsync();
        }
        public void Add(AttemptAnswer entity)
        {
            _context.AttemptAnswers.Add(entity);
        }
        public async Task GetMyAttemptAnswer(int userId, int attemptId)
        {
            //await _context.AttemptAnswers.Where(a => a.AttemptId == attemptId && a.Attempt.UserId == userId).Select().ToListAsync();
        }
        public async Task<IEnumerable<AttemptAnswerDto>> GetAttemptAnswers(int attemptId)
        {
            return await _context.AttemptAnswers
                 .AsNoTracking()
                 .Where(a => a.AttemptId == attemptId)
                 .Select(a => new AttemptAnswerDto
                 {
                     Id = a.Id,
                     AttemptId = a.AttemptId,
                     QuestionId = a.QuestionId,
                     SelectedOptionId = a.SelectedOptionId,
                     IsCorrect = a.IsCorrect,
                     EarnedPoints = a.EarnedPoints
                 }).ToListAsync();
        }
        public async Task AddRangeAsync(IEnumerable<AttemptAnswer> answers)
        {
            await _context.AttemptAnswers.AddRangeAsync(answers);
        }
    }
}


