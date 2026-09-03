namespace Models.DTOs.Option
{
    public class OptionDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Content { get; set; } = null!;

        public bool IsCorrect { get; set; }
    }
}
