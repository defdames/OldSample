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
            uxHidDetailTaskID.Text = "0";
            uxHidDetailID.Text = "0";
            uxHidDetailName.Text = "";
 
            uxTasksStore.Reload();
            uxComments.Text = "";

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
            string detailSheetID = uxHidDetailTaskID.Text;
            string url = "/Views/Modules/BudgetBidding/umAddEditMonthProject.aspx?orgID=" + orgID + " &orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&budBidID=" + budBidID + "&projectID=" + projectID + "&projectNum=" + projectNumber + "&projectName=" + projectName + "&projectType=" + projectType + "&detailSheetID=" + detailSheetID + "&addNew=" + addNew;
            
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
            uxHidBudBidID.Text = "0";
            uxProjectsStore.Reload();
            uxTasksStore.Reload();
            uxMonthDetailStore.Reload();
        }

        protected void RefreshProjects()
        {
            uxHidBudBidID.Text = "0";
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

            //CalcSummaryTotals();
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
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            if (budBidID == 0)
            {
                uxTasksStore.RemoveAll();
            }
            else
            {
                uxTasksStore.DataSource = BBMonthSummary.TaskGrid.Data(budBidID);
            }          
        }

        protected void deSelectTask(object sender, DirectEventArgs e)
        {
            string detailTaskID = e.ExtraParams["DetailTaskID"];
            string detailID = e.ExtraParams["DetailID"];
            string detailName = e.ExtraParams["DetailName"];
            uxHidDetailTaskID.Text = detailTaskID;
            uxHidDetailID.Text = detailID;
            uxHidDetailName.Text = detailName;

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

                //if (BB.ProjectStillExists(budBidID) == false)
                //{
                //    StandardMsgBox("Edit", "Task has been deleted or has changed.  Please refresh tasks.", "INFO");
                //    return;
                //}

                windowTitle = "Edit Task";
            }
            else
            {
                windowTitle = "Add New Task";
            }

            string projectID = uxHidProjectID.Text;
            string projectNumber = uxHidProjectNum.Text;
            string projectType = uxHidType.Text;
            string projectName = HttpUtility.UrlEncode(uxHidProjectName.Text);
            string detailID = uxHidDetailID.Text;
            string detailName = uxHidDetailName.Text;

            string url = "/Views/Modules/BudgetBidding/umAddEditMonthTask.aspx?orgID=" + orgID + " &orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&budBidID=" + budBidID + "&projectID=" + projectID + "&projectNum=" + projectNumber + "&projectName=" + projectName + "&projectType=" + projectType + "&detailTaskID=" + detailTaskID + "&detailID=" + detailID + "&detailName=" + detailName + "&addNew=" + addNew;

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
            //if (uxHidBudBidID.Text == "0")
            //{
            //    StandardMsgBox("Delete", "A task must be selected before it can be deleted.", "INFO");
            //    return;
            //}

            //X.MessageBox.Confirm("Delete", "Are you sure you want to delete the selected task? Once it's been deleted it cannot be retrieved.", new MessageBoxButtonsConfig
            //{
            //    Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteSelectedTaskContiued()", Text = "Yes" },
            //    No = new MessageBoxButtonConfig { Text = "No" }
            //}).Show();
        }
        [DirectMethod]
        public void DeleteSelectedTaskContiued()
        {
            //long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            //if (BB.ProjectStillExists(budBidID) == false)
            //{
            //    StandardMsgBox("Delete", "Task has already been deleted or has changed.  Please refresh summary", "INFO");
            //    return;
            //}

            //BBProject.DBDelete(budBidID);
            //uxHidBudBidID.Text = "0";
            //uxProjectsStore.Reload();
            //uxTasksStore.Reload();
            //uxMonthDetailStore.Reload();
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

            //CalcSummaryTotals();
        }
        


        // Main Grid
        protected void deReadMainGridData(object sender, StoreReadDataEventArgs e)
        {
            string orgID = Request.QueryString["orgID"];
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailTaskID.Text;
            long weMonth = 1;  //FIX!

            uxMonthDetailStore.DataSource = BBMonthSummary.MainGrid.Data(yearID, verID, weMonth, orgID, budBidProjectID, detailSheetID);
        }











        protected void deEditSelectedRow(object sender, DirectEventArgs e)
        {
            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailTaskID.Text;


            if (detailSheetID == "0") { return; }

            string lineID = e.ExtraParams["LineID"];
            string lineDesc = e.ExtraParams["LineDesc"];
            //string hierID = Request.QueryString["hierID"];
            //string leOrgID = Request.QueryString["leOrgID"];
            //long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            //long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            //long verID = long.Parse(Request.QueryString["verID"]);
            //string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);

            string url = "/Views/Modules/BudgetBidding/umEditMonthRow.aspx?budBidProjectID=" + budBidProjectID + "&detailSheetID=" + detailSheetID + "&lineID=" + lineID;

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
            //uxReportsStore.DataSource = BB.YearSummaryReports();
        }

        protected void deChooseSummaryReport(object sender, DirectEventArgs e)
        {
            //string selectedReport = uxSummaryReports.Text;

            //long orgID = long.Parse(Request.QueryString["OrgID"]);
            //string orgName = HttpUtility.UrlEncode(Request.QueryString["orgName"]);
            //long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            //long verID = long.Parse(Request.QueryString["verID"]);
            //string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);
            //string prevYearID = uxHidPrevYear.Text;
            //string prevVerID = uxHidPrevVer.Text;
            //string projectNum = uxProjectNum.Text;
            //string projectName = HttpUtility.UrlEncode(uxProjectName.Text);
            //string url = "";

            //uxSummaryReports.Clear();
            //Int32 reportHeight = 0;
            //Int32 reportWidth = 0;

            //switch (selectedReport)
            //{
            //    case "Org Summary":
            //        url = "/Views/Modules/BudgetBidding/Reports/umRepOrgSum.aspx?orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID;
            //        reportHeight = 600;
            //        reportWidth = 1020;
            //        break;

            //    case "Comments & Variances":
            //        url = "/Views/Modules/BudgetBidding/Reports/umRepOrgComm.aspx?orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID;
            //        reportHeight = 600;
            //        reportWidth = 1020;
            //        break;

            //    case "Liabilities":
            //        url = "/Views/Modules/BudgetBidding/Reports/umRepOrgLiab.aspx?orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID;
            //        reportHeight = 600;
            //        reportWidth = 1020;
            //        break;

            //    case "Selected Project":
            //        if (uxHidBudBidID.Text == "")
            //        {
            //            StandardMsgBox("Report", "A project must be selected before a report can be generated.", "INFO");
            //            return;
            //        }

            //        long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            //        if (BB.ProjectStillExists(budBidID) == false)
            //        {
            //            StandardMsgBox("Report", "Project has been deleted or has changed.  Please refresh summary", "INFO");
            //            return;
            //        }

            //        url = "/Views/Modules/BudgetBidding/Reports/umRepOrgProject.aspx?orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&budBidprojectID=" + budBidID + "&projectNum=" + projectNum + "&projectName=" + projectName;
            //        reportHeight = 600;
            //        reportWidth = 850;
            //        break;

            //    case "All Projects - Including Detail Sheets":
            //        break;
            //}

            //Window win = new Window
            //{
            //    ID = "uxReport",
            //    Title = "Report",
            //    Height = reportHeight,
            //    Width = reportWidth,
            //    Modal = true,
            //    Resizable = true,
            //    CloseAction = CloseAction.Destroy,
            //    Loader = new ComponentLoader
            //    {
            //        Mode = LoadMode.Frame,
            //        DisableCaching = true,
            //        Url = url,
            //        AutoLoad = true,
            //        LoadMask =
            //        {
            //            ShowMask = true
            //        }
            //    }
            //};
            //win.Render(this.Form);
            //win.Show();
        }

        protected void deUpdateAllActuals(object sender, DirectEventArgs e)
        {

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
    }
}