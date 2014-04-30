using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umChooseSupportProject : System.Web.UI.Page
    {
        private List<long> ComboBoxes = new List<long>();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void deReadSupportProjects(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var ProjectList = (from p in _context.PROJECTS_V
                                   where p.PROJECT_TYPE == "SUPPORT OVERHEAD" && p.TEMPLATE_FLAG == "N" && p.PROJECT_STATUS_CODE == "APPROVED"
                                   select new { p.PROJECT_ID, p.LONG_NAME }).ToList();
                uxSupportProjectStore.DataSource = ProjectList;
            }
        }

        protected void deSupportProjectChoice(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            foreach (long EmployeeId in ComboBoxes)
            {
                ComboBox SupportBox = FindControl("Combo" +EmployeeId.ToString()) as ComboBox;
                DAILY_ACTIVITY_EMPLOYEE EmployeeToUpdate;

                long ProjectID = long.Parse(SupportBox.Value.ToString());
                using (Entities _context = new Entities())
                {
                    EmployeeToUpdate = _context.DAILY_ACTIVITY_EMPLOYEE.Where(emp => emp.EMPLOYEE_ID == EmployeeId).SingleOrDefault();
                    EmployeeToUpdate.SUPPORT_PROJ_ID = long.Parse(SupportBox.SelectedItem.Value);

                }
                GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(EmployeeToUpdate);
            }

            X.Js.Call("parent.App.uxPlaceholderWindow.hide()");
        }
    }
}