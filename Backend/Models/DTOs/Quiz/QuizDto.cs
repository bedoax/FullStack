namespace Models.DTOs.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? MaxAttempts { get; set; }
        public decimal? PassPercentage { get; set; }
        public int DurationMinutes { get; set; }          
        public DateTime? AvailableFrom { get; set; }      
        public DateTime? AvailableTo { get; set; }       
        public DateTime? CreatedAt { get; set; }
    }
}