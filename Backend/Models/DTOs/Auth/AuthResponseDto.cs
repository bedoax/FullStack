using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool SignInByGoogle { get; set; } = false;
    }
}
