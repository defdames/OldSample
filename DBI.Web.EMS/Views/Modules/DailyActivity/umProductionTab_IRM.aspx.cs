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
    public partial class umProductionTab_IRM : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            GetCurrentProduction();
        }

        /// <summary>
        /// Gets list of current Production Items
        /// </summary>
        protected void GetCurrentProduction()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Get list of current production entries and set to store
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.TASK_NUMBER, t.DESCRIPTION, d.BILL_RATE, d.STATION, d.COMMENTS, d.UNIT_OF_MEASURE, d.EXPENDITURE_TYPE, d.SURFACE_TYPE, d.QUANTITY }).ToList();

                uxCurrentProductionStore.DataSource = data;
            }
        }


        /// <summary>
        /// Remove production item from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveProduction(object sender, DirectEventArgs e)
        {
            long ProductionId = long.Parse(e.ExtraParams["ProductionId"]);
            DAILY_ACTIVITY_PRODUCTION data;

            //Get record to be deleted
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }

            //Process deletion
            GenericData.Delete<DAILY_ACTIVITY_PRODUCTION>(data);

            uxCurrentProductionStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Production Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deLoadProductionWindow(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (e.ExtraParams["Type"] == "Add")
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadProductionWindow_IRM('{0}', '{1}', '{2}')", "Add", HeaderId.ToString(), "None"));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadProductionWindow_IRM('{0}', '{1}', '{2}')", "Edit", HeaderId.ToString(), e.ExtraParams["ProductionId"]));
            }
        }       

    }
}