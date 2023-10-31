using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PRN231_CMS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_CMS
{
    public static class JwtUtils
    {
        private static readonly string jwtSecretKey = "key kieu eo gi cha dc";

        public static Student? DecodeJwt(string tokenString)
        {
            if (string.IsNullOrEmpty(tokenString)) return null;
            // Giải mã JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));

            ClaimsPrincipal claimsPrincipal;

            claimsPrincipal = tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false
            }, out _);


            // Lấy chuỗi JSON từ claim "user"
            var userJson = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "user")?.Value;

            var user = JsonConvert.DeserializeObject<Student>(userJson);

            return user;
        }

        public static string GenerateToken(string userId, string email, int expirationMinutes = 30)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
                // Add any additional claims you want to include in the token
            }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                // Token validation failed
                return null;
            }
        }
    }
}
