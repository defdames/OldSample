using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using System.Data.Objects.SqlClient;

namespace DBI.Data
{
    public class CROSSING_MAINTENANCE
    {
        public static IQueryable<ProjectList> GetProjectList(decimal RailroadId, Entities _context)
        {

            return (from v in _context.PROJECTS_V
                    where !(from p in _context.CROSSING_PROJECT
                            where v.PROJECT_ID == p.PROJECT_ID && p.RAILROAD_ID == RailroadId
                            select p.PROJECT_ID)
                        .Contains(v.PROJECT_ID)
                    select new ProjectList { PROJECT_TYPE = v.PROJECT_TYPE, CARRYING_OUT_ORGANIZATION_ID = v.CARRYING_OUT_ORGANIZATION_ID, PROJECT_STATUS_CODE = v.PROJECT_STATUS_CODE, TEMPLATE_FLAG = v.TEMPLATE_FLAG, PROJECT_ID = v.PROJECT_ID, LONG_NAME = v.LONG_NAME, ORGANIZATION_NAME = v.ORGANIZATION_NAME, SEGMENT1 = v.SEGMENT1 });

        }
        public static IQueryable<ProjectList> GetCrossingSecurityProjectList(decimal RailroadId, Entities _context)
        {

            return (from v in _context.PROJECTS_V
                    select new ProjectList { PROJECT_TYPE = v.PROJECT_TYPE, CARRYING_OUT_ORGANIZATION_ID = v.CARRYING_OUT_ORGANIZATION_ID, PROJECT_STATUS_CODE = v.PROJECT_STATUS_CODE, TEMPLATE_FLAG = v.TEMPLATE_FLAG, PROJECT_ID = v.PROJECT_ID, LONG_NAME = v.LONG_NAME, ORGANIZATION_NAME = v.ORGANIZATION_NAME, SEGMENT1 = v.SEGMENT1 });

        }
        public static IQueryable<ProjectList> GetProjectList(decimal RailroadId, decimal CrossingId, Entities _context)
        {

            return (from v in _context.PROJECTS_V
                    select new ProjectList { PROJECT_TYPE = v.PROJECT_TYPE, CARRYING_OUT_ORGANIZATION_ID = v.CARRYING_OUT_ORGANIZATION_ID, PROJECT_STATUS_CODE = v.PROJECT_STATUS_CODE, TEMPLATE_FLAG = v.TEMPLATE_FLAG, PROJECT_ID = v.PROJECT_ID, LONG_NAME = v.LONG_NAME, ORGANIZATION_NAME = v.ORGANIZATION_NAME, SEGMENT1 = v.SEGMENT1 });

        }
        public static IQueryable<CrossingList> GetCrossingProjectList(decimal RailroadId, Entities _context)
        {

            return (from d in _context.CROSSINGS
                    where !(from r in _context.CROSSING_RELATIONSHIP
                            where d.CROSSING_ID == r.CROSSING_ID
                            select r.CROSSING_ID)
                       .Contains(d.CROSSING_ID)
                    where d.RAILROAD_ID == RailroadId
                    select new CrossingList {RAILROAD_ID = d.RAILROAD, CONTACT_ID = d.CONTACT_ID, STATUS = d.STATUS, STATE = d.STATE, CROSSING_ID = d.CROSSING_ID, CROSSING_NUMBER = d.CROSSING_NUMBER, SERVICE_UNIT = d.SERVICE_UNIT, SUB_DIVISION = d.SUB_DIVISION, CONTACT_NAME = d.CROSSING_CONTACTS.CONTACT_NAME });

        }
        public static IQueryable<CrossingList> GetCrossingProjectListIncidents(decimal RailroadId, Entities _context)
        {

            return (from d in _context.CROSSINGS
                    where !(from r in _context.CROSSING_RELATIONSHIP
                            where d.CROSSING_ID == r.CROSSING_ID
                            select r.CROSSING_ID)
                       .Contains(d.CROSSING_ID)
                    where d.RAILROAD_ID == RailroadId && d.STATUS == "ACTIVE"
                    select new CrossingList { RAILROAD_ID = d.RAILROAD, CONTACT_ID = d.CONTACT_ID, STATUS = d.STATUS, STATE = d.STATE, CROSSING_ID = d.CROSSING_ID, CROSSING_NUMBER = d.CROSSING_NUMBER, SERVICE_UNIT = d.SERVICE_UNIT, SUB_DIVISION = d.SUB_DIVISION, CONTACT_NAME = d.CROSSING_CONTACTS.CONTACT_NAME });

        }
        public static IQueryable<CrossingList> GetCrossingNoProjectList(decimal RailroadId, Entities _context)
        {

            return (from d in _context.CROSSINGS
                    where d.RAILROAD_ID == RailroadId
                    select new CrossingList {RAILROAD_ID = d.RAILROAD, CONTACT_ID = d.CONTACT_ID, STATUS = d.STATUS, STATE = d.STATE, CROSSING_ID = d.CROSSING_ID, CROSSING_NUMBER = d.CROSSING_NUMBER, SERVICE_UNIT = d.SERVICE_UNIT, SUB_DIVISION = d.SUB_DIVISION, CONTACT_NAME = d.CROSSING_CONTACTS.CONTACT_NAME });

        }
        public static List<CrossingProject> CrossingsProjectList(decimal CrossingId)
        {
            using (Entities _context = new Entities())
            {
                var data = (from r in _context.CROSSING_RELATIONSHIP
                            join p in _context.PROJECTS_V on r.PROJECT_ID equals p.PROJECT_ID
                            where r.CROSSING_ID == CrossingId
                            select new CrossingProject { PROJECT_ID = r.PROJECT_ID, LONG_NAME = p.LONG_NAME, SEGMENT1 = p.SEGMENT1, ORGANIZATION_NAME = p.ORGANIZATION_NAME }).ToList();
                return data;
            }
        }
        public static IQueryable<StateList> GetState(decimal RailroadId, Entities _context)
        {

            return (from d in _context.CROSSINGS
                           where d.RAILROAD_ID == RailroadId
                           select new StateList { STATE = d.STATE  }).Distinct();
        }
        public static IQueryable<CrossingAssignedList> GetCrossingsAssigned(decimal RailroadId, Entities _context)
        {
            
            return (from r in _context.CROSSING_RELATIONSHIP
                        join d in _context.CROSSINGS on r.CROSSING_ID equals d.CROSSING_ID
                        join i in _context.CROSSING_RAILROAD on d.RAILROAD_ID equals i.RAILROAD_ID
                        where d.RAILROAD_ID == RailroadId

                        select new CrossingAssignedList { CROSSING_ID = r.CROSSING_ID, PROJECT_ID = r.PROJECT_ID, CROSSING_NUMBER = d.CROSSING_NUMBER, RAILROAD = i.RAILROAD, SERVICE_UNIT = d.SERVICE_UNIT, SUB_DIVISION = d.SUB_DIVISION, STATE = d.STATE });
        }
        public static IQueryable<ServiceUnitList> GetKCSServiceUnit(Entities _context)
        {
            return (from d in _context.CROSSING_SERVICE_UNIT
                    select new ServiceUnitList { SERVICE_UNIT_ID = d.SERVICE_UNIT_ID, SERVICE_UNIT_NAME = d.SERVICE_UNIT_NAME });
        }
        public static List<CrossingData1> GetAppCrossingList(decimal RailroadId, decimal UserId)
        {            
            string sql = string.Format(@"                           
               WITH
ALLOWED_RECORDS AS (
  SELECT a.RAILROAD_ID,
    a.CONTACT_ID,
    a.CROSSING_ID,
    a.STATUS,
    a.STATE,
    a.CROSSING_NUMBER,
    a.SERVICE_UNIT,
    a.SUB_DIVISION   
  FROM CROSSINGS a
  INNER JOIN CROSSING_RELATIONSHIP b ON a.CROSSING_ID = b.CROSSING_ID
  INNER JOIN PROJECTS_V c ON b.PROJECT_ID = c.PROJECT_ID
  INNER JOIN SYS_USER_ORGS d ON c.CARRYING_OUT_ORGANIZATION_ID = d.ORG_ID
  WHERE d.USER_ID = {1}
)
SELECT MAX(CROSSING_APPLICATION.APPLICATION_REQUESTED) APPLICATION_REQUESTED,
                      ALLOWED_RECORDS.RAILROAD_ID,
                      ALLOWED_RECORDS.CONTACT_ID,
                      ALLOWED_RECORDS.CROSSING_ID,
                      ALLOWED_RECORDS.STATUS,
                      ALLOWED_RECORDS.STATE,
                      ALLOWED_RECORDS.CROSSING_NUMBER,
                      ALLOWED_RECORDS.SERVICE_UNIT,
                      ALLOWED_RECORDS.SUB_DIVISION,
                      PROJECTS_V.PROJECT_TYPE                      
                    FROM (ALLOWED_RECORDS
                    LEFT JOIN CROSSING_APPLICATION ON ALLOWED_RECORDS.CROSSING_ID = CROSSING_APPLICATION.CROSSING_ID
                    LEFT JOIN CROSSING_RELATIONSHIP ON ALLOWED_RECORDS.CROSSING_ID = CROSSING_RELATIONSHIP.CROSSING_ID)
                    LEFT JOIN PROJECTS_V ON CROSSING_RELATIONSHIP.PROJECT_ID = PROJECTS_V.PROJECT_ID
                    WHERE ALLOWED_RECORDS.RAILROAD_ID = {0} AND ALLOWED_RECORDS.STATUS <> 'DELETED'  AND PROJECTS_V.PROJECT_TYPE = 'CUSTOMER BILLING' AND PROJECTS_V.TEMPLATE_FLAG = 'N' AND PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED'
                    GROUP BY ALLOWED_RECORDS.RAILROAD_ID,
                      ALLOWED_RECORDS.CONTACT_ID,
                      ALLOWED_RECORDS.CROSSING_ID,
                      ALLOWED_RECORDS.STATUS,
                      ALLOWED_RECORDS.STATE,
                      ALLOWED_RECORDS.CROSSING_NUMBER,
                      ALLOWED_RECORDS.SERVICE_UNIT,
                      ALLOWED_RECORDS.SUB_DIVISION,
                      PROJECTS_V.PROJECT_TYPE    

                   ", RailroadId, UserId);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<CrossingData1>(sql).ToList();
            }
        }

