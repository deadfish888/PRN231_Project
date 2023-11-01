using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string email, string password)
        {
            using HttpClient client = new HttpClient();
            using HttpResponseMessage res = await client.PostAsJsonAsync(link + "Students/Login", new { email = email, password = password });
            if (res.StatusCode == HttpStatusCode.NotFound)
            {
                ViewData["error"] = "true";
                return View();
            }
            using HttpContent content = res.Content;
            string data = await content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Student>(data);
            var tokenString = JObject.Parse(data)["token"].ToString();
            var userJson = JsonConvert.SerializeObject(user);

            Response.Cookies.Append("Token", tokenString, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60), // Thời gian tồn tại của cookies (60 phút trong ví dụ này)
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict // Điều này giúp bảo mật chống lại các cuộc tấn công CSRF
            });
            Response.Cookies.Append("User", userJson, new CookieOptions
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
            Response.Cookies.Delete("User");
            return Redirect("/Home");
        }
    }
}
