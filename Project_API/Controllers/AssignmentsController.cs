using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    public class AssignmentsController : ODataController
    {
        private readonly Prn231PrjContext _db;

        public AssignmentsController(Prn231PrjContext db)
        {
            _db = db;
        }

        [Authorize("Student")]
        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_db.Assignments.Include(s => s.Item).ThenInclude(s => s.Section).FirstOrDefault(s => s.AssignmentId == key));
        }
    }
}
