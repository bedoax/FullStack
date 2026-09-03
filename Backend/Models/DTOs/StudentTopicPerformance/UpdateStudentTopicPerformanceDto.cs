using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.StudentTopicPerformance
{
    public class UpdateStudentTopicPerformanceDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TopicId { get; set; }
        [Required]
        public int CorrectAnswers { get; set; }
        [Required]
        public int  WrongAnswers { get; set; }
        [Required]
        public decimal  SuccessRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
