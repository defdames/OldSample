using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.Oracle
{
    public class PA
    {

        /// <summary>
        /// Gets the labor burden for a fiscal year and organization id
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="fiscalYear"></param>
        /// <returns></returns>
        public static decimal laborBurden(string buid, string fiscalYear)
        {
            try
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
            catch (Exception)
            {
                throw;
            }

        }


    }
}
