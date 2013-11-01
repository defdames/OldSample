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
        //[Authorize]
        [HttpGet]
        public IEnumerable<PA_TASKS_V> Get()
        {
            Entities _context = new Entities();
            List<PA_TASKS_V> pl = _context.PA_TASKS_V.ToList();
            return pl;
        }
    }
}
