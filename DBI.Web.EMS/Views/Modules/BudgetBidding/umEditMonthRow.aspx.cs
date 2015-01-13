using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umEdmitMonthRow : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                uxBudgetsOrActuals.Text = "Budgets";
            }
        }

        protected void deLoadBudgetsOrActuals(object sender, StoreReadDataEventArgs e)
        {
            uxBudgetsOrActualsStore.DataSource = BBMonthSummary.MainGrid.BudgetsOrActuals();
        }

        protected void deLoadDetailLinesStore(object sender, StoreReadDataEventArgs e)
        {
            long lineID = Convert.ToInt64(Request.QueryString["lineID"]);
            long budBidProjectID = Convert.ToInt64(Request.QueryString["budBidProjectID"]);
            long detailSheetID = Convert.ToInt64(Request.QueryString["detailSheetID"]);

            bool budgetNums;
            if (uxBudgetsOrActuals.Text == "Budgets")
            {
                budgetNums = true;
            }
            else
            {
                budgetNums = false;
            }
            

            uxDetailStore.DataSource = BBMonthSummary.MainGrid.BudgetLine.Data(budgetNums, budBidProjectID, detailSheetID, lineID);
        }

        protected void deSaveDetailLine(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            List<BBMonthSummary.MainGrid.BudgetLine.Fields> _gridValues = JSON.Deserialize<List<BBMonthSummary.MainGrid.BudgetLine.Fields>>(json);

            long budgetNumID = Convert.ToInt64(e.ExtraParams["IDField"]);
            
            BUD_BID_BUDGET_NUM data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_BUDGET_NUM.Where(x => x.BUDGET_NUM_ID == budgetNumID).Single();
            }

            foreach (BBMonthSummary.MainGrid.BudgetLine.Fields _detail in _gridValues)
            {
                switch (_detail.MONTH)
                {
                    case "NOV":
                        data.NOV = _detail.AMOUNT;
                        break;

                    case "DEC":
                        data.DEC = _detail.AMOUNT;
                        break;

                    case "JAN":
                        data.JAN = _detail.AMOUNT;
                        break;

                    case "FEB":
                        data.FEB = _detail.AMOUNT;
                        break;

                    case "MAR":
                        data.MAR = _detail.AMOUNT;
                        break;

                    case "APR":
                        data.APR = _detail.AMOUNT;
                        break;

                    case "MAY":
                        data.MAY = _detail.AMOUNT;
                        break;

                    case "JUN":
                        data.JUN = _detail.AMOUNT;
                        break;

                    case "JUL":
                        data.JUL = _detail.AMOUNT;
                        break;

                    case "AUG":
                        data.AUG = _detail.AMOUNT;
                        break;

                    case "SEP":
                        data.SEP = _detail.AMOUNT;
                        break;

                    case "OCT":
                        data.OCT = _detail.AMOUNT;
                        break;
                }
            }

            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = HttpContext.Current.User.Identity.Name;

            GenericData.Update<BUD_BID_BUDGET_NUM>(data);
            
            // Calculate total lines
            long projectID = Convert.ToInt64(e.ExtraParams["Project"]);
            long taskDetailID = Convert.ToInt64(e.ExtraParams["DetailTask"]);
            
            // Update Gross Receipts
            BUD_BID_BUDGET_NUM data1;
            using (Entities context = new Entities())
            {
                data1 = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == projectID && x.DETAIL_TASK_ID == taskDetailID && x.LINE_ID == 60).Single();
            }

            data1.NOV = 9;

            //GenericData.Update<BUD_BID_BUDGET_NUM>(data);

            // Update Gross Revenue

            // Total

            // Total Directs

            // Operating Profit

            // Net Contribution

        }

        protected void deSelectBudgetsOrActuals(object sender, DirectEventArgs e)
        {
            uxDetailStore.Reload();
        }
    }
}