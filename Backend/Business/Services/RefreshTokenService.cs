using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.Auth;
using Models.DTOs.User;
using Models.Entities;

namespace Business.Services
{
    public class RefreshTokenService: IRefreshTokenService
    {
        private IUnitOfWork _unitOfWork;
        private ITokenService _tokenService;
        public RefreshTokenService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException("Refresh token is required.");

            var refreshTokenEntity =
                await _unitOfWork.RefreshTokenRepository
                    .GetRefreshToken(refreshToken);

            if (refreshTokenEntity == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            // Reuse Attack Detection
            if (refreshTokenEntity.IsRevoked)
            {
                if (refreshTokenEntity.RevokedAt.HasValue &&
                    refreshTokenEntity.RevokedAt.Value.AddSeconds(30) > DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException(
                        "Token was recently refreshed. Please retry.");
                }

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    await RevokeAllUserTokensAsync(refreshTokenEntity.UserId);

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }


                throw new UnauthorizedAccessException(
                    "Attempted reuse of revoked refresh token. Security breach flagged.");
            }

            if (refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token is expired.");

            var user = MapToUserEntityWithRole(refreshTokenEntity.User);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                RevokeRefreshToken(refreshTokenEntity);

                var newRefreshTokenEntity =
                    CreateRefreshToken(
                        refreshTokenEntity.UserId,
                        newRefreshToken);

                await _unitOfWork.RefreshTokenRepository
                    .AddRefreshTokenAsync(newRefreshTokenEntity);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return MapToAuthResponseDto(
                    accessToken,
                    newRefreshToken,
                    newRefreshTokenEntity.ExpiresAt,
                    user);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private RefreshToken CreateRefreshToken(int userId,string token)
        {
            return new RefreshToken
            {
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
        }
        private UserEntityWithRole MapToUserEntityWithRole(User user)
        {
            return new UserEntityWithRole
            {
                Id = user.Id,
                Email = user.Email,
                RoleName = user.Role.Name,
                RoleId = user.RoleId,
                Username = user.Username
            };
        }
        private AuthResponseDto MapToAuthResponseDto(GenerateTokenDto accessToken, string refreshToken, DateTime refreshTokenExpiresAt, UserEntityWithRole user)
        {
            return new AuthResponseDto
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
                UserId = user.Id,
                Email = user.Email,
                RoleName = user.RoleName,
                Username = user.Username
            };
        }
        private void RevokeRefreshToken(RefreshToken refreshTokenEntity)
        {
            refreshTokenEntity.IsRevoked = true;
            refreshTokenEntity.RevokedAt = DateTime.UtcNow;
        }
        private async Task RevokeAllUserTokensAsync(int userId)
        {
            var activeTokens = await _unitOfWork.RefreshTokenRepository.GetActiveTokensByUserIdAsync(userId);
            foreach (var token in activeTokens)
            {
                RevokeRefreshToken(token);
            }

        }
    }
}
