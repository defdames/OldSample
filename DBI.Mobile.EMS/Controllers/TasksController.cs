using System;
using System.Collections.Generic;
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
                returnList.Add(nTask);
            }

            return returnList;

        }
    }
}
