using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class InventoryController : ApiController
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<INVENTORY_V> Get()
        {
            Entities _context = new Entities();
            return _context.INVENTORY_V.ToList();  
        }



    }
}
