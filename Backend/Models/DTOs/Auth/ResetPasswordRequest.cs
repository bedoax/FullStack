using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string OTP { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
