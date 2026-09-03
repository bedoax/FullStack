using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Auth
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }

        [Required]
        public string OTP { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
