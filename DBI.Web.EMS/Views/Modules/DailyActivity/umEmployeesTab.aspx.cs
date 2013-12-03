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
            GetGridData();
        }

        /// <summary>
        /// Get Current Employee Data
        /// </summary>
        protected void GetGridData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID
                            join p in _context.PROJECTS_V on eq.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.EMPLOYEE_ID, d.HEADER_ID, d.PERSON_ID, e.EMPLOYEE_NAME, d.EQUIPMENT_ID, p.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS }).ToList();
                          
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
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new {d.EQUIPMENT_ID, p.NAME, d.PROJECT_ID }).ToList();
                if (e.Parameters["Form"] == "EquipmentAdd")
                {
                    uxAddEmployeeEqStore.DataSource = data;
                    uxAddEmployeeEqStore.DataBind();
                }
                else
                {
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
            long EquipmentId = long.Parse(uxAddEmployeeEqDropDown.Value.ToString());
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            decimal TravelTime = decimal.Parse(uxAddEmployeeTravelTime.Value.ToString());
            decimal DriveTime = decimal.Parse(uxAddEmployeeDriveTime.Value.ToString());
            
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
                EQUIPMENT_ID = EquipmentId,
                TIME_IN = TimeIn,
                TIME_OUT = TimeOut,
                TRAVEL_TIME = TravelTime,
                DRIVE_TIME = DriveTime,
                COMMENTS = uxAddEmployeeComments.Value.ToString(),
                PER_DIEM = PerDiem,
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name
            };
            GenericData.Insert<DAILY_ACTIVITY_EMPLOYEE>(data);

            uxAddEmployeeWindow.Hide();
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
            long EquipmentId = long.Parse(uxEditEmployeeEqDropDown.Value.ToString());
            decimal TravelTime = decimal.Parse(uxEditEmployeeTravelTime.Value.ToString());
            decimal DriveTime = decimal.Parse(uxEditEmployeeDriveTime.Value.ToString());

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
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                        where d.EMPLOYEE_ID == EmployeeId
                        select d).Single();
            }
            data.PERSON_ID = PersonId;
            data.EQUIPMENT_ID = EquipmentId;
            data.TRAVEL_TIME = TravelTime;
            data.DRIVE_TIME = DriveTime;
            data.TIME_IN = TimeIn;
            data.TIME_OUT = TimeOut;
            data.PER_DIEM = PerDiem;
            data.COMMENTS = uxEditEmployeeComments.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(data);

            uxCurrentEmployeeStore.Reload();
            uxEditEmployeeWindow.Hide();
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
        /// Validate DateIn and DateOut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void valDate(object sender, RemoteValidationEventArgs e)
        {
            DateField Field = (DateField)sender;
            DateTime DateIn;
            DateTime DateOut;

            if (e.ExtraParams["Type"] == "Add")
            {
                if (e.ExtraParams["InOut"] == "Out")
                {
                    DateIn = DateTime.Parse(uxAddEmployeeTimeInDate.Value.ToString());
                    DateOut = DateTime.Parse(Field.Value.ToString());
                }
                else
                {
                    DateIn = DateTime.Parse(Field.Value.ToString());
                    DateOut = DateTime.Parse(uxAddEmployeeTimeOutTime.Value.ToString());
                }

            }
            else
            {
                if (e.ExtraParams["InOut"] == "Out")
                {
                    DateIn = DateTime.Parse(uxEditEmployeeTimeInDate.Value.ToString());
                    DateOut = DateTime.Parse(Field.Value.ToString());
                }
                else
                {
                    DateIn = DateTime.Parse(Field.Value.ToString());
                    DateOut = DateTime.Parse(uxEditEmployeeTimeOutDate.Value.ToString());
                }
            }

            if (DateOut >= DateIn)
            {
                e.Success = true;
            }
            else
            {
                e.Success = false;
                e.ErrorMessage = "Date Out must be greater than or equal to Date In";
            }
        }

        /// <summary>
        /// Validate TimeIn and TimeOut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void valTime(object sender, RemoteValidationEventArgs e)
        {
            TimeField Field = (TimeField)sender;
            DateTime TimeIn;
            DateTime TimeOut;

            DateTime DateIn;
            DateTime DateOut;
            if (e.ExtraParams["Type"] == "Add")
            {
                DateIn = DateTime.Parse(uxAddEmployeeTimeInDate.Value.ToString());
                DateOut = DateTime.Parse(uxAddEmployeeTimeOutDate.Value.ToString());

                if (e.ExtraParams["InOut"] == "Out")
                {
                    TimeIn = DateTime.Parse(uxAddEmployeeTimeInTime.Value.ToString());
                    TimeOut = DateTime.Parse(Field.Value.ToString());
                }
                else
                {
                    TimeIn = DateTime.Parse(Field.Value.ToString());
                    TimeOut = DateTime.Parse(uxAddEmployeeTimeOutTime.Value.ToString());
                }

                TimeIn = DateIn.Date + TimeIn.TimeOfDay;
                TimeOut = DateOut.Date + TimeOut.TimeOfDay;

            }
            else
            {
                DateIn = DateTime.Parse(uxEditEmployeeTimeInDate.Value.ToString());
                DateOut = DateTime.Parse(uxEditEmployeeTimeOutDate.Value.ToString());

                if (e.ExtraParams["InOut"] == "Out")
                {
                    TimeIn = DateTime.Parse(uxEditEmployeeTimeInTime.Value.ToString());
                    TimeOut = DateTime.Parse(Field.Value.ToString());
                }
                else
                {
                    TimeIn = DateTime.Parse(Field.Value.ToString());
                    TimeOut = DateTime.Parse(uxEditEmployeeTimeOutTime.Value.ToString());
                }

                TimeIn = DateIn.Date + TimeIn.TimeOfDay;
                TimeOut = DateOut.Date + TimeOut.TimeOfDay;
            }

            if (TimeOut >= TimeIn)
            {
                e.Success = true;
            }
            else
            {
                e.Success = false;
                e.ErrorMessage = "Time Out must be greater than or equal to Time In";
            }
        }
    }
}