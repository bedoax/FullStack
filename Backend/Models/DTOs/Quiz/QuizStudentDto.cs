

namespace Models.DTOs.Quiz
{
    public class QuizStudentDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public decimal? Score { get; set; }

        public bool? Passed { get; set; }
    }
}
