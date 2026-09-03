using Microsoft.AspNetCore.Http;

namespace Business.Helper
{
    public static class RefreshTokenInCookieExtensions
    {
        public static void AppendRefreshTokenCookie(this HttpResponse response, string refreshToken, DateTime expiresAt)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiresAt,
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public static void DeleteRefreshTokenCookie(this HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,

            };

            response.Cookies.Delete("refreshToken", cookieOptions);
        }
    }
}