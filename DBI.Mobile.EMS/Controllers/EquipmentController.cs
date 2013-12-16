using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class EquipmentController : ApiController
    {
        /// <summary>
        /// This returns a list of vehicles
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<MBL_EQUIPMENT_V> Get()
        {
            Entities _context = new Entities();

            List<MBL_EQUIPMENT_V> returnList = new List<MBL_EQUIPMENT_V>();

            List<PROJECTS_V> pl = _context.PROJECTS_V.Where(p => p.PROJECT_TYPE == "TRUCK & EQUIPMENT").ToList();

            foreach (PROJECTS_V item in pl)
            {
                MBL_EQUIPMENT_V rItem = new MBL_EQUIPMENT_V();
                rItem.PROJECT_ID = item.PROJECT_ID;
                rItem.EQUIPMENT_NAME = item.LONG_NAME;
                rItem.SEGMENT1 = item.SEGMENT1;
                rItem.ORGANIZATION_NAME = item.ORGANIZATION_NAME;
                rItem.ORGANIZATION_ID = item.CARRYING_OUT_ORGANIZATION_ID;
                rItem.ORG_ID = Double.Parse(item.ORG_ID.ToString());
                rItem.CLASS_CODE = PROJECTS_V.ClassCodeByProjectId(item.PROJECT_ID);
                rItem.PROJECT_STATUS_CODE = item.PROJECT_STATUS_CODE;
                returnList.Add(rItem);
            }
            return returnList;
        }


        [Authorize]
        [HttpGet]
        public IEnumerable<MBL_EQUIPMENT_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<MBL_EQUIPMENT_V> returnList = new List<MBL_EQUIPMENT_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<PROJECTS_V> pl = _context.PROJECTS_V.Where(p => p.LAST_UPDATE_DATE >= checkDate && p.PROJECT_TYPE == "TRUCK & EQUIPMENT").ToList();
                foreach (PROJECTS_V item in pl)
                {
                    MBL_EQUIPMENT_V rItem = new MBL_EQUIPMENT_V();
                    rItem.PROJECT_ID = item.PROJECT_ID;
                    rItem.EQUIPMENT_NAME = item.LONG_NAME;
                    rItem.SEGMENT1 = item.SEGMENT1;
                    rItem.ORGANIZATION_NAME = item.ORGANIZATION_NAME;
                    rItem.ORGANIZATION_ID = item.CARRYING_OUT_ORGANIZATION_ID;
                    rItem.ORG_ID = Double.Parse(item.ORG_ID.ToString());
                    rItem.CLASS_CODE = PROJECTS_V.ClassCodeByProjectId(item.PROJECT_ID);
                    rItem.PROJECT_STATUS_CODE = item.PROJECT_STATUS_CODE;
                    returnList.Add(rItem);
                }
            }
            return returnList;
        } 
        

    }
}
