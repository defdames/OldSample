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
        public IEnumerable<MOBILE_INVENTORY_V> Get()
        {
            List<MOBILE_INVENTORY_V> data = INVENTORY_V.MobileInventoryList();
            return data;
        }

    }
}
