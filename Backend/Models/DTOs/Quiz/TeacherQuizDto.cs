namespace Models.DTOs.Quiz
{
    public class TeacherQuizDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? MaxAttempts { get; set; }

        public decimal? PassPercentage { get; set; }
        public bool IsPublished { get; set; } = false;

        public DateTime? CreatedAt { get; set; }
        public int CreatedByTeacherId { get; set; }
        public int DurationMinutes { get; set; }
        // nullable for when make it draft
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
    }
}
