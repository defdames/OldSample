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
    public partial class WebForm2 : BasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest) ;

            if (uxToggleApproved.Checked)
            { uxApproveButton.Disabled = true; }
            else
            { uxApproveButton.Disabled = false; }
  

        }
            
        

        protected void deGetEmployeeHoursData(object sender, StoreReadDataEventArgs e)
        {

            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            
            using (Entities _context = new Entities())
            {   //Manager Query
                if ((uxToggleApproved.Checked == false) && (validateComponentSecurity("SYS.TimeClock.Payroll")))
                {   //Payroll query

                    var data = TIMECLOCK.EmployeeTimeCompletedUnapprovedPayroll();

                    foreach (var item in data)
                    {
                        TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                        DateTime dow = (DateTime)item.TIME_IN;


                        TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                        item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");


                        TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                        item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                        TimeSpan adjustedlunch = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_LUNCH.Value));
                        item.ADJUSTED_LUNCH_GRID = adjustedlunch.ToString("dd\\.hh\\:mm");
                       


                    }
                    uxEmployeeHoursStore.DataSource = data;
                }

                else if ((uxToggleApproved.Checked) && (validateComponentSecurity("SYS.TimeClock.Payroll")))
                {   //Payroll query
                    var data = TIMECLOCK.EmployeeTimeCompletedPayroll();


                    foreach (var item in data)
                    {
                        TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                        DateTime dow = (DateTime)item.TIME_IN;

                        TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                        item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");

                        TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                        item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                        TimeSpan adjustedlunch = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_LUNCH.Value));
                        item.ADJUSTED_LUNCH_GRID = adjustedlunch.ToString("dd\\.hh\\:mm");
                       



                    }
                    uxEmployeeHoursStore.DataSource = data;
                }

                else if ((uxToggleApproved.Checked == false) && validateComponentSecurity("SYS.TimeClock.Manager"))
                {

                    var data = TIMECLOCK.EmployeeTimeCompletedUnapproved(person_id);


                    foreach (var item in data)
                    {
                        TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                        DateTime dow = (DateTime)item.TIME_IN;

                        TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                        item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");

                        TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                        item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                        TimeSpan adjustedlunch = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_LUNCH.Value));
                        item.ADJUSTED_LUNCH_GRID = adjustedlunch.ToString("dd\\.hh\\:mm");
                       


                    }
                    uxEmployeeHoursStore.DataSource = data;

                }

                else if ((uxToggleApproved.Checked) && (validateComponentSecurity("SYS.TimeClock.Manager")))
                {
                    var data = TIMECLOCK.EmployeeTimeCompleted(person_id);


                    foreach (var item in data)
                    {
                        TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                        DateTime dow = (DateTime)item.TIME_IN;

                        TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                        item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");

                        TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                        item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                        TimeSpan adjustedlunch = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_LUNCH.Value));
                        item.ADJUSTED_LUNCH_GRID = adjustedlunch.ToString("dd\\.hh\\:mm");
                       
                        

                    }
                    uxEmployeeHoursStore.DataSource = data;
                
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
                win.Listeners.Close.Handler = "#{uxEmployeeHoursGrid}.getStore().load();";

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
                win.Listeners.Close.Handler = "#{uxEmployeeHoursGrid}.getStore().load();";

                win.Render(this.Form);
                win.Show();

            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
        }
        
        protected void deApproveTime(object sender, DirectEventArgs e)
        {
            
            List<TIME_CLOCK> ApprovedTime = JSON.Deserialize<List<TIME_CLOCK>>(e.ExtraParams["ApprovedTime"]);
            ChangeRecords<EmployeeTime> NewTime = new StoreDataHandler(e.ExtraParams["NewTime"]).BatchObjectData<EmployeeTime>();
            string person_name = User.Identity.Name;//Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);


            TIMECLOCK.EmployeeTimeSelectionApproved(ApprovedTime);
            
            //foreach (TIME_CLOCK Approved in ApprovedTime)
            //{
            //    TIME_CLOCK data;
            //    using (Entities _context = new Entities())
            //    {
            //         data = (from tc in _context.TIME_CLOCK
            //                    join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
            //                    where tc.COMPLETED == "Y" && tc.TIME_CLOCK_ID == Approved.TIME_CLOCK_ID  //Took out tc.SUPERVISOR_ID == person_id
            //                    select tc).SingleOrDefault();
            //    }
            //    data.APPROVED = "Y";
            //    GenericData.Update<TIME_CLOCK>(data);
                  uxEmployeeHoursStore.Reload();
               
               
            //}

            foreach (EmployeeTime Updated in NewTime.Updated)
            {
                 TIME_CLOCK data;
                 using (Entities _context = new Entities())
                 {
                     data = (from tc in _context.TIME_CLOCK
                             join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                             where tc.COMPLETED == "Y" && tc.TIME_CLOCK_ID == Updated.TIME_CLOCK_ID  //Took out tc.SUPERVISOR_ID == person_id
                             select tc).SingleOrDefault();
                 }
                 
                 var colonreplace = new NumberFormatInfo();
                 colonreplace.NumberDecimalSeparator = ":";
                 TimeSpan adjusthoursgrid = TimeSpan.Parse(Updated.ADJUSTED_HOURS_GRID);
                 data.ADJUSTED_HOURS = (decimal)adjusthoursgrid.TotalHours;
                 data.MODIFIED_BY = person_name;
                 data.MODIFY_DATE = DateTime.Now;
                 data.APPROVED = "Y";
                 GenericData.Update<TIME_CLOCK>(data);
                 uxEmployeeHoursStore.Reload();
                
            }
        }   
    }

    public class EmployeeTime
    {
        public decimal TIME_CLOCK_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public DateTime TIME_IN { get; set; }
        public DateTime TIME_OUT { get; set; }
        public string ADJUSTED_HOURS_GRID { get; set; }
        public string ADJUSTED_LUNCH_GRID { get; set; }
        public string DAY_OF_WEEK { get; set; }
        public string ACTUAL_HOURS_GRID { get; set; }
        public decimal? ACTUAL_HOURS { get; set; }
        public decimal? ADJUSTED_HOURS { get; set; }
        public string APPROVED { get; set; }
        public string SUBMITTED { get; set; }
        
    }
}
