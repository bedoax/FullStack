namespace Models.DTOs.Option
{
    public class OptionCreateDto
    {
        public int QuestionId { get; set; }
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
