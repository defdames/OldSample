using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DBI.Data
{
    public class Interface
    {
        public static void PostToOracle(long HeaderId, string UserByUserName)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    //return user information for logged in user account
                    SYS_USER_INFORMATION userInformation = SYS_USER_INFORMATION.UserByUserName(UserByUserName);

                    XXDBI_DAILY_ACTIVITY_HEADER_V header;
                    Interface.createHeaderRecords(HeaderId, userInformation.USER_ID, out header);

                    List<XXDBI_LABOR_HEADER_V> laborRecords;
                    Interface.createLaborRecords(HeaderId, userInformation.USER_ID, header, out laborRecords);

                    //Create truck records
                    Interface.createTruckUsageRecords(HeaderId, userInformation.USER_ID, header, laborRecords);

                    //Create perdiem
                    Interface.createPerDiemRecords(HeaderId, userInformation.USER_ID, header);

                    //Create Inventory 
                    Interface.PostInventory(HeaderId, userInformation.USER_ID);

                    //Create Production
                    Interface.PostProduction(HeaderId, userInformation.USER_ID);

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

                    Interface.postNotificationMessage(userInformation.EMPLOYEE_NAME, HeaderRecord);

                    transaction.Complete();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }



        //public static  long GetProjectGLCode(long project_id)
        //{
        //   using (Entities _context = new Entities())
        //       {
        //            string sql = @"select XXEMS"
        //       }
        //}

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

        public static long getGlCode(long ProjectId)
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select xxems.EMS_INVENTORY.GET_GL_CODE (" + ProjectId + ")from dual";
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

        public static decimal maxLaborHoursCalculation(List<XXDBI_LABOR_HEADER_V> laborRecords)
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


        public static void createHeaderRecords(long dailyActivityHeaderId, long postedByUserId, out XXDBI_DAILY_ACTIVITY_HEADER_V xxdbiHeaderRecord)
        {
            using (Entities _context = new Entities())
            {

                try
                {
                     var query = from h in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                            where h.HEADER_ID == dailyActivityHeaderId
                            select new { h, p, l};

                var data = query.SingleOrDefault();

                XXDBI_DAILY_ACTIVITY_HEADER_V header = new XXDBI_DAILY_ACTIVITY_HEADER_V();
                header.DA_HEADER_ID = generateDailyActivityHeaderSequence();
                header.STATE = data.l.REGION;
                header.COUNTY = "NONE";
                header.ACTIVITY_DATE = (DateTime)data.h.DA_DATE;
                header.ORG_ID = (Decimal)data.p.ORG_ID;
                header.PROJECT_NUMBER = data.p.SEGMENT1;
                header.PROJECT_NAME = data.p.NAME;
                header.CREATED_BY = postedByUserId;
                header.CREATION_DATE = DateTime.Now;
                header.LAST_UPDATED_BY = postedByUserId;
                header.LAST_UPDATE_DATE = DateTime.Now;

                xxdbiHeaderRecord = header;

                GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER_V>(header);

                }
                catch (Exception ex)
                {
                    throw(ex);
                }
            }

        }

        /// <summary>
        /// Gets the task used on the daily activity, we link to the first one used that matches a task ID otherwise point it to 9999
        /// </summary>
        /// <param name="dailyActivityHeaderID"></param>
        /// <returns></returns>
        public static string returnDailyActivityTaskNumber(long dailyActivityHeaderID)
        {
            using (Entities _context = new Entities())
            {
                var productionData = (from p in _context.DAILY_ACTIVITY_PRODUCTION
                                                           join t in _context.PA_TASKS_V on p.TASK_ID equals t.TASK_ID into task
                                                           from tasks in task.DefaultIfEmpty()
                                                           where p.HEADER_ID == dailyActivityHeaderID
                                                           select new { tasks.TASK_NUMBER });

                return (productionData != null) ? productionData.First().TASK_NUMBER : "9999";

            }
        }

        public static void createLaborRecords(long dailyActivityHeaderId, long postedByUserId, XXDBI_DAILY_ACTIVITY_HEADER_V xxdbiDailyActivityHeader, out List<XXDBI_LABOR_HEADER_V> xxdbiLaborHeaderRecords)
        {
            try
            {
                List<XXDBI_LABOR_HEADER_V> records = new List<XXDBI_LABOR_HEADER_V>();
                using (Entities _context = new Entities())
                {


                    var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                                where d.HEADER_ID == dailyActivityHeaderId
                                select new { d.TRAVEL_TIME, d.DRIVE_TIME, p.SEGMENT1, e.PERSON_ID, e.EMPLOYEE_NUMBER, e.EMPLOYEE_NAME, d.ROLE_TYPE, d.STATE, l.REGION, d.COUNTY, p.ORG_ID, d.TIME_IN, d.TIME_OUT, d.LUNCH, d.LUNCH_LENGTH}).ToList();

                    //Add Time Entry Wages
                    foreach (var r in data)
                    {
                        XXDBI_LABOR_HEADER_V record = new XXDBI_LABOR_HEADER_V();
                        record.LABOR_HEADER_ID = DBI.Data.Interface.generateLaborHeaderSequence();
                        record.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
                        record.PROJECT_NUMBER = r.SEGMENT1;
                        record.TASK_NUMBER = returnDailyActivityTaskNumber(dailyActivityHeaderId);
                        record.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                        record.EMP_FULL_NAME = DBI.Data.EMPLOYEES_V.oracleEmployeeName(r.PERSON_ID);
                        record.ROLE = r.ROLE_TYPE;
                        record.STATE = (r.STATE == null) ? r.REGION : r.STATE;
                        record.COUNTY = r.COUNTY;
                        record.LAB_HEADER_DATE = xxdbiDailyActivityHeader.ACTIVITY_DATE;
                        record.QUANTITY = payrollHoursCalculation((DateTime)r.TIME_IN, (DateTime)r.TIME_OUT,r.LUNCH,r.LUNCH_LENGTH);
                        record.ELEMENT = "Time Entry Wages";
                        record.ADJUSTMENT = "N";
                        record.STATUS = "UNPROCESSED";
                        record.ORG_ID = (decimal)r.ORG_ID;
                        record.CREATED_BY = postedByUserId;
                        record.CREATION_DATE = DateTime.Now;
                        record.LAST_UPDATE_DATE = DateTime.Now;
                        record.LAST_UPDATED_BY = postedByUserId;
                        records.Add(record);
                        GenericData.Insert<XXDBI_LABOR_HEADER_V>(record);

                        //Check if record is IRM and Drive time added, then add Drive Time Base
                        if (xxdbiDailyActivityHeader.ORG_ID == 123 && r.DRIVE_TIME > 0)
                        {
                            XXDBI_LABOR_HEADER_V dtrecord = new XXDBI_LABOR_HEADER_V();
                            dtrecord.LABOR_HEADER_ID = DBI.Data.Interface.generateLaborHeaderSequence();
                            dtrecord.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
                            dtrecord.PROJECT_NUMBER = r.SEGMENT1;
                            dtrecord.TASK_NUMBER = returnDailyActivityTaskNumber(dailyActivityHeaderId);
                            dtrecord.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                            dtrecord.EMP_FULL_NAME = DBI.Data.EMPLOYEES_V.oracleEmployeeName(r.PERSON_ID);
                            dtrecord.ROLE = null;
                            dtrecord.STATE = xxdbiDailyActivityHeader.STATE;
                            dtrecord.COUNTY = xxdbiDailyActivityHeader.COUNTY;
                            dtrecord.LAB_HEADER_DATE = xxdbiDailyActivityHeader.ACTIVITY_DATE;
                            dtrecord.QUANTITY = (decimal)r.DRIVE_TIME;
                            dtrecord.ELEMENT = "Drive Time Base";
                            dtrecord.ADJUSTMENT = "N";
                            dtrecord.STATUS = "UNPROCESSED";
                            dtrecord.ORG_ID = (decimal)r.ORG_ID;
                            dtrecord.CREATED_BY = postedByUserId;
                            dtrecord.CREATION_DATE = DateTime.Now;
                            dtrecord.LAST_UPDATE_DATE = DateTime.Now;
                            dtrecord.LAST_UPDATED_BY = postedByUserId;
                            records.Add(dtrecord);
                            GenericData.Insert<XXDBI_LABOR_HEADER_V>(dtrecord);
                        }

                        //Check if record is IRM and Travel time added, then add Travel Time Base
                        if (xxdbiDailyActivityHeader.ORG_ID == 123 && r.DRIVE_TIME > 0)
                        {
                            XXDBI_LABOR_HEADER_V dtrecord = new XXDBI_LABOR_HEADER_V();
                            dtrecord.LABOR_HEADER_ID = DBI.Data.Interface.generateLaborHeaderSequence();
                            dtrecord.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
                            dtrecord.PROJECT_NUMBER = r.SEGMENT1;
                            dtrecord.TASK_NUMBER = returnDailyActivityTaskNumber(dailyActivityHeaderId);
                            dtrecord.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                            dtrecord.EMP_FULL_NAME = DBI.Data.EMPLOYEES_V.oracleEmployeeName(r.PERSON_ID);
                            dtrecord.ROLE = null;
                            dtrecord.STATE = xxdbiDailyActivityHeader.STATE;
                            dtrecord.COUNTY = xxdbiDailyActivityHeader.COUNTY;
                            dtrecord.LAB_HEADER_DATE = xxdbiDailyActivityHeader.ACTIVITY_DATE;
                            dtrecord.QUANTITY = (decimal)r.TRAVEL_TIME;
                            dtrecord.ELEMENT = "Travel Time Base";
                            dtrecord.ADJUSTMENT = "N";
                            dtrecord.STATUS = "UNPROCESSED";
                            dtrecord.ORG_ID = (decimal)r.ORG_ID;
                            dtrecord.CREATED_BY = postedByUserId;
                            dtrecord.CREATION_DATE = DateTime.Now;
                            dtrecord.LAST_UPDATE_DATE = DateTime.Now;
                            dtrecord.LAST_UPDATED_BY = postedByUserId;
                            records.Add(dtrecord);
                            GenericData.Insert<XXDBI_LABOR_HEADER_V>(dtrecord);
                        }


                    }


                    xxdbiLaborHeaderRecords = records;
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }
           
        }

        public static void createPerDiemRecords(long dailyActivityHeaderId, long postedByUserId, XXDBI_DAILY_ACTIVITY_HEADER_V xxdbiDailyActivityHeader)
        {
            try
            {
                using (Entities _context = new Entities())
                {

                    var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                                where d.HEADER_ID == dailyActivityHeaderId && d.PER_DIEM == "Y"
                                select new { e.EMPLOYEE_NAME, e.EMPLOYEE_NUMBER, p.ORG_ID, e.PERSON_ID}).ToList();

                    foreach (var r in data)
                    {
                        XXDBI_PER_DIEM_V record = new XXDBI_PER_DIEM_V();
                        record.TRANSACTION_ID = DBI.Data.Interface.generatePerDiemSequence();
                        record.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
                        record.PROJECT_NUMBER = xxdbiDailyActivityHeader.PROJECT_NUMBER;
                        record.TASK_NUMBER = returnDailyActivityTaskNumber(dailyActivityHeaderId);
                        record.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
                        record.EMP_FULL_NAME = DBI.Data.EMPLOYEES_V.oracleEmployeeName(r.PERSON_ID);
                        record.EXPENDITURE_TYPE = "PER DIEM";
                        record.PER_DIEM_DATE =xxdbiDailyActivityHeader.ACTIVITY_DATE;
                        record.AMOUNT = (decimal)((xxdbiDailyActivityHeader.ORG_ID == 121) ? 25.00 : (xxdbiDailyActivityHeader.ORG_ID == 123) ? 35.00 : 25.00);
                        record.APPROVAL_STATUS = "N";
                        record.STATUS = "UNPROCESSED";
                        record.ORG_ID = (long)r.ORG_ID;
                        record.CREATED_BY = postedByUserId;
                        record.CREATION_DATE = DateTime.Now;
                        record.LAST_UPDATE_DATE = DateTime.Now;
                        record.LAST_UPDATED_BY = postedByUserId;
                        GenericData.Insert<XXDBI_PER_DIEM_V>(record);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        public static void createTruckUsageRecords(long dailyActivityHeaderId, long postedByUserId, XXDBI_DAILY_ACTIVITY_HEADER_V dailyActivityHeaderRecord, List<XXDBI_LABOR_HEADER_V> laborRecords)
        {
            decimal maxHours = maxLaborHoursCalculation(laborRecords);
            List<XXDBI_TRUCK_EQUIP_USAGE_V> records = new List<XXDBI_TRUCK_EQUIP_USAGE_V>();
            using (Entities _context = new Entities())
            {

                var data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == dailyActivityHeaderId
                            select new { p.NAME, p.SEGMENT1, p.ORG_ID}).ToList();

                            foreach(var r in data)
                            {
                                XXDBI_TRUCK_EQUIP_USAGE_V record = new XXDBI_TRUCK_EQUIP_USAGE_V();
                               record.DA_HEADER_ID = dailyActivityHeaderRecord.DA_HEADER_ID;
                               record.CREATED_BY = postedByUserId;
                               record.CREATION_DATE = DateTime.Now;
                               record.LAST_UPDATED_BY = postedByUserId;
                               record.LAST_UPDATE_DATE = DateTime.Now;
                               record.TRUCK_EQUIP = r.NAME;
                               record.TRANSACTION_ID = generateEquipmentUsageSequence();
                               record.PROJECT_NUMBER = dailyActivityHeaderRecord.PROJECT_NUMBER;
                               record.TASK_NUMBER = returnDailyActivityTaskNumber(dailyActivityHeaderId);
                               record.QUANTITY = maxHours;
                               record.USAGE_DATE = dailyActivityHeaderRecord.ACTIVITY_DATE;
                               record.STATUS = "UNPROCESSED";
                               record.ORG_ID = (decimal)r.ORG_ID;
                               records.Add(record);
                            }

                            foreach (XXDBI_TRUCK_EQUIP_USAGE_V record in records)
                            {
                                GenericData.Insert<XXDBI_TRUCK_EQUIP_USAGE_V>(record);
                            }
            }

        }

        public static void PostInventory(long HeaderId, long postedByUserId)
        {
            try
            {
                List<MTL_TRANSACTION_INT_V> RecordsToInsert = new List<MTL_TRANSACTION_INT_V>();
                using (Entities _context = new Entities())
                {
                    var InventoryList = (from i in _context.DAILY_ACTIVITY_INVENTORY
                                         join h in _context.DAILY_ACTIVITY_HEADER on i.HEADER_ID equals h.HEADER_ID
                                         join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                         join iv in _context.INVENTORY_V on new {JoinProperty1 = (decimal) i.ITEM_ID, JoinProperty2 = (long)i.SUB_INVENTORY_ORG_ID } equals new {JoinProperty1 = iv.ITEM_ID, JoinProperty2 = iv.ORGANIZATION_ID }                                        
                                         where i.HEADER_ID == HeaderId
                                         select new { i, iv, p.ORG_ID, h.PROJECT_ID}).ToList();

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
                                                

                        long GlCode = getGlCode((long)InventoryItem.PROJECT_ID);
                        decimal Quantity = -Math.Abs((decimal)InventoryItem.i.RATE);
                        //GL_CODE Account = GetProjectGLCode (InventoryItem.PROJECT_ID);
                        MTL_TRANSACTION_INT_V Record = new MTL_TRANSACTION_INT_V
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
                            DISTRIBUTION_ACCOUNT_ID = GlCode,
                            LOCK_FLAG = 2,
                            VALIDATION_REQUIRED = 1,
                            LAST_UPDATE_DATE = DateTime.Now,
                            LAST_UPDATED_BY = postedByUserId,
                            CREATED_BY = postedByUserId,
                            CREATION_DATE = DateTime.Now,
                        };
                        InventoryCount++;
                        RecordsToInsert.Add(Record);
                        GenericData.Insert<MTL_TRANSACTION_INT_V>(Record);
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

        public static void postNotificationMessage(string postedByUser, DAILY_ACTIVITY_HEADER headerDetails)
        {
            try
            {
                using(Entities _context = new Entities())
                {
                    DAILY_ACTIVITY_IMPORT importDetails = _context.DAILY_ACTIVITY_IMPORT.Where(h => h.HEADER_ID == headerDetails.HEADER_ID).SingleOrDefault();

                    PROJECTS_V projectDetails = _context.PROJECTS_V.Where(p => p.PROJECT_ID == headerDetails.PROJECT_ID).SingleOrDefault();

                    if(importDetails != null || importDetails.DEVICE_ID != "0001")
                    {
                        SYS_MOBILE_NOTIFICATIONS notification = new SYS_MOBILE_NOTIFICATIONS();
                        notification.DEVICE_ID = importDetails.DEVICE_ID;
                        notification.CREATE_DATE = DateTime.Now;
                        notification.MESSAGE = string.Format("Daily activity for {0} completed on {1} has been posted by {2}",projectDetails.LONG_NAME,DateTime.Parse(headerDetails.DA_DATE.ToString()).ToShortDateString(),postedByUser);
                        notification.SOUND = "alert.caf";
                        GenericData.Insert<SYS_MOBILE_NOTIFICATIONS>(notification);
                    }
                }


            }
            catch (Exception ex)
            {
                
                throw;
            }

        }


        public static void PostProduction(long HeaderId, long postedByUserId)
        {
            try
            {
                PA_TRANSACTION_INT_V RowToAdd;
                using (Entities _context = new Entities())
                {
                    var ProductionList = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                          join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                                          join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                          join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                          where d.HEADER_ID == HeaderId
                                          select new { d, p, t.TASK_NUMBER, h.DA_DATE }).ToList();
                    
                    foreach (var Production in ProductionList)
                    {
                        if (Production.p.ORG_ID == 123)
                        {
                            DateTime periodDate = GetPADateFromHeader((DateTime) Production.DA_DATE, 123);

                            string transReference = "EMS" + Production.p.SEGMENT1 + Production.d.PRODUCTION_ID.ToString();
                            string batchName = Production.p.SEGMENT1 + DateTime.Now;
                            string taskName = returnDailyActivityTaskNumber(HeaderId);
                            RowToAdd = new PA_TRANSACTION_INT_V
                            {
                                QUANTITY = (decimal) Production.d.QUANTITY,
                                ORIG_TRANSACTION_REFERENCE = transReference,
                                TRANSACTION_SOURCE = "DBI Daily Activity Sheet",
                                BATCH_NAME = batchName,
                                EXPENDITURE_ENDING_DATE = periodDate,
                                EXPENDITURE_ITEM_DATE = (DateTime) Production.DA_DATE,
                                PROJECT_NUMBER = Production.p.SEGMENT1,
                                TASK_NUMBER = taskName,
                                EXPENDITURE_TYPE = Production.d.EXPENDITURE_TYPE,
                                TRANSACTION_STATUS_CODE = "P",
                                ORG_ID = Production.p.ORG_ID,
                                ORGANIZATION_NAME = Production.p.ORGANIZATION_NAME,
                                UNMATCHED_NEGATIVE_TXN_FLAG = "N",
                                CREATED_BY = postedByUserId,
                                CREATION_DATE = DateTime.Now,
                                LAST_UPDATE_DATE = DateTime.Now,
                                LAST_UPDATED_BY = postedByUserId
                            };
                            GenericData.Insert<PA_TRANSACTION_INT_V>(RowToAdd);
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




