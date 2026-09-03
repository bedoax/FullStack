using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetRefreshToken(string refreshToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<List<RefreshToken>> GetUserRefreshTokens(int userId);
        Task RevokeAllUserTokensAsync(int userId);
        Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
    }
}
