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
            if (!X.IsAjaxRequest)
            {
                deGetEmployees();
                uxDateInField.Value = DateTime.Now;
                uxDateOutField.Value = DateTime.Now;
                uxTimeInField.SelectedTime = new TimeSpan(8, 30, 0);
                uxTimeOutField.SelectedTime = new TimeSpan(17, 0, 0);
            }
        }

        protected void deAddTime(object sender, DirectEventArgs e)
        {
            decimal supervisor_id = Decimal.Parse(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            string supervisor_name = User.Identity.Name;
            string person_name = uxEmployees.DisplayField;
            decimal person_id = Decimal.Parse(uxEmployees.SelectedItem.Value);

            DateTime updateTimeIn = uxDateInField.SelectedDate + uxTimeInField.SelectedTime;
            DateTime updateTimeOut = uxDateOutField.SelectedDate + uxTimeOutField.SelectedTime;
            TIMECLOCK.InsertAddedEmployeeTime(updateTimeIn, updateTimeOut, person_name, person_id, supervisor_id, supervisor_name);


            X.Js.Call("parent.App.uxAddTimeWin.close()");
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
        protected void deGetEmployees()
        {
            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));

            using (Entities _context = new Entities())
            {
                var _data = (from su in _context.SYS_USER_INFORMATION
                            where su.SUPERVISOR_ID == person_id
                                select new EmployeeAddView {PERSON_ID = su.PERSON_ID, PERSON_NAME = su.EMPLOYEE_NAME}).ToList();

                uxEmployeesStore.DataSource = _data;

            }
        }

    }
    public class EmployeeAddView
    {
        public DateTime TIME_IN { get; set; }
        public DateTime? TIME_OUT { get; set; }
        public decimal PERSON_ID { get; set; }
        public string PERSON_NAME { get; set; }
        public DateTime? MODIFIED_TIME_IN { get; set; }
        public DateTime? MODIFIED_TIME_OUT { get; set; }

    }
}