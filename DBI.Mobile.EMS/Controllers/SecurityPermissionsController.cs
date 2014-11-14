using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class SecurityPermissionsController : ApiController
    {

        //[Authorize]
        [HttpGet]
        public List<returnData> Get(string username)
        {

            List<Claim> claims = SYS_PERMISSIONS.Claims(username.ToUpper());

            List<returnData> _userClaims = new List<returnData>();

            foreach (Claim _claim in claims)
            {
                returnData _data = new returnData();
                _data.permission_name = _claim.Value;
                _userClaims.Add(_data);
            }

            return _userClaims;

        }

        public class returnData
        {
            public string permission_name { get; set; }
        }

    }
}



