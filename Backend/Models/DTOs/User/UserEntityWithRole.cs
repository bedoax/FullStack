namespace Models.DTOs.User
{
    public class UserEntityWithRole
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int RoleId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
