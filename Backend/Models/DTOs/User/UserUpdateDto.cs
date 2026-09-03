using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
