using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umChoosePerDiem : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long EmployeeId = long.Parse(Request.QueryString["EmployeeId"]);
            FillComboBox(HeaderId, EmployeeId);
        }

        protected void FillComboBox(long HeaderId, long EmployeeId)
        {
            using (Entities _context = new Entities()){

                long PersonId = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmployeeId).Select(x => x.PERSON_ID).Single();
                DateTime HeaderDate = (from d in _context.DAILY_ACTIVITY_HEADER
                                       where d.HEADER_ID == HeaderId
                                       select (DateTime)d.DA_DATE).Single<DateTime>();

                List<DAILY_ACTIVITY_HEADER> HeaderComboStore = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                       join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                       where h.DA_DATE == HeaderDate && d.PERSON_ID == PersonId && d.PER_DIEM == "Y"
                                                       select h).ToList();
                
                List<LunchInfo> ComboList = new List<LunchInfo>();
                foreach (DAILY_ACTIVITY_HEADER Header in HeaderComboStore)
                {
                    PROJECTS_V ProjectName = _context.PROJECTS_V.Where(x => x.PROJECT_ID == Header.PROJECT_ID).Single();
                    if (ProjectName.ORG_ID == 121 || ProjectName.ORG_ID == 123)
                    {
                        if (Header.DAILY_ACTIVITY_PRODUCTION.Count > 0)
                        {
                            foreach (DAILY_ACTIVITY_PRODUCTION ProductionEntry in Header.DAILY_ACTIVITY_PRODUCTION)
                            {
                                PA_TASKS_V TaskInfo = _context.PA_TASKS_V.Where(x => x.TASK_ID == ProductionEntry.TASK_ID && x.PROJECT_ID == Header.PROJECT_ID).Single();
                                ComboList.Add(new LunchInfo
                                {
                                    HeaderId = Header.HEADER_ID,
                                    ProjectTask = string.Format("{0} (Task: {1} - {2})", ProjectName.LONG_NAME, TaskInfo.TASK_NUMBER, TaskInfo.DESCRIPTION)
                                });
                            }
                        }
                        else
                        {
                            ComboList.Add(new LunchInfo
                            {
                                HeaderId = Header.HEADER_ID,
                                ProjectTask = string.Format("{0} (Task: 9999 - Production)", ProjectName.LONG_NAME)
                            });
                        }
                    }
                }
                uxChoosePerDiemHeaderIdStore.DataSource = ComboList;
            }
        }

        protected void deUpdatePerDiem(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long EmployeeId = long.Parse(Request.QueryString["EmployeeId"]);
            long ChosenHeaderId = long.Parse(uxChoosePerDiemHeaderId.SelectedItem.Value.ToString());
            List<DAILY_ACTIVITY_EMPLOYEE> RecordsToUpdate;
            long? OrgId;
            using (Entities _context = new Entities())
            {
                long PersonId = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmployeeId).Select(x => x.PERSON_ID).Single();
                DateTime HeaderDate = (from d in _context.DAILY_ACTIVITY_HEADER
                                       where d.HEADER_ID == HeaderId
                                       select (DateTime)d.DA_DATE).Single<DateTime>();

                RecordsToUpdate = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                   where h.DA_DATE == HeaderDate && d.PERSON_ID == PersonId && d.PER_DIEM == "Y"
                                   select d).ToList();
                OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
                         join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                         where d.HEADER_ID == HeaderId
                         select p.ORG_ID).Single();
                
            }

            foreach (DAILY_ACTIVITY_EMPLOYEE Record in RecordsToUpdate)
            {
                if (Record.HEADER_ID == ChosenHeaderId)
                {
                    Record.PER_DIEM = "Y";
                }
                else
                {
                    Record.PER_DIEM = "N";
                }
                GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(Record);
            }

            X.Js.Call("parent.App.uxPlaceholderWindow.hide()");

        }
    }

    public class HeaderDetails
    {
        public long HEADER_ID { get; set; }
        public string LONG_NAME { get; set; }
        public long PERSON_ID { get; set; }
    }
}