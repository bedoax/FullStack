using Business.Helper;
using Business.Interfaces;
using Business.Interfaces.Repository;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Models.DTOs.Auth;
using Models.DTOs.Google;
using Models.DTOs.StudentProfile;
using Models.DTOs.User;
using Models.Entities;
using System.Security.Cryptography;


namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private IUnitOfWork _unitOfWork;
        private IPasswordService _passwordService;
        private ITokenService _tokenService;
        private IRefreshTokenService _refreshTokenService;
        private IEmailService _emailService;
        private IConfiguration _configuration;
        public AuthService(IUnitOfWork unitOfWork, IPasswordService passwordService,ITokenService tokenService, IRefreshTokenService refreshTokenService, IEmailService emailService,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task ChangePassword(ChangePasswordDto dto)
        {
            var user = await _unitOfWork.Users.GetUserEntityById(dto.UserId);

            if (user == null)
                throw new KeyNotFoundException();

            bool isValid = _passwordService.VerifyPassword(
                dto.OldPassword,
                user.Password
                );
            if(!isValid)
                throw new UnauthorizedAccessException("Old password is incorrect.");

            var hashedPassword = _passwordService.HashPassword(dto.NewPassword);
            await _unitOfWork.Users.ChangePassword(dto.UserId,hashedPassword);
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var user = await _unitOfWork.Users.GetUserEntityByUsername(dto.Username);
            if (user == null)
                throw new UnauthorizedAccessException(
                    "username or password is incorrect.");

            bool isValid = _passwordService.VerifyPassword(
                                             dto.Password,
                                             user.Password);
            if (!isValid)
                throw new UnauthorizedAccessException("username or password is incorrect.");
            // make the acssess token 
            var Gentoken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.RefreshTokenRepository.AddRefreshTokenAsync(
                new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = refreshTokenExpiresAt,
                    CreatedAt = DateTime.UtcNow
                });

            await _unitOfWork.SaveChangesAsync();

            var result = new AuthResponseDto
            {
                AccessToken = Gentoken.Token,
                AccessTokenExpiresAt = Gentoken.ExpiresAt,
                Email = user.Email,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
                RoleName = user.RoleName,
                UserId = user.Id,
                Username = user.Username,
                SignInByGoogle = false
            };
            return result;
        }
        public async Task<AuthResponseDto> GoogleLogin(GoogleLoginDto dto)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken,new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[]
                    {
                _configuration["Google:ClientId"]
                    }
                });

            var googleId = payload.Subject;
            var email = payload.Email;
            var username = payload.Name;

            var user = await _unitOfWork.Users
                            .GetUserByGoogleIdAsync(googleId);

            if (user == null)
            {
                user = await _unitOfWork.Users
                    .GetUserEntityByEmail(email);

                if (user != null)
                {
                    user.GoogleId = googleId;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            if (user == null)
            {
                var roleName = RolesConstants.Student;
                var role = await _unitOfWork.Roles.GetRoleByName(roleName);
                var randomNumber = RandomNumberGenerator.GetInt32(10000, 100000);

                var newUsername =
                    $"{username}_{email[..3]}_{randomNumber}";
                var newUser = new UserCreateDto
                {
                    Username = newUsername,
                    Email = email,
                    GoogleId = googleId,
                    RoleId = role.Id
                };
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    user = await _unitOfWork.Users.AddUserAsync(newUser);
                    await _unitOfWork.SaveChangesAsync();
                    var studentProfileDto = new CreateStudentProfileDto
                    {
                        UserId = user.Id,
                        TotalAttempts = 0

                    };
                    await _unitOfWork.StudentProfiles.AddStudentProfile(studentProfileDto);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }

            }
            var userWithRole = new UserEntityWithRole
            {
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                Id = user.Id,
                Password = user.Password,
                RoleId = user.RoleId,
                RoleName = RolesConstants.Student,
                Username = user.Username
            };
            var Gentoken = _tokenService.GenerateAccessToken(userWithRole);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.RefreshTokenRepository.AddRefreshTokenAsync(new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = refreshTokenExpiresAt,
                    CreatedAt = DateTime.UtcNow
                });

            await _unitOfWork.SaveChangesAsync();

            var result = new AuthResponseDto
            {
                AccessToken = Gentoken.Token,
                AccessTokenExpiresAt = Gentoken.ExpiresAt,
                Email = userWithRole.Email,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
                RoleName = userWithRole.RoleName,
                UserId = userWithRole.Id,
                Username = userWithRole.Username,
                SignInByGoogle = true
            };
            return result;


        }
        public async Task Logout(int userId)
        {
            // first we need to make table for refresh token which have userid and id and refreshtoken hashed and revoked boolen or revoked at and created at
            // remove the acssess token and refresh
            var refreshTokens =  await _unitOfWork.RefreshTokenRepository.GetUserRefreshTokens(userId);

            foreach (var token in refreshTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();
        }
       
        public Task<AuthResponseDto> RefreshToken(string refreshToken)
        {
            return _refreshTokenService.RefreshTokenAsync(refreshToken);
        }

        public async Task Register(RegisterDto registerDto)
        {
            var user = new UserCreateDto
            {
                Username = registerDto.Username,
                Password = _passwordService.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                RoleId = 2
            };
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createdUser = await _unitOfWork.Users.AddUserAsync(user);
                await _unitOfWork.SaveChangesAsync();
                var studentProfileDto = new CreateStudentProfileDto
                {
                    UserId = createdUser.Id,
                    TotalAttempts = 0

                };
                await _unitOfWork.StudentProfiles.AddStudentProfile(studentProfileDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }
        public async Task RequestPasswordReset(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            if (user == null)
                // for securty rather than using throw new KeyNotFoundException("User with the provided email does not exist.");
                return;
            
            string otpCode = GenerateOtpCode();

            var passwordResetOtp = new PasswordResetOtp
            {
                UserId = user.Id,
                Code = otpCode,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                CreatedAt = DateTime.UtcNow,
                IsUsed = false,
                UsedAt = null
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {

                await _unitOfWork.OtpRepository.RevokeActiveOtpsAsync(user.Id);
                
                await _unitOfWork.OtpRepository.AddOtpAsync(passwordResetOtp);
                
                await _unitOfWork.SaveChangesAsync();
                
                await _unitOfWork.CommitAsync();
                
                await _emailService.SendOtpAsync(email, otpCode);

            }
            catch
            {
                await _unitOfWork.RollbackAsync(); 
                throw;
            }
        }
        public async Task ResetPassword(ResetPasswordDto dto)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(dto.Email);
            
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var respond = await  _unitOfWork.OtpRepository.GetOtpByUserIdAndCodeAsync(user.Id, dto.OTP);
            if (respond == null)
                throw new InvalidOperationException("Invalid or expired OTP.");

            var newPassword =   _passwordService.HashPassword(dto.NewPassword);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.Users.ChangePassword(user.Id, newPassword);

                respond.IsUsed = true;
                respond.UsedAt = DateTime.UtcNow;
                await _unitOfWork.RefreshTokenRepository.RevokeAllUserTokensAsync(user.Id);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        private string GenerateOtpCode()
        {
            return RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();
        }
    }
}
