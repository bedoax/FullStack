using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    
    public  class RefreshTokenRepository : IRefreshTokenRepository
    {
        private AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            return  await _context.RefreshTokens
                 .Include(rt => rt.User)
                 .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
           
        }

        public Task<List<RefreshToken>> GetUserRefreshTokens(int userId)
        {
            return _context.RefreshTokens
                .Where(rt => rt.UserId == userId).ToListAsync();
                
        }
        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var userTokens = await GetUserRefreshTokens(userId);
            foreach (var token in userTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }
      
        }
        public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
