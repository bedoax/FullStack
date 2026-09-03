using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Quiz
{
    public class QuizUpdateDto
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
        public DateTime? AvailableTo { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public int? DurationInMinutes { get; set; }
        public bool? IsPublished {  get; set; }
    }
    
}
