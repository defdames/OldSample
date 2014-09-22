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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deAddTime(object sender, DirectEventArgs e)
        {
            string person_name = User.Identity.Name;//Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);

            DateTime updateTimeIn = uxDateInField.SelectedDate + uxTimeInField.SelectedTime;
            DateTime updateTimeOut = uxDateOutField.SelectedDate + uxTimeOutField.SelectedTime;
            TIMECLOCK.InsertAddedEmployeeTime(updateTimeIn, updateTimeOut, person_name);

        }

        protected void ValidateDateTime(object sender, RemoteValidationEventArgs e)
        {
            DateTime StartDate = DateTime.Parse(uxDateInField.Value.ToString());
            DateTime StartTime = DateTime.Parse(uxTimeInField.Value.ToString());
            DateTime EndDate = DateTime.Parse(uxDateOutField.Value.ToString());
            DateTime EndTime = DateTime.Parse(uxTimeOutField.Value.ToString());

            DateTime CombinedStart = StartDate.Date + StartTime.TimeOfDay;
            DateTime CombinedEnd = EndDate.Date + EndTime.TimeOfDay;
            if (CombinedStart > CombinedEnd)
            {
                e.Success = false;
                e.ErrorMessage = "End Date and Time must be later than Start Date and Time";
                uxDateInField.MarkInvalid();
                uxDateOutField.MarkInvalid();
                uxTimeInField.MarkInvalid();
                uxTimeOutField.MarkInvalid();
                uxAddTime.Disabled = true;
            }
            else
            {
                e.Success = true;
                uxDateInField.ClearInvalid();
                uxDateInField.MarkAsValid();
                uxDateOutField.ClearInvalid();
                uxDateOutField.MarkAsValid();
                uxTimeInField.ClearInvalid();
                uxTimeInField.MarkAsValid();
                uxTimeOutField.ClearInvalid();
                uxTimeOutField.MarkAsValid();
                uxAddTime.Disabled = false;
            }
        }
    }
}