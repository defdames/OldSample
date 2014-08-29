using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;
using System.Web;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    //public class CrystalReportClass
    //{
    //    #region Variables
    //    long BB_PROJ_ID;
    //    string PROJ_ID;
    //    string PROJ_NUM;
    //    string Type;
    //    string PROJ_NAME;
    //    string Status;
    //    decimal Acres;
    //    decimal Days;
    //    decimal GRS_REC;
    //    decimal MAT_USE;
    //    decimal GRS_REV;
    //    decimal DR_EXP;
    //    decimal op;
    //    decimal PRE_OP;
    //    decimal MAT_PER;
    //    decimal GR_PER;
    //    decimal DIR_PER;
    //    decimal OP_PER;
    //    decimal OP_VA;
    //    #endregion

    //    #region Cons
    //    public CrystalReportClass()
    //    {
        
    //    }
    //    #endregion

    //    #region Fields

    //            public long BUD_BID_PROJECTS_ID
    //            {
    //                get { return BB_PROJ_ID; }
    //                set { BB_PROJ_ID = value; }
    //            }
    //                    public string PROJECT_ID 
    //            {
    //                get { return PROJ_ID;}
    //                set { PROJ_ID = value;} 
    //            }
    //                    public string PROJECT_NUM
    //            {
    //                get { return PROJ_NUM;}
    //                set { PROJ_NUM = value ;} 
    //            }
    //            public string TYPE
    //            {
    //                get { return Type; }
    //                set { Type = value; }
    //            }
    //            public string PROJECT_NAME
    //            {   get { return PROJ_NAME; }
    //                set { PROJ_NAME = value; } 
    //            }
    //            public string STATUS
    //            {
    //                get { return Status; }
    //                set { Status = value; } 
    //            }
    //            public decimal ACRES 
    //            {
    //                get { return Acres; }
    //                set { Acres = value;} 
    //            }
    //            public decimal DAYS 
    //            {
    //                get { return Days; }
    //                set { Days = value; } 
    //            }
    //            public decimal GROSS_REC 
    //            {
    //                get { return GRS_REC; }
    //                set { GRS_REC = value; } 
    //            }
    //            public decimal MAT_USAGE 
    //            {
    //                get { return MAT_USE; }
    //                set { MAT_USE = value; } 
    //            }
    //            public decimal GROSS_REV 
    //            {
    //                get { return GRS_REV; }
    //                set { GRS_REV = value; } 
    //            }
    //            public decimal DIR_EXP 
    //            {
    //                get { return DR_EXP; }
    //                set { DR_EXP = value; } 
    //            }
    //            public decimal OP 
    //            {
    //                get { return op; }
    //                set { op = value; } 
    //            }
    //            public decimal PREV_OP 
    //            {
    //                get { return PRE_OP; }
    //                set { PRE_OP = value; } 
    //            }
    //            public decimal MAT_PERC 
    //            {
    //                get { return MAT_PER; }
    //                set { MAT_PER = value; } 
    //            }
    //            public decimal GR_PERC 
    //            {
    //                get { return GR_PER; }
    //                set { GR_PER = value; } 
    //            }
    //            public decimal DIRECTS_PERC 
    //            {
    //                get { return DIR_PER; }
    //                set { DIR_PER = value; } 
    //            }
    //            public decimal OP_PERC 
    //            {
    //                get { return OP_PER; }
    //                set { OP_PER = value; } 
    //            }
    //            public decimal OP_VAR 
    //            {
    //                get { return OP_VA; }
    //                set { OP_VA = value; } 
    //            }
            
    //        #endregion
    //}
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
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REC = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.MAT_USAGE/BUDGET_LINE_AMOUNTS.GROSS_REC) * 100, 2) END MAT_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.GROSS_REC/BUDGET_LINE_AMOUNTS.GROSS_REV) * 100, 2) END GR_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.DIR_EXP/BUDGET_LINE_AMOUNTS.GROSS_REV) * 100, 2) END DIRECTS_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.OP/BUDGET_LINE_AMOUNTS.GROSS_REV) * 100, 2) END OP_PERC,       
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
}