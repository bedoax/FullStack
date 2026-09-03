using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.StudentTopicPerformance
{
    public class CreateStudentTopicPerformanceDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Topic ID must be a positive integer.")]
        public int TopicId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Correct answers must be a non-negative integer.")]

        public int CorrectAnswers { get; set; } = 0;
        [Range(0, int.MaxValue, ErrorMessage = "Wrong answers must be a non-negative integer.")]

        public int WrongAnswers { get; set; } = 0;
        public decimal SuccessRate { get; set; }
    }
}
