using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN231_CMS.Models;

namespace PRN231_CMS.Controllers
{
    public class ProfileController : Controller
    {
        string link = "https://localhost:7167/odata/";
        public IActionResult Index()
        {
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                return Redirect("/Auth/Login");
            }
            var userString = HttpContext.Request.Cookies["User"];
            Student s = JsonConvert.DeserializeObject<Student>(userString);
            ViewBag.user = s;
            return View();
        }

        public async Task<IActionResult> EditAsync()
        {
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                return Redirect("/Auth/Login");
            }
            var userString = HttpContext.Request.Cookies["User"];
            Student s = JsonConvert.DeserializeObject<Student>(userString);
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

            using HttpResponseMessage res = await client.GetAsync(link + "Students(" + s.UserId + ")");
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Redirect("/Auth/Login");
            }
            using HttpContent content = res.Content;
            string data = await content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<Student>(data);
            ViewBag.user = student;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(Student student)
        {
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                return Redirect("/Auth/Login");
            }
            var userString = HttpContext.Request.Cookies["User"];
            Student s = JsonConvert.DeserializeObject<Student>(userString);
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

            using HttpResponseMessage res = await client.PutAsJsonAsync(link + "Students(" + s.UserId + ")", student);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Redirect("/Auth/Login");
            }
            using HttpContent content = res.Content;
            string data = await content.ReadAsStringAsync();
            var newS = JsonConvert.DeserializeObject<Student>(data);
            var userJson = JsonConvert.SerializeObject(newS);
            Response.Cookies.Append("User", userJson, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60), // Thời gian tồn tại của cookies (60 phút trong ví dụ này)
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict // Điều này giúp bảo mật chống lại các cuộc tấn công CSRF
            });
            return Redirect("/Profile/Edit");
        }
    }
}
