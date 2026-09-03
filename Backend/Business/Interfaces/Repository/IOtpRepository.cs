using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Repository
{
    public  interface IOtpRepository
    {
         Task<PasswordResetOtp?> GetOtpByUserIdAndCodeAsync(int userId, string code);
        Task<List<PasswordResetOtp?>> GetActivesOtpByUserIdAsync(int userId);
        Task RevokeActiveOtpsAsync(int userId);
        Task AddOtpAsync(PasswordResetOtp otp);
    }
}
