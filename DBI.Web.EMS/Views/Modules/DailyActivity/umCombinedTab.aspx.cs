using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umCombinedTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetHeaderData();
            GetEmployeeData();
            GetProductionData();
            GetWeatherData();
            GetChemicalMixData();
            GetInventory();
        }

        protected void GetHeaderData()
        {
            using (Entities _context = new Entities()){
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            where d.HEADER_ID == HeaderId
                            select new {d.PROJECT_ID, p.LONG_NAME, d.DA_DATE, d.SUBDIVISION, d.CONTRACTOR, d.PERSON_ID, e.EMPLOYEE_NAME, d.LICENSE, d.STATE, d.APPLICATION_TYPE, d.DENSITY }).ToList();
                uxHeaderStore.DataSource = data;
            }
        }

        protected void GetEmployeeData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID
                            join p in _context.PROJECTS_V on eq.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { e.EMPLOYEE_NAME, p.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS }).ToList();

                uxEquipmentStore.DataSource = data;
            }
        }

        protected void GetProductionData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            join t in _context.PA_TASKS_V on p.PROJECT_ID equals t.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new {t.DESCRIPTION, d.TIME_IN, d.TIME_OUT, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.GALLONS }).ToList();
                uxProductionStore.DataSource = data;
            }
        }

        protected void GetWeatherData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from w in _context.DAILY_ACTIVITY_WEATHER
                            where w.HEADER_ID == HeaderId
                            select w).ToList();
                uxWeatherStore.DataSource = data;
            }
        }

        protected void GetChemicalMixData()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from c in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                            where c.HEADER_ID == HeaderId
                            select new {c.CHEMICAL_MIX_NUMBER, c.TARGET_AREA, c.GALLON_ACRE, c.GALLON_STARTING, c.GALLON_MIXED, c.GALLON_REMAINING, c.STATE, c.COUNTY, TOTAL = c.GALLON_STARTING + c.GALLON_MIXED, USED = c.GALLON_STARTING + c.GALLON_MIXED - c.GALLON_REMAINING, SPRAYED = (c.GALLON_STARTING + c.GALLON_MIXED - c.GALLON_REMAINING) / c.GALLON_ACRE }).ToList();
                uxChemicalStore.DataSource = data;
            }
        }

        protected void GetInventory()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                            join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                            where d.HEADER_ID == HeaderId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new {c.CHEMICAL_MIX_NUMBER, d.SUB_INVENTORY_SECONDARY_NAME, j.DESCRIPTION, d.RATE, u.UNIT_OF_MEASURE, d.EPA_NUMBER }).ToList();
                uxInventoryStore.DataSource = data;
            }
        }
    }
}