using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using DBI.Data.DataFactory;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umAddEditProduction_IRM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxAddProductionSurfaceTypeStore.Data = StaticLists.SurfaceTypes;
                if (Request.QueryString["Type"] == "Add")
                {
                    uxFormType.Value = "Add";
                }
                else
                {
                    uxFormType.Value = "Edit";
                    LoadEditProductionForm();
                }
                GetTaskList();
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
                            where t.PROJECT_ID == ProjectId  && t.START_DATE <= DateTime.Now && (t.COMPLETION_DATE >= DateTime.Now || t.COMPLETION_DATE == null)
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
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);
            using(Entities _context = new Entities()){
                var Production = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                  join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                                  where d.PRODUCTION_ID == ProductionId
                                  select new { d, t.DESCRIPTION }).Single();
                uxAddProductionTask.SetValue(Production.d.TASK_ID.ToString(), Production.DESCRIPTION);
                uxAddProductionStation.SetValue(Production.d.STATION);
                uxAddProductionExpenditureType.SetValue(Production.d.EXPENDITURE_TYPE, Production.d.EXPENDITURE_TYPE);
                uxAddProductionQuantity.SetValue(Production.d.QUANTITY);
                uxAddProductionBillRate.SetValue(Production.d.BILL_RATE);
                uxAddProductionUOM.SetValue(Production.d.UNIT_OF_MEASURE);
                uxAddProductionSurfaceType.SetValueAndFireSelect(Production.d.SURFACE_TYPE);
                uxAddProductionSurfaceType.SetValue(Production.d.SURFACE_TYPE);
                uxAddProductionComments.SetValue(Production.d.COMMENTS);
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
            decimal BillRate = decimal.Parse(uxAddProductionBillRate.Value.ToString());
            decimal Quantity = decimal.Parse(uxAddProductionQuantity.Text);
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            data = new DAILY_ACTIVITY_PRODUCTION();
            data.HEADER_ID = HeaderId;
            data.TASK_ID = TaskId;
            data.EXPENDITURE_TYPE = uxAddProductionExpenditureType.Value.ToString();
            data.BILL_RATE = BillRate;
            data.COMMENTS = uxAddProductionComments.Text;
            data.STATION = uxAddProductionStation.Text;
            data.UNIT_OF_MEASURE = uxAddProductionUOM.Text;
            data.QUANTITY = Quantity;
            data.SURFACE_TYPE = uxAddProductionSurfaceType.Text;
            data.CREATE_DATE = DateTime.Now;
            data.MODIFY_DATE = DateTime.Now;
            data.CREATED_BY = User.Identity.Name;
            data.MODIFIED_BY = User.Identity.Name;

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
            decimal Quantity = decimal.Parse(uxAddProductionQuantity.Text);
            decimal BillRate = decimal.Parse(uxAddProductionBillRate.Value.ToString());
            long ProductionId = long.Parse(Request.QueryString["ProductionId"]);

            //Get record to be edited
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                        where d.PRODUCTION_ID == ProductionId
                        select d).Single();
            }
            data.TASK_ID = TaskId;
            data.EXPENDITURE_TYPE = uxAddProductionExpenditureType.Value.ToString();
            data.QUANTITY = Quantity;
            data.STATION = uxAddProductionStation.Text;
            data.BILL_RATE = BillRate;
            data.MODIFY_DATE = DateTime.Now;
            data.UNIT_OF_MEASURE = uxAddProductionUOM.Value.ToString();
            data.SURFACE_TYPE = uxAddProductionSurfaceType.SelectedItem.Value;
            data.COMMENTS = uxAddProductionComments.Text;
            data.MODIFIED_BY = User.Identity.Name;

            //Write to DB
            GenericData.Update<DAILY_ACTIVITY_PRODUCTION>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");

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
            }
        }

        protected void deStoreExpenditureType(object sender, DirectEventArgs e)
        {
            uxAddProductionExpenditureType.SetValue(e.ExtraParams["ExpenditureType"], e.ExtraParams["ExpenditureType"]);
            uxAddProductionUOM.SetValue(e.ExtraParams["UnitOfMeasure"]);
            uxAddProductionBillRate.SetValue(e.ExtraParams["BillRate"]);

            uxAddProductionExpenditureStore.ClearFilter();
        }

        protected void deStoreTask(object sender, DirectEventArgs e)
        {
                uxAddProductionTask.SetValue(e.ExtraParams["TaskId"], e.ExtraParams["Description"]);
                uxAddProductionTaskStore.ClearFilter();
        }
    }
}