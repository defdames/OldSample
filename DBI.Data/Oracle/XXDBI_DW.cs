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
        public static JOB_COST_V JCSummaryLineAmounts(long projectId, string weekEndingDate)
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
        public static JOB_COST_V JCSummaryLineAmounts(long hierarchyId, long organizationId, string weekEndingDate)
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
        public static JOB_COST_V JCSummaryLineAmounts(long organizationId, string rollupName, string weekEndingDate)
        {
            string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 10 AND DIVISION_ID = {0} AND PROJECT_LONG_NAME = '{1}' AND CLASS_CATEGORY = 'Job Cost Rollup' AND JC_WK_DATE = TO_DATE('{2}', 'DD-Mon-YYYY')", organizationId, rollupName, weekEndingDate);
            return GetJCNumbers(sql);
        }

        protected static JOB_COST_V GetJCNumbers(string sql)
        {
            JOB_COST_V data;
            using (Entities context = new Entities())
            {
                data = context.Database.SqlQuery<JOB_COST_V>(sql).SingleOrDefault();                
            }
            
            if (data == null)
            {
                JOB_COST_V nullData = new JOB_COST_V();
                nullData.FY_GREC = 0;
                nullData.FY_MU = 0;
                nullData.FY_GREV = 0;
                nullData.FY_TDE = 0;
                nullData.FY_TOP = 0;
                data = nullData;
            }

                return data;   
        }

        public static IQueryable<Threshold> JobCostbyProjectList(List<long> OrgsList, long HierarchyId, Entities _context)
        {
            string sql = string.Format(@"
                SELECT JC_WK_DATE FROM(SELECT JC_WK_DATE FROM APPS.XX_JOBCOST_DATES_MV WHERE HIERARCHY_ID = {0} ORDER BY JC_WK_DATE DESC) WHERE ROWNUM = 1", HierarchyId);
            DateTime JCDate = _context.Database.SqlQuery<DateTime>(sql).Single();
            var data = _context.JOB_COST
                .Join(_context.CUSTOMER_SURVEY_THRESH_AMT, jc => jc.DIVISION_ID, tham => tham.ORG_ID, (jc, tham) => new { job_cost = jc, threshold = tham })
                .Where(x => x.job_cost.JC_WK_DATE == JCDate)
                .Where(x => OrgsList.Contains((long)x.job_cost.DIVISION_ID))
                .Where(x => x.job_cost.LEVEL_SORT == 8)
                .Where(x => x.job_cost.BGT_GREC >= x.threshold.LOW_DOLLAR_AMT && x.job_cost.BGT_GREC <= x.threshold.HIGH_DOLLAR_AMT)
                .Join(_context.CUSTOMER_SURVEY_THRESHOLDS, tham => tham.threshold.AMOUNT_ID, th => th.AMOUNT_ID, (tham, th) => new { job_cost = tham, threshold = th })
                .Select(x => new Threshold { PROJECT_NAME = x.job_cost.job_cost.PROJECT_NAME, PROJECT_NUMBER = x.job_cost.job_cost.PROJECT_NUMBER, PERCENTAGE = (x.job_cost.job_cost.BGT_GREC == 0 ? 0 : Math.Round((double)(x.job_cost.job_cost.FY_GREC / x.job_cost.job_cost.BGT_GREC * 100))), THRESHOLD = (double)x.threshold.THRESHOLD, THRESHOLD_ID = x.threshold.THRESHOLD_ID, ORG_ID = (long)x.job_cost.job_cost.DIVISION_ID, PROJECT_ID = (long)x.job_cost.job_cost.PROJECT_ID })
                .Where(x => x.PERCENTAGE > (x.THRESHOLD - 5));

            var filtereddata = (from d in data
                                where !_context.SURVEY_FORMS_COMP.Any(x => x.THRESHOLD_ID == d.THRESHOLD_ID && x.PROJECT_ID == d.PROJECT_ID)
                                select d);
                                
            return filtereddata;
            
                
        }

        /// <summary>
        /// Returns a list of loaded job cost week ending dates in EMS
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="optionalsortDescending"></param>
        /// <param name="optionalNumOfReturnRecords"></param>
        /// <returns></returns>
        public static List<SingleCombo> LoadedJCWeDates(long hierarchyId, bool optionalsortDescending = false, long optionalNumOfReturnRecords = long.MaxValue)
        {
            string sortOrder = optionalsortDescending == false ? sortOrder = "ASC" : sortOrder = "DESC";
            string sql = string.Format(@"
                SELECT '-- OVERRIDE --' AS ID_NAME FROM DUAL
                UNION ALL
                SELECT TO_CHAR(JC_WK_DATE,'DD-Mon-YYYY') ID_NAME
                FROM (SELECT DISTINCT JC_WK_DATE FROM APPS.XX_JOBCOST_DATES_MV WHERE HIERARCHY_ID = {0} ORDER BY JC_WK_DATE {1}) JC_DATES
                WHERE ROWNUM <= {2}", hierarchyId, sortOrder, optionalNumOfReturnRecords);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<SingleCombo>(sql).ToList();                           
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

        public class Threshold
        {
            public string PROJECT_NUMBER { get; set; }
            public double PERCENTAGE { get; set; }
            public string PROJECT_NAME { get; set; }
            public double THRESHOLD { get; set; }
            public decimal THRESHOLD_ID { get; set; }
            public long PROJECT_ID { get; set; }
            public long ORG_ID { get; set; }
        }
    }
}
