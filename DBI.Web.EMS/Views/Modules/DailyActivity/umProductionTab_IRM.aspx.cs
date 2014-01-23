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
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
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
                            select new { d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.DESCRIPTION, d.BILL_RATE, d.STATION, d.COMMENTS, d.UNIT_OF_MEASURE, d.EXPENDITURE_TYPE, d.QUANTITY }).ToList();

                if (data.Count >= 1)
                {
                    uxAddProductionButton.Disabled = true;
                }
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

            //Query for project ID, and get tasks for that project
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                 where d.HEADER_ID == HeaderId
                                 select d.PROJECT_ID).Single();
                var data = (from t in _context.PA_TASKS_V
                            where t.PROJECT_ID == ProjectId
                            select t).ToList();

                //Set datasource for Add/Edit store
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
                
                uxEditProductionTask.SelectedItems.Clear();
                uxEditProductionTask.SetValueAndFireSelect(Production["TASK_ID"]);
                uxEditProductionTask.SelectedItems.Add(new Ext.Net.ListItem(Production["DESCRIPTION"], Production["TASK_ID"]));
                uxEditProductionTask.UpdateSelectedItems();
                uxEditProductionStation.SetValue(Production["STATION"]);
                uxEditProductionExpenditureType.SetValue(Production["EXPENDITURE_TYPE"], Production["EXPENDITURE_TYPE"]);
                uxEditProductionQuantity.SetValue(Production["QUANTITY"]);
                uxEditProductionBillRate.SetValue(Production["BILL_RATE"]);
                uxEditProductionUOM.SetValue(Production["UNIT_OF_MEASURE"]);
                uxEditProductionComments.SetValue(Production["COMMENTS"]);
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
            decimal BillRate = decimal.Parse(uxAddProductionBillRate.Value.ToString());
            decimal Quantity = decimal.Parse(uxAddProductionQuantity.Value.ToString());
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            using (Entities _context = new Entities())
            {
                data = new DAILY_ACTIVITY_PRODUCTION()
                {
                    HEADER_ID = HeaderId,
                    TASK_ID = TaskId,
                    EXPENDITURE_TYPE = uxAddProductionExpenditureType.Value.ToString(),
                    BILL_RATE = BillRate,
                    COMMENTS = uxAddProductionComments.Value.ToString(),
                    STATION = uxAddProductionStation.Value.ToString(),
                    UNIT_OF_MEASURE = uxAddProductionUOM.Value.ToString(),
                    QUANTITY = Quantity,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name
                };
            }

            //Write to DB
            GenericData.Insert<DAILY_ACTIVITY_PRODUCTION>(data);

            uxAddProductionWindow.Hide();
            uxAddProductionForm.Reset();
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

            //Do type conversions
            long TaskId = long.Parse(uxEditProductionTask.Value.ToString());
            decimal Quantity = decimal.Parse(uxEditProductionQuantity.Value.ToString());
            decimal BillRate = decimal.Parse(uxEditProductionBillRate.Value.ToString());
            long ProductionId = long.Parse(e.ExtraParams["ProductionId"]);
            
            //Get record to be edited
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }
            data.TASK_ID = TaskId;
            data.EXPENDITURE_TYPE = uxEditProductionExpenditureType.Value.ToString();
            data.QUANTITY = Quantity;
            data.STATION = uxEditProductionStation.Value.ToString();
            data.BILL_RATE = BillRate;
            data.MODIFY_DATE = DateTime.Now;
            data.UNIT_OF_MEASURE = uxEditProductionUOM.Value.ToString();
            data.COMMENTS = uxEditProductionComments.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;

            //Write to DB
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

        protected void deReadExpenditures(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId= long.Parse(Request.QueryString["HeaderId"]);
                long ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                      where d.HEADER_ID == HeaderId
                                      select (long) d.PROJECT_ID).Single();

                List<EXPENDITURE_TYPE_V> dataIn = (from d in _context.EXPENDITURE_TYPE_V
                                           where d.PROJECT_ID == ProjectId
                                           select d).ToList();
                int count;
                List<EXPENDITURE_TYPE_V> data = GenericData.EnumerableFilterHeader<EXPENDITURE_TYPE_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();
                e.Total = count;

                uxAddProductionExpenditureStore.DataSource = data;
                uxEditProductionExpenditureStore.DataSource = data;
            }
        }

        protected void deStoreExpenditureType(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Type"] == "Edit")
            {
                uxEditProductionExpenditureType.SetValue(e.ExtraParams["ExpenditureType"], e.ExtraParams["ExpenditureType"]);
                uxEditProductionUOM.SetValue(e.ExtraParams["UnitOfMeasure"]);
                uxEditProductionBillRate.SetValue(e.ExtraParams["BillRate"]);

                uxEditProductionExpenditureStore.ClearFilter();
            }
            else
            {
                uxAddProductionExpenditureType.SetValue(e.ExtraParams["ExpenditureType"], e.ExtraParams["ExpenditureType"]);
                uxAddProductionUOM.SetValue(e.ExtraParams["UnitOfMeasure"]);
                uxAddProductionBillRate.SetValue(e.ExtraParams["BillRate"]);

                uxAddProductionExpenditureStore.ClearFilter();
            }
        }

    }
}