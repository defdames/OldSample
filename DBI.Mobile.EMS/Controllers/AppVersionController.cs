using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Mobile.EMS.Models;

namespace DBI.Mobile.EMS.Controllers
{
    public class AppVersionController : ApiController
    {

        [HttpGet]
        public VersionResponse.RootObject GetValidateToken()
        {
            VersionResponse.RootObject response = new VersionResponse.RootObject();

            VersionResponse.Version details = new VersionResponse.Version();
            details.number = "B140220";
            details.mandatory = true;
            details.install_url = "itms-services://?action=download-manifest&url=http://emsmobile.dbiservices.com/iems/iEMS.plist";

            response.min_version_number = 1.0;
            response.version = details;

            return response;
        }


    }
}
