using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;


namespace DBI.Mobile.EMS.Controllers
{
    public class EmployeeController : ApiController
    {
         /// <summary>
        /// This returns a list of customer billing projects
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<MBL_EMPLOYEE_V> Get()
        {
            Entities _context = new Entities();
            List<EMPLOYEES_V> pl = _context.EMPLOYEES_V.ToList();

            List<MBL_EMPLOYEE_V> returnList = new List<MBL_EMPLOYEE_V>();

            foreach (EMPLOYEES_V item in pl)
            {
                MBL_EMPLOYEE_V rItem = new MBL_EMPLOYEE_V();
                rItem.PERSON_ID = item.PERSON_ID;
                rItem.EMPLOYEE_NAME = item.EMPLOYEE_NAME;
                rItem.JOB_NAME = item.JOB_NAME;
                rItem.CURRENT_ORGANIZATION = item.ORGANIZATION_NAME;
                rItem.CURRENT_ORG_ID = item.ORGANIZATION_ID;
                rItem.CURRENT_EMPLOYEE_FLAG = (item.CURRENT_EMPLOYEE_FLAG == null) ? "N" : "Y";
                returnList.Add(rItem);
            }
            return returnList;
        }
        [Authorize]
        [HttpGet]
        public IEnumerable<MBL_EMPLOYEE_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<MBL_EMPLOYEE_V> returnList = new List<MBL_EMPLOYEE_V>();
            
            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<EMPLOYEES_V> pl = _context.EMPLOYEES_V.Where(p => p.LAST_UPDATE_DATE >= checkDate && p.CURRENT_EMPLOYEE_FLAG == "Y").ToList();
                foreach (EMPLOYEES_V item in pl)
                    {
                        MBL_EMPLOYEE_V rItem = new MBL_EMPLOYEE_V();
                        rItem.PERSON_ID = item.PERSON_ID;
                        rItem.EMPLOYEE_NAME = item.EMPLOYEE_NAME;
                        rItem.JOB_NAME = item.JOB_NAME;
                        rItem.CURRENT_ORGANIZATION = item.ORGANIZATION_NAME;
                        rItem.CURRENT_ORG_ID = item.ORGANIZATION_ID;
                        rItem.CURRENT_EMPLOYEE_FLAG = (item.CURRENT_EMPLOYEE_FLAG == null) ? "N" : "Y";
                        returnList.Add(rItem);
                    }
            }
            return returnList;
            
        }

    }
}
