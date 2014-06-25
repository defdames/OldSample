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
            if (!X.IsAjaxRequest) ; 
        }

        protected void deGetEmployeesHourData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            { 
            if (validateComponentSecurity("SYS.TimeClock.Payroll"))
            {
                var data = TIME_CLOCK.EmployeeTimeCompletedApprovedPayroll();

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

             if ((uxToggleSubmitted.Checked) && (validateComponentSecurity("SYS.TimeClock.Payroll")))
            {
                var data = TIME_CLOCK.EmployeeTimeCompletedApprovedSubmittedPayroll();
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
        }


        protected void deEditTime(object sender, DirectEventArgs e)
        {
            try
            {

                string _tcId = e.ExtraParams["id"];

                string url = "/Views/Modules/TimeClock/Edit/umEditTime.aspx?tcID=" + _tcId;

                Window win = new Window
                {
                    ID = "uxAddEditTime",
                    Title = "Edit Time",
                    Height = 350,
                    Width = 500,
                    Modal = true,
                    Resizable = false,
                    CloseAction = CloseAction.Destroy,
                    Loader = new ComponentLoader
                    {
                        Mode = LoadMode.Frame,
                        DisableCaching = true,
                        Url = url,
                        AutoLoad = true,
                        LoadMask =
                        {
                            ShowMask = true
                        }
                    }


                };
                win.Listeners.Close.Handler = "#{uxPayrollAuditGrid}.getStore().load();";

                win.Render(this.Form);
                win.Show();

            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }


        }

        protected void deDeleteTime(object sender, DirectEventArgs e)
        {
            try
            {

                string _tcId = e.ExtraParams["delId"];

                string url = "/Views/Modules/TimeClock/Edit/umDeleteTime.aspx?tcID=" + _tcId;

                Window win = new Window
                {
                    ID = "uxDeleteTime",
                    Title = "Delete Time",
                    Height = 350,
                    Width = 500,
                    Modal = true,
                    Resizable = false,
                    CloseAction = CloseAction.Destroy,
                    Loader = new ComponentLoader
                    {
                        Mode = LoadMode.Frame,
                        DisableCaching = true,
                        Url = url,
                        AutoLoad = true,
                        LoadMask =
                        {
                            ShowMask = true
                        }
                    }


                };
                win.Listeners.Close.Handler = "#{uxPayrollAuditGrid}.getStore().load();";

                win.Render(this.Form);
                win.Show();

            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
        }


        protected void deSubmitTime (object sender, DirectEventArgs e)
        {

        }

        

    }

    //public class EmployeeTimePayroll
    //{
    //    public decimal TIME_CLOCK_ID { get; set; }
    //    public string EMPLOYEE_NAME { get; set; }
    //    public DateTime TIME_IN { get; set; }
    //    public DateTime TIME_OUT { get; set; }
    //    public string ADJUSTED_HOURS_GRID { get; set; }
    //    public string DAY_OF_WEEK { get; set; }
    //    public string ACTUAL_HOURS_GRID { get; set; }
    //    public decimal? ACTUAL_HOURS { get; set; }
    //    public decimal? ADJUSTED_HOURS { get; set; }

    //}
}