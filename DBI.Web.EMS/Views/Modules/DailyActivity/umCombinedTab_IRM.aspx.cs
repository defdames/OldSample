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

                uxStateList.Data = StaticLists.StateList;
                uxStateStore.Data = StaticLists.StateList;
                this.uxRedWarning.Value = ResourceManager.GetInstance().GetIconUrl(Ext.Net.Icon.Exclamation);
                this.uxYellowWarning.Value = ResourceManager.GetInstance().GetIconUrl(Ext.Net.Icon.Error);

                if (GetStatus(long.Parse(Request.QueryString["HeaderId"])) != 2)
                {
                    uxEmployeeToolbar.Hide();
                    uxEquipmentToolbar.Hide();
                    uxProductionToolbar.Hide();
                    uxWeatherToolbar.Hide();
                    uxInventoryToolbar.Hide();

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
                else
                {
                    X.Js.Call("showButtons");
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
                            X.Js.Call("parent.App.uxApproveActivityButton.enable()");
                            break;
                        case 3:
                            X.Js.Call("parent.App.uxPostActivityButton.enable()");
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
                        X.Js.Call("parent.App.uxApproveActivityButton.enable()");
                        break;
                    case 3:
                        X.Js.Call("parent.App.uxPostActivityButton.enable()");
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
                            select new {d.HEADER_ID, d.PROJECT_ID, p.SEGMENT1, p.LONG_NAME, d.DA_DATE, d.SUBDIVISION, d.CONTRACTOR, d.PERSON_ID, e.EMPLOYEE_NAME, d.LICENSE, d.STATE, d.APPLICATION_TYPE, d.DENSITY, d.DA_HEADER_ID }).Single();
                DateTime Da_date = DateTime.Parse(data.DA_DATE.ToString());
                uxProjectField.SetValue(data.PROJECT_ID.ToString(), data.LONG_NAME);
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
                            select new EmployeeDetails{PERSON_ID = e.PERSON_ID, EMPLOYEE_ID = d.EMPLOYEE_ID,EMPLOYEE_NAME = e.EMPLOYEE_NAME, FOREMAN_LICENSE = d.FOREMAN_LICENSE, NAME = projects.NAME,TIME_IN =  (DateTime)d.TIME_IN, TIME_OUT =  (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), PER_DIEM = d.PER_DIEM, COMMENTS = d.COMMENTS, ROLE_TYPE = d.ROLE_TYPE }).ToList();

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

                        X.Js.Call("disableOnError");
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
                        X.Js.Call("disableOnError");
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
                        X.Js.Call("disableOnError");
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
                            select new {d.INVENTORY_ID, j.INV_NAME, j.SEGMENT1, d.SUB_INVENTORY_SECONDARY_NAME, j.DESCRIPTION, d.RATE, j.UOM_CODE, d.UNIT_OF_MEASURE }).ToList();
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
        /// Remove Employee entry from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveEmployee(object sender, DirectEventArgs e)
        {

            long EmployeeId = long.Parse(e.ExtraParams["EmployeeID"]);
            //Get Record to Remove
            DAILY_ACTIVITY_EMPLOYEE data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                        where d.EMPLOYEE_ID == EmployeeId
                        select d).Single();
            }
            GenericData.Delete<DAILY_ACTIVITY_EMPLOYEE>(data);
            X.Js.Call("parent.App.uxDetailsPanel.reload()");
        }

        /// <summary>
        /// Remove equipment from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveEquipment(object sender, DirectEventArgs e)
        {
            //Convert EquipmentId to long
            long EquipmentId = long.Parse(e.ExtraParams["EquipmentId"]);
            DAILY_ACTIVITY_EQUIPMENT data;

            //Get record to be deleted
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                        where d.EQUIPMENT_ID == EquipmentId
                        select d).Single();
            }

            GenericData.Delete<DAILY_ACTIVITY_EQUIPMENT>(data);
            X.Js.Call("parent.App.uxDetailsPanel.reload()");
        }

        /// <summary>
        /// Remove production item from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveProduction(object sender, DirectEventArgs e)
        {
            long ProductionId = long.Parse(e.ExtraParams["ProductionId"]);
            DAILY_ACTIVITY_PRODUCTION data;

            //Get record to be deleted
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }

            //Process deletion
            GenericData.Delete<DAILY_ACTIVITY_PRODUCTION>(data);
            X.Js.Call("parent.App.uxDetailsPanel.reload()");
        }

        /// <summary>
        /// Remove weather from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveWeather(object sender, DirectEventArgs e)
        {
            long WeatherId = long.Parse(e.ExtraParams["WeatherId"]);
            DAILY_ACTIVITY_WEATHER data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_WEATHER
                        where d.WEATHER_ID == WeatherId
                        select d).Single();
            }
            GenericData.Delete<DAILY_ACTIVITY_WEATHER>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload()");
        }

        /// <summary>
        /// Remove inventory entry from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveInventory(object sender, DirectEventArgs e)
        {
            long InventoryId = long.Parse(e.ExtraParams["InventoryId"]);
            DAILY_ACTIVITY_INVENTORY data;

            //Get record to be deleted
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                        where d.INVENTORY_ID == InventoryId
                        select d).Single();
            }

            //Delete from DB
            GenericData.Delete<DAILY_ACTIVITY_INVENTORY>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload()");
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
            uxProjectField.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["LongName"]);
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
    }
}