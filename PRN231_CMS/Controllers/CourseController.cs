using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PRN231_CMS.Models;
using System.Text.Json.Nodes;
using System.Net;

namespace PRN231_CMS.Controllers
{
    public class CourseController : Controller
    {
        string link = "https://localhost:7167/odata/";
        public async Task<IActionResult> IndexAsync()
        {
            using HttpClient client = new HttpClient();
            using HttpResponseMessage res = await client.GetAsync(link + "Categories?$expand=Courses");
            using HttpContent content = res.Content;
            string data = await content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(JObject.Parse(data)["value"].ToString());
            ViewBag.categories = categories;
            return View();
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                return Redirect("/Auth/Login");
            }
            var userString = HttpContext.Request.Cookies["User"];
            Student s = JsonConvert.DeserializeObject<Student>(userString);
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer "+tokenString);

            using HttpResponseMessage res = await client.GetAsync(link + "Courses(" + id + ")?$expand=Sections($expand=Items($expand=Resource,Assignment)),CourseStudents");
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Redirect("/Auth/Login");
            }
            using HttpContent content = res.Content;
            string data = await content.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<Course>(data);
            if (!course.CourseStudents.Any(cs => cs.UserId == s.UserId))
            {
                return Redirect("/Course/Enroll?id=" + id);
            }
            ViewBag.currentCourse = course;
            return View();
        }

        public async Task<IActionResult> SearchAsync()
        {
            string search = Request.Query["search"];
            if (string.IsNullOrEmpty(search))
            {
                ViewData["search"] = "";
                ViewBag.results = null;
                return View();
            }
            using HttpClient client = new HttpClient();
            using HttpResponseMessage res = await client.GetAsync(link + "Courses?$filter=contains(CourseName, '"+search.Trim()+"')&$expand=Category");
            using HttpContent content = res.Content;

            string data = await content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<Course>>(JObject.Parse(data)["value"].ToString());
            ViewBag.search = search;
            ViewBag.results = results;
            return View();
        }

        public async Task<IActionResult> EnrollAsync(int id)
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

            using HttpResponseMessage res = await client.GetAsync(link + "Courses(" + id + ")?$expand=Category,CourseStudents");
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/Auth/Login");
            }
            using HttpContent content = res.Content;

            string data = await content.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<Course>(data);
            if (course.CourseStudents.Any(cs => cs.UserId == s.UserId))
            {
                return Redirect("/Course/Details?id=" + id);
            }
            ViewBag.user = s;
            ViewBag.enrollCourse = course;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnrollAsync(int userId, int courseId)
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
            using HttpResponseMessage res = await client.PostAsJsonAsync(link+"CourseStudents", new CourseStudent()
            {
                UserId = userId,
                CourseId = courseId
            });
            if (res.IsSuccessStatusCode)
            {
                return Redirect("/Course/Details?id=" + courseId);
            }
            return Redirect("/Auth/Login"); ;
        }

        public async Task<IActionResult> UnenrollAsync(int id)
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
            using HttpResponseMessage res = await client.DeleteAsync(link + "CourseStudents(" + s.UserId + "," + id + ")");
            if (res.IsSuccessStatusCode)
            {
                return Redirect("/Home");
            }
            return View();
        }
    }
}
