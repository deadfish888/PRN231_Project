using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Project_API.Controllers
{
    public class StudentsController : ODataController
    {
        Prn231PrjContext _db;
        private readonly IConfiguration _configuration;
        public StudentsController(Prn231PrjContext db, IConfiguration configuration) { _db = db; _configuration = configuration; }

        [HttpPost("odata/Students/Login")]
        public IActionResult Login([FromBody]LoginDTO login)
        {
            //User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            //if (user == null || BCrypt.Verify(password, user.Password) == false)
            //{
            //    return null;
            //}
            Student s = _db.Students.FirstOrDefault(s => s.EmailAddress.Equals(login.email) && s.Password.Equals(login.password));
            if (s == null)
            {
                return NotFound("Wrong email or password!");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, s.EmailAddress),
            // Add other claims as needed
        }),
                IssuedAt = DateTime.UtcNow,
                Issuer = "issuer",
                Audience = "audience",
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                UserId = s.UserId,
                FullName = s.FullName,
                EmailAddress = s.EmailAddress,
                Img = s.Img,
                Description = s.Description,
                Token = tokenHandler.WriteToken(token)
            });
        }
        [Authorize("Student")]
        public IActionResult Get([FromRoute]int key)
        {
            var s = _db.Students.FirstOrDefault(s => s.UserId==key);
            if (s == null)
            {
                return NotFound();
            }
            s.Password = "";
            return Ok(s);
        }
        [Authorize("Student")]
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

    public class LoginDTO
    {
        public string email { get;set; }
        public string password { get;set; }
    }
}
