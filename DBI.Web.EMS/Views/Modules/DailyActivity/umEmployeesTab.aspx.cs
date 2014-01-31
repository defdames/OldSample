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
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            GetGridData();
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
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                            from projects in proj.DefaultIfEmpty()
                            where d.HEADER_ID == HeaderId
                            select new { d.EMPLOYEE_ID, d.HEADER_ID, d.PERSON_ID, e.EMPLOYEE_NAME, d.EQUIPMENT_ID, projects.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS, d.ROLE_TYPE }).ToList();
                          
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
    }
}