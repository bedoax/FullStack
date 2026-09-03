using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Question
{
    public class QuestionUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        [Required]
        public int TopicId { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        public string? Difficulty { get; set; }
        public int? Points { get; set; }
    }
}
