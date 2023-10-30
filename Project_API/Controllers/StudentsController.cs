﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using System.Net;

namespace Project_API.Controllers
{
    public class StudentsController : ODataController
    {
        Prn231PrjContext _db;
        public StudentsController(Prn231PrjContext db) { _db = db; }

        
        public IActionResult Get([FromRoute]int key)
        {
            var s = _db.Students.FirstOrDefault(s => s.UserId==key);
            if (s == null)
            {
                return NotFound();
            }
            return Ok(s);
        }
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Expand)]
        public IActionResult GetCourseStudents([FromODataUri] int key)
        {
            var s = _db.Students.Include(s => s.CourseStudents).ThenInclude(s => s.Course).FirstOrDefault(s => s.UserId == key);
            if (s == null)
            {
                return NotFound();
            }
            return Ok(s.CourseStudents);
        }
    }
}