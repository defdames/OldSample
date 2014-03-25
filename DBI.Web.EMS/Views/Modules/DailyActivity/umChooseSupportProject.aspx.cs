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
            long HeaderId = long.Parse(Request.QueryString["HeaderID"]);
            GenerateForm(HeaderId);
        }

        protected void GenerateForm(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<SupportEmployeeData> EmployeesNeedingSupportProject = ValidationChecks.employeesWithUnassignedShopTime(HeaderId);
                var count = 0;

                foreach (SupportEmployeeData Employee in EmployeesNeedingSupportProject)
                {

                    ComboBox AddSupportListComboBox = new ComboBox()
                    {
                        ID = "Combo" + Employee.EMPLOYEE_ID.ToString(),
                        FieldLabel = Employee.EMPLOYEE_NAME,
                        EmptyText = "Select a support project to assign shop time",
                        TypeAhead = true,
                        QueryMode = DataLoadMode.Local,
                        ValueField = "PROJECT_ID",
                        ForceSelection=true,
                        DisplayField = "LONG_NAME"
                    };

                    var ProjectList =( from p in _context.PROJECTS_V
                                      where p.PROJECT_TYPE == "SUPPORT OVERHEAD" && p.TEMPLATE_FLAG == "N" && p.PROJECT_STATUS_CODE == "APPROVED"
                                     select new {p.PROJECT_ID, p.LONG_NAME }).ToList();

                    Store ComboStore = new Store()
                    {
                        ID = string.Format("Store{0}", Employee.EMPLOYEE_ID.ToString()),
                        AutoDataBind = true,
                        DataSource= ProjectList
                    };

                    Model ComboModel = new Model();
                    ComboModel.Fields.Add(new ModelField
                    {
                        Name = "PROJECT_ID"
                    });
                    ComboModel.Fields.Add(new ModelField
                    {
                        Name = "LONG_NAME"
                    });
                    ComboStore.Model.Add(ComboModel);
                    AddSupportListComboBox.Store.Add(ComboStore);

                    uxChooseSupportProject.Items.Add(AddSupportListComboBox);
                    ComboBoxes.Add(Employee.EMPLOYEE_ID);
                }
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

            X.Js.Call(string.Format("parent.App.direct.dmRefreshShowSubmit_IRM('{0}')", Request.QueryString["HeaderId"]));
        }
    }
}