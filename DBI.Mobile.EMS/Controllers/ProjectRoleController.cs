using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class ProjectRoleController : ApiController
    {

        /// <summary>
        /// This returns a list of employee roles
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<PA_ROLES_V> Get()
        {
            Entities _context = new Entities();
            List<PA_ROLES_V> pl = _context.PA_ROLES_V.Where(p =>p.EFFECTIVE_END_DATE <= DateTime.Now).ToList();
            return pl;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<PA_ROLES_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<PA_ROLES_V> pl = new List<PA_ROLES_V>();
            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                 pl = _context.PA_ROLES_V.Where(p =>p.LAST_UPDATE_DATE >= checkDate).ToList();     
            }
            return pl;
        }

    }
}
