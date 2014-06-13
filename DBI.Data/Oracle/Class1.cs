using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.Oracle
{
    public class Class1
    {
        public static void test()
        {
            try
            {
                BUD_BID_BUDGET_NUM startNumsdata = new BUD_BID_BUDGET_NUM();
                startNumsdata.PROJECT_ID = 1;
                startNumsdata.DETAIL_TASK_ID = 2;
                startNumsdata.LINE_ID = 3;
                startNumsdata.NOV = 4;
                GenericData.Insert<BUD_BID_BUDGET_NUM>(startNumsdata);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
