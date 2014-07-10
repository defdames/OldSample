using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Core
{
    public static class String_xm
    {
        public static long ToLong(this string str)
        {
            // you can throw an exception or return a default value here
            if (string.IsNullOrEmpty(str))
                return 0;

            long d;

            // you could throw an exception or return a default value on failure
            if (!long.TryParse(str, out d))
                return 0;

            return d;
        }
    }
}
