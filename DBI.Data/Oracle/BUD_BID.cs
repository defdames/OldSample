using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;

namespace DBI.Data
{
    public class BUD_BID
    {
        /// <summary>
        /// Returns list of projects also containing org and override for a given org
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public static List<BUD_BID.BUD_SUMMARY_V> ProjectList(long orgID, string orgName)
        {
            using (Entities context = new Entities())
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

                List<BUD_BID.BUD_SUMMARY_V> data = context.Database.SqlQuery<BUD_BID.BUD_SUMMARY_V>(sql).ToList();
                return data;
            }
        }

        /// <summary>
        /// Returns list of projects along with project summary line info for a given org
        /// </summary>
        /// <param name="orgName"></param>
        /// <param name="orgID"></param>
        /// <param name="yearID"></param>
        /// <param name="verID"></param>
        /// <param name="prevYearID"></param>
        /// <param name="prevVerID"></param>
        /// <returns></returns>
        public static List<BUD_BID.BUD_SUMMARY_V> SummaryProjectsWithLineInfo(string orgName, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format(@"WITH
                    CUR_PROJECT_INFO_WITH_STATUS AS(
                        SELECT BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.TYPE, BUD_BID_PROJECTS.PRJ_NAME, BUD_BID_STATUS.STATUS,
                            BUD_BID_PROJECTS.ACRES, BUD_BID_PROJECTS.DAYS FROM BUD_BID_PROJECTS
                        INNER JOIN BUD_BID_STATUS
                        ON BUD_BID_PROJECTS.STATUS_ID = BUD_BID_STATUS.STATUS_ID
                        WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {2} AND BUD_BID_PROJECTS.VER_ID = {3}
                    ),     
                    ORACLE_PROJECT_NAMES AS (
                        SELECT '{1}' AS PROJECT_ID, '{0} (Org)' AS PROJECT_NAME, 'ORG' AS TYPE
                        FROM DUAL
                            UNION ALL
                        SELECT CAST(PROJECTS_V.PROJECT_ID AS varchar(20)) AS PROJECT_ID, PROJECTS_V.LONG_NAME AS PROJECT_NAME, 'PROJECT' AS TYPE
                        FROM PROJECTS_V
                        LEFT JOIN PA.PA_PROJECT_CLASSES
                        ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                        WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                            UNION ALL
                        SELECT CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_ID, CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_NAME, 'ROLLUP' AS TYPE
                        FROM PROJECTS_V
                        LEFT JOIN PA.PA_PROJECT_CLASSES
                        ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                        WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup'
                        AND PA.PA_PROJECT_CLASSES.CLASS_CODE <> 'None' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                        GROUP BY CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) 
                    ),
                    BUDGET_LINE_AMOUNTS AS (
                        SELECT * FROM (SELECT PROJECT_ID, LINE_ID, SUM(NOV) NOV FROM BUD_BID_BUDGET_NUM GROUP BY PROJECT_ID, LINE_ID) PIVOT (SUM(NOV) FOR LINE_ID IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                    ),
                    PREV_OP AS (
                        SELECT BUD_BID_PROJECTS.PROJECT_ID, NOV PREV_OP FROM BUD_BID_PROJECTS    
                        LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_BUDGET_NUM.PROJECT_ID
                        WHERE BUD_BID_BUDGET_NUM.LINE_ID = 10 AND BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {4} AND BUD_BID_PROJECTS.VER_ID = {5}
                    )  
                    SELECT CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID,
                        CUR_PROJECT_INFO_WITH_STATUS.BUD_BID_PROJECTS_ID,
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
                        CASE WHEN PREV_OP.PREV_OP IS NULL THEN 0 ELSE PREV_OP.PREV_OP END PREV_OP,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REC = 0 THEN 0 ELSE ROUND (BUDGET_LINE_AMOUNTS.MAT_USAGE/BUDGET_LINE_AMOUNTS.GROSS_REC,2)*100 END MAT_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND (BUDGET_LINE_AMOUNTS.GROSS_REC/BUDGET_LINE_AMOUNTS.GROSS_REV,2)*100 END GR_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND (BUDGET_LINE_AMOUNTS.DIR_EXP/BUDGET_LINE_AMOUNTS.GROSS_REV,2)*100 END DIRECTS_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND (BUDGET_LINE_AMOUNTS.OP/BUDGET_LINE_AMOUNTS.GROSS_REV,2)*100 END OP_PERC,       
                        CASE WHEN PREV_OP.PREV_OP IS NULL THEN BUDGET_LINE_AMOUNTS.OP ELSE (BUDGET_LINE_AMOUNTS.OP - PREV_OP.PREV_OP) END OP_VAR      
                    FROM CUR_PROJECT_INFO_WITH_STATUS
                    LEFT OUTER JOIN ORACLE_PROJECT_NAMES ON CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID = ORACLE_PROJECT_NAMES.PROJECT_ID AND CUR_PROJECT_INFO_WITH_STATUS.TYPE = ORACLE_PROJECT_NAMES.TYPE
                    LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON CUR_PROJECT_INFO_WITH_STATUS.BUD_BID_PROJECTS_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID
                    LEFT OUTER JOIN PREV_OP ON CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID = PREV_OP.PROJECT_ID
                    ORDER BY LOWER(PROJECT_NAME)", orgName, orgID, yearID, verID, prevYearID, prevVerID);

                List<BUD_BID.BUD_SUMMARY_V> data = context.Database.SqlQuery<BUD_BID.BUD_SUMMARY_V>(sql).ToList();
                return data;
            }
        }
        
        /// <summary>
        /// Returns project detail information 
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static BUD_BID.BUD_SUMMARY_V SummaryProjectsDetail(long projectID)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format(@"
                SELECT BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.TYPE, BUD_BID_PROJECTS.PRJ_NAME PROJECT_NAME, 
                    BUD_BID_STATUS.STATUS, BUD_BID_PROJECTS.ACRES, BUD_BID_PROJECTS.DAYS, BUD_BID_PROJECTS.APP_TYPE, BUD_BID_PROJECTS.CHEMICAL_MIX, BUD_BID_PROJECTS.COMMENTS,
                    BUD_BID_PROJECTS.LIABILITY, BUD_BID_PROJECTS.LIABILITY_OP, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT
                FROM BUD_BID_PROJECTS
                INNER JOIN BUD_BID_STATUS
                ON BUD_BID_PROJECTS.STATUS_ID = BUD_BID_STATUS.STATUS_ID
                WHERE BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = {0}", projectID);

                return context.Database.SqlQuery<BUD_BID.BUD_SUMMARY_V>(sql).SingleOrDefault();
             }
        }

        /// <summary>
        /// Returns true if a project exists for a given org, year and version
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="yearID"></param>
        /// <param name="verID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static bool ProjectExists(long orgID, long yearID, long verID, long projectID)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format(@"
                SELECT BUD_BID_PROJECTS.PROJECT_ID
                FROM BUD_BID_PROJECTS
                WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID  = {1} AND
                    BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.PROJECT_ID = {3}", orgID, yearID, verID, projectID);

                long recCount = context.Database.SqlQuery<BUD_BID.BUD_SUMMARY_V>(sql).Count();

                if (recCount == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public class BUD_SUMMARY_V
        {
            public long BUD_BID_PROJECTS_ID { get; set; }
            public string PROJECT_ID { get; set; }
            public string TYPE { get; set; }
            public string PROJECT_NUM { get; set; }
            public string PROJECT_NAME { get; set; }
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
            public string ORDERKEY { get; set; }
        }
    }
}
