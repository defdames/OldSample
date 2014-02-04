using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.GMS
{
    public class ServiceUnitData
    {
        public static List<ServiceUnitResponse> ServiceUnits()
        {
            string gmsData = new WebClient().DownloadString("http://gms.dbiservices.com/services/up/mms/list/mms_divisions");
            List<ServiceUnitResponse> results = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServiceUnitResponse>>(gmsData);
            return results;
        }

        public static List<ServiceUnitResponse> ServiceUnitTypes()
        {
            List<ServiceUnitResponse> results = (from s in ServiceUnits()  group s by s.project into x select  x.First()).ToList();
         
            return results;
        }
        public static List<ServiceUnitResponse> ServiceUnitUnits()
        {
            List<ServiceUnitResponse> results = (from u in ServiceUnits() where u.project == u.service_unit group u by u.service_unit into x select x.First()).ToList();
            return results;
        }
        public static List<ServiceUnitResponse> ServiceUnitDivisions()
        {
            List<ServiceUnitResponse> results = (from s in ServiceUnits() group s by s.sub_division into x select x.First()).ToList();
            return results;
        }
    }


       public class ServiceUnitResponse
        {
            public string project
            {
                get { return m_project; }
                set { m_project = value; }
            }

            private string m_project;

            public string service_unit
            {
                get { return m_service_unit; }
                set { m_service_unit = value; }
            }

            private string m_service_unit;

            public string sub_division
            {
                get { return m_sub_division; }
                set { m_sub_division = value; }
            }
            string m_sub_division;
        }

}