        public static List<CrossingData1> GetSuppCrossingList(decimal RailroadId, decimal UserId)
        {
            string sql = string.Format(@"                           
               WITH
ALLOWED_RECORDS AS (
  SELECT a.RAILROAD_ID,
    a.CONTACT_ID,
    a.CROSSING_ID,
    a.STATUS,
    a.STATE,
    a.CROSSING_NUMBER,
    a.SERVICE_UNIT,
    a.SUB_DIVISION   
  FROM CROSSINGS a
  INNER JOIN CROSSING_RELATIONSHIP b ON a.CROSSING_ID = b.CROSSING_ID
  INNER JOIN PROJECTS_V c ON b.PROJECT_ID = c.PROJECT_ID
  INNER JOIN SYS_USER_ORGS d ON c.CARRYING_OUT_ORGANIZATION_ID = d.ORG_ID
  WHERE d.USER_ID = {1}
)
SELECT 
                      ALLOWED_RECORDS.RAILROAD_ID,
                      ALLOWED_RECORDS.CONTACT_ID,
                      ALLOWED_RECORDS.CROSSING_ID,
                      ALLOWED_RECORDS.STATUS,
                      ALLOWED_RECORDS.STATE,
                      ALLOWED_RECORDS.CROSSING_NUMBER,
                      ALLOWED_RECORDS.SERVICE_UNIT,
                      ALLOWED_RECORDS.SUB_DIVISION,
                      PROJECTS_V.PROJECT_TYPE                      
                    FROM (ALLOWED_RECORDS
                    LEFT JOIN CROSSING_SUPPLEMENTAL ON ALLOWED_RECORDS.CROSSING_ID = CROSSING_SUPPLEMENTAL.CROSSING_ID
                    LEFT JOIN CROSSING_RELATIONSHIP ON ALLOWED_RECORDS.CROSSING_ID = CROSSING_RELATIONSHIP.CROSSING_ID)
                    LEFT JOIN PROJECTS_V ON CROSSING_RELATIONSHIP.PROJECT_ID = PROJECTS_V.PROJECT_ID
                    WHERE ALLOWED_RECORDS.RAILROAD_ID = {0} AND PROJECTS_V.PROJECT_TYPE = 'CUSTOMER BILLING' AND PROJECTS_V.TEMPLATE_FLAG = 'N' AND PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED'
                    GROUP BY ALLOWED_RECORDS.RAILROAD_ID,
                      ALLOWED_RECORDS.CONTACT_ID,
                      ALLOWED_RECORDS.CROSSING_ID,
                      ALLOWED_RECORDS.STATUS,
                      ALLOWED_RECORDS.STATE,
                      ALLOWED_RECORDS.CROSSING_NUMBER,
                      ALLOWED_RECORDS.SERVICE_UNIT,
                      ALLOWED_RECORDS.SUB_DIVISION,
                      PROJECTS_V.PROJECT_TYPE    

                   ", RailroadId, UserId);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<CrossingData1>(sql).ToList();
            }
        }

