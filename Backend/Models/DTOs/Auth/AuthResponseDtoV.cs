using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Auth
{
    public class AuthResponseDtoV
    {
        public string AccessToken { get; set; } = null!;
        public DateTime AccessTokenExpiresAt { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool SignInByGoogle { get; set; } = false;

    }
}
