using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Project_API.Models;

namespace Project_API.Controllers
{
    public class SectionsController : ODataController
    {
        Prn231PrjContext _db;

        public SectionsController(Prn231PrjContext db)
        {
            _db = db;
        }
        [EnableQuery]
        public IActionResult Get([FromODataUri]int courseId)
        {
            return Ok(_db.Sections.Where(s => s.CourseId == courseId).AsQueryable());
        }
    }
}
