using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.User
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [MinLength(8)]
        public string? Password { get; set; }

        public int RoleId { get; set; } = 2; // student by default
        public string? GoogleId { get; set; }
    }
}
