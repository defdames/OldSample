using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class SubInventoryController : ApiController
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<SUBINVENTORY_V> Get()
        {
            Entities _context = new Entities();
            List<SUBINVENTORY_V> sinvl = _context.SUBINVENTORY_V.ToList();
            return _context.SUBINVENTORY_V.ToList();
        }

    }
}
