using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using System.Security.Claims;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;
using DBI.Data.DataFactory;
using System.Globalization;


namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class umAdjustedverseActualHoursReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deGetEmployeeHoursData(object sender, StoreReadDataEventArgs e)
        {

            var data = TIMECLOCK.EmployeeTimeCompletedPayroll();

            foreach (var item in data)
            {
                TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                DateTime dow = (DateTime)item.TIME_IN;

                TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");

                TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                TimeSpan timeDiff =adjustedhours - actualhours;
                string strTimeDiff = ForamtTimeSpan(timeDiff);
                item.TIME_DIFF = strTimeDiff;




            }

            uxHoursStore.DataSource = data;
            List<object> _data = data.ToList<object>();
            int count = 0;
            uxHoursStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
            e.Total = count;


        }

        private string ForamtTimeSpan(TimeSpan time)
        {
            return ((time < TimeSpan.Zero) ? "-" : "") + time.ToString("dd\\.hh\\:mm");
        }
    }
}