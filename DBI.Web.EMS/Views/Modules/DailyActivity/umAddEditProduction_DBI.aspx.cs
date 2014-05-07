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
    public partial class umAddEditProduction_DBI : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["Type"] == "Add")
                {
                    uxAddProductionForm.Show();
                    GetTaskList();
                }
                else
                {
                    uxEditProductionForm.Show();
                    LoadEditProductionForm();
                    GetTaskList();
                }
            }
        }
        /// <summary>
        /// Get Task List Based on project of header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetTaskList()
        {

            //Query for project ID, and get tasks for that project
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                 where d.HEADER_ID == HeaderId
                                 select d.PROJECT_ID).Single();
                var data = (from t in _context.PA_TASKS_V
                            where t.PROJECT_ID == ProjectId && t.START_DATE <= DateTime.Now && (t.COMPLETION_DATE >= DateTime.Now || t.COMPLETION_DATE == null)
                            select t).ToList();

                //Set datasource for Add/Edit store
                if (Request.QueryString["Type"] == "Add")
                {
                    uxAddProductionTaskStore.DataSource = data;
                    uxAddProductionTaskStore.DataBind();
                }
                else
                {
                    uxEditProductionTaskStore.DataSource = data;
                    uxEditProductionTaskStore.DataBind();
                }
            }
        }

        /// <summary>
        /// Load current values into Edit Production Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadEditProductionForm()
        {
            GetTaskList();
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);
            using(Entities _context = new Entities()){
                var Production = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                  join e in _context.PA_TASKS_V on d.TASK_ID equals e.TASK_ID
                                  where d.PRODUCTION_ID == ProductionId
                                  select new { d.TASK_ID, e.DESCRIPTION, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.QUANTITY }).Single();

                uxEditProductionTask.SetValue(Production.TASK_ID.ToString(), Production.DESCRIPTION);
                uxEditProductionWorkArea.SetValue(Production.WORK_AREA);
                uxEditProductionPoleFrom.SetValue(Production.POLE_FROM);
                uxEditProductionPoleTo.SetValue(Production.POLE_TO);
                uxEditProductionAcresPerMile.SetValue(Production.ACRES_MILE);
                uxEditProductionGallons.SetValue(Production.QUANTITY);
            }
        }

        /// <summary>
        /// Add Production Item to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddProduction(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_PRODUCTION data;

            //Do type conversions
            long TaskId = long.Parse(uxAddProductionTask.Value.ToString());
            decimal AcresPerMile = decimal.Parse(uxAddProductionAcresPerMile.Value.ToString());
            decimal Gallons = decimal.Parse(uxAddProductionGallons.Value.ToString());
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            using (Entities _context = new Entities())
            {
                data = new DAILY_ACTIVITY_PRODUCTION()
                {
                    HEADER_ID = HeaderId,
                    TASK_ID = TaskId,
                    WORK_AREA = uxAddProductionWorkArea.Value.ToString(),
                    ACRES_MILE = AcresPerMile,
                    QUANTITY = Gallons,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name
                };
                try
                {
                    data.POLE_FROM = uxAddProductionPoleFrom.Value.ToString();
                }
                catch { }
                try
                {
                    data.POLE_TO = uxAddProductionPoleTo.Value.ToString();
                }
                catch { }
            }

            //Write to DB
            GenericData.Insert<DAILY_ACTIVITY_PRODUCTION>(data);

            uxAddProductionForm.Reset();
            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxProductionTab.reload()");

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Production Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditProduction(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_PRODUCTION data;

            //Do type conversions
            long TaskId = long.Parse(uxEditProductionTask.Value.ToString());
            decimal AcresPerMile = decimal.Parse(uxEditProductionAcresPerMile.Value.ToString());
            long Gallons = long.Parse(uxEditProductionGallons.Value.ToString());
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);

            //Get record to be edited
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }
            data.TASK_ID = TaskId;
            data.WORK_AREA = uxEditProductionWorkArea.Value.ToString();
            try
            {
                data.POLE_FROM = uxEditProductionPoleFrom.Value.ToString();
            }
            catch
            {
                data.POLE_FROM = null;
            }
            try
            {
                data.POLE_TO = uxEditProductionPoleTo.Value.ToString();
            }
            catch
            {
                data.POLE_TO = null;
            }
            data.ACRES_MILE = AcresPerMile;
            data.QUANTITY = Gallons;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = User.Identity.Name;

            //Write to DB
            GenericData.Update<DAILY_ACTIVITY_PRODUCTION>(data);

            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxProductionTab.reload()");

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Production Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deStoreTask(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Type"] == "Edit")
            {
                uxEditProductionTask.SetValue(e.ExtraParams["TaskId"], e.ExtraParams["Description"]);

                uxEditProductionTaskStore.ClearFilter();
            }
            else
            {
                uxAddProductionTask.SetValue(e.ExtraParams["TaskId"], e.ExtraParams["Description"]);

                uxAddProductionTaskStore.ClearFilter();
            }
        }
    }
}