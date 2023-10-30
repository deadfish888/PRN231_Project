using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PRN231_CMS.Models;
using System.Diagnostics;

namespace PRN231_CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        string link = "https://localhost:7167/odata/";
        public async Task<IActionResult> IndexAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(link + "Courses"))
                {
                    using (HttpContent
                        content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        var courses = JsonConvert.DeserializeObject<List<Course>>(JObject.Parse(data)["value"].ToString());
                        Console.WriteLine(courses[0].ToString());
                    }
                }


            }
            return View();
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}