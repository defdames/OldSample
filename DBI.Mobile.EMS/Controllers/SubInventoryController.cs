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
        public IEnumerable<MOBILE_SUBINVENTORY_V> Get()
        {
            List<MOBILE_SUBINVENTORY_V> data = INVENTORY_V.MobileSubInventoryList();
            return data;
        }

    }
}

