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

        /// <summary>
        /// returns all service units
        /// </summary>
        /// <returns></returns>
        public static List<ServiceUnitResponse> ServiceUnitTypes()
        {
            List<ServiceUnitResponse> results = (from s in ServiceUnits() group s by s.project into x select x.First()).ToList();
            return results;
        }

        /// <summary>
        /// Returns all service units by rail road type.
        /// </summary>
        /// <param name="rrType"></param>
        /// <returns></returns>
        public static List<ServiceUnitResponse> ServiceUnitTypes(string rrType)
        {
            List<ServiceUnitResponse> results = (from s in ServiceUnits().Where(a => a.project == rrType) group s by s.project into x select x.First()).ToList();
            return results;
           
        }

        public static List<ServiceUnitResponse> ServiceUnitUnits(string proj)
        {
            CROSSING data = new CROSSING();
            List<ServiceUnitResponse> results = (from s in ServiceUnits() group s by s.service_unit into x select x.First()).Where(a => a.project == proj).ToList();
            results.Add(new ServiceUnitResponse { service_unit = "N / A" });   
            
            using (Entities _context = new Entities())
            {
                CROSSING_RAILROAD _cr = _context.CROSSING_RAILROAD.Where(x => x.RAILROAD == proj).SingleOrDefault();
                List<CROSSING_SERVICE_UNIT> _csu = _context.CROSSING_SERVICE_UNIT.Where(x => x.RAILROAD_ID == _cr.RAILROAD_ID).ToList();

                foreach (var _serviceUnit in _csu)
                {
                    ServiceUnitResponse _sur = new ServiceUnitResponse();
                    _sur.project = _cr.RAILROAD;
                    _sur.service_unit = _serviceUnit.SERVICE_UNIT_NAME;
                    results.Add(_sur);
                }
            }


            return results;
        }
        public static List<ServiceUnitResponse> ServiceUnitDivisions(string unit)
        {
           List<ServiceUnitResponse> results = (from s in ServiceUnits() group s by s.sub_division into x select x.First()).Where(a => a.service_unit == unit).ToList();
           results.Add(new ServiceUnitResponse { sub_division = "N / A" });
              long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
             
            if (results.Count() == 0 && RailroadId == 21)
            {
                using (Entities _context = new Entities())
                {

                    CROSSING_SERVICE_UNIT _csu = _context.CROSSING_SERVICE_UNIT.Where(x => x.SERVICE_UNIT_NAME == unit).SingleOrDefault();
                    List<CROSSING_SUB_DIVISION> _csd = _context.CROSSING_SUB_DIVISION.Where(x => x.SERVICE_UNIT_ID == _csu.SERVICE_UNIT_ID).ToList();

                    foreach (var _subDiv in _csd)
                    {
                        ServiceUnitResponse _sur = new ServiceUnitResponse();
                        _sur.service_unit = _csu.SERVICE_UNIT_NAME;
                        _sur.sub_division = _subDiv.SUB_DIVISION_NAME;
                        results.Add(_sur);
                    }
                }
              
            }
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
       public class ExtraServiceUnit
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
          //public string service_unit{get; set;}
          //public string project { get; set; }
          //public string sub_division { get; set; }
       }

