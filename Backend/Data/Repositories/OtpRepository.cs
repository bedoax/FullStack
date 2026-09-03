using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private AppDbContext _context;
        public OtpRepository(AppDbContext context)

        {
            _context = context;
        }
        public Task<PasswordResetOtp?> GetOtpByUserIdAndCodeAsync(int userId, string code)
        {
            return _context.PasswordResetOtps
             .FirstOrDefaultAsync(x =>
           x.UserId == userId &&
           x.Code == code &&
           !x.IsUsed &&
            x.UsedAt == null &&
           x.ExpiresAt > DateTime.UtcNow);
        }
        public  Task<List<PasswordResetOtp?>> GetActivesOtpByUserIdAsync(int userId)
        {
            return  _context.PasswordResetOtps
             .Where(x =>
           x.UserId == userId &&
           !x.IsUsed &&
           x.UsedAt == null &&
           x.ExpiresAt > DateTime.UtcNow).ToListAsync();
        }
        public async Task AddOtpAsync(PasswordResetOtp otp)
        {
           await _context.PasswordResetOtps.AddAsync(otp);
          
        }
        public async Task RevokeActiveOtpsAsync(int userId)
        {
            var activeOtps = await _context.PasswordResetOtps
                .Where(x =>
                    x.UserId == userId &&
                    !x.IsUsed &&
                    x.UsedAt == null &&
                    x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
                foreach (var otp in activeOtps)
                {
                    otp.IsUsed = true;
                    otp.UsedAt = DateTime.UtcNow;
                }

        }
    }
}
