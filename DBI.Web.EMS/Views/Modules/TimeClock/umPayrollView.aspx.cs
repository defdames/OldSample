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
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class WebForm3 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.TimeClock.Payroll"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }


            if (uxToggleSubmitted.Checked)
            { uxSubmitButton.Disabled = true; }
            else
            { uxSubmitButton.Disabled = false; }
        }

        protected void deGetEmployeesHourData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            { 
            if (validateComponentSecurity("SYS.TimeClock.Payroll"))
            {
                var data = TIMECLOCK.EmployeeTimeCompletedApprovedPayroll();

                foreach (var item in data)
                {
                    TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                    DateTime dow = (DateTime)item.TIME_IN;


                    TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                    item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");


                    TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                    item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                    if (item.ADJUSTED_LUNCH != null)
                    {
                        TimeSpan adjustedlunch = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_LUNCH.Value));
                        item.ADJUSTED_LUNCH_GRID = adjustedlunch.ToString("dd\\.hh\\:mm");
                    }
                    else
                    {
                        item.ADJUSTED_LUNCH_GRID = "0.00:00";
                    }


                }
                uxPayrollAuditStore.DataSource = data;
            }

             if ((uxToggleSubmitted.Checked) && (validateComponentSecurity("SYS.TimeClock.Payroll")))
            {
                var data = TIMECLOCK.EmployeeTimeCompletedApprovedSubmittedPayroll();
                foreach (var item in data)
                {
                    TimeSpan ts = (DateTime)item.TIME_OUT - (DateTime)item.TIME_IN;
                    DateTime dow = (DateTime)item.TIME_IN;


                    TimeSpan adjustedhours = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_HOURS.Value));
                    item.ADJUSTED_HOURS_GRID = adjustedhours.ToString("dd\\.hh\\:mm");


                    TimeSpan actualhours = TimeSpan.FromHours(decimal.ToDouble(item.ACTUAL_HOURS.Value));
                    item.ACTUAL_HOURS_GRID = actualhours.ToString("dd\\.hh\\:mm");

                    if (item.ADJUSTED_LUNCH != null)
                    {
                        TimeSpan adjustedlunch = TimeSpan.FromHours(decimal.ToDouble(item.ADJUSTED_LUNCH.Value));
                        item.ADJUSTED_LUNCH_GRID = adjustedlunch.ToString("dd\\.hh\\:mm");
                    }
                    else
                    {
                        item.ADJUSTED_LUNCH_GRID = "0.00:00";
                    }


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

        public static long generatePayrollAuditSequence()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select XXDBI.XXDBI_PAYROLL_AUDIT_S.NEXTVAL from dual";
                long query = _context.Database.SqlQuery<long>(sql).First();
                return query;
            }
        }

        public static long getEmployeeBusinedId(long PersonId)
         {
             using (Entities _context = new Entities())
             {
                 string sql = @"SELECT hr.organization_id
                                FROM apps.per_all_assignments_f a
                                INNER JOIN
                                  ( SELECT person_id,
                                      MAX (effective_start_date) AS effective_start_date
                                      FROM apps.per_all_assignments_f
                                      GROUP BY person_id) b
                                ON a.person_id = b.person_id
                                INNER JOIN apps.pay_payrolls_x x ON x.payroll_id = a.PAYROLL_ID
                                INNER JOIN  apps.hr_operating_units hr ON hr.set_of_books_id = x.gl_set_of_books_id
                                AND a.effective_start_date = b.effective_start_date
                                WHERE a.person_id = (" + PersonId + ")";
                 long query = _context.Database.SqlQuery<long>(sql).First();
                 return query;
             }

         }

        //protected void deSubmitTime (object sender, DirectEventArgs e)
        //{
        //            //Get the support project information
        //            //var dataSupport = (from p in _context.PROJECTS_V
        //            //            join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
        //            //            where p.PROJECT_ID == r.SUPPORT_PROJ_ID
        //            //            select new {p.SEGMENT1,l.REGION}).SingleOrDefault();
                    
        //            List<TIME_CLOCK> SubmittedTime = JSON.Deserialize<List<TIME_CLOCK>>(e.ExtraParams["SubmittedTime"]);
        //            long person_id = Convert.ToInt64(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));

        //            TIMECLOCK.EmployeeTimeSelectionSubmitted(SubmittedTime);
                    
        //    foreach (TIME_CLOCK Submitted in SubmittedTime)
        //    {

        //        using (Entities _context = new Entities())
        //        {
        //            var r = (from tc in _context.TIME_CLOCK
        //                     join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
        //                     where tc.TIME_CLOCK_ID == Submitted.TIME_CLOCK_ID
        //                     select new { tc.PERSON_ID, ev.EMPLOYEE_NUMBER, ev.FULL_NAME, tc.ADJUSTED_HOURS }).SingleOrDefault();



        //            XXDBI_PAYROLL_AUDIT_V dtrecord = new XXDBI_PAYROLL_AUDIT_V();
        //            dtrecord.PAYROLL_AUDIT_ID = generatePayrollAuditSequence();
        //            //dtrecord.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
        //            dtrecord.EMPLOYEE_NUMBER = r.EMPLOYEE_NUMBER;
        //            dtrecord.EMPLOYEE_NAME =  r.FULL_NAME;
        //            dtrecord.ELEMENT = "Time Entry Wages";
        //            dtrecord.STATUS = "UNPROCESSED";
        //            dtrecord.OVERTIME_STATUS = "UNPROCESSED";
        //            dtrecord.FRINGE_STATUS = "UNPROCESSED";
        //            dtrecord.PROJECT_STATUS = "UNPROCESSED";
        //            dtrecord.ORG_ID = (decimal)getEmployeeBusinedId(person_id);
        //            dtrecord.CREATED_BY = person_id;
        //            dtrecord.CREATION_DATE = DateTime.Now;
        //            dtrecord.LAST_UPDATE_DATE = DateTime.Now;
        //            dtrecord.LAST_UPDATED_BY = person_id;
        //            dtrecord.SLIDING_SCALE_FLAG = "N";
        //            dtrecord.DAILY_OVERTIME_FLAG = "N";
        //            dtrecord.WAGE_SOURCE = "Regular";
        //            dtrecord.ADJUSTMENT = "N";
        //            dtrecord.FRINGE_RATE = 0;
        //            dtrecord.TOTAL_HOURS = r.ADJUSTED_HOURS;

                    

        //            //Get the total hours (Time Entry Wages)
        //            TimeSpan span = new TimeSpan();

        //            //Get day of the week and add time
        //            switch((int)Submitted.TIME_IN.Value.DayOfWeek)
        //            {
        //                case 0:
        //                    dtrecord.SUNDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                case 1:
        //                    dtrecord.MONDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                case 2:
        //                    dtrecord.TUESDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                case 3:
        //                    dtrecord.WEDNESDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                case 4:
        //                    dtrecord.THURSDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                case 5:
        //                    dtrecord.FRIDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                case 6:
        //                    dtrecord.SATURDAY = dtrecord.TOTAL_HOURS;
        //                    break;
        //                default:
        //                    break;
        //            }

        //            DateTime current = DateTime.Now;

        //            dtrecord.PREVAILING_WAGE_RATE = null;
        //            dtrecord.EFFECTIVE_START_DATE = current.GetFirstDayOfWeek().Date;
        //            dtrecord.EFFECTIVE_END_DATE = current.GetLastDayOfWeek().Date;
        //            GenericData.Insert<XXDBI_PAYROLL_AUDIT_V>(dtrecord);
        //            uxPayrollAuditStore.Reload();
        //            uxPayrollAuditGrid.Refresh();
        //        }

        //        }
        //    }

        private static decimal getDays(List<TIME_CLOCK> employeeData, string dayweek)
        {
           
            decimal? returnhours = 0;
            var _value = employeeData.Where(x => x.DAY_OF_WEEK == dayweek).ToList();
                foreach(var _timevalue in _value)
                {
                    if(_timevalue != null)
                    {
                    returnhours = _value.Sum(x => x.ACTUAL_HOURS);
                  
                    }
                }
            return (decimal)returnhours;
        }

        protected void deSubmitTime(object sender, DirectEventArgs e)
            {
                List<TIME_CLOCK> SubmittedTime = JSON.Deserialize<List<TIME_CLOCK>>(e.ExtraParams["SubmittedTime"]);
                long person_id = Convert.ToInt64(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
                DateTime current = DateTime.Now;

                TIMECLOCK.EmployeeTimeSelectionSubmitted(SubmittedTime);

                 using (Entities _context = new Entities())
                    {

                        List<DistinctPerson> _employeeSubmittedTime = SubmittedTime.GroupBy(x => x.PERSON_ID).Select(g => new DistinctPerson{employee_id = g.Key}).ToList();

                        foreach(DistinctPerson _person in _employeeSubmittedTime)
                        {

                            var _employeeInfo = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == _person.employee_id).SingleOrDefault();
                            var _employeeTime = SubmittedTime.Where(x => x.PERSON_ID == _person.employee_id).ToList();
                            var _TimeSum = _employeeTime.Sum(x => x.ACTUAL_HOURS);
                            var _DailySum = _employeeTime.GroupBy(x => x.DAY_OF_WEEK).Select(group => group.Sum(x => x.ACTUAL_HOURS));


                            XXDBI_PAYROLL_AUDIT_V dtrecord = new XXDBI_PAYROLL_AUDIT_V();
                            dtrecord.PAYROLL_AUDIT_ID = generatePayrollAuditSequence();
                            //dtrecord.DA_HEADER_ID = xxdbiDailyActivityHeader.DA_HEADER_ID;
                            dtrecord.EMPLOYEE_NUMBER = _employeeInfo.EMPLOYEE_NUMBER;
                            dtrecord.EMPLOYEE_NAME =  _employeeInfo.FULL_NAME;
                            dtrecord.ELEMENT = "Time Entry Wages";
                            dtrecord.STATUS = "UNPROCESSED";
                            dtrecord.OVERTIME_STATUS = "UNPROCESSED";
                            dtrecord.FRINGE_STATUS = "UNPROCESSED";
                            dtrecord.PROJECT_STATUS = "UNPROCESSED";
                            dtrecord.ORG_ID = (decimal)getEmployeeBusinedId(_employeeInfo.PERSON_ID);
                            dtrecord.CREATED_BY = person_id;
                            dtrecord.CREATION_DATE = DateTime.Now;
                            dtrecord.LAST_UPDATE_DATE = DateTime.Now;
                            dtrecord.LAST_UPDATED_BY = person_id;
                            dtrecord.SLIDING_SCALE_FLAG = "N";
                            dtrecord.DAILY_OVERTIME_FLAG = "N";
                            dtrecord.WAGE_SOURCE = "Regular";
                            dtrecord.ADJUSTMENT = "N";
                            dtrecord.FRINGE_RATE = 0;
                            dtrecord.TOTAL_HOURS =_TimeSum;
                            dtrecord.FRINGE_RATE = 0;
                            dtrecord.PREVAILING_WAGE_RATE = (decimal)DBI.Data.TIMECLOCK.ReturnEmployeeSalary(_employeeInfo.PERSON_ID);
                            dtrecord.EFFECTIVE_START_DATE = current.GetFirstDayOfWeek().AddDays(-7).Date;
                            dtrecord.EFFECTIVE_END_DATE = current.GetLastDayOfWeek().AddDays(-7).Date;

                            //XXDBI_PAYROLL_AUDIT_V _payrollData = new XXDBI_PAYROLL_AUDIT_V();

                            foreach (var _time in _employeeTime)
                            {
                                dtrecord.SUNDAY = (decimal)getDays(_employeeTime, "Sunday");
                            }
                            foreach (var _time in _employeeTime)
                            {
                                dtrecord.MONDAY = (decimal)getDays(_employeeTime, "Monday");
                            }
                            foreach (var _time in _employeeTime)
                            {
                            dtrecord.TUESDAY = (decimal)getDays(_employeeTime, "Tuesday");
                            }
                            foreach (var _time in _employeeTime)
                            {
                                dtrecord.WEDNESDAY = (decimal)getDays(_employeeTime, "Wednesday");
                            }
                            foreach (var _time in _employeeTime)
                            {
                                dtrecord.THURSDAY = (decimal)getDays(_employeeTime, "Thursday");
                            }
                            foreach (var _time in _employeeTime)
                            {
                                dtrecord.FRIDAY = (decimal)getDays(_employeeTime, "Friday");
                            }
                            foreach (var _time in _employeeTime)
                            {
                                dtrecord.SATURDAY = (decimal)getDays(_employeeTime, "Saturday");
                            }
                            
                            GenericData.Insert<XXDBI_PAYROLL_AUDIT_V>(dtrecord);
                            uxPayrollAuditStore.Reload();
                            uxPayrollAuditGrid.Refresh();
                        }
                    }
            }
   }

    public class DistinctPerson
    {
        public decimal employee_id { get; set; }
    }
}

