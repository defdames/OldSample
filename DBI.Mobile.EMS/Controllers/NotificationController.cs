using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class NotificationController : ApiController
    {
        [Authorize]
        [HttpPost]
        public void Post(HttpRequestMessage req)
        {

            //Check the system for the registered device and if it's not saved to the database
            using (Entities _context = new Entities())
            {

            }



        }
    }
}
