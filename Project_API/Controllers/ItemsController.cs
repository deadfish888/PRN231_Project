using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Project_API.Models;

namespace Project_API
{
    [Authorize("Student")]
    public class ItemsController : ODataController
    {
        Prn231PrjContext _db;

        public ItemsController(Prn231PrjContext db)
        {
            _db = db;
        }
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Items.AsQueryable());
        }
    }
}
