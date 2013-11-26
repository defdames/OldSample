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
    public partial class umProductionTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetCurrentProduction();
        }

        protected void GetCurrentProduction()
        {
            using (Entities _context = new Entities())
            {

            }
        }

        protected void deGetTaskList(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                where d.HEADER_ID == HeaderId
                                select d.PROJECT_ID).Single();
                var data = (from t in _context.PA_TASKS_V
                            where t.PROJECT_ID == ProjectId
                            select t).ToList();
                if (e.ExtraParams["Type"] == "Add")
                {
                    uxAddProductionTaskStore.DataSource = data;
                }
                else
                {
                    uxEditProductionTaskStore.DataSource = data;
                }
            }
        }

        protected void deEditProductionForm(object sender, DirectEventArgs e)
        {
            deGetTaskList(sender, e);
        }

        protected void deRemoveProduction(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {

            }
        }

        protected void deAddProduction(object sender, DirectEventArgs e)
        {
            DateTime TimeIn = DateTime.Parse(uxAddProductionTimeIn.Value.ToString());
            DateTime TimeOut = DateTime.Parse(uxAddProductionTimeOut.Value.ToString());
            long TaskId = long.Parse(uxAddProductionTask.Value.ToString());
            long AcresPerMile = long.Parse(uxAddProductionAcresPerMile.Value.ToString());
            long Gallons = long.Parse(uxAddProductionGallons.Value.ToString());

            using (Entities _context = new Entities())
            {

            }
        }

        protected void deEditProduction(object sender, DirectEventArgs e)
        {
            DateTime TimeIn = DateTime.Parse(uxEditProductionTimeIn.Value.ToString());
            DateTime TimeOut = DateTime.Parse(uxEditProductionTimeOut.Value.ToString());
            long TaskId = long.Parse(uxAddProductionTask.Value.ToString());
            long AcresPerMile = long.Parse(uxEditProductionAcresPerMile.Value.ToString());
            long Gallons = long.Parse(uxEditProductionGallons.Value.ToString());

            using (Entities _context = new Entities())
            {

            }
        }
    }
}