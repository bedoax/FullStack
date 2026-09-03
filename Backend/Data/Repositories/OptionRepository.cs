using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Option;
using Models.Entities;
namespace Data.Repositories
{
    public class OptionRepository : IOptionRepository
    {
        private  AppDbContext _context;
        public OptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddOption(OptionCreateDto option)
        {
            bool questionExists = await _context.Questions
                .AnyAsync(q => q.Id == option.QuestionId);

            if (!questionExists)
                throw new KeyNotFoundException("Question not found.");

            var newOption = new Option
            {
                QuestionId = option.QuestionId,
                Content = option.Content.Trim(),
                IsCorrect = option.IsCorrect
            };

            await _context.Options.AddAsync(newOption);

        }

        public Task DeleteOption(int optionId)
        {
            // wait for now to see if there in realLife anyNeed to delete an option from a question,
            // because it will affect the question and the quiz that contains it, so we will not implement this method for now
            throw new NotImplementedException();
        }

        public  async Task<OptionDto?> GetOptionById(int optionId)
        {
          return await _context.Options
                           .AsNoTracking()
                          .Where(o => o.Id == optionId)
                          .Select(o => new OptionDto
                          {
                              Id = o.Id,
                              QuestionId = o.QuestionId,
                              Content = o.Content,
                              IsCorrect = o.IsCorrect
                          })
                          .FirstOrDefaultAsync();
        }

        public  async Task<IEnumerable<OptionForStudentDto>> GetQuestionOptions(int questionId)
        {
            return await _context.Options
                                     .AsNoTracking()
                                     .Where(o => o.QuestionId == questionId)
                                     .Select(o => new OptionForStudentDto
                                     {
                                         Id = o.Id,
                                         Content = o.Content
                                     })
                                     .ToListAsync();
        }

        public async Task UpdateOption(OptionUpdateDto option)
        {
            var existingOption = await _context.Options
                 .FirstOrDefaultAsync(o => o.Id == option.Id);

            if (existingOption == null)
                throw new KeyNotFoundException("Option not found.");

            existingOption.Content = option.Content.Trim();
            existingOption.IsCorrect = option.IsCorrect;
        }
    }
}


