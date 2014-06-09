﻿using System;
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
            //if (validateComponentSecurity("SYS.TimeClock.Payroll"))
            //{
            //    deGetEnployeesHourData();
            //}
            //else
            //{
            //    X.Redirect("~/Views/uxDefault.aspx");
            //}
        }

        protected void deGetEnployeesHourData(object sender, StoreReadDataEventArgs e)
        {
            var data = TIME_CLOCK.EmployeeTimeCompletedUnapprovedPayroll();

            foreach (var item in data)
            {
                TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                DateTime dow = (DateTime)item.TIME_IN;


                TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("hh\\:mm");


                TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                item.ACTUAL_HOURS_GRID = actualhours.ToString("hh\\:mm");


            }
            uxPayrollAuditStore.DataSource = data;
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