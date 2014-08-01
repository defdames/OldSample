using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;
using System.Web;

namespace DBI.Data
{
    public class BB
    {
        public static void CleanOldTempRecords(int numOfDaysOld)
        {
            DateTime delDate = DateTime.Today.AddDays(- numOfDaysOld);
            List<BUD_BID_PROJECTS> projectData;
            List<BUD_BID_ACTUAL_NUM> actualData;
            List<BUD_BID_BUDGET_NUM> budgetData;
            List<BUD_BID_DETAIL_SHEET> detailSheetData;
            List<BUD_BID_DETAIL_TASK> taskInfoData;

            using (Entities context = new Entities())
            {
                projectData = context.BUD_BID_PROJECTS.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
                actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
                budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
                detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
                taskInfoData = context.BUD_BID_DETAIL_TASK.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
            }

            GenericData.Delete<BUD_BID_PROJECTS>(projectData);
            GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
            GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);
            GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
            GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData);
        }
        public static List<DoubleComboLongID> BudgetVersions()
        {
            List<DoubleComboLongID> comboItem = new List<DoubleComboLongID>();
            comboItem.Add(new DoubleComboLongID { ID = 1, ID_NAME = "Bid" });
            comboItem.Add(new DoubleComboLongID { ID = 2, ID_NAME = "First Draft" });
            comboItem.Add(new DoubleComboLongID { ID = 3, ID_NAME = "Final Draft" });
            comboItem.Add(new DoubleComboLongID { ID = 4, ID_NAME = "1st Reforecast" });
            comboItem.Add(new DoubleComboLongID { ID = 5, ID_NAME = "2nd Reforecast" });
            comboItem.Add(new DoubleComboLongID { ID = 6, ID_NAME = "3rd Reforecast" });
            comboItem.Add(new DoubleComboLongID { ID = 7, ID_NAME = "4th Reforecast" });
            return comboItem;
        }
        public static List<SingleCombo> YearSummaryProjectActions()
        {
            List<SingleCombo> comboItems = new List<SingleCombo>();
            comboItems.Add(new SingleCombo { ID_NAME = "Add a New Project" });
            comboItems.Add(new SingleCombo { ID_NAME = "Edit Selected Project" });
            comboItems.Add(new SingleCombo { ID_NAME = "Copy Selected Project" });
            comboItems.Add(new SingleCombo { ID_NAME = "Delete Selected Project" });
            return comboItems;
        }
        public static List<SingleCombo> YearSummaryDetailActions()
        {
            List<SingleCombo> comboItem = new List<SingleCombo>();
            comboItem.Add(new SingleCombo { ID_NAME = "Add a New Sheet" });
            comboItem.Add(new SingleCombo { ID_NAME = "Edit Selected Sheet" });
            comboItem.Add(new SingleCombo { ID_NAME = "Copy Selected Sheet" });
            comboItem.Add(new SingleCombo { ID_NAME = "Delete Selected Sheet" });
            comboItem.Add(new SingleCombo { ID_NAME = "Reorder Sheets" });
            comboItem.Add(new SingleCombo { ID_NAME = "Print All Sheets" });
            return comboItem;
        }
        public static long CopyAllProjectDataAsTemp(long budBidID)
        {
            // BBProject
            BUD_BID_PROJECTS projectData;
            using (Entities context = new Entities())
            {
                projectData = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
            }
            projectData.MODIFIED_BY = "TEMP";
            projectData.MODIFY_DATE = DateTime.Now;
            GenericData.Insert<BUD_BID_PROJECTS>(projectData);
            decimal newBudBidID = projectData.BUD_BID_PROJECTS_ID;

            // Tasks
            List<BUD_BID_DETAIL_TASK> taskData;
            using (Entities context = new Entities())
            {
                taskData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            decimal detailTaskID;
            decimal newDetailTaskID;
            foreach (BUD_BID_DETAIL_TASK taskField in taskData)
            {
                detailTaskID = taskField.DETAIL_TASK_ID;
                taskField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                taskField.MODIFIED_BY = "TEMP";
                taskField.MODIFY_DATE = DateTime.Now;
                GenericData.Insert<BUD_BID_DETAIL_TASK>(taskData.Where(x => x.DETAIL_TASK_ID == detailTaskID).Single());
                newDetailTaskID = taskField.DETAIL_TASK_ID;

                // Actuals
                List<BUD_BID_ACTUAL_NUM> actualData;
                using (Entities context = new Entities())
                {
                    actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }

                foreach (BUD_BID_ACTUAL_NUM actualField in actualData)
                {
                    actualField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    actualField.DETAIL_TASK_ID = newDetailTaskID;
                    actualField.MODIFIED_BY = "TEMP";
                    actualField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_ACTUAL_NUM>(actualData);

                // Budgets
                List<BUD_BID_BUDGET_NUM> budgetData;
                using (Entities context = new Entities())
                {
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_BUDGET_NUM budgetField in budgetData)
                {
                    budgetField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    budgetField.DETAIL_TASK_ID = newDetailTaskID;
                    budgetField.MODIFIED_BY = "TEMP";
                    budgetField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_BUDGET_NUM>(budgetData);

                // Detail sheets
                List<BUD_BID_DETAIL_SHEET> sheetData;
                using (Entities context = new Entities())
                {
                    sheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_DETAIL_SHEET detailField in sheetData)
                {
                    detailField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    detailField.DETAIL_TASK_ID = newDetailTaskID;
                    detailField.MODIFIED_BY = "TEMP";
                    detailField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(sheetData);
            }

            return Convert.ToInt64(newBudBidID);
        }
        
        #region For sub grid deserializing
        public class BBSubGridV
        {
            public long DETAIL_SHEET_ID { get; set; }
            public long PROJECT_ID { get; set; }
            public long DETAIL_TASK_ID { get; set; }
            public string REC_TYPE { get; set; }
            public string DESC_1 { get; set; }
            public string DESC_2 { get; set; }
            public string AMT_1 { get; set; }
            public string AMT_2 { get; set; }
            public string AMT_3 { get; set; }
            public string AMT_4 { get; set; }
            public string AMT_5 { get; set; }
            public string TOTAL { get; set; }
        }
        #endregion
    }
    
    public class BBSummary
    {
        public class Grid
        {
            #region Fields
            public class Fields
            {
                public long BUD_BID_PROJECTS_ID { get; set; }
                public string PROJECT_ID { get; set; }
                public string PROJECT_NUM { get; set; }
                public string TYPE { get; set; }
                public string PROJECT_NAME { get; set; }
                public string STATUS { get; set; }
                public decimal ACRES { get; set; }
                public decimal DAYS { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
                public decimal PREV_OP { get; set; }
                public decimal MAT_PERC { get; set; }
                public decimal GR_PERC { get; set; }
                public decimal DIRECTS_PERC { get; set; }
                public decimal OP_PERC { get; set; }
                public decimal OP_VAR { get; set; }
            }
            #endregion

            public static List<Fields> Data(string orgName, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string sql = string.Format(@"
                    WITH
                        CUR_PROJECT_INFO_WITH_STATUS AS(
                            SELECT BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.TYPE, BUD_BID_PROJECTS.PRJ_NAME, BUD_BID_STATUS.STATUS,
                                BUD_BID_PROJECTS.ACRES, BUD_BID_PROJECTS.DAYS, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT
                            FROM BUD_BID_PROJECTS
                            INNER JOIN BUD_BID_STATUS
                            ON BUD_BID_PROJECTS.STATUS_ID = BUD_BID_STATUS.STATUS_ID
                            WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {2} AND BUD_BID_PROJECTS.VER_ID = {3} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP'
                        ),     
                        ORACLE_PROJECT_NAMES AS (
                            SELECT '{1}' AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '{0} (Org)' AS PROJECT_NAME, 'ORG' AS TYPE
                            FROM DUAL
                                UNION ALL
                            SELECT CAST(PROJECTS_V.PROJECT_ID AS varchar(20)) AS PROJECT_ID, PROJECTS_V.SEGMENT1 AS PROJECT_NUM, PROJECTS_V.LONG_NAME AS PROJECT_NAME, 'PROJECT' AS TYPE
                            FROM PROJECTS_V
                            LEFT JOIN PA.PA_PROJECT_CLASSES
                            ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                            WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                                UNION ALL
                            SELECT CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_ID, 'N/A' AS PROJECT_NUM, CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_NAME, 'ROLLUP' AS TYPE
                            FROM PROJECTS_V
                            LEFT JOIN PA.PA_PROJECT_CLASSES
                            ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                            WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup'
                            AND PA.PA_PROJECT_CLASSES.CLASS_CODE <> 'None' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                            GROUP BY CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) 
                        ),
                        BUDGET_LINE_AMOUNTS AS (
                            SELECT * FROM (           
                                SELECT BUD_BID_BUDGET_NUM.PROJECT_ID, BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {2} AND BUD_BID_PROJECTS.VER_ID = {3} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                            PIVOT(
                                SUM(NOV) FOR (LINE_ID)
                                IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                        ),
                        PREV_OP AS (                        
                            SELECT BUD_BID_PROJECTS.PROJECT_ID, NOV PREV_OP  
                            FROM BUD_BID_PROJECTS
                            LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                            LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                            WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {4} AND BUD_BID_PROJECTS.VER_ID = {5} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10        
                        ) 
                    SELECT CUR_PROJECT_INFO_WITH_STATUS.BUD_BID_PROJECTS_ID,
                        CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID, 
                        CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'OVERRIDE' THEN '-- OVERRIDE --'
                            WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'ORG' THEN 'N/A'
                            WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'ROLLUP' THEN 'N/A'
                            ELSE ORACLE_PROJECT_NAMES.PROJECT_NUM END PROJECT_NUM,                       
                        CUR_PROJECT_INFO_WITH_STATUS.TYPE,
                        CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'OVERRIDE' THEN CUR_PROJECT_INFO_WITH_STATUS.PRJ_NAME ELSE ORACLE_PROJECT_NAMES.PROJECT_NAME END PROJECT_NAME,
                        CUR_PROJECT_INFO_WITH_STATUS.STATUS, 
                        CUR_PROJECT_INFO_WITH_STATUS.ACRES, 
                        CUR_PROJECT_INFO_WITH_STATUS.DAYS,
                        BUDGET_LINE_AMOUNTS.GROSS_REC,
                        BUDGET_LINE_AMOUNTS.MAT_USAGE,
                        BUDGET_LINE_AMOUNTS.GROSS_REV,
                        BUDGET_LINE_AMOUNTS.DIR_EXP,
                        BUDGET_LINE_AMOUNTS.OP,                        
                        CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_AMOUNT ELSE (CASE WHEN PREV_OP.PREV_OP IS NULL THEN 0 ELSE PREV_OP.PREV_OP END) END PREV_OP,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REC = 0 THEN 0 ELSE ROUND(BUDGET_LINE_AMOUNTS.MAT_USAGE/BUDGET_LINE_AMOUNTS.GROSS_REC,2)*100 END MAT_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND(BUDGET_LINE_AMOUNTS.GROSS_REC/BUDGET_LINE_AMOUNTS.GROSS_REV,2)*100 END GR_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND(BUDGET_LINE_AMOUNTS.DIR_EXP/BUDGET_LINE_AMOUNTS.GROSS_REV,2)*100 END DIRECTS_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND(BUDGET_LINE_AMOUNTS.OP/BUDGET_LINE_AMOUNTS.GROSS_REV,2)*100 END OP_PERC,       
                        BUDGET_LINE_AMOUNTS.OP - (CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_AMOUNT ELSE (CASE WHEN PREV_OP.PREV_OP IS NULL THEN 0 ELSE PREV_OP.PREV_OP END) END) OP_VAR    
                    FROM CUR_PROJECT_INFO_WITH_STATUS
                    LEFT OUTER JOIN ORACLE_PROJECT_NAMES ON CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID = ORACLE_PROJECT_NAMES.PROJECT_ID AND CUR_PROJECT_INFO_WITH_STATUS.TYPE = ORACLE_PROJECT_NAMES.TYPE
                    LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON CUR_PROJECT_INFO_WITH_STATUS.BUD_BID_PROJECTS_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID
                    LEFT OUTER JOIN PREV_OP ON CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID = PREV_OP.PROJECT_ID
                    ORDER BY LOWER(PROJECT_NAME)", orgName, orgID, yearID, verID, prevYearID, prevVerID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).ToList();                    
                }
            }
        }

        public class Subtotals
        {
            #region Fields
            public class Fields
            {
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
                public decimal COMPARE_PRJ_AMOUNT { get; set; }
                public decimal PREV_OP { get; set; }
                public decimal OP_VAR { get; set; }
            }
            #endregion

            public static Fields Data(bool active, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string statusID = active == true ? "<>" : "=";
                string sql = string.Format(@"
                        WITH
                            CUR_PROJECT_INFO AS(
                                SELECT BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.STATUS_ID, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT
                                FROM BUD_BID_PROJECTS
                                WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP'
                            ),     
                            BUDGET_LINE_AMOUNTS AS (
                                SELECT * FROM (           
                                    SELECT BUD_BID_BUDGET_NUM.PROJECT_ID, BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV
                                    FROM BUD_BID_PROJECTS
                                    LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                    LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                    WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                                PIVOT(
                                    SUM(NOV) FOR (LINE_ID)
                                    IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                            ),
                            PREV_OP AS (                       
                              SELECT BUD_BID_PROJECTS.PROJECT_ID, NOV PREV_OP  
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {3} AND BUD_BID_PROJECTS.VER_ID = {4} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10        
                            )    
                            SELECT NVL(SUM(BUDGET_LINE_AMOUNTS.GROSS_REC),0) GROSS_REC,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.MAT_USAGE),0) MAT_USAGE,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.GROSS_REV),0) GROSS_REV,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.DIR_EXP),0) DIR_EXP,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.OP),0) OP,
                                NVL(SUM(CUR_PROJECT_INFO.COMPARE_PRJ_AMOUNT),0) COMPARE_PRJ_AMOUNT,     
                                NVL(SUM(CASE WHEN CUR_PROJECT_INFO.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO.COMPARE_PRJ_AMOUNT ELSE PREV_OP.PREV_OP END),0) PREV_OP,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.OP),0) - NVL(SUM(CASE WHEN CUR_PROJECT_INFO.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO.COMPARE_PRJ_AMOUNT ELSE PREV_OP.PREV_OP END),0) OP_VAR 
                            FROM CUR_PROJECT_INFO
                            LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON CUR_PROJECT_INFO.BUD_BID_PROJECTS_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID
                            LEFT OUTER JOIN PREV_OP ON CUR_PROJECT_INFO.PROJECT_ID = PREV_OP.PROJECT_ID
                            WHERE CUR_PROJECT_INFO.STATUS_ID " + statusID + " 45", orgID, yearID, verID, prevYearID, prevVerID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        } 
    }
    
    public class BBAdjustments
    {
        #region Fields
        public class Fields
        {
            public long ADJ_ID { get; set; }
            public string ADJUSTMENT { get; set; }
            public decimal? MAT_ADJ { get; set; }
            public decimal? WEATHER_ADJ { get; set; }
        }
        #endregion

        public static long Count(long orgID, long yearID, long verID)
        {
            using (Entities context = new Entities())
            {
                return context.BUD_BID_ADJUSTMENT.Where(x => x.ORG_ID == orgID && x.YEAR_ID == yearID && x.VER_ID == verID).Count();
            }
        }
        public static void Create(long orgID, long yearID, long verID)
        {
            BUD_BID_ADJUSTMENT data = new BUD_BID_ADJUSTMENT();

            // Material
            data.ORG_ID = orgID;
            data.YEAR_ID = yearID;
            data.VER_ID = verID;
            data.MAT_ADJ = 0;
            data.WEATHER_ADJ = null;
            data.CREATE_DATE = DateTime.Now;
            data.CREATED_BY = HttpContext.Current.User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
            GenericData.Insert<BUD_BID_ADJUSTMENT>(data);

            // Weather
            data.MAT_ADJ = null;
            data.WEATHER_ADJ = 0;
            GenericData.Insert<BUD_BID_ADJUSTMENT>(data);
        }
        public static List<Fields> Data(long orgID, long yearID, long verID)
        {
            string sql = string.Format(@"
                SELECT ADJ_ID,
                    CASE WHEN MAT_ADJ IS NOT NULL THEN 'Material Adjustment' ELSE 'Weather Adjustment' END ADJUSTMENT,
                    MAT_ADJ,
                    WEATHER_ADJ
                FROM BUD_BID_ADJUSTMENT
                WHERE ORG_ID = {0} AND YEAR_ID = {1} AND VER_ID = {2}
                ORDER BY ADJUSTMENT", orgID, yearID, verID);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<Fields>(sql).ToList();
            }
        }

        public class Subtotal
        {
            #region Fields
            public class Fields
            {
                public decimal MAT_ADJ { get; set; }
                public decimal WEATHER_ADJ { get; set; }
            }
            #endregion

            public static Fields Data(long orgID, long yearID, long verID)
            {
                string sql = string.Format(@"
                    SELECT NVL(SUM(MAT_ADJ),0) MAT_ADJ,
                        NVL(SUM(WEATHER_ADJ),0) WEATHER_ADJ
                    FROM 
                    (
                        SELECT MAT_ADJ,
                            WEATHER_ADJ
                        FROM BUD_BID_ADJUSTMENT
                        WHERE ORG_ID = {0} AND YEAR_ID = {1} AND VER_ID = {2}
                    )", orgID, yearID, verID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }  
    }

    public class BBOH
    {
        #region Fields
        public class Fields
        {
            public string ADJUSTMENT { get; set; }
            public decimal OH { get; set; }
        }
        #endregion

        public static List<Fields> Data(long orgID, long yearID, long verID)
        {
            string sql = string.Format(@"
                SELECT 'Overhead' ADJUSTMENT,
                    50000 OH
                FROM DUAL", orgID, yearID, verID);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<Fields>(sql).ToList();
            }
        }

        public class Subtotal
        {
            #region Fields
            public class Fields
            {
                public string ADJUSTMENT { get; set; }
                public decimal OH { get; set; }
            }
            #endregion

            public static Fields Data(long orgID, long yearID, long verID)
            {
                string sql = string.Format(@"
                    SELECT 'Overhead' ADJUSTMENT,
                        50000 OH
                    FROM DUAL", orgID, yearID, verID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }  
    }

    public class BBProject
    {
        public static long Count(long orgID, long yearID, long verID, string projectID, long curBudBidID)
        {
            using (Entities context = new Entities())
            {
                return context.BUD_BID_PROJECTS.Where(x => x.MODIFIED_BY != "TEMP" && x.BUD_BID_PROJECTS_ID != curBudBidID && x.ORG_ID == orgID && x.YEAR_ID == yearID && x.VER_ID == verID && x.PROJECT_ID == projectID).Count();
            }
        }
        public static void DBDelete(long budBidID)
        {
            BUD_BID_PROJECTS projectData;
            List<BUD_BID_ACTUAL_NUM> actualData;
            List<BUD_BID_BUDGET_NUM> budgetData;
            List<BUD_BID_DETAIL_SHEET> detailSheetData;
            List<BUD_BID_DETAIL_TASK> taskInfoData;

            using (Entities context = new Entities())
            {
                projectData = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
                actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID).ToList();
                taskInfoData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }

            GenericData.Delete<BUD_BID_PROJECTS>(projectData);
            GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
            GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);
            GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
            GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData);
        }
        public static List<DoubleComboLongID> Statuses()
        {
            string sql = "SELECT STATUS_ID ID, STATUS ID_NAME FROM BUD_BID_STATUS ORDER BY STATUS";

            using (Entities context = new Entities())
            {     
                return context.Database.SqlQuery<DoubleComboLongID>(sql).ToList();
            }
        }

        public class StartNumbers
        {
            #region Fields
            public class Fields
            {
                public long PROJECT_ID { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static Fields Data(long projectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_ACTUAL_NUM.PROJECT_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_ACTUAL_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_ACTUAL_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'  )
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", projectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }

        public class EndNumbers
        {
            #region Fields
            public class Fields
            {
                public long PROJECT_ID { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static Fields Data(long projectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_BUDGET_NUM.PROJECT_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'  )
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", projectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }  

        public class EndNumbersW0
        {
            public static Fields Data(long budBidProjectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_DETAIL_TASK.PROJECT_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", budBidProjectID);

                Fields data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }

                if (data == null)
                {
                    Fields nullData = new Fields();
                    nullData.GROSS_REC = 0;
                    nullData.MAT_USAGE = 0;
                    nullData.GROSS_REV = 0;
                    nullData.DIR_EXP = 0;
                    nullData.OP = 0;
                    data = nullData;
                }

                return data;
            }
            public static void DBUpdate(long budBidProjectID, decimal eGrossRec, decimal eMaterial, decimal eGrossRev, decimal eDirects, decimal eOP)
            {
                string sql = string.Format(@"
                    SELECT *
                    FROM BUD_BID_DETAIL_TASK
                    LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                    WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
                    ORDER BY LINE_ID", budBidProjectID);
                long[] lineNum = { 6, 7, 8, 9, 10 };
                decimal[] endNum = { eGrossRec, eMaterial, eGrossRev, eDirects, eOP };

                List<BUD_BID_BUDGET_NUM> data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<BUD_BID_BUDGET_NUM>(sql).ToList();
                }

                int a = 0;
                foreach (BUD_BID_BUDGET_NUM field in data)
                {
                    field.NOV = endNum[a];
                    field.MODIFY_DATE = DateTime.Now;
                    field.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                    a++;
                }

                GenericData.Update<BUD_BID_BUDGET_NUM>(data);
            }

            #region Fields
            public class Fields
            {
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion
        }

        public class Listing
        {
            #region Fields
            public class Fields
            {
                public string PROJECT_ID { get; set; }
                public string PROJECT_NUM { get; set; }
                public string PROJECT_NAME { get; set; }
                public string TYPE { get; set; }
                public string ORDERKEY { get; set; }
            }
            #endregion

            public static List<Fields> Data(long orgID, string orgName)
            {
                string sql = string.Format(@"
                    SELECT TO_CHAR(SYSDATE, 'YYMMDDHH24MISS') AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '-- OVERRIDE --' AS PROJECT_NAME, 'OVERRIDE' AS TYPE, 'ID1' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT '{1}' AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '{0} (Org)' AS PROJECT_NAME, 'ORG' AS TYPE, 'ID2' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT CAST(PROJECTS_V.PROJECT_ID AS VARCHAR(20)) AS PROJECT_ID, PROJECTS_V.SEGMENT1 AS PROJECT_NUM, PROJECTS_V.LONG_NAME AS PROJECT_NAME, 'PROJECT' AS TYPE, 'ID3' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN PA.PA_PROJECT_CLASSES
                    ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                        UNION ALL
                    SELECT CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_ID, 'N/A' AS PROJECT_NUM, CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_NAME, 'ROLLUP' AS TYPE, 'ID4' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN PA.PA_PROJECT_CLASSES
                    ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PA.PA_PROJECT_CLASSES.CLASS_CODE <> 'None' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                    GROUP BY CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) 
                    ORDER BY ORDERKEY, PROJECT_NAME", orgName, orgID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).ToList();
                }
            }        
        }

        public class OP
        {
            #region Fields
            public class Fields
            {
                public decimal OP { get; set; }
            }
            #endregion

            public static Fields Data(long orgID, long prevYearID, long prevVerID, long projectID)
            {
                string sql = string.Format(@"
                    SELECT NOV OP 
                    FROM BUD_BID_PROJECTS
                    LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                    LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                    WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.PROJECT_ID = {3} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10", orgID, prevYearID, prevVerID, projectID);

                Fields data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }

                if (data == null)
                {
                    Fields nullData = new Fields();
                    nullData.OP = 0;
                    data = nullData;
                }

                return data;
            }
            public static Fields OverridenOP(long budBidprojectID)
            {
                string sql = string.Format(@"
                    SELECT COMPARE_PRJ_AMOUNT OP
                    FROM BUD_BID_PROJECTS
                    WHERE BUD_BID_PROJECTS_ID = {0}", budBidprojectID);

                Fields data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }

                if (data == null)
                {
                    Fields nullData = new Fields();
                    nullData.OP = 0;
                    data = nullData;
                }
                    
                return data;                    
            }
        }

        public class Detail
        {
            #region Fields
            public class Fields
            {
                public string PROJECT_ID { get; set; }
                public long BUD_BID_PROJECTS_ID { get; set; }
                public string TYPE { get; set; }
                public string PROJECT_NAME { get; set; }
                public long STATUS_ID { get; set; }
                public string STATUS { get; set; }
                public decimal ACRES { get; set; }
                public decimal DAYS { get; set; }
                public string APP_TYPE { get; set; }
                public string CHEMICAL_MIX { get; set; }
                public string COMMENTS { get; set; }
                public string LIABILITY { get; set; }
                public decimal LIABILITY_OP { get; set; }
                public string COMPARE_PRJ_OVERRIDE { get; set; }
                public decimal COMPARE_PRJ_AMOUNT { get; set; }
                public DateTime? WE_DATE { get; set; }
                public string WE_OVERRIDE { get; set; }
            }
            #endregion

            public static Fields Data(long budBidProjectID)
            {
                string sql = string.Format(@"
                    SELECT BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.TYPE, BUD_BID_PROJECTS.PRJ_NAME PROJECT_NAME, BUD_BID_STATUS.STATUS_ID,
                        BUD_BID_STATUS.STATUS, BUD_BID_PROJECTS.ACRES, BUD_BID_PROJECTS.DAYS, BUD_BID_PROJECTS.APP_TYPE, BUD_BID_PROJECTS.CHEMICAL_MIX, BUD_BID_PROJECTS.COMMENTS,
                        BUD_BID_PROJECTS.LIABILITY, BUD_BID_PROJECTS.LIABILITY_OP, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT, BUD_BID_PROJECTS.WE_DATE,
                        BUD_BID_PROJECTS.WE_OVERRIDE
                    FROM BUD_BID_PROJECTS
                    INNER JOIN BUD_BID_STATUS
                    ON BUD_BID_PROJECTS.STATUS_ID = BUD_BID_STATUS.STATUS_ID
                    WHERE BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = {0}", budBidProjectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }
    }
    
    public class BBDetail
    {
        public class MainGrid
        {
            #region Fields
            public class Fields
            {
                public long DETAIL_TASK_ID { get; set; }
                public string DETAIL_NAME { get; set; }
                public long SHEET_ORDER { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static List<Fields> Data(long projectID)
            {
                string sql = string.Format(@"
                    WITH
                        TASKS AS (            
                            SELECT PROJECT_ID, DETAIL_TASK_ID, DETAIL_NAME, SHEET_ORDER, MODIFIED_BY
                            FROM BUD_BID_DETAIL_TASK
                            WHERE PROJECT_ID = {0}
                        ),          
                        BUDGET_LINE_AMOUNTS AS (
                            SELECT * FROM (
                                SELECT PROJECT_ID, DETAIL_TASK_ID, LINE_ID, NOV, MODIFIED_BY
                                FROM BUD_BID_BUDGET_NUM
                                WHERE PROJECT_ID = {0})
                            PIVOT(
                                SUM(NOV) FOR (LINE_ID)
                                IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                        )         
                    SELECT TASKS.PROJECT_ID,
                        TASKS.DETAIL_TASK_ID,
                        TASKS.DETAIL_NAME,
                        TASKS.SHEET_ORDER,
                        BUDGET_LINE_AMOUNTS.GROSS_REC,
                        BUDGET_LINE_AMOUNTS.MAT_USAGE,
                        BUDGET_LINE_AMOUNTS. GROSS_REV,
                        BUDGET_LINE_AMOUNTS.DIR_EXP,
                        BUDGET_LINE_AMOUNTS.OP
                    FROM TASKS 
                    INNER JOIN BUDGET_LINE_AMOUNTS ON TASKS.PROJECT_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID AND TASKS.DETAIL_TASK_ID = BUDGET_LINE_AMOUNTS.DETAIL_TASK_ID
                    WHERE TASKS.DETAIL_NAME <> 'SYS_PROJECT'
                    ORDER BY SHEET_ORDER", projectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).ToList();
                }
            }
        }

        public class Sheet
        {
            public static long ID(long budBidProjectID, long order)
            {
                BUD_BID_DETAIL_TASK data;
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.SHEET_ORDER == (order)).SingleOrDefault();
                }

                if (data == null)
                {
                    return 0;
                }

                return Convert.ToInt64(data.DETAIL_TASK_ID);
            }
            public static void DBDelete(long detailSheetID)
            {
                List<BUD_BID_ACTUAL_NUM> actualData;
                List<BUD_BID_BUDGET_NUM> budgetData;
                List<BUD_BID_DETAIL_SHEET> detailSheetData;
                List<BUD_BID_DETAIL_TASK> taskInfoData;

                using (Entities context = new Entities())
                {
                    actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    taskInfoData = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                }

                GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
                GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);
                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
                GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData);
            }
            public static long DBAdd(long budBidID, long sheetOrder)
            {
                BUD_BID_DETAIL_TASK detailData = new BUD_BID_DETAIL_TASK();
                detailData.PROJECT_ID = budBidID;
                detailData.DETAIL_NAME = "SYS_DETAIL_" + sheetOrder;
                detailData.SHEET_ORDER = sheetOrder;
                detailData.CREATE_DATE = DateTime.Now;
                detailData.CREATED_BY = HttpContext.Current.User.Identity.Name;
                detailData.MODIFY_DATE = DateTime.Now;
                detailData.MODIFIED_BY = "TEMP";
                GenericData.Insert<BUD_BID_DETAIL_TASK>(detailData);

                long newDetailID = Convert.ToInt64(detailData.DETAIL_TASK_ID);

                long[] arrLineNum = { 6, 7, 8, 9, 10 };
                List<BUD_BID_BUDGET_NUM> budNumsData = new List<BUD_BID_BUDGET_NUM>();
                for (int i = 0; i <= 4; i++)
                {
                    budNumsData.Add(new BUD_BID_BUDGET_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = newDetailID,
                        LINE_ID = arrLineNum[i],
                        NOV = 0,
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = HttpContext.Current.User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = "TEMP"
                    });
                }
                GenericData.Insert<BUD_BID_BUDGET_NUM>(budNumsData);

                return newDetailID;
            }
            public static void DBUpdateName(long detailSheetID, string sheetName)
            {
                BUD_BID_DETAIL_TASK data;

                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailSheetID).Single();
                }

                data.DETAIL_NAME = sheetName;

                GenericData.Update<BUD_BID_DETAIL_TASK>(data);
            }

            public class MainTabField
            {

                public static Fields Data(long detailSheetID)
                {
                    string sql = string.Format(@"
                        SELECT * FROM (
                            SELECT DETAIL_TASK_ID, REC_TYPE, TOTAL
                            FROM BUD_BID_DETAIL_SHEET
                            WHERE DETAIL_TASK_ID = {0})
                        PIVOT(
                            SUM(TOTAL) FOR (REC_TYPE)
                            IN ('RECREMAIN' RECREMAIN, 'DAYSREMAIN' DAYSREMAIN, 'UNITREMAIN' UNITREMAIN, 'DAYSWORKED' DAYSWORKED))", detailSheetID);

                    Fields data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        Fields nullData = new Fields();
                        nullData.RECREMAIN = 0;
                        nullData.DAYSREMAIN = 0;
                        nullData.UNITREMAIN = 0;
                        nullData.DAYSWORKED = 0;
                        return nullData;
                    }

                    return data;
                }
                public static void DBUpdate(long budBidProjectID, long detailSheetID, string recType, decimal amount)
                {
                    BUD_BID_DETAIL_SHEET data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_TASK_ID == detailSheetID && x.REC_TYPE == recType).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        data = new BUD_BID_DETAIL_SHEET();
                        data.PROJECT_ID = budBidProjectID;
                        data.REC_TYPE = recType;
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = amount;
                        data.CREATE_DATE = DateTime.Now;
                        data.CREATED_BY = HttpContext.Current.User.Identity.Name;
                        data.MODIFY_DATE = DateTime.Now;
                        data.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                        data.DETAIL_TASK_ID = detailSheetID;
                        GenericData.Insert<BUD_BID_DETAIL_SHEET>(data);
                    }

                    else
                    {
                        data.TOTAL = amount;
                        data.MODIFY_DATE = DateTime.Now;
                        data.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                }

                #region Fields
                public class Fields
                {
                    public decimal? RECREMAIN { get; set; }
                    public decimal? DAYSREMAIN { get; set; }
                    public decimal? UNITREMAIN { get; set; }
                    public decimal? DAYSWORKED { get; set; }
                }
                #endregion
            }

            public class Subtotals
            {

                public static Fields Get(long detailSheetID)
                {
                    string sql = string.Format(@"
                        SELECT * FROM (
                            SELECT DETAIL_TASK_ID, REC_TYPE, TOTAL
                            FROM BUD_BID_DETAIL_SHEET
                            WHERE DETAIL_TASK_ID = {0})
                        PIVOT(
                            SUM(TOTAL) FOR (REC_TYPE)
                            IN ('MATERIAL' MATERIAL, 'EQUIPMENT' EQUIPMENT, 'PERSONNEL' PERSONNEL, 'PERDIEM' PERDIEM,
                                'TRAVEL' TRAVEL, 'MOTELS' MOTELS, 'MISC' MISC, 'LUMPSUM' LUMPSUM))", detailSheetID);

                    Fields data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        Fields nullData = new Fields();
                        nullData.MATERIAL = 0;
                        nullData.EQUIPMENT = 0;
                        nullData.PERSONNEL = 0;
                        nullData.PERDIEM = 0;
                        nullData.TRAVEL = 0;
                        nullData.MOTELS = 0;
                        nullData.MISC = 0;
                        nullData.LUMPSUM = 0;
                        return nullData;
                    }

                    return data;
                }

                #region Fields
                public class Fields
                {
                    public decimal? MATERIAL { get; set; }
                    public decimal? EQUIPMENT { get; set; }
                    public decimal? PERSONNEL { get; set; }
                    public decimal? PERDIEM { get; set; }
                    public decimal? TRAVEL { get; set; }
                    public decimal? MOTELS { get; set; }
                    public decimal? MISC { get; set; }
                    public decimal? LUMPSUM { get; set; }
                }
                #endregion
            }

            public class BottomNumbers
            {
                public static Fields Calculate(long detailSheetID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP, decimal totalDaysRemain, 
                    decimal totalUnitsRemain, decimal totalDaysWorked)
                {
                    BBDetail.Sheet.Subtotals.Fields subtotal = BBDetail.Sheet.Subtotals.Get(detailSheetID);
                    decimal totalMaterial = subtotal.MATERIAL ?? 0;
                    decimal totalEquipment = subtotal.EQUIPMENT ?? 0;
                    decimal totalPersonnel = subtotal.PERSONNEL ?? 0;
                    decimal totalPerDiem = subtotal.PERDIEM ?? 0;
                    decimal totalTravel = subtotal.TRAVEL ?? 0;
                    decimal totalMotels = subtotal.MOTELS ?? 0;
                    decimal totalMisc = subtotal.MISC ?? 0;
                    decimal totalLumpSum = subtotal.LUMPSUM ?? 0;

                    decimal laborBurdenRate = .40M;  //FIX
                    decimal laborBurden = (totalPersonnel + totalTravel) * laborBurdenRate;
                    decimal totalWklyDirects = totalEquipment + totalPersonnel + totalPerDiem + totalTravel + totalMotels + totalMisc + laborBurden;
                    decimal totalDirectsPerDay = totalDaysWorked == 0 ? 0 : totalWklyDirects / totalDaysWorked;
                    decimal avgUnitsPerDay = totalDaysRemain == 0 ? 0 : totalUnitsRemain / totalDaysRemain;
                    decimal totalDirectsLeft = (totalDirectsPerDay * totalDaysRemain) + totalLumpSum;
                    decimal totalMaterialLeft = totalMaterial * totalUnitsRemain;

                    Fields returnData = new Fields();
                    returnData.LABOR_BURDEN = laborBurden;
                    returnData.TOTAL_WKLY_DIRECTS = totalWklyDirects;
                    returnData.TOTAL_DIRECTS_PER_DAY = totalDirectsPerDay;
                    returnData.AVG_UNITS_PER_DAY = avgUnitsPerDay;
                    returnData.TOTAL_DIRECTS_LEFT = totalDirectsLeft;
                    returnData.TOTAL_MATERIAL_LEFT = totalMaterialLeft;
                    return returnData;
                }

                #region Fields
                public class Fields
                {
                    public decimal LABOR_BURDEN { get; set; }
                    public decimal TOTAL_WKLY_DIRECTS { get; set; }
                    public decimal TOTAL_DIRECTS_PER_DAY { get; set; }
                    public decimal AVG_UNITS_PER_DAY { get; set; }
                    public decimal TOTAL_DIRECTS_LEFT { get; set; }
                    public decimal TOTAL_MATERIAL_LEFT { get; set; }
                }
                #endregion
            }

            public class EndNumbers
            {
                public static Fields Get(long detailSheetID)
                {
                    string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_DETAIL_TASK.DETAIL_TASK_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = {0})
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", detailSheetID);

                    Fields data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        Fields nullData = new Fields();
                        nullData.GROSS_REC = 0;
                        nullData.MAT_USAGE = 0;
                        nullData.GROSS_REV = 0;
                        nullData.DIR_EXP = 0;
                        nullData.OP = 0;
                        return nullData;
                    }

                    return data;
                }
                public static void DBUpdate(long detailSheetID, decimal eGrossRec, decimal eMaterial, decimal eGrossRev, decimal eDirects, decimal eOP)
                {
                    long[] lineNum = { 6, 7, 8, 9, 10 };
                    decimal[] endNum = { eGrossRec, eMaterial, eGrossRev, eDirects, eOP };

                    List<BUD_BID_BUDGET_NUM> data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_BUDGET_NUM.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    }

                    int a = 0;
                    foreach (BUD_BID_BUDGET_NUM field in data)
                    {
                        field.NOV = endNum[a];
                        field.MODIFY_DATE = DateTime.Now;
                        field.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                        a++;
                    }

                    GenericData.Update<BUD_BID_BUDGET_NUM>(data);
                }
                public static Fields Calculate(long detailSheetID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP,  decimal totalReceiptsRemain, 
                    decimal totalDaysRemain, decimal totalUnitsRemain, decimal totalDaysWorked)
                {
                    BBDetail.Sheet.BottomNumbers.Fields data = BBDetail.Sheet.BottomNumbers.Calculate(detailSheetID, sGrossRec, sMaterial, sGrossRev, sDirects, sOP, 
                        totalDaysRemain, totalUnitsRemain, totalDaysWorked);
                    decimal totalMatLeft = data.TOTAL_MATERIAL_LEFT;
                    decimal totalDirectsLeft = data.TOTAL_DIRECTS_LEFT;

                    decimal eGrossRec = sGrossRec + totalReceiptsRemain;
                    decimal eMaterial = sMaterial + totalMatLeft;
                    decimal eGrossRev = eGrossRec - eMaterial;
                    decimal eDirects = sDirects + totalDirectsLeft;
                    decimal eOP = eGrossRev - eDirects;

                    Fields endNums = new Fields();
                    endNums.GROSS_REC = eGrossRec;
                    endNums.MAT_USAGE = eMaterial;
                    endNums.GROSS_REV = eGrossRev;
                    endNums.DIR_EXP = eDirects;
                    endNums.OP = eOP;
                    return endNums;
                }
                
                #region Fields
                public class Fields
                {
                    public decimal GROSS_REC { get; set; }
                    public decimal MAT_USAGE { get; set; }
                    public decimal GROSS_REV { get; set; }
                    public decimal DIR_EXP { get; set; }
                    public decimal OP { get; set; }
                }
                #endregion
            }
        }

        public class Sheets
        {
            public static long MaxOrder(long budBidProjectID)
            {
                BUD_BID_DETAIL_TASK data;
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID).OrderByDescending(y => y.SHEET_ORDER).FirstOrDefault();
                }

                if (data == null)
                {
                    return 0;
                }

                return Convert.ToInt64(data.SHEET_ORDER);
            }
            public static void DBResetOrder(long budBidProjectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM BUD_BID_DETAIL_TASK
                    WHERE PROJECT_ID = {0} AND DETAIL_NAME <> 'SYS_PROJECT'
                    ORDER BY SHEET_ORDER", budBidProjectID);

                List<BUD_BID_DETAIL_TASK> data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<BUD_BID_DETAIL_TASK>(sql).ToList();
                }

                int i = 1;
                foreach (BUD_BID_DETAIL_TASK field in data)
                {
                    field.SHEET_ORDER = i;
                    i++;
                    GenericData.Update<BUD_BID_DETAIL_TASK>(data);
                }                
            }

            public class EndNumbers
            {
                public static void DBCalculate(long budBidProjectID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP)
                {
                    List<BUD_BID_DETAIL_TASK> data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_NAME != "SYS_PROJECT").OrderBy(y => y.SHEET_ORDER).ToList();
                    }

                    long detailSheetID;
                    decimal eGrossRec = sGrossRec;
                    decimal eMatUsage = sMaterial;
                    decimal eGrossRev = sGrossRev;
                    decimal eDirects = sDirects;
                    decimal eOP = sOP;
                    foreach (BUD_BID_DETAIL_TASK field in data)
                    {
                        detailSheetID = Convert.ToInt64(field.DETAIL_TASK_ID);

                        BBDetail.Sheet.MainTabField.Fields mainTabData = BBDetail.Sheet.MainTabField.Data(detailSheetID);
                        decimal totalReceiptsRemain = mainTabData.RECREMAIN ?? 0;
                        decimal totalDaysRemain = mainTabData.DAYSREMAIN ?? 0;
                        decimal totalUnitsRemain = mainTabData.UNITREMAIN ?? 0;
                        decimal totalDaysWorked = mainTabData.DAYSWORKED ?? 0;

                        BBDetail.Sheet.EndNumbers.Fields endNums = BBDetail.Sheet.EndNumbers.Calculate(detailSheetID, sGrossRec, sMaterial, sGrossRev, sDirects, sOP,  
                            totalReceiptsRemain, totalDaysRemain,  totalUnitsRemain,  totalDaysWorked);
                        eGrossRec = endNums.GROSS_REC;
                        eMatUsage = endNums.MAT_USAGE;
                        eGrossRev = endNums.GROSS_REV;
                        eDirects = endNums.DIR_EXP;
                        eOP = endNums.OP;
                        sGrossRec = eGrossRec;
                        sMaterial = eMatUsage;
                        sGrossRev = eGrossRev;
                        sDirects = eDirects;
                        sOP = eOP;

                        BBDetail.Sheet.EndNumbers.DBUpdate(detailSheetID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);
                    }

                    BBProject.EndNumbersW0.DBUpdate(budBidProjectID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);
                }
                #region Fields
                public class Fields
                {
                    public decimal GROSS_REC { get; set; }
                    public decimal MAT_USAGE { get; set; }
                    public decimal GROSS_REV { get; set; }
                    public decimal DIR_EXP { get; set; }
                    public decimal OP { get; set; }
                }
                #endregion
            }
        }

        public class SubGrid
        {
            public static void DeleteRecord(long detailSheetID)
            {
                BUD_BID_DETAIL_SHEET detailSheetData;

                using (Entities context = new Entities())
                {
                    detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == detailSheetID).SingleOrDefault();
                }

                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
            }

            public class Data
            {
                public static List<Fields> Get(long projectID, long detailSheetID, string recType)
                {
                    string sql = string.Format(@"
                            SELECT DETAIL_SHEET_ID,
                                PROJECT_ID,
                                DETAIL_TASK_ID,
                                REC_TYPE,
                                DESC_1,
                                DESC_2,
                                AMT_1,
                                AMT_2,
                                AMT_3,
                                AMT_4,
                                AMT_5,
                                TOTAL
                            FROM BUD_BID_DETAIL_SHEET 
                            WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND REC_TYPE = '{2}'
                            ORDER BY DETAIL_SHEET_ID DESC", projectID, detailSheetID, recType);

                    List<Fields> data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).ToList();                        
                    }

                    return data;
                }
                #region Fields
                public class Fields
                {
                    public long DETAIL_SHEET_ID { get; set; }
                    public long PROJECT_ID { get; set; }
                    public long DETAIL_TASK_ID { get; set; }
                    public string REC_TYPE { get; set; }
                    public string DESC_1 { get; set; }
                    public string DESC_2 { get; set; }
                    public decimal? AMT_1 { get; set; }
                    public decimal? AMT_2 { get; set; }
                    public decimal? AMT_3 { get; set; }
                    public decimal? AMT_4 { get; set; }
                    public decimal? AMT_5 { get; set; }
                    public decimal? TOTAL { get; set; }
                }
                #endregion
            }

        }
    }
}
