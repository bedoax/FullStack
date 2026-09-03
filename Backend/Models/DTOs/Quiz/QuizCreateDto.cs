using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Quiz
{
    public class QuizCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 100)]
        public int? MaxAttempts { get; set; }

        [Range(0, 100)]
        public decimal? PassPercentage { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public int DurationInMinutes { get; set; } = 60; // Duration in minutes, 0 means no time limit
    }
    
}
