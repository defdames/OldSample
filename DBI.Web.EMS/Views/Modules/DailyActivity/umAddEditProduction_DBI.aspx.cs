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
                GetTaskList();
                if (Request.QueryString["Type"] == "Add")
                {
                    uxFormType.Value = "Add";
                }
                else
                {
                    uxFormType.Value = "Edit";
                    LoadEditProductionForm();
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
                uxAddProductionTaskStore.DataSource = data;
                uxAddProductionTaskStore.DataBind();
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

                uxAddProductionTask.SetValue(Production.TASK_ID.ToString(), Production.DESCRIPTION);
                uxAddProductionWorkArea.SetValue(Production.WORK_AREA);
                uxAddProductionPoleFrom.SetValue(Production.POLE_FROM);
                uxAddProductionPoleTo.SetValue(Production.POLE_TO);
                uxAddProductionAcresPerMile.SetValue(Production.ACRES_MILE);
                uxAddProductionGallons.SetValue(Production.QUANTITY);
            }
        }

        protected void deProcessForm(object sender, DirectEventArgs e)
        {
            if (uxFormType.Value.ToString() == "Add")
            {
                deAddProduction(sender, e);
            }
            else
            {
                deEditProduction(sender, e);
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
            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");
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
            long TaskId = long.Parse(uxAddProductionTask.Value.ToString());
            decimal AcresPerMile = decimal.Parse(uxAddProductionAcresPerMile.Value.ToString());
            long Gallons = long.Parse(uxAddProductionGallons.Value.ToString());
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);

            //Get record to be edited
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }
            data.TASK_ID = TaskId;
            data.WORK_AREA = uxAddProductionWorkArea.Value.ToString();
            try
            {
                data.POLE_FROM = uxAddProductionPoleFrom.Value.ToString();
            }
            catch
            {
                data.POLE_FROM = null;
            }
            try
            {
                data.POLE_TO = uxAddProductionPoleTo.Value.ToString();
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

            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");
        }

        protected void deStoreTask(object sender, DirectEventArgs e)
        {
            uxAddProductionTask.SetValue(e.ExtraParams["TaskId"], e.ExtraParams["Description"]);
            uxAddProductionTaskStore.ClearFilter();
        }
    }
}