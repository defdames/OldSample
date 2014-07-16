using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Security;
using System.Security.Claims;

namespace DBI.Web.EMS.Views.Modules.TimeClock.Edit
{
    public partial class umEditTime : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!X.IsAjaxRequest)
            {

                string _TimeClockId = Request.QueryString["tcID"].ToString();
                DateTime dataDateIn = TIMECLOCK.ManagerDateInEditScreen(decimal.Parse(_TimeClockId));
                TimeSpan dataTimeIn = TIMECLOCK.ManagerTimeInEditScreen(decimal.Parse(_TimeClockId));

                DateTime dataDateOut = TIMECLOCK.ManagerDateOutEditScreen(decimal.Parse(_TimeClockId));
                TimeSpan dataTimeOut = TIMECLOCK.ManagerTimeOutEditScreen(decimal.Parse(_TimeClockId));




                uxDateInField.SelectedDate = dataDateIn;
                uxTimeInField.SelectedTime = dataTimeIn;



                uxDateOutField.SelectedDate = dataDateOut;
                uxTimeOutField.SelectedTime = dataTimeOut;
            }
        }

         protected void deEditTime(object sender, DirectEventArgs e)
        {

            string person_name = User.Identity.Name;//Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);
             string _TimeClockId = Request.QueryString["tcID"].ToString();

             DateTime updateTimeIn = uxDateInField.SelectedDate + uxTimeInField.SelectedTime;
             DateTime updateTimeOut = uxDateOutField.SelectedDate + uxTimeOutField.SelectedTime;

             TIMECLOCK.InsertEditedEmployeeTime(decimal.Parse(_TimeClockId), updateTimeIn, updateTimeOut, person_name);
            
        }
    }
}