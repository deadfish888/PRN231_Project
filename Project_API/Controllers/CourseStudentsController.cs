using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    [Authorize("Student")]
    public class CourseStudentsController : ODataController
    {
        Prn231PrjContext _db;
        public CourseStudentsController(Prn231PrjContext db)
        {
            _db = db;
        }

        public IActionResult Post([FromBody]CourseStudent cs)
        {
            try
            {
                _db.CourseStudents.Add(cs);
                return Ok(_db.SaveChanges());
            }
            catch
            {
                return Conflict();
            }
        }
        [HttpDelete("odata/CourseStudents({key1},{key2})")]
        public IActionResult Delete( int key1, int key2)
        {
            try
            {
                var check = _db.CourseStudents.FirstOrDefault(cs => cs.UserId == key1 && cs.CourseId == key2);
                if (check == null)
                {
                    return NotFound();
                }
                _db.CourseStudents.Remove(check);
                return Ok(_db.SaveChanges());
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
