using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Attempt
{
    public class CreateAttemptDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]

        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "QuizId must be greater than 0")]

        public int QuizId { get; set; }

    }
}
