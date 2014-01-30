using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class Interface
    {
        public static XXDBI_DAILY_ACTIVITY_HEADER saveHeader(XXDBI_DAILY_ACTIVITY_HEADER header)
        {
            GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header);
            return header;
        }
    }
}




