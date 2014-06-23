using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;

namespace DBI.Data
{
    public class XXDBI_DW
    {
        /// <summary>
        /// Returns the following job cost amounts for a project:  Gross Receipts, Mat Usage, Gross Rev, Total Directs and OP
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="weekEndingDate"></param>
        /// <returns></returns>
        public static XXDBI_DW.JOB_COST_V JcSummaryLineAmounts(long projectId, string weekEndingDate)
        {            
            string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 8 AND PROJECT_ID = {0} AND JC_WK_DATE = TO_DATE('{1}', 'DD-Mon-YYYY')", projectId, weekEndingDate);
            return GetJCNumbers(sql);            
        }

        /// <summary>
        /// Returns the following job cost amounts for an org:  Gross Receipts, Mat Usage, Gross Rev, Total Directs and OP
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <param name="weekEndingDate"></param>
        /// <returns></returns>
        public static XXDBI_DW.JOB_COST_V JcSummaryLineAmounts(long hierarchyId, long organizationId, string weekEndingDate)
        {
            string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE HIERARCHY_ID = {0} AND DIVISION_ID = {1} AND (CLASS_CATEGORY = 'Job Cost Rollup' OR CLASS_CATEGORY IS NULL) AND JC_WK_DATE = TO_DATE('{2}', 'DD-Mon-YYYY')", hierarchyId, organizationId, weekEndingDate);
            return GetJCNumbers(sql);
        }

        /// <summary>
        /// Returns the following job cost amounts for a job cost rollup:  Gross Receipts, Mat Usage, Gross Rev, Total Directs and OP
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="rollupName"></param>
        /// <param name="weekEndingDate"></param>
        /// <returns></returns>
        public static XXDBI_DW.JOB_COST_V JcSummaryLineAmounts(long organizationId, string rollupName, string weekEndingDate)
        {
            string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 10 AND DIVISION_ID = {0} AND PROJECT_LONG_NAME = '{1}' AND CLASS_CATEGORY = 'Job Cost Rollup' AND JC_WK_DATE = TO_DATE('{2}', 'DD-Mon-YYYY')", organizationId, rollupName, weekEndingDate);
            return GetJCNumbers(sql);
        }

        protected static XXDBI_DW.JOB_COST_V GetJCNumbers(string sql)
        {
            using (Entities context = new Entities())
            {
                XXDBI_DW.JOB_COST_V data = context.Database.SqlQuery<XXDBI_DW.JOB_COST_V>(sql).SingleOrDefault();

                // Return 0s if record doesn't exist
                if (data == null)
                {
                    XXDBI_DW.JOB_COST_V returnZeroRecord = new XXDBI_DW.JOB_COST_V();
                    returnZeroRecord.FY_GREC = 0;
                    returnZeroRecord.FY_MU = 0;
                    returnZeroRecord.FY_GREV = 0;
                    returnZeroRecord.FY_TDE = 0;
                    returnZeroRecord.FY_TOP = 0;
                    return returnZeroRecord;
                }

                // Return record
                else
                {
                    return data;
                }
            }            
        }

        /// <summary>
        /// Returns a list of loaded job cost week ending dates in EMS.
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <param name="optionalNumOfReturnRecords"></param>
        /// <returns></returns>
        public static List<SingleCombo> LoadedJcWeDates(long hierarchyId, bool optionalsortDescending = false, long optionalNumOfReturnRecords = long.MaxValue)
        {
            using (Entities context = new Entities())
            {
                string sortOrder;

                if (optionalsortDescending == false)
                {
                    sortOrder = "ASC";
                }

                else
                {
                    sortOrder = "DESC";
                }

                string sql = string.Format(@"SELECT TO_CHAR(JC_WK_DATE,'DD-Mon-YYYY') ID_NAME
                    FROM (SELECT DISTINCT JC_WK_DATE FROM APPS.XX_JOBCOST_DATES_MV WHERE HIERARCHY_ID = {0} ORDER BY JC_WK_DATE {1}) JC_DATES
                    WHERE ROWNUM <= {2}
                    ORDER BY ROWNUM", hierarchyId, sortOrder, optionalNumOfReturnRecords);
                List<SingleCombo> data = context.Database.SqlQuery<SingleCombo>(sql).ToList();
                return data;
            }
        }              

        public class JOB_COST_V
        {
            public decimal FY_GREC { get; set; }
            public decimal FY_MU { get; set; }
            public decimal FY_GREV { get; set; }
            public decimal FY_TDE { get; set; }
            public decimal FY_TOP { get; set; }
        }
    }
}
