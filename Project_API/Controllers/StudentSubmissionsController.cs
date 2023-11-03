using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    [Authorize("Student")]
    public class StudentSubmissionsController : ODataController
    {
        private readonly Prn231PrjContext _db;
        public StudentSubmissionsController(Prn231PrjContext db) { _db = db; }


        [HttpGet("odata/StudentSubmissions({userId}, {assignId})")]
        public IActionResult Get(int userId, int assignId)
        {
            return Ok(_db.StudentSubmissions.FirstOrDefault(s => s.UserId == userId && s.AssignmentId == assignId));
        }

        public IActionResult Post([FromBody] StudentSubmission ss)
        {
            try
            {
                _db.StudentSubmissions.Add(ss);
                return Ok(_db.SaveChanges());
            }
            catch
            {
                return Conflict();
            }

        }

        [HttpPut("odata/StudentSubmissions({userId}, {assignId})")]
        public IActionResult Put(int userId, int assignId, [FromBody] StudentSubmission ss)
        {
            _db.Entry(ss).State = EntityState.Modified;
            return Ok(_db.SaveChanges());
        }
    }
}

