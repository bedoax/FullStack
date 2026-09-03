using Models.DTOs.AttemptAnswer;
using Models.Entities;

namespace Business.Interfaces.Repository
{
    public interface IAttemptAnswersRepository
    {
        Task<IEnumerable<AttemptAnswerDto>>
            GetAttemptAnswers(
                int attemptId);

        Task<AttemptAnswerDto>
            GetAttemptAnswerById(
                int id);
        void Add(AttemptAnswer entity);
        Task AddAttemptAnswer(CreateAttemptAnswerDto attemptAnswer);
        Task AddRangeAsync(IEnumerable<AttemptAnswer> answers);

    }
}
