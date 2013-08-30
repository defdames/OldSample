using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Core
{
    public static class DateTime_xm 
    {
        public static DateTime InvariantCulture(this DateTime dt)
        {
            // Creates a CultureInfo set to InvariantCulture.
            CultureInfo InvC = new CultureInfo("");
            return DateTime.Parse(dt.ToString("d", InvC));
        }

    }
}
