using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using DBI.Data.DataFactory;
using Ext.Net;
using System.Security.Claims;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umCombinedTab_IRM : BasePage
    {
        protected List<WarningData> WarningList = new List<WarningData>();

        protected void Page_Load(object sender, EventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }

            if (!X.IsAjaxRequest)
            {
                GetInventoryDropDown();
                GetHeaderData();
                GetEmployeeData();
                GetEquipmentData();
                GetIRMProductionData();
                GetWeatherData();
                GetInventory();
                GetFooterData();
                GetWarnings();

                uxAddProductionSurfaceTypeStore.Data = StaticLists.SurfaceTypes;
                uxStateList.Data = StaticLists.StateList;
                uxStateStore.Data = StaticLists.StateList;
                uxAddWeatherWindStore.Data = StaticLists.WindDirection;
                
                this.uxRedWarning.Value = ResourceManager.GetInstance().GetIconUrl(Ext.Net.Icon.Exclamation);
                this.uxYellowWarning.Value = ResourceManager.GetInstance().GetIconUrl(Ext.Net.Icon.Error);

                using (Entities _context = new Entities())
                {
                    DateTime HeaderDate = _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId).Select(x => (DateTime)x.DA_DATE).Single();
                    uxEmployeeTimeInDate.Value = HeaderDate.Date.ToString("MM/dd/yyyy");
                    uxEmployeeTimeOutDate.MinDate = HeaderDate;
                    uxEmployeeTimeOutDate.MaxDate = HeaderDate.AddDays(1);
                }

                if (GetStatus(long.Parse(Request.QueryString["HeaderId"])) != 2)
                {
                    uxEmployeeToolbar.Hidden = true;
                    uxEquipmentToolbar.Hidden = true;
                    uxProductionToolbar.Hidden = true;
                    uxWeatherToolbar.Hidden = true;
                    uxInventoryToolbar.Hidden = true;
                    uxSaveFooterButton.Hidden = true;
                    uxSaveHeaderButton.Hidden = true;

                    uxDateField.ReadOnly = true;
                    uxProjectField.ReadOnly = true;
                    uxSubDivisionField.ReadOnly = true;
                    uxContractorField.ReadOnly = true;
                    uxSupervisorField.ReadOnly = true;
                    uxLicenseField.ReadOnly = true;
                    uxStateField.ReadOnly = true;
                    uxTypeField.ReadOnly = true;
                    uxDensityField.ReadOnly = true;

                    uxReasonForNoWorkField.ReadOnly = true;
                    uxHotelField.ReadOnly = true;
                    uxCityField.ReadOnly = true;
                    uxFooterStateField.ReadOnly = true;
                    uxPhoneField.ReadOnly = true;
                    uxForemanNameField.ReadOnly = true;
                    uxForemanImageField.Hide();
                    uxDOTRep.ReadOnly = true;
                    uxDotRepImageField.Hide();
                    uxContractNameField.ReadOnly = true;
                    uxContractImageField.Hide();

                }
            }
        }

        protected int GetStatus(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                return (int)_context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId).Select(x => x.STATUS).Single();
            }
        }

        protected void GetWarnings()
        {
            int Status = GetStatus(long.Parse(Request.QueryString["HeaderId"]));

            if (WarningList.Count > 0)
            {
                uxWarningStore.DataSource = WarningList;
                if (!WarningList.Exists(x => x.WarningType == "Error"))
                {
                    switch (Status)
                    {
                        case 2:
                            if (validateComponentSecurity("SYS.DailyActivity.Approve"))
                            {
                                X.Js.Call("parent.App.uxApproveActivityButton.enable();parent.App.uxInactiveActivityButton.enable()");
                            }
                            break;
                        case 3:
                            if (validateComponentSecurity("SYS.DailyActivity.Post"))
                            {
                                X.Js.Call("parent.App.uxPostActivityButton.enable();parent.App.uxInactiveActivityButton.enable()");
                            }
                            break;
                    }
                }
            }
            else
            {
                uxWarningGrid.Hide();
                switch (Status)
                {
                    case 2:
                        if (validateComponentSecurity("SYS.DailyActivity.Approve"))
                        {
                            X.Js.Call("parent.App.uxApproveActivityButton.enable();parent.App.uxInactiveActivityButton.enable()");
                        }
                        break;
                    case 3:
                        if (validateComponentSecurity("SYS.DailyActivity.Post"))
                        {
                            X.Js.Call("parent.App.uxPostActivityButton.enable();parent.App.uxInactiveActivityButton.enable()");
                        }
                        break;
                }

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
                            select new {d.HEADER_ID, d.PROJECT_ID, p.SEGMENT1, p.LONG_NAME, d.DA_DATE, d.SUBDIVISION, d.CONTRACTOR, d.PERSON_ID, e.EMPLOYEE_NAME, d.LICENSE, d.STATE, d.APPLICATION_TYPE, d.DENSITY, d.DA_HEADER_ID, d.STATUS }).Single();
                DateTime Da_date = DateTime.Parse(data.DA_DATE.ToString());
                uxProjectField.SetValue(data.PROJECT_ID.ToString(), string.Format("({0}) - {1}", data.SEGMENT1, data.LONG_NAME));
                uxDateField.SelectedDate = Da_date;
                uxDensityField.SetValue(data.DENSITY);
                uxSubDivisionField.Value = data.SUBDIVISION;
                uxLicenseField.Value = data.LICENSE;
                uxStateField.SetValue(data.STATE);
                uxSupervisorField.SetValue(data.PERSON_ID.ToString(), data.EMPLOYEE_NAME);
                uxContractorField.Value = data.CONTRACTOR;
                uxTypeField.Value = data.APPLICATION_TYPE;
                uxHeaderField.Value = data.HEADER_ID.ToString();
                uxOracleField.Value = data.DA_HEADER_ID.ToString();
                uxStatusField.Value = data.STATUS.ToString();
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
                            select new EmployeeDetails {PERSON_ID = e.PERSON_ID, EMPLOYEE_ID = d.EMPLOYEE_ID, DA_DATE = d.DAILY_ACTIVITY_HEADER.DA_DATE, EMPLOYEE_NAME = e.EMPLOYEE_NAME, FOREMAN_LICENSE = d.FOREMAN_LICENSE, NAME = projects.NAME, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, TIME_IN_TIME = (DateTime)d.TIME_IN, TIME_OUT_TIME = (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), PER_DIEM = (d.PER_DIEM == "Y" ? true: false), COMMENTS = d.COMMENTS, ROLE_TYPE = d.ROLE_TYPE, STATUS = d.DAILY_ACTIVITY_HEADER.STATUS, EQUIPMENT_ID = d.EQUIPMENT_ID }).ToList();
                foreach (var item in data)
                {
                    item.PREVAILING_WAGE = roleNeeded();
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

                    List<WarningData> Overlaps = ValidationChecks.employeeTimeOverlapCheck(item.PERSON_ID, item.TIME_IN, HeaderId);
                    if (Overlaps.Count > 0)
                    {
                        WarningList.AddRange(Overlaps);
                        if (item.STATUS == 3)
                        {
                            X.Js.Call("disablePostOnError");
                            
                        }
                        else
                        {
                            X.Js.Call("disableOnError");
                        }
                    }
                    
                    WarningData EmployeeBusinessUnitFailures = ValidationChecks.EmployeeBusinessUnitCheck((long)item.EMPLOYEE_ID);
                    if (EmployeeBusinessUnitFailures != null)
                    {
                        WarningList.Add(EmployeeBusinessUnitFailures);
                        if (item.STATUS == 3 && EmployeeBusinessUnitFailures.WarningType == "Warning")
                        {
                            X.Js.Call("disablePostOnError");
                        }
                        else
                        {
                            X.Js.Call("disableOnError");
                        }
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
                    List<WarningData> DuplicatePerDiems = ValidationChecks.checkPerDiem((long)item.EMPLOYEE_ID, item.HEADER_ID);
                    if (DuplicatePerDiems.Count > 0)
                    {
                        WarningList.AddRange(DuplicatePerDiems);
                        if (item.STATUS == 3)
                        {
                            X.Js.Call("disablePostOnError");
                        }
                        else
                        {
                            X.Js.Call("disableOnError");
                        }
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
                            select new { p.CLASS_CODE, p.SEGMENT1, p.ORGANIZATION_NAME, e.ODOMETER_START, e.ODOMETER_END, e.PROJECT_ID, e.EQUIPMENT_ID, p.NAME, e.HEADER_ID, STATUS = e.DAILY_ACTIVITY_HEADER.STATUS }).ToList();
                foreach (var item in data)
                {
                    WarningData BusinessUnitWarning = ValidationChecks.EquipmentBusinessUnitCheck(item.EQUIPMENT_ID);
                    if (BusinessUnitWarning != null)
                    {
                        WarningList.Add(BusinessUnitWarning);
                        if (item.STATUS == 3 && BusinessUnitWarning.WarningType == "Warning")
                        {
                            X.Js.Call("disablePostOnError");
                        }
                        else
                        {
                            X.Js.Call("disableOnError");
                        }
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
                            select new ProductionDetails { PRODUCTION_ID = d.PRODUCTION_ID, PROJECT_ID = h.PROJECT_ID, LONG_NAME = p.LONG_NAME, TASK_ID = t.TASK_ID, TASK_NUMBER = t.TASK_NUMBER, DESCRIPTION = t.DESCRIPTION, SURFACE_TYPE = d.SURFACE_TYPE, WORK_AREA = d.WORK_AREA, QUANTITY = d.QUANTITY, STATION = d.STATION, EXPENDITURE_TYPE = d.EXPENDITURE_TYPE, BILL_RATE = d.BILL_RATE, UNIT_OF_MEASURE = d.UNIT_OF_MEASURE, COMMENTS = d.COMMENTS }).ToList();
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
                            select new WeatherDetails { WEATHER_ID = w.WEATHER_ID, HEADER_ID = w.HEADER_ID, COMMENTS = w.COMMENTS, HUMIDITY = w.HUMIDITY, TEMP = w.TEMP, WEATHER_DATE = (DateTime)w.WEATHER_DATE_TIME, WEATHER_TIME = (DateTime)w.WEATHER_DATE_TIME, WIND_DIRECTION = w.WIND_DIRECTION, WIND_VELOCITY = w.WIND_VELOCITY }).ToList();
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
                            select new InventoryDetails { INVENTORY_ID = d.INVENTORY_ID, INV_NAME = j.INV_NAME, SEGMENT1 = j.SEGMENT1, SUB_INVENTORY_SECONDARY_NAME = d.SUB_INVENTORY_SECONDARY_NAME, ITEM_ID = (decimal)d.ITEM_ID, SUB_INVENTORY_ORG_ID = d.SUB_INVENTORY_ORG_ID, DESCRIPTION = j.DESCRIPTION, RATE = d.RATE, UOM_CODE = j.UOM_CODE, UNIT_OF_MEASURE = d.UNIT_OF_MEASURE }).ToList();
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
                    uxFooterStateField.SetValue(data.d.HOTEL_STATE);
                    uxPhoneField.Value = data.d.HOTEL_PHONE;
                    uxContractNameField.Value = data.d.CONTRACT_REP_NAME;
                    uxDOTRep.Value = data.d.DOT_REP_NAME;
                    uxForemanNameField.Value = data.EMPLOYEE_NAME;
                    if (data.d.FOREMAN_SIGNATURE == null || data.d.FOREMAN_SIGNATURE.Length == 0)
                    {
                        uxForemanImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                    else
                    {
                        uxForemanImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=foreman", HeaderId);
                    }
                    if (data.d.CONTRACT_REP == null || data.d.CONTRACT_REP.Length == 0)
                    {
                        uxContractImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                    else
                    {
                        uxContractImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=contract", HeaderId);
                    }

                    if (data.d.DOT_REP == null || data.d.DOT_REP.Length == 0)
                    {
                        uxDOTImage.ImageUrl = "../../../Resources/Images/1pixel.jpg";
                    }
                    else
                    {
                        uxDOTImage.ImageUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=dot", HeaderId);
                    }
                }
            }
        }

        /// <summary>
        /// Reads/Filters Project Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadProjectData(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_PROJECTS_V> dataIn;
            if (uxFormProjectToggleOrg.Pressed)
            {
                //Get All Projects
                dataIn = WEB_PROJECTS_V.ProjectList();
            }
            else
            {
                int CurrentOrg = Convert.ToInt32(Authentication.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get projects for my org only
                dataIn = WEB_PROJECTS_V.ProjectList(CurrentOrg);
            }

            int count;

            List<WEB_PROJECTS_V> data = GenericData.EnumerableFilterHeader<WEB_PROJECTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxFormProjectStore.DataSource = data;
            uxFormProjectStore.DataBind();
        }

        /// <summary>
        /// Puts value into DropDownField and clears filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreProjectValue(object sender, DirectEventArgs e)
        {
            //Set value and text
            uxProjectField.SetValue(e.ExtraParams["ProjectId"], string.Format("({0}) - {1}", e.ExtraParams["ProjectNumber"], e.ExtraParams["LongName"]));
            //Clear existing filters
            uxFormProjectFilter.ClearFilter();
        }

        /// <summary>
        /// Toggles the text for the dropdowns based on what the current text is and reloads the store.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReloadStore(object sender, DirectEventArgs e)
        {
            string type = e.ExtraParams["Type"];
            if (type == "Employee")
            {
                uxFormEmployeeStore.Reload();
                if (uxFormEmployeeToggleOrg.Pressed)
                {
                    uxFormEmployeeToggleOrg.Text = "My Region";
                }
                else
                {
                    uxFormEmployeeToggleOrg.Text = "All Regions";
                }
            }
            else
            {
                uxFormProjectStore.Reload();
                if (uxFormProjectToggleOrg.Pressed)
                {
                    uxFormProjectToggleOrg.Text = "My Region";
                }
                else
                {
                    uxFormProjectToggleOrg.Text = "All Regions";
                }
            }
        }

        /// <summary>
        /// Puts value into Employee DropDownField and clears filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreEmployee(object sender, DirectEventArgs e)
        {
            //Set value and text for employee
            uxSupervisorField.SetValue(e.ExtraParams["PersonID"], e.ExtraParams["EmployeeName"]);
            //Clear existing filters
            uxFormEmployeeFilter.ClearFilter();
        }

        /// <summary>
        /// Reads/Filters Employee Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadEmployees(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            List<EMPLOYEES_V> dataIn = new List<EMPLOYEES_V>();
            if (uxFormEmployeeToggleOrg.Pressed)
            {
                //Get Employees for all regions
                dataIn = EMPLOYEES_V.EmployeeDropDown();
            }
            else
            {
                int CurrentOrg = Convert.ToInt32(Authentication.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get Employees for my region only
                dataIn = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
            }
            int count;

            List<EMPLOYEES_V> data = GenericData.EnumerableFilterHeader<EMPLOYEES_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxFormEmployeeStore.DataSource = data;
            uxFormEmployeeStore.DataBind();
        }

        protected void deUpdateHeader(object sender, Ext.Net.DirectEventArgs e)
        {
            //Get values in correct formats
            long ProjectId = long.Parse(uxProjectField.Value.ToString());
            DateTime DaDate = (DateTime)uxDateField.Value;
            int PersonId = int.Parse(uxSupervisorField.Value.ToString());

            DAILY_ACTIVITY_HEADER data;

            using (Entities _context = new Entities())
            {
                var HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                        where d.HEADER_ID == HeaderId
                        select d).Single();
            }
            data.PROJECT_ID = ProjectId;
            data.DA_DATE = DaDate;
            data.SUBDIVISION = uxSubDivisionField.Text;
            data.CONTRACTOR = uxContractorField.Text;
            data.PERSON_ID = PersonId;
            data.LICENSE = uxLicenseField.Value.ToString();
            data.STATE = uxStateField.Value.ToString();
            data.APPLICATION_TYPE = uxTypeField.Text;
            data.DENSITY = uxDensityField.SelectedItem.Value;
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload()");
        }

        /// <summary>
        /// Direct event to store footer to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deUpdateFooter(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_FOOTER data;

            //Set HeaderId
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                //Check if footer record exists
                data = (from d in _context.DAILY_ACTIVITY_FOOTER
                        where d.HEADER_ID == HeaderId
                        select d).SingleOrDefault();
            }

            if (data != null)
            {
                //Check for empty values
                data.COMMENTS = uxReasonForNoWorkField.Text;
                data.HOTEL_NAME = uxHotelField.Text;
                data.HOTEL_CITY = uxCityField.Text;
                data.HOTEL_STATE = uxFooterStateField.Text;
                data.HOTEL_PHONE = uxPhoneField.Text;
                data.CONTRACT_REP_NAME = uxContractNameField.Text;
                data.DOT_REP_NAME = uxDOTRep.Text;
                
                //file upload
                HttpPostedFile ForemanSignatureFile = uxForemanImageField.PostedFile;
                byte[] ForemanSignatureArray = ImageToByteArray(ForemanSignatureFile);
                if (ForemanSignatureFile.ContentLength > 0)
                {
                    data.FOREMAN_SIGNATURE = ForemanSignatureArray;
                }

                //file upload
                HttpPostedFile ContractRepFile = uxContractImageField.PostedFile;
                byte[] ContractRepArray = ImageToByteArray(ContractRepFile);

                if (ContractRepFile.ContentLength > 0)
                {
                    data.CONTRACT_REP = ContractRepArray;
                }

                //file upload
                HttpPostedFile DotRepFile = uxDotRepImageField.PostedFile;
                byte[] DotRepArray = ImageToByteArray(DotRepFile);

                if (DotRepFile.ContentLength > 0)
                {
                    data.DOT_REP = DotRepArray;
                }

                data.MODIFIED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;

                GenericData.Update<DAILY_ACTIVITY_FOOTER>(data);
            }
            else
            {
                data = new DAILY_ACTIVITY_FOOTER();

                data.HEADER_ID = HeaderId;

                //Check for empty values
                data.COMMENTS = uxReasonForNoWorkField.Text;
                data.HOTEL_NAME = uxHotelField.Text;
                data.HOTEL_CITY = uxCityField.Text;
                data.HOTEL_STATE = uxFooterStateField.Text;
                data.HOTEL_PHONE = uxPhoneField.Text;
                data.CONTRACT_REP_NAME = uxContractNameField.Text;
                data.DOT_REP_NAME = uxDOTRep.Text;

                try
                {
                    //file upload
                    HttpPostedFile ForemanSignatureFile = uxForemanImageField.PostedFile;
                    byte[] ForemanSignatureArray = ImageToByteArray(ForemanSignatureFile);

                    data.FOREMAN_SIGNATURE = ForemanSignatureArray;
                }
                catch
                {
                    data.FOREMAN_SIGNATURE = null;
                }

                try
                {
                    //file upload
                    HttpPostedFile ContractRepFile = uxContractImageField.PostedFile;
                    byte[] ContractRepArray = ImageToByteArray(ContractRepFile);

                    data.CONTRACT_REP = ContractRepArray;
                }

                catch
                {
                    data.CONTRACT_REP = null;
                }

                try
                {
                    string DotRepName = uxDOTRep.Value.ToString();
                    data.DOT_REP_NAME = DotRepName;
                }
                catch
                {
                    data.DOT_REP_NAME = null;
                }

                try
                {
                    //file upload
                    HttpPostedFile DotRepFile = uxDotRepImageField.PostedFile;
                    byte[] DotRepArray = ImageToByteArray(DotRepFile);

                    data.DOT_REP = DotRepArray;
                }
                catch
                {
                    data.DOT_REP = null;
                }

                data.CREATED_BY = User.Identity.Name;
                data.MODIFIED_BY = User.Identity.Name;
                data.CREATE_DATE = DateTime.Now;
                data.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<DAILY_ACTIVITY_FOOTER>(data);

            }

            X.Js.Call("parent.App.uxDetailsPanel.reload()");
        }

        /// <summary>
        /// Converts uploaded image file to byte array for DB storage
        /// </summary>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        protected byte[] ImageToByteArray(HttpPostedFile ImageFile)
        {
            byte[] ImageArray = null;
            BinaryReader b = new BinaryReader(ImageFile.InputStream);
            ImageArray = b.ReadBytes(ImageFile.ContentLength);
            return ImageArray;
        }

        protected bool roleNeeded()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            using (Entities _context = new Entities())
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
        }

        protected void deReadRoleData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                List<PA_ROLES_V> RoleList = (from d in _context.DAILY_ACTIVITY_HEADER
                                             join p in _context.PA_ROLES_V on d.PROJECT_ID equals p.PROJECT_ID
                                             where d.HEADER_ID == HeaderId
                                             select p).ToList();

                uxEmployeeRoleStore.DataSource = RoleList;
            }
        }

        protected void deStoreRoleGridValue(object sender, DirectEventArgs e)
        {
            uxEmployeeRole.SetValue(e.ExtraParams["Meaning"], e.ExtraParams["Meaning"]);
            uxEmployeeState.SetValue(e.ExtraParams["State"]);
            uxEmployeeCounty.SetValue(e.ExtraParams["County"]);
        }

        /// <summary>
        /// Get List of employees from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadEmployeeData(object sender, StoreReadDataEventArgs e)
        {
            List<EMPLOYEES_V> dataIn;

            if (uxAddEmployeeRegion.Pressed)
            {
                //Get All Projects
                dataIn = EMPLOYEES_V.EmployeeDropDown();
            }
            else
            {
                int CurrentOrg = Convert.ToInt32(Authentication.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get projects for my org only
                dataIn = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
            }
            int count;

            //Get paged,filterable list of Employees
            List<EMPLOYEES_V> data = GenericData.EnumerableFilterHeader<EMPLOYEES_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxEmployeeEmpStore.DataSource = data;
            uxEmployeeEmpStore.DataBind();
        }

        /// <summary>
        /// Update selected item of what's chosen from Gridpanel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Type"] == "Equipment")
            {
                uxEmployeeEqDropDown.SetValue(e.ExtraParams["EquipmentId"], e.ExtraParams["Name"]);
            }
            else
            {
                uxEmployeeEmpDropDown.SetValue(e.ExtraParams["PersonId"], e.ExtraParams["Name"]);
                uxEmployeeEmpFilter.ClearFilter();
            }
        }

        protected void deStoreTask(object sender, DirectEventArgs e)
        {
            uxAddProductionTask.SetValue(e.ExtraParams["TaskId"], e.ExtraParams["Description"]);
            uxAddProductionTaskStore.ClearFilter();
        }

        /// <summary>
        /// Checks for a project with an existing per diem on the current date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deCheckExistingPerDiem(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long PersonId = long.Parse(e.ExtraParams["PersonId"]);

            //Get the date from this header record
            using (Entities _context = new Entities())
            {
                var HeaderDate = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.HEADER_ID == HeaderId
                                  select d.DA_DATE).Single();

                //Get all headerIds on this date for this person
                var HeaderList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                  where d.PERSON_ID == PersonId && d.DAILY_ACTIVITY_HEADER.DA_DATE == HeaderDate
                                  select d.PER_DIEM).ToList();
                bool Disable = false;

                foreach (var Header in HeaderList)
                {
                    if (Header == "Y")
                    {
                        Disable = true;
                    }
                }

                if (Disable)
                {
                    uxPerDiemColumn.Editable = false;
                }
            }
        }

        /// <summary>
        /// Get Equipment entered on equipment page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadEquipmentData(object sender, StoreReadDataEventArgs e)
        {
            //Query for list of equipment
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                            join p in _context.CLASS_CODES_V on d.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.EQUIPMENT_ID, p.NAME, d.PROJECT_ID, p.ORGANIZATION_NAME, p.CLASS_CODE, p.SEGMENT1, d.ODOMETER_START, d.ODOMETER_END }).ToList();
                //Set add store
                uxEmployeeEqStore.DataSource = data;
                uxEmployeeEqStore.DataBind();

            }

        }

        /// <summary>
        /// Toggle Region text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRegionToggle(object sender, DirectEventArgs e)
        {
            if (uxAddEmployeeRegion.Pressed)
            {
                uxAddEmployeeRegion.Text = "My Region";
                uxEmployeeEmpStore.Reload();
            }
            else
            {
                uxAddEmployeeRegion.Text = "All Regions";
                uxEmployeeEmpStore.Reload();
            }
        }

        protected void deSetTimeInDate(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                DateTime? HeaderDate = _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId).Select(x => x.DA_DATE).Single();
                if (HeaderDate != null)
                {
                    uxEmployeeTimeInDate.SelectedDate = (DateTime)HeaderDate;
                }
            }
        }

        protected void deSaveEmployee(object sender, DirectEventArgs e)
        {
            ChangeRecords<EmployeeDetails> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<EmployeeDetails>();

            foreach (EmployeeDetails item in data.Created)
            {
                DAILY_ACTIVITY_EMPLOYEE NewEmployee = new DAILY_ACTIVITY_EMPLOYEE();
                NewEmployee.COMMENTS = item.COMMENTS;
                NewEmployee.CREATE_DATE = DateTime.Now;
                NewEmployee.CREATED_BY = User.Identity.Name;
                NewEmployee.EQUIPMENT_ID = item.EQUIPMENT_ID;
                NewEmployee.FOREMAN_LICENSE = item.FOREMAN_LICENSE;
                NewEmployee.HEADER_ID = long.Parse(Request.QueryString["HeaderId"]);
                NewEmployee.MODIFIED_BY = User.Identity.Name;
                NewEmployee.MODIFY_DATE = DateTime.Now;
                NewEmployee.PER_DIEM = (item.PER_DIEM == true ? "Y" : "N");
                NewEmployee.PERSON_ID = item.PERSON_ID;
                NewEmployee.TIME_IN = item.TIME_IN.Date + item.TIME_IN_TIME.TimeOfDay;
                NewEmployee.TIME_OUT = item.TIME_OUT.Date + item.TIME_OUT_TIME.TimeOfDay;
                NewEmployee.TRAVEL_TIME = (decimal)item.TRAVEL_TIME_FORMATTED.TimeOfDay.TotalMinutes / 60;
                NewEmployee.DRIVE_TIME = (decimal)item.DRIVE_TIME_FORMATTED.TimeOfDay.TotalMinutes / 60;
                NewEmployee.SHOPTIME_AM = (decimal)item.SHOPTIME_AM_FORMATTED.TimeOfDay.TotalMinutes / 60;
                NewEmployee.SHOPTIME_PM = (decimal)item.SHOPTIME_PM_FORMATTED.TimeOfDay.TotalMinutes / 60;
                NewEmployee.STATE = item.STATE;
                NewEmployee.COUNTY = item.COUNTY;
                NewEmployee.ROLE_TYPE = item.ROLE_TYPE;

                GenericData.Insert<DAILY_ACTIVITY_EMPLOYEE>(NewEmployee);

                string EmployeeName;
                string EquipmentName;
                using (Entities _context = new Entities())
                {
                    EmployeeName = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == item.PERSON_ID).Select(x => x.EMPLOYEE_NAME).Single();
                    EquipmentName = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                                     join eq in _context.CLASS_CODES_V on d.PROJECT_ID equals eq.PROJECT_ID
                                     where d.EQUIPMENT_ID == item.EQUIPMENT_ID
                                     select eq.NAME).SingleOrDefault();
                }
                ModelProxy Record = uxEmployeeStore.GetByInternalId(item.PhantomID);
                Record.CreateVariable = true;
                Record.SetId(NewEmployee.EMPLOYEE_ID);
                Record.Set("EMPLOYEE_NAME", EmployeeName);
                if (EquipmentName != null)
                {
                    Record.Set("NAME", EquipmentName);
                }
                Record.Commit();
            }

            foreach (EmployeeDetails item in data.Updated)
            {
                DAILY_ACTIVITY_EMPLOYEE UpdatedEmployee;

                using (Entities _context = new Entities())
                {
                    UpdatedEmployee = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID).Single();
                }

                UpdatedEmployee.COMMENTS = item.COMMENTS;
                UpdatedEmployee.EQUIPMENT_ID = item.EQUIPMENT_ID;
                UpdatedEmployee.FOREMAN_LICENSE = item.FOREMAN_LICENSE;
                UpdatedEmployee.MODIFIED_BY = User.Identity.Name;
                UpdatedEmployee.MODIFY_DATE = DateTime.Now;
                UpdatedEmployee.PER_DIEM = (item.PER_DIEM == true ? "Y" : "N");
                UpdatedEmployee.PERSON_ID = item.PERSON_ID;
                UpdatedEmployee.TIME_IN = item.TIME_IN.Date + item.TIME_IN_TIME.TimeOfDay;
                UpdatedEmployee.TIME_OUT = item.TIME_OUT.Date + item.TIME_OUT_TIME.TimeOfDay;

                UpdatedEmployee.TRAVEL_TIME = (decimal)item.TRAVEL_TIME_FORMATTED.TimeOfDay.TotalMinutes / 60;
                UpdatedEmployee.DRIVE_TIME = (decimal)item.DRIVE_TIME_FORMATTED.TimeOfDay.TotalMinutes / 60;
                UpdatedEmployee.SHOPTIME_AM = (decimal)item.SHOPTIME_AM_FORMATTED.TimeOfDay.TotalMinutes / 60;
                UpdatedEmployee.SHOPTIME_PM = (decimal)item.SHOPTIME_PM_FORMATTED.TimeOfDay.TotalMinutes / 60;
                UpdatedEmployee.STATE = item.STATE;
                UpdatedEmployee.COUNTY = item.COUNTY;
                UpdatedEmployee.ROLE_TYPE = item.ROLE_TYPE;

                GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(UpdatedEmployee);

                string EmployeeName;
                string EquipmentName;
                using (Entities _context = new Entities())
                {
                    EmployeeName = _context.EMPLOYEES_V.Where(x => x.PERSON_ID == item.PERSON_ID).Select(x => x.EMPLOYEE_NAME).Single();
                    EquipmentName = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                                     join eq in _context.CLASS_CODES_V on d.PROJECT_ID equals eq.PROJECT_ID
                                     where d.EQUIPMENT_ID == item.EQUIPMENT_ID
                                     select eq.NAME).SingleOrDefault();
                }

                ModelProxy Record = uxEmployeeStore.GetById(item.EMPLOYEE_ID);
                Record.CreateVariable = true;
                Record.Set("EMPLOYEE_NAME", EmployeeName);
                
                if (EquipmentName != null)
                {
                    Record.Set("NAME", EquipmentName);
                }
                Record.Commit();
            }

            uxEmployeeStore.CommitChanges();
        }

        protected void deSaveEquipment(object sender, DirectEventArgs e)
        {
            ChangeRecords<EquipmentDetails> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<EquipmentDetails>();

            foreach (EquipmentDetails item in data.Created)
            {

                DAILY_ACTIVITY_EQUIPMENT NewEquipment = new DAILY_ACTIVITY_EQUIPMENT();
                CLASS_CODES_V EquipmentItem;

                NewEquipment.PROJECT_ID = item.PROJECT_ID;
                NewEquipment.ODOMETER_START = item.ODOMETER_START;
                NewEquipment.ODOMETER_END = item.ODOMETER_END;
                NewEquipment.HEADER_ID = long.Parse(Request.QueryString["HeaderId"]);
                NewEquipment.CREATE_DATE = DateTime.Now;
                NewEquipment.MODIFY_DATE = DateTime.Now;
                NewEquipment.CREATED_BY = User.Identity.Name;
                NewEquipment.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert(NewEquipment);

                using (Entities _context = new Entities())
                {
                    EquipmentItem = _context.CLASS_CODES_V.Where(x => x.PROJECT_ID == item.PROJECT_ID).Single();
                }
                ModelProxy Record = uxEquipmentStore.GetByInternalId(item.PhantomID);
                Record.CreateVariable = true;
                Record.SetId(NewEquipment.EQUIPMENT_ID);
                Record.Set("CLASS_CODE", EquipmentItem.CLASS_CODE);
                Record.Set("NAME", EquipmentItem.NAME);
                Record.Set("ORGANIZATION_NAME", EquipmentItem.ORGANIZATION_NAME);
                Record.Set("SEGMENT1", EquipmentItem.SEGMENT1);
                Record.Commit();
            }

            foreach (EquipmentDetails item in data.Updated)
            {
                DAILY_ACTIVITY_EQUIPMENT UpdatedEquipment;
                CLASS_CODES_V EquipmentItem;

                using (Entities _context = new Entities())
                {
                    UpdatedEquipment = _context.DAILY_ACTIVITY_EQUIPMENT.Where(x => x.EQUIPMENT_ID == item.EQUIPMENT_ID).Single();
                }
                UpdatedEquipment.PROJECT_ID = item.PROJECT_ID;
                UpdatedEquipment.ODOMETER_END = item.ODOMETER_END;
                UpdatedEquipment.ODOMETER_START = item.ODOMETER_START;
                UpdatedEquipment.MODIFIED_BY = User.Identity.Name;
                UpdatedEquipment.MODIFY_DATE = DateTime.Now;

                GenericData.Update(UpdatedEquipment);

                using (Entities _context = new Entities())
                {
                    EquipmentItem = _context.CLASS_CODES_V.Where(x => x.PROJECT_ID == item.PROJECT_ID).Single();
                }

                ModelProxy Record = uxEquipmentStore.GetById(item.EQUIPMENT_ID);
                Record.CreateVariable = true;
                Record.Set("CLASS_CODE", EquipmentItem.CLASS_CODE);
                Record.Set("NAME", EquipmentItem.NAME);
                Record.Set("ORGANIZATION_NAME", EquipmentItem.ORGANIZATION_NAME);
                Record.Set("SEGMENT1", EquipmentItem.SEGMENT1);
                Record.Commit();
            }

            uxEquipmentStore.CommitChanges();
        }

        protected void deReadTaskData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                 where d.HEADER_ID == HeaderId
                                 select d.PROJECT_ID).Single();
                var data = (from t in _context.PA_TASKS_V
                            where t.PROJECT_ID == ProjectId && t.START_DATE <= DateTime.Now && (t.COMPLETION_DATE >= DateTime.Now || t.COMPLETION_DATE == null)
                            select t).ToList();

                //Set datasource for Add/Edit store
                int count;
                var pagedData = GenericData.EnumerableFilterHeader(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                uxAddProductionTaskStore.DataSource = pagedData;
                e.Total = count;
                uxAddProductionTaskStore.DataBind();
            }
        }

        protected void deSaveProduction(object sender, DirectEventArgs e)
        {
            ChangeRecords<ProductionDetails> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<ProductionDetails>();

            foreach (ProductionDetails item in data.Created)
            {
                DAILY_ACTIVITY_PRODUCTION NewProduction = new DAILY_ACTIVITY_PRODUCTION();

                NewProduction.HEADER_ID = long.Parse(Request.QueryString["HeaderId"]);
                NewProduction.QUANTITY = item.QUANTITY;
                NewProduction.BILL_RATE = item.BILL_RATE;
                NewProduction.EXPENDITURE_TYPE = item.EXPENDITURE_TYPE;
                NewProduction.STATION = item.STATION;
                NewProduction.UNIT_OF_MEASURE = item.UNIT_OF_MEASURE;
                NewProduction.SURFACE_TYPE = item.SURFACE_TYPE;
                NewProduction.TASK_ID = item.TASK_ID;
                NewProduction.COMMENTS = item.COMMENTS;
                NewProduction.WORK_AREA = item.WORK_AREA;
                NewProduction.CREATE_DATE = DateTime.Now;
                NewProduction.MODIFY_DATE = DateTime.Now;
                NewProduction.CREATED_BY = User.Identity.Name;
                NewProduction.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert(NewProduction);

                PA_TASKS_V TaskItem;
                using (Entities _context = new Entities())
                {
                    TaskItem = _context.PA_TASKS_V.Where(x => x.TASK_ID == item.TASK_ID).Single();
                }
                ModelProxy Record = uxProductionStore.GetByInternalId(item.PhantomID);
                Record.CreateVariable = true;
                Record.SetId(NewProduction.PRODUCTION_ID);
                Record.Set("DESCRIPTION", TaskItem.DESCRIPTION);
                Record.Set("TASK_NUMBER", TaskItem.TASK_NUMBER);
                Record.Commit();
            }

            foreach (ProductionDetails item in data.Updated)
            {
                DAILY_ACTIVITY_PRODUCTION UpdatedProduction;

                using (Entities _context = new Entities())
                {
                    UpdatedProduction = _context.DAILY_ACTIVITY_PRODUCTION.Where(x => x.PRODUCTION_ID == item.PRODUCTION_ID).Single();
                }
                UpdatedProduction.POLE_FROM = item.POLE_TO;
                UpdatedProduction.POLE_TO = item.POLE_TO;
                UpdatedProduction.BILL_RATE = item.BILL_RATE;
                UpdatedProduction.EXPENDITURE_TYPE = item.EXPENDITURE_TYPE;
                UpdatedProduction.STATION = item.STATION;
                UpdatedProduction.UNIT_OF_MEASURE = item.UNIT_OF_MEASURE;
                UpdatedProduction.SURFACE_TYPE = item.SURFACE_TYPE;
                UpdatedProduction.QUANTITY = item.QUANTITY;
                UpdatedProduction.ACRES_MILE = item.ACRES_MILE;
                UpdatedProduction.TASK_ID = item.TASK_ID;
                UpdatedProduction.COMMENTS = item.COMMENTS;
                UpdatedProduction.WORK_AREA = item.WORK_AREA;
                UpdatedProduction.MODIFIED_BY = User.Identity.Name;
                UpdatedProduction.MODIFY_DATE = DateTime.Now;

                GenericData.Update(UpdatedProduction);

                PA_TASKS_V TaskItem;
                using (Entities _context = new Entities())
                {
                    TaskItem = _context.PA_TASKS_V.Where(x => x.TASK_ID == item.TASK_ID).Single();
                }
                ModelProxy Record = uxProductionStore.GetById(item.PRODUCTION_ID);
                Record.CreateVariable = true;

                Record.Set("DESCRIPTION", TaskItem.DESCRIPTION);
                Record.Set("TASK_NUMBER", TaskItem.TASK_NUMBER);
                Record.Commit();
            }
            uxProductionStore.CommitChanges();
        }

        protected void deSaveWeather(object sender, DirectEventArgs e)
        {
            ChangeRecords<WeatherDetails> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<WeatherDetails>();

            foreach (WeatherDetails item in data.Created)
            {
                DAILY_ACTIVITY_WEATHER NewWeather = new DAILY_ACTIVITY_WEATHER();

                NewWeather.HEADER_ID = long.Parse(Request.QueryString["HeaderId"]);
                NewWeather.WEATHER_DATE_TIME = item.WEATHER_DATE.Date + item.WEATHER_TIME.TimeOfDay;
                NewWeather.HUMIDITY = item.HUMIDITY;
                NewWeather.TEMP = item.TEMP;
                NewWeather.WIND_DIRECTION = item.WIND_DIRECTION;
                NewWeather.WIND_VELOCITY = item.WIND_VELOCITY;
                NewWeather.COMMENTS = item.COMMENTS;
                NewWeather.CREATE_DATE = DateTime.Now;
                NewWeather.MODIFY_DATE = DateTime.Now;
                NewWeather.CREATED_BY = User.Identity.Name;
                NewWeather.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert(NewWeather);

                ModelProxy Record = uxWeatherStore.GetByInternalId(item.PhantomID);
                Record.CreateVariable = true;
                Record.SetId(NewWeather.WEATHER_ID);
                Record.Commit();
            }

            foreach (WeatherDetails item in data.Updated)
            {
                DAILY_ACTIVITY_WEATHER UpdatedWeather;

                using (Entities _context = new Entities())
                {
                    UpdatedWeather = _context.DAILY_ACTIVITY_WEATHER.Where(x => x.WEATHER_ID == item.WEATHER_ID).Single();
                }

                UpdatedWeather.HUMIDITY = item.HUMIDITY;
                UpdatedWeather.COMMENTS = item.COMMENTS;
                UpdatedWeather.TEMP = item.TEMP;
                UpdatedWeather.WEATHER_DATE_TIME = item.WEATHER_DATE.Date + item.WEATHER_TIME.TimeOfDay;
                UpdatedWeather.WIND_DIRECTION = item.WIND_DIRECTION;
                UpdatedWeather.WIND_VELOCITY = item.WIND_VELOCITY;
                UpdatedWeather.MODIFIED_BY = User.Identity.Name;
                UpdatedWeather.MODIFY_DATE = DateTime.Now;

                GenericData.Update(UpdatedWeather);
            }

            uxWeatherStore.CommitChanges();
        }

        protected void deSaveInventory(object sender, DirectEventArgs e)
        {
            ChangeRecords<InventoryDetails> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<InventoryDetails>();

            foreach (InventoryDetails item in data.Created)
            {
                DAILY_ACTIVITY_INVENTORY NewInventory = new DAILY_ACTIVITY_INVENTORY();
                NewInventory.CREATE_DATE = DateTime.Now;
                NewInventory.CREATED_BY = User.Identity.Name;
                NewInventory.HEADER_ID = long.Parse(Request.QueryString["HeaderId"]);
                NewInventory.ITEM_ID = item.ITEM_ID;
                NewInventory.MODIFIED_BY = User.Identity.Name;
                NewInventory.MODIFY_DATE = DateTime.Now;
                NewInventory.RATE = item.RATE;
                NewInventory.SUB_INVENTORY_ORG_ID = item.SUB_INVENTORY_ORG_ID;
                NewInventory.SUB_INVENTORY_SECONDARY_NAME = e.ExtraParams["SecondaryInvName"];
                NewInventory.UNIT_OF_MEASURE = item.UOM_CODE;

                string unit;
                string InvName;
                long SubOrg = long.Parse(item.SUB_INVENTORY_ORG_ID.ToString());
                INVENTORY_V Item;
                using (Entities _context = new Entities())
                {
                    Item = _context.INVENTORY_V.Where(x => x.ITEM_ID == item.ITEM_ID && x.ORGANIZATION_ID == item.SUB_INVENTORY_ORG_ID).Single();
                    unit = _context.UNIT_OF_MEASURE_V.Where(x => x.UOM_CODE == item.UOM_CODE).Select(x => x.UNIT_OF_MEASURE).Single();
                    InvName = _context.INVENTORY_V.Where(x => x.ORGANIZATION_ID == SubOrg).Select(x => x.INV_NAME).Distinct().Single();
                }
                GenericData.Insert<DAILY_ACTIVITY_INVENTORY>(NewInventory);

                ModelProxy Record = uxInventoryStore.GetByInternalId(item.PhantomID);
                Record.CreateVariable = true;
                Record.SetId(NewInventory.INVENTORY_ID);
                Record.Set("SEGMENT1", Item.SEGMENT1);
                Record.Set("DESCRIPTION", Item.DESCRIPTION);
                Record.Set("UNIT_OF_MEASURE", unit);
                Record.Set("INV_NAME", InvName);
                Record.Commit();

            }

            foreach (InventoryDetails item in data.Updated)
            {
                DAILY_ACTIVITY_INVENTORY UpdatedInventory;

                using (Entities _context = new Entities())
                {
                    UpdatedInventory = _context.DAILY_ACTIVITY_INVENTORY.Where(x => x.INVENTORY_ID == item.INVENTORY_ID).Single();
                }

                UpdatedInventory.ITEM_ID = item.ITEM_ID;
                UpdatedInventory.MODIFIED_BY = User.Identity.Name;
                UpdatedInventory.MODIFY_DATE = DateTime.Now;
                UpdatedInventory.RATE = item.RATE;
                UpdatedInventory.SUB_INVENTORY_ORG_ID = item.SUB_INVENTORY_ORG_ID;
                UpdatedInventory.SUB_INVENTORY_SECONDARY_NAME = item.SUB_INVENTORY_SECONDARY_NAME;
                UpdatedInventory.UNIT_OF_MEASURE = item.UNIT_OF_MEASURE;

                string uomCode;
                INVENTORY_V Item;
                string unit;
                string InvName;
                long SubOrg = long.Parse(item.SUB_INVENTORY_ORG_ID.ToString());
                using (Entities _context = new Entities())
                {
                    Item = _context.INVENTORY_V.Where(x => x.ITEM_ID == item.ITEM_ID && x.ORGANIZATION_ID == item.SUB_INVENTORY_ORG_ID).Single();
                    unit = _context.UNIT_OF_MEASURE_V.Where(x => x.UOM_CODE == item.UOM_CODE).Select(x => x.UNIT_OF_MEASURE).Single();
                    InvName = _context.INVENTORY_V.Where(x => x.ORGANIZATION_ID == SubOrg).Select(x => x.INV_NAME).Distinct().Single();
                }
                GenericData.Update(UpdatedInventory);

                ModelProxy Record = uxInventoryStore.GetById(item.INVENTORY_ID);
                Record.CreateVariable = true;
                Record.Set("SEGMENT1", Item.SEGMENT1);
                Record.Set("DESCRIPTION", Item.DESCRIPTION);
                Record.Set("UNIT_OF_MEASURE", unit);
                Record.Set("INV_NAME", InvName);
                Record.Commit();
            }

            uxInventoryStore.CommitChanges();
        }

        protected void deStoreEquipmentGridValue(object sender, DirectEventArgs e)
        {
            //Set value and text for equipment
            uxAddEquipmentDropDown.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["EquipmentName"]);

            //Clear existing filters
            uxAddEquipmentFilter.ClearFilter();
        }

        protected void deReloadEquipmentStore(object sender, DirectEventArgs e)
        {
            uxAddEquipmentDropDownStore.Reload();
            if (uxAddEquipmentToggleOrg.Pressed)
            {
                uxAddEquipmentToggleOrg.Text = "My Region";
            }
            else
            {
                uxAddEquipmentToggleOrg.Text = "All Regions";
            }
        }

        protected void deReadEquipmentGrid(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_EQUIPMENT_V> dataIn;

            if (uxAddEquipmentToggleOrg.Pressed)
            {
                //Get All Projects
                dataIn = WEB_EQUIPMENT_V.ListEquipment();
            }
            else
            {
                int CurrentOrg = Convert.ToInt32(Authentication.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get projects for my org only
                dataIn = WEB_EQUIPMENT_V.ListEquipment(CurrentOrg);
            }

            int count;

            //Get paged, filterable list of Equipment
            List<WEB_EQUIPMENT_V> data = GenericData.EnumerableFilterHeader<WEB_EQUIPMENT_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxAddEquipmentDropDownStore.DataSource = data;
        }

        protected void deReadExpenditures(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                long ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.HEADER_ID == HeaderId
                                  select (long)d.PROJECT_ID).Single();

                List<EXPENDITURE_TYPE_V> dataIn = (from d in _context.EXPENDITURE_TYPE_V
                                                   where d.PROJECT_ID == ProjectId
                                                   select d).ToList();
                int count;
                List<EXPENDITURE_TYPE_V> data = GenericData.EnumerableFilterHeader<EXPENDITURE_TYPE_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();
                e.Total = count;

                uxAddProductionExpenditureStore.DataSource = data;
            }
        }

        protected void deStoreExpenditureType(object sender, DirectEventArgs e)
        {
            uxAddProductionExpenditureType.SetValue(e.ExtraParams["ExpenditureType"]);
            uxAddProductionUOM.SetValue(e.ExtraParams["UnitOfMeasure"]);
            uxAddProductionBillRate.SetValue(e.ExtraParams["BillRate"]);

            uxAddProductionExpenditureStore.ClearFilter();
        }

        /// <summary>
        /// Load SubInventories for selected region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadSubinventory(object sender, DirectEventArgs e)
        {
            if ((sender is Ext.Net.RowEditing && int.Parse(e.ExtraParams["InventoryId"]) != 0) || sender is Ext.Net.ComboBox)
            {
                decimal OrgId;
                OrgId = decimal.Parse(e.ExtraParams["value"]);
                GetSubInventory(OrgId);
            }

            if (sender is Ext.Net.RowEditing && int.Parse(e.ExtraParams["InventoryId"]) != 0)
            {
                //uxAddInventorySub.SetRawValue(e.ExtraParams["SubName"]);
                uxAddInventorySub.SelectedItems.Add(new Ext.Net.ListItem(e.ExtraParams["SubName"], e.ExtraParams["SubName"]));
                uxAddInventorySub.UpdateSelectedItems();

                GetUnitOfMeasure(e.ExtraParams["uom"]);

                uxAddInventoryMeasure.SelectedItems.Add(new Ext.Net.ListItem(e.ExtraParams["uom"], e.ExtraParams["uom"]));
                uxAddInventoryMeasure.UpdateSelectedItems();
            }
        }

        /// <summary>
        /// Gets the project org ID of the Header's Project
        /// </summary>
        /// <param name="HeaderId"></param>
        protected long? GetProjectOrg(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                long? ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                   where d.HEADER_ID == HeaderId
                                   select d.PROJECT_ID).Single();
                long? OrgId = (from d in _context.PROJECTS_V
                               where d.PROJECT_ID == ProjectId
                               select d.ORG_ID).Single();
                return OrgId;
            }
        }

        protected void GetInventoryDropDown()
        {
            //Get inventory regions from db and set datasource for either add or edit
            using (Entities _context = new Entities())
            {
                string ProjectOrg = GetProjectOrg(long.Parse(Request.QueryString["HeaderId"])).ToString();
                var data = (from d in _context.INVENTORY_V
                            where d.LE == ProjectOrg
                            select new { d.ORGANIZATION_ID, d.INV_NAME }).Distinct().OrderBy(x => x.INV_NAME).ToList();

                uxAddInventoryRegionStore.DataSource = data;
                uxAddInventoryRegionStore.DataBind();
            }
        }

        protected void GetSubInventory(decimal OrgId)
        {
            //Get list of subinventories
            using (Entities _context = new Entities())
            {
                var data = (from s in _context.SUBINVENTORY_V
                            orderby s.SECONDARY_INV_NAME ascending
                            where s.ORG_ID == OrgId
                            select s).ToList();

                //uxAddInventorySub.Clear();
                //uxAddInventoryItem.Clear();
                uxAddInventorySubStore.DataSource = data;
                uxAddInventorySubStore.DataBind();
            }
        }
        /// <summary>
        /// Updates selection of Items from Add/Edit Forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreItemGridValue(object sender, DirectEventArgs e)
        {
            uxAddInventoryItem.SetValue(e.ExtraParams["ItemId"], e.ExtraParams["Description"]);
            uxAddInventoryItemStore.ClearFilter();

            uxAddInventoryItemSegment.Value = e.ExtraParams["Segment1"];
        }

        /// <summary>
        /// Get List of Inventory Items for OrgId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadItems(object sender, StoreReadDataEventArgs e)
        {
            long OrgId;
            List<INVENTORY_V> dataIn;
            OrgId = long.Parse(e.Parameters["OrgId"]);
            dataIn = INVENTORY_V.GetActiveInventory(OrgId);

            int count;

            //Get paged, filterable list of inventory
            List<INVENTORY_V> data = GenericData.EnumerableFilterHeader<INVENTORY_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();
            uxAddInventoryItemStore.DataSource = data;
            uxAddInventoryItemStore.DataBind();
            e.Total = count;
        }

        protected void deGetUnitOfMeasure(object sender, DirectEventArgs e)
        {
            GetUnitOfMeasure(e.ExtraParams["uomCode"]);
        }

        /// <summary>
        /// Gets Units of Measure from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetUnitOfMeasure(string uomCode)
        {
            //Query Db for units of measure based on uom_code
            using (Entities _context = new Entities())
            {
                List<UNIT_OF_MEASURE_V> data;
                var uomClass = (from u in _context.UNIT_OF_MEASURE_V
                                where u.UOM_CODE == uomCode
                                select u.UOM_CLASS).Single().ToString();

                data = (from u in _context.UNIT_OF_MEASURE_V
                        where u.UOM_CLASS == uomClass
                        select u).ToList();

                //Set datasource for store add/edit
                uxAddInventoryMeasureStore.DataSource = data;
                uxAddInventoryMeasureStore.DataBind();
            }
        }

        [DirectMethod]
        public void dmDeleteEmployee(string EmployeeId)
        {
            DAILY_ACTIVITY_EMPLOYEE DeletedEmployee;
            long EmpId = long.Parse(EmployeeId);
            using (Entities _context = new Entities())
            {
                DeletedEmployee = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmpId).Single();
            }

            GenericData.Delete<DAILY_ACTIVITY_EMPLOYEE>(DeletedEmployee);
            uxEmployeeGrid.GetView();

        }

        [DirectMethod]
        public void dmDeleteEquipment(string EquipmentId)
        {
            List<DAILY_ACTIVITY_EMPLOYEE> EmployeeCheck;
            DAILY_ACTIVITY_EQUIPMENT DeletedEquipment;
            long EqId = long.Parse(EquipmentId);
            using (Entities _context = new Entities())
            {
                EmployeeCheck = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EQUIPMENT_ID == EqId).ToList();
                DeletedEquipment = _context.DAILY_ACTIVITY_EQUIPMENT.Where(x => x.EQUIPMENT_ID == EqId).Single();
            }

            if (EmployeeCheck.Count == 0)
            {
                GenericData.Delete(DeletedEquipment);
                uxEquipmentGrid.GetView();
            }
            else
            {
                X.Msg.Alert("Error", "Cannot delete Equipment entry as there's currently an Employee entry using the equipment").Show();
            }

        }

        [DirectMethod]
        public void dmDeleteProduction(string ProductionId)
        {
            DAILY_ACTIVITY_PRODUCTION DeletedProduction;
            long ProdId = long.Parse(ProductionId);
            using (Entities _context = new Entities())
            {
                DeletedProduction = _context.DAILY_ACTIVITY_PRODUCTION.Where(x => x.PRODUCTION_ID == ProdId).Single();
            }

            GenericData.Delete(DeletedProduction);
            uxProductionGrid.GetView();
        }

        [DirectMethod]
        public void dmDeleteWeather(string WeatherId)
        {
            DAILY_ACTIVITY_WEATHER DeletedWeather;
            long Weather = long.Parse(WeatherId);
            using (Entities _context = new Entities())
            {
                DeletedWeather = _context.DAILY_ACTIVITY_WEATHER.Where(x => x.WEATHER_ID == Weather).Single();
            }
            GenericData.Delete(DeletedWeather);
            uxWeatherGrid.GetView();
        }

        [DirectMethod]
        public void dmDeleteChemical(string ChemicalId)
        {
            long ChemId = long.Parse(ChemicalId);
            //check for existing inven
            DAILY_ACTIVITY_CHEMICAL_MIX data;


            //Get record to be deleted.
            using (Entities _context = new Entities())
            {
                data = _context.DAILY_ACTIVITY_CHEMICAL_MIX.Include("DAILY_ACTIVITY_INVENTORY").Where(x => x.CHEMICAL_MIX_ID == ChemId).Single();

            }
            if (data.DAILY_ACTIVITY_INVENTORY.Count == 0)
            {
                //Log Mix #
                long DeletedMix = data.CHEMICAL_MIX_NUMBER;

                //Delete from db
                GenericData.Delete<DAILY_ACTIVITY_CHEMICAL_MIX>(data);

                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                //Get all records from this header where mix# is greater than the one that was deleted
                using (Entities _context = new Entities())
                {
                    var Updates = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                   where d.CHEMICAL_MIX_NUMBER > DeletedMix && d.HEADER_ID == HeaderId
                                   select d).ToList();

                    //Loop through and update db
                    foreach (var ToUpdate in Updates)
                    {
                        ToUpdate.CHEMICAL_MIX_NUMBER = ToUpdate.CHEMICAL_MIX_NUMBER - 1;
                        _context.SaveChanges();
                    }

                }
                X.Js.Call("parent.App.uxDetailsPanel.reload()");
            }
            else
            {
                X.Msg.Alert("Error", "You must first delete the associated inventory entries before deleting this item").Show();
            }
        }

        [DirectMethod]
        public void dmDeleteInventory(string InventoryId)
        {
            DAILY_ACTIVITY_INVENTORY DeletedInventory;
            long Inventory = long.Parse(InventoryId);
            using (Entities _context = new Entities())
            {
                DeletedInventory = _context.DAILY_ACTIVITY_INVENTORY.Where(x => x.INVENTORY_ID == Inventory).Single();
            }
            GenericData.Delete(DeletedInventory);
            uxInventoryGrid.GetView();
        }
    }
}