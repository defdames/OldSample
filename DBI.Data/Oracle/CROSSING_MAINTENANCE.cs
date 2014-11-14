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
                    where d.RAILROAD_ID == RailroadId
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
        public static List<ProjectList> ApplicationProjectList()
        {
            using (Entities _context = new Entities())
            {
                var data = (from v in _context.PROJECTS_V
                        select new ProjectList { PROJECT_TYPE = v.PROJECT_TYPE, CARRYING_OUT_ORGANIZATION_ID = v.CARRYING_OUT_ORGANIZATION_ID, PROJECT_STATUS_CODE = v.PROJECT_STATUS_CODE, TEMPLATE_FLAG = v.TEMPLATE_FLAG, PROJECT_ID = v.PROJECT_ID, LONG_NAME = v.LONG_NAME, ORGANIZATION_NAME = v.ORGANIZATION_NAME, SEGMENT1 = v.SEGMENT1 }).ToList();
             
                return data;
            }
        }
        public static List<CrossingPricing> CrossingsPricingList()

        {
            using (Entities _context = new Entities())
            {
                var data = (from r in _context.CROSSING_PRICING
                            where !r.SERVICE_CATEGORY.StartsWith("Application")                     
                            select new CrossingPricing { PRICING_ID = r.PRICING_ID, SERVICE_CATEGORY = r.SERVICE_CATEGORY, PRICE = r.PRICE}).ToList();
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
        public static List<CrossingData1> GetAppCrossingList(decimal RailroadId, decimal UserId, decimal ProjectId)
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
  WHERE d.USER_ID = {1} AND b.PROJECT_ID = {2}
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

                   ", RailroadId, UserId, ProjectId);

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
                    WHERE ALLOWED_RECORDS.RAILROAD_ID = {0} AND PROJECTS_V.PROJECT_TYPE = 'CUSTOMER BILLING' AND PROJECTS_V.TEMPLATE_FLAG = 'N' AND PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND ALLOWED_RECORDS.STATUS = 'ACTIVE'
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
                    CUT_TIME = s.CUT_TIME, SERVICE_TYPE = s.SERVICE_TYPE, INSPECT_START = s.INSPECT_START, INSPECT_END = s.INSPECT_END, SQUARE_FEET = s.SQUARE_FEET, TRUCK_NUMBER = s.TRUCK_NUMBER,
                    SPRAY = s.SPRAY, CUT = s.CUT, INSPECT = s.INSPECT, MAINTAIN = s.MAINTAIN, RECURRING = s.RECURRING, REMARKS = s.REMARKS });

        }
          public static IQueryable<IncidentList> GetIncidents( Entities _context)
          {
              return (from i in _context.CROSSING_INCIDENT
                      join c in _context.CROSSINGS on i.CROSSING_ID equals c.CROSSING_ID
                      select new IncidentList { CROSSING_NUMBER = c.CROSSING_NUMBER, CROSSING_ID = i.CROSSING_ID, INCIDENT_ID = i.INCIDENT_ID, INCIDENT_NUMBER = i.INCIDENT_NUMBER,
                      DATE_REPORTED = i.DATE_REPORTED, DATE_CLOSED = i.DATE_CLOSED, SLOW_ORDER = i.SLOW_ORDER, REMARKS = i.REMARKS });

          }
          public static IQueryable<CompletedCrossings> GetCompletedCrossings(decimal RailroadId, decimal Application, Entities _context)
          {
              return (from a in _context.CROSSING_APPLICATION
                      join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                      join v in _context.PROJECTS_V on a.PROJECT_ID equals v.PROJECT_ID
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
                         SUB_CONTRACTED = d.SUB_CONTRACTED,
                         PROJECT_ID = a.PROJECT_ID,
                         SEGMENT1 = v.SEGMENT1
                      });

          }
          public static IQueryable<CompletedCrossings> GetCompletedCrossingsBilling(decimal RailroadId, decimal Application, long? ProjectId, Entities _context)
          {
              return (from a in _context.CROSSING_APPLICATION
                      join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                      join r in _context.CROSSING_RELATIONSHIP on d.CROSSING_ID equals r.CROSSING_ID
                      where d.RAILROAD_ID == RailroadId && a.APPLICATION_REQUESTED == Application && r.PROJECT_ID == ProjectId
                      select new CompletedCrossings
                      {
                          PROJECT_ID = r.PROJECT_ID,
                          CROSSING_ID = d.CROSSING_ID,
                          APPLICATION_ID = a.APPLICATION_ID,
                          APPLICATION_DATE = a.APPLICATION_DATE,
                          APPLICATION_REQUESTED = a.APPLICATION_REQUESTED,
                          CROSSING_NUMBER = d.CROSSING_NUMBER,
                          SUB_DIVISION = d.SUB_DIVISION,
                          SERVICE_UNIT = d.SERVICE_UNIT,
                          STATE = d.STATE,
                          MILE_POST = d.MILE_POST,
                          SUB_CONTRACTED = d.SUB_CONTRACTED,
                      });

          }
          public static IQueryable<CompletedCrossingsSupplemental> GetCompletedCrossingsSupplemental(decimal RailroadId, Entities _context)
          {
              return (from a in _context.CROSSING_SUPPLEMENTAL
                      join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                      join v in _context.PROJECTS_V on a.PROJECT_ID equals v.PROJECT_ID
                      join p in _context.CROSSING_PRICING on a.SERVICE_TYPE equals p.SERVICE_CATEGORY
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
                          SEGMENT1 = v.SEGMENT1,
                          PRICE = p.PRICE,
                      });

          }
          public static IQueryable<StateCrossingList> GetStateCrossingList(decimal RailroadId, Entities _context)
          {
              return (from d in _context.CROSSINGS
                      where d.RAILROAD_ID == RailroadId && d.STATUS != "DELETED" && d.PROPERTY_TYPE == "PUB"
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
                      join p in _context.PROJECTS_V on a.PROJECT_ID equals p.PROJECT_ID
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

          public static List<ApplicationDetails> GetInvoiceReport(string selectedApp)
          {
              long selected = Convert.ToInt64(selectedApp);
          
              string sql = string.Format(@"                           

                                SELECT
                                   a.INVOICE_ID,
                                   v.INVOICE_NUMBER,
                                   v.INVOICE_DATE,
                                   d.CROSSING_ID,
                                   a.APPLICATION_ID,
                                   a.APPLICATION_DATE,
                                   a.APPLICATION_REQUESTED,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.MILE_POST,
                                   d.SERVICE_UNIT,
                                   p.PROJECT_ID,
                                   p.SEGMENT1,
                                   d.STATE
                FROM CROSSING_APPLICATION a
                LEFT JOIN CROSSINGS d ON a.CROSSING_ID = d.CROSSING_ID
                LEFT JOIN CROSSING_INVOICE v ON a.INVOICE_ID = v.INVOICE_ID
                LEFT JOIN PROJECTS_V p ON a.PROJECT_ID = p.PROJECT_ID
                WHERE a.INVOICE_ID = {0}

                   ", selectedApp);

              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<ApplicationDetails>(sql).ToList();
              }
          }
          public static List<SupplementalReport> GetSupplementalReport(string selectedSupp)
          {
              long selected = Convert.ToInt64(selectedSupp);
            
              string sql = string.Format(@"                           

                             SELECT
                                   v.INVOICE_SUPP_NUMBER,
                                   v.INVOICE_SUPP_DATE,
                                   v.INVOICE_SUPP_ID,
                                   a.APPROVED_DATE,
                                   a.SQUARE_FEET,
                                   a.SUPPLEMENTAL_ID,
                                   a.SERVICE_TYPE,
                                   a.TRUCK_NUMBER,
                                   d.CROSSING_ID,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.MILE_POST,
                                   d.SERVICE_UNIT,
                                   p.PROJECT_ID,
                                   p.SEGMENT1,
                                   pr.PRICE,
                                   d.STATE
                FROM CROSSING_SUPPLEMENTAL a
                LEFT JOIN CROSSINGS d ON a.CROSSING_ID = d.CROSSING_ID
                LEFT JOIN CROSSING_SUPP_INVOICE v ON a.INVOICE_SUPP_ID = v.INVOICE_SUPP_ID
                LEFT JOIN PROJECTS_V p ON a.PROJECT_ID = p.PROJECT_ID
                LEFT JOIN CROSSING_PRICING pr ON a.SERVICE_TYPE = pr.SERVICE_CATEGORY
                WHERE a.INVOICE_SUPP_ID = {0}

                   ", selectedSupp);

              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<SupplementalReport>(sql).ToList();
              }
          }
          public static List<StateCrossingList> GetStateCrossingList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState, string selectedSpray)
          {
             
              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.COUNTY,
                                d.SERVICE_UNIT,
                                d.CITY,
                                d.MILE_POST,
                                d.ROWNE,
                                d.ROWNW,
                                d.ROWSE,
                                d.ROWSW,
                                d.STREET,
                                d.STATUS,
                                d.SUB_CONTRACTED,
                                d.LONGITUDE,
                                d.LATITUDE,
                                d.SPECIAL_INSTRUCTIONS,
                                a.SPRAY
                FROM CROSSINGS d 
                LEFT JOIN CROSSING_APPLICATION a ON d.CROSSING_ID = a.CROSSING_ID");
              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
            
              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
             string sql3 = "";
             if (selectedSpray == "Sprayed")
             {
                 sql3 = string.Format(@" AND a.SPRAY = 'Y'

                   ", selectedSpray);
             }
             else if (selectedSpray == "Not Sprayed")
             {
                 sql3 = string.Format(@" AND a.SPRAY = 'N'

                   ", selectedSpray);
             }
             
                   string sql = sql1 + sql2 + sql3;
                          using (Entities context = new Entities())
                          {
                              return context.Database.SqlQuery<StateCrossingList>(sql).ToList();
                          }
                      }

          public static List<StateCrossingList> GetCrossingSummaryList(string selectedRailroad, string selectedStart, string selectedEnd)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.COUNTY,
                                d.SERVICE_UNIT,
                                d.CITY,
                                d.MILE_POST,
                                d.ROWNE,
                                d.ROWNW,
                                d.ROWSE,
                                d.ROWSW,
                                d.STREET,
                                d.STATUS,
                                d.SUB_CONTRACTED,
                                d.LONGITUDE,
                                d.LATITUDE,
                                d.SPECIAL_INSTRUCTIONS,
                                a.SPRAY
                FROM CROSSINGS d 
                LEFT JOIN CROSSING_APPLICATION a ON d.CROSSING_ID = a.CROSSING_ID");
              string sql2 = "";
              if (selectedStart == "Y")
              {
                  sql2 = string.Format(@"
                     AND i.DATE_CLOSED is null 
                     ", selectedStart);
              }
              else if (selectedEnd == "Y")
              {
                  sql2 = string.Format(@"
                      AND i.DATE_CLOSED is not null
                                  
                      ", selectedEnd);
              }
             

              string sql = sql1 + sql2;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<StateCrossingList>(sql).ToList();
              }
          }

          public static List<ApplicationDateList> GetAppDateList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState, decimal selectedApplication, DateTime selectedStart, DateTime selectedEnd)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.SERVICE_UNIT,
                                d.MILE_POST,
                                d.REMARKS,
                                d.STREET,
                                d.CITY,
                                a.TRUCK_NUMBER,
                                a.APPLICATION_ID,
                                a.APPLICATION_REQUESTED,
                                a.APPLICATION_DATE,
                                a.SPRAY,
                                a.CUT,
                                a.INSPECT
                FROM CROSSING_APPLICATION a
                LEFT JOIN CROSSINGS d ON a.CROSSING_ID = d.CROSSING_ID 
                 ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0} AND d.STATE = '{3}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE  d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PUB' AND d.RAILROAD_ID = {0}
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);
              }
              string sql3 = "";
              if (selectedStart != DateTime.MinValue && selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE >= ('{0}') AND a.APPLICATION_DATE <= ('{1}')

                   ", selectedStart.ToString("dd-MMM-yyyy"), selectedEnd.ToString("dd-MMM-yyyy"));
              }
              else if (selectedStart != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE >= ('{0}')

                   ", selectedStart.ToString("dd-MMM-yyyy"));
              }

              else if (selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE <= ('{0}')

                   ", selectedEnd.ToString("dd-MMM-yyyy"));
              }
              string sql = sql1 + sql2 + sql3;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<ApplicationDateList>(sql).ToList();
              }
          }

          public static List<ApplicationDateList> GetInspectionList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState, decimal selectedApplication, DateTime selectedStart, DateTime selectedEnd)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.SERVICE_UNIT,
                                d.MILE_POST,
                                d.STREET,
                                d.CITY,
                                a.REMARKS,                                
                                a.TRUCK_NUMBER,
                                a.APPLICATION_ID,
                                a.APPLICATION_REQUESTED,
                                a.APPLICATION_DATE,
                                a.SPRAY,
                                a.CUT,
                                a.INSPECT
                FROM CROSSING_APPLICATION a
                LEFT JOIN CROSSINGS d ON a.CROSSING_ID = d.CROSSING_ID ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.STATE = '{3}'
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE a.INSPECT = 'Y' AND d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0}
                 AND a.APPLICATION_REQUESTED = {4}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState, selectedApplication);
              }
              string sql3 = "";
              if (selectedStart != DateTime.MinValue && selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE >= ('{0}') AND a.APPLICATION_DATE <= ('{1}')

                   ", selectedStart.ToString("dd-MMM-yyyy"), selectedEnd.ToString("dd-MMM-yyyy"));
              }
              else if (selectedStart != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE >= ('{0}')

                   ", selectedStart.ToString("dd-MMM-yyyy"));
              }

              else if (selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE <= ('{0}')

                   ", selectedEnd.ToString("dd-MMM-yyyy"));
              }

              string sql = sql1 + sql2 + sql3;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<ApplicationDateList>(sql).ToList();
              }
          }

          public static List<IncidentReportList> GetIncidentList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState, DateTime selectedStart, DateTime selectedEnd, string selectedOpen, string selectedClosed)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.SERVICE_UNIT,
                                d.MILE_POST,
                                i.REMARKS,                                
                                i.INCIDENT_NUMBER,
                                i.INCIDENT_ID,
                                i.SLOW_ORDER,
                                i.DATE_REPORTED,
                                i.DATE_CLOSED
                FROM CROSSING_INCIDENT i
                LEFT JOIN CROSSINGS d ON i.CROSSING_ID = d.CROSSING_ID ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'
             

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0}
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              string sql3 = "";
              if (selectedStart != DateTime.MinValue && selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND i.DATE_REPORTED >= ('{0}') AND i.DATE_REPORTED <= ('{1}')

                   ", selectedStart.ToString("dd-MMM-yyyy"), selectedEnd.ToString("dd-MMM-yyyy"));
              }
              else if (selectedStart != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND i.DATE_REPORTED >= ('{0}')

                   ", selectedStart.ToString("dd-MMM-yyyy"));
              }

              else if (selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND i.DATE_REPORTED <= ('{0}')

                   ", selectedEnd.ToString("dd-MMM-yyyy"));
              }

                 string sql4 = "";
                 if (selectedOpen == "Y")
                 {
                     sql4 = string.Format(@"
                     AND i.DATE_CLOSED is null 
                     ", selectedOpen);
                     }
                 else if (selectedClosed == "Y")
                   {
                      sql4 = string.Format(@"
                      AND i.DATE_CLOSED is not null
                                  
                      ", selectedClosed);
                   }

              string sql = sql1 + sql2 + sql3 + sql4;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<IncidentReportList>(sql).ToList();
              }
          }

          public static List<SupplementalReport> GetSupplementalReportList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState, DateTime selectedStart, DateTime selectedEnd, string selectedType)
          {

              string sql1 = string.Format(@"                          
                              SELECT
                                   v.INVOICE_SUPP_NUMBER,
                                   v.INVOICE_SUPP_DATE,
                                   v.INVOICE_SUPP_ID,
                                   a.APPROVED_DATE,
                                   a.SQUARE_FEET,
                                   a.SUPPLEMENTAL_ID,
                                   a.SERVICE_TYPE,
                                   a.TRUCK_NUMBER,
                                   d.CROSSING_ID,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.MILE_POST,
                                   d.SERVICE_UNIT,
                                   p.PROJECT_ID,
                                   p.SEGMENT1,
                                   pr.PRICE,
                                   d.STATE
                FROM CROSSING_SUPPLEMENTAL a
                LEFT JOIN CROSSINGS d ON a.CROSSING_ID = d.CROSSING_ID
                LEFT JOIN CROSSING_SUPP_INVOICE v ON a.INVOICE_SUPP_ID = v.INVOICE_SUPP_ID
                LEFT JOIN PROJECTS_V p ON a.PROJECT_ID = p.PROJECT_ID
                LEFT JOIN CROSSING_PRICING pr ON a.SERVICE_TYPE = pr.SERVICE_CATEGORY ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'
                 

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0} AND d.STATE = '{3}'
                

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.RAILROAD_ID = {0}
               

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              string sql3 = "";
              if (selectedStart != DateTime.MinValue && selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPROVED_DATE >= ('{0}') AND a.APPROVED_DATE <= ('{1}')

                   ", selectedStart.ToString("dd-MMM-yyyy"), selectedEnd.ToString("dd-MMM-yyyy"));
              }
              else if (selectedStart != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPROVED_DATE >= ('{0}')

                   ", selectedStart.ToString("dd-MMM-yyyy"));
              }

              else if (selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPROVED_DATE <= ('{0}')

                   ", selectedEnd.ToString("dd-MMM-yyyy"));
              }

              string sql4 = "";
              if (selectedType == "Approved/Not Complete")
              {
                  sql4 = string.Format(@" 
                  AND a.CUT_TIME IS NULL

                   ", selectedType);
              }
              else if (selectedType == "Complete Crossings")
              {
                  sql4 = string.Format(@" 
                  AND a.CUT_TIME IS NOT NULL

                   ", selectedType);
              }
           
              string sql = sql1 + sql2 + sql3 + sql4;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<SupplementalReport>(sql).ToList();
              }
          }
          public static List<StateCrossingList> GetPrivateCrossingList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.COUNTY,
                                d.SERVICE_UNIT,
                                d.CITY,
                                d.MILE_POST,
                                d.ROWNE,
                                d.ROWNW,
                                d.ROWSE,
                                d.ROWSW,
                                d.STREET,
                                d.STATUS,
                                d.SUB_CONTRACTED,
                                d.LONGITUDE,
                                d.LATITUDE,
                                d.SPECIAL_INSTRUCTIONS
                FROM CROSSINGS d ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0} AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.PROPERTY_TYPE = 'PRI' AND d.RAILROAD_ID = {0}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              string sql = sql1 + sql2;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<StateCrossingList>(sql).ToList();
              }
          }

          public static List<StateCrossingList> GetMissingROWList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                d.COUNTY,
                                d.SERVICE_UNIT,
                                d.CITY,
                                d.MILE_POST,
                                d.ROWNE,
                                d.ROWNW,
                                d.ROWSE,
                                d.ROWSW,
                                d.STREET,
                                d.STATUS,
                                d.SUB_CONTRACTED,
                                d.LONGITUDE,
                                d.LATITUDE,
                                d.SPECIAL_INSTRUCTIONS
                FROM CROSSINGS d ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0 AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0 AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0 AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0  AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0 AND d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0 AND d.RAILROAD_ID = {0} AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.ROWNE = 0 AND d.ROWNW = 0 AND d.ROWSE = 0 AND d.ROWSW = 0 AND d.RAILROAD_ID = {0}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              string sql = sql1 + sql2;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<StateCrossingList>(sql).ToList();
              }
          }

          public static List<WeeklyWorkList> GetWeeklyWorkList(string selectedRailroad, string selectedServiceUnit, string selectedSubDiv, string selectedState, DateTime selectedStart, DateTime selectedEnd)
          {

              string sql1 = string.Format(@"                          
                             SELECT
                                d.CROSSING_ID,
                                d.CROSSING_NUMBER,
                                d.SUB_DIVISION,
                                d.STATE,
                                a.APPLICATION_DATE,
                                a.SPRAY,
                                a.CUT,
                                a.INSPECT,
                                d.SERVICE_UNIT,                       
                                d.MILE_POST
                FROM CROSSING_APPLICATION a
                LEFT JOIN CROSSINGS d ON a.CROSSING_ID = d.CROSSING_ID ");

              string sql2 = "";
              if (selectedServiceUnit != null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }

              else if (selectedServiceUnit != null && selectedSubDiv != null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.SUB_DIVISION = '{2}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit != null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SERVICE_UNIT = '{1}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }

              else if (selectedServiceUnit == null && selectedSubDiv != null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.SUB_DIVISION = '{2}' AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState != null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0} AND d.STATE = '{3}'

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);

              }
              else if (selectedServiceUnit == null && selectedSubDiv == null && selectedState == null)
              {
                  sql2 = string.Format(@" 
                WHERE d.STATUS = 'ACTIVE' AND d.RAILROAD_ID = {0}

                   ", selectedRailroad, selectedServiceUnit, selectedSubDiv, selectedState);
              }
              string sql3 = "";
              if (selectedStart != DateTime.MinValue && selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE >= ('{0}') AND a.APPLICATION_DATE <= ('{1}')

                   ", selectedStart.ToString("dd-MMM-yyyy"), selectedEnd.ToString("dd-MMM-yyyy"));
              }
              else if (selectedStart != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE >= ('{0}')

                   ", selectedStart.ToString("dd-MMM-yyyy"));
              }

              else if (selectedEnd != DateTime.MinValue)
              {
                  sql3 = string.Format(@" 
                AND a.APPLICATION_DATE <= ('{0}')

                   ", selectedEnd.ToString("dd-MMM-yyyy"));
              }
              string sql = sql1 + sql2 + sql3;
              using (Entities context = new Entities())
              {
                  return context.Database.SqlQuery<WeeklyWorkList>(sql).ToList();
              }
          }
          public class SupplementalReport
          {
              public decimal? INVOICE_SUPP_ID { get; set; }
              public decimal? SUPPLEMENTAL_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public long PROJECT_ID { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public DateTime APPROVED_DATE { get; set; }
              public string SERVICE_TYPE { get; set; }
              public string TRUCK_NUMBER { get; set; }
              public string STATE { get; set; }
              public long SQUARE_FEET { get; set; }
              public string SEGMENT1 { get; set; }
              public decimal? PRICE { get; set; }
              public string INVOICE_SUPP_NUMBER { get; set; }
              public DateTime? INVOICE_SUPP_DATE { get; set; }
              public string SUB_DIVISION { get; set; }
              public string SERVICE_UNIT { get; set; }
              public decimal? MILE_POST { get; set; }
      
          }
          public class ApplicationDetails
          {
              public decimal? INVOICE_ID { get; set; }
              public long APPLICATION_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public decimal? APPLICATION_REQUESTED { get; set; }
              public DateTime? APPLICATION_DATE { get; set; }
              public string INVOICE_NUMBER { get; set; }
              public DateTime? INVOICE_DATE { get; set; }
              public decimal? MILE_POST { get; set; }
              public string SUB_DIVISION { get; set; }
              public string SEGMENT1 { get; set; }
              public string STATE { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public string SERVICE_UNIT { get; set; }
              public long? PROJECT_ID { get; set; }
          }
          public class SupplementalDetails
          {
              public decimal? INVOICE_SUPP_ID { get; set; }
              public decimal SUPPLEMENTAL_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public DateTime APPROVED_DATE { get; set; }
              public string SERVICE_TYPE { get; set; }
              public string TRUCK_NUMBER { get; set; }
              public long SQUARE_FEET { get; set; }


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
              public string INCIDENT_NUMBER { get; set; }
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
              public string SPRAY { get; set; }
              public string CUT { get; set; }
              public string INSPECT { get; set; }
              public string STREET { get; set; }
              public string CITY { get; set; }
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
              public long RAILROAD_ID { get; set; }
          }
          public class CompletedCrossingsSupplemental
          {
              public decimal? PRICE { get; set; }
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
              public string SEGMENT1 { get; set; }
          }
          public class CompletedCrossings
          {
              public string SEGMENT1 { get; set; }
              public long? PROJECT_ID { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public long APPLICATION_ID { get; set; }
              public long CROSSING_ID { get; set; }
              public decimal? APPLICATION_REQUESTED { get; set; }
              public DateTime? APPLICATION_DATE { get; set; }
              public string SERVICE_UNIT { get; set; }
              public string SUB_DIVISION { get; set; }
              public string STATE { get; set; }
              public decimal? MILE_POST { get; set; }
              public string SUB_CONTRACTED { get; set; }
          }
          public class IncidentList
          {
              public long CROSSING_ID { get; set; }
              public string CROSSING_NUMBER { get; set; }
              public long INCIDENT_ID { get; set; }
              public DateTime? DATE_CLOSED { get; set; }
              public string REMARKS { get; set; }
              public string INCIDENT_NUMBER { get; set; }
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
            public DateTime? CUT_TIME { get; set; }
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
        public class CrossingPricing
        {
            public decimal PRICING_ID { get; set; }
            public decimal? PRICE{ get; set; }
            public string SERVICE_CATEGORY { get; set; }
     
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
