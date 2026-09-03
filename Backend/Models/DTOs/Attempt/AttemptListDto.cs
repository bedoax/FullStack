namespace Models.DTOs.Attempt
{
    public class AttemptListDto
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;

        public int AttemptNumber { get; set; }

        public decimal? Score { get; set; }

        public decimal? Percentage { get; set; }

        public bool? Passed { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }

    }
}
