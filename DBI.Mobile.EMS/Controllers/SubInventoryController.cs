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

        [Authorize]
        [HttpGet]
        public IEnumerable<MOBILE_SUBINVENTORY_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<MOBILE_SUBINVENTORY_V> returnList = new List<MOBILE_SUBINVENTORY_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<SUBINVENTORY_V> sl = _context.SUBINVENTORY_V.Where(p => p.LAST_UPDATE_DATE >= checkDate).ToList();
                foreach (SUBINVENTORY_V item in sl)
                {
                    MOBILE_SUBINVENTORY_V rItem = new MOBILE_SUBINVENTORY_V();
                    rItem.ORG_ID = item.ORG_ID;
                    rItem.SECONDARY_INV_NAME = item.SECONDARY_INV_NAME;
                    rItem.SUBINVENTORY_DESCRIPTION = item.DESCRIPTION;
                    returnList.Add(rItem);
                }
            }
            return returnList;
            
        }
    }
}

