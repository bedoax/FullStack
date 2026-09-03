namespace Models.DTOs.StudentTopicPerformance
{
    public class UpdateStudentTopicPerformanceDtoForService
    {
        public int UserId { get; set; }

        public int TopicId { get; set; }

        public int CorrectAnswers { get; set; }

        public int WrongAnswers { get; set; }
    }
}
