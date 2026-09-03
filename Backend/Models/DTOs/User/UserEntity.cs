using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User
{
    public  class UserEntity
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int RoleId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string Password { get; set; }
    }
}
