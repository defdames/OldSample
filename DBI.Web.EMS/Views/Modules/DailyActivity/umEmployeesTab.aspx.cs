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

        protected void deReadEmployeeData(object sender, StoreReadDataEventArgs e)
        {
            List<EMPLOYEES_V> data;
            if(e.Parameters["Form"] == "EmployeeAdd"){
                if (uxAddEmployeeRegion.Pressed)
                {
                    //Get All Projects
                    data = EMPLOYEES_V.EmployeeDropDown();
                }
                else
                {
                    var MyAuth = new Authentication();
                    int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                    //Get projects for my org only
                    data = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
                }
            }
            else{
                if (uxEditEmployeeEmpRegion.Pressed)
                {
                    //Get All Projects
                    data = EMPLOYEES_V.EmployeeDropDown();
                }
                else
                {
                    var MyAuth = new Authentication();
                    int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                    //Get projects for my org only
                    data = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
                }
            }
            //-- start filtering -----------------------------------------------------------
            FilterHeaderConditions fhc = new FilterHeaderConditions(e.Parameters["filterheader"]);

            foreach (FilterHeaderCondition condition in fhc.Conditions)
            {
                string dataIndex = condition.DataIndex;
                FilterType type = condition.Type;
                string op = condition.Operator;
                object value = null;

                switch (condition.Type)
                {
                    case FilterType.Boolean:
                        value = condition.Value<bool>();
                        break;

                    case FilterType.Date:
                        switch (condition.Operator)
                        {
                            case "=":
                                value = condition.Value<DateTime>();
                                break;

                            case "compare":
                                value = FilterHeaderComparator<DateTime>.Parse(condition.JsonValue);
                                break;
                        }
                        break;

                    case FilterType.Numeric:
                        bool isInt = data.Count > 0 && data[0].GetType().GetProperty(dataIndex).PropertyType == typeof(int);
                        switch (condition.Operator)
                        {
                            case "=":
                                if (isInt)
                                {
                                    value = condition.Value<int>();
                                }
                                else
                                {
                                    value = condition.Value<double>();
                                }
                                break;

                            case "compare":
                                if (isInt)
                                {
                                    value = FilterHeaderComparator<int>.Parse(condition.JsonValue);
                                }
                                else
                                {
                                    value = FilterHeaderComparator<double>.Parse(condition.JsonValue);
                                }

                                break;
                        }

                        break;
                    case FilterType.String:
                        value = condition.Value<string>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                data.RemoveAll(item =>
                {
                    object oValue = item.GetType().GetProperty(dataIndex).GetValue(item, null);
                    string matchValue = null;
                    string itemValue = null;

                    if (type == FilterType.String)
                    {
                        matchValue = (string)value;
                        matchValue = matchValue.ToLower();
                        itemValue = oValue as string;
                        itemValue = itemValue.ToLower();
                    }
                    return itemValue == null || itemValue.IndexOf(matchValue) < 0;
                });
            }
            //-- end filtering ------------------------------------------------------------


            //-- start sorting ------------------------------------------------------------
            if (e.Sort.Length > 0)
            {
                data.Sort(delegate(EMPLOYEES_V x, EMPLOYEES_V y)
                {
                    object a;
                    object b;

                    int direction = e.Sort[0].Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                    a = x.GetType().GetProperty(e.Sort[0].Property).GetValue(x, null);
                    b = y.GetType().GetProperty(e.Sort[0].Property).GetValue(y, null);
                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }
            //-- end sorting ------------------------------------------------------------


            //-- start paging ------------------------------------------------------------
            int limit = e.Limit;

            if ((e.Start + e.Limit) > data.Count)
            {
                limit = data.Count - e.Start;
            }

            List<EMPLOYEES_V> rangeData = (e.Start < 0 || limit < 0) ? data : data.GetRange(e.Start, limit);
            //-- end paging ------------------------------------------------------------

            e.Total = data.Count;
            if (e.Parameters["Form"] == "EmployeeAdd")
            {
                uxAddEmployeeEmpStore.DataSource = rangeData;
                uxAddEmployeeEmpStore.DataBind();
            }
            else
            {
                uxEditEmployeeEmpStore.DataSource = rangeData;
                uxEditEmployeeEmpStore.DataBind();
            }
        }

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
    }
}