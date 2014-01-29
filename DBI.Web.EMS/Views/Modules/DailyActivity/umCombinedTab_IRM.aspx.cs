using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umCombinedTab_IRM : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            if (!X.IsAjaxRequest)
            {
                GetHeaderData();
                GetEmployeeData();
                GetIRMProductionData();
                GetWeatherData();
                GetInventory();
                GetFooterData();
            }
        }

        /// <summary>
        /// Get data for header grid
        /// </summary>
        protected void GetHeaderData()
        {
            //Query and set datasource for header
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.PROJECT_ID, p.LONG_NAME, d.DA_DATE, d.SUBDIVISION, d.CONTRACTOR, d.PERSON_ID, e.EMPLOYEE_NAME, d.LICENSE, d.STATE, d.APPLICATION_TYPE, d.DENSITY }).ToList();
                uxHeaderStore.DataSource = data;
                uxHeaderStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Employee/Equipment grid
        /// </summary>
        protected void GetEmployeeData()
        {
            //Query and set datasource for employees
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
                            select new { e.EMPLOYEE_NAME, projects.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS }).ToList();

                uxEquipmentStore.DataSource = data;
                uxEquipmentStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Production grid
        /// </summary>
        protected void GetIRMProductionData()
        {
            //Query and set datasource for Production
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.DESCRIPTION, d.WORK_AREA, d.QUANTITY, d.STATION, d.EXPENDITURE_TYPE, d.BILL_RATE, d.UNIT_OF_MEASURE, d.COMMENTS }).ToList();
                uxProductionStore.DataSource = data;
                uxProductionStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Weather grid
        /// </summary>
        protected void GetWeatherData()
        {
            //Query and set datasource for Weather
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from w in _context.DAILY_ACTIVITY_WEATHER
                            where w.HEADER_ID == HeaderId
                            select w).ToList();
                uxWeatherStore.DataSource = data;
                uxWeatherStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Inventory grid
        /// </summary>
        protected void GetInventory()
        {
            //Query and set datasource for Inventory
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            where d.HEADER_ID == HeaderId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new {d.SUB_INVENTORY_SECONDARY_NAME, j.DESCRIPTION, d.RATE }).ToList();
                uxInventoryStore.DataSource = data;
                uxInventoryStore.DataBind();
            }
        }

        /// <summary>
        /// Get data for Footer grid
        /// </summary>
        protected void GetFooterData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_FOOTER
                            where d.HEADER_ID == HeaderId
                            select new { d.HOTEL_CITY, d.HOTEL_NAME, d.HOTEL_PHONE, d.HOTEL_STATE, d.COMMENTS, d.FOREMAN_SIGNATURE, d.CONTRACT_REP, d.DOT_REP, d.DOT_REP_NAME }).ToList();
                var processedData = (from d in data
                                     select new { d.HOTEL_CITY, d.HOTEL_NAME, d.HOTEL_PHONE, d.HOTEL_STATE, d.COMMENTS, FOREMAN_SIGNATURE = d.FOREMAN_SIGNATURE.Length > 0 ? true : false, CONTRACT_REP = d.CONTRACT_REP.Length > 0 ? true : false, DOT_REP = d.DOT_REP.Length > 0 ? true : false, d.DOT_REP_NAME}).ToList();
                uxFooterStore.DataSource = processedData;
                uxFooterStore.DataBind();
            }
        }
    }
}