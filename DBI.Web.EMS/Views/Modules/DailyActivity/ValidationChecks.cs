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
                                      where d.DAILY_ACTIVITY_HEADER.STATUS != 4 && d.DAILY_ACTIVITY_HEADER.STATUS != 5
                                      group d by new { d.DAILY_ACTIVITY_HEADER.DA_DATE, d.PERSON_ID } into g
                                      select new { g.Key.PERSON_ID, g.Key.DA_DATE, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value)) }).ToList();

                List<EmployeeData> OffendingProjects = new List<EmployeeData>();
                foreach (var TotalHour in TotalHoursList)
                {

                    if (TotalHour.TotalMinutes / 60 >= Hours)
                    {
                        var ProjectsWithEmployeeHoursOver24 = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                               join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                                               where d.PERSON_ID == TotalHour.PERSON_ID && d.DAILY_ACTIVITY_HEADER.DA_DATE == TotalHour.DA_DATE && d.DAILY_ACTIVITY_HEADER.STATUS != 5
                                                               select new EmployeeData { HEADER_ID = d.HEADER_ID, EMPLOYEE_NAME = e.EMPLOYEE_NAME, DA_DATE = d.DAILY_ACTIVITY_HEADER.DA_DATE }).ToList();
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

        public static WarningData checkEmployeeTime(int Hours, long PersonId, DateTime HeaderDate)
        {
            using (Entities _context = new Entities())
            {
                var TotalMinutes = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                    where d.DAILY_ACTIVITY_HEADER.STATUS != 4 && d.DAILY_ACTIVITY_HEADER.STATUS != 5 && d.PERSON_ID == PersonId && EntityFunctions.TruncateTime(d.DAILY_ACTIVITY_HEADER.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate)
                                    group d by new { d.PERSON_ID } into g
                                    select g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value))).SingleOrDefault();
                if (TotalMinutes / 60 > Hours)
                {
                    string Name = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == PersonId).Select(x => x.EMPLOYEE_NAME).Single();
                    if (Hours == 14)
                    {
                        return new WarningData { WarningType = "Warning", RecordType = Name, AdditionalInformation = string.Format("More than {0} hours logged on {1}", Hours.ToString(), HeaderDate.ToString("MM-dd-yyyy")) };
                    }
                    else if (Hours == 24)
                    {
                        return new WarningData { WarningType = "Error", RecordType = Name, AdditionalInformation = string.Format("More than {0} hours logged on {1}", Hours.ToString(), HeaderDate.ToString("MM-dd-yyyy")) };
                    }
                }
                return null;
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

        public static WarningData MeterCheck(long EquipmentID)
        {
            using (Entities _context = new Entities())
            {
                DAILY_ACTIVITY_EQUIPMENT EquipmentRecord = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                                                            where d.EQUIPMENT_ID == EquipmentID
                                                            select d).Single();
                if (EquipmentRecord.ODOMETER_START == 0 || EquipmentRecord.ODOMETER_END == 0)
                {
                    string Name = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                                   join p in _context.PROJECTS_V on e.PROJECT_ID equals p.PROJECT_ID
                                   where e.EQUIPMENT_ID == EquipmentID
                                   select p.NAME).Single();
                    string ClassCode = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                                        join c in _context.CLASS_CODES_V on e.PROJECT_ID equals c.PROJECT_ID
                                        where e.EQUIPMENT_ID == EquipmentID
                                        select c.CLASS_CODE).Single();
                    return new WarningData { WarningType = "Warning", RecordType = string.Format("{0} - {1}", Name, ClassCode), AdditionalInformation = "Equipment Missing Meter Reading" };
                }
                else
                {
                    return null;
                }
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
                        DateTime CurrentTimeIn = (DateTime)Header.TIME_IN;
                        DateTime CurrentTimeOut = (DateTime)Header.TIME_OUT;

                        if (count > 0)
                        {
                            if (CurrentTimeIn <= PreviousTimeOut)
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

        public static bool employeeTimeOverlapCheck(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<DAILY_ACTIVITY_EMPLOYEE> EmployeeList = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.HEADER_ID == HeaderId).ToList();
                foreach (DAILY_ACTIVITY_EMPLOYEE Person in EmployeeList)
                {
                    DateTime HeaderDate = (DateTime)Person.DAILY_ACTIVITY_HEADER.DA_DATE;
                    List<DAILY_ACTIVITY_EMPLOYEE> EmployeeData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                  join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                                  orderby d.TIME_IN ascending
                                                                  where d.PERSON_ID == Person.PERSON_ID && EntityFunctions.TruncateTime(Person.TIME_IN) == EntityFunctions.TruncateTime(HeaderDate) && h.STATUS != 5 && h.DA_DATE == HeaderDate
                                                                  select d).ToList();
                    DateTime PreviousTimeIn = DateTime.Parse("1/11/1955");
                    DateTime PreviousTimeOut = DateTime.Parse("1/11/1955");
                    long PreviousHeader = 0;
                    int count = 0;
                    foreach (DAILY_ACTIVITY_EMPLOYEE Employee in EmployeeData)
                    {
                        DateTime CurrentTimeIn = (DateTime)Employee.TIME_IN;
                        DateTime CurrentTimeOut = (DateTime)Employee.TIME_OUT;
                        if (count > 0)
                        {

                            if (CurrentTimeIn < PreviousTimeOut)
                            {
                                return true;
                            }
                        }
                        PreviousTimeIn = CurrentTimeIn;
                        PreviousTimeOut = CurrentTimeOut;
                        PreviousHeader = Employee.HEADER_ID;
                        count++;
                    }
                }
                return false;
            }
        }
        public static List<WarningData> employeeTimeOverlapCheck(long PersonId, DateTime HeaderDate, long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<DAILY_ACTIVITY_EMPLOYEE> EmployeeData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                              join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                              orderby d.TIME_IN ascending
                                                              where d.PERSON_ID == PersonId && EntityFunctions.TruncateTime(d.TIME_IN) == EntityFunctions.TruncateTime(HeaderDate.Date) && h.STATUS != 5
                                                              select d).ToList();
                List<WarningData> HeaderIdList = new List<WarningData>();
                DateTime PreviousTimeIn = DateTime.Parse("1/11/1955");
                DateTime PreviousTimeOut = DateTime.Parse("1/11/1955");
                long PreviousHeader = 0;
                int count = 0;
                foreach (DAILY_ACTIVITY_EMPLOYEE Employee in EmployeeData)
                {
                    DateTime CurrentTimeIn = (DateTime)Employee.TIME_IN;
                    DateTime CurrentTimeOut = (DateTime)Employee.TIME_OUT;
                    if (count > 0)
                    {

                        if (CurrentTimeIn < PreviousTimeOut)
                        {
                            string Name = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == PersonId).Select(x => x.EMPLOYEE_NAME).Single();
                            if (PreviousHeader == HeaderId)
                            {
                                HeaderIdList.Add(new WarningData { WarningType = "Error", RecordType = Name, AdditionalInformation = string.Format("Employee has time overlap on DRS Id:{0}", Employee.HEADER_ID.ToString()) });
                            }
                            else
                            {
                                HeaderIdList.Add(new WarningData { WarningType = "Error", RecordType = Name, AdditionalInformation = string.Format("Employee has time overlap on DRS Id:{0}", PreviousHeader.ToString()) });
                            }
                        }
                    }
                    PreviousTimeIn = CurrentTimeIn;
                    PreviousTimeOut = CurrentTimeOut;
                    PreviousHeader = Employee.HEADER_ID;
                    count++;
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

        public static bool EquipmentBusinessUnitCheck(long HeaderId, string type)
        {
            using (Entities _context = new Entities())
            {
                var HeaderInfo = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.HEADER_ID == HeaderId
                                  select new { d.HEADER_ID, d.PROJECT_ID, d.DAILY_ACTIVITY_EQUIPMENT }).Single();

                long ProjectId = (long)HeaderInfo.PROJECT_ID;
                long? ProjectOrgId = (from d in _context.PROJECTS_V
                                      where d.PROJECT_ID == ProjectId
                                      select d.ORG_ID).Single<long?>();
                foreach (DAILY_ACTIVITY_EQUIPMENT Equipment in HeaderInfo.DAILY_ACTIVITY_EQUIPMENT)
                {
                    long? EquipmentBusinessUnit = (from p in _context.PROJECTS_V
                                                   where p.PROJECT_ID == Equipment.PROJECT_ID
                                                   select p.ORG_ID).Single();
                    if (EquipmentBusinessUnit != ProjectOrgId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        public static WarningData EquipmentBusinessUnitCheck(long EquipmentId)
        {
            using (Entities _context = new Entities())
            {
                DAILY_ACTIVITY_EQUIPMENT EquipmentData = _context.DAILY_ACTIVITY_EQUIPMENT.Where(x => x.EQUIPMENT_ID == EquipmentId).Single();

                long ProjectOrgId = _context.PROJECTS_V.Where(x => x.PROJECT_ID == EquipmentData.DAILY_ACTIVITY_HEADER.PROJECT_ID).Select(x => (long)x.ORG_ID).Single();
                long EquipmentOrgId = _context.PROJECTS_V.Where(x => x.PROJECT_ID == EquipmentData.PROJECT_ID).Select(x => (long)x.ORG_ID).Single();
                if (ProjectOrgId != EquipmentOrgId)
                {
                    string ProjectOrg = (from p in _context.PROJECTS_V
                                         join o in _context.ORG_HIER_V on p.CARRYING_OUT_ORGANIZATION_ID equals o.ORG_ID
                                         where p.PROJECT_ID == EquipmentData.DAILY_ACTIVITY_HEADER.PROJECT_ID
                                         select o.PARENT_ORG).Distinct().Single();
                    string EquipmentOrg = (from p in _context.PROJECTS_V
                                           join o in _context.ORG_HIER_V on p.CARRYING_OUT_ORGANIZATION_ID equals o.ORG_ID
                                           where p.PROJECT_ID == EquipmentData.PROJECT_ID
                                           select o.PARENT_ORG).Distinct().Single();
                    string Name = _context.PROJECTS_V.Where(x => x.PROJECT_ID == EquipmentData.PROJECT_ID).Select(x => x.NAME).Single();
                    string ClassCode = _context.CLASS_CODES_V.Where(x => x.PROJECT_ID == EquipmentData.PROJECT_ID).Select(x => x.CLASS_CODE).Single();


                    return new WarningData { WarningType = "Warning", RecordType = string.Format("{0} - {1}", Name, ClassCode), AdditionalInformation = string.Format("Equipment BU is {0}, Project BU is {1}", EquipmentOrg, ProjectOrg) };
                }
                return null;
            }
        }

        public static List<long> EmployeeBusinessUnitCheck()
        {
            using (Entities _context = new Entities())
            {
                List<long> OffendingHeaders = new List<long>();

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

        public static bool EmployeeBusinessUnitCheck(long HeaderId, string Type)
        {
            using (Entities _context = new Entities())
            {
                var HeaderInfo = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.HEADER_ID == HeaderId
                                  select new { d.HEADER_ID, d.PROJECT_ID, d.DAILY_ACTIVITY_EMPLOYEE }).Single();
                long ProjectId = (long)HeaderInfo.PROJECT_ID;
                long? ProjectOrgId = (from d in _context.PROJECTS_V
                                      where d.PROJECT_ID == ProjectId
                                      select d.ORG_ID).Single<long?>();
                foreach (DAILY_ACTIVITY_EMPLOYEE Employee in HeaderInfo.DAILY_ACTIVITY_EMPLOYEE)
                {
                    long? EmployeeOrgId = (from e in _context.EMPLOYEES_V
                                           where e.PERSON_ID == Employee.PERSON_ID
                                           select e.ORGANIZATION_ID).Single();
                    long EmployeeBusinessUnit = EMPLOYEES_V.GetEmployeeBusinessUnit((long)EmployeeOrgId);
                    if (EmployeeBusinessUnit != ProjectOrgId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }

        }
        public static WarningData EmployeeBusinessUnitCheck(long EmployeeId)
        {
            using (Entities _context = new Entities())
            {
                DAILY_ACTIVITY_EMPLOYEE Employee = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmployeeId).Single();
                long ProjectOrgID = _context.PROJECTS_V.Where(x => x.PROJECT_ID == Employee.DAILY_ACTIVITY_HEADER.PROJECT_ID).Select(x => (long)x.ORG_ID).Single();
                long EmployeeOrgID = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == Employee.PERSON_ID).Select(x => (long)x.ORGANIZATION_ID).Single();

                if (ProjectOrgID != EMPLOYEES_V.GetEmployeeBusinessUnit(EmployeeOrgID))
                {
                    string ProjectOrg = (from p in _context.PROJECTS_V
                                         join o in _context.ORG_HIER_V on p.CARRYING_OUT_ORGANIZATION_ID equals o.ORG_ID
                                         where p.PROJECT_ID == Employee.DAILY_ACTIVITY_HEADER.PROJECT_ID
                                         select o.PARENT_ORG).Distinct().Single();
                    string EmployeeOrg = (from e in _context.EMPLOYEES_V
                                          join o in _context.ORG_HIER_V on e.ORGANIZATION_ID equals o.ORG_ID
                                          where e.ORGANIZATION_ID == EmployeeOrgID
                                          select o.PARENT_ORG).Distinct().Single();


                    string Name = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == Employee.PERSON_ID).Select(x => x.EMPLOYEE_NAME).Single();
                    return new WarningData { WarningType = "Warning", RecordType = Name, AdditionalInformation = string.Format("Employee BU is {0}, Project BU is {1}", EmployeeOrg, ProjectOrg) };
                }
                return null;
            }
        }

        public static WarningData checkPerDiem(long HeaderId)
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
                                                      where h.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.PER_DIEM == "Y" && h.STATUS != 5
                                                      select new HeaderDetails { HEADER_ID = d.HEADER_ID, LONG_NAME = p.LONG_NAME, PERSON_ID = d.PERSON_ID }).ToList();

                    if (HeaderList.Count > 1)
                    {
                        return new WarningData { WarningType = "Error", RecordType = "Per Diem", AdditionalInformation = "An employee has a duplicate per diem" };
                    }

                }
                return null;
            }
        }

        public static List<WarningData> checkPerDiem(long EmployeeId, long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                //Get Person Id
                DAILY_ACTIVITY_EMPLOYEE EmployeeInfo = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmployeeId).Single();
                List<DAILY_ACTIVITY_EMPLOYEE> HeaderList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                            where d.PERSON_ID == EmployeeInfo.PERSON_ID && h.DA_DATE == EmployeeInfo.DAILY_ACTIVITY_HEADER.DA_DATE && d.PER_DIEM == "Y" && h.STATUS != 5
                                                            select d).ToList();
                List<WarningData> Warnings = new List<WarningData>();
                if (HeaderList.Count > 1)
                {
                    foreach (DAILY_ACTIVITY_EMPLOYEE Header in HeaderList)
                    {
                        string EmployeeName = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == Header.PERSON_ID).Select(x => x.EMPLOYEE_NAME).Single();
                        Warnings.Add(new WarningData
                        {
                            WarningType = "Error",
                            RecordType = "Per Diem",
                            AdditionalInformation = string.Format("{0} has an overlapping per diem on DRS# {1}", EmployeeName, Header.HEADER_ID.ToString())
                        });
                    }
                }
                return Warnings;
            }
        }

        public static List<WarningData> LunchCheck(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<WarningData> WarningList = new List<WarningData>();
                var EmployeeList = (from e in _context.DAILY_ACTIVITY_EMPLOYEE
                                    join h in _context.DAILY_ACTIVITY_HEADER on e.HEADER_ID equals h.HEADER_ID
                                    join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                    join em in _context.EMPLOYEES_V on e.PERSON_ID equals em.PERSON_ID
                                    where e.HEADER_ID == HeaderId
                                    select new {e.EMPLOYEE_ID, e.PERSON_ID, em.EMPLOYEE_NAME, h.DA_DATE, e.TRAVEL_TIME, e.DRIVE_TIME, p.ORG_ID}).ToList();
                foreach (var Employee in EmployeeList)
                {
                    var TotalMinutes = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                        join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                        where EntityFunctions.TruncateTime(h.DA_DATE) == EntityFunctions.TruncateTime(Employee.DA_DATE) && d.PERSON_ID == Employee.PERSON_ID && h.STATUS != 5
                                        group d by new { d.PERSON_ID } into g
                                        select new { g.Key.PERSON_ID, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value)), TravelTime = g.Sum(d => d.TRAVEL_TIME), DriveTime = g.Sum(d => d.DRIVE_TIME) }).SingleOrDefault();
                    
                    if (Employee.ORG_ID == 121 && TotalMinutes != null) 
                    {
                        decimal TotalTime = (decimal)TotalMinutes.TotalMinutes;
                        try
                        {
                            decimal TravelDeduct = (decimal)((TotalMinutes.TravelTime == null ?0:TotalMinutes.TravelTime * 60));
                            TotalTime = TotalTime - (decimal)(TotalMinutes.TravelTime == null ?0:TotalMinutes.TravelTime * 60) - (decimal)(TotalMinutes.DriveTime == null ?0:TotalMinutes.DriveTime * 60);
                        }
                        catch { }
                        if (TotalTime >= 308)
                        {
                            var LoggedLunches = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                 join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                 where h.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.LUNCH == "Y"
                                                 select d.LUNCH).Count();
                            if (LoggedLunches == 0)
                            {
                                WarningList.Add(new WarningData
                                {
                                    WarningType = "Error",
                                    RecordType = "Lunch Check",
                                    AdditionalInformation = string.Format("{0} has no lunch entered on {1}", Employee.EMPLOYEE_NAME, Employee.DA_DATE)
                                });
                            }
                            else if(LoggedLunches == 1)
                            {
                                var LunchLength = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                   where h.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.LUNCH == "Y"
                                                   select d.LUNCH_LENGTH).Single();
                                if (TotalTime >= 308 && TotalTime < 728)
                                {
                                    if (LunchLength == 60)
                                    {
                                        WarningList.Add(new WarningData
                                        {
                                            WarningType = "Error",
                                            RecordType = "Lunch Length",
                                            AdditionalInformation = string.Format("{0} has an incorrect lunch length", Employee.EMPLOYEE_NAME)
                                        });
                                    }
                                }
                                else
                                {
                                    if (LunchLength == 30)
                                    {
                                        WarningList.Add(new WarningData
                                        {
                                            WarningType = "Error",
                                            RecordType = "Lunch Length",
                                            AdditionalInformation = string.Format("{0} has an incorrect lunch length", Employee.EMPLOYEE_NAME)
                                        });
                                    }
                                }
                            }

                        }
                    }

                }
                return WarningList;
            }
        }

        public static WarningData LunchCheck(long PersonId, DateTime HeaderDate)
        {
            using (Entities _context = new Entities())
            {
                var TotalMinutes = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                    join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                    where EntityFunctions.TruncateTime(h.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate) && d.PERSON_ID == PersonId && h.STATUS != 5
                                    group d by new { d.PERSON_ID } into g
                                    select new { g.Key.PERSON_ID, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value)), TravelTime = g.Sum(d => d.TRAVEL_TIME), DriveTime = g.Sum(d => d.DRIVE_TIME) }).Single();
                var Employee = (from e in _context.EMPLOYEES_V
                                where e.PERSON_ID == PersonId
                                select e.EMPLOYEE_NAME).Single();
                decimal TotalTime = (decimal)TotalMinutes.TotalMinutes;
                try
                {
                    TotalTime = TotalTime - ((decimal)TotalMinutes.TravelTime * 60) - ((decimal)TotalMinutes.DriveTime * 60);
                }
                catch { }
                if (TotalTime >= 308)
                {
                    var LoggedLunches = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                         join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                         where EntityFunctions.TruncateTime(h.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate) && d.PERSON_ID == PersonId && d.LUNCH == "Y"
                                         select d.LUNCH).Count();
                    if (LoggedLunches == 0)
                    {
                        
                        return new WarningData
                        {
                            WarningType = "Error",
                            RecordType = "Lunch Check",
                            AdditionalInformation = string.Format("{0} has no lunch entered on {1}", Employee, HeaderDate.ToString("MM-dd-yyyy"))
                        };
                    }
                    else if(LoggedLunches == 1)
                    {
                        var LunchLength = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                           join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                           where EntityFunctions.TruncateTime(h.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate) && d.PERSON_ID == PersonId && d.LUNCH == "Y"
                                           select d.LUNCH_LENGTH).Single();
                        if (TotalTime >= 308 && TotalTime < 728)
                        {
                            if (LunchLength == 60)
                            {
                                return new WarningData
                                {
                                    WarningType = "Error",
                                    RecordType = "Lunch Length",
                                    AdditionalInformation = string.Format("{0} has an incorrect lunch length", Employee)
                                };
                            }
                        }
                        else
                        {
                            if (LunchLength == 30)
                            {
                                return new WarningData
                                {
                                    WarningType = "Error",
                                    RecordType = "Lunch Length",
                                    AdditionalInformation = string.Format("{0} has an incorrect lunch length", Employee)
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }
        //public static List<EmployeeData> LunchCheck(long HeaderId)
        //{
        //    using (Entities _context = new Entities())
        //    {
        //        List<EmployeeData> EmployeeList = new List<EmployeeData>();
        //        var HeaderEmployees = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
        //                               join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
        //                               join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
        //                               join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
        //                               where d.HEADER_ID == HeaderId && h.STATUS != 5
        //                               select new { d.PERSON_ID, e.EMPLOYEE_NAME, d.DAILY_ACTIVITY_HEADER.DA_DATE, d.TRAVEL_TIME, d.DRIVE_TIME, p.ORG_ID }).ToList();
        //        foreach (var Employee in HeaderEmployees)
        //        {
        //            var TotalMinutes = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
        //                                join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
        //                                where d.DAILY_ACTIVITY_HEADER.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && h.STATUS != 5
        //                                group d by new { d.PERSON_ID } into g
        //                                select new { g.Key.PERSON_ID, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value)) }).Single();
        //            decimal totalTime = (decimal)TotalMinutes.TotalMinutes;
        //            if (Employee.ORG_ID == 121)
        //            {
        //                try
        //                {
        //                    totalTime = totalTime - ((decimal)Employee.TRAVEL_TIME * 60) - ((decimal)Employee.DRIVE_TIME * 60);
        //                }
        //                catch (Exception e)
        //                {

        //                }

        //                if (totalTime >= 308 && totalTime < 728)
        //                {
        //                    var LoggedLunches = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
        //                                         where d.DAILY_ACTIVITY_HEADER.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.LUNCH == "Y"
        //                                         select d.LUNCH).Count();
        //                    if (LoggedLunches == 0)
        //                    {
        //                        EmployeeList.Add(new EmployeeData
        //                        {
        //                            PERSON_ID = Employee.PERSON_ID,
        //                            EMPLOYEE_NAME = Employee.EMPLOYEE_NAME,
        //                            LUNCH_LENGTH = 30,
        //                            DA_DATE = Employee.DA_DATE
        //                        });
        //                    }
        //                }
        //                else if (totalTime >= 728)
        //                {
        //                    var LoggedLunches = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
        //                                         where d.DAILY_ACTIVITY_HEADER.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID && d.LUNCH == "Y" && d.LUNCH_LENGTH == 60
        //                                         select d.LUNCH).Count();
        //                    if (LoggedLunches == 0)
        //                    {
        //                        EmployeeList.Add(new EmployeeData
        //                        {
        //                            PERSON_ID = Employee.PERSON_ID,
        //                            LUNCH_LENGTH = 60,
        //                            EMPLOYEE_NAME = Employee.EMPLOYEE_NAME,
        //                            DA_DATE = Employee.DA_DATE
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //        return EmployeeList;
        //    }
        //}
    }

    public class EmployeeData
    {
        public long HEADER_ID { get; set; }
        public string LONG_NAME { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public DateTime? DA_DATE { get; set; }
        public long PERSON_ID { get; set; }
        public int LUNCH_LENGTH { get; set; }
    }

    public class HeaderData
    {
        public long HEADER_ID { get; set; }
        public long? PROJECT_ID { get; set; }
        public DateTime? DA_DATE { get; set; }
        public string SEGMENT1 { get; set; }
        public string LONG_NAME { get; set; }
        public decimal? DA_HEADER_ID { get; set; }
        public string STATUS_VALUE { get; set; }
        public string WARNING { get; set; }
        public string WARNING_TYPE { get; set; }
        public int? STATUS { get; set; }
        public long? ORG_ID { get; set; }
    }

    public class WarningData
    {
        public string WarningType { get; set; }
        public string RecordType { get; set; }
        public string AdditionalInformation { get; set; }
    }
}