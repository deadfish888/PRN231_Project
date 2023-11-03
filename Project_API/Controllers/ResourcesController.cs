using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    public class ResourcesController : ODataController
    {
        private readonly Prn231PrjContext _db;

        public ResourcesController(Prn231PrjContext db)
        {
            _db = db;
        }
        [EnableQuery]
        public IActionResult Get([FromRoute] int key)
        {
            return Ok(_db.Resources.Include(s=>s.Item).FirstOrDefault(s => s.ResourceId == key));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ResourceDTO resource)
        {
            Resource r = new Resource()
            {
                ItemId = resource.ItemId,
                Data = resource.Data
            };
            _db.Resources.Add(r);
            return Ok(_db.SaveChanges());
        }
    }

    public class ResourceDTO
    {
        public int? ItemId { get; set; }

        public byte[]? Data { get; set; }
    }
}
