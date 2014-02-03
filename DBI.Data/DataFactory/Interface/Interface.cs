using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class Interface
    {
        public static int nextHeaderID()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_DAILYACT_HEADERID_S.NEXTVAL from dual";
                decimal query = _context.Database.SqlQuery<decimal>(sql).First();
                return int.Parse(query.ToString());
            }
        }

        //public int GetNextLaborHeaderID()
        //{
        //    return ExecuteFunc(() =>
        //    {
        //        string sql = @"select XXDBI.XXDBI_LABOR_HEADERID_S.NEXTVAL from dual";
        //        decimal query = ObjectContext.ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
        //        return int.Parse(query.ToString());
        //    });
        //}

        //public int GetNextTransId()
        //{
        //    return ExecuteFunc(() =>
        //    {
        //        string sql = @"select XXDBI.XXDBI_MTLTRANSID_S.NEXTVAL from dual";
        //        decimal query = ObjectContext.ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
        //        return int.Parse(query.ToString());
        //    });
        //}


        //public int GetNextPaTransID()
        //{
        //    int? transid = ObjectContext.PA_TRANSACTION_INTERFACE_CP.Max(u => (int?)u.TRANS_ID);
        //    if (transid == null)
        //    {
        //        transid = 0;
        //    }
        //    return int.Parse(transid.ToString());
        //}



    }
}




