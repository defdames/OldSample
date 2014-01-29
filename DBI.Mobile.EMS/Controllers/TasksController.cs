using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;


namespace DBI.Mobile.EMS.Controllers
{
    public class TasksController : ApiController
    {
        /// <summary>
        /// This returns a list of customer billing projects
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<MOBILE_TASK_V> Get()
        {
            Entities _context = new Entities();
            List<PA_TASKS_V> pl = _context.PA_TASKS_V.ToList();
            List<MOBILE_TASK_V> returnList = new List<MOBILE_TASK_V>();

            foreach(PA_TASKS_V task in pl)
            {
                MOBILE_TASK_V nTask = new MOBILE_TASK_V();
                nTask.TASK_ID = task.TASK_ID;
                nTask.TASK_NUMBER = task.TASK_NUMBER;
                nTask.TASK_DESCRIPTION = task.DESCRIPTION;
                nTask.PROJECT_ID = task.PROJECT_ID;
                nTask.LAST_UPDATE_DATE = task.LAST_UPDATE_DATE;
                nTask.START_DATE = task.START_DATE.ToString();
                nTask.COMPLETION_DATE = task.COMPLETION_DATE.ToString();
                returnList.Add(nTask);
            }

            return returnList;

        }

        [Authorize]
        [HttpGet]
        public IEnumerable<MOBILE_TASK_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<MOBILE_TASK_V> returnList = new List<MOBILE_TASK_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
                List<PA_TASKS_V> pt = _context.PA_TASKS_V.Where(p => p.LAST_UPDATE_DATE >= checkDate).ToList();
                foreach (PA_TASKS_V task in pt)
                {
                    MOBILE_TASK_V nTask = new MOBILE_TASK_V();
                    nTask.TASK_ID = task.TASK_ID;
                    nTask.TASK_NUMBER = task.TASK_NUMBER;
                    nTask.TASK_DESCRIPTION = task.DESCRIPTION;
                    nTask.PROJECT_ID = task.PROJECT_ID;
                    nTask.LAST_UPDATE_DATE = task.LAST_UPDATE_DATE;
                    nTask.START_DATE = task.START_DATE.ToString();
                    nTask.COMPLETION_DATE = task.COMPLETION_DATE.ToString();
                    returnList.Add(nTask);
                }
            }
            return returnList;
        }

    }
}
