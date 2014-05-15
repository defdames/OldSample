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

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class WebForm3 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (validateComponentSecurity("SYS.TimeClock.Payroll"))
            {
                GetEnployeesHourData();
            }
            else
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
        }

        protected void GetEnployeesHourData()
        {
            using (Entities _context = new Entities())
            {
                var data = (from tc in _context.TIME_CLOCK
                            join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                            where tc.APPROVED=="Y"
                            select new EmployeeTimePayroll { TIME_IN = (DateTime)tc.TIME_IN, TIME_OUT = (DateTime)tc.TIME_OUT, EMPLOYEE_NAME = ev.EMPLOYEE_NAME, DAY_OF_WEEK = tc.DAY_OF_WEEK, TIME_CLOCK_ID = tc.TIME_CLOCK_ID, ADJUSTED_HOURS = tc.ADJUSTED_HOURS, ACTUAL_HOURS = tc.ACTUAL_HOURS }).ToList();
            }
        }
    }

    public class EmployeeTimePayroll
    {
        public decimal TIME_CLOCK_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public DateTime TIME_IN { get; set; }
        public DateTime TIME_OUT { get; set; }
        public string ADJUSTED_HOURS_GRID { get; set; }
        public string DAY_OF_WEEK { get; set; }
        public string ACTUAL_HOURS_GRID { get; set; }
        public decimal? ACTUAL_HOURS { get; set; }
        public decimal? ADJUSTED_HOURS { get; set; }

    }
}