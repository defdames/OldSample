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
    public partial class WebForm1 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {   //Load loggeed in user info as well check for unfinisehd records and load the users gridpanel with exsisting records and user name
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
                //Check if this is a new record
                time = (from tc in _context.TIME_CLOCK
                        where tc.PERSON_ID == person_id && tc.COMPLETED == "N"
                        select tc).SingleOrDefault();

            }

            if (time == null)  //If it is a new record do the following
            {


                uxTime_InTextBox.Text = uxDateTime.Value.ToString();
                //uxTime_InTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
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
                    DELETED = "N",
                    DAY_OF_WEEK = dow.DayOfWeek.ToString(),
                    SUPERVISOR_ID = data.SUPERVISOR_ID,

                };
                GenericData.Insert<TIME_CLOCK>(Record);
                uxHoursStore.Reload();
                uxTimeButton.Text = "Clock Out";
            }


            else  //If it isnt a new record due the following
            {
                uxTime_OutTextBox.Text = uxDateTime.Value.ToString();
                //uxTime_OutTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
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
                decimal adjts = GetAdjustedHours((TimeSpan)ts);
                time.ADJUSTED_HOURS = adjts;
                decimal lcts = GetLunchTime(adjts);
                time.ADJUSTED_LUNCH = lcts;
               
                


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
        {       //Check for and get exsisting record so user can clock out


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
                uxTime_InTextBox.Text = Convert.ToDateTime(data.TIME_IN).ToString("MM/dd/yyyy hh:mm tt");
                uxTimeButton.Text = "Clock Out";
            }
            else
            {
                uxTimeButton.Text = "Clock In";
            }


        }
        protected void FillGridPanel() 
        {   //Fill the Users Gridpanel so they can see their exsisting time
            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            using (Entities _context = new Entities())
            {
                var data = (from tc in _context.TIME_CLOCK
                            where tc.PERSON_ID == person_id && tc.DELETED =="N"
                            select new EmployeeTimeView {TIME_IN = (DateTime)tc.TIME_IN, TIME_OUT = tc.TIME_OUT, MODIFIED_BY = tc.MODIFIED_BY, APPROVED = tc.APPROVED, MODIFIED_TIME_IN = tc.MODIFIED_TIME_IN, MODIFIED_TIME_OUT = tc.MODIFIED_TIME_OUT}).ToList();
                
                foreach (var item in data)
                {
                    //if (item.MODIFIED_TIME_OUT != null)
                    //{
                    //    TimeSpan ts = (DateTime)item.MODIFIED_TIME_OUT - (DateTime)item.MODIFIED_TIME_IN;
                    //    item.TOTAL_HOURS = ts.ToString("dd\\.hh\\:mm");
                    //}
                     if (item.TIME_OUT != null)
                    {
                        TimeSpan ts = (DateTime)item.TIME_OUT - item.TIME_IN;
                        item.TOTAL_HOURS = ts.ToString("dd\\.hh\\:mm");
                    }                    
                }


                uxHoursStore.DataSource = data;
            }

        }

        protected static decimal GetAdjustedHours(TimeSpan adjts)
        {   //Adjust time to nearest quarter of hour and store in table
            double adjtime = (adjts.Minutes > 0 && adjts.Minutes <= 8) ? 0
                         : (adjts.Minutes > 8 && adjts.Minutes <= 23) ? .25
                         : (adjts.Minutes > 23 && adjts.Minutes <= 38) ? .50
                         : (adjts.Minutes > 38 && adjts.Minutes <= 53) ? .75
                         : (adjts.Minutes > 53 && adjts.Minutes <= 60) ? 1
                         : 0;

            decimal fixedtime = adjts.Hours + (decimal)adjtime;
            return fixedtime;

        }

        protected static decimal GetLunchTime(decimal lcts)
        {
            
            if (lcts >= 5 && lcts < 12)
            {
                decimal lunchded = lcts - .5m;
                //decimal lunchtime = (decimal)lunchded / 60;
                return lunchded;
            }
            else if (lcts < 5)
            {
                decimal lunchded = lcts;
                //decimal lunchtime = (decimal)lunchded / 60;
                return lunchded;
            }
            else
            {
                decimal lunchded = lcts - 1m;
                //decimal lunchtime = (decimal)lunchded / 60;
                return lunchded;
            }
        }
    }
    public class EmployeeTimeView
    {
        public DateTime TIME_IN { get; set; }
        public DateTime? TIME_OUT { get; set; }
        public string TOTAL_HOURS { get; set; }
        public string MODIFIED_BY { get; set; }
        public string APPROVED { get; set; }
        public DateTime? MODIFIED_TIME_IN { get; set; }
        public DateTime? MODIFIED_TIME_OUT { get; set; }

    }

}