using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Objects;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public static class ValidationChecks
    {

        /// <summary>
        /// Checks for more than 24 hours in a day by an employee
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        public static List<EmployeeData> checkEmployeeTime(string CheckType)
        {

            using (Entities _context = new Entities())
            {
                var TotalHoursList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                      where d.DAILY_ACTIVITY_HEADER.STATUS != 4 || d.DAILY_ACTIVITY_HEADER.STATUS != 5
                                     group d by new { d.PERSON_ID, d.DAILY_ACTIVITY_HEADER.DA_DATE } into g
                                     select new { g.Key.PERSON_ID, g.Key.DA_DATE, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value))}).ToList();

                int i = 0;
                List<EmployeeData> OffendingProjects = new List<EmployeeData>();
                foreach (var TotalHour in TotalHoursList)
                {
                    if (CheckType == "Hours per day")
                    {
                        if (TotalHour.TotalMinutes / 60 >= 24)
                        {
                            var ProjectsWithEmployeeHoursOver24 = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                   join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                                                   where d.PERSON_ID == TotalHour.PERSON_ID && d.DAILY_ACTIVITY_HEADER.DA_DATE == TotalHour.DA_DATE
                                                                   select new EmployeeData{HEADER_ID = d.HEADER_ID, EMPLOYEE_NAME = e.EMPLOYEE_NAME, DA_DATE = d.DAILY_ACTIVITY_HEADER.DA_DATE}).ToList();
                            foreach (var Project in ProjectsWithEmployeeHoursOver24)
                            {
                                OffendingProjects.Add(new EmployeeData
                                {
                                    HEADER_ID = Project.HEADER_ID,
                                    EMPLOYEE_NAME = Project.EMPLOYEE_NAME,
                                    DA_DATE = Project.DA_DATE
                                });
                            }
                        }
                    }
                    else
                    {
                        if (TotalHour.TotalMinutes / 60 >= 14  && TotalHour.TotalMinutes / 60 < 24)
                        {
                            var ProjectsWithEmployeeHoursOver14 = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                   join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                                                   where d.PERSON_ID == TotalHour.PERSON_ID && d.DAILY_ACTIVITY_HEADER.DA_DATE == TotalHour.DA_DATE
                                                                   select new EmployeeData{HEADER_ID = d.HEADER_ID, EMPLOYEE_NAME = e.EMPLOYEE_NAME, DA_DATE = d.DAILY_ACTIVITY_HEADER.DA_DATE}).ToList();
                            foreach (var Project in ProjectsWithEmployeeHoursOver14)
                            {
                                OffendingProjects.Add(new EmployeeData
                                {
                                    HEADER_ID = Project.HEADER_ID,
                                    EMPLOYEE_NAME = Project.EMPLOYEE_NAME,
                                    DA_DATE = Project.DA_DATE
                                });
                            }
                        }
                    }
                }
                return OffendingProjects;

            }
        }
        
        public static List<long> employeeTimeOverlapCheck()
        {
            using (Entities _context = new Entities())
            {
                //Get List of Employees
                var PersonIdList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            select d.PERSON_ID).Distinct().ToList();

                List<long> HeaderIdList = new List<long>();

                foreach (var PersonId in PersonIdList)
                {
                    //Get Headers for that employee
                    List<DAILY_ACTIVITY_EMPLOYEE> EmployeeHeaderList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                 orderby d.TIME_IN ascending
                                                                 where d.PERSON_ID == PersonId
                                                                 select d).ToList<DAILY_ACTIVITY_EMPLOYEE>();
                    int count = 0;
                    DateTime PreviousTimeIn = DateTime.Parse("1/11/1955");
                    DateTime PreviousTimeOut = DateTime.Parse("1/11/1955");
                    long PreviousHeaderId = 0;
                    foreach (DAILY_ACTIVITY_EMPLOYEE Header in EmployeeHeaderList)
                    {
                        DateTime CurrentTimeIn = (DateTime) Header.TIME_IN;
                        DateTime CurrentTimeOut = (DateTime) Header.TIME_OUT;

                        if (count > 0)
                        {
                            if (CurrentTimeIn < PreviousTimeOut)
                            {
                                HeaderIdList.Add(PreviousHeaderId);
                                HeaderIdList.Add(Header.DAILY_ACTIVITY_HEADER.HEADER_ID);
                            }
                        }
                        PreviousHeaderId = Header.HEADER_ID;
                        PreviousTimeIn = CurrentTimeIn;
                        PreviousTimeOut = CurrentTimeOut;
                        count++;
                    }
                }
                return HeaderIdList;
            }
        }

        public static List<long> headerBusinessUnitCheck()
        {
            using (Entities _context = new Entities())
            {
                List<long> OffendingHeaders = new List<long>();
                List<DAILY_ACTIVITY_HEADER> HeaderList = (from d in _context.DAILY_ACTIVITY_HEADER
                                                          where d.STATUS != 5
                                                          select d).ToList<DAILY_ACTIVITY_HEADER>();

                foreach (DAILY_ACTIVITY_HEADER Header in HeaderList)
                {
                    long ProjectId = (long)Header.PROJECT_ID;
                    long? ProjectOrgId = (from d in _context.PROJECTS_V
                                          where d.PROJECT_ID == ProjectId
                                          select d.ORG_ID).Single<long?>();
                    bool BreakLoop = false;

                    //foreach (DAILY_ACTIVITY_EMPLOYEE Employee in Header.DAILY_ACTIVITY_EMPLOYEE)
                    //{
                    //    long? EmployeeBusinessUnit = (from e in _context.EMPLOYEES_V
                    //                                  where e.PERSON_ID == Employee.PERSON_ID
                    //                                  select e.ORGANIZATION_ID).Single();
                    //    if (EmployeeBusinessUnit != ProjectOrgId)
                    //    {
                    //        OffendingHeaders.Add(Header.HEADER_ID);
                    //        BreakLoop = true;
                    //        break;
                    //    }

                    //}
                    foreach (DAILY_ACTIVITY_EQUIPMENT Equipment in Header.DAILY_ACTIVITY_EQUIPMENT)
                    {
                        if (!BreakLoop)
                        {
                            long? EquipmentBusinessUnit = (from p in _context.PROJECTS_V
                                                           where p.PROJECT_ID == Equipment.PROJECT_ID
                                                           select p.ORG_ID).Single();
                            if (EquipmentBusinessUnit != ProjectOrgId)
                            {
                                OffendingHeaders.Add(Header.HEADER_ID);
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return OffendingHeaders;
            }
        }
    }

    public class EmployeeData
    {
        public long HEADER_ID { get; set; }
        public string EMPLOYEE_NAME {get; set;}
        public DateTime? DA_DATE {get; set;}
    }

    public class HeaderData
    {
        public long HEADER_ID { get; set; }
        public long PROJECT_ID { get; set; }
        public DateTime DA_DATE { get; set; }
        public string SEGMENT1 { get; set; }
        public string LONG_NAME { get; set; }
        public string STATUS_VALUE { get; set; }
        public string WARNING { get; set; }
    }
}