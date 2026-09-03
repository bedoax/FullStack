using Models.DTOs.Option;
namespace Models.DTOs.Question
{
    public class QuestionDetailsDtoForStudent
    {
        //Question details with options to let student choose the answers
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Content { get; set; } = null!;
        public string? Difficulty { get; set; }
        public int? Points { get; set; }
        public IEnumerable<OptionForStudentDto> Options { get; set; } = new List<OptionForStudentDto>();
    }
}
