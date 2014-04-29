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
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            GetGridData();

            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            
            int Status = GetStatus(HeaderId);
            if (Status == 4)
            {
                uxAddEmployee.Disabled = true;
            }
            if (Status == 3 && !validateComponentSecurity("SYS.DailyActivity.Post"))
            {
                uxAddEmployee.Disabled = true;
            }

            if (GetOrgId(HeaderId) == 123)
            {
                uxShopTimeAMColumn.Show();
                uxShopTimePMColumn.Show();
                uxDriveTimeColumn.Show();
                uxSupportProjectColumn.Show();
            }

        }

        protected void deEnableEdit(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            int Status = GetStatus(HeaderId);

            if (Status == 4)
            {
                uxEditEmployee.Disabled = true;
                uxRemoveEmployee.Disabled = true;
                uxChooseLunchHeader.Disabled = true;
            }
            else if (Status == 3 && !validateComponentSecurity("SYS.DailyActivity.Post"))
            {
                uxEditEmployee.Disabled = true;
                uxRemoveEmployee.Disabled = true;
                uxChooseLunchHeader.Disabled = true;
            }
            else
            {
                uxEditEmployee.Disabled = false;
                uxRemoveEmployee.Disabled = false;
                uxChooseLunchHeader.Disabled = false;
            }
        }
        
        protected int GetStatus(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                int Status = (from d in _context.DAILY_ACTIVITY_HEADER
                              where d.HEADER_ID == HeaderId
                              select (int)d.STATUS).Single();
                return Status;
            }
        }

        protected long GetOrgId(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                long OrgId;
                return OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
                                join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                where d.HEADER_ID == HeaderId
                                select (long)p.ORG_ID).Single();
            }

        }

        /// <summary>
        /// Get Current Employee Data
        /// </summary>
        protected void GetGridData()
        {
            //Get Employee data and set datasource
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join p in _context.PROJECTS_V on d.SUPPORT_PROJ_ID equals p.PROJECT_ID into first
                            from f in first.DefaultIfEmpty()
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                            from projects in proj.DefaultIfEmpty()
                            where d.HEADER_ID == HeaderId
                            select new EmployeeDetails { EMPLOYEE_ID = d.EMPLOYEE_ID, SUPPORT_PROJECT = f.NAME, HEADER_ID = d.HEADER_ID, LUNCH = d.LUNCH, LUNCH_LENGTH = d.LUNCH_LENGTH, PERSON_ID = d.PERSON_ID, EMPLOYEE_NAME = e.EMPLOYEE_NAME, EQUIPMENT_ID = d.EQUIPMENT_ID, NAME = projects.NAME, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), PER_DIEM = d.PER_DIEM, COMMENTS = d.COMMENTS, ROLE_TYPE = d.ROLE_TYPE }).ToList();
                foreach (var item in data)
                {
                    
                    double Hours = Math.Truncate((double)item.TRAVEL_TIME);
                    double Minutes = Math.Round(((double)item.TRAVEL_TIME - Hours) * 60);
                    TimeSpan TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.TRAVEL_TIME_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
                    Hours = Math.Truncate((double)item.DRIVE_TIME);
                    Minutes = Math.Round(((double)item.DRIVE_TIME - Hours) * 60);
                    TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.DRIVE_TIME_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
                    Hours = Math.Truncate((double)item.SHOPTIME_AM);
                    Minutes = Math.Round(((double)item.SHOPTIME_AM - Hours) * 60);
                    TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.SHOPTIME_AM_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
                    Hours = Math.Truncate((double)item.SHOPTIME_PM);
                    Minutes = Math.Round(((double)item.SHOPTIME_PM - Hours) * 60);
                    TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.SHOPTIME_PM_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");

                    
                }
                uxCurrentEmployeeStore.DataSource = data;
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

        protected void deLoadEmployeeWindow(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (e.ExtraParams["type"] == "Add")
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadEmployeeWindow('{0}', '{1}', '{2}')", "Add", HeaderId.ToString(), "None"));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadEmployeeWindow('{0}', '{1}', '{2}')", "Edit", HeaderId.ToString(), e.ExtraParams["EmployeeId"]));
            }
        }

        protected void deChooseLunchHeader(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            X.Js.Call(string.Format("parent.App.direct.dmLoadLunchWindow('{0}', '{1}')", HeaderId.ToString(), e.ExtraParams["EmployeeId"]));
        }

        protected void deChoosePerDiem(object sender, DirectEventArgs e)
        {

        }
    }

    public class EmployeeDetails
    {
        public long EMPLOYEE_ID { get; set; }
        public long HEADER_ID { get; set; }
        public int PERSON_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public long? EQUIPMENT_ID { get; set; }
        public string NAME { get; set; }
        public DateTime TIME_IN { get; set; }
        public DateTime TIME_OUT { get; set; }
        public decimal? TRAVEL_TIME { get; set; }
        public string TRAVEL_TIME_FORMATTED { get; set; }
        public decimal? SHOPTIME_AM { get; set; }
        public string SHOPTIME_AM_FORMATTED { get; set; }
        public decimal? SHOPTIME_PM { get; set; }
        public string SHOPTIME_PM_FORMATTED { get; set; }
        public decimal? DRIVE_TIME { get; set; }
        public string DRIVE_TIME_FORMATTED { get; set; }
        public string SUPPORT_PROJECT { get; set; }
        public string PER_DIEM { get; set; }
        public string COMMENTS { get; set; }
        public string ROLE_TYPE { get; set; }
        public string FOREMAN_LICENSE { get; set; }
        public string LUNCH { get; set; }
        public decimal? LUNCH_LENGTH { get; set; }

    }
}