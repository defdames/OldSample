using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umAddEditMonthProject : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                uxHidBudBidID.Text = Request.QueryString["budBidID"];
                uxHidProjectNumID.Text = Request.QueryString["projectNumID"];
            }
        }

        protected void deLoadProjectDropdown(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            string orgName = Request.QueryString["orgName"];
            List<object> dataSource = BBProject.Listing.Data(orgID, orgName).ToList<object>();
            int count;

            uxProjectNumStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string projectID = e.ExtraParams["ProjectID"];
            string projectType = e.ExtraParams["Type"];
            string projectNum = projectType == "OVERRIDE" ? "-- OVERRIDE --" : e.ExtraParams["ProjectNum"];
            string projectName = projectType == "OVERRIDE" ? null : e.ExtraParams["ProjectName"];

            uxProjectNum.SetValue(projectID, projectNum);
            uxProjectName.Text = projectName;
            uxHidProjectNumID.Text = projectID;
            uxHidType.Text = projectType;

            if (projectType == "OVERRIDE")
            {
                uxProjectName.ReadOnly = false;
                uxHidProjectNumID.Text = DateTime.Now.ToString("yyMMddHHmmss");
            }
            else
            {
                uxProjectName.ReadOnly = true;
            }
            uxProjectFilter.ClearFilter();

            deCheckAllowSave(sender, e);
        }

        protected void deCheckAllowSave(object sender, DirectEventArgs e)
        {
            char[] charsToTrim = { ' ', '\t' };
            string projectName = uxProjectName.Text.Trim(charsToTrim);

            if (projectName == "")
            {
                uxSave.Disable();
            }
            else
            {
                uxSave.Enable();
            }
        }

   

        // Save project
        protected void deSave(object sender, DirectEventArgs e)
        {
            //long orgID = long.Parse(Request.QueryString["OrgID"]);
            //long yearID = long.Parse(Request.QueryString["yearID"]);
            //long verID = long.Parse(Request.QueryString["verID"]);
            //string budBidID = uxHidBudBidID.Text; 
            //string projectID = uxHidProjectNumID.Text;

            //if (BBProject.Count(orgID, yearID, verID, projectID, curBudBidID) == 0)
            //{
                SaveInsertNewRecord();
            //}
            //else
            //{
                //SaveUpdateExistingRecord();
            //}
                        
            X.Js.Call("closeUpdate");
        }
        protected void SaveInsertNewRecord()
        {
            BUD_BID_PROJECTS projectData = ProjectFormDetailData(true);
            GenericData.Insert<BUD_BID_PROJECTS>(projectData);

            // Get newly created project id from BUD_BID_PROJECTS table
            long budBidID = Convert.ToInt64(projectData.BUD_BID_PROJECTS_ID);
            uxHidBudBidID.Text = budBidID.ToString();

            BUD_BID_DETAIL_TASK taskData = CreateProjectLevelDetailSheet(budBidID);
            GenericData.Insert<BUD_BID_DETAIL_TASK>(taskData);

            // Get newly created detail sheet id from BUD_BID_DETAIL_TASK table
            long detailTaskID = Convert.ToInt64(taskData.DETAIL_TASK_ID);

            List<BUD_BID_ACTUAL_NUM> startData = ProjectFormStartData(true, budBidID, detailTaskID);
            GenericData.Insert<BUD_BID_ACTUAL_NUM>(startData);

            List<BUD_BID_BUDGET_NUM> endData = ProjectFormEndData(true, budBidID, detailTaskID);
            GenericData.Insert<BUD_BID_BUDGET_NUM>(endData);
        }
        protected void SaveUpdateExistingRecord()
        {
            long budBidID = long.Parse(Request.QueryString["budBidID"]);
            BUD_BID_PROJECTS projectData = ProjectFormDetailData(false, budBidID);
            List<BUD_BID_ACTUAL_NUM> startData = ProjectFormStartData(false, budBidID);
            List<BUD_BID_BUDGET_NUM> endData = ProjectFormEndData(false, budBidID);

            GenericData.Update<BUD_BID_PROJECTS>(projectData);
            GenericData.Update<BUD_BID_ACTUAL_NUM>(startData);
            GenericData.Update<BUD_BID_BUDGET_NUM>(endData);

            UpdateDetailSheetsModifiedDateAndBy(budBidID);
        }
        protected BUD_BID_PROJECTS ProjectFormDetailData(bool insert, long budBidID = 0)
        {
            BUD_BID_PROJECTS data;

            //if (insert == true)
            //{
                data = new BUD_BID_PROJECTS();
                data.PROJECT_ID = uxHidProjectNumID.Text;
                data.ORG_ID = Convert.ToInt64(Request.QueryString["orgID"]);
                data.YEAR_ID = Convert.ToInt64(Request.QueryString["yearID"]);
                data.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            //}
            //else
            //{
            //    using (Entities context = new Entities())
            //    {
            //        data = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
            //    }

            //    data.PROJECT_ID = uxHidProjectNumID.Text;
            //    data.ORG_ID = Convert.ToInt64(Request.QueryString["orgID"]);
            //    data.YEAR_ID = Convert.ToInt64(Request.QueryString["yearID"]);
            //    data.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
            //    data.STATUS_ID = Convert.ToInt64(uxHidStatusID.Text);
            //    data.ACRES = ForceToDecimal(uxAcres.Text, -9999999999.99M, 9999999999.99M);
            //    data.DAYS = ForceToDecimal(uxDays.Text, -9999999999.99M, 9999999999.99M);
            //    data.APP_TYPE = uxAppType.Text;
            //    data.CHEMICAL_MIX = uxChemMix.Text;
            //    data.COMMENTS = uxComments.Text;

            //    if (uxProjectNum.Text == "-- OVERRIDE --")
            //    {
            //        data.PRJ_NAME = uxProjectName.Text;
            //    }
            //    else
            //    {
            //        data.PRJ_NAME = null;
            //    }

            //    if (uxJCDate.Text == "")
            //    {
            //        data.WE_OVERRIDE = null;
            //        data.WE_DATE = null;
            //    }
            //    else
            //    {
            //        if (uxJCDate.Text == "-- OVERRIDE --")
            //        {
            //            data.WE_OVERRIDE = "Y";
            //            data.WE_DATE = null;
            //        }
            //        else
            //        {
            //            data.WE_OVERRIDE = "N";
            //            data.WE_DATE = Convert.ToDateTime(uxJCDate.Text);
            //        }
            //    }


            //    data.TYPE = uxHidType.Text;
            //    data.LIABILITY = uxLiabilityCheckbox.Checked == true ? "Y" : "N";
            //    data.LIABILITY_OP = ForceToDecimal(uxLiabilityAmount.Text, -9999999999.99M, 9999999999.99M);
            //    bool overridenOP = uxCompareOverride.Checked;
            //    data.COMPARE_PRJ_OVERRIDE = overridenOP == true ? "Y" : "N";

            //    if (overridenOP == true)
            //    {
            //        data.COMPARE_PRJ_AMOUNT = ForceToDecimal(uxCompareOP.Text);
            //    }
            //    else
            //    {
            //        data.COMPARE_PRJ_AMOUNT = 0;
            //    }

            //    data.MODIFY_DATE = DateTime.Now;
            //    data.MODIFIED_BY = User.Identity.Name;
            //}
            return data;
        }
        protected BUD_BID_DETAIL_TASK CreateProjectLevelDetailSheet(long budBidID)
        {
            BUD_BID_DETAIL_TASK data = new BUD_BID_DETAIL_TASK();

            data.PROJECT_ID = Convert.ToInt64(uxHidBudBidID.Text);//long.Parse(Request.QueryString["budBidID"]);
            data.DETAIL_NAME = "SYS_PROJECT";
            data.SHEET_ORDER = 0;
            data.CREATE_DATE = DateTime.Now;
            data.CREATED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = User.Identity.Name;

            return data;
        }
        protected List<BUD_BID_ACTUAL_NUM> ProjectFormStartData(bool insert, long budBidID, long detailTaskID = 0)
        {
            long[] arrLineNum = { 6, 7, 8, 9, 10 };
            //Ext.Net.TextField[] arrControl = { uxSGrossRec, uxSMatUsage, uxSGrossRev, uxSDirects, uxSOP };
            List<BUD_BID_ACTUAL_NUM> data;

            //if (insert == true)
            //{
                data = new List<BUD_BID_ACTUAL_NUM>();

                for (int i = 0; i <= 4; i++)
                {
                    data.Add(new BUD_BID_ACTUAL_NUM
                    {
                        PROJECT_ID = budBidID,                        
                        DETAIL_TASK_ID = detailTaskID,
                        LINE_ID = arrLineNum[i],
                        NOV = ForceToDecimal("0", -9999999999.99M, 9999999999.99M),//arrControl[i].Text, -9999999999.99M, 9999999999.99M),
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = "TEMP"
                    });
                }
            //}
//            else
//            {
//                using (Entities context = new Entities())
//                {
//                    string sql = string.Format(@"
//                        SELECT BUD_BID_ACTUAL_NUM.ACTUAL_NUM_ID,
//                            BUD_BID_ACTUAL_NUM.PROJECT_ID,
//                            BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID,
//                            BUD_BID_ACTUAL_NUM.LINE_ID,
//                            BUD_BID_ACTUAL_NUM.NOV,
//                            BUD_BID_ACTUAL_NUM.DEC,
//                            BUD_BID_ACTUAL_NUM.JAN,
//                            BUD_BID_ACTUAL_NUM.FEB,
//                            BUD_BID_ACTUAL_NUM.MAR,
//                            BUD_BID_ACTUAL_NUM.APR,
//                            BUD_BID_ACTUAL_NUM.MAY,
//                            BUD_BID_ACTUAL_NUM.JUN,
//                            BUD_BID_ACTUAL_NUM.JUL,
//                            BUD_BID_ACTUAL_NUM.AUG,
//                            BUD_BID_ACTUAL_NUM.SEP,
//                            BUD_BID_ACTUAL_NUM.OCT,
//                            BUD_BID_ACTUAL_NUM.CREATE_DATE,
//                            BUD_BID_ACTUAL_NUM.CREATED_BY,
//                            BUD_BID_ACTUAL_NUM.MODIFY_DATE,
//                            BUD_BID_ACTUAL_NUM.MODIFIED_BY
//                        FROM BUD_BID_DETAIL_TASK
//                        LEFT JOIN BUD_BID_ACTUAL_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_ACTUAL_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID
//                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
//                        ORDER BY LINE_ID", budBidID);
//                    data = context.Database.SqlQuery<BUD_BID_ACTUAL_NUM>(sql).ToList();
//                }

//                int a = 0;
//                foreach (BUD_BID_ACTUAL_NUM field in data)
//                {
//                    field.NOV = ForceToDecimal("0", -9999999999.99M, 9999999999.99M);//arrControl[a].Text, -9999999999.99M, 9999999999.99M);
//                    field.MODIFY_DATE = DateTime.Now;
//                    field.MODIFIED_BY = User.Identity.Name;
//                    a++;
//                }
 //           }

            return data;
        }
        protected List<BUD_BID_BUDGET_NUM> ProjectFormEndData(bool insert, long budBidID, long detailTaskID = 0)
        {
            long[] arrLineNum = { 6, 7, 8, 9, 10 };
            //Ext.Net.TextField[] arrControl = { uxEGrossRec, uxEMatUsage, uxEGrossRev, uxEDirects, uxEOP };
            List<BUD_BID_BUDGET_NUM> data;

            if (insert == true)
            {
                data = new List<BUD_BID_BUDGET_NUM>();

                for (int i = 0; i <= 4; i++)
                {
                    data.Add(new BUD_BID_BUDGET_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = detailTaskID,
                        LINE_ID = arrLineNum[i],
                        NOV = ForceToDecimal("0", -9999999999.99M, 9999999999.99M),//arrControl[i].Text, -9999999999.99M, 9999999999.99M),
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = "TEMP"
                    });
                }
            }
            else
            {
                using (Entities context = new Entities())
                {
                    string sql = string.Format(@"
                        SELECT BUD_BID_BUDGET_NUM.BUDGET_NUM_ID,
                            BUD_BID_BUDGET_NUM.PROJECT_ID,
                            BUD_BID_BUDGET_NUM.DETAIL_TASK_ID,
                            BUD_BID_BUDGET_NUM.LINE_ID,
                            BUD_BID_BUDGET_NUM.NOV,
                            BUD_BID_BUDGET_NUM.DEC,
                            BUD_BID_BUDGET_NUM.JAN,
                            BUD_BID_BUDGET_NUM.FEB,
                            BUD_BID_BUDGET_NUM.MAR,
                            BUD_BID_BUDGET_NUM.APR,
                            BUD_BID_BUDGET_NUM.MAY,
                            BUD_BID_BUDGET_NUM.JUN,
                            BUD_BID_BUDGET_NUM.JUL,
                            BUD_BID_BUDGET_NUM.AUG,
                            BUD_BID_BUDGET_NUM.SEP,
                            BUD_BID_BUDGET_NUM.OCT,
                            BUD_BID_BUDGET_NUM.CREATE_DATE,
                            BUD_BID_BUDGET_NUM.CREATED_BY,
                            BUD_BID_BUDGET_NUM.MODIFY_DATE,
                            BUD_BID_BUDGET_NUM.MODIFIED_BY
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
                        ORDER BY LINE_ID", budBidID);
                    data = context.Database.SqlQuery<BUD_BID_BUDGET_NUM>(sql).ToList();

                    int a = 0;
                    foreach (BUD_BID_BUDGET_NUM field in data)
                    {
                        field.NOV = ForceToDecimal("0", -9999999999.99M, 9999999999.99M);//arrControl[a].Text, -9999999999.99M, 9999999999.99M);
                        field.MODIFY_DATE = DateTime.Now;
                        field.MODIFIED_BY = User.Identity.Name;
                        a++;
                    }
                }
            }
            return data;
        }
        protected void UpdateDetailSheetsModifiedDateAndBy(long budBidID)
        {
            List<BUD_BID_DETAIL_TASK> taskData;
            using (Entities context = new Entities())
            {
                taskData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            foreach (BUD_BID_DETAIL_TASK taskField in taskData)
            {
                taskField.MODIFY_DATE = DateTime.Now;
                taskField.MODIFIED_BY = User.Identity.Name;
            }
            GenericData.Update<BUD_BID_DETAIL_TASK>(taskData);

            List<BUD_BID_BUDGET_NUM> budgetData;
            using (Entities context = new Entities())
            {
                budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            foreach (BUD_BID_BUDGET_NUM budgetField in budgetData)
            {
                budgetField.MODIFY_DATE = DateTime.Now;
                budgetField.MODIFIED_BY = User.Identity.Name;
            }
            GenericData.Update<BUD_BID_BUDGET_NUM>(budgetData);

            List<BUD_BID_DETAIL_SHEET> detailData;
            using (Entities context = new Entities())
            {
                detailData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            foreach (BUD_BID_DETAIL_SHEET detailField in detailData)
            {
                detailField.MODIFY_DATE = DateTime.Now;
                detailField.MODIFIED_BY = User.Identity.Name;
            }
            GenericData.Update<BUD_BID_DETAIL_SHEET>(detailData);
        }










        protected void StandardMsgBox(string title, string msg, string msgIcon)
        {
            // msgIcon can be:  ERROR, INFO, QUESTION, WARNING
            X.Msg.Show(new MessageBoxConfig()
            {
                Title = title,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), msgIcon)
            });
        }

        protected decimal ForceToDecimal(string number)
        {
            decimal amount;
            decimal.TryParse(number, out amount);
            return amount;
        }
        protected decimal ForceToDecimal(string number, decimal lowRange, decimal highRange)
        {
            decimal amount;
            decimal.TryParse(number, out amount);
            if (amount < lowRange || amount > highRange) { amount = 0; }
            return amount;
        }

    }
}