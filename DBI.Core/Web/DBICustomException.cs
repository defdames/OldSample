using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Core.Web
{
    public class DBICustomException: Exception
    {
        public DBICustomException()
        {
        }

        public DBICustomException(string message)
            : base(message)
        {
        }

        public DBICustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
