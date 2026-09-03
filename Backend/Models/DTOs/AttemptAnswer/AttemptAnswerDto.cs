namespace Models.DTOs.AttemptAnswer
{
    public class AttemptAnswerDto
    {
        public int Id { get; set; }

        public int AttemptId { get; set; }

        public int QuestionId { get; set; }

        public int? SelectedOptionId { get; set; }

        public bool? IsCorrect { get; set; }

        public int? EarnedPoints { get; set; }
    }
}
