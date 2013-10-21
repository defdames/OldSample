using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class ProjectsController : ApiController
    {
        /// <summary>
        /// This returns a list of projects
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        public IEnumerable<MBL_PROJECT_V> Get()
        {
            Entities _context = new Entities();
            List<PROJECTS_V> pl = _context.PROJECTS_V.ToList();

            List<MBL_PROJECT_V> returnList = new List<MBL_PROJECT_V>();

            foreach (PROJECTS_V item in pl)
            {
                MBL_PROJECT_V rItem = new MBL_PROJECT_V();
                rItem.PROJECT_ID = item.PROJECT_ID;
                rItem.SEGMENT1 = item.SEGMENT1;
                rItem.LONG_NAME = item.LONG_NAME;
                rItem.ORG_ID = Double.Parse(item.ORG_ID.ToString());
                rItem.CARRYING_OUT_ORGANIZATION_NAME = item.ORGANIZATION_NAME;
                rItem.CARRYING_OUT_ORGANIZATION_ID = item.CARRYING_OUT_ORGANIZATION_ID;
                rItem.LAST_UPDATED_DATE = item.LAST_UPDATE_DATE;
                returnList.Add(rItem);
            }
            return returnList;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<MBL_PROJECT_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<MBL_PROJECT_V> returnList = new List<MBL_PROJECT_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<PROJECTS_V> pl = _context.PROJECTS_V.Where(p =>p.LAST_UPDATE_DATE >= checkDate).ToList();
                foreach (PROJECTS_V item in pl)
                 {
                    MBL_PROJECT_V rItem = new MBL_PROJECT_V();
                    rItem.PROJECT_ID = item.PROJECT_ID;
                    rItem.SEGMENT1 = item.SEGMENT1;
                    rItem.LONG_NAME = item.LONG_NAME;
                    rItem.ORG_ID = Double.Parse(item.ORG_ID.ToString());
                    rItem.CARRYING_OUT_ORGANIZATION_NAME = item.ORGANIZATION_NAME;
                    rItem.CARRYING_OUT_ORGANIZATION_ID = item.CARRYING_OUT_ORGANIZATION_ID;
                    rItem.LAST_UPDATED_DATE = item.LAST_UPDATE_DATE;
                    returnList.Add(rItem);
                }
            }
            return returnList;
        }
    }
}
