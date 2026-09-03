namespace Models.DTOs.QuizQuestionDto
{
    public class CreateQuizQuestionsDto
    {
        public int QuizId { get; set; }
        public List<int> QuestionIds { get; set; } = new List<int>();
    }
}
