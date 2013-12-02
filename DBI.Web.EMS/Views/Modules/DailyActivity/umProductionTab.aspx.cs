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

        /// <summary>
        /// Gets list of current Production Items
        /// </summary>
        protected void GetCurrentProduction()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.DESCRIPTION, d.TIME_IN, d.TIME_OUT, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.GALLONS }).ToList();
                uxCurrentProductionStore.DataSource = data;
            }
        }

        /// <summary>
        /// Get Task List Based on project of header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        protected void deEditProductionForm(object sender, DirectEventArgs e)
        {
            deGetTaskList(sender, e);

            //JSON Decode Row and assign to variables
            string JsonValues = e.ExtraParams["ProductionInfo"];
            Dictionary<string, string>[] ProductionInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);

            foreach (Dictionary<string, string> Production in ProductionInfo)
            {
                DateTime TimeIn = DateTime.Parse(Production["TIME_IN"]);
                DateTime TimeOut = DateTime.Parse(Production["TIME_OUT"]);

                uxEditProductionTask.SelectedItems.Clear();
                uxEditProductionTask.SetValueAndFireSelect(Production["TASK_ID"]);
                uxEditProductionTask.SelectedItems.Add(new Ext.Net.ListItem(Production["DESCRIPTION"], Production["TASK_ID"]));
                uxEditProductionTask.UpdateSelectedItems();
                uxEditProductionDateIn.SetValue(TimeIn.Date);
                uxEditProductionTimeIn.SetValue(TimeIn.TimeOfDay);
                uxEditProductionDateOut.SetValue(TimeOut.Date);
                uxEditProductionTimeOut.SetValue(TimeOut.TimeOfDay);
                uxEditProductionWorkArea.SetValue(Production["WORK_AREA"]);
                uxEditProductionPoleFrom.SetValue(Production["POLE_FROM"]);
                uxEditProductionPoleTo.SetValue(Production["POLE_TO"]);
                uxEditProductionAcresPerMile.SetValue(Production["ACRES_MILE"]);
                uxEditProductionGallons.SetValue(Production["GALLONS"]);
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
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                       where d.PRODUCTION_ID == ProductionId
                       select d).Single();
            }
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

        /// <summary>
        /// Add Production Item to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddProduction(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_PRODUCTION data;
            
            long TaskId = long.Parse(uxAddProductionTask.Value.ToString());
            long AcresPerMile = long.Parse(uxAddProductionAcresPerMile.Value.ToString());
            long Gallons = long.Parse(uxAddProductionGallons.Value.ToString());
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Combine DateTime Values
            DateTime TimeIn = DateTime.Parse(uxAddProductionDateIn.Value.ToString());
            DateTime TimeInTime = DateTime.Parse(uxAddProductionTimeIn.Value.ToString());
            DateTime TimeOut = DateTime.Parse(uxAddProductionDateOut.Value.ToString());
            DateTime TimeOutTime = DateTime.Parse(uxAddProductionTimeOut.Value.ToString());

            TimeIn = TimeIn + TimeInTime.TimeOfDay;
            TimeOut = TimeOut + TimeOutTime.TimeOfDay;

            using (Entities _context = new Entities())
            {
                data = new DAILY_ACTIVITY_PRODUCTION()
                {
                    HEADER_ID = HeaderId,
                    TIME_IN = TimeIn,
                    TIME_OUT = TimeOut,
                    TASK_ID = TaskId,
                    WORK_AREA = uxAddProductionWorkArea.Value.ToString(),
                    POLE_FROM = uxAddProductionPoleFrom.Value.ToString(),
                    POLE_TO = uxAddProductionPoleTo.Value.ToString(),
                    ACRES_MILE = AcresPerMile,
                    GALLONS = Gallons,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name
                };
            }
            GenericData.Insert<DAILY_ACTIVITY_PRODUCTION>(data);

            uxAddProductionWindow.Hide();
            uxCurrentProductionStore.Reload();

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

            long TaskId = long.Parse(uxEditProductionTask.Value.ToString());
            long AcresPerMile = long.Parse(uxEditProductionAcresPerMile.Value.ToString());
            long Gallons = long.Parse(uxEditProductionGallons.Value.ToString());
            long ProductionId = long.Parse(e.ExtraParams["ProductionId"]);


            //Combine DateTime Values
            DateTime TimeIn = DateTime.Parse(uxEditProductionDateIn.Value.ToString());
            DateTime TimeInTime = DateTime.Parse(uxEditProductionTimeIn.Value.ToString());
            DateTime TimeOut = DateTime.Parse(uxEditProductionDateOut.Value.ToString());
            DateTime TimeOutTime = DateTime.Parse(uxEditProductionTimeOut.Value.ToString());

            TimeIn = TimeIn + TimeInTime.TimeOfDay;
            TimeOut = TimeOut + TimeOutTime.TimeOfDay;

            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }
            data.TIME_IN = TimeIn;
            data.TIME_OUT = TimeOut;
            data.TASK_ID = TaskId;
            data.WORK_AREA = uxEditProductionWorkArea.Value.ToString();
            data.POLE_FROM = uxEditProductionPoleFrom.Value.ToString();
            data.POLE_TO = uxEditProductionPoleTo.Value.ToString();
            data.ACRES_MILE = AcresPerMile;
            data.GALLONS = Gallons;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = User.Identity.Name;

            GenericData.Update<DAILY_ACTIVITY_PRODUCTION>(data);

            uxEditProductionWindow.Hide();
            uxCurrentProductionStore.Reload();

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
    }
}