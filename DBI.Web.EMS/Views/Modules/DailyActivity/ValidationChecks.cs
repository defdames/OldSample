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
        public static List<EmployeeData> checkEmployeeTime(int Hours)
        {

            using (Entities _context = new Entities())
            {
                var TotalHoursList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                      where d.DAILY_ACTIVITY_HEADER.STATUS != 4 || d.DAILY_ACTIVITY_HEADER.STATUS != 5
                                     group d by new {d.DAILY_ACTIVITY_HEADER.DA_DATE, d.PERSON_ID } into g
                                     select new { g.Key.PERSON_ID, g.Key.DA_DATE, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value))}).ToList();

                List<EmployeeData> OffendingProjects = new List<EmployeeData>();
                foreach (var TotalHour in TotalHoursList)
                {

                    if (TotalHour.TotalMinutes / 60 >= Hours)
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
                return OffendingProjects;

            }
        }

        /// <summary>
        /// Validation check to see if equipment meter readings are missing
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        public static bool AreMetersMissing(long HeaderId)
        {
            //Get List of equipment
            List<DAILY_ACTIVITY_EQUIPMENT> EquipmentList;

            using (Entities _context = new Entities())
            {
                EquipmentList = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                                 where d.HEADER_ID == HeaderId
                                 select d).ToList();
            }
            int NumberOfMissingMeters = 0;
            foreach (DAILY_ACTIVITY_EQUIPMENT Equipment in EquipmentList)
            {
                if (Equipment.ODOMETER_START == null || Equipment.ODOMETER_END == null)
                {
                    NumberOfMissingMeters++;
                }
            }

            if (NumberOfMissingMeters > 0)
            {
                return true;
            }
            return false;
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
                                                                        join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                                 orderby d.TIME_IN ascending
                                                                 where d.PERSON_ID == PersonId && h.STATUS != 5
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

        public static List<long> EquipmentBusinessUnitCheck()
        {
            List<long> OffendingHeaders = new List<long>();
            using (Entities _context = new Entities())
            {

                var HeaderList = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.STATUS != 5
                                  select new { d.HEADER_ID, d.PROJECT_ID, d.DAILY_ACTIVITY_EQUIPMENT }).ToList();

                foreach (var Header in HeaderList)
                {
                    long ProjectId = (long)Header.PROJECT_ID;
                    long? ProjectOrgId = (from d in _context.PROJECTS_V
                                          where d.PROJECT_ID == ProjectId
                                          select d.ORG_ID).Single<long?>();

                    foreach (DAILY_ACTIVITY_EQUIPMENT Equipment in Header.DAILY_ACTIVITY_EQUIPMENT)
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
                }
                return OffendingHeaders;
            }
        }

        public static List<long> EmployeeBusinessUnitCheck(){
            using (Entities _context = new Entities()){
                List<long>OffendingHeaders = new List<long>();

                var HeaderList = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.STATUS != 5
                                  select new { d.HEADER_ID, d.PROJECT_ID, d.DAILY_ACTIVITY_EMPLOYEE }).ToList();
                foreach (var Header in HeaderList)
                {
                    long ProjectId = (long)Header.PROJECT_ID;
                    long? ProjectOrgId = (from d in _context.PROJECTS_V
                                          where d.PROJECT_ID == ProjectId
                                          select d.ORG_ID).Single<long?>();
                    foreach (DAILY_ACTIVITY_EMPLOYEE Employee in Header.DAILY_ACTIVITY_EMPLOYEE)
                    {
                        long? EmployeeOrgId = (from e in _context.EMPLOYEES_V
                                                where e.PERSON_ID == Employee.PERSON_ID
                                                select e.ORGANIZATION_ID).Single();
                        long EmployeeBusinessUnit = EMPLOYEES_V.GetEmployeeBusinessUnit((long)EmployeeOrgId);
                        if (EmployeeBusinessUnit != ProjectOrgId)
                        {
                            OffendingHeaders.Add(Header.HEADER_ID);
                            break;
                        }
                    }
                }
                return OffendingHeaders;
            }
        }

        public static EmployeeData checkPerDiem(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                //Get List of Employees for this header with Per-Diem active
                var EmployeeList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                    join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                    where d.HEADER_ID == HeaderId && d.PER_DIEM == "Y"
                                    select new { d.PERSON_ID, d.DAILY_ACTIVITY_HEADER.DA_DATE, e.EMPLOYEE_NAME }).ToList();

                EmployeeData BadHeaders = null;
                //Check for Additional active PerDiems on that day
                foreach (var Employee in EmployeeList)
                {
                    List<HeaderDetails> HeaderList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                      join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                      join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                      where h.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.PER_DIEM == "Y" && h.STATUS !=5
                                      select new HeaderDetails { HEADER_ID = d.HEADER_ID, LONG_NAME = p.LONG_NAME, PERSON_ID = d.PERSON_ID }).ToList();

                    if (HeaderList.Count > 1)
                    {
                        HeaderDetails ThisHeader = HeaderList.Find(h => h.HEADER_ID == HeaderId);
                        BadHeaders = new EmployeeData
                        {
                            HEADER_ID = HeaderId,
                            EMPLOYEE_NAME = Employee.EMPLOYEE_NAME,
                            LONG_NAME = ThisHeader.LONG_NAME,
                            DA_DATE = Employee.DA_DATE,
                            PERSON_ID = ThisHeader.PERSON_ID
                        };

                    }

                }
                return BadHeaders;
            }
        }

        public static List<EmployeeData> LunchCheck(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<EmployeeData> EmployeeList = new List<EmployeeData>();
                var HeaderEmployees = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                       join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                       join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                       join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                       where d.HEADER_ID == HeaderId && h.STATUS != 5
                                       select new {d.PERSON_ID, e.EMPLOYEE_NAME, d.DAILY_ACTIVITY_HEADER.DA_DATE, d.TRAVEL_TIME, d.DRIVE_TIME, p.ORG_ID}).ToList();
                foreach (var Employee in HeaderEmployees)
                {
                    var TotalMinutes = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                        join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID    
                                        where d.DAILY_ACTIVITY_HEADER.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && h.STATUS != 5
                                            group d by new {d.PERSON_ID} into g
                                            select new{g.Key.PERSON_ID, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value))}).Single();
                    decimal totalTime = (decimal)TotalMinutes.TotalMinutes;
                    if (Employee.ORG_ID == 121)
                    {
                        try
                        {
                            totalTime = totalTime - ((decimal)Employee.TRAVEL_TIME * 60) -((decimal)Employee.DRIVE_TIME * 60);
                        }
                        catch(Exception e){
                            
                        }
                        
                        if (totalTime >= 308 && totalTime < 728)
                        {
                            var LoggedLunches = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                 where d.DAILY_ACTIVITY_HEADER.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.LUNCH == "Y"
                                                 select d.LUNCH).Count();
                            if (LoggedLunches == 0)
                            {
                                EmployeeList.Add(new EmployeeData
                                {
                                    PERSON_ID = Employee.PERSON_ID,
                                    EMPLOYEE_NAME = Employee.EMPLOYEE_NAME,
                                    LUNCH_LENGTH = 30,
                                    DA_DATE = Employee.DA_DATE
                                });
                            }
                        }
                        else if (totalTime >= 728)
                        {
                            var LoggedLunches = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                 where d.DAILY_ACTIVITY_HEADER.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.LUNCH == "Y" && d.LUNCH_LENGTH == 60
                                                 select d.LUNCH).Count();
                            if (LoggedLunches == 0)
                            {
                                EmployeeList.Add(new EmployeeData
                                {
                                    PERSON_ID = Employee.PERSON_ID,
                                    LUNCH_LENGTH = 60,
                                    EMPLOYEE_NAME = Employee.EMPLOYEE_NAME,
                                    DA_DATE = Employee.DA_DATE
                                });
                            }
                        }
                    }
                }
                return EmployeeList;
            }
        }
    }

    public class EmployeeData
    {
        public long HEADER_ID { get; set; }
        public string LONG_NAME { get; set; }
        public string EMPLOYEE_NAME {get; set;}
        public DateTime? DA_DATE {get; set;}
        public long PERSON_ID { get; set; }
        public int LUNCH_LENGTH { get; set; }
    }

    public class HeaderData
    {
        public long HEADER_ID { get; set; }
        public long PROJECT_ID { get; set; }
        public DateTime DA_DATE { get; set; }
        public string SEGMENT1 { get; set; }
        public string LONG_NAME { get; set; }
        public decimal DA_HEADER_ID { get; set; }
        public string STATUS_VALUE { get; set; }
        public string WARNING { get; set; }
        public string WARNING_TYPE { get; set; }
        public int STATUS { get; set; }
    }
}