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

                bool showActualDropdown = Convert.ToBoolean(Request.QueryString["showActualDropdown"]);

                if (showActualDropdown == true)
                {
                    Toolbar1.Show();
                }
                else
                {
                    Toolbar1.Hide();
                }
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
            
            // Calculate and update total lines
            long projectID = Convert.ToInt64(e.ExtraParams["Project"]);
            long taskDetailID = Convert.ToInt64(e.ExtraParams["DetailTask"]);
            string modifiedDate = DateTime.Now.ToString("dd-MMM-yy");
            string modifiedBy = HttpContext.Current.User.Identity.Name;

            string sqlStart = string.Format(@"
                SELECT BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{0}', 'DD-Mon-YYYY')  MODIFY_DATE, '{1}' MODIFIED_BY       
                FROM
                (", modifiedDate, modifiedBy);

            string totalLineID = "60";
            string sqlGrossRec = string.Format(@"            
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (20, 40)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY", projectID, taskDetailID, totalLineID, modifiedDate, modifiedBy);

            totalLineID = "100";
            string sqlGrossRev = string.Format(@"                    
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (20, 40)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                    UNION ALL
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, -1 * SUM(NOV) NOV, -1 * SUM(DEC) DEC, -1 * SUM(JAN) JAN, -1 * SUM(FEB) FEB, -1 * SUM(MAR) MAR, -1 * SUM(APR) APR, -1 * SUM(MAY) MAY, -1 * SUM(JUN) JUN, -1 * SUM(JUL) JUL, -1 * SUM(AUG) AUG, -1 * SUM(SEP) SEP, -1 * SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (80)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY", projectID, taskDetailID, totalLineID, modifiedDate, modifiedBy);

            totalLineID = "160";
            string sqlTotal = string.Format(@"            
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (120, 140)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY", projectID, taskDetailID, totalLineID, modifiedDate, modifiedBy);

            totalLineID = "300";
            string sqlTotalDirects = string.Format(@"            
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (120, 140, 180, 200, 220, 240, 260, 280)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY", projectID, taskDetailID, totalLineID, modifiedDate, modifiedBy);

            totalLineID = "320";
            string sqlOP = string.Format(@"                    
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (20, 40)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                    UNION ALL
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, -1 * SUM(NOV) NOV, -1 * SUM(DEC) DEC, -1 * SUM(JAN) JAN, -1 * SUM(FEB) FEB, -1 * SUM(MAR) MAR, -1 * SUM(APR) APR, -1 * SUM(MAY) MAY, -1 * SUM(JUN) JUN, -1 * SUM(JUL) JUL, -1 * SUM(AUG) AUG, -1 * SUM(SEP) SEP, -1 * SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (80)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                    UNION ALL
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, -1 * SUM(NOV) NOV, -1 * SUM(DEC) DEC, -1 * SUM(JAN) JAN, -1 * SUM(FEB) FEB, -1 * SUM(MAR) MAR, -1 * SUM(APR) APR, -1 * SUM(MAY) MAY, -1 * SUM(JUN) JUN, -1 * SUM(JUL) JUL, -1 * SUM(AUG) AUG, -1 * SUM(SEP) SEP, -1 * SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (120, 140, 180, 200, 220, 240, 260, 280)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY", projectID, taskDetailID, totalLineID, modifiedDate, modifiedBy);

            totalLineID = "360";
            string sqlNetCont = string.Format(@"                    
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, SUM(NOV) NOV, SUM(DEC) DEC, SUM(JAN) JAN, SUM(FEB) FEB, SUM(MAR) MAR, SUM(APR) APR, SUM(MAY) MAY, SUM(JUN) JUN, SUM(JUL) JUL, SUM(AUG) AUG, SUM(SEP) SEP, SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (20, 40)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                    UNION ALL
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, -1 * SUM(NOV) NOV, -1 * SUM(DEC) DEC, -1 * SUM(JAN) JAN, -1 * SUM(FEB) FEB, -1 * SUM(MAR) MAR, -1 * SUM(APR) APR, -1 * SUM(MAY) MAY, -1 * SUM(JUN) JUN, -1 * SUM(JUL) JUL, -1 * SUM(AUG) AUG, -1 * SUM(SEP) SEP, -1 * SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (80)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                    UNION ALL
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, -1 * SUM(NOV) NOV, -1 * SUM(DEC) DEC, -1 * SUM(JAN) JAN, -1 * SUM(FEB) FEB, -1 * SUM(MAR) MAR, -1 * SUM(APR) APR, -1 * SUM(MAY) MAY, -1 * SUM(JUN) JUN, -1 * SUM(JUL) JUL, -1 * SUM(AUG) AUG, -1 * SUM(SEP) SEP, -1 * SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (120, 140, 180, 200, 220, 240, 260, 280)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                    UNION ALL
                SELECT (SELECT BUDGET_NUM_ID FROM BUD_BID_BUDGET_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, {2} LINE_ID, -1 * SUM(NOV) NOV, -1 * SUM(DEC) DEC, -1 * SUM(JAN) JAN, -1 * SUM(FEB) FEB, -1 * SUM(MAR) MAR, -1 * SUM(APR) APR, -1 * SUM(MAY) MAY, -1 * SUM(JUN) JUN, -1 * SUM(JUL) JUL, -1 * SUM(AUG) AUG, -1 * SUM(SEP) SEP, -1 * SUM(OCT) OCT, CREATE_DATE, CREATED_BY, TO_DATE('{3}', 'DD-Mon-YYYY')  MODIFY_DATE, '{4}' MODIFIED_BY
                FROM BUD_BID_BUDGET_NUM
                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID IN (340)
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY", projectID, taskDetailID, totalLineID, modifiedDate, modifiedBy);

            string sqlEnd = string.Format(@"
                )
                GROUP BY BUDGET_NUM_ID, PROJECT_ID, DETAIL_TASK_ID, LINE_ID, CREATE_DATE, CREATED_BY, MODIFY_DATE, MODIFIED_BY
                ORDER BY LINE_ID", modifiedDate, modifiedBy);

            string sql = sqlStart + sqlGrossRec + " UNION ALL " + sqlGrossRev + " UNION ALL " + sqlTotal + " UNION ALL " + sqlTotalDirects + " UNION ALL " + sqlOP + " UNION ALL " + sqlNetCont + sqlEnd;

            List<BUD_BID_BUDGET_NUM> totalData;
            using (Entities context = new Entities())
            {
                totalData = context.Database.SqlQuery<BUD_BID_BUDGET_NUM>(sql).ToList();
            }

            GenericData.Update<BUD_BID_BUDGET_NUM>(totalData);       
        }

        protected void deSelectBudgetsOrActuals(object sender, DirectEventArgs e)
        {
            uxDetailStore.Reload();
        }
    }
}