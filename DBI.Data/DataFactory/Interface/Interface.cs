using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class Interface
    {
        public static long generateDailyActivityHeaderSequence()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_DAILYACT_HEADERID_S.NEXTVAL from dual";
                long query = _context.Database.SqlQuery<long>(sql).First();
                return query;
            }
        }

        public static long generateLaborHeaderSequence()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_LABOR_HEADERID_S.NEXTVAL from dual";
                long query = _context.Database.SqlQuery<long>(sql).First();
                return query;
            }
        }

        public static long generateInventorySequence()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_MTLTRANSID_S.NEXTVAL from dual";
                long query = _context.Database.SqlQuery<long>(sql).First();
                return query;
            }
        }


        public IQueryable dailyActivityRecordById(long dailyActivityHeaderId)
        {
            using (Entities _context = new Entities())
            {
                var query = from h in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                            join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
                            where h.HEADER_ID == dailyActivityHeaderId
                            select new { h, p, l, u };
                return query;
            }
        }



        //public static void insertDailyActivityHeader(long dailyActivityHeaderId, out long headerId)
        //{
        //    try
        //    {
        //        using (Entities _context = new Entities())
        //        {
        //            var query = from h in _context.DAILY_ACTIVITY_HEADER
        //                        join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
        //                        join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
        //                        join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
        //                        where h.HEADER_ID == dailyActivityHeaderId
        //                        select new { h, p, l, u };

        //            var data = query.Single();

        //            XXDBI_DAILY_ACTIVITY_HEADER header = new XXDBI_DAILY_ACTIVITY_HEADER();
        //            headerId = generateDailyActivityHeaderSequence();
        //            header.DA_HEADER_ID = headerId;
        //            header.STATE = data.l.REGION;
        //            header.COUNTY = "NONE";
        //            header.ACTIVITY_DATE = (DateTime)data.h.DA_DATE;
        //            header.ORG_ID = (Decimal)data.p.ORG_ID;
        //        header.PROJECT_NUMBER = data.p.SEGMENT1;
        //        header.PROJECT_NAME = data.p.NAME;
        //        header.CREATED_BY = data.u.USER_ID;
        //        header.CREATION_DATE = DateTime.Now;
        //        header.LAST_UPDATED_BY = data.u.USER_ID;
        //        header.LAST_UPDATE_DATE = DateTime.Now;

        //        var columns = new[] { "STATE", "COUNTY", "ACTIVITY_DATE", "ORG_ID", "PROJECT_NUMBER", "PROJECT_NAME", "CREATED_BY", "CREATION_DATE", "LAST_UPDATED_BY", "LAST_UPDATE_DATE" };
        //        GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header, columns, "XXDBI.XXDBI_DAILY_ACTIVITY_HEADER");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw(ex);
        //    }
        //}

        //public static void insertDailyActivityLaborRecords(long dailyActivityHeaderId, long daHeaderId)
        //{
        //    try
        //    {
        //        using (Entities _context = new Entities())
        //        {
        //            var query = from h in _context.DAILY_ACTIVITY_HEADER
        //                        join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
        //                        join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
        //                        join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
        //                        where h.HEADER_ID == dailyActivityHeaderId
        //                        select new { h, p, l, u };

        //            var data = query.Single();

        //            List<DAILY_ACTIVITY_EMPLOYEE> employees = data.h.DAILY_ACTIVITY_EMPLOYEE.ToList();
                    
        //            DAILY_ACTIVITY_PRODUCTION production = data.h.DAILY_ACTIVITY_PRODUCTION.FirstOrDefault();

        //            foreach (DAILY_ACTIVITY_EMPLOYEE employee in employees)
        //            {
        //                XXDBI_LABOR_HEADER header = new XXDBI_LABOR_HEADER();
        //                header.LABOR_HEADER_ID = generateLaborHeaderSequence();
        //                header.DA_HEADER_ID = daHeaderId;
        //                header.PROJECT_NUMBER = data.p.SEGMENT1;
        //                header.TASK_NUMBER = production

        //            }

     

        //            //var columns = new[] { "STATE", "COUNTY", "ACTIVITY_DATE", "ORG_ID", "PROJECT_NUMBER", "PROJECT_NAME", "CREATED_BY", "CREATION_DATE", "LAST_UPDATED_BY", "LAST_UPDATE_DATE" };
        //            //GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header, columns, "XXDBI.XXDBI_DAILY_ACTIVITY_HEADER");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}



        //public int GetNextPaTransID()
        //{
        //    int? transid = ObjectContext.PA_TRANSACTION_INTERFACE_CP.Max(u => (int?)u.TRANS_ID);
        //    if (transid == null)
        //    {
        //        transid = 0;
        //    }
        //    return int.Parse(transid.ToString());
        //}



    }
}




