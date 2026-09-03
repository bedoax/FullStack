using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Question
{
    public class QuestionCreateDto
    {
        [Required]

        public int TopicId { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        public string? Difficulty { get; set; }
        public int? Points { get; set; }
    }
}
