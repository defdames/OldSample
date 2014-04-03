using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class PersonController : ApiController
    {

        [Authorize]
        [HttpGet]
        public SYS_USER_INFORMATION Get(string username)
        {
            Entities _context = new Entities();
            SYS_USER_INFORMATION pl = new SYS_USER_INFORMATION();

            pl = _context.SYS_USER_INFORMATION.Where(p => p.USER_NAME.ToLower() == username.ToLower()).FirstOrDefault();

            if (pl.EMPLOYEE_NAME == null)
            {
                // Error 204 no account information found
                throw new HttpResponseException(HttpStatusCode.BadGateway);
            }

            IEnumerable<string> headerValues = Request.Headers.GetValues("DeviceID");
            var id = headerValues.FirstOrDefault();
            Utility.registerDeviceForNotifications(id.ToString(),pl.USER_ID);

            return pl;
        }

        //[Authorize]
        public List<SYS_USER_INFORMATION> Get()
        {
            Entities _context = new Entities();
            List<SYS_USER_INFORMATION> pl = _context.SYS_USER_INFORMATION.ToList();
            return pl;
        }


    }
}
