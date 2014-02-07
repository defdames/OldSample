using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBI.Mobile.EMS.Controllers
{
    public class SecurityController : ApiController
    {

        [Authorize]
        [HttpGet]
        public Data.MBL_STRING GetValidateToken()
        {


                Data.MBL_STRING returnValue = new Data.MBL_STRING();
                returnValue.TokenValidation = "Valid";
                return returnValue;
        }

    }
}
