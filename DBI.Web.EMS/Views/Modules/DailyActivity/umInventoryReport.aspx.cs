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
    public partial class umInventoryReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadReport(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                List<ReportFields> data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                                           join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                                           join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                           join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                           join pr in _context.DAILY_ACTIVITY_PRODUCTION on d.HEADER_ID equals pr.HEADER_ID
                                           join t in _context.PA_TASKS_V on pr.TASK_ID equals t.TASK_ID
                                           join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                                           where d.DAILY_ACTIVITY_HEADER.STATUS == 3
                                           from j in joined
                                           where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID && OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID)
                                           select new ReportFields { HeaderId = d.HEADER_ID, ProjectId = p.SEGMENT1, ProjectDescription = p.LONG_NAME, OrgName = p.ORGANIZATION_NAME, TaskNumber = t.TASK_NUMBER, ActivityDate = h.DA_DATE, ItemNumber = j.SEGMENT1, ItemDescription = j.DESCRIPTION, Total = d.TOTAL, Units = u.UOM_CODE, Inventory = j.INV_NAME, SubInventory = d.SUB_INVENTORY_SECONDARY_NAME, State = h.STATE }).ToList<ReportFields>();
                int count;
                uxReportStore.DataSource = GenericData.EnumerableFilterHeader<ReportFields>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
    }

    public class ReportFields
    {
        public long HeaderId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectDescription { get; set; }
        public string OrgName { get; set; }
        public string TaskNumber { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public decimal? Total { get; set; }
        public string Units { get; set; }
        public string Inventory { get; set; }
        public string SubInventory { get; set; }
        public string State { get; set; }
    }
}