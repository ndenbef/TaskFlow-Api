using System.IdentityModel.Tokens.Jwt;

namespace TaskFlow.Security.Shared
{
    public static class JwtHelper
    {
        public static string? GetUserIdFromToken(string token)
        {
            var tokenWithoutBearerKeyword = token.Replace("Bearer ","");

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(tokenWithoutBearerKeyword);

            var userIdClaims = jwtToken.Claims.FirstOrDefault(options =>
            options.Type == "sub");

            return userIdClaims?.Value.ToString();

        }
    }
}
