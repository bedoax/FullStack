using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string email, string otpCode);
    }
}
