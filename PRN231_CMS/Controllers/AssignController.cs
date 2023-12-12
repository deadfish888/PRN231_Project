using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN231_CMS.Models;
using System.Net;

namespace PRN231_CMS.Controllers
{
    public class AssignController : Controller
    {
        string link = "https://localhost:7167/odata/";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EditAsync(int id, string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                ViewData["er"] = "true";
            }
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

        [HttpPost]
        public async Task<IActionResult> EditAsync(IFormFile file, int assignId, int patch)
        {
            if (file == null)
            {
                return Redirect("/Assign/Edit?id="+assignId+"&msg=false");
            }
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                return Redirect("/Auth/Login");
            }
            var userString = HttpContext.Request.Cookies["User"];
            Student s = JsonConvert.DeserializeObject<Student>(userString);
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();

                    // Save the file data to the database
                    var uploadedFile = new StudentSubmission
                    {
                        UserId = s.UserId,
                        AssignmentId = assignId,
                        Status = true,
                        LastModified = DateTime.Now,
                        FileName = file.FileName,
                        FileData = fileData
                    };
                    using HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                    using HttpResponseMessage res = await client.PostAsJsonAsync(link + "StudentSubmissions", uploadedFile);
                    if (res.StatusCode == HttpStatusCode.Conflict)
                    {
                        return Redirect("/Assign/Edit?id=" + assignId);
                    }
                    return Redirect("/Course/Assign?id=" + assignId);
                }
            }
            return NoContent();
        }

        public async Task<IActionResult> FileAsync(int id)
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
            using HttpResponseMessage res = await client.GetAsync(link + "StudentSubmissions(" + s.UserId + ", " + id + ")");
            using HttpContent content = res.Content;
            if (res.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            var data = await content.ReadAsStringAsync();
            var ss = JsonConvert.DeserializeObject<StudentSubmission>(data);
            return File(ss.FileData, "application/octet-stream", ss.FileName);
        }

        public IActionResult res()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> resAsync(IFormFile file, int itemId)
        {
            if (file == null)
            {
                return Redirect("/Home");
            }
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();

                    // Save the file data to the database
                    var uploadedFile = new Resource
                    {
                        ItemId = itemId,
                        Data = fileData
                    };
                    using HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                    using HttpResponseMessage res = await client.PostAsJsonAsync(link + "Resources", uploadedFile);
                    if (res.StatusCode == HttpStatusCode.Conflict)
                    {
                        return Redirect("/Assign/res");
                    }
                    return Redirect("/Assign/res");
                }
            }
            return NoContent();
        }
    }
}
