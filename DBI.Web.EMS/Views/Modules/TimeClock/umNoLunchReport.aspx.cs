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
using System.Data.Entity;
using System.Data.Objects;

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class umNoLunchReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deGetEmployeeHoursData(object sender, StoreReadDataEventArgs e)
        {

            var data = NoLunch(1);

            foreach (var item in data)
            {
                TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                DateTime dow = (DateTime)item.TIME_IN;

                //TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                //item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("hh\\:mm");

                //TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                //item.ACTUAL_HOURS_GRID = actualhours.ToString("hh\\:mm");

            }

            uxHoursStore.DataSource = data;
        }


        public static List<EmployeeTime> NoLunch(int Hours)
        {

            using (Entities _context = new Entities())
            {
                var TotalHoursList = (from d in _context.TIME_CLOCK
                                      where d.COMPLETED == "Y"
                                      group d by new { TIME_IN = EntityFunctions.TruncateTime(d.TIME_IN), d.PERSON_ID } into g
                                      select new { g.Key.PERSON_ID, g.Key.TIME_IN, TotalCount = g.Count() }).ToList();

                List<EmployeeTime> OffendingProjects = new List<EmployeeTime>();
                foreach (var TotalHour in TotalHoursList)
                {

                    if (TotalHour.TotalCount == Hours)
                    {
                        var EmployeeOverTwelveHoursinDay = (from d in _context.TIME_CLOCK
                                                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                                            where d.PERSON_ID == TotalHour.PERSON_ID && EntityFunctions.TruncateTime(d.TIME_IN) == TotalHour.TIME_IN
                                                            select new EmployeeTime { EMPLOYEE_NAME = e.EMPLOYEE_NAME, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT }).ToList();
                        foreach (var Project in EmployeeOverTwelveHoursinDay)
                        {
                            OffendingProjects.Add(new EmployeeTime
                            {
                                EMPLOYEE_NAME = Project.EMPLOYEE_NAME,
                                TIME_IN = Project.TIME_IN,
                                TIME_OUT = Project.TIME_OUT
                               

                            });
                        }
                    }
                }
                return OffendingProjects;

            }
        }


    }
}