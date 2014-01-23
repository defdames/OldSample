using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class ExpenditureController : ApiController
    {
         /// <summary>
        /// This returns a list of customer billing projects
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<EXPENDITURE_TYPE_V> Get()
        {
            Entities _context = new Entities();
            List<EXPENDITURE_TYPE_V> pl = _context.EXPENDITURE_TYPE_V.ToList();
            return pl;

        }

        [Authorize]
        [HttpGet]
        public IEnumerable<EXPENDITURE_TYPE_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<EXPENDITURE_TYPE_V> returnList = new List<EXPENDITURE_TYPE_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<EXPENDITURE_TYPE_V> pt = _context.EXPENDITURE_TYPE_V.Where(p => p.LAST_UPDATE_DATE >= checkDate).ToList();
                return pt;
            }
            return returnList;
        }

    }
}