        //public static IQueryable<CrossingData> GetSuppCrossingList(decimal RailroadId, Entities _context)
        //{
        //    return (from d in _context.CROSSINGS
        //            join r in _context.CROSSING_RELATIONSHIP on d.CROSSING_ID equals r.CROSSING_ID
        //            join p in _context.PROJECTS_V on r.PROJECT_ID equals p.PROJECT_ID
        //            //where (from c in _context.CROSSINGS
        //            //       select c.CROSSING_NUMBER).Distinct()
        //            where d.RAILROAD_ID == RailroadId

        //            select new CrossingData
        //            {
        //                RAILROAD_ID = d.RAILROAD_ID,
        //                CONTACT_ID = d.CONTACT_ID,
        //                CROSSING_ID = d.CROSSING_ID,
        //                CROSSING_NUMBER = d.CROSSING_NUMBER,
        //                SERVICE_UNIT = d.SERVICE_UNIT,
        //                SUB_DIVISION = d.SUB_DIVISION,
        //                CONTACT_NAME = d.CROSSING_CONTACTS.CONTACT_NAME,
        //                PROJECT_TYPE = p.PROJECT_TYPE,
        //                CARRYING_OUT_ORGANIZATION_ID = p.CARRYING_OUT_ORGANIZATION_ID,
        //                PROJECT_STATUS_CODE = p.PROJECT_STATUS_CODE,
        //                TEMPLATE_FLAG = p.TEMPLATE_FLAG,
        //                PROJECT_ID = p.PROJECT_ID,
        //                STATE = d.STATE,
        //                ORGANIZATION_NAME = p.ORGANIZATION_NAME
        //            }).Distinct();
                      
