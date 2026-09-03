using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.AttemptAnswer
{
    public class CreateAttemptAnswerDto
    {
/*        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "AttemptId is required.")]
        public int AttemptId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "QuestionId is required.")]
        public int QuestionId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "SelectedOptionId is required.")]
        public int SelectedOptionId { get; set; }*/


        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
        public bool IsCorrect { get; set; }
        public int EarnedPoints { get; set; }
    }
}
