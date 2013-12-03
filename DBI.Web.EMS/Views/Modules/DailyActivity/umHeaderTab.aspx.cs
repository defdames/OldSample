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
            List<WEB_PROJECTS_V> dataIn;
            if (uxFormProjectToggleOrg.Pressed)
            {
                //Get All Projects
                dataIn = WEB_PROJECTS_V.ProjectList();
            }
            else
            {
                var MyAuth = new Authentication();
                int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
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
            List<EMPLOYEES_V> dataIn = new List<EMPLOYEES_V>();
            if (uxFormEmployeeToggleOrg.Pressed)
            {
                //Get Employees for all regions
                dataIn = EMPLOYEES_V.EmployeeDropDown();
            }
            else
            {
                var MyAuth = new Authentication();
                int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get Employees for my region only
                dataIn = EMPLOYEES_V.EmployeeDropDown(CurrentOrg);
            }
            int count;

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
            try
            {
                data.SUBDIVISION = uxFormSubDivision.Value.ToString();
            }
            catch (NullReferenceException)
            {
                data.SUBDIVISION = "";
            }
            try
            {
                data.CONTRACTOR = uxFormContractor.Value.ToString();
            }
            catch (NullReferenceException)
            {
                data.CONTRACTOR = "";
            }
            data.PERSON_ID = PersonId;
            data.LICENSE = uxFormLicense.Value.ToString();
            data.STATE = uxFormState.Value.ToString();
            try
            {
                data.APPLICATION_TYPE = uxFormType.Value.ToString();
            }
            catch (NullReferenceException)
            {
                data.APPLICATION_TYPE = "";
            }
            try
            {
                data.DENSITY = uxFormDensity.Value.ToString();
            }
            catch (NullReferenceException)
            {
                data.DENSITY = "";
            }
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