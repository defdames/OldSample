using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.Oracle.XXDBI_DW
{
   public class JOB_COST
    {
        public static JOB_COST_V jobCostByProjectIdWeekendingDate(long projectId, string jcWkDate)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP from XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 8 AND PROJECT_ID = {0} AND JC_WK_DATE = '{1}'", projectId, jcWkDate);
                JOB_COST_V returnRecord = context.Database.SqlQuery<JOB_COST_V>(sql).SingleOrDefault();
                
                // Return 0s if record doesn't exist
                if (returnRecord == null)
                {
                    JOB_COST_V returnZeroRecord = new JOB_COST_V();
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
                    return returnRecord;       
                }    
            }
        }

        public static JOB_COST_V jobCostByOrgIdWeekendingDate(long hierID, long orgID, string jcWkDate)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP from XXDBI_DW.JOB_COST WHERE HIERARCHY_ID = {0} AND DIVISION_ID = {1} AND (CLASS_CATEGORY = 'Job Cost Rollup' OR CLASS_CATEGORY is NULL) AND JC_WK_DATE = '{2}'", hierID, orgID, jcWkDate);
                JOB_COST_V returnRecord = context.Database.SqlQuery<JOB_COST_V>(sql).SingleOrDefault();

                // Return 0s if record doesn't exist
                if (returnRecord == null)
                {
                    JOB_COST_V returnZeroRecord = new JOB_COST_V();
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
                    return returnRecord;
                }
            }
        }

        public static JOB_COST_V jobCostByRollupWeekendingDate(long orgID, string projectName, string jcWkDate)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format("SELECT FY_GREC, FY_MU, FY_GREV, FY_TDE, FY_TOP from XXDBI_DW.JOB_COST WHERE LEVEL_SORT = 10 AND DIVISION_ID = {0} AND PROJECT_LONG_NAME = '{1}' AND CLASS_CATEGORY = 'Job Cost Rollup' AND JC_WK_DATE = '{2}' ", orgID, projectName, jcWkDate);
                JOB_COST_V returnRecord = context.Database.SqlQuery<JOB_COST_V>(sql).SingleOrDefault();

                // Return 0s if record doesn't exist
                if (returnRecord == null)
                {
                    JOB_COST_V returnZeroRecord = new JOB_COST_V();
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
                    return returnRecord;
                }
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

        public static string getWeekEndingDateByFYAndDate(string queryDate)
        {
            using (Entities context = new Entities())
            {
                string sql = string.Format("SELECT TO_CHAR(END_DATE,'DD-MON-YYYY') FROM APPS.PA_PERIODS_ALL WHERE (TO_DATE('{0}', 'DD-MON-YYYY') BETWEEN START_DATE AND END_DATE) GROUP BY END_DATE", queryDate);
                string weekEnding = context.Database.SqlQuery<string>(sql).SingleOrDefault();
                return weekEnding;   
            }
        }



    }
}
