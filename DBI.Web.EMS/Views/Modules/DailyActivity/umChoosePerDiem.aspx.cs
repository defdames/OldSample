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
            long PersonId = long.Parse(Request.QueryString["PersonId"]);
            FillComboBox(HeaderId, PersonId);
        }

        protected void FillComboBox(long HeaderId, long PersonId)
        {
            using (Entities _context = new Entities()){
                DateTime HeaderDate = (from d in _context.DAILY_ACTIVITY_HEADER
                                       where d.HEADER_ID == HeaderId
                                       select (DateTime)d.DA_DATE).Single<DateTime>();

                List<EmployeeData> HeaderComboStore = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                           join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                                           join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                                           where h.DA_DATE == HeaderDate && d.PERSON_ID == PersonId && d.PER_DIEM =="Y"
                                                           select new EmployeeData{HEADER_ID = d.HEADER_ID, LONG_NAME = p.LONG_NAME}).ToList();
                uxChoosePerDiemHeaderIdStore.DataSource = HeaderComboStore;
            }
        }

        protected void deUpdatePerDiem(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long PersonId = long.Parse(Request.QueryString["PersonId"]);
            long ChosenHeaderId = long.Parse(uxChoosePerDiemHeaderId.SelectedItem.Value.ToString());
            List<DAILY_ACTIVITY_EMPLOYEE> RecordsToUpdate;
            long? OrgId;
            using (Entities _context = new Entities())
            {
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