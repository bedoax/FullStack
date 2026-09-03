using System.Security.Claims;


namespace Business.Helper
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // الـ GlobalExceptionHandlingMiddleware بتاعنا هيمسك دي ويرجع 401
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User identification claim is missing or invalid.");
            }


            if (!int.TryParse(userIdClaim, out var userId))
            {
                // لو الـ ID في التوكن مش رقم (مثلاً GUID)، بنرمي Exception تانية
                throw new UnauthorizedAccessException("User ID in claims is not a valid integer.");
            }

            return userId;
        }
        public static string GetUserRole(this ClaimsPrincipal user)
        {

            var userRoleCaim = user.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRoleCaim))
                throw new UnauthorizedAccessException("User Role Claim is missing or invalid");
            return userRoleCaim;
        }
    }
}
