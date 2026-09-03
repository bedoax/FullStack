namespace Models.DTOs.Question
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? Difficulty { get; set; }

        public int? Points { get; set; }
    }
}
