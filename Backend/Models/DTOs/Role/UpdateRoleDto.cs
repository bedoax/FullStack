using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Role
{
    public class UpdateRoleDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
    }
}
