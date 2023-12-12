using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PRN231_CMS.Models;
using System.Text.Json.Nodes;
using System.Net;
using Humanizer;

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
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

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
            var encodedSearch = Uri.EscapeDataString(search.Trim());
            var queryParams = new Dictionary<string, string>
{
    { "$filter", $"contains(CourseName, '{encodedSearch}')" },
    { "$expand", "Category" }
};

            var queryBuilder = new System.Text.StringBuilder("Courses?");
            foreach (var kvp in queryParams)
            {
                queryBuilder.Append($"{kvp.Key}={kvp.Value}&");
            }

            var queryString = queryBuilder.ToString().TrimEnd('&');

            using HttpResponseMessage res = await client.GetAsync(link + queryString);
            if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["msg"] = "Bad Request";
                return View();
            }
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
            using HttpResponseMessage res = await client.PostAsJsonAsync(link + "CourseStudents", new CourseStudent()
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

        public async Task<IActionResult> AssignAsync(int id)
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
            using HttpResponseMessage res = await client.GetAsync(link + "Assignments(" + id + ")?$expand=Item($expand=Section)");
            if (!res.IsSuccessStatusCode)
            {
                return Redirect("/Home");
            }
            using HttpContent content = res.Content;

            string data = await content.ReadAsStringAsync();
            var assign = JsonConvert.DeserializeObject<Assignment>(data);
            ViewBag.assign = assign;

            using HttpResponseMessage res2 = await client.GetAsync(link + "Courses(" + assign?.Item?.Section?.CourseId + ")");
            using HttpContent content2 = res2.Content;
            data = await content2.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<Course>(data);
            ViewBag.course = course;

            using HttpResponseMessage res3 = await client.GetAsync(link + "StudentSubmissions(" + s.UserId + ", " + assign.AssignmentId + ")");
            using HttpContent content3 = res3.Content;
            if (res3.StatusCode == HttpStatusCode.NoContent)
            {
                return View();
            }
            data = await content3.ReadAsStringAsync();
            var ss = JsonConvert.DeserializeObject<StudentSubmission>(data);
            ViewData["studentSubmission"] = ss;
            return View();
        }

        public async Task<IActionResult> ResourceAsync(int id)
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
            using HttpResponseMessage res = await client.GetAsync(link + "Resources(" + id + ")?$expand=Item");
            using HttpContent content = res.Content;
            if (res.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            var data = await content.ReadAsStringAsync();
            var ss = JsonConvert.DeserializeObject<Resource>(data);
            return File(ss.Data, "application/octet-stream", ss.Item.ItemName);
        }
    }
}