        //}
        public static IQueryable<ApplicationList> GetApplications( Entities _context)
        {
           
            return (from a in _context.CROSSING_APPLICATION
                    join c in _context.CROSSINGS on a.CROSSING_ID equals c.CROSSING_ID
                    select new ApplicationList { CROSSING_NUMBER = c.CROSSING_NUMBER, CROSSING_ID = a.CROSSING_ID, APPLICATION_ID = a.APPLICATION_ID, APPLICATION_NUMBER = a.APPLICATION_NUMBER,
                    APPLICATION_REQUESTED = a.APPLICATION_REQUESTED, APPLICATION_DATE = a.APPLICATION_DATE, TRUCK_NUMBER = a.TRUCK_NUMBER, SPRAY = a.SPRAY, CUT = a.CUT, INSPECT = a.INSPECT,
                    REMARKS = a.REMARKS });
        }
          public static IQueryable<SupplementalList> GetSupplementals( Entities _context)
        {
            return (from s in _context.CROSSING_SUPPLEMENTAL
                    join c in _context.CROSSINGS on s.CROSSING_ID equals c.CROSSING_ID
                    select new SupplementalList { CROSSING_NUMBER = c.CROSSING_NUMBER, CROSSING_ID = s.CROSSING_ID, SUPPLEMENTAL_ID = s.SUPPLEMENTAL_ID, APPROVED_DATE = s.APPROVED_DATE,
                    CUT_DATE = s.CUT_TIME, SERVICE_TYPE = s.SERVICE_TYPE, INSPECT_START = s.INSPECT_START, INSPECT_END = s.INSPECT_END, SQUARE_FEET = s.SQUARE_FEET, TRUCK_NUMBER = s.TRUCK_NUMBER,
                    SPRAY = s.SPRAY, CUT = s.CUT, INSPECT = s.INSPECT, MAINTAIN = s.MAINTAIN, RECURRING = s.RECURRING, REMARKS = s.REMARKS });

        }
          public static IQueryable<IncidentList> GetIncidents( Entities _context)
          {
              return (from i in _context.CROSSING_INCIDENT
                      join c in _context.CROSSINGS on i.CROSSING_ID equals c.CROSSING_ID
                      //where i.CROSSING_ID == CrossingId
                      select new IncidentList { CROSSING_NUMBER = c.CROSSING_NUMBER, CROSSING_ID = i.CROSSING_ID, INCIDENT_ID = i.INCIDENT_ID, INCIDENT_NUMBER = i.INCIDENT_NUMBER,
                      DATE_REPORTED = i.DATE_REPORTED, DATE_CLOSED = i.DATE_CLOSED, SLOW_ORDER = i.SLOW_ORDER, REMARKS = i.REMARKS });

          }
          public static IQueryable<CompletedCrossings> GetCompletedCrossings(decimal RailroadId, decimal Application, Entities _context)
          {
              return (from a in _context.CROSSING_APPLICATION
                      join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                      where d.RAILROAD_ID == RailroadId && a.APPLICATION_REQUESTED == Application
                      select new CompletedCrossings
                      {
                         CROSSING_ID = d.CROSSING_ID,
                         APPLICATION_ID = a.APPLICATION_ID,
                         APPLICATION_DATE = a.APPLICATION_DATE,
                         APPLICATION_REQUESTED = a.APPLICATION_REQUESTED,
                         CROSSING_NUMBER = d.CROSSING_NUMBER,
                         SUB_DIVISION = d.SUB_DIVISION,
                         SERVICE_UNIT = d.SERVICE_UNIT,
                         STATE = d.STATE,
                         MILE_POST = d.MILE_POST,
                       
                      });

          }
         
