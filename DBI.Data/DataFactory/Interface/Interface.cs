using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class Interface
    {
        public static void PostToOracle(long HeaderId)
        {
            XXDBI_DAILY_ACTIVITY_HEADER header;
            Interface.createHeaderRecords(HeaderId, out header);

            List<XXDBI_LABOR_HEADER> laborRecords;
            Interface.createLaborRecords(HeaderId, header, out laborRecords);

            //Create truck records
            Interface.createTruckUsageRecords(HeaderId, header, laborRecords);

            //Create perdiem
            Interface.createPerDiemRecords(HeaderId, header);

            //Create Inventory 
            Interface.PostInventory(HeaderId);

            //Create Production
            Interface.PostProduction(HeaderId);

            //Update Header with Daily Activity ID
            DAILY_ACTIVITY_HEADER HeaderRecord;
            using (Entities _context = new Entities())
            {
                HeaderRecord = (from d in _context.DAILY_ACTIVITY_HEADER
                                                      where d.HEADER_ID == HeaderId
                                                      select d).Single();
                HeaderRecord.DA_HEADER_ID = header.DA_HEADER_ID;
                HeaderRecord.STATUS = 4;
            }
            GenericData.Update<DAILY_ACTIVITY_HEADER>(HeaderRecord);


        }

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

        public static long generateEquipmentUsageSequence()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_MTLTRANSID_S.NEXTVAL from dual";
                long query = _context.Database.SqlQuery<long>(sql).First();
                return query;
            }
        }

        public static long generatePerDiemSequence()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_PERDIEMTRANSID_S.NEXTVAL from dual";
                long query = _context.Database.SqlQuery<long>(sql).First();
                return query;
            }
        }



        public static decimal payrollHoursCalculation(DateTime dateIn, DateTime dateOut, string lunchFlag, decimal? lunchAmount)
        {
            TimeSpan span = dateOut.Subtract(dateIn);
            double calc = (span.Minutes > 0 && span.Minutes <= 8) ? 0
                         : (span.Minutes > 8 && span.Minutes <= 23) ? .25
                         : (span.Minutes > 23 && span.Minutes <= 38) ? .50
                         : (span.Minutes > 38 && span.Minutes <= 53) ? .75
                         : (span.Minutes > 53 && span.Minutes <= 60) ? 1
                         : 0;
            decimal returnValue = span.Hours + (decimal)calc;

            //Lunch calculation 
            if(lunchFlag == "Y")
            {
                returnValue = returnValue - (decimal)((lunchAmount == 30) ? .50 : 1);
            }

             return returnValue;
        }

        public static decimal maxLaborHoursCalculation(List<XXDBI_LABOR_HEADER> laborRecords)
        {
            decimal returnValue = 0;

            if (laborRecords.Count > 1)
            {
                var r = from records in laborRecords
                        group records by records.EMPLOYEE_NUMBER into g
                        orderby g.Key ascending
                        select new
                        {
                            EMPLOYEENUM = g.Key,
                            TOTALHOURS = g.Sum(h => h.QUANTITY)
                        };
                returnValue = r.Max(h => h.TOTALHOURS);
            }
            else
            {
                returnValue = 0;
            }
            return returnValue;

        }


        public static void createHeaderRecords(long dailyActivityHeaderId, out XXDBI_DAILY_ACTIVITY_HEADER xxdbiHeaderRecord)
        {
            using (Entities _context = new Entities())
            {

                try
                {
                     var query = from h in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                            join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
                            where h.HEADER_ID == dailyActivityHeaderId
                            select new { h, p, l, u };

                var data = query.SingleOrDefault();

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

                xxdbiHeaderRecord = header;

                var columns = new[] { "DA_HEADER_ID", "STATE", "COUNTY", "ACTIVITY_DATE", "ORG_ID", "PROJECT_NUMBER", "PROJECT_NAME", "CREATED_BY", "CREATION_DATE", "LAST_UPDATED_BY", "LAST_UPDATE_DATE" };
                GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header, columns, "XXDBI.XXDBI_DAILY_ACTIVITY_HEADER");

                //Update the header
                //data.h.DA_HEADER_ID = header.DA_HEADER_ID;
                //_context.Set<DAILY_ACTIVITY_HEADER>().Attach(data.h);
                // _context.Entry(data.h).State = System.Data.EntityState.Modified;
                //_context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw(ex);
                }
            }

        }

        public static void createLaborRecords(long dailyActivityHeaderId, XXDBI_DAILY_ACTIVITY_HEADER xxdbiDailyActivityHeader, out List<XXDBI_LABOR_HEADER> xxdbiLaborHeaderRecords)
        {
            try
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
                                    TASK_NUMBER = (tasks.TASK_NUMBER == null) ? "9999" : tasks.TASK_NUMBER,
                                    EMPLOYEE_NUMBER = e.EMPLOYEE_NUMBER,
                                    EMP_FULL_NAME = e.EMPLOYEE_NAME,
                                    ROLE = d.ROLE_TYPE,
                                    STATE = (d.STATE == null) ? l.REGION : d.STATE,
                                    COUNTY = (d.COUNTY == null) ? "NONE" : d.COUNTY,
                                    LAB_HEADER_DATE = xxdbiDailyActivityHeader.ACTIVITY_DATE,
                                    ELEMENT = "Time Entry Wages",
                                    ADJUSTMENT = "N",
                                    STATUS = "UNPROCESSED",
                                    ORG_ID = (decimal)p.ORG_ID,
                                    CREATED_BY = u.USER_ID,
                                    LAST_UPDATED_BY = u.USER_ID,
                                    TIME_IN = d.TIME_IN,
                                    TIME_OUT = d.TIME_OUT,
                                    LUNCH_FLAG = d.LUNCH,
                                    LUNCH_LENGTH = d.LUNCH_LENGTH
                                }).ToList();

                    foreach (var r in data)
                    {
                        XXDBI_LABOR_HEADER record = new XXDBI_LABOR_HEADER();
                        record.LABOR_HEADER_ID = DBI.Data.Interface.generateLaborHeaderSequence();
                        record.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
                        record.PROJECT_NUMBER = r.PROJECT_NUMBER;
                        record.TASK_NUMBER = r.TASK_NUMBER;
                        record.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                        record.EMP_FULL_NAME = r.EMP_FULL_NAME;
                        record.ROLE = r.ROLE;
                        record.STATE = r.STATE;
                        record.COUNTY = r.COUNTY;
                        record.LAB_HEADER_DATE = r.LAB_HEADER_DATE;
                        record.QUANTITY = payrollHoursCalculation((DateTime)r.TIME_IN, (DateTime)r.TIME_OUT,r.LUNCH_FLAG,r.LUNCH_LENGTH);
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

                    foreach (XXDBI_LABOR_HEADER record in records)
                    {
                        var laborColumns = new[] { "LABOR_HEADER_ID", "DA_HEADER_ID", "PROJECT_NUMBER", "TASK_NUMBER", "EMPLOYEE_NUMBER", "EMP_FULL_NAME", "ROLE", "STATE", "COUNTY", "LAB_HEADER_DATE", "QUANTITY", "ELEMENT", "ADJUSTMENT", "STATUS", "ORG_ID", "CREATED_BY", "CREATION_DATE", "LAST_UPDATE_DATE", "LAST_UPDATED_BY" };
                        GenericData.Insert<XXDBI_LABOR_HEADER>(record, laborColumns, "XXDBI.XXDBI_LABOR_HEADER");
                    }

                    xxdbiLaborHeaderRecords = records;
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }
           
        }

        public static void createPerDiemRecords(long dailyActivityHeaderId, XXDBI_DAILY_ACTIVITY_HEADER xxdbiDailyActivityHeader)
        {
            try
            {
                List<XXDBI_PER_DIEM> records = new List<XXDBI_PER_DIEM>();
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
                                where d.HEADER_ID == dailyActivityHeaderId && d.PER_DIEM == "Y"
                                select new
                                {
                                    DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID,
                                    PROJECT_NUMBER = xxdbiDailyActivityHeader.PROJECT_NUMBER,
                                    TASK_NUMBER = (tasks.TASK_NUMBER == null) ? "9999" : tasks.TASK_NUMBER,
                                    EMPLOYEE_NUMBER = e.EMPLOYEE_NUMBER,
                                    EMP_FULL_NAME = e.EMPLOYEE_NAME,
                                    EXPENDITURE_TYPE = "PER DIEM",
                                    PER_DIEM_DATE = xxdbiDailyActivityHeader.ACTIVITY_DATE,
                                    AMOUNT = (xxdbiDailyActivityHeader.ORG_ID == 121) ? 25.00 : (xxdbiDailyActivityHeader.ORG_ID == 123) ? 35.00 : 25.00,
                                    APPROVAL_STATUS = "N",
                                    STATUS = "UNPROCESSED",
                                    ORG_ID = (decimal)p.ORG_ID,
                                    CREATED_BY = u.USER_ID,
                                    LAST_UPDATED_BY = u.USER_ID,
                                }).ToList();

                    foreach (var r in data)
                    {
                        XXDBI_PER_DIEM record = new XXDBI_PER_DIEM();
                        record.TRANSACTION_ID = DBI.Data.Interface.generatePerDiemSequence();
                        record.DA_HEADER_ID = r.DA_HEADER_ID;
                        record.PROJECT_NUMBER = r.PROJECT_NUMBER;
                        record.TASK_NUMBER = r.TASK_NUMBER;
                        record.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                        record.EMP_FULL_NAME = r.EMP_FULL_NAME;
                        record.EXPENDITURE_TYPE = r.EXPENDITURE_TYPE;
                        record.PER_DIEM_DATE = r.PER_DIEM_DATE;
                        record.AMOUNT = (decimal)r.AMOUNT;
                        record.APPROVAL_STATUS = r.APPROVAL_STATUS;
                        record.STATUS = r.STATUS;
                        record.ORG_ID = r.ORG_ID;
                        record.CREATED_BY = r.CREATED_BY;
                        record.CREATION_DATE = DateTime.Now;
                        record.LAST_UPDATE_DATE = DateTime.Now;
                        record.LAST_UPDATED_BY = r.LAST_UPDATED_BY;
                        records.Add(record);
                    }

                    foreach (XXDBI_PER_DIEM record in records)
                    {
                        var columns = new[] { "TRANSACTION_ID", "DA_HEADER_ID", "PROJECT_NUMBER", "TASK_NUMBER", "EMPLOYEE_NUMBER", "EMP_FULL_NAME", "EXPENDITURE_TYPE", "PER_DIEM_DATE", "AMOUNT", "APPROVAL_STATUS", "STATUS", "ORG_ID", "CREATED_BY", "CREATION_DATE", "LAST_UPDATE_DATE", "LAST_UPDATED_BY" };
                        GenericData.Insert<XXDBI_PER_DIEM>(record, columns, "XXDBI.XXDBI_PER_DIEM");
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

      
        public static void createTruckUsageRecords(long dailyActivityHeaderId, XXDBI_DAILY_ACTIVITY_HEADER dailyActivityHeaderRecord, List<XXDBI_LABOR_HEADER> laborRecords)
        {
            decimal maxHours = maxLaborHoursCalculation(laborRecords);
            List<XXDBI_TRUCK_EQUIP_USAGE> records = new List<XXDBI_TRUCK_EQUIP_USAGE>();
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
                            join pro in _context.DAILY_ACTIVITY_PRODUCTION on d.HEADER_ID equals pro.HEADER_ID into prod
                            from production in prod.DefaultIfEmpty()
                            join tsk in _context.PA_TASKS_V on production.TASK_ID equals tsk.TASK_ID into tsks
                            from tasks in tsks.DefaultIfEmpty()
                            where d.HEADER_ID == dailyActivityHeaderId
                            select new {
                                TRUCK_EQUIP = p.NAME,
                                DA_HEADER_ID = dailyActivityHeaderRecord.DA_HEADER_ID,
                                PROJECT_NUMBER = p.SEGMENT1,
                                TASK_NUMBER = (tasks.TASK_NUMBER == null) ? "9999" : tasks.TASK_NUMBER,
                                USAGE_DATE = dailyActivityHeaderRecord.ACTIVITY_DATE,
                                STATUS = "UNPROCESSED",
                                ORG_ID = (Decimal)p.ORG_ID,
                                USER_ID = u.USER_ID
                                       });

                            foreach(var r in data)
                            {
                               XXDBI_TRUCK_EQUIP_USAGE record = new XXDBI_TRUCK_EQUIP_USAGE();
                               record.DA_HEADER_ID = r.DA_HEADER_ID;
                               record.CREATED_BY = r.USER_ID;
                               record.CREATION_DATE = DateTime.Now;
                               record.LAST_UPDATED_BY = r.USER_ID;
                               record.LAST_UPDATE_DATE = DateTime.Now;
                               record.TRUCK_EQUIP = r.TRUCK_EQUIP;
                               record.TRANSACTION_ID = generateEquipmentUsageSequence();
                               record.PROJECT_NUMBER = r.PROJECT_NUMBER;
                               record.TASK_NUMBER = r.TASK_NUMBER;
                               record.QUANTITY = maxHours;
                               record.USAGE_DATE = r.USAGE_DATE;
                               record.STATUS = r.STATUS;
                               record.ORG_ID = r.ORG_ID;
                               records.Add(record);
                            }

                            foreach (XXDBI_TRUCK_EQUIP_USAGE record in records)
                            {
                                var columns = new[] { "TRUCK_EQUIP", "TRANSACTION_ID", "DA_HEADER_ID", "PROJECT_NUMBER", "TASK_NUMBER", "USAGE_DATE", "QUANTITY", "STATUS", "ORG_ID", "CREATED_BY", "CREATION_DATE", "LAST_UPDATED_BY", "LAST_UPDATE_DATE" };
                                GenericData.Insert<XXDBI_TRUCK_EQUIP_USAGE>(record, columns, "XXDBI.XXDBI_TRUCK_EQUIP_USAGE");
                            }
            }

        }

        public static void PostInventory(long HeaderId)
        {
            try
            {
                List<MTL_TRANSACTIONS_INTERFACE> RecordsToInsert = new List<MTL_TRANSACTIONS_INTERFACE>();
                using (Entities _context = new Entities())
                {
                    var InventoryList = (from i in _context.DAILY_ACTIVITY_INVENTORY
                                         join u in _context.SYS_USER_INFORMATION on i.MODIFIED_BY equals u.USER_NAME
                                         join h in _context.DAILY_ACTIVITY_HEADER on i.HEADER_ID equals h.HEADER_ID
                                         join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                         join iv in _context.INVENTORY_V on i.ITEM_ID equals iv.ITEM_ID
                                         where i.HEADER_ID == HeaderId
                                         select new { i, iv, u.USER_ID, p.ORG_ID }).ToList();

                    int InventoryCount = 1;
                    foreach (var InventoryItem in InventoryList)
                    {
                        decimal TransactionType;
                        if (InventoryItem.ORG_ID == 121)
                        {
                            TransactionType = 161;
                        }
                        else
                        {
                            TransactionType = 120;
                        }
                        decimal Quantity = -Math.Abs((decimal)InventoryItem.i.RATE);
                        MTL_TRANSACTIONS_INTERFACE Record = new MTL_TRANSACTIONS_INTERFACE
                        {
                            SOURCE_CODE = "EMS",
                            SOURCE_HEADER_ID = InventoryItem.i.HEADER_ID,
                            SOURCE_LINE_ID = InventoryCount,
                            PROCESS_FLAG = 2,
                            TRANSACTION_MODE = 3,
                            INVENTORY_ITEM_ID = InventoryItem.i.ITEM_ID,
                            ORGANIZATION_ID = (decimal)InventoryItem.i.SUB_INVENTORY_ORG_ID,
                            SUBINVENTORY_CODE = InventoryItem.i.SUB_INVENTORY_SECONDARY_NAME,
                            ITEM_SEGMENT1 = InventoryItem.iv.SEGMENT1,
                            TRANSACTION_QUANTITY = Quantity,
                            TRANSACTION_UOM = InventoryItem.i.UNIT_OF_MEASURE,
                            TRANSACTION_DATE = (DateTime)InventoryItem.i.DAILY_ACTIVITY_HEADER.DA_DATE,
                            TRANSACTION_TYPE_ID = TransactionType,
                            TRANSACTION_SOURCE_NAME = "EMS",
                            LOCK_FLAG = 2,
                            VALIDATION_REQUIRED = 1,
                            LAST_UPDATE_DATE = DateTime.Now,
                            LAST_UPDATED_BY = InventoryItem.USER_ID,
                            CREATED_BY = InventoryItem.USER_ID,
                            CREATION_DATE = DateTime.Now,
                        };
                        InventoryCount++;
                        RecordsToInsert.Add(Record);
                        var InventoryColumns = new[] { "SOURCE_CODE", "SOURCE_HEADER_ID", "SOURCE_LINE_ID", "PROCESS_FLAG", "TRANSACTION_MODE", "INVENTORY_ITEM_ID", "ORGANIZATION_ID", "SUBINVENTORY_CODE", "ITEM_SEGMENT1", "TRANSACTION_QUANTITY", "TRANSACTION_UOM", "TRANSACTION_DATE", "TRANSACTION_TYPE_ID", "TRANSACTION_SOURCE_NAME", "LOCK_FLAG", "VALIDATION_REQUIRED", "LAST_UPDATE_DATE", "LAST_UPDATED_BY", "CREATED_BY", "CREATION_DATE" };
                        GenericData.Insert<MTL_TRANSACTIONS_INTERFACE>(Record, InventoryColumns, "INV.MTL_TRANSACTIONS_INTERFACE");
                    }

                }
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public static DateTime GetPADateFromHeader(DateTime HeaderDate, long OrgId)
        {
            using (Entities _context = new Entities())
            {
                DateTime returnDate = (from p in _context.PA_PERIODS_ALL
                                       where p.ORG_ID == OrgId && HeaderDate >= p.START_DATE && HeaderDate <= p.END_DATE
                                       select p).Max(p => p.END_DATE);
                return returnDate;
            }
        }


        public static void PostProduction(long HeaderId)
        {
            try
            {
                PA_TRANSACTION_INTERFACE_ALL RowToAdd;
                using (Entities _context = new Entities())
                {
                    var ProductionList = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                          join u in _context.SYS_USER_INFORMATION on d.CREATED_BY equals u.USER_NAME
                                          join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                                          join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                          join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                          where d.HEADER_ID == HeaderId
                                          select new { d, p, t.TASK_NUMBER, u.USER_ID, h.DA_DATE }).ToList();
                    
                    foreach (var Production in ProductionList)
                    {
                        if (Production.p.ORG_ID == 123)
                        {
                            DateTime periodDate = GetPADateFromHeader((DateTime) Production.DA_DATE, 123);

                            string transReference = Production.p.SEGMENT1 + Production.d.PRODUCTION_ID.ToString();
                            string batchName = Production.p.SEGMENT1 + DateTime.Now;
                            RowToAdd = new PA_TRANSACTION_INTERFACE_ALL
                            {
                                QUANTITY = (decimal) Production.d.QUANTITY,
                                ORIG_TRANSACTION_REFERENCE = transReference,
                                TRANSACTION_SOURCE = "DBI Daily Activity Sheet",
                                BATCH_NAME = batchName,
                                EXPENDITURE_ENDING_DATE = periodDate,
                                EXPENDITURE_ITEM_DATE = (DateTime) Production.DA_DATE,
                                PROJECT_NUMBER = Production.p.SEGMENT1,
                                TASK_NUMBER = Production.TASK_NUMBER,
                                EXPENDITURE_TYPE = Production.d.EXPENDITURE_TYPE,
                                TRANSACTION_STATUS_CODE = "P",
                                ORG_ID = Production.p.ORG_ID,
                                ORGANIZATION_NAME = Production.p.ORGANIZATION_NAME,
                                UNMATCHED_NEGATIVE_TXN_FLAG = "N",
                                CREATED_BY = Production.USER_ID,
                                CREATION_DATE = DateTime.Now,
                                LAST_UPDATE_DATE = DateTime.Now,
                                LAST_UPDATED_BY = Production.USER_ID
                            };
                            var ProductionColumns = new[] { "QUANTITY", "ORIG_TRANSACTION_REFERENCE", "TRANSACTION_SOURCE", "BATCH_NAME", "EXPENDITURE_ENDING_DATE", "EXPENDITURE_ITEM_DATE", "PROJECT_NUMBER", "TASK_NUMBER", "EXPENDITURE_TYPE", "TRANSACTION_STATUS_CODE", "ORG_ID", "ORGANIZATION_NAME", "UNMATCHED_NEGATIVE_TXN_FLAG", "CREATED_BY", "CREATION_DATE", "LAST_UPDATED_BY", "LAST_UPDATE_DATE" };
                            GenericData.Insert<PA_TRANSACTION_INTERFACE_ALL>(RowToAdd, ProductionColumns, "PA.PA_TRANSACTION_INTERFACE_ALL");
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

   
      


    }
}




