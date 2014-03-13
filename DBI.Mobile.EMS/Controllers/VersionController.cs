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
        public RootObject Get()
        {
            Version ver = new Version();
            ver.number = "1.0.B140312";
            ver.mandatory = true;

            RootObject root = new RootObject();
            root.version = ver;
            root.min_version_number = 1.0;

            return root;
        }
    }



    public class Version
    {
    public string number { get; set; }
    public bool mandatory { get; set; }
    public string install_url { get; set; }
    }

    public class RootObject
    {
    public Version version { get; set; }
    public double  min_version_number { get; set; }
    }
}