          public static IQueryable<CompletedCrossingsSupplemental> GetCompletedCrossingsSupplemental(decimal RailroadId, Entities _context)
          {
              return (from a in _context.CROSSING_SUPPLEMENTAL
                      join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                      where d.RAILROAD_ID == RailroadId
                      select new CompletedCrossingsSupplemental
                      {
                          CROSSING_ID = d.CROSSING_ID,
                          SUPPLEMENTAL_ID = a.SUPPLEMENTAL_ID,
                          APPROVED_DATE = a.APPROVED_DATE,
                          CROSSING_NUMBER = d.CROSSING_NUMBER,
                          SUB_DIVISION = d.SUB_DIVISION,
                          SERVICE_UNIT = d.SERVICE_UNIT,
                          STATE = d.STATE,
                          SERVICE_TYPE = a.SERVICE_TYPE,
                          MILE_POST = d.MILE_POST,
                          TRUCK_NUMBER = a.TRUCK_NUMBER,
                          SQUARE_FEET = a.SQUARE_FEET,
                          REMARKS = a.REMARKS,
                      });

          }
          public static IQueryable<StateCrossingList> GetStateCrossingList(decimal RailroadId, decimal Application, Entities _context)
          {
              return (from d in _context.CROSSINGS
                      join a in _context.CROSSING_APPLICATION on d.CROSSING_ID equals a.CROSSING_ID
                      where d.RAILROAD_ID == RailroadId && a.APPLICATION_REQUESTED == Application && d.STATUS != "DELETED"
                      select new StateCrossingList
                            {
                                CROSSING_ID = d.CROSSING_ID,
                                CROSSING_NUMBER = d.CROSSING_NUMBER,
                                SUB_DIVISION = d.SUB_DIVISION,
                                STATE = d.STATE,
                                COUNTY = d.COUNTY,
                                SERVICE_UNIT = d.SERVICE_UNIT,
                                CITY = d.CITY,
                                MILE_POST = d.MILE_POST,
                                ROWNE = d.ROWNE,
                                ROWNW = d.ROWNW,
                                ROWSE = d.ROWSE,
                                ROWSW = d.ROWSW,
                                STREET = d.STREET,
                                STATUS = d.STATUS,
                                SUB_CONTRACTED = d.SUB_CONTRACTED,
                                LONGITUDE = d.LONGITUDE,
                                LATITUDE = d.LATITUDE,
                                SPECIAL_INSTRUCTIONS = d.SPECIAL_INSTRUCTIONS,
                                SPRAY = a.SPRAY,
                                CUT = a.CUT,
                                INSPECT = a.INSPECT,
                                APPLICATION_ID = a.APPLICATION_ID,
                                APPLICATION_REQUESTED = a.APPLICATION_REQUESTED
                            });
          }
          public static IQueryable<ApplicationDateList> GetAppDate(decimal RailroadId, decimal Application, Entities _context)
          {
              return (from d in _context.CROSSING_APPLICATION
                      join c in _context.CROSSINGS on d.CROSSING_ID equals c.CROSSING_ID
                      where c.RAILROAD_ID == RailroadId && d.APPLICATION_REQUESTED == Application
                      select new ApplicationDateList
                      {
                         CROSSING_ID = d.CROSSING_ID,
                         CROSSING_NUMBER = c.CROSSING_NUMBER,
                         SUB_DIVISION = c.SUB_DIVISION,
                         SERVICE_UNIT = c.SERVICE_UNIT,
                         STATE = c.STATE,
                         MILE_POST = c.MILE_POST,
                         REMARKS = d.REMARKS,
                         APPLICATION_DATE = d.APPLICATION_DATE,
                         TRUCK_NUMBER = d.TRUCK_NUMBER,
                         APPLICATION_ID = d.APPLICATION_ID,
                         APPLICATION_REQUESTED = d.APPLICATION_REQUESTED,
                      });
          }
          public static IQueryable<ApplicationDateList> GetInspections(decimal RailroadId, decimal Application, Entities _context)
          {
              return (from d in _context.CROSSING_APPLICATION
                      join c in _context.CROSSINGS on d.CROSSING_ID equals c.CROSSING_ID
                      where c.RAILROAD_ID == RailroadId && d.APPLICATION_REQUESTED == Application && d.INSPECT == "Y"
                      select new ApplicationDateList
                      {
                          CROSSING_ID = d.CROSSING_ID,
                          CROSSING_NUMBER = c.CROSSING_NUMBER,
                          SUB_DIVISION = c.SUB_DIVISION,
                          SERVICE_UNIT = c.SERVICE_UNIT,
                          STATE = c.STATE,
                          MILE_POST = c.MILE_POST,
                          REMARKS = d.REMARKS,
                          APPLICATION_DATE = d.APPLICATION_DATE,
                          TRUCK_NUMBER = d.TRUCK_NUMBER,
                          APPLICATION_ID = d.APPLICATION_ID,
                          APPLICATION_REQUESTED = d.APPLICATION_REQUESTED,
                      });
          }
          
