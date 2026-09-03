using Business.Helper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Auth;
using Models.DTOs.Google;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public  async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                await _authService.Register(request);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
           
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto  request)
        {
           var res = await _authService.Login(request);
                // add refresh token to cookie http-only
                Response.AppendRefreshTokenCookie(res.RefreshToken, res.RefreshTokenExpiresAt);

            var result = new AuthResponseDtoV
            {
                AccessToken = res.AccessToken,
                AccessTokenExpiresAt = res.AccessTokenExpiresAt,
                Email = res.Email,
                RoleName = res.RoleName,
                UserId = res.UserId,
                Username = res.Username,
                SignInByGoogle = res.SignInByGoogle
            };
            return Ok(result);
        }

        [HttpPost("google")]
        public async Task<ActionResult<AuthResponseDto>> GoogleLogin(GoogleLoginDto dto)
        {
            var res = await _authService.GoogleLogin(dto);
            // add refresh token to cookie http-only
            Response.AppendRefreshTokenCookie(res.RefreshToken, res.RefreshTokenExpiresAt);

            var result = new AuthResponseDtoV
            {
                AccessToken = res.AccessToken,
                AccessTokenExpiresAt = res.AccessTokenExpiresAt,
                Email = res.Email,
                RoleName = res.RoleName,
                UserId = res.UserId,
                Username = res.Username,
                SignInByGoogle = res.SignInByGoogle
            };
            return Ok(result);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // get it from jwt 
            var userId = User.GetUserId();
            if (userId <= 0)
                return BadRequest("Invalid user ID.");
            //get the cookie and pass it to logout to delete it from database too
            await _authService.Logout(userId);
            Response.DeleteRefreshTokenCookie();
            return NoContent();
        }
        

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            Console.WriteLine($"COOKIE RECEIVED: {refreshToken}");

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Refresh token is missing.");

            var res = await _authService.RefreshToken(refreshToken);
            Response.AppendRefreshTokenCookie(res.RefreshToken, res.RefreshTokenExpiresAt);
            var result = new AuthResponseDtoV
            {
                AccessToken = res.AccessToken,
                AccessTokenExpiresAt = res.AccessTokenExpiresAt,
                Email = res.Email,
                RoleName = res.RoleName,
                UserId = res.UserId,
                Username = res.Username
            };
            return Ok(result);
        }
        
        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

                await _authService.RequestPasswordReset(dto.Email);
            
            return Ok(new
            {
                Message = "If an account with that email exists, a password reset code has been sent."
            });
        }
        
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            await _authService.ResetPassword(request);
            return NoContent();
        }
        
        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userId = User.GetUserId();
            var requestWithUserId = new ChangePasswordDto
            {
                UserId = userId,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword
            };
            await _authService.ChangePassword(requestWithUserId);
            return NoContent();
        }
    }
}
