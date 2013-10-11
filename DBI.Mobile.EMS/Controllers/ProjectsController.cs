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
        [Authorize]
        public IEnumerable<IOS_PROJECT_V> GetProjects()
        {
            Entities _context = new Entities();
            List<XX_PROJECTS_V> pl = _context.XX_PROJECTS_V.ToList();

            List<IOS_PROJECT_V> returnList = new List<IOS_PROJECT_V>();

            foreach (XX_PROJECTS_V item in pl)
            {
                IOS_PROJECT_V rItem = new IOS_PROJECT_V();
                rItem.PROJECT_ID = item.PROJECT_ID;
                rItem.SEGMENT1 = item.SEGMENT1;
                rItem.LONG_NAME = item.LONG_NAME;
                rItem.ORG_ID = Double.Parse(item.ORG_ID.ToString());
                rItem.LAST_UPDATE_DATE = item.LAST_UPDATE_DATE;
                returnList.Add(rItem);
            }
            return returnList;
        }
    }
}
