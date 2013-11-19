using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umHeaderTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                GetFormData();
            }
        }

        /// <summary>
        /// Get Value of current Header to prepopulate fields
        /// </summary>
        protected void GetFormData()
        {
            using (Entities _context = new Entities())
            {
                var HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            where d.HEADER_ID == HeaderId
                            select new { d, p.NAME, e.EMPLOYEE_NAME }).Single();
                uxFormProject.SetValue(data.d.PROJECT_ID.ToString(), data.NAME);
                uxFormDate.SetValue(data.d.DA_DATE);
                uxFormSubDivision.SetValue(data.d.SUBDIVISION);
                uxFormContractor.SetValue(data.d.CONTRACTOR);
                uxFormEmployee.SetValue(data.d.PERSON_ID.ToString(), data.EMPLOYEE_NAME);
                uxFormLicense.SetValue(data.d.LICENSE);
                uxFormState.SetValue(data.d.STATE);
                uxFormType.SetValue(data.d.APPLICATION_TYPE);
                uxFormDensity.SetValue(data.d.DENSITY);                
            }
        }
        
        /// <summary>
        /// Reads/Filters Project Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadData(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_PROJECTS_V> data = new List<WEB_PROJECTS_V>();
            if (uxFormProjectToggleOrg.Pressed)
            {
                //Get All Projects
                data = WEB_PROJECTS_V.ProjectList();
            }
            else
            {
                var MyAuth = new Authentication();
                int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get projects for my org only
                data = WEB_PROJECTS_V.ProjectList(CurrentOrg);
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
                        itemValue = oValue as string;
                    }

                    //switch (op)
                    //{
                    //    case "=":
                    //        return oValue == null || !oValue.Equals(value);
                    //    case "compare":
                    //        return !((IEquatable<IComparable>)value).Equals((IComparable)oValue);
                    //    case "+":
                    //        return itemValue == null || !itemValue.StartsWith(matchValue);
                    //    case "-":
                    //        return itemValue == null || !itemValue.EndsWith(matchValue);
                    //    case "!":
                    //        return itemValue == null || itemValue.IndexOf(matchValue) >= 0;
                    //    case "*":
                    return itemValue == null || itemValue.IndexOf(matchValue) < 0;
                    //    default:
                    //        throw new Exception("Not supported operator");
                    //}
                });
            }
            //-- end filtering ------------------------------------------------------------


            //-- start sorting ------------------------------------------------------------
            if (e.Sort.Length > 0)
            {
                data.Sort(delegate(WEB_PROJECTS_V x, WEB_PROJECTS_V y)
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

            List<WEB_PROJECTS_V> rangeData = (e.Start < 0 || limit < 0) ? data : data.GetRange(e.Start, limit);
            //-- end paging ------------------------------------------------------------

            e.Total = data.Count;
            uxFormProjectStore.DataSource = rangeData;
            uxFormProjectStore.DataBind();
        }

        /// <summary>
        /// Puts value into DropDownField and clears filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreValue(object sender, DirectEventArgs e)
        {
            //Set value and text
            uxFormProject.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["LongName"]);
            //Clear existing filters
            uxFormProjectFilter.ClearFilter();
        }

        /// <summary>
        /// Reads/Filters Employee Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadEmployees(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            List<EMPLOYEES_V> data = new List<EMPLOYEES_V>();
            if (uxFormEmployeeToggleOrg.Pressed)
            {
                //Get Employees for all regions
                data = EMPLOYEES_V.EmployeeDropDown();
            }
            else
            {
                var MyAuth = new Authentication();
                int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get Employees for my region only
                data = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
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
                    //switch (op)
                    //{
                    //    case "=":
                    //        return oValue == null || !oValue.Equals(value);
                    //    case "compare":
                    //        return !((IEquatable<IComparable>)value).Equals((IComparable)oValue);
                    //    case "+":
                    //        return itemValue == null || !itemValue.StartsWith(matchValue);
                    //    case "-":
                    //        return itemValue == null || !itemValue.EndsWith(matchValue);
                    //    case "!":
                    //        return itemValue == null || itemValue.IndexOf(matchValue) >= 0;
                    //    case "*":
                    //        return itemValue == null || itemValue.IndexOf(matchValue) < 0;
                    //    default:
                    //        throw new Exception("Not supported operator");
                    //}
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
            uxFormEmployeeStore.DataSource = rangeData;
            uxFormEmployeeStore.DataBind();
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
            uxFormEmployee.SetValue(e.ExtraParams["PersonID"], e.ExtraParams["EmployeeName"]);
            //Clear existing filters
            uxFormEmployeeFilter.ClearFilter();
        }
        
        /// <summary>
        /// Direct Event that stores the Daily Activity form data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 

        protected void deStoreHeader(object sender, Ext.Net.DirectEventArgs e)
        {
            //Get values in correct formats
            long ProjectId = Convert.ToInt64(uxFormProject.Value);
            DateTime DaDate = (DateTime)uxFormDate.Value;
            int PersonId = Convert.ToInt32(uxFormEmployee.Value);
            var MyAuth = new Authentication();

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
            data.SUBDIVISION = uxFormSubDivision.Value.ToString();
            data.CONTRACTOR = uxFormContractor.Value.ToString();
            data.PERSON_ID = PersonId;
            data.LICENSE = uxFormLicense.Value.ToString();
            data.STATE = uxFormState.Value.ToString();
            data.APPLICATION_TYPE = uxFormType.Value.ToString();
            data.DENSITY = uxFormDensity.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Header Updated Successfully",
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