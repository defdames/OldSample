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
    public partial class umMonthBudget : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }



        // Project Grid
        protected void deLoadProjectActions(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxProjectActionsStore.DataSource = BBMonthSummary.ProjectActions(orgID, yearID, verID);
        }

        protected void deChooseProjectAction(object sender, DirectEventArgs e)
        {
            string selectedAction = uxProjectActions.Text;

            switch (selectedAction)
            {
                case "Add a New Project":
                    AddEditProject(true);
                    break;

                case "Edit Selected Project":
                    AddEditProject(false);
                    break;

                case "View Selected Project":
                    AddEditProject(false);
                    break;

                case "Copy Selected Project":
                    //CopySelectedProject();
                    break;

                case "Delete Selected Project":
                    DeleteSelectedProject();
                    break;

                case "Refresh Projects":
                    RefreshProjects();
                    break;
            }

            uxProjectActions.Clear();
        }

        protected void deReadProjectGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            string orgName = Request.QueryString["orgName"];
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = Convert.ToInt64(Request.QueryString["verID"]);

            uxProjectsStore.DataSource = BBMonthSummary.ProjectGrid.Data(orgName, orgID, yearID, verID);
        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string budBidprojectID = e.ExtraParams["BudBidProjectID"];
            string projectID = e.ExtraParams["ProjectID"];
            string projectNum = e.ExtraParams["ProjectNum"];
            string projectName = e.ExtraParams["ProjectName"];           
            string projectType = e.ExtraParams["Type"];

            uxHidBudBidID.Text = budBidprojectID;
            uxHidProjectID.Text = projectID;
            uxHidProjectNum.Text = projectNum;
            uxHidProjectName.Text = projectName;
            uxHidType.Text = projectType;

            uxHidDetailTaskID.Text = "";
            uxHidDetailID.Text = "";
            uxHidDetailName.Text = "";
            uxHidDetailType.Text = "";
 
            uxTasksStore.Reload();
            uxComments.Text = BBMonthSummary.Comments.Data(Convert.ToInt64(budBidprojectID));

            if (budBidprojectID == "0")
            {
                uxTasks.Disable();
                uxMonthDetailStore.Reload();
            }
            else
            {
                uxTasks.Enable();                
            }
        }

        protected void AddEditProject(bool addNew)
        {
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            string orgName = HttpUtility.UrlEncode(Request.QueryString["orgName"]);
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);
            string windowTitle;

            // Edit
            if (addNew == false)
            {
                if (budBidID == 0)
                {
                    if (BB.IsReadOnly(orgID, yearID, verID) == true)
                    {
                        StandardMsgBox("View", "A project must be selected before it can be viewed.", "INFO");
                    }
                    else
                    {
                        StandardMsgBox("Edit", "A project must be selected before it can be edited.", "INFO");
                    }
                    return;
                }

                if (BB.ProjectStillExists(budBidID) == false)
                {
                    StandardMsgBox("Edit", "Project has been deleted or has changed.  Please refresh projects.", "INFO");
                    return;
                }

                windowTitle = "Edit Project";
            }
            else
            {
                windowTitle = "Add New Project";
            }

            string projectID = uxHidProjectID.Text;
            string projectNumber = uxHidProjectNum.Text;
            string projectName = HttpUtility.UrlEncode(uxHidProjectName.Text);
            string projectType = uxHidType.Text;
            string url = "/Views/Modules/BudgetBidding/umAddEditMonthProject.aspx?orgID=" + orgID + " &orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&budBidID=" + budBidID + "&projectID=" + projectID + "&projectNum=" + projectNumber + "&projectName=" + projectName + "&projectType=" + projectType + "&addNew=" + addNew;
            
            Window win = new Window
            {
                ID = "uxAddEditProjectForm",
                Height = 700,//410,
                Width = 700,
                Title = windowTitle,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
                Closable = false,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = url,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };
            win.Render(this.Form);
            win.Show();
        }

        protected void DeleteSelectedProject()
        {
            if (uxHidBudBidID.Text == "0")
            {
                StandardMsgBox("Delete", "A project must be selected before it can be deleted.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Delete", "Are you sure you want to delete the selected project? Once it's been deleted it cannot be retrieved.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteSelectedProjectContiued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }
        [DirectMethod]
        public void DeleteSelectedProjectContiued()
        {
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            if (BB.ProjectStillExists(budBidID) == false)
            {
                StandardMsgBox("Delete", "Project has already been deleted or has changed.  Please refresh summary", "INFO");
                return;
            }

            BBProject.DBDelete(budBidID);
            uxHidBudBidID.Text = "0";  // This is in here for task grid refresh.
            uxProjectsStore.Reload();
            uxTasksStore.Reload();
            uxMonthDetailStore.Reload();
        }

        protected void RefreshProjects()
        {
            uxHidBudBidID.Text = "0";  // This is in here for task grid refresh.
            uxProjectsStore.Reload();
            uxTasksStore.Reload();
            uxMonthDetailStore.Reload();
        }

        [DirectMethod]
        public void CloseAddEditProjectWindow()
        {
            uxProjectsStore.Reload();
            uxTaskActionsStore.Reload();
            uxMonthDetailStore.Reload();
        }
        


        // Task Grid
        protected void deLoadTaskActions(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxTaskActionsStore.DataSource = BBMonthSummary.TaskActions(orgID, yearID, verID);
        }

        protected void deChooseTaskAction(object sender, DirectEventArgs e)
        {
            string selectedAction = uxTaskActions.Text;

            switch (selectedAction)
            {
                case "Add a New Task":
                    AddEditTask(true);
                    break;

                case "Auto-Generate Tasks":
                    //AutoGenerateTasks
                    break;

                case "Edit Selected Task":
                    AddEditTask(false);
                    break;

                case "Copy Selected Task":
                    //CopySelectedTask();
                    break;

                case "Delete Selected Task":
                    DeleteSelectedTask();
                    break;

                case "Refresh Tasks":
                    RefreshTasks();
                    break;
            }

            uxTaskActions.Clear();
        }

        protected void deReadTaskGridData(object sender, StoreReadDataEventArgs e)
        {
            uxTasksStore.DataSource = BBMonthSummary.TaskGrid.Data(Convert.ToInt64(uxHidBudBidID.Text));        
        }

        protected void deSelectTask(object sender, DirectEventArgs e)
        {
            string detailTaskID = e.ExtraParams["DetailTaskID"];
            string detailID = e.ExtraParams["DetailID"];
            string detailNum = e.ExtraParams["DetailNum"];
            string detailName = e.ExtraParams["DetailName"];
            string type = e.ExtraParams["Type"];
            uxHidDetailTaskID.Text = detailTaskID;
            uxHidDetailID.Text = detailID;
            uxHidDetailNum.Text = detailNum;
            uxHidDetailName.Text = detailName;
            uxHidDetailType.Text = type;

            if (Convert.ToInt64(detailTaskID) == 0)
            {
                long budBidprojectID = Convert.ToInt64(uxHidBudBidID.Text);
                uxComments.Text = BBMonthSummary.Comments.Data(budBidprojectID);
            }
            else
            {
                uxComments.Text = BBDetail.Sheet.MainTabField.Comment(Convert.ToInt64(detailTaskID));
            }

            uxMonthDetailStore.Reload();
        }

        protected void AddEditTask(bool addNew)
        {
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            string orgName = HttpUtility.UrlEncode(Request.QueryString["orgName"]);
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);
            long detailTaskID = Convert.ToInt64(uxHidDetailTaskID.Text);
            string windowTitle;

            // Edit
            if (addNew == false)
            {
                if (detailTaskID == 0)
                {
                    if (BB.IsReadOnly(orgID, yearID, verID) == true)
                    {
                        StandardMsgBox("View", "A task must be selected before it can be viewed.", "INFO");
                    }
                    else
                    {
                        StandardMsgBox("Edit", "A task must be selected before it can be edited.", "INFO");
                    }
                    return;
                }

                if (BBMonthSummary.TaskStillExists(detailTaskID) == false)
                {
                    StandardMsgBox("Edit", "Task has been deleted or has changed.  Please refresh tasks.", "INFO");
                    return;
                }

                windowTitle = "Edit Task";
            }
            else
            {
                windowTitle = "Add New Task";
            }

            string projectID = uxHidProjectID.Text;
            string projectNumber = uxHidProjectNum.Text;
            string projectName = HttpUtility.UrlEncode(uxHidProjectName.Text);
            string projectType = uxHidType.Text;
            string detailID = uxHidDetailID.Text;
            string detailNumber = uxHidDetailNum.Text;
            string detailName = uxHidDetailName.Text;
            string detailType = uxHidDetailType.Text;

            string url = "/Views/Modules/BudgetBidding/umAddEditMonthTask.aspx?orgID=" + orgID + " &orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&budBidID=" + budBidID + "&projectID=" + projectID + "&projectNum=" + projectNumber + "&projectName=" + projectName + "&projectType=" + projectType + "&detailTaskID=" + detailTaskID + "&detailID=" + detailID + "&detailNumber=" + detailNumber + "&detailName=" + detailName + "&detailType=" + detailType + "&addNew=" + addNew;

            Window win = new Window
            {
                ID = "uxAddEditTaskForm",
                Height = 700,//410,
                Width = 700,
                Title = windowTitle,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
                Closable = false,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = url,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };
            win.Render(this.Form);
            win.Show();
        }

        protected void DeleteSelectedTask()
        {
            if (uxHidDetailTaskID.Text == "0")
            {
                StandardMsgBox("Delete", "A task must be selected before it can be deleted.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Delete", "Are you sure you want to delete the selected task? Once it's been deleted it cannot be retrieved.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteSelectedTaskContiued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }
        [DirectMethod]
        public void DeleteSelectedTaskContiued()
        {
            long detailTaskID = Convert.ToInt64(uxHidDetailTaskID.Text);

            if (BBMonthSummary.TaskStillExists(detailTaskID) == false)
            {
                StandardMsgBox("Delete", "Task has already been deleted or has changed.  Please refresh summary", "INFO");
                return;
            }

            BBMonthSummary.Tasks.DBDelete(detailTaskID);
            uxTasksStore.Reload();
            uxMonthDetailStore.Reload();
        }

        protected void RefreshTasks()
        {
            uxTasksStore.Reload();
            uxMonthDetailStore.Reload();
        }

        [DirectMethod]
        public void CloseAddEditTaskWindow()
        {
            uxProjectsStore.Reload();
            uxTaskActionsStore.Reload();
            uxMonthDetailStore.Reload();
        }
        


        // Main Grid
        protected void deReadMainGridData(object sender, StoreReadDataEventArgs e)
        {
            string orgID = Request.QueryString["orgID"];
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailTaskID.Text;
            long weMonth = BB.WEDateMonth("09-Jun-2012");  //FIX

            uxMonthDetailStore.DataSource = BBMonthSummary.MainGrid.Data(yearID, verID, weMonth, orgID, budBidProjectID, detailSheetID);
        }

        protected void deEditSelectedRow(object sender, DirectEventArgs e)
        {
            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailTaskID.Text;


            if (detailSheetID == "0") { return; }

            string[] arrLineNum = { "60", "100", "160", "300", "320", "360" };
            string lineID = e.ExtraParams["LineID"];


            int pos = Array.IndexOf(arrLineNum, lineID);
            if (pos > -1)
            {
                return;
            }

            string lineDesc = e.ExtraParams["LineDesc"];
            bool showActualDropdown = lineID == "340" ? true : false;
            //string hierID = Request.QueryString["hierID"];
            //string leOrgID = Request.QueryString["leOrgID"];
            //long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            //long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            //long verID = long.Parse(Request.QueryString["verID"]);
            //string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);

            string url = "/Views/Modules/BudgetBidding/umEditMonthRow.aspx?budBidProjectID=" + budBidProjectID + "&detailSheetID=" + detailSheetID + "&lineID=" + lineID + "&showActualDropdown=" + showActualDropdown;

            Window win = new Window
            {
                ID = "uxDetailLineMaintenance",
                Height = 465,
                Width = 500,
                Title = lineDesc,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
                Closable = false,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = url,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };
            win.Render(this.Form);
            win.Show();
        }
        [DirectMethod]
        public void CloseUpdateAllActualsWindow()
        {
            uxMonthDetailStore.Reload();
        }

        protected void deLoadSummaryReports(object sender, StoreReadDataEventArgs e)
        {

        }

        protected void deChooseSummaryReport(object sender, DirectEventArgs e)
        {

        }

        protected void deUpdateAllActuals(object sender, DirectEventArgs e)
        {
            UpdateJCNumbers();
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





        protected void UpdateJCNumbers()
        {
            string weDate = "09-Jun-2012";  // FIX TO BE JC WE DATE FY!    
            string fiscalYear = BB.WEDateFiscalYear(weDate);   
            string budBidProjectID = uxHidBudBidID.Text;
            string projectID = uxHidProjectID.Text;
            string detailTaskID = uxHidDetailTaskID.Text;
            string taskID = uxHidDetailID.Text;
            string fiscalYearStart = BB.WEDateFiscalYearStartDate(fiscalYear);
            string modifiedDate = DateTime.Now.ToString("dd-MMM-yy");
            string modifiedBy = HttpContext.Current.User.Identity.Name;
            
            string sql1 = string.Format(@"
                WITH        
                    JC_PERIODS AS (
                        SELECT PERIOD_NAME, START_DATE, END_DATE, PERIOD_TYPE, ENTERED_PERIOD_NAME
                        FROM APPS.GL_PERIODS_V
                        WHERE PERIOD_SET_NAME = 'DBI Calendar' AND PERIOD_YEAR = {0} AND PERIOD_TYPE = 'Month' AND END_DATE < TO_DATE('{1}', 'DD-Mon-YYYY')      
   
                            UNION ALL 
   
                        SELECT * FROM (
                            SELECT PERIOD_NAME, START_DATE, END_DATE, PERIOD_TYPE, SUBSTR(ENTERED_PERIOD_NAME, 1, 3) ENTERED_PERIOD_NAME
                            FROM APPS.GL_PERIODS_V
                            WHERE PERIOD_SET_NAME = 'DBI Calendar' AND PERIOD_YEAR = {0} AND PERIOD_TYPE = 'Week' AND END_DATE <= TO_DATE('{1}', 'DD-Mon-YYYY')
                            ORDER BY END_DATE DESC
                        )
                        WHERE ROWNUM = 1  
      
                            UNION ALL   
     
                        SELECT PERIOD_NAME, START_DATE, END_DATE, PERIOD_TYPE, ENTERED_PERIOD_NAME
                        FROM APPS.GL_PERIODS_V
                        WHERE PERIOD_SET_NAME = 'DBI Calendar' AND PERIOD_YEAR = {0} AND PERIOD_TYPE = 'Month' AND START_DATE > TO_DATE('{1}', 'DD-Mon-YYYY') 
                        ),

                    JC_DATA AS (
                        SELECT JC_WK_DATE, PROJECT_ID, TASK_ID, MTD_RI, MTD_RNB, MTD_RNI, MTD_GREC, MTD_MU, MTD_GREV, MTD_DP, MTD_LB, MTD_TU, MTD_EU, MTD_SC, MTD_ESC, MTD_T, MTD_FE, MTD_PD, MTD_PB, MTD_RRPL, MTD_ME, MTD_TDE, MTD_TOP
                        FROM XXDBI_DW.JOB_COST
                        WHERE PROJECT_ID = {2} AND TASK_ID = {3} AND LEVEL_SORT = 9 AND (JC_WK_DATE >= TO_DATE('{4}', 'DD-Mon-YYYY') AND JC_WK_DATE <= TO_DATE('{1}', 'DD-Mon-YYYY'))
                    )", fiscalYear, weDate, projectID, taskID, fiscalYearStart);

            long[] lineNum = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360 };



            string[] jcLineName = { "NVL(MTD_GREC, 0)",                                                      // 20   Receipts
                                    "0",                                            // 40   Insurance Reimbursements
                                    "NVL(MTD_GREC, 0)",                                                      // 60   Gross Receipts
                                    "NVL(MTD_MU, 0)",                                                        // 80   Materials
                                    "NVL(MTD_GREV, 0)",                                                      // 100  Gross Revenue
                                    "NVL(MTD_DP, 0) + NVL(MTD_LB, 0)",                                        // 120  Direct Payroll & Labor Burden
                                    "NVL(MTD_TU, 0) + NVL(MTD_EU, 0)",                                        // 140  Truck & Equipment Usage
                                    "NVL(MTD_DP, 0) + NVL(MTD_LB, 0) + NVL(MTD_TU, 0) + NVL(MTD_EU, 0)",        // 160  Total
                                    "NVL(MTD_SC, 0)",                                                        // 180  Subcontractor
                                    "NVL(MTD_ESC, 0)",                                                       // 200  Est. Subcontractor
                                    "NVL(MTD_T, 0) + NVL(MTD_FE, 0)",                                         // 220  Travel (and Food & Entertainment)
                                    "NVL(MTD_PD, 0)",                                                        // 240  Per Diem
                                    "NVL(MTD_PB, 0)",                                                        // 260  Bond
                                    "NVL(MTD_RRPL, 0) + NVL(MTD_ME, 0)",                                      // 280  Repairs
                                    "NVL(MTD_TDE, 0)",                                                       // 300  Total Directs
                                    "NVL(MTD_TOP, 0)",                                                       // 320  Operating Profit
                                    "1000",                                            // 340  Overhead
                                    "NVL(MTD_TOP, 0) - 0" };                        // 360  Net Contribution
                         
            string sql2 ="";


            for (int i = 0; i <= 17; i++)
            {
                if (i != 0) { sql2 = sql2 + " UNION ALL "; }
                sql2 = sql2 + string.Format(@"
                    SELECT * FROM (       
                        SELECT ENTERED_PERIOD_NAME, (SELECT ACTUAL_NUM_ID FROM BUD_BID_ACTUAL_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) ACTUAL_NUM_ID, {0} PROJECT_ID, {1} DETAIL_TASK_ID, {2} LINE_ID, {3} AMOUNT, (SELECT CREATE_DATE FROM BUD_BID_ACTUAL_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2})  CREATE_DATE, (SELECT CREATED_BY FROM BUD_BID_ACTUAL_NUM WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND LINE_ID = {2}) CREATED_BY, TO_DATE('{4}', 'DD-Mon-YYYY')  MODIFY_DATE, '{5}' MODIFIED_BY
                        FROM JC_PERIODS
                        LEFT JOIN JC_DATA ON JC_PERIODS.END_DATE = JC_DATA.JC_WK_DATE                    
                    )
                    
                    PIVOT(
                    SUM(AMOUNT)
                    FOR (ENTERED_PERIOD_NAME)
                    IN ('NOV' NOV, 'DEC' DEC, 'JAN' JAN, 'FEB' FEB, 'MAR' MAR, 'APR' APR, 'MAY' MAY, 'JUN' JUN, 'JUL' JUL, 'AUG' AUG, 'SEP' SEP, 'OCT' OCT))", budBidProjectID, detailTaskID, lineNum[i], jcLineName[i], modifiedDate, modifiedBy);
            }

            string sql = sql1 + sql2;

            List<BUD_BID_ACTUAL_NUM> jcData;
            using (Entities context = new Entities())
            {
                jcData = context.Database.SqlQuery<BUD_BID_ACTUAL_NUM>(sql).ToList();
            }

            GenericData.Update<BUD_BID_ACTUAL_NUM>(jcData);           
        }

    }
}