          public static IQueryable<IncidentReportList> GetIncidentReport(decimal RailroadId, Entities _context)
          {
              return (from d in _context.CROSSINGS
                      join i in _context.CROSSING_INCIDENT on d.CROSSING_ID equals i.CROSSING_ID
                      where d.RAILROAD_ID == RailroadId
                      select new IncidentReportList
                      {
                         CROSSING_ID = d.CROSSING_ID,
                         INCIDENT_ID = i.INCIDENT_ID,
                         CROSSING_NUMBER = d.CROSSING_NUMBER,
                         SUB_DIVISION = d.SUB_DIVISION,
                         SERVICE_UNIT = d.SERVICE_UNIT,
                         STATE = d.STATE,
                         MILE_POST = d.MILE_POST,
                         REMARKS = i.REMARKS,
                         SLOW_ORDER = i.SLOW_ORDER,
                         INCIDENT_NUMBER = i.INCIDENT_NUMBER,
                         DATE_REPORTED = i.DATE_REPORTED,
                         DATE_CLOSED = i.DATE_CLOSED,
                      });

          }
          public static IQueryable<SupplementalBillingList> GetSupplementalBillingReport(decimal RailroadId, Entities _context)
          {
              return (from d in _context.CROSSINGS
                      join a in _context.CROSSING_SUPPLEMENTAL on d.CROSSING_ID equals a.CROSSING_ID
                      join r in _context.CROSSING_RELATIONSHIP on d.CROSSING_ID equals r.CROSSING_ID
                      join p in _context.PROJECTS_V on r.PROJECT_ID equals p.PROJECT_ID
                      where d.RAILROAD_ID == RailroadId
                      select new SupplementalBillingList
                      {
                         CROSSING_ID = d.CROSSING_ID,
                         SUPPLEMENTAL_ID = a.SUPPLEMENTAL_ID,
                         CROSSING_NUMBER = d.CROSSING_NUMBER,
                         SUB_DIVISION = d.SUB_DIVISION,
                         SERVICE_UNIT = d.SERVICE_UNIT,
                         STATE = d.STATE,
                         MILE_POST = d.MILE_POST,
                         SERVICE_TYPE = a.SERVICE_TYPE,
                         SQUARE_FEET = a.SQUARE_FEET,
                         APPROVED_DATE = a.APPROVED_DATE,
                         SPRAY = a.SPRAY,
                         CUT = a.CUT,
                         INSPECT = a.INSPECT,
                         SEGMENT1 = p.SEGMENT1,
                         SPECIAL_INSTRUCTIONS = d.SPECIAL_INSTRUCTIONS,

                      });

          }
          public static IQueryable<WeeklyWorkList> GetWeeklyWork(decimal RailroadId, Entities _context)
          {
              return (from d in _context.CROSSINGS
                               join a in _context.CROSSING_APPLICATION on d.CROSSING_ID equals a.CROSSING_ID
                               where d.RAILROAD_ID == RailroadId 
                               select new WeeklyWorkList
                               {
                                  CROSSING_ID = d.CROSSING_ID,
                                  APPLICATION_ID = a.APPLICATION_ID,
                                  CROSSING_NUMBER = d.CROSSING_NUMBER,
                                  SUB_DIVISION = d.SUB_DIVISION,
                                  SERVICE_UNIT = d.SERVICE_UNIT,
                                  STATE = d.STATE,
                                  MILE_POST = d.MILE_POST,
                                  APPLICATION_DATE = a.APPLICATION_DATE,
                                  SPRAY = a.SPRAY,
                                  CUT = a.CUT,
                                  INSPECT = a.INSPECT,                                  
                               });
          }
          public static IQueryable<StateCrossingList> GetPrivateCrossings(decimal RailroadId, Entities _context)
          {
              return (from d in _context.CROSSINGS
                      where d.RAILROAD_ID == RailroadId && d.PROPERTY_TYPE == "PRI"
                      select new StateCrossingList
                      {
                         CROSSING_ID = d.CROSSING_ID,
                         CROSSING_NUMBER = d.CROSSING_NUMBER,
                         SUB_DIVISION = d.SUB_DIVISION,
                         STATE = d.STATE,
                         SERVICE_UNIT = d.SERVICE_UNIT,
                         COUNTY = d.COUNTY,
                         CITY = d.CITY,
                         MILE_POST = d.MILE_POST,
                         ROWNE = d.ROWNE,
                         ROWNW = d.ROWNW,
                         ROWSE = d.ROWSE,
                         ROWSW = d.ROWSW,
                         STREET = d.STREET,
                         SUB_CONTRACTED = d.SUB_CONTRACTED,
                         LONGITUDE = d.LONGITUDE,
                         LATITUDE = d.LATITUDE,
                         SPECIAL_INSTRUCTIONS = d.SPECIAL_INSTRUCTIONS,
                      });

          }
          public static IQueryable<StateCrossingList> GetMissingROW(decimal RailroadId, Entities _context)
          {
              return (from d in _context.CROSSINGS
                      where d.RAILROAD_ID == RailroadId && d.ROWNE == 0 && d.ROWNW == 0 && d.ROWSE == 0 && d.ROWSW == 0
                      select new StateCrossingList
                      {
                          CROSSING_ID = d.CROSSING_ID,
                          CROSSING_NUMBER = d.CROSSING_NUMBER,
                          SUB_DIVISION = d.SUB_DIVISION,
                          STATE = d.STATE,
                          SERVICE_UNIT = d.SERVICE_UNIT,
                          COUNTY = d.COUNTY,
                          CITY = d.CITY,
                          MILE_POST = d.MILE_POST,
                          ROWNE = d.ROWNE,
                          ROWNW = d.ROWNW,
                          ROWSE = d.ROWSE,
                          ROWSW = d.ROWSW,
                          STREET = d.STREET,
                          SUB_CONTRACTED = d.SUB_CONTRACTED,
                          LONGITUDE = d.LONGITUDE,
                          LATITUDE = d.LATITUDE,
                          SPECIAL_INSTRUCTIONS = d.SPECIAL_INSTRUCTIONS,
                      });

          }
            public class WeeklyWorkList
          {
              public string CROSSING_NUMBER { get; set; }
              public long APPLICATION_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public DateTime? APPLICATION_DATE { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public string STATE { get; set; }
              public decimal? MILE_POST { get; set; }
              public string SPRAY { get; set; }
              public string CUT { get; set; }
              public string INSPECT { get; set; }
          }
            public class SupplementalBillingList
            {
                public long SUPPLEMENTAL_ID { get; set; }
                public long CROSSING_ID { get; set; }
                public string CROSSING_NUMBER { get; set; }
                public DateTime? APPROVED_DATE { get; set; }
                public string SERVICE_TYPE { get; set; }
                public string SERVICE_UNIT { get; set; }
                public string SPRAY { get; set; }
                public decimal? SQUARE_FEET { get; set; }
                public string CUT { get; set; }
                public string MAINTAIN { get; set; }
                public string INSPECT { get; set; }
                public decimal? MILE_POST { get; set; }
                public string SUB_DIVISION { get; set; }
                public string SEGMENT1 { get; set; }
                public string STATE { get; set; }
                public string SPECIAL_INSTRUCTIONS { get; set; }
            }

