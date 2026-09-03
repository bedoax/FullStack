using Models.DTOs.Auth;
using Models.DTOs.User;


namespace Business.Interfaces
{
    public interface ITokenService
    {
        GenerateTokenDto GenerateAccessToken(UserEntityWithRole user);
        string GenerateRefreshToken();
    }


}
