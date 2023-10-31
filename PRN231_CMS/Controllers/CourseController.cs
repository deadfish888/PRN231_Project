using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PRN231_CMS.Models;

namespace PRN231_CMS.Controllers
{
    public class CourseController : Controller
    {
        string link = "https://localhost:7167/odata/";
        public async Task<IActionResult> IndexAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(link + "Courses(1)?$expand=Sections($expand=Items($expand=Resource,Assignment))"))
                {
                    using (HttpContent
                        content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        var course = JsonConvert.DeserializeObject<Course>(data);
                        ViewBag.currentCourse = course;
                    }
                }


            }
            return View();
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(link + "Courses("+id+")?$expand=Sections($expand=Items($expand=Resource,Assignment))"))
                {
                    using (HttpContent
                        content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        var course = JsonConvert.DeserializeObject<Course>(data);
                        ViewBag.currentCourse = course;
                    }
                }


            }
            return View();
        }
    }
}
