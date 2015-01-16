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
    public partial class umAddEditMonthTask : BasePage
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
                uxHidProjectID.Text = Request.QueryString["projectID"];
                uxHidProjectNum.Text = Request.QueryString["projectNum"];
                uxHidProjectName.Text = Request.QueryString["projectName"];
                uxHidType.Text = Request.QueryString["projectType"];

                bool addNew = Convert.ToBoolean(Request.QueryString["addNew"]);
                uxHidAddNew.Text = addNew.ToString();

                if (addNew == true)
                {
                
                    uxHidDetailTaskID.Text="";
                    uxHidDetailID.Text = "";
                    uxHidDetailName.Text = "";
                    uxHidDetailType.Text = "";
                }
                else
                {
                    string detailTaskID = Request.QueryString["detailTaskID"];
                    string detailID = Request.QueryString["detailID"];
                    string detailNum = Request.QueryString["detailNumber"];
                    string detailName = Request.QueryString["detailName"];
                    string detailType = Request.QueryString["detailType"];

                    uxHidDetailTaskID.Text = detailTaskID;
                    uxHidDetailID.Text = detailID;
                    uxHidDetailNum.Text = detailNum;
                    uxHidDetailName.Text = detailName;
                    uxHidDetailType.Text = detailType;

                    uxTaskNum.SetValue(detailTaskID, detailNum);
                    uxTaskName.Text = uxHidDetailName.Text;

                    if (detailType == "OVERRIDE") { uxTaskName.ReadOnly = false; }
                    uxSave.Enable();
                }
            }
        }

        protected void deLoadTaskDropdown(object sender, StoreReadDataEventArgs e)
        {
            long projectID = Convert.ToInt64(uxHidProjectID.Text);
            List<object> dataSource = BBMonthSummary.Tasks.Data(projectID).ToList<object>();
            int count;

            uxTaskNumStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }

        protected void deSelectTask(object sender, DirectEventArgs e)
        {
            string taskID = e.ExtraParams["TaskID"];
            string taskType = e.ExtraParams["Type"];
            string taskNum = taskType == "OVERRIDE" ? "-- OVERRIDE --" : e.ExtraParams["TaskNum"];
            string taskName = taskType == "OVERRIDE" ? null : e.ExtraParams["TaskName"];

            uxTaskNum.SetValue(taskID, taskNum);
            uxTaskName.Text = taskName;            
            uxHidDetailID.Text = taskID;
            uxHidDetailNum.Text = taskNum;
            uxHidDetailName.Text = uxTaskName.Text;
            uxHidDetailType.Text = taskType;

            if (taskType == "OVERRIDE")
            {
                uxTaskName.ReadOnly = false;
                uxHidDetailID.Text = DateTime.Now.ToString("yyMMddHHmmss");
            }
            else
            {
                uxTaskName.ReadOnly = true;
            }
            uxTaskFilter.ClearFilter();

            deCheckAllowSave(sender, e);
        }

        protected void deCheckAllowSave(object sender, DirectEventArgs e)
        {
            char[] charsToTrim = { ' ', '\t' };
            string taskName = uxTaskName.Text.Trim(charsToTrim);

            if (taskName == "")
            {
                uxSave.Disable();
            }
            else
            {
                uxSave.Enable();
            }
        }
        
        // Save task
        protected void deSave(object sender, DirectEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            long yearID = long.Parse(Request.QueryString["yearID"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);
            long detailTaskID = uxHidDetailTaskID.Text == "" ? 0 : Convert.ToInt64(uxHidDetailTaskID.Text);
            long detailID = Convert.ToInt64(uxHidDetailID.Text);     

            if (BBMonthSummary.Tasks.Count(orgID, yearID, verID, budBidID, detailID, detailTaskID) == 0)
            {
                if (uxHidAddNew.Text == "True")
                {
                    SaveInsertNewRecord();
                }
                else
                {
                    SaveUpdateExistingRecord();
                }
            }
            else
            {
                StandardMsgBox("Exists", "This task already exists.  Please select a different task or edit/delete the existing one.", "INFO");
                return;
            }

            X.Js.Call("closeUpdate");
        }
        protected void SaveInsertNewRecord()
        {
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            BUD_BID_DETAIL_TASK taskData = DetailSheet(true, budBidID);
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
            BUD_BID_DETAIL_TASK detailData = DetailSheet(false, budBidID);

            GenericData.Update<BUD_BID_DETAIL_TASK>(detailData);
        }
        protected BUD_BID_DETAIL_TASK DetailSheet(bool insert, long budBidID)
        {
            BUD_BID_DETAIL_TASK data;

            if (insert == true)
            {
                data = new BUD_BID_DETAIL_TASK();
                data.TASK_ID = uxHidDetailID.Text;
                if (uxTaskNum.Text == "-- OVERRIDE --")
                {
                    data.DETAIL_NAME = uxTaskName.Text;
                }
                else
                {
                    data.DETAIL_NAME = null;
                }
                data.PROJECT_ID = Convert.ToInt64(uxHidBudBidID.Text);
                data.TASK_ID = uxHidDetailID.Text;
                data.TASK_TYPE = uxHidDetailType.Text;
                data.SHEET_ORDER = 0;
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            }
            else
            {
                long detailTaskID = Convert.ToInt64(uxHidDetailTaskID.Text);
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailTaskID).Single();
                }

                data.TASK_ID = uxHidDetailID.Text;
                if (uxTaskNum.Text == "-- OVERRIDE --")
                {
                    data.DETAIL_NAME = uxTaskName.Text;
                }
                else
                {
                    data.DETAIL_NAME = null;
                }
                data.TASK_TYPE = uxHidDetailType.Text;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            }
            return data;
        }
        protected List<BUD_BID_ACTUAL_NUM> ProjectFormStartData(bool insert, long budBidID, long detailTaskID = 0)
        {
            long[] arrLineNum = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360 };
            List<BUD_BID_ACTUAL_NUM> data = null;

            if (insert == true)
            {
                data = new List<BUD_BID_ACTUAL_NUM>();

                for (int i = 0; i <= 17; i++)
                {
                    data.Add(new BUD_BID_ACTUAL_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = detailTaskID,
                        LINE_ID = arrLineNum[i],
                        NOV = 0,
                        DEC = 0,
                        JAN = 0,
                        FEB = 0,
                        MAR = 0,
                        APR = 0,
                        MAY = 0,
                        JUN = 0,
                        JUL = 0,
                        AUG = 0,
                        SEP = 0,
                        OCT = 0,
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = User.Identity.Name,
                    });
                }
            }

            return data;
        }
        protected List<BUD_BID_BUDGET_NUM> ProjectFormEndData(bool insert, long budBidID, long detailTaskID = 0)
        {
            long[] arrLineNum = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360 };
            List<BUD_BID_BUDGET_NUM> data = null;

            if (insert == true)
            {
                data = new List<BUD_BID_BUDGET_NUM>();

                for (int i = 0; i <= 17; i++)
                {
                    data.Add(new BUD_BID_BUDGET_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = detailTaskID,
                        LINE_ID = arrLineNum[i],
                        NOV = 0,
                        DEC = 0,
                        JAN = 0,
                        FEB = 0,
                        MAR = 0,
                        APR = 0,
                        MAY = 0,
                        JUN = 0,
                        JUL = 0,
                        AUG = 0,
                        SEP = 0,
                        OCT = 0,
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = User.Identity.Name,
                    });
                }
            }

            return data;
        }

                
        
        // Other
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