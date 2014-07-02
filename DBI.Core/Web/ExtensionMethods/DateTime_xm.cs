using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public static DateTime GetFirstDayOfWeek(this DateTime sourceDateTime)
        {
            var daysAhead = (DayOfWeek.Sunday - (int)sourceDateTime.DayOfWeek);

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        public static DateTime GetLastDayOfWeek(this DateTime sourceDateTime)
        {
            var daysAhead = DayOfWeek.Saturday - (int)sourceDateTime.DayOfWeek;

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

    }

    
}
