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
    public partial class umCombinedTab_IRM : BasePage
    {
        protected List<WarningData> WarningList = new List<WarningData>();

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
                GetIRMProductionData();
                GetWeatherData();
                GetInventory();
                GetFooterData();
                GetWarnings();
            }

            if (!X.IsAjaxRequest && !IsPostBack)
            {
                this.uxRedWarning.Value = ResourceManager.GetInstance().GetIconUrl(Icon.Exclamation);
                this.uxYellowWarning.Value = ResourceManager.GetInstance().GetIconUrl(Icon.Error);
            }
        }

        protected void GetWarnings()
        {
            if (WarningList.Count > 0)
            {
                uxWarningStore.DataSource = WarningList;
            }
            else
            {
                uxWarningGrid.Hide();
            }
        }

        /// <summary>
        /// Get data for header grid
        /// </summary>
        protected void GetHeaderData()
        {
            //Query and set datasource for header
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            where d.HEADER_ID == HeaderId
                            select new {d.HEADER_ID, d.PROJECT_ID, p.SEGMENT1, p.LONG_NAME, d.DA_DATE, d.SUBDIVISION, d.CONTRACTOR, d.PERSON_ID, e.EMPLOYEE_NAME, d.LICENSE, d.STATE, d.APPLICATION_TYPE, d.DENSITY, d.DA_HEADER_ID }).Single();
                DateTime Da_date = DateTime.Parse(data.DA_DATE.ToString());
                uxProjectField.Value = string.Format("{0} ({1})", data.LONG_NAME, data.SEGMENT1);
                uxDateField.Value = Da_date.ToString("MM-dd-yyyy");
                uxDensityField.Value = data.DENSITY;
                uxSubDivisionField.Value = data.SUBDIVISION;
                uxLicenseField.Value = data.LICENSE;
                uxStateField.Value = data.STATE;
                uxSupervisorField.Value = data.EMPLOYEE_NAME;
                uxContractorField.Value = data.CONTRACTOR;
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
                            select new EmployeeDetails{PERSON_ID = e.PERSON_ID, EMPLOYEE_ID = d.EMPLOYEE_ID,EMPLOYEE_NAME = e.EMPLOYEE_NAME, FOREMAN_LICENSE = d.FOREMAN_LICENSE, NAME = projects.NAME,TIME_IN =  (DateTime)d.TIME_IN, TIME_OUT =  (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), PER_DIEM = d.PER_DIEM, COMMENTS = d.COMMENTS }).ToList();

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
                    Hours = Math.Truncate((double)item.SHOPTIME_AM);
                    Minutes = Math.Round(((double)item.SHOPTIME_AM - Hours) * 60);
                    TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.SHOPTIME_AM_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
                    Hours = Math.Truncate((double)item.SHOPTIME_PM);
                    Minutes = Math.Round(((double)item.SHOPTIME_PM - Hours) * 60);
                    TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.SHOPTIME_PM_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");

                    List<WarningData> Overlaps = ValidationChecks.employeeTimeOverlapCheck(item.PERSON_ID, item.TIME_IN, HeaderId);
                    if (Overlaps.Count > 0)
                    {
                        WarningList.AddRange(Overlaps);
                    }
                    
                    WarningData EmployeeBusinessUnitFailures = ValidationChecks.EmployeeBusinessUnitCheck(item.EMPLOYEE_ID);
                    if (EmployeeBusinessUnitFailures != null)
                    {
                        WarningList.Add(EmployeeBusinessUnitFailures);
                        X.Js.Call("parent.App.uxPostActivityButton.disable(); parent.App.uxApproveActivityButton.disable()");
                    }
                    WarningData EmployeeOver24 = ValidationChecks.checkEmployeeTime(24, item.PERSON_ID, item.TIME_IN);
                    if (EmployeeOver24 != null)
                    {
                        WarningList.Add(EmployeeOver24);
                    }
                    else
                    {
                        WarningData EmployeeOver14 = ValidationChecks.checkEmployeeTime(14, item.PERSON_ID, item.TIME_IN);
                        if (EmployeeOver14 != null)
                        {
                            WarningList.Add(EmployeeOver14);
                        }
                    }
                    List<WarningData> DuplicatePerDiems = ValidationChecks.checkPerDiem(item.EMPLOYEE_ID, item.HEADER_ID);
                    if (DuplicatePerDiems.Count > 0)
                    {
                        WarningList.AddRange(DuplicatePerDiems);
                        X.Js.Call("parent.App.uxPostActivityButton.disable(); parent.App.uxApproveActivityButton.disable()");
                    }
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
                            select new { p.CLASS_CODE, p.SEGMENT1, p.ORGANIZATION_NAME, e.ODOMETER_START, e.ODOMETER_END, e.PROJECT_ID, e.EQUIPMENT_ID, p.NAME, e.HEADER_ID }).ToList();
                foreach (var item in data)
                {
                    WarningData BusinessUnitWarning = ValidationChecks.EquipmentBusinessUnitCheck(item.EQUIPMENT_ID);
                    if (BusinessUnitWarning != null)
                    {
                        WarningList.Add(BusinessUnitWarning);
                        X.Js.Call("parent.App.uxPostActivityButton.disable(); parent.App.uxApproveActivityButton.disable()");
                    }
                    WarningData MeterWarning = ValidationChecks.MeterCheck(item.EQUIPMENT_ID);
                    if (MeterWarning != null)
                    {
                        WarningList.Add(MeterWarning);
                    }
                }
                uxEquipmentStore.DataSource = data;
                uxEquipmentStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Production grid
        /// </summary>
        protected void GetIRMProductionData()
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
                            select new {d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.TASK_NUMBER, t.DESCRIPTION, d.SURFACE_TYPE, d.WORK_AREA, d.QUANTITY, d.STATION, d.EXPENDITURE_TYPE, d.BILL_RATE, d.UNIT_OF_MEASURE, d.COMMENTS }).ToList();
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
                            where d.HEADER_ID == HeaderId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new {j.INV_NAME, j.SEGMENT1, d.SUB_INVENTORY_SECONDARY_NAME, j.DESCRIPTION, d.RATE }).ToList();
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
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join e in _context.EMPLOYEES_V on h.PERSON_ID equals e.PERSON_ID
                            where d.HEADER_ID == HeaderId
                            select new { d, e.EMPLOYEE_NAME }).SingleOrDefault();
                if (data != null)
                {
                    uxReasonForNoWorkField.Value = data.d.COMMENTS;
                    uxHotelField.Value = data.d.HOTEL_NAME;
                    uxCityField.Value = data.d.HOTEL_CITY;
                    uxFooterStateField.Value = data.d.HOTEL_STATE;
                    uxPhoneField.Value = data.d.HOTEL_PHONE;
                    uxContractNameField.Value = data.d.CONTRACT_REP_NAME;
                    uxDOTRep.Value = data.d.DOT_REP_NAME;
                    uxForemanNameField.Value = data.EMPLOYEE_NAME;
                    try
                    {
                        byte[] ForemanImage = data.d.FOREMAN_SIGNATURE.ToArray();
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
                        byte[] ContractImage = data.d.CONTRACT_REP.ToArray();
                        if (ContractImage.Length > 0)
                        {
                            uxContractImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=contract", HeaderId);
                        }
                    }
                    catch (Exception)
                    {
                        uxContractImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                    try
                    {
                        byte[] DOTImage = data.d.DOT_REP.ToArray();
                        if (DOTImage.Length > 0)
                        {
                            uxDOTImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=dot", HeaderId);
                        }
                    }
                    catch (Exception)
                    {
                        uxDOTImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                }
            }
        }
    }
}