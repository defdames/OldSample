using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class SecurityOrgsController : ApiController
    {

        [Authorize]
        [HttpGet]
        public List<returnData> Get(string username)
        {
            //Get UserID using usernamne
            SYS_USER_INFORMATION _userInformation = SYS_USER_INFORMATION.UserByUserName(username.ToUpper());

            var orgs = SYS_USER_ORGS.GetUserOrgs(_userInformation.USER_ID);

            List<returnData> _userOrgs = new List<returnData>();

            foreach (SYS_USER_ORGS _org in orgs)
            {
                returnData _data = new returnData();
                _data.organization_id =_org.ORG_ID;
                _userOrgs.Add(_data);
            }

            return _userOrgs;
        }

        public class returnData
        {
            public long organization_id { get; set; }
        }

    }
}
