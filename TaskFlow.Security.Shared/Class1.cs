using System.IdentityModel.Tokens.Jwt;

namespace TaskFlow.Security.Shared
{
    public static class JwtHelper
    {
        public static string? GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaims = jwtToken.Claims.FirstOrDefault(options =>
            options.Type == "Id");

            return userIdClaims?.Value.ToString();

        }
    }
}
