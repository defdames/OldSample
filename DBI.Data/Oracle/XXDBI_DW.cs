﻿using System;
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
        /// Returns job cost amounts for a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="weekEndingDate"></param>
        /// <returns></returns>
        public static JOB_COST_V JCSummaryLineAmounts(long projectId, string weekEndingDate)
        {
            string sql = string.Format("SELECT FY_RI, FY_RNB, FY_RNI, FY_GREC, FY_MU, FY_GREV, FY_DP, FY_LB, FY_TU, FY_EU, FY_SC, FY_ESC, FY_T, FY_FE, FY_PD, FY_PB, FY_RRPL, FY_ME, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 8 AND PROJECT_ID = {0} AND JC_WK_DATE = TO_DATE('{1}', 'DD-Mon-YYYY')", projectId, weekEndingDate);
            return GetJCNumbers(sql);            
        }

        /// <summary>
        /// Returns job cost amounts for an org
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <param name="weekEndingDate"></param>
        /// <returns></returns>
        public static JOB_COST_V JCSummaryLineAmounts(long hierarchyId, long organizationId, string weekEndingDate)
        {
            string sql = string.Format("SELECT FY_RI, FY_RNB, FY_RNI, FY_GREC, FY_MU, FY_GREV, FY_DP, FY_LB, FY_TU, FY_EU, FY_SC, FY_ESC, FY_T, FY_FE, FY_PD, FY_PB, FY_RRPL, FY_ME, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE HIERARCHY_ID = {0} AND DIVISION_ID = {1} AND (CLASS_CATEGORY = 'Job Cost Rollup' OR CLASS_CATEGORY IS NULL) AND JC_WK_DATE = TO_DATE('{2}', 'DD-Mon-YYYY')", hierarchyId, organizationId, weekEndingDate);
            return GetJCNumbers(sql);
        }

        /// <summary>
        /// Returns job cost amounts for a job cost rollup
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="rollupName"></param>
        /// <param name="weekEndingDate"></param>
        /// <returns></returns>
        public static JOB_COST_V JCSummaryLineAmounts(long organizationId, string rollupName, string weekEndingDate)
        {
            string sql = string.Format("SELECT FY_RI, FY_RNB, FY_RNI, FY_GREC, FY_MU, FY_GREV, FY_DP, FY_LB, FY_TU, FY_EU, FY_SC, FY_ESC, FY_T, FY_FE, FY_PD, FY_PB, FY_RRPL, FY_ME, FY_TDE, FY_TOP FROM XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 10 AND DIVISION_ID = {0} AND PROJECT_LONG_NAME = '{1}' AND CLASS_CATEGORY = 'Job Cost Rollup' AND JC_WK_DATE = TO_DATE('{2}', 'DD-Mon-YYYY')", organizationId, rollupName, weekEndingDate);
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
                nullData.FY_RI = 0;
                nullData.FY_RNB = 0;
                nullData.FY_RNI = 0;
                nullData.FY_GREC = 0;
                nullData.FY_MU = 0;
                nullData.FY_GREV = 0;
                nullData.FY_DP = 0;
                nullData.FY_LB = 0;
                nullData.FY_TU = 0;
                nullData.FY_EU = 0;
                nullData.FY_SC = 0;
                nullData.FY_ESC = 0;
                nullData.FY_T = 0;
                nullData.FY_FE = 0;
                nullData.FY_PD = 0;
                nullData.FY_PB = 0;
                nullData.FY_RRPL = 0;
                nullData.FY_ME = 0;
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
                .Select(x => new Threshold { PROJECT_NAME = x.job_cost.job_cost.PROJECT_NAME, PROJECT_NUMBER = x.job_cost.job_cost.PROJECT_NUMBER, TYPE_ID = x.job_cost.threshold.TYPE_ID, TYPE_NAME = x.job_cost.threshold.SURVEY_TYPES.TYPE_NAME  ,PERCENTAGE = (x.job_cost.job_cost.BGT_GREC == 0 ? 0 : Math.Round((double)(x.job_cost.job_cost.FY_GREC / x.job_cost.job_cost.BGT_GREC * 100))), THRESHOLD = (double)x.threshold.THRESHOLD, THRESHOLD_ID = x.threshold.THRESHOLD_ID, ORG_ID = (long)x.job_cost.job_cost.DIVISION_ID, PROJECT_ID = (long)x.job_cost.job_cost.PROJECT_ID })
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
        public static List<SingleCombo> LoadedJCWeDates(long hierarchyId, long fiscalYear, bool optionalsortDescending = false, long optionalNumOfReturnRecords = long.MaxValue)
        {
            string sortOrder = optionalsortDescending == false ? sortOrder = "ASC" : sortOrder = "DESC";
            string sql = string.Format(@"
                SELECT '-- OVERRIDE --' AS ID_NAME FROM DUAL

                    UNION ALL
    
                SELECT TO_CHAR(JC_WK_DATE,'DD-Mon-YYYY') ID_NAME
                FROM 
                    (SELECT DISTINCT JC_WK_DATE 
                     FROM APPS.XX_JOBCOST_DATES_MV
                     WHERE HIERARCHY_ID = {0} 
                        AND JC_WK_DATE >= (SELECT START_DATE FROM APPS.GL_PERIODS_V WHERE PERIOD_YEAR = {1} AND period_num = 1 AND PERIOD_SET_NAME = 'DBI Calendar' AND period_type = 'Month')
                        AND JC_WK_DATE <= (SELECT END_DATE FROM APPS.GL_PERIODS_V WHERE PERIOD_YEAR = {1} AND period_num = 12 AND PERIOD_SET_NAME = 'DBI Calendar' AND period_type = 'Month') 
                     ORDER BY JC_WK_DATE {2}) JC_DATES
                WHERE ROWNUM <= {3}", hierarchyId, fiscalYear, sortOrder, optionalNumOfReturnRecords);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<SingleCombo>(sql).ToList();                           
            }
        }              

        public class JOB_COST_V
        {
            public decimal FY_RI { get; set; }
            public decimal FY_RNB { get; set; }
            public decimal FY_RNI { get; set; }
            public decimal FY_GREC { get; set; }
            public decimal FY_MU { get; set; }
            public decimal FY_GREV { get; set; }
            public decimal FY_DP { get; set; }
            public decimal FY_LB { get; set; }
            public decimal FY_TU { get; set; }
            public decimal FY_EU { get; set; }
            public decimal FY_SC { get; set; }
            public decimal FY_ESC { get; set; }
            public decimal FY_T { get; set; }
            public decimal FY_FE { get; set; }
            public decimal FY_PD { get; set; }
            public decimal FY_PB { get; set; }
            public decimal FY_RRPL { get; set; }
            public decimal FY_ME { get; set; }
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
            public string TYPE_NAME { get; set; }
            public decimal TYPE_ID { get; set; }
            public long PROJECT_ID { get; set; }
            public long ORG_ID { get; set; }
        }
    }
}
