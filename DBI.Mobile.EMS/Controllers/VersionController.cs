using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBI.Mobile.EMS.Controllers
{
    public class VersionController : ApiController
    {
        /// <summary>
        /// This returns a list of customer billing projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RootObject Get(string id)
        {
            if (id == "production")
            {
                Version ver = new Version();
                ver.number = "1.0.T140923";
                ver.mandatory = true;
                ver.install_url = "itms-services://?action=download-manifest&url=https://emsmobiletest.dbiservices.com/iems/iEMS.plist";
                ver.descriptionText = "Testing for EPA Number, EPA Number for Items 40001,40007 and 40012 should show the correct EPA number.";

                RootObject root = new RootObject();
                root.version = ver;
                root.min_version_number = 1.0;
                return root;
            }
            else if (id == "internal")
            {
                Version ver = new Version();
                ver.number = "1.0.T140626";
                ver.mandatory = false;
                ver.install_url = "itms-services://?action=download-manifest&url=https://emsmobiletest.dbiservices.com/iems/iEMS.plist";
                ver.descriptionText = "Employee drive time removed from non IRM Jobs. Added updated version control and modified the copy feature.";

                RootObject root = new RootObject();
                root.version = ver;
                root.min_version_number = 1.0;
                return root;
            }
            return null;
        }
    }

    public class Version
    {
    public string number { get; set; }
    public bool mandatory { get; set; }
    public string install_url { get; set; }
    public string descriptionText { get; set; }
    }

    public class RootObject
    {
    public Version version { get; set; }
    public double  min_version_number { get; set; }
    }
}
