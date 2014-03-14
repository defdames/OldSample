using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Data.DataFactory;
using DBI.Data;
using Ext.Net;
using System.Text;
using System.Collections;
using DBI.Core.Security;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umDailyActivity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }

            if (!X.IsAjaxRequest)
            {
                uxStateList.Data = StaticLists.StateList;
                uxFormDate.SelectedDate = DateTime.Now.Date;
            }
        }

        /// <summary>
        /// Reads/Filters Project Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadData(object sender, StoreReadDataEventArgs e)
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
            //Get paged, filterable list of data
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
            List<EMPLOYEES_V> dataIn;
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
            //Get paged, filterable list of Employees
            List<EMPLOYEES_V> data = GenericData.EnumerableFilterHeader<EMPLOYEES_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxFormEmployeeStore.DataSource = data;
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
            var icp = User as ClaimsPrincipal;
            var AddingUser = Authentication.GetClaimValue(ClaimTypes.Name, icp);
            
            string SubDivision;
            string HeaderType;
            string Contractor;
            string Density;

            
            //Create new Daily Activity Header
            DAILY_ACTIVITY_HEADER ToStore = new DAILY_ACTIVITY_HEADER()
            {
                PROJECT_ID = ProjectId,
                DA_DATE = DaDate,
                PERSON_ID = PersonId,
                LICENSE = uxFormLicense.Value.ToString(),
                STATE = uxFormState.Value.ToString(),
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = AddingUser,
                MODIFIED_BY = AddingUser,
                STATUS = 3,
                DA_HEADER_ID = 0
            };

            //check for SubDivision value
            try
            {
                SubDivision = uxFormSubDivision.Value.ToString();
                ToStore.SUBDIVISION = SubDivision;
            }
            catch (NullReferenceException)
            {
                ToStore.SUBDIVISION = null;
            }

            //check for Type value
            try
            {
                HeaderType = uxFormType.Value.ToString();
                ToStore.APPLICATION_TYPE = HeaderType;
            }
            catch (NullReferenceException)
            {
                ToStore.APPLICATION_TYPE = null;
            }

            //check for Contractor
            try
            {
                Contractor = uxFormContractor.Value.ToString();
                ToStore.CONTRACTOR = Contractor;
            }
            catch (NullReferenceException)
            {
                ToStore.CONTRACTOR = null;
            }

            //check for Density
            try
            {
                Density = uxFormDensity.Value.ToString();
                ToStore.DENSITY = Density;
            }
            catch (NullReferenceException)
            {
                ToStore.DENSITY = null;
            }
            //Write to the DB
            GenericData.Insert<DAILY_ACTIVITY_HEADER>(ToStore);
            X.Js.Call("parent.App.direct.dmHideAddWindow()");
        }
    }
}