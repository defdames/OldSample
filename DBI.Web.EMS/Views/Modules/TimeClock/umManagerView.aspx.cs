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
using System.Globalization;

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class WebForm2 : BasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            GetEmployeeHoursData();
            
        }

        protected void GetEmployeeHoursData()
        {

            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            
            

            using (Entities _context = new Entities())
            {   //Manager Query
                if (validateComponentSecurity("SYS.TimeClock.Manager"))
                {
                    var data = (from tc in _context.TIME_CLOCK
                                join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                                where tc.SUPERVISOR_ID == person_id && tc.COMPLETED == "Y" && tc.APPROVED == "N"
                                select new EmployeeTime { TIME_IN = (DateTime)tc.TIME_IN, TIME_OUT = (DateTime)tc.TIME_OUT, EMPLOYEE_NAME = ev.EMPLOYEE_NAME, DAY_OF_WEEK = tc.DAY_OF_WEEK, TIME_CLOCK_ID = tc.TIME_CLOCK_ID, ADJUSTED_HOURS = tc.ADJUSTED_HOURS, ACTUAL_HOURS = tc.ACTUAL_HOURS }).ToList();


                    foreach (var item in data)
                    {
                        TimeSpan ts = item.TIME_OUT - item.TIME_IN;
                        DateTime dow = item.TIME_IN;




                        TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                        item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("hh\\:mm");


                        TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                        item.ACTUAL_HOURS_GRID = actualhours.ToString("hh\\:mm");


                    }
                    uxEmployeeHoursStore.DataSource = data;

                }

                else if (validateComponentSecurity("SYS.TimeClock.Payroll"))
                {   //Payroll query
                     var data = (from tc in _context.TIME_CLOCK
                                join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                                where tc.COMPLETED == "Y" && tc.APPROVED == "N"
                                select new EmployeeTime { TIME_IN = (DateTime)tc.TIME_IN, TIME_OUT = (DateTime)tc.TIME_OUT, EMPLOYEE_NAME = ev.EMPLOYEE_NAME, DAY_OF_WEEK = tc.DAY_OF_WEEK, TIME_CLOCK_ID = tc.TIME_CLOCK_ID, ADJUSTED_HOURS = tc.ADJUSTED_HOURS, ACTUAL_HOURS = tc.ACTUAL_HOURS }).ToList();


                    foreach (var item in data)
                    {
                        TimeSpan ts = item.TIME_OUT - item.TIME_IN;
                        DateTime dow = item.TIME_IN;




                        TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                        item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("hh\\:mm");


                        TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                        item.ACTUAL_HOURS_GRID = actualhours.ToString("hh\\:mm");


                    }
                    uxEmployeeHoursStore.DataSource = data;
                }

            }

        }
        
        protected void deApproveTime(object sender, DirectEventArgs e)
        {
            string person_name = Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);
            List<TIME_CLOCK> ApprovedTime = JSON.Deserialize<List<TIME_CLOCK>>(e.ExtraParams["ApprovedTime"]);
            ChangeRecords<EmployeeTime> NewTime = new StoreDataHandler(e.ExtraParams["NewTime"]).BatchObjectData<EmployeeTime>();
 
            
            foreach (TIME_CLOCK Approved in ApprovedTime)
            {
                TIME_CLOCK data;
                using (Entities _context = new Entities())
                {
                     data = (from tc in _context.TIME_CLOCK
                                join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                                where tc.COMPLETED == "Y" && tc.TIME_CLOCK_ID == Approved.TIME_CLOCK_ID  //Took out tc.SUPERVISOR_ID == person_id
                                select tc).SingleOrDefault();
                }
                data.APPROVED = "Y";
                GenericData.Update<TIME_CLOCK>(data);
                uxEmployeeHoursStore.Reload();
               
               
            }

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
        public string DAY_OF_WEEK { get; set; }
        public string ACTUAL_HOURS_GRID { get; set; }
        public decimal? ACTUAL_HOURS { get; set; }
        public decimal? ADJUSTED_HOURS { get; set; }
        
    }
}
