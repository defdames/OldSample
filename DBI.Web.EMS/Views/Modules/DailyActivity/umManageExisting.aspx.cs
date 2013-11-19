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
    public partial class umManageExisting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetGridData();
        }

        protected void GetGridData()
        {
            using (Entities _context = new Entities())
            {
                
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME }).ToList();
                uxManageGridStore.DataSource = data;
            }
        }

        protected void deSelectHeader(object sender, DirectEventArgs e)
        {
            string headerUrl = string.Format("umHeaderTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string equipUrl = string.Format("umEquipmentTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string prodUrl = string.Format("umProductionTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string emplUrl = string.Format("umEmployeesTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string chemUrl = string.Format("umChemicalTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string weatherUrl = string.Format("umWeatherTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string invUrl = string.Format("umInventoryTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
           
            uxHeaderTab.LoadContent(headerUrl);
            uxEquipmentTab.LoadContent(equipUrl);
            uxProductionTab.LoadContent(prodUrl);
            uxEmployeeTab.LoadContent(emplUrl);
            uxChemicalTab.LoadContent(chemUrl);
            uxWeatherTab.LoadContent(weatherUrl);
        }
    }
}