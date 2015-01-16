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

                bool addNew = Convert.ToBoolean(Request.QueryString["addNew"]);
                uxHidAddNew.Text = addNew.ToString();

                if (addNew == true)
                {
                    uxHidBudBidID.Text = "";
                    uxHidProjectID.Text = "";
                    uxHidProjectNum.Text = "";
                    uxHidProjectName.Text = "";
                    uxHidType.Text = "";
                }
                else
                {
                    string budBBidID = Request.QueryString["budBidID"];
                    string projectID = Request.QueryString["projectID"];
                    string projectNum = Request.QueryString["projectNum"];
                    string projectName = Request.QueryString["projectName"];
                    string projectType = Request.QueryString["projectType"];

                    uxHidBudBidID.Text = budBBidID;
                    uxHidProjectID.Text = projectID;
                    uxHidProjectNum.Text = projectNum;
                    uxHidProjectName.Text = projectName;
                    uxHidType.Text = projectType;

                    uxProjectNum.SetValue(projectID, projectNum);
                    uxProjectName.Text = projectName;

                    if (projectType == "OVERRIDE") { uxProjectName.ReadOnly = false; }
                    uxSave.Enable();
                }
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
            uxHidProjectID.Text = projectID;
            uxHidProjectName.Text = uxProjectName.Text;
            uxHidType.Text = projectType;
 
            if (projectType == "OVERRIDE")
            {
                uxProjectName.ReadOnly = false;
                uxHidProjectID.Text = DateTime.Now.ToString("yyMMddHHmmss");
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
            long orgID = long.Parse(Request.QueryString["orgID"]);
            long yearID = long.Parse(Request.QueryString["yearID"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long budBidID = uxHidBudBidID.Text == "" ? 0 : Convert.ToInt64(uxHidBudBidID.Text);           
            string projectID = uxHidProjectID.Text; 

            if (BBProject.Count(orgID, yearID, verID, projectID, budBidID) == 0)
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
                StandardMsgBox("Exists", "This project already exists.  Please select a different project or edit/delete the existing one.", "INFO");
                return;
            }
                                   
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

            List<BUD_BID_ACTUAL_NUM> startData = ProjectFormStartData(budBidID, detailTaskID);
            GenericData.Insert<BUD_BID_ACTUAL_NUM>(startData);

            List<BUD_BID_BUDGET_NUM> endData = ProjectFormEndData(budBidID, detailTaskID);
            GenericData.Insert<BUD_BID_BUDGET_NUM>(endData);
        }
        protected void SaveUpdateExistingRecord()
        {
            long budBidID = long.Parse(Request.QueryString["budBidID"]);
            BUD_BID_PROJECTS projectData = ProjectFormDetailData(false, budBidID);

            GenericData.Update<BUD_BID_PROJECTS>(projectData);
        }
        protected BUD_BID_PROJECTS ProjectFormDetailData(bool insert, long budBidID = 0)
        {
            BUD_BID_PROJECTS data;

            if (insert == true)
            {
                data = new BUD_BID_PROJECTS();
                data.PROJECT_ID = uxHidProjectID.Text;
                if (uxProjectNum.Text == "-- OVERRIDE --")
                {
                    data.PRJ_NAME = uxProjectName.Text;
                }
                else
                {
                    data.PRJ_NAME = null;
                }
                data.ORG_ID = Convert.ToInt64(Request.QueryString["orgID"]);
                data.YEAR_ID = Convert.ToInt64(Request.QueryString["yearID"]);
                data.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
                data.STATUS_ID = 47;
                data.ACRES = 0;
                data.DAYS = 0;
                data.APP_TYPE = null;
                data.CHEMICAL_MIX = null;
                data.WE_DATE = null;
                data.TYPE = uxHidType.Text;
                data.LIABILITY = "N";
                data.LIABILITY_OP = 0;
                data.COMPARE_PRJ_OVERRIDE = "N";
                data.COMPARE_PRJ_AMOUNT = 0;
                data.WE_OVERRIDE = "N";
                data.COMMENTS = "";
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            }
            else
            {
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
                }

                data.PROJECT_ID = uxHidProjectID.Text;
                if (uxProjectNum.Text == "-- OVERRIDE --")
                {
                    data.PRJ_NAME = uxProjectName.Text;
                }
                else
                {
                    data.PRJ_NAME = null;
                }
                data.TYPE = uxHidType.Text;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            }
            return data;
        }
        protected BUD_BID_DETAIL_TASK CreateProjectLevelDetailSheet(long budBidID)
        {
            BUD_BID_DETAIL_TASK data = new BUD_BID_DETAIL_TASK();

            data.PROJECT_ID = Convert.ToInt64(uxHidBudBidID.Text);
            data.DETAIL_NAME = "SYS_PROJECT";
            data.SHEET_ORDER = 0;
            data.CREATE_DATE = DateTime.Now;
            data.CREATED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = User.Identity.Name;

            return data;
        }
        protected List<BUD_BID_ACTUAL_NUM> ProjectFormStartData(long budBidID, long detailTaskID)
        {
            long[] arrLineNum = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360 };
            List<BUD_BID_ACTUAL_NUM> data = new List<BUD_BID_ACTUAL_NUM>();

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

            return data;
        }
        protected List<BUD_BID_BUDGET_NUM> ProjectFormEndData(long budBidID, long detailTaskID)
        {
            long[] arrLineNum = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360 };
            List<BUD_BID_BUDGET_NUM> data = new List<BUD_BID_BUDGET_NUM>();

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
    }
}