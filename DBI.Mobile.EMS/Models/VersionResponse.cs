using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBI.Mobile.EMS.Models
{
    public class VersionResponse
    {
        public class Version
        {
            public string number { get; set; }
            public bool mandatory { get; set; }
            public string install_url { get; set; }
        }

        public class RootObject
        {
            public Version version { get; set; }
            public double min_version_number { get; set; }
        }

    }
}