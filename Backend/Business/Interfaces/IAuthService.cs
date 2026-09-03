using Models.DTOs.Auth;
using Models.DTOs.Google;


namespace Business.Interfaces
{
    public interface IAuthService 
    {
        /*
            Register
            Login
            Logout
            Refresh Token
            Change Password
            Forgot Password
            Reset Password
         */
        Task Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto dto);
        Task<AuthResponseDto> GoogleLogin(GoogleLoginDto dto);
        Task Logout(int userId);

        Task<AuthResponseDto> RefreshToken(string refreshToken);

        Task ChangePassword(ChangePasswordDto dto);

        Task RequestPasswordReset(string email);

        Task ResetPassword(ResetPasswordDto dto);
    }


}
