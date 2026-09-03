using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class PasswordResetOtp
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Code { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UsedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public User User { get; set; } = null!;
    }
}
