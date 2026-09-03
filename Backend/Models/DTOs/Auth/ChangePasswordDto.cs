using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
