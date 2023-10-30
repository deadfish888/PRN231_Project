using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    public class CoursesController : ODataController
    {
        Prn231PrjContext _db;

        public CoursesController(Prn231PrjContext db)
        {
            _db = db;
        }
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Courses.AsQueryable());
        }
        [EnableQuery(MaxExpansionDepth = 3)]
        public IActionResult Get([FromRoute] int key)
        {
            var s = _db.Courses.Include(s => s.Category).Include(s => s.Sections).ThenInclude(s => s.Items).ThenInclude(s => s.Resource)
                .Include(s => s.Sections).ThenInclude(s => s.Items).ThenInclude(s => s.Assignment)
                .AsSplitQuery()
                .FirstOrDefault(s => s.CourseId == key);
            if (s == null)
            {
                return NotFound();
            }
            return Ok(s);
        }
    }
}
