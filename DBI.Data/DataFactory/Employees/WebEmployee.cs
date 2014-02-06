using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class DAILY_ACTIVITY_EMPLOYEE
    {
        public static List<DAILY_ACTIVITY_EMPLOYEE> GetEmployees()
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<DAILY_ACTIVITY_EMPLOYEE>().ToList();
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


        public static List<LABOR_HEADER_RECORD> interfaceRecords(long dailyActivityHeaderId, DateTime daDate)
        {
            using (Entities _context = new Entities())
            {

                var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                            from projects in proj.DefaultIfEmpty()
                            join pro in _context.DAILY_ACTIVITY_PRODUCTION on d.HEADER_ID equals pro.HEADER_ID into prod
                            from production in prod.DefaultIfEmpty()
                            join tsk in _context.PA_TASKS_V on production.TASK_ID equals tsk.TASK_ID into tsks
                            from tasks in tsks.DefaultIfEmpty()
                            join l in _context.PA_LOCATIONS_V on projects.LOCATION_ID equals (long)l.LOCATION_ID
                            join u in _context.SYS_USER_INFORMATION on d.CREATED_BY equals u.USER_NAME
                            where d.HEADER_ID == dailyActivityHeaderId
                            select new LABOR_HEADER_RECORD
                            {
                                LABOR_HEADER_ID = 0, //DBI.Data.Interface.generateLaborHeaderSequence(),
                                DA_HEADER_ID = 0, 
                                PROJECT_NUMBER = projects.SEGMENT1,
                                TASK_NUMBER = tasks.TASK_NUMBER,
                                EMPLOYEE_NUMBER = e.EMPLOYEE_NUMBER,
                                EMP_FULL_NAME = e.EMPLOYEE_NAME,
                                ROLE = d.ROLE_TYPE,
                                STATE = l.REGION,
                                COUNTY = "NONE",
                                LAB_HEADER_DATE = daDate,
                                QUANTITY = payrollHoursCalculation((DateTime)d.TIME_IN, (DateTime)d.TIME_OUT),
                                ELEMENT = "Time Entry Wages",
                                ADJUSTMENT = "N",
                                STATUS = "UNPROCESSED",
                                ORG_ID = (decimal)projects.ORG_ID,
                                CREATED_BY = u.USER_ID,
                                CREATION_DATE = DateTime.Now,
                                LAST_UPDATE_DATE = DateTime.Now,
                                LAST_UPDATED_BY = u.USER_ID
                            }).ToList();

                return data;
            }
        }

        public class LABOR_HEADER_RECORD
        {
            public decimal LABOR_HEADER_ID { get; set; }
            public decimal DA_HEADER_ID { get; set; }
            public string PROJECT_NUMBER { get; set; }
            public string TASK_NUMBER { get; set; }
            public string WORK_ORDER_NUM { get; set; }
            public string EMPLOYEE_NUMBER { get; set; }
            public string EMP_FULL_NAME { get; set; }
            public string ROLE { get; set; }
            public string STATE { get; set; }
            public string COUNTY { get; set; }
            public System.DateTime LAB_HEADER_DATE { get; set; }
            public decimal QUANTITY { get; set; }
            public string ELEMENT { get; set; }
            public string ADJUSTMENT { get; set; }
            public string STATUS { get; set; }
            public decimal ORG_ID { get; set; }
            public decimal CREATED_BY { get; set; }
            public System.DateTime CREATION_DATE { get; set; }
            public decimal LAST_UPDATED_BY { get; set; }
            public System.DateTime LAST_UPDATE_DATE { get; set; }
            public Nullable<decimal> LAST_UPDATE_LOGIN { get; set; }
            public string OVR_DAILYOT_FLAG { get; set; }
        }
 
    }
}
