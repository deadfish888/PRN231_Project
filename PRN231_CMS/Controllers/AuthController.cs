using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PRN231_CMS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PRN231_CMS.Controllers
{
    public class AuthController : Controller
    {
        string link = "https://localhost:7167/odata/";
        private static readonly string jwtSecretKey = "key kieu eo gi cha dc";

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string email, string password)
        {
            using HttpClient client = new HttpClient();
            using HttpResponseMessage res = await client.GetAsync(link + "Students?email=" + email.Trim()
                                                                       + "&password=" + password.Trim());
            if (res.StatusCode == HttpStatusCode.NotFound)
            {
                ViewData["error"] = "true";
                return View();
            }
            using HttpContent content = res.Content;
            string data = await content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Student>(data);
            var userJson = JsonConvert.SerializeObject(user);

            // Mã hóa chuỗi JSON thành JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(60), // Thời gian tồn tại của token (60 phút trong ví dụ này)
                signingCredentials: creds,
                claims: new[] { new Claim("user", userJson) } // Thêm claim user chứa chuỗi JSON của người dùng
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Cookies.Append("Token", tokenString, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60), // Thời gian tồn tại của cookies (60 phút trong ví dụ này)
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict // Điều này giúp bảo mật chống lại các cuộc tấn công CSRF
            });
            return Redirect("/Home");
        }

        public IActionResult Logout() 
        {
            Response.Cookies.Delete("Token");
            return Redirect("/Home");
        }
    }
}
