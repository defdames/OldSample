using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umChooseLunchHeader : BasePage
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
                List<EmployeeData> EmployeesNeedingLunch = ValidationChecks.LunchCheck(HeaderId);
                var count = 1;
                foreach (EmployeeData Employee in EmployeesNeedingLunch)
                {
                    Hidden LunchLength = new Hidden
                    {
                        ID = "Length" + Employee.PERSON_ID.ToString(),
                        Value = Employee.LUNCH_LENGTH
                    };
                    uxChooseLunchForm.Items.Add(LunchLength);

                    ComboBox AddLunchComboBox = new ComboBox()
                    {
                        ID = Employee.PERSON_ID.ToString(),
                        FieldLabel = Employee.EMPLOYEE_NAME,
                        EmptyText = "Select a Project to assign lunch to",
                        TypeAhead = true,
                        QueryMode = DataLoadMode.Local,
                        ValueField = "PROJECT_ID",
                        DisplayField = "LONG_NAME"
                    };

                    var ProjectList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                       join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                       join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                       where d.PERSON_ID == Employee.PERSON_ID && h.DA_DATE == Employee.DA_DATE && h.STATUS != 5
                                       select new { p.PROJECT_ID, p.LONG_NAME }).ToList();

                    Store ComboStore = new Store()
                    {
                        ID = string.Format("Store{0}", Employee.PERSON_ID.ToString()),
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
                    AddLunchComboBox.Store.Add(ComboStore);

                    uxChooseLunchForm.Items.Add(AddLunchComboBox);
                    ComboBoxes.Add(Employee.PERSON_ID);
                }
            }
        }

        protected void deStoreLunchChoice(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long? OrgId;
            foreach (long PersonId in ComboBoxes)
            {
                ComboBox LunchBox = FindControl(PersonId.ToString()) as ComboBox;
                DAILY_ACTIVITY_EMPLOYEE EmployeeToUpdate;

                long ProjectID = long.Parse(LunchBox.Value.ToString());
                using (Entities _context = new Entities())
                {
                    Hidden Length = FindControl("Length" + PersonId.ToString()) as Hidden;

                    try
                    {
                        //Check for existing lunch
                        var LunchCheck = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                          where d.PERSON_ID == PersonId && d.HEADER_ID == HeaderId && d.LUNCH == "Y"
                                          select new { d.LUNCH_LENGTH, d.DAILY_ACTIVITY_HEADER.STATUS, d.DAILY_ACTIVITY_HEADER.PROJECT_ID }).Single();

                        EmployeeToUpdate = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                            join h in _context.DAILY_ACTIVITY_HEADER
                                                on d.HEADER_ID equals h.HEADER_ID
                                            join p in _context.PROJECTS_V
                                                on h.PROJECT_ID equals p.PROJECT_ID
                                            where h.PROJECT_ID == ProjectID && d.PERSON_ID == PersonId && d.HEADER_ID == HeaderId
                                            select d).Single();

                        if (decimal.Parse(Length.Value.ToString()) > LunchCheck.LUNCH_LENGTH && LunchCheck.STATUS == 5)
                        {

                            EmployeeToUpdate.LUNCH = "Y";
                            EmployeeToUpdate.LUNCH_LENGTH = 30;
                        }
                        else if (decimal.Parse(Length.Value.ToString()) > LunchCheck.LUNCH_LENGTH && LunchCheck.PROJECT_ID == ProjectID)
                        {
                            EmployeeToUpdate.LUNCH_LENGTH = 60;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        EmployeeToUpdate = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                            join h in _context.DAILY_ACTIVITY_HEADER
                                                on d.HEADER_ID equals h.HEADER_ID
                                            join p in _context.PROJECTS_V
                                                on h.PROJECT_ID equals p.PROJECT_ID
                                            where h.PROJECT_ID == ProjectID && d.PERSON_ID == PersonId && d.HEADER_ID == HeaderId
                                            select d).Single();
                        EmployeeToUpdate.LUNCH = "Y";
                        EmployeeToUpdate.LUNCH_LENGTH = decimal.Parse(Length.Value.ToString());
                    }

                }
                GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(EmployeeToUpdate);
            }
            using (Entities _context = new Entities())
            {
                OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
                         join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                         where d.HEADER_ID == HeaderId
                         select p.ORG_ID).Single();
            }

            if (OrgId == 121)
            {
                X.Js.Call(string.Format("parent.App.direct.dmRefreshShowSubmit_DBI('{0}')", Request.QueryString["HeaderId"]));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmRefreshShowSubmit_IRM('{0}')", Request.QueryString["HeaderId"]));
            }
        }
    }
}