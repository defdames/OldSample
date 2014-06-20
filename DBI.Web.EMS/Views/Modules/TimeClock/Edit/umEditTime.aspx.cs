using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.TimeClock.Edit
{
    public partial class umEditTime : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _TimeClockId = Request.QueryString["tcID"].ToString();
            DateTime dataTimeIn = TIME_CLOCK.ManagerTimeInEditScreen(decimal.Parse(_TimeClockId));
           TimeSpan dataTimeOut = TIME_CLOCK.ManagerTimeOutEditScreen(decimal.Parse(_TimeClockId));




            uxDateTimeInField.SelectedDate = dataTimeIn;
            uxDateTimeOutField.SelectedTime = dataTimeOut;
        }
    }
}