          public class IncidentReportList
          {
              public long CROSSING_ID { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public long INCIDENT_ID { get; set; }
              public DateTime? DATE_CLOSED { get; set; }
              public string REMARKS { get; set; }
              public long INCIDENT_NUMBER { get; set; }
              public DateTime? DATE_REPORTED { get; set; }
              public string SLOW_ORDER { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public string STATE { get; set; }
              public decimal? MILE_POST { get; set; }

          }
          public class ApplicationDateList
          {
              public string CROSSING_NUMBER { get; set; }
              public long APPLICATION_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public decimal? APPLICATION_REQUESTED { get; set; }
              public DateTime? APPLICATION_DATE { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public string REMARKS { get; set; }
              public string TRUCK_NUMBER { get; set; }
              public string STATE { get; set; }
              public decimal? MILE_POST { get; set; }
          }
          public class StateCrossingList
          {
              public long? APPLICATION_ID { get; set; }
              public decimal? ROWNE { get; set; }
              public decimal? ROWNW { get; set; }
              public decimal? ROWSE { get; set; }
              public decimal? ROWSW { get; set; }
              public string STATE { get; set; }
              public long CROSSING_ID { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public decimal? MILE_POST { get; set; }
              public string SPRAY { get; set; }
              public string CUT { get; set; }
              public string INSPECT { get; set; }
              public string SUB_CONTRACTED { get; set; }
              public decimal? LONGITUDE { get; set; }
              public decimal? LATITUDE { get; set; }
              public string SPECIAL_INSTRUCTIONS { get; set; }
              public decimal? APPLICATION_REQUESTED { get; set; }
              public string COUNTY { get; set; }
              public string CITY { get; set; }
              public string STREET { get; set; }
              public string STATUS { get; set; }

          }
          public class CompletedCrossingsSupplemental
          {
              public string CROSSING_NUMBER { get; set; }
              public long SUPPLEMENTAL_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public DateTime? APPROVED_DATE { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public string STATE { get; set; }
              public decimal? MILE_POST { get; set; }
              public string TRUCK_NUMBER { get; set; }
              public string SERVICE_TYPE { get; set; }
              public decimal? SQUARE_FEET { get; set; }
              public string REMARKS { get; set; }
          }
          public class CompletedCrossings
          {
              public string CROSSING_NUMBER { get; set; }
              public long APPLICATION_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public decimal? APPLICATION_REQUESTED { get; set; }
              public DateTime? APPLICATION_DATE { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public string STATE { get; set; }
              public decimal? MILE_POST { get; set; }
          }
          public class IncidentList
          {
              public long CROSSING_ID { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public long INCIDENT_ID { get; set; }
              public DateTime? DATE_CLOSED { get; set; }
              public string REMARKS { get; set; }
              public long INCIDENT_NUMBER { get; set; }
              public DateTime? DATE_REPORTED { get; set; }
              public string SLOW_ORDER { get; set; }

          }
        public class ApplicationList
        {
            public long APPLICATION_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public decimal APPLICATION_NUMBER { get; set; }
            public decimal? APPLICATION_REQUESTED { get; set; }
            public DateTime? APPLICATION_DATE { get; set; }
            public string TRUCK_NUMBER { get; set; }
            public long FISCAL_YEAR { get; set; }
            public string SPRAY { get; set; }
            public string CUT { get; set; }
            public string INSPECT { get; set; }
            public string REMARKS { get; set; }
        }
        public class SupplementalList
        {
            public long SUPPLEMENTAL_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public DateTime? APPROVED_DATE { get; set; }
            public DateTime? CUT_DATE { get; set; }
            public string SERVICE_TYPE { get; set; }
            public string TRUCK_NUMBER { get; set; }
            public DateTime? INSPECT_START { get; set; }
            public DateTime? INSPECT_END { get; set; }
            public string SPRAY { get; set; }
            public decimal? SQUARE_FEET { get; set; }
            public string CUT { get; set; }
            public string MAINTAIN { get; set; }
            public string INSPECT { get; set; }
            public string RECURRING { get; set; }
            public string REMARKS { get; set; }

        }

        public class ProjectList
        {
            public long PROJECT_ID { get; set; }
            public long RAILROAD_ID { get; set; }
            public string LONG_NAME { get; set; }
            public string SEGMENT1 { get; set; }
            public string PROJECT_TYPE { get; set; }
            public string TEMPLATE_FLAG { get; set; }
            public string PROJECT_STATUS_CODE { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public long CARRYING_OUT_ORGANIZATION_ID { get; set; }
            public long CROSSING_ID { get; set; }
        }
       
        public class CrossingList
        {
            public string RAILROAD_ID { get; set; }
            public long? CONTACT_ID { get; set; }
            public string STATUS { get; set; }
            public string STATE { get; set; }
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string CONTACT_NAME { get; set; }

        }
        public class CrossingAssignedList
        {
            public string RAILROAD { get; set; }
            public long? PROJECT_ID { get; set; }
            public long? CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string STATE { get; set; }
        }
        public class CrossingProject
        {
            public long? PROJECT_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public string LONG_NAME { get; set; }
            public string SEGMENT1 { get; set; }
        }
        public class StateList
        {
           
            public string STATE { get; set; }

        }
        public class ServiceUnitList
        {

            public long SERVICE_UNIT_ID { get; set; }
            public string SERVICE_UNIT_NAME { get; set; }

        }
        public class CrossingData
        {
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string DOT { get; set; }
            public long? PROJECT_ID { get; set; }
            public string STATE { get; set; }
            public decimal? CONTACT_ID { get; set; }
            public string CONTACT_NAME { get; set; }
            public decimal? RAILROAD_ID { get; set; }
            public string PROJECT_TYPE { get; set; }
            public string TEMPLATE_FLAG { get; set; }
            public string PROJECT_STATUS_CODE { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public string STATUS { get; set; }
            public long CARRYING_OUT_ORGANIZATION_ID { get; set; }
            public decimal? APPLICATION_REQUESTED { get; set; }
        }
        public class CrossingData1
        {
            public decimal? APPLICATION_REQUESTED { get; set; }
            public decimal? RAILROAD_ID { get; set; }
            public decimal? CONTACT_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public string STATUS { get; set; }
            public string STATE { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string PROPERTY_TYPE { get; set; }
            public long CARRYING_OUT_ORGANIZATION_ID { get; set; }
            public string PROJECT_STATUS_CODE { get; set; }
            public string TEMPLATE_FLAG { get; set; }
            public long? PROJECT_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public string PROJECT_TYPE { get; set; }
        }
    }
}
