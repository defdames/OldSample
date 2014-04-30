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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            GetEmployeeHoursData();
            
        }
        protected void GetEmployeeHoursData()
        {

            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));


            using (Entities _context = new Entities())
            {
                var data = (from tc in _context.TIME_CLOCK
                            join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                            where tc.SUPERVISOR_ID == person_id && tc.COMPLETED == "Y"                                                        
                            select new EmployeeTime { TIME_IN = (DateTime)tc.TIME_IN, TIME_OUT = (DateTime)tc.TIME_OUT, EMPLOYEE_NAME = ev.EMPLOYEE_NAME, DAY_OF_WEEK = tc.DAY_OF_WEEK, TIME_CLOCK_ID = tc.TIME_CLOCK_ID}).ToList();
               
                foreach (var item in data)
                {
                    TimeSpan ts = item.TIME_OUT - item.TIME_IN;
                    DateTime dow = item.TIME_IN;
                    
                    //Calcualtion to round time to quarter increments and display the adjusted hours
                    double adjtime = (ts.Minutes > 0 && ts.Minutes <= 8) ? 0
                         : (ts.Minutes > 8 && ts.Minutes <= 23) ? .25
                         : (ts.Minutes > 23 && ts.Minutes <= 38) ? .50
                         : (ts.Minutes > 38 && ts.Minutes <= 53) ? .75
                         : (ts.Minutes > 53 && ts.Minutes <= 60) ? 1
                         : 0;
                 
                    decimal fixedtime = ts.Hours + (decimal)adjtime;
                    TimeSpan fixedtime2 = TimeSpan.FromHours(decimal.ToDouble(fixedtime));
                    item.TOTAL_HOURS = fixedtime2.ToString("hh\\:mm");

                    item.ACTUAL_HOURS = ts.ToString("hh\\:mm");
                   
                    
                }
                uxEmployeeHoursStore.DataSource = data;
                

            }

        }
        
        protected void deApproveTime(object sender, DirectEventArgs e)
        {
            List<TIME_CLOCK> ApprovedTime = JSON.Deserialize<List<TIME_CLOCK>>(e.ExtraParams["ApprovedTime"]);
            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            
            foreach (TIME_CLOCK Approved in ApprovedTime)
            {
                TIME_CLOCK data;
                using (Entities _context = new Entities())
            {
                 data = (from tc in _context.TIME_CLOCK
                            join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                            where tc.SUPERVISOR_ID == person_id && tc.COMPLETED == "Y" && tc.TIME_CLOCK_ID  == Approved.TIME_CLOCK_ID
                            select tc).SingleOrDefault();
            }
                data.APPROVED = "Y";
                GenericData.Update<TIME_CLOCK>(data);
            }
        }
      

        protected void deUpdateEmployeeGrid(object sender, StoreReadDataEventArgs e)
        {

        }

   
    }

    public class EmployeeTime
    {
        public decimal TIME_CLOCK_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public DateTime TIME_IN { get; set; }
        public DateTime TIME_OUT { get; set; }
        public string TOTAL_HOURS { get; set; }
        public string DAY_OF_WEEK { get; set; }
        public string ACTUAL_HOURS { get; set; }
        
    }
}
