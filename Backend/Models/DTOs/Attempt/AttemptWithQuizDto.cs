namespace Models.DTOs.Attempt
{
    public class AttemptWithQuizDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public int? MaxAttempts { get; set; }
        public decimal? PassPercentage { get; set; }
        public int Score { get; set; }
        public decimal? Percentage { get; set; }
        public bool  Passed { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }
}
