using Models.DTOs.Option;
namespace Models.DTOs.Question
{
    public class QuestionDetailsDto
    {
        //Question details with options to let student choose the answers
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Content { get; set; } = null!;
        public string? Difficulty { get; set; }
        public int? Points { get; set; }
        public IEnumerable<OptionDto> Options { get; set; } = new List<OptionDto>();
    }
}
