using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class DAILY_ACTIVITY
    {
        public static IQueryable<HeaderData> GetHeaders(Entities _context, bool ShowInactive, bool ShowPosted, List<long> OrgsList = null, long? PersonId = null)
        {
            if (OrgsList != null)
            {
                if (ShowInactive && ShowPosted)
                {
                    return (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.HEADER_ID equals eq.HEADER_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join pr in _context.PROJECTS_V on equip.PROJECT_ID equals pr.PROJECT_ID into pro
                            from proj in pro.DefaultIfEmpty()
                            join em in _context.DAILY_ACTIVITY_EMPLOYEE on d.HEADER_ID equals em.HEADER_ID into emp
                            from empl in emp.DefaultIfEmpty()
                            join empv in _context.EMPLOYEES_V on empl.PERSON_ID equals empv.PERSON_ID into emplv
                            from withempl in emplv.DefaultIfEmpty()
                            where OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID)
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = d.PROJECT_ID, DA_DATE = d.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = d.DA_HEADER_ID, STATUS = d.STATUS, ORG_ID = p.ORG_ID }).Distinct();
                }
                else if (ShowInactive)
                {
                    return (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.HEADER_ID equals eq.HEADER_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join pr in _context.PROJECTS_V on equip.PROJECT_ID equals pr.PROJECT_ID into pro
                            from proj in pro.DefaultIfEmpty()
                            join em in _context.DAILY_ACTIVITY_EMPLOYEE on d.HEADER_ID equals em.HEADER_ID into emp
                            from empl in emp.DefaultIfEmpty()
                            join empv in _context.EMPLOYEES_V on empl.PERSON_ID equals empv.PERSON_ID into emplv
                            from withempl in emplv.DefaultIfEmpty()
                            where d.STATUS != 4 && (OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID))
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = d.PROJECT_ID, DA_DATE = d.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = d.DA_HEADER_ID, STATUS = d.STATUS, ORG_ID = p.ORG_ID }).Distinct();
                }
                else if (ShowPosted)
                {
                    return (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.HEADER_ID equals eq.HEADER_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join pr in _context.PROJECTS_V on equip.PROJECT_ID equals pr.PROJECT_ID into pro
                            from proj in pro.DefaultIfEmpty()
                            join em in _context.DAILY_ACTIVITY_EMPLOYEE on d.HEADER_ID equals em.HEADER_ID into emp
                            from empl in emp.DefaultIfEmpty()
                            join empv in _context.EMPLOYEES_V on empl.PERSON_ID equals empv.PERSON_ID into emplv
                            from withempl in emplv.DefaultIfEmpty()
                            where d.STATUS != 5 && (OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID))
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = d.PROJECT_ID, DA_DATE = d.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = d.DA_HEADER_ID, STATUS = d.STATUS, ORG_ID = p.ORG_ID }).Distinct();
                }
                else
                {
                    return (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.HEADER_ID equals eq.HEADER_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join pr in _context.PROJECTS_V on equip.PROJECT_ID equals pr.PROJECT_ID into pro
                            from proj in pro.DefaultIfEmpty()
                            join em in _context.DAILY_ACTIVITY_EMPLOYEE on d.HEADER_ID equals em.HEADER_ID into emp
                            from empl in emp.DefaultIfEmpty()
                            join empv in _context.EMPLOYEES_V on empl.PERSON_ID equals empv.PERSON_ID into emplv
                            from withempl in emplv.DefaultIfEmpty()
                            where d.STATUS != 4 && d.STATUS != 5 && (OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID))
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = d.PROJECT_ID, DA_DATE = d.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = d.DA_HEADER_ID, STATUS = d.STATUS, ORG_ID = p.ORG_ID }).Distinct();
                }
            }
            else
            {
                if (ShowInactive && ShowPosted)
                {
                    return (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                               join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                               join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                               join s in _context.DAILY_ACTIVITY_STATUS on h.STATUS equals s.STATUS
                               where d.PERSON_ID == PersonId
                               select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = h.PROJECT_ID, DA_DATE = h.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = h.DA_HEADER_ID, STATUS = h.STATUS });
                }
                else if (ShowInactive)
                {
                    return (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on h.STATUS equals s.STATUS
                            where d.PERSON_ID == PersonId && h.STATUS != 4
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = h.PROJECT_ID, DA_DATE = h.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = h.DA_HEADER_ID, STATUS = h.STATUS });
                }
                else if (ShowPosted)
                {
                    return (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on h.STATUS equals s.STATUS
                            where d.PERSON_ID == PersonId && h.STATUS != 5
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = h.PROJECT_ID, DA_DATE = h.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = h.DA_HEADER_ID, STATUS = h.STATUS });
                }
                else
                {
                    return (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on h.STATUS equals s.STATUS
                            where d.PERSON_ID == PersonId && h.STATUS != 4 && h.STATUS != 5
                            select new DAILY_ACTIVITY.HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = h.PROJECT_ID, DA_DATE = h.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, STATUS_VALUE = s.STATUS_VALUE, DA_HEADER_ID = h.DA_HEADER_ID, STATUS = h.STATUS });
                }
            }
        }

        public static IQueryable<PA_ROLES_V> GetRoles(Entities _context, long HeaderId)
        {
            return from d in _context.DAILY_ACTIVITY_HEADER
                   join p in _context.PA_ROLES_V on d.PROJECT_ID equals p.PROJECT_ID
                   where d.HEADER_ID == HeaderId
                   select p;
        }

        public static IQueryable<DAILY_ACTIVITY_HEADER> GetHeader(Entities _context, long HeaderId)
        {
            return _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId);
        }

        public static IQueryable<HeaderData> GetHeaderData(Entities _context, long HeaderId)
        {
            var data = (from d in _context.DAILY_ACTIVITY_HEADER
                        join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                        join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                        where d.HEADER_ID == HeaderId
                        select new HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = d.PROJECT_ID, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, DA_DATE = d.DA_DATE, SUBDIVISION = d.SUBDIVISION, CONTRACTOR = d.CONTRACTOR, PERSON_ID = d.PERSON_ID, EMPLOYEE_NAME = e.EMPLOYEE_NAME, LICENSE = d.LICENSE, STATE = d.STATE, APPLICATION_TYPE = d.APPLICATION_TYPE, DENSITY = d.DENSITY, DA_HEADER_ID = d.DA_HEADER_ID, STATUS = d.STATUS });
            if (data == null)
            {
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                        join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                        where d.HEADER_ID == HeaderId
                        select new HeaderData { HEADER_ID = d.HEADER_ID, PROJECT_ID = d.PROJECT_ID, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, DA_DATE = d.DA_DATE, SUBDIVISION = d.SUBDIVISION, CONTRACTOR = d.CONTRACTOR, PERSON_ID = d.PERSON_ID, LICENSE = d.LICENSE, STATE = d.STATE, APPLICATION_TYPE = d.APPLICATION_TYPE, DENSITY = d.DENSITY, DA_HEADER_ID = d.DA_HEADER_ID, STATUS = d.STATUS });
            }

            return data;
        }

        public static List<EmployeeDetails> GetDBIEmployeeData(Entities _context, long HeaderId)
        {
            List<EmployeeDetails> data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                          join em in _context.EMPLOYEES_V on d.PERSON_ID equals em.PERSON_ID
                                          join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                                          from equip in equ.DefaultIfEmpty()
                                          join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                                          from projects in proj.DefaultIfEmpty()
                                          where d.HEADER_ID == HeaderId
                                          select new EmployeeDetails { EMPLOYEE_ID = d.EMPLOYEE_ID, PERSON_ID = em.PERSON_ID, DA_DATE = d.DAILY_ACTIVITY_HEADER.DA_DATE, EMPLOYEE_NAME = em.EMPLOYEE_NAME, FOREMAN_LICENSE = d.FOREMAN_LICENSE, NAME = projects.NAME, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), PER_DIEM = (d.PER_DIEM == "Y" ? true : false), COMMENTS = d.COMMENTS, LUNCH_LENGTH = d.LUNCH_LENGTH, STATUS = d.DAILY_ACTIVITY_HEADER.STATUS, HEADER_ID = d.HEADER_ID, EQUIPMENT_ID = d.EQUIPMENT_ID }).ToList();
            foreach (var item in data)
            {
                item.PREVAILING_WAGE = RoleNeeded(_context, HeaderId);
                double Hours = Math.Truncate((double)item.TRAVEL_TIME);
                double Minutes = Math.Round(((double)item.TRAVEL_TIME - Hours) * 60);
                item.TOTAL_HOURS = (item.TIME_OUT - item.TIME_IN).ToString("hh\\:mm");
                item.TIME_IN_TIME = item.TIME_IN;
                item.TIME_OUT_TIME = item.TIME_OUT;
                TimeSpan TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                item.TRAVEL_TIME_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                Hours = Math.Truncate((double)item.DRIVE_TIME);
                Minutes = Math.Round(((double)item.DRIVE_TIME - Hours) * 60);
                TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                item.DRIVE_TIME_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
            }
            return data;
        }

        public static List<EmployeeDetails> GetIRMEmployeeData(Entities _context, long HeaderId)
        {

            var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                        join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                        join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                        from equip in equ.DefaultIfEmpty()
                        join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                        from projects in proj.DefaultIfEmpty()
                        where d.HEADER_ID == HeaderId
                        select new EmployeeDetails { PERSON_ID = e.PERSON_ID, EMPLOYEE_ID = d.EMPLOYEE_ID, DA_DATE = d.DAILY_ACTIVITY_HEADER.DA_DATE, EMPLOYEE_NAME = e.EMPLOYEE_NAME, FOREMAN_LICENSE = d.FOREMAN_LICENSE, NAME = projects.NAME, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, TIME_IN_TIME = (DateTime)d.TIME_IN, TIME_OUT_TIME = (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), PER_DIEM = (d.PER_DIEM == "Y" ? true : false), COMMENTS = d.COMMENTS, ROLE_TYPE = d.ROLE_TYPE, HEADER_ID = d.HEADER_ID, STATUS = d.DAILY_ACTIVITY_HEADER.STATUS, EQUIPMENT_ID = d.EQUIPMENT_ID }).ToList();
            foreach (var item in data)
            {
                item.PREVAILING_WAGE = RoleNeeded(_context, HeaderId);
                double Hours = Math.Truncate((double)item.TRAVEL_TIME);
                double Minutes = Math.Round(((double)item.TRAVEL_TIME - Hours) * 60);
                TimeSpan TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                item.TRAVEL_TIME_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                item.TOTAL_HOURS = (item.TIME_OUT - item.TIME_IN).ToString("hh\\:mm");
                Hours = Math.Truncate((double)item.DRIVE_TIME);
                Minutes = Math.Round(((double)item.DRIVE_TIME - Hours) * 60);
                TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                item.DRIVE_TIME_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                Hours = Math.Truncate((double)item.SHOPTIME_AM);
                Minutes = Math.Round(((double)item.SHOPTIME_AM - Hours) * 60);
                TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                item.SHOPTIME_AM_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                Hours = Math.Truncate((double)item.SHOPTIME_PM);
                Minutes = Math.Round(((double)item.SHOPTIME_PM - Hours) * 60);
                TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                item.SHOPTIME_PM_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
            }
            return data;

        }

        public static IQueryable<DAILY_ACTIVITY_EMPLOYEE> GetEmployee(Entities _context, long EmployeeId)
        {
            return _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmployeeId);
        }

        public static IQueryable<EquipmentDetails> GetEquipmentData(Entities _context, long HeaderId)
        {
            return (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                    join p in _context.CLASS_CODES_V on e.PROJECT_ID equals p.PROJECT_ID
                    where e.HEADER_ID == HeaderId
                    select new DAILY_ACTIVITY.EquipmentDetails { CLASS_CODE = p.CLASS_CODE, SEGMENT1 = p.SEGMENT1, ORGANIZATION_NAME = p.ORGANIZATION_NAME, ODOMETER_START = e.ODOMETER_START, ODOMETER_END = e.ODOMETER_END, PROJECT_ID = e.PROJECT_ID, EQUIPMENT_ID = e.EQUIPMENT_ID, NAME = p.NAME, HEADER_ID = e.HEADER_ID, STATUS = e.DAILY_ACTIVITY_HEADER.STATUS });
        }

        public static IQueryable<DAILY_ACTIVITY_EQUIPMENT> GetEquipment(Entities _context, long EquipmentId)
        {
            return _context.DAILY_ACTIVITY_EQUIPMENT.Where(x => x.EQUIPMENT_ID == EquipmentId);
        }

        public static IQueryable<PA_TASKS_V> GetTasks(Entities _context, long ProjectId)
        {
            return _context.PA_TASKS_V.Where(x => x.PROJECT_ID == ProjectId && x.START_DATE <= DateTime.Now && (x.COMPLETION_DATE >= DateTime.Now || x.COMPLETION_DATE == null));
        }

        public static bool RoleNeeded(Entities _context, long HeaderId)
        {
            string PrevailingWage = (from d in _context.DAILY_ACTIVITY_HEADER
                                     join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                     where d.HEADER_ID == HeaderId
                                     select p.ATTRIBUTE3).Single();
            if (PrevailingWage == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IQueryable<ProductionDetails> GetDBIProductionData(Entities _context, long HeaderId)
        {
            return from d in _context.DAILY_ACTIVITY_PRODUCTION
                   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                   join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                   join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                   where d.HEADER_ID == HeaderId
                   select new DAILY_ACTIVITY.ProductionDetails { PRODUCTION_ID = d.PRODUCTION_ID, PROJECT_ID = h.PROJECT_ID, LONG_NAME = p.LONG_NAME, TASK_NUMBER = t.TASK_NUMBER, TASK_ID = t.TASK_ID, DESCRIPTION = t.DESCRIPTION, TIME_IN = d.TIME_IN, TIME_OUT = d.TIME_OUT, WORK_AREA = d.WORK_AREA, POLE_FROM = d.POLE_FROM, POLE_TO = d.POLE_TO, ACRES_MILE = d.ACRES_MILE, QUANTITY = d.QUANTITY };
        }

        public static IQueryable<ProductionDetails> GetIRMProductionData(Entities _context, long HeaderId)
        {
            return from d in _context.DAILY_ACTIVITY_PRODUCTION
                   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                   join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                   join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                   where d.HEADER_ID == HeaderId
                   select new DAILY_ACTIVITY.ProductionDetails { PRODUCTION_ID = d.PRODUCTION_ID, PROJECT_ID = h.PROJECT_ID, LONG_NAME = p.LONG_NAME, TASK_ID = t.TASK_ID, TASK_NUMBER = t.TASK_NUMBER, DESCRIPTION = t.DESCRIPTION, SURFACE_TYPE = d.SURFACE_TYPE, WORK_AREA = d.WORK_AREA, QUANTITY = d.QUANTITY, STATION = d.STATION, EXPENDITURE_TYPE = d.EXPENDITURE_TYPE, BILL_RATE = d.BILL_RATE, UNIT_OF_MEASURE = d.UNIT_OF_MEASURE, COMMENTS = d.COMMENTS };
        }

        public static IQueryable<DAILY_ACTIVITY_PRODUCTION> GetProduction(Entities _context, long ProductionId)
        {
            return _context.DAILY_ACTIVITY_PRODUCTION.Where(x => x.PRODUCTION_ID == ProductionId);
        }

        public static IQueryable<WeatherDetails> GetWeatherData(Entities _context, long HeaderId)
        {
            return from w in _context.DAILY_ACTIVITY_WEATHER
                   where w.HEADER_ID == HeaderId
                   select new DAILY_ACTIVITY.WeatherDetails { WEATHER_ID = w.WEATHER_ID, HEADER_ID = w.HEADER_ID, COMMENTS = w.COMMENTS, HUMIDITY = w.HUMIDITY, TEMP = w.TEMP, WEATHER_DATE = (DateTime)w.WEATHER_DATE_TIME, WEATHER_TIME = (DateTime)w.WEATHER_DATE_TIME, WIND_DIRECTION = w.WIND_DIRECTION, WIND_VELOCITY = w.WIND_VELOCITY };
        }

        public static IQueryable<DAILY_ACTIVITY_WEATHER> GetWeather(Entities _context, long WeatherId)
        {
            return _context.DAILY_ACTIVITY_WEATHER.Where(x => x.WEATHER_ID == WeatherId);
        }

        public static IQueryable<ChemicalDetails> GetChemicalData(Entities _context, long HeaderId)
        {
            return (from c in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                    where c.HEADER_ID == HeaderId
                    select new DAILY_ACTIVITY.ChemicalDetails { CHEMICAL_MIX_ID = c.CHEMICAL_MIX_ID, CHEMICAL_MIX_NUMBER = c.CHEMICAL_MIX_NUMBER, COUNTY = c.COUNTY, STATE = c.STATE, TARGET_AREA = c.TARGET_AREA, GALLON_ACRE = c.GALLON_ACRE, GALLON_STARTING = c.GALLON_STARTING, GALLON_MIXED = c.GALLON_MIXED, GALLON_REMAINING = c.GALLON_REMAINING, ACRES_SPRAYED = c.ACRES_SPRAYED, TOTAL = c.GALLON_STARTING + c.GALLON_MIXED, GALLON_USED = c.GALLON_STARTING + c.GALLON_MIXED - c.GALLON_REMAINING, HEADER_ID = c.HEADER_ID }).OrderBy(x => x.CHEMICAL_MIX_NUMBER);
        }

        public static IQueryable<DAILY_ACTIVITY_CHEMICAL_MIX> GetChemical(Entities _context, long ChemicalId)
        {
            return _context.DAILY_ACTIVITY_CHEMICAL_MIX.Where(x => x.CHEMICAL_MIX_ID == ChemicalId);
        }

        public static IQueryable<InventoryDetails> GetDBIInventoryData(Entities _context, long HeaderId)
        {
            return from d in _context.DAILY_ACTIVITY_INVENTORY
                   join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                   join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID into mix
                   from withmix in mix.DefaultIfEmpty()
                   join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                   where d.HEADER_ID == HeaderId
                   from j in joined
                   where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                   select new DAILY_ACTIVITY.InventoryDetails { INVENTORY_ID = d.INVENTORY_ID, ENABLED_FLAG = j.ENABLED_FLAG, ITEM_ID = j.ITEM_ID, ACTIVE = j.ACTIVE, LE = j.LE, SEGMENT1 = j.SEGMENT1, LAST_UPDATE_DATE = j.LAST_UPDATE_DATE, ATTRIBUTE2 = j.ATTRIBUTE2, INV_LOCATION = j.INV_LOCATION, CONTRACTOR_SUPPLIED = (d.CONTRACTOR_SUPPLIED == "Y" ? true : false), TOTAL = d.TOTAL, INV_NAME = j.INV_NAME, CHEMICAL_MIX_ID = d.CHEMICAL_MIX_ID, CHEMICAL_MIX_NUMBER = withmix.CHEMICAL_MIX_NUMBER, SUB_INVENTORY_SECONDARY_NAME = d.SUB_INVENTORY_SECONDARY_NAME, SUB_INVENTORY_ORG_ID = d.SUB_INVENTORY_ORG_ID, DESCRIPTION = j.DESCRIPTION, RATE = d.RATE, UOM_CODE = u.UOM_CODE, UNIT_OF_MEASURE = u.UNIT_OF_MEASURE, EPA_DESCRIPTION = (d.EPA_NUMBER == null ? j.EPA_DESCRIPTION : d.EPA_NUMBER) };
        }

        public static IQueryable<InventoryDetails> GetIRMInventoryData(Entities _context, long HeaderId)
        {
            return from d in _context.DAILY_ACTIVITY_INVENTORY
                   join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                   where d.HEADER_ID == HeaderId
                   from j in joined
                   where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                   select new DAILY_ACTIVITY.InventoryDetails { INVENTORY_ID = d.INVENTORY_ID, INV_NAME = j.INV_NAME, SEGMENT1 = j.SEGMENT1, SUB_INVENTORY_SECONDARY_NAME = d.SUB_INVENTORY_SECONDARY_NAME, ITEM_ID = (decimal)d.ITEM_ID, SUB_INVENTORY_ORG_ID = d.SUB_INVENTORY_ORG_ID, DESCRIPTION = j.DESCRIPTION, RATE = d.RATE, UOM_CODE = j.UOM_CODE, UNIT_OF_MEASURE = d.UNIT_OF_MEASURE };
        }

        public static IQueryable<DAILY_ACTIVITY_INVENTORY> GetInventory(Entities _context, long InventoryId)
        {
            return _context.DAILY_ACTIVITY_INVENTORY.Where(x => x.INVENTORY_ID == InventoryId);
        }

        public static IQueryable<SYS_ATTACHMENTS> GetAttachmentData(Entities _context, long HeaderId)
        {
            return _context.SYS_ATTACHMENTS.Where(x => x.REFERENCE_TABLE == "DAILY_ACTIVITY_HEADER" && x.REFERENCE_NUMBER == HeaderId);
        }

        public static IQueryable<SYS_ATTACHMENTS> GetAttachment(Entities _context, long AttachmentId)
        {
            return _context.SYS_ATTACHMENTS.Where(x => x.ATTACHMENT_ID == AttachmentId);
        }

        public static IQueryable<FooterData> GetFooterData(Entities _context, long HeaderId)
        {
            return from d in _context.DAILY_ACTIVITY_FOOTER
                   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                   join e in _context.EMPLOYEES_V on h.PERSON_ID equals e.PERSON_ID
                   where d.HEADER_ID == HeaderId
                   select new FooterData { COMMENTS = d.COMMENTS, CONTRACT_REP = d.CONTRACT_REP, CONTRACT_REP_NAME = d.CONTRACT_REP_NAME, DOT_REP = d.DOT_REP, EMPLOYEE_NAME = e.EMPLOYEE_NAME, FOREMAN_SIGNATURE = d.FOREMAN_SIGNATURE, HOTEL_CITY = d.HOTEL_CITY, HOTEL_NAME = d.HOTEL_NAME, HOTEL_PHONE = d.HOTEL_PHONE, HOTEL_STATE = d.HOTEL_STATE };
        }

        public static IQueryable<DAILY_ACTIVITY_FOOTER> GetFooter(Entities _context, long HeaderId)
        {
            return _context.DAILY_ACTIVITY_FOOTER.Where(x => x.HEADER_ID == HeaderId);
        }

        public static long GetHeaderOrgId(Entities _context, long HeaderId)
        {
            return (from d in _context.DAILY_ACTIVITY_HEADER
                    join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                    where d.HEADER_ID == HeaderId
                    select (long)p.ORG_ID).Single();
        }

        public static long GetHeaderCoOrgId(Entities _context, long HeaderId)
        {
            return (from d in _context.DAILY_ACTIVITY_HEADER
                    join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                    where d.HEADER_ID == HeaderId
                    select p.CARRYING_OUT_ORGANIZATION_ID).Single();
        }


        public class EmployeeDetails
        {
            public long EMPLOYEE_ID { get; set; }
            public long HEADER_ID { get; set; }
            public int PERSON_ID { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public long? EQUIPMENT_ID { get; set; }
            public string NAME { get; set; }
            public DateTime TIME_IN { get; set; }
            public DateTime TIME_IN_TIME { get; set; }
            public DateTime TIME_OUT { get; set; }
            public DateTime TIME_OUT_TIME { get; set; }
            public decimal? TRAVEL_TIME { get; set; }
            public DateTime TRAVEL_TIME_FORMATTED { get; set; }
            public decimal? SHOPTIME_AM { get; set; }
            public DateTime SHOPTIME_AM_FORMATTED { get; set; }
            public decimal? SHOPTIME_PM { get; set; }
            public DateTime SHOPTIME_PM_FORMATTED { get; set; }
            public decimal? DRIVE_TIME { get; set; }
            public DateTime DRIVE_TIME_FORMATTED { get; set; }
            public bool PER_DIEM { get; set; }
            public string FOREMAN_LICENSE { get; set; }
            public string COMMENTS { get; set; }
            public bool PREVAILING_WAGE { get; set; }
            public string ROLE_TYPE { get; set; }
            public string LUNCH { get; set; }
            public decimal? LUNCH_LENGTH { get; set; }
            public DateTime? DA_DATE { get; set; }
            public int? STATUS { get; set; }
            public string STATE { get; set; }
            public string COUNTY { get; set; }
            public string TOTAL_HOURS { get; set; }
            public string PhantomID { get; set; }
        }

        public class EquipmentDetails
        {
            public string SEGMENT1 { get; set; }
            public string CLASS_CODE { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public long? ODOMETER_START { get; set; }
            public long? ODOMETER_END { get; set; }
            public long? PROJECT_ID { get; set; }
            public long? EQUIPMENT_ID { get; set; }
            public string NAME { get; set; }
            public long? HEADER_ID { get; set; }
            public int? STATUS { get; set; }
            public string PhantomID { get; set; }
        }

        public class ProductionDetails
        {
            public long? PRODUCTION_ID { get; set; }
            public long? PROJECT_ID { get; set; }
            public string LONG_NAME { get; set; }
            public string TASK_NUMBER { get; set; }
            public long TASK_ID { get; set; }
            public string DESCRIPTION { get; set; }
            public string STATION { get; set; }
            public string EXPENDITURE_TYPE { get; set; }
            public decimal? BILL_RATE { get; set; }
            public string SURFACE_TYPE { get; set; }
            public string COMMENTS { get; set; }
            public string UNIT_OF_MEASURE { get; set; }
            public DateTime? TIME_IN { get; set; }
            public DateTime? TIME_OUT { get; set; }
            public string WORK_AREA { get; set; }
            public string POLE_FROM { get; set; }
            public string POLE_TO { get; set; }
            public decimal? ACRES_MILE { get; set; }
            public decimal? QUANTITY { get; set; }
            public string PhantomID { get; set; }
        }

        public class WeatherDetails
        {
            public long? WEATHER_ID { get; set; }
            public long HEADER_ID { get; set; }
            public DateTime WEATHER_DATE { get; set; }
            public DateTime WEATHER_TIME { get; set; }
            public string WIND_DIRECTION { get; set; }
            public string WIND_VELOCITY { get; set; }
            public string HUMIDITY { get; set; }
            public string COMMENTS { get; set; }
            public string TEMP { get; set; }
            public string PhantomID { get; set; }
        }

        public class ChemicalDetails
        {
            public long? CHEMICAL_MIX_ID { get; set; }
            public long? CHEMICAL_MIX_NUMBER { get; set; }
            public long? HEADER_ID { get; set; }
            public string TARGET_AREA { get; set; }
            public decimal? GALLON_ACRE { get; set; }
            public decimal? GALLON_STARTING { get; set; }
            public decimal? GALLON_MIXED { get; set; }
            public decimal? GALLON_REMAINING { get; set; }
            public decimal? ACRES_SPRAYED { get; set; }
            public decimal? GALLON_USED { get; set; }
            public decimal? TOTAL { get; set; }
            public string STATE { get; set; }
            public string COUNTY { get; set; }
            public string PhantomID { get; set; }
        }

        public class InventoryDetails
        {
            public long? INVENTORY_ID { get; set; }
            public string ENABLED_FLAG { get; set; }
            public decimal ITEM_ID { get; set; }
            public string ACTIVE { get; set; }
            public string LE { get; set; }
            public string SEGMENT1 { get; set; }
            public DateTime? LAST_UPDATE_DATE { get; set; }
            public string ATTRIBUTE2 { get; set; }
            public long? INV_LOCATION { get; set; }
            public bool CONTRACTOR_SUPPLIED { get; set; }
            public decimal? TOTAL { get; set; }
            public string INV_NAME { get; set; }
            public long? CHEMICAL_MIX_ID { get; set; }
            public long? CHEMICAL_MIX_NUMBER { get; set; }
            public string SUB_INVENTORY_SECONDARY_NAME { get; set; }
            public decimal? SUB_INVENTORY_ORG_ID { get; set; }
            public string DESCRIPTION { get; set; }
            public decimal? RATE { get; set; }
            public string UOM_CODE { get; set; }
            public string UNIT_OF_MEASURE { get; set; }
            public string EPA_DESCRIPTION { get; set; }
            public string PhantomID { get; set; }
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
            public int? PERSON_ID { get; set; }
            public string APPLICATION_TYPE { get; set; }
            public string CONTRACTOR { get; set; }
            public string DENSITY { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public string LICENSE { get; set; }
            public string STATE { get; set; }
            public string SUBDIVISION { get; set; }
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

        public class FooterData
        {
            public string COMMENTS { get; set; }
            public string HOTEL_NAME { get; set; }
            public string HOTEL_CITY { get; set; }
            public string HOTEL_STATE { get; set; }
            public string HOTEL_PHONE { get; set; }
            public string CONTRACT_REP_NAME { get; set; }
            public string DOT_REP_NAME { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public byte[] FOREMAN_SIGNATURE { get; set; }
            public byte[] CONTRACT_REP { get; set; }
            public byte[] DOT_REP { get; set; }
        }
    }
}

