﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class TIMECLOCK
    {
        public static List<Employee> EmployeeTime()
        {

            using (Entities _context = new Entities())
            {


                var _data = (from tc in _context.TIME_CLOCK
                             join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                             select new Employee
                             {
                                 TIME_IN = ((DateTime)tc.MODIFIED_TIME_IN == null) ? tc.TIME_IN : tc.MODIFIED_TIME_IN,
                                 TIME_OUT = ((DateTime)tc.MODIFIED_TIME_OUT == null) ? tc.TIME_OUT : tc.MODIFIED_TIME_OUT,
                                 EMPLOYEE_NAME = ev.EMPLOYEE_NAME,
                                 DAY_OF_WEEK = tc.DAY_OF_WEEK,
                                 TIME_CLOCK_ID = tc.TIME_CLOCK_ID,
                                 ADJUSTED_HOURS = tc.ADJUSTED_HOURS,
                                 ACTUAL_HOURS = tc.ACTUAL_HOURS,
                                 SUBMITTED = tc.SUBMITTED,
                                 APPROVED = tc.APPROVED,
                                 COMPLETED = tc.COMPLETED,
                                 SUPERVISOR_ID = (int)tc.SUPERVISOR_ID,
                                 //MODIFIED_TIME_IN = (DateTime)tc.MODIFIED_TIME_IN,
                                 //MODIFIED_TIME_OUT = (DateTime)tc.MODIFIED_TIME_OUT,
                                 MODIFY_DATE = (DateTime)tc.MODIFY_DATE,
                                 MODIFIED_BY = tc.MODIFIED_BY,
                                 DELETED = tc.DELETED,
                                 DELETED_COMMENTS = tc.DELETED_COMMENTS
                             }).ToList();
                return _data;
            }
        }

        /// <summary>
        /// Returns all employee time by supervisior Id that has been completed by the user and is NOT approved
        /// </summary>
        /// <param name="supervisorId"></param>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedUnapproved(decimal supervisorId)
        {

            var _data = EmployeeTime().Where(x => x.SUPERVISOR_ID == supervisorId && x.COMPLETED == "Y" && x.APPROVED == "N" && x.DELETED == "N").ToList();
            return _data;


        }
        /// <summary>
        /// Returns all employee time that has been completed and NOT approved
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedUnapprovedPayroll()
        {

            var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.APPROVED == "N" && x.DELETED == "N").ToList();
            return _data;

        }
        /// <summary>
        /// Returns all employee time that has been completed by supervisor ID
        /// </summary>
        /// <param name="supervisorId"></param>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompleted(decimal supervisorId)
        {

            var _data = EmployeeTime().Where(x => x.SUPERVISOR_ID == supervisorId && x.COMPLETED == "Y" && x.DELETED == "N").ToList();
            return _data;


        }
        /// <summary>
        /// Reuturns all employee time that has been completed
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedPayroll()
        {
            var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.DELETED == "N").ToList();
            return _data;


        }
        /// <summary>
        /// Returns all Employee time that has been completed and approved
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedApprovedPayroll()
        {

            var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.APPROVED == "Y" && x.SUBMITTED == "N" && x.DELETED == "N").ToList();
            return _data;

        }
        /// <summary>
        /// Shows all records from the TIME CLOCK table under the Payroll Manager screen
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedApprovedSubmittedPayroll()
        {
            var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.APPROVED == "Y" && x.DELETED == "N").ToList();
            return _data;
        }
        /// <summary>
        /// Returns data for Edit screen popup on both the manager and payroll screen
        /// </summary>
        /// <param name="tcID"></param>
        /// <returns></returns>
        public static DateTime ManagerDateInEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_IN;
            return (DateTime)_data;
        }
        /// <summary>
        /// Returns data for Edit screen popup on both the manager and payroll screen
        /// </summary>
        /// <param name="tcID"></param>
        /// <returns></returns>
        public static TimeSpan ManagerTimeInEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_IN;
            TimeSpan ts = _data.Value.TimeOfDay;

            return ts;
        }
        /// <summary>
        /// Returns data for Edit screen popup on both the manager and payroll screen
        /// </summary>
        /// <param name="tcID"></param>
        /// <returns></returns>
        public static DateTime ManagerDateOutEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_OUT;
            return (DateTime)_data;
        }
        /// <summary>
        /// Returns data for Edit screen popup on both the manager and payroll screen
        /// </summary>
        /// <param name="tcID"></param>
        /// <returns></returns>
        public static TimeSpan ManagerTimeOutEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_OUT;
            TimeSpan ts = _data.Value.TimeOfDay;

            return ts;
        }

        /// <summary>
        /// Updates TIME CLOCK table with new time edited by Manager or Payroll Manager
        /// </summary>
        /// <param name="tcID"></param>
        /// <param name="newTimeIn"></param>
        /// <param name="newTimeOut"></param>
        /// <param name="personName"></param>

        public static void InsertEditedEmployeeTime(decimal tcID, DateTime newTimeIn, DateTime newTimeOut, string personName)
        {

            TIME_CLOCK _data;
            using (Entities _context = new Entities())
            {
                TimeSpan ts = newTimeOut - newTimeIn;
                decimal adjts = ConvertTimeToOraclePayrollFormat(ts);


                _data = _context.TIME_CLOCK.Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault();
                _data.ACTUAL_HOURS = (decimal)ts.TotalHours;
                _data.ADJUSTED_HOURS = adjts;
                _data.MODIFIED_TIME_IN = newTimeIn;
                _data.MODIFIED_TIME_OUT = newTimeOut;
                _data.MODIFIED_BY = personName;
                _data.MODIFY_DATE = DateTime.Now;

            }

            DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
        }

        /// <summary>
        /// Marks a flag on the TIMECLOCK table that a time was record was deleted.  Said flg will hide record from all screens
        /// </summary>
        /// <param name="tcId"></param>
        /// <param name="comment"></param>
        /// <param name="personName"></param>
        public static void DeleteEmployeeTime(decimal tcID, string comment, string personName)
        {
            TIME_CLOCK _data;
            using (Entities _context = new Entities())
            {
                _data = _context.TIME_CLOCK.Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault();
                _data.DELETED = "Y";
                _data.DELETED_COMMENTS = comment;
                _data.MODIFIED_BY = personName;
                _data.MODIFY_DATE = DateTime.Now;

            }
            DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
        }

        /// <summary>
        /// Approves Employee time so payroll can submit
        /// </summary>
        /// <param name="selection"></param>
        public static void EmployeeTimeSelectionApproved(List<TIME_CLOCK> selection)
        {

            foreach (TIME_CLOCK selected in selection)
            {

                TIME_CLOCK _data;
                using (Entities _context = new Entities())
                {
                    _data = _context.TIME_CLOCK.Where(x => x.COMPLETED == "Y" && x.TIME_CLOCK_ID == selected.TIME_CLOCK_ID).SingleOrDefault();

                    _data.APPROVED = "Y";
                }
                DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
            }

        }
        /// <summary>
        /// Submit selected time for payroll
        /// </summary>
        /// <param name="selection"></param>
        public static void EmployeeTimeSelectionSubmitted(List<TIME_CLOCK> selection)
        {
            foreach (TIME_CLOCK selected in selection)
            {
                TIME_CLOCK _data;
                using (Entities _context = new Entities())
                {
                    _data = _context.TIME_CLOCK.Where(x => x.APPROVED == "Y" && x.TIME_CLOCK_ID == selected.TIME_CLOCK_ID).SingleOrDefault();
                    _data.SUBMITTED = "Y";
                }
                DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
            }
        }

        /// <summary>
        /// ADjusts time to be  for oracle payroll
        /// </summary> Adjust time to nearest quarter of hour and store in table
        /// <param name="adjts"></param>
        /// <returns></returns>
        public static decimal ConvertTimeToOraclePayrollFormat(TimeSpan adjts)
        {

            double adjtime = (adjts.Minutes > 0 && adjts.Minutes <= 8) ? 0
                         : (adjts.Minutes > 8 && adjts.Minutes <= 23) ? .25
                         : (adjts.Minutes > 23 && adjts.Minutes <= 38) ? .50
                         : (adjts.Minutes > 38 && adjts.Minutes <= 53) ? .75
                         : (adjts.Minutes > 53 && adjts.Minutes <= 60) ? 1
                         : 0;

            decimal fixedtime = adjts.Hours + (decimal)adjtime;
            return fixedtime;
        }

        public class Employee : TIME_CLOCK
        {
            public decimal TIME_CLOCK_ID { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public DateTime? TIME_IN { get; set; }
            public DateTime? TIME_OUT { get; set; }
            public string ADJUSTED_HOURS_GRID { get; set; }
            public string DAY_OF_WEEK { get; set; }
            public string ACTUAL_HOURS_GRID { get; set; }
            public decimal? ACTUAL_HOURS { get; set; }
            public decimal? ADJUSTED_HOURS { get; set; }
            public string APPROVED { get; set; }
            public string SUBMITTED { get; set; }
            public int SUPERVISOR_ID { get; set; }
            public string COMPLETED { get; set; }
            public string MODIFIED_BY { get; set; }
            public DateTime? MODIFIED_TIME_IN { get; set; }
            public DateTime? MODIFIED_TIME_OUT { get; set; }
            public DateTime? MODIFY_DATE { get; set; }
            public string DELETED { get; set; }
            public string DELETED_COMMENTS { get; set; }
            public string TIME_DIFF { get; set; }



        }
    }
}
