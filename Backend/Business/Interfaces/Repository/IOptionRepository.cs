using Models.DTOs.Option;

namespace Business.Interfaces.Repository
{
    public interface IOptionRepository
    {
        Task<OptionDto> GetOptionById(
            int optionId);

        Task<IEnumerable<OptionForStudentDto>> GetQuestionOptions(
            int questionId);

        Task AddOption(
            OptionCreateDto option);

        Task UpdateOption(
            OptionUpdateDto option);

        Task DeleteOption(
            int optionId);
    }
}
