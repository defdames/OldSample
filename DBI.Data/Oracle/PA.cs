using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;

namespace DBI.Data
{
    public class PA
    {
        /// <summary>
        /// Gets the labor burden for a fiscal year and organization id
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="fiscalYear"></param>
        /// <returns></returns>
        public static decimal LaborBurden(string buid, string fiscalYear)
        {
            using (Entities _context = new Entities())
            {
                string sql = @" SELECT (Icm.Multiplier / 100) as labor_burden_rate
                                FROM Pa.Pa_Ind_Rate_Schedules_All_Bg Irsab
                                INNER JOIN Pa.Pa_Ind_Rate_Sch_Revisions Irsr
                                    ON Irsab.Ind_Rate_Sch_Id = Irsr.Ind_Rate_Sch_Id
                                INNER JOIN Pa.Pa_Ind_Cost_Multipliers Icm
                                    ON Irsr.Ind_Rate_Sch_Revision_Id =
                                    Icm.Ind_Rate_Sch_Revision_Id
                                WHERE     ICM.ORGANIZATION_ID = '" + buid + @"'
                                AND Icm.Ind_Cost_Code = 'Labor Burden'
                                AND irsr.ind_rate_sch_revision = '" + fiscalYear + "'";

                decimal _returnValue = _context.Database.SqlQuery<decimal>(sql).FirstOrDefault();

                if (_returnValue == 0)
                {
                    throw new DBICustomException("Labor burden does not exist for this organization and fiscal year.");
                }

                return _returnValue;
            }
        }

        /// <summary>
        /// Returns a list of week ending dates from Oracle
        /// </summary>
        /// <returns></returns>
        public static List<SingleCombo> AllFiscalYears()
        {
            string sql = "SELECT DISTINCT TO_CHAR(END_DATE, 'YYYY') ID_NAME FROM APPS.PA_PERIODS_ALL ORDER BY ID_NAME";

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<SingleCombo>(sql).ToList();                
            }
        }

        /// <summary>
        /// Returns a list of fiscal years by GL
        /// </summary>
        /// <returns></returns>
        public static List<SingleCombo> FiscalYearsGL()
        {
            string sql = "SELECT DISTINCT TO_CHAR(END_DATE, 'YYYY') ID_NAME FROM APPS.GL_PERIODS_V ORDER BY ID_NAME DESC";

            List<SingleCombo> data;
            using (Entities context = new Entities())
            {
                data = context.Database.SqlQuery<SingleCombo>(sql).ToList();
            }
            return data;
        }        

        public static IQueryable<PROJECTS_V> GetProjectsByOrg(List<long> OrgsList, Entities _context)
        {
            return _context.PROJECTS_V.Where(x => OrgsList.Contains(x.CARRYING_OUT_ORGANIZATION_ID) && x.PROJECT_TYPE == "CUSTOMER BILLING");
        }

    }
}
