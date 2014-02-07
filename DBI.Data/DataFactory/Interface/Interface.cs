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


        public static decimal payrollHoursCalculation(DateTime dateIn, DateTime dateOut)
        {
            TimeSpan span = dateOut.Subtract(dateIn);
            double calc = (span.Minutes > 0 && span.Minutes <= 8) ? 0
                         : (span.Minutes > 8 && span.Minutes <= 23) ? .25
                         : (span.Minutes > 23 && span.Minutes <= 38) ? .50
                         : (span.Minutes > 38 && span.Minutes <= 53) ? .75
                         : (span.Minutes > 53 && span.Minutes <= 60) ? 1
                         : 0;
            decimal returnValue = span.Hours + (decimal)calc;
            return returnValue;
        }


        public static List<XXDBI_LABOR_HEADER> laborInterfaceRecords(decimal dailyActivityHeaderId, decimal generatedHeaderID, DateTime daDate)
        {
            List<XXDBI_LABOR_HEADER> records = new List<XXDBI_LABOR_HEADER>();
            using (Entities _context = new Entities())
            {

                var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                            join u in _context.SYS_USER_INFORMATION on d.CREATED_BY equals u.USER_NAME
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join pro in _context.DAILY_ACTIVITY_PRODUCTION on d.HEADER_ID equals pro.HEADER_ID into prod
                            from production in prod.DefaultIfEmpty()
                            join tsk in _context.PA_TASKS_V on production.TASK_ID equals tsk.TASK_ID into tsks
                            from tasks in tsks.DefaultIfEmpty()
                            where d.HEADER_ID == dailyActivityHeaderId
                            select new
                            {
                                PROJECT_NUMBER = p.SEGMENT1,
                                TASK_NUMBER = tasks.TASK_NUMBER,
                                EMPLOYEE_NUMBER = e.EMPLOYEE_NUMBER,
                                EMP_FULL_NAME = e.EMPLOYEE_NAME,
                                ROLE = d.ROLE_TYPE,
                                STATE = l.REGION,
                                COUNTY = "NONE",
                                LAB_HEADER_DATE = daDate,
                                ELEMENT = "Time Entry Wages",
                                ADJUSTMENT = "N",
                                STATUS = "UNPROCESSED",
                                ORG_ID = (decimal)p.ORG_ID,
                                CREATED_BY = u.USER_ID,
                                LAST_UPDATED_BY = u.USER_ID,
                                TIME_IN = d.TIME_IN,
                                TIME_OUT = d.TIME_OUT
                            }).ToList();

                foreach (var r in data)
                {
                    XXDBI_LABOR_HEADER record = new XXDBI_LABOR_HEADER();
                    record.LABOR_HEADER_ID = DBI.Data.Interface.generateLaborHeaderSequence();
                    record.DA_HEADER_ID = generatedHeaderID;
                    record.PROJECT_NUMBER = r.PROJECT_NUMBER;
                    record.TASK_NUMBER = r.TASK_NUMBER;
                    record.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                    record.EMP_FULL_NAME = r.EMP_FULL_NAME;
                    record.ROLE = r.ROLE;
                    record.STATE = r.STATE;
                    record.COUNTY = r.COUNTY;
                    record.LAB_HEADER_DATE = daDate;
                    record.QUANTITY = payrollHoursCalculation((DateTime)r.TIME_IN, (DateTime)r.TIME_OUT);
                    record.ELEMENT = r.ELEMENT;
                    record.ADJUSTMENT = r.ADJUSTMENT;
                    record.STATUS = r.STATUS;
                    record.ORG_ID = r.ORG_ID;
                    record.CREATED_BY = r.CREATED_BY;
                    record.CREATION_DATE = DateTime.Now;
                    record.LAST_UPDATE_DATE = DateTime.Now;
                    record.LAST_UPDATED_BY = r.LAST_UPDATED_BY;
                    records.Add(record);
                }

                return records;
            }
        }

        public static XXDBI_DAILY_ACTIVITY_HEADER headerInterfaceRecords(long dailyActivityHeaderId)
        {
            using (Entities _context = new Entities())
            {

                 var query = from h in _context.DAILY_ACTIVITY_HEADER
                                join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                                join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
                                where h.HEADER_ID == dailyActivityHeaderId
                                select new { h, p, l, u };

                    var data = query.Single();

                    XXDBI_DAILY_ACTIVITY_HEADER header = new XXDBI_DAILY_ACTIVITY_HEADER();
                    header.DA_HEADER_ID = generateDailyActivityHeaderSequence();
                    header.STATE = data.l.REGION;
                    header.COUNTY = "NONE";
                    header.ACTIVITY_DATE = (DateTime)data.h.DA_DATE;
                    header.ORG_ID = (Decimal)data.p.ORG_ID;
                    header.PROJECT_NUMBER = data.p.SEGMENT1;
                    header.PROJECT_NAME = data.p.NAME;
                    header.CREATED_BY = data.u.USER_ID;
                    header.CREATION_DATE = DateTime.Now;
                    header.LAST_UPDATED_BY = data.u.USER_ID;
                    header.LAST_UPDATE_DATE = DateTime.Now;

                return header;
            }
        }






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




