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
    public partial class umAddEditProduction_IRM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["Type"] == "Add")
                {
                    uxAddProductionForm.Show();
                }
                else
                {
                    uxEditProductionForm.Show();
                    LoadEditProductionForm();
                }
                GetTaskList(Request.QueryString["Type"]);
            }
        }

        /// <summary>
        /// Get Task List Based on project of header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetTaskList(string FormType)
        {

            //Query for project ID, and get tasks for that project
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                var ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                 where d.HEADER_ID == HeaderId
                                 select d.PROJECT_ID).Single();
                var data = (from t in _context.PA_TASKS_V
                            where t.PROJECT_ID == ProjectId  && t.START_DATE <= DateTime.Now && (t.COMPLETION_DATE >= DateTime.Now || t.COMPLETION_DATE == null)
                            select t).ToList();

                //Set datasource for Add/Edit store
                if (FormType == "Add")
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
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);
            using(Entities _context = new Entities()){
                var Production = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                  join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                                  where d.PRODUCTION_ID == ProductionId
                                  select new { d, t.DESCRIPTION }).Single();
                uxEditProductionTask.SelectedItems.Clear();
                uxEditProductionTask.SetValueAndFireSelect(Production.d.TASK_ID);
                uxEditProductionTask.SelectedItems.Add(new Ext.Net.ListItem(Production.DESCRIPTION, Production.d.TASK_ID));
                uxEditProductionTask.UpdateSelectedItems();
                uxEditProductionStation.SetValue(Production.d.STATION);
                uxEditProductionExpenditureType.SetValue(Production.d.EXPENDITURE_TYPE, Production.d.EXPENDITURE_TYPE);
                uxEditProductionQuantity.SetValue(Production.d.QUANTITY);
                uxEditProductionBillRate.SetValue(Production.d.BILL_RATE);
                uxEditProductionUOM.SetValue(Production.d.UNIT_OF_MEASURE);
                uxEditProductionComments.SetValue(Production.d.COMMENTS);
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
            decimal Quantity = decimal.Parse(uxEditProductionQuantity.Value.ToString());
            decimal BillRate = decimal.Parse(uxEditProductionBillRate.Value.ToString());
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);

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

        protected void deReadExpenditures(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                long ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                  where d.HEADER_ID == HeaderId
                                  select (long)d.PROJECT_ID).Single();

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