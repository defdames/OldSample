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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string person_name = Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);
            GetTimeRecord();
            FillGridPanel();
            uxUser_NameTextBox.Text = person_name;

        }


        protected void deSetTime(object sender, DirectEventArgs e)
        {
            //Insert person_id time_in and supervisor_id.  Also set some fields to default values


            SYS_USER_INFORMATION data;
            TIME_CLOCK time;

            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            using (Entities _context = new Entities())
            {

                time = (from tc in _context.TIME_CLOCK
                        where tc.PERSON_ID == person_id && tc.COMPLETED == "N"
                        select tc).SingleOrDefault();

            }

            if (time == null)
            {
                uxTime_InTextBox.Text = DateTime.Now.ToString();
                uxTimeButton.Text = "Clock In";
                DateTime dow = Convert.ToDateTime(uxTime_InTextBox.Text);

                using (Entities _context = new Entities())
                {
                    data = (from su in _context.SYS_USER_INFORMATION
                            where su.PERSON_ID == person_id
                            select su).Single();

                }

                TIME_CLOCK Record = new TIME_CLOCK
                {
                    PERSON_ID = person_id,
                    TIME_IN = Convert.ToDateTime(uxTime_InTextBox.Text),
                    APPROVED = "N",
                    COMPLETED = "N",
                    SUBMITTED = "N",
                    DAY_OF_WEEK = dow.DayOfWeek.ToString(),
                    SUPERVISOR_ID = data.SUPERVISOR_ID,

                };
                GenericData.Insert<TIME_CLOCK>(Record);
                uxHoursStore.Reload();
                uxTimeButton.Text = "Clock Out";
            }


            else
            {
                uxTime_OutTextBox.Text = DateTime.Now.ToString();
                using (Entities _context = new Entities())
                {


                    time = (from tc in _context.TIME_CLOCK
                            where tc.PERSON_ID == person_id && tc.COMPLETED == "N"
                            select tc).SingleOrDefault();
                }


                time.TIME_OUT = Convert.ToDateTime(uxTime_OutTextBox.Text);
                time.COMPLETED = "Y";
                TimeSpan ts = (DateTime)time.TIME_OUT - (DateTime)time.TIME_IN;
                time.ACTUAL_HOURS = (decimal)ts.TotalHours;

                GenericData.Update<TIME_CLOCK>(time);
                uxHoursStore.Reload();
                uxTimeButton.Disable(true);
            }

        }

      

        protected void deGetTimeRecord(object sender, DirectEventArgs e)
        {
            GetTimeRecord();

        }

        protected void GetTimeRecord()
        {       //Check for an get exsisting record so user can clock out


            decimal person_id = Decimal.Parse(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));

            TIME_CLOCK data;
            using (Entities _context = new Entities())
            {

                data = (from tc in _context.TIME_CLOCK
                        where tc.PERSON_ID == person_id && tc.COMPLETED == "N"
                        select tc).SingleOrDefault();

            }

            if (data != null)
            {
                uxTime_InTextBox.Text = data.TIME_IN.ToString();
                uxTimeButton.Text = "Clock Out";
            }
            else
            {
                uxTimeButton.Text = "Clock In";
            }


        }
        protected void FillGridPanel()
        {
            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            using (Entities _context = new Entities())
            {
                var data = (from tc in _context.TIME_CLOCK
                            where tc.PERSON_ID == person_id
                            select new EmployeeTimeView { TIME_IN = (DateTime)tc.TIME_IN, TIME_OUT = tc.TIME_OUT}).ToList();
                
                foreach (var item in data)
                {
                    if (item.TIME_OUT != null)
                    {
                        TimeSpan ts = (DateTime)item.TIME_OUT - item.TIME_IN;
                        item.TOTAL_HOURS = ts.ToString(@"dd\.hh\:mm");
                    }

                }


                uxHoursStore.DataSource = data;
            }

        }
    }
    public class EmployeeTimeView
    {
        public DateTime TIME_IN { get; set; }
        public DateTime? TIME_OUT { get; set; }
        public string TOTAL_HOURS { get; set; }
    }

}