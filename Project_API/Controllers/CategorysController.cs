using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    public class CategoriesController : ODataController
    {
        Prn231PrjContext _db;

        public CategoriesController(Prn231PrjContext db)
        {
            _db = db;
        }
        [EnableQuery(MaxExpansionDepth = 1)]
        public IActionResult Get()
        {
            return Ok(_db.Categories.AsQueryable());
        }
    }
}
