using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;


namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umEmployeesTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            GetGridData();
            if (!X.IsAjaxRequest)
            {
                uxAddEmployeeTimeInDate.SelectedDate = DateTime.Now.Date;
                uxAddEmployeeTimeOutDate.SelectedDate = DateTime.Now.Date;
            }
        }

        /// <summary>
        /// Get Current Employee Data
        /// </summary>
        protected void GetGridData()
        {
            //Get Employee data and set datasource
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
                            select new { d.EMPLOYEE_ID, d.HEADER_ID, d.PERSON_ID, e.EMPLOYEE_NAME, d.EQUIPMENT_ID, projects.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS }).ToList();
                          
                uxCurrentEmployeeStore.DataSource = data;
            }
        }

        /// <summary>
        /// Preload Employee form for editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditEmployeeForm(object sender, DirectEventArgs e)
        {            
            //JSON Decode Row and assign to variables
            string JsonValues = e.ExtraParams["EmployeeInfo"];
            Dictionary<string, string>[] EmployeeInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);

            //Populate form with existing data
            foreach (Dictionary<string, string> Employee in EmployeeInfo)
            {
                DateTime TimeIn = DateTime.Parse(Employee["TIME_IN"]);
                DateTime TimeOut = DateTime.Parse(Employee["TIME_OUT"]);
                
                uxEditEmployeeEmpDropDown.SetValue(Employee["PERSON_ID"], Employee["EMPLOYEE_NAME"]);
                uxEditEmployeeEqDropDown.SetValue(Employee["EQUIPMENT_ID"], Employee["NAME"]);
                uxEditEmployeeTimeInDate.SetValue(TimeIn.Date);
                uxEditEmployeeTimeInTime.SetValue(TimeIn.TimeOfDay);
                uxEditEmployeeTimeOutDate.SetValue(TimeOut.Date);
                uxEditEmployeeTimeOutTime.SetValue(TimeOut.TimeOfDay);
                uxEditEmployeeComments.SetValue(Employee["COMMENTS"]);
                uxEditEmployeeDriveTime.SetValue(Employee["DRIVE_TIME"]);
                uxEditEmployeeTravelTime.SetValue(Employee["TRAVEL_TIME"]);
                if (Employee["PER_DIEM"] == "Y")
                {
                    uxEditEmployeePerDiem.Checked = true;
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
            uxCurrentEmployeeStore.Reload();
        }

        /// <summary>
        /// Get List of employees from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadEmployeeData(object sender, StoreReadDataEventArgs e)
        {
            List<EMPLOYEES_V> dataIn;
            if(e.Parameters["Form"] == "EmployeeAdd"){
                if (uxAddEmployeeRegion.Pressed)
                {
                    //Get All Projects
                    dataIn = EMPLOYEES_V.EmployeeDropDown();
                }
                else
                {
                    var MyAuth = new Authentication();
                    int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                    //Get projects for my org only
                    dataIn = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
                }
            }
            else{
                if (uxEditEmployeeEmpRegion.Pressed)
                {
                    //Get All Projects
                    dataIn = EMPLOYEES_V.EmployeeDropDown();
                }
                else
                {
                    var MyAuth = new Authentication();
                    int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                    //Get projects for my org only
                    dataIn = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
                }
            }

            int count;

            //Get paged,filterable list of Employees
            List<EMPLOYEES_V> data = GenericData.EnumerableFilterHeader<EMPLOYEES_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            if (e.Parameters["Form"] == "EmployeeAdd")
            {
                uxAddEmployeeEmpStore.DataSource = data;
                uxAddEmployeeEmpStore.DataBind();
            }
            else
            {
                uxEditEmployeeEmpStore.DataSource = data;
                uxEditEmployeeEmpStore.DataBind();
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
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new {d.EQUIPMENT_ID, p.NAME, d.PROJECT_ID }).ToList();
                if (e.Parameters["Form"] == "EquipmentAdd")
                {
                    //Set add store
                    uxAddEmployeeEqStore.DataSource = data;
                    uxAddEmployeeEqStore.DataBind();
                }
                else
                {
                    //Set edit store
                    uxEditEmployeeEqStore.DataSource = data;
                    uxEditEmployeeEqStore.DataBind();
                }
            }
            
        }
           
        /// <summary>
        /// Toggle Region text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRegionToggle(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"]){
                case "EmployeeAdd":
                    if (uxAddEmployeeRegion.Pressed)
                    {
                        uxAddEmployeeRegion.Text = "My Region";
                        uxAddEmployeeEmpStore.Reload();
                    }
                    else
                    {
                        uxAddEmployeeRegion.Text = "All Regions";
                        uxAddEmployeeEmpStore.Reload();
                    }
                    break;
                case "EmployeeEdit":
                    if (uxEditEmployeeEmpRegion.Pressed)
                    {
                        uxEditEmployeeEmpStore.Reload();
                        uxEditEmployeeEmpRegion.Text = "My Region";
                    }
                    else
                    {
                        uxEditEmployeeEmpStore.Reload();
                        uxEditEmployeeEmpRegion.Text = "All Regions";
                    }
                    break;
            }
        }

        /// <summary>
        /// Update selected item of what's chosen from Gridpanel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "EmployeeAdd":
                    uxAddEmployeeEmpDropDown.SetValue(e.ExtraParams["PersonId"], e.ExtraParams["Name"]);
                    uxAddEmployeeEmpFilter.ClearFilter();
                    break;
                case "EmployeeEdit":
                    uxEditEmployeeEmpDropDown.SetValue(e.ExtraParams["PersonId"], e.ExtraParams["Name"]);
                    uxEditEmployeeEmpFilter.ClearFilter();
                    break;
                case "EquipmentAdd":
                    uxAddEmployeeEqDropDown.SetValue(e.ExtraParams["EquipmentId"], e.ExtraParams["Name"]);
                    break;
                case "EquipmentEdit":
                    uxEditEmployeeEqDropDown.SetValue(e.ExtraParams["EquipmentId"], e.ExtraParams["Name"]);
                    break;
            }
        }
        
        /// <summary>
        /// Add Employee to Db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddEmployee(object sender, DirectEventArgs e)
        {
            //Convert to correct types
            int PersonId = int.Parse(uxAddEmployeeEmpDropDown.Value.ToString());
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);


            //Combine Date/Time for TimeIn/Out
            DateTime TimeIn = DateTime.Parse(uxAddEmployeeTimeInDate.Value.ToString());
            DateTime TimeInTime = DateTime.Parse(uxAddEmployeeTimeInTime.Value.ToString());
            DateTime TimeOut = DateTime.Parse(uxAddEmployeeTimeOutDate.Value.ToString());
            DateTime TimeOutTime = DateTime.Parse(uxAddEmployeeTimeOutTime.Value.ToString());

            TimeIn = TimeIn + TimeInTime.TimeOfDay;
            TimeOut = TimeOut + TimeOutTime.TimeOfDay;

            //Convert PerDiem to string
            string PerDiem;
            if (uxAddEmployeePerDiem.Checked)
            {
                PerDiem = "Y";
            }
            else
            {
                PerDiem = "N";
            }

            
            DAILY_ACTIVITY_EMPLOYEE data = new DAILY_ACTIVITY_EMPLOYEE()
            {
                HEADER_ID = HeaderId,
                PERSON_ID = PersonId,
                TIME_IN = TimeIn,
                TIME_OUT = TimeOut,
                PER_DIEM = PerDiem,
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name
            };
            
            //Check for travel time
            try
            {
                decimal TravelTime = decimal.Parse(uxAddEmployeeTravelTime.Value.ToString());
                data.TRAVEL_TIME = TravelTime;
            }
            catch (NullReferenceException)
            {
                data.TRAVEL_TIME = null;
            }

            //Check for drive time
            try
            {
                decimal DriveTime= decimal.Parse(uxAddEmployeeDriveTime.Value.ToString());
                data.DRIVE_TIME = DriveTime;
            }
            catch (NullReferenceException)
            {
                data.DRIVE_TIME = null;
            }

            //Check for comments
            try
            {
                string Comments = uxAddEmployeeComments.Value.ToString();;
                data.COMMENTS = Comments;
            }
            catch (NullReferenceException)
            {
                data.COMMENTS = null;
            }

            //Check for Equipment
            try
            {
                long EquipmentId = long.Parse(uxAddEmployeeEqDropDown.Value.ToString());
                data.EQUIPMENT_ID = EquipmentId;
            }
            catch (FormatException)
            {
                data.EQUIPMENT_ID = null;
            }

            //Write to DB
            GenericData.Insert<DAILY_ACTIVITY_EMPLOYEE>(data);

            uxAddEmployeeWindow.Hide();
            uxAddEmployeeForm.Reset();
            uxCurrentEmployeeStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Employee Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Store Edit Employee to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditEmployee(object sender, DirectEventArgs e)
        {
            //Convert to correct types
            int PersonId = int.Parse(uxEditEmployeeEmpDropDown.Value.ToString());            

            //Combine Date/Time for TimeIn/Out
            DateTime TimeIn = DateTime.Parse(uxEditEmployeeTimeInDate.Value.ToString());
            DateTime TimeInTime = DateTime.Parse(uxEditEmployeeTimeInTime.Value.ToString());
            DateTime TimeOut = DateTime.Parse(uxEditEmployeeTimeOutDate.Value.ToString());
            DateTime TimeOutTime = DateTime.Parse(uxEditEmployeeTimeOutTime.Value.ToString());

            TimeIn = TimeIn + TimeInTime.TimeOfDay;
            TimeOut = TimeOut + TimeOutTime.TimeOfDay;

            //Convert PerDiem to string
            string PerDiem;
            if (uxEditEmployeePerDiem.Checked)
            {
                PerDiem = "Y";
            }
            else
            {
                PerDiem = "N";
            }
            DAILY_ACTIVITY_EMPLOYEE data;
            long EmployeeId = long.Parse(e.ExtraParams["EmployeeID"]);
            
            //Get record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                        where d.EMPLOYEE_ID == EmployeeId
                        select d).Single();
            }
            data.PERSON_ID = PersonId;

            //Check for Equipment
            try
            {
                long EquipmentId = long.Parse(uxEditEmployeeEqDropDown.Value.ToString());
                data.EQUIPMENT_ID = EquipmentId;
            }
            catch (NullReferenceException)
            {
                data.EQUIPMENT_ID = null;
            }

            //Check for Travel Time
            try
            {
                decimal TravelTime = decimal.Parse(uxEditEmployeeTravelTime.Value.ToString());
                data.TRAVEL_TIME = TravelTime;
            }
            catch (NullReferenceException)
            {
                data.TRAVEL_TIME = null;
            }

            //Check for Drive Time
            try
            {
                decimal DriveTime = decimal.Parse(uxEditEmployeeDriveTime.Value.ToString());
                data.DRIVE_TIME = DriveTime;
            }
            catch (NullReferenceException)
            {
                data.DRIVE_TIME = null;
            }

            try
            {
                data.COMMENTS = uxEditEmployeeComments.Value.ToString();
            }
            catch (NullReferenceException)
            {
                data.COMMENTS = null;
            }
            data.TIME_IN = TimeIn;
            data.TIME_OUT = TimeOut;
            data.PER_DIEM = PerDiem;
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            //Write to db
            GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(data);

            uxCurrentEmployeeStore.Reload();
            uxEditEmployeeWindow.Hide();
            X.Js.Call("parent.App.uxManageGridStore.reload()");
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Employee Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
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
                    if (e.ExtraParams["Form"] == "Add")
                    {
                        uxAddEmployeePerDiem.Disabled = true;
                    }
                    else
                    {
                        uxEditEmployeePerDiem.Disabled = true;
                    }
                }
            }
        }
    }
}