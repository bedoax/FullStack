namespace Models.DTOs.StudentTopicPerformance
{
    public class StudentTopicPerformanceDto
    {
        public int UserId { get; set; }
        public string TopicName { get; set; }

        public int TopicId { get; set; }

        public int? CorrectAnswers { get; set; }

        public int? WrongAnswers { get; set; }
        
        public decimal? SuccessRate { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
