using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umCombinedTab_DBI : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            if (!X.IsAjaxRequest)
            {
                GetHeaderData();
                GetEmployeeData();
                GetEquipmentData();
                GetDBIProductionData();
                GetWeatherData();
                GetChemicalMixData();
                GetInventory();
                GetFooterData();
            }
        }

        /// <summary>
        /// Get data for header grid
        /// </summary>
        protected void GetHeaderData()
        {
            //Query and set datasource for header
            using (Entities _context = new Entities()){
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            where d.HEADER_ID == HeaderId
                            select new {d.HEADER_ID, d.PROJECT_ID, p.LONG_NAME, d.DA_DATE, d.SUBDIVISION, d.CONTRACTOR, d.PERSON_ID, e.EMPLOYEE_NAME, d.LICENSE, d.STATE, d.APPLICATION_TYPE, d.DENSITY, d.DA_HEADER_ID }).Single();
                DateTime Da_date = DateTime.Parse(data.DA_DATE.ToString());
                uxProjectField.Value = data.LONG_NAME;
                uxDateField.Value = Da_date.ToString("MM-dd-yyyy");
                uxDensityField.Value = data.DENSITY;
                uxSubDivisionField.Value = data.SUBDIVISION;
                uxLicenseField.Value = data.LICENSE;
                uxStateField.Value = data.STATE;
                uxSupervisorField.Value = data.EMPLOYEE_NAME;
                uxTypeField.Value = data.APPLICATION_TYPE;
                uxHeaderField.Value = data.HEADER_ID.ToString();
                uxOracleField.Value = data.DA_HEADER_ID.ToString();
            }
        }

        /// <summary>
        /// Get data for Employee/Equipment grid
        /// </summary>
        protected void GetEmployeeData()
        {
            //Query and set datasource for employees
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                            from projects in proj.DefaultIfEmpty()
                            where d.HEADER_ID == HeaderId
                            select new EmployeeDetails {EMPLOYEE_NAME = e.EMPLOYEE_NAME, NAME = projects.NAME, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), PER_DIEM = d.PER_DIEM, COMMENTS = d.COMMENTS }).ToList();
                foreach (var item in data)
                {
                    double Hours = Math.Truncate((double)item.TRAVEL_TIME);
                    double Minutes = Math.Round(((double)item.TRAVEL_TIME - Hours) * 60);
                    TimeSpan TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.TRAVEL_TIME_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
                    Hours = Math.Truncate((double)item.DRIVE_TIME);
                    Minutes = Math.Round(((double)item.DRIVE_TIME - Hours) * 60);
                    TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.DRIVE_TIME_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
                }

                uxEmployeeStore.DataSource = data;
                uxEmployeeStore.DataBind();
            }
        }

        protected void GetEquipmentData()
        {
            
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["headerId"]);
                var data = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                            join p in _context.CLASS_CODES_V on e.PROJECT_ID equals p.PROJECT_ID
                            where e.HEADER_ID == HeaderId
                            select new {p.CLASS_CODE, p.ORGANIZATION_NAME, e.ODOMETER_START, e.ODOMETER_END, e.PROJECT_ID, e.EQUIPMENT_ID, p.NAME, e.HEADER_ID }).ToList();
                uxEquipmentStore.DataSource = data;
                uxEquipmentStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Production grid
        /// </summary>
        protected void GetDBIProductionData()
        {
            //Query and set datasource for Production
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.DESCRIPTION, d.TIME_IN, d.TIME_OUT, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.QUANTITY }).ToList();
                uxProductionStore.DataSource = data;
                uxProductionStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Weather grid
        /// </summary>
        protected void GetWeatherData()
        {
            //Query and set datasource for Weather
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from w in _context.DAILY_ACTIVITY_WEATHER
                            where w.HEADER_ID == HeaderId
                            select w).ToList();
                uxWeatherStore.DataSource = data;
                uxWeatherStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Chemical Mix grid
        /// </summary>
        protected void GetChemicalMixData()
        {
            //Query and set datasource for Chemical Mix
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from c in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                            where c.HEADER_ID == HeaderId
                            select new { c.CHEMICAL_MIX_NUMBER, c.COUNTY, c.STATE, c.TARGET_AREA, c.GALLON_ACRE, c.GALLON_STARTING, c.GALLON_MIXED, c.GALLON_REMAINING, c.ACRES_SPRAYED, TOTAL = c.GALLON_STARTING + c.GALLON_MIXED, USED = c.GALLON_STARTING + c.GALLON_MIXED - c.GALLON_REMAINING }).ToList();
                uxChemicalStore.DataSource = data;
                uxChemicalStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Inventory grid
        /// </summary>
        protected void GetInventory()
        {
            //Query and set datasource for Inventory
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                            join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                            where d.HEADER_ID == HeaderId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new {c.CHEMICAL_MIX_NUMBER, d.SUB_INVENTORY_SECONDARY_NAME, d.TOTAL, j.DESCRIPTION, d.RATE, u.UNIT_OF_MEASURE, d.EPA_NUMBER }).ToList();
                uxInventoryStore.DataSource = data;
                uxInventoryStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Footer grid
        /// </summary>
        protected void GetFooterData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_FOOTER
                            where d.HEADER_ID == HeaderId
                            select d).SingleOrDefault();
                if (data != null)
                {
                    uxReasonForNoWorkField.Value = data.COMMENTS;
                    uxHotelField.Value = data.HOTEL_NAME;
                    uxCityField.Value = data.HOTEL_CITY;
                    uxFooterStateField.Value = data.HOTEL_STATE;
                    uxPhoneField.Value = data.HOTEL_PHONE;
                    uxContractNameField.Value = data.CONTRACT_REP_NAME;
                    try
                    {
                        byte[] ForemanImage = data.FOREMAN_SIGNATURE.ToArray();
                        if (ForemanImage.Length > 0)
                        {
                            uxForemanImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=foreman", HeaderId);
                        }
                    }
                    catch (Exception)
                    {
                        uxForemanImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                    try
                    {
                        byte[] ContractImage = data.CONTRACT_REP.ToArray();
                        if (ContractImage.Length > 0)
                        {
                            uxContractImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=contract", HeaderId);
                        }
                    }
                    catch (Exception)
                    {
                        uxContractImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                }
            }
        }
    }
}