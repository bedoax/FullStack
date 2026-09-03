using Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Auth;
using Models.DTOs.User;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Business.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        { 
            _configuration = configuration;
        }
        public GenerateTokenDto GenerateAccessToken(UserEntityWithRole user)
        {
            var claims = new List<Claim>
            {
                // see if you want to fetch the name of role id from userEntity when log in or use extra request
                new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString() ),
                new Claim(ClaimTypes.Name , user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role , user.RoleName),
                new Claim("id", user.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expire = DateTime.UtcNow.AddDays(2);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expire,
                SigningCredentials = creds,
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };
            var tokenHandler = new JsonWebTokenHandler();
            var genToken = new GenerateTokenDto
            {
                Token = tokenHandler.CreateToken(tokenDescriptor),
                ExpiresAt = expire
            };
            return genToken; 
        }

        public string GenerateRefreshToken()
        {
            // ensure that the refresh token is exist and not expired , then create  a new one and revoked the old one and return the new one ?
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
