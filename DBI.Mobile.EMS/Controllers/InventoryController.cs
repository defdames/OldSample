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
           Entities _context = new Entities();
            List<MOBILE_INVENTORY_V> returnList = new List<MOBILE_INVENTORY_V>();

                List<INVENTORY_V> pl = _context.INVENTORY_V.Where(p => p.ACTIVE == "Y" && p.ENABLED_FLAG == "Y").ToList();
                foreach (INVENTORY_V item in pl)
                {
                    MOBILE_INVENTORY_V rItem = new MOBILE_INVENTORY_V();
                    rItem.ATTRIBUTE2 = item.ATTRIBUTE2;
                    rItem.INV_NAME = item.INV_NAME;
                    rItem.INV_LOCATION = item.INV_LOCATION;
                    rItem.ORGANIZATION_ID = item.ORGANIZATION_ID;
                    rItem.ITEM_ID = item.ITEM_ID;
                    rItem.SEGMENT1 = item.SEGMENT1;
                    rItem.ITEM_DESCRIPTION = item.DESCRIPTION;
                    rItem.UOM_CODE = item.UOM_CODE;
                    rItem.ENABLED_FLAG = item.ENABLED_FLAG;
                    rItem.ACTIVE = item.ACTIVE;
                    rItem.ITEM_COST = item.ITEM_COST;
                    rItem.LAST_UPDATE_DATE = item.LAST_UPDATE_DATE;
                    rItem.LE = item.LE;
                    rItem.EPA_DESCRIPTION = item.EPA_DESCRIPTION;
                    returnList.Add(rItem);
                 }
            return returnList;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<MOBILE_INVENTORY_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<MOBILE_INVENTORY_V> returnList = new List<MOBILE_INVENTORY_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<INVENTORY_V> pl = _context.INVENTORY_V.Where(p => p.LAST_UPDATE_DATE >= checkDate).ToList();
                foreach (INVENTORY_V item in pl)
                {
                    MOBILE_INVENTORY_V rItem = new MOBILE_INVENTORY_V();
                    rItem.ATTRIBUTE2 = item.ATTRIBUTE2;
                    rItem.INV_NAME = item.INV_NAME;
                    rItem.INV_LOCATION = item.INV_LOCATION;
                    rItem.ORGANIZATION_ID = item.ORGANIZATION_ID;
                    rItem.ITEM_ID = item.ITEM_ID;
                    rItem.SEGMENT1 = item.SEGMENT1;
                    rItem.ITEM_DESCRIPTION = item.DESCRIPTION;
                    rItem.UOM_CODE = item.UOM_CODE;
                    rItem.ENABLED_FLAG = item.ENABLED_FLAG;
                    rItem.ACTIVE = item.ACTIVE;
                    rItem.ITEM_COST = item.ITEM_COST;
                    rItem.LAST_UPDATE_DATE = item.LAST_UPDATE_DATE;
                    rItem.LE = item.LE;
                    rItem.EPA_DESCRIPTION = item.EPA_DESCRIPTION;
                    returnList.Add(rItem);
                }
            }
            return returnList;
        }
    }

   
}
