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

                uxHidBudBidID.Text = "0";
                uxHidProjectNumID.Text = "0";
                uxHidDetailSheetID.Text = "0";
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
                    AddNewProject();
                    break;

                case "Edit Selected Project":
                    //EditSelectedProject();
                    break;

                case "Copy Selected Project":
                    //CopySelectedProject();
                    break;

                case "Delete Selected Project":
                    //DeleteSelectedProject();
                    break;

                case "Refresh Projects":
                    //RefreshProjects();
                    break;
            }

            uxProjectActions.Clear();
        }

        protected void deReadProjectGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = Convert.ToInt64(Request.QueryString["verID"]);

            uxProjectsStore.DataSource = BBMonthSummary.ProjectGrid.Data(orgID, yearID, verID);
        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string budBidprojectID = e.ExtraParams["BudBidProjectID"];
            uxHidBudBidID.Text = budBidprojectID;

            uxTasksStore.Reload();
            uxHidDetailSheetID.Text = "0";
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

        protected void AddNewProject()
        {
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            string orgName = HttpUtility.UrlEncode(Request.QueryString["orgName"]);
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            string budBidID = "0";
            string projectID = "0";

            string url = "/Views/Modules/BudgetBidding/umAddEditMonthProject.aspx?orgID=" + orgID + " &orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&budBidID=" + budBidID + "&projectNumID=" + projectID;
            
            Window win = new Window
            {
                ID = "uxAddEditProjectForm",
                Height = 210,
                Width = 600,
                Title = "Add New Project",
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
                    //AddNewTask();
                    break;

                case "Auto-Generate Tasks":
                    //AutoGenerateTasks
                    break;

                case "Edit Selected Task":
                    //EditSelectedTask();
                    break;

                case "Copy Selected Task":
                    //CopySelectedTask();
                    break;

                case "Delete Selected Task":
                    //DeleteSelectedTask();
                    break;

                case "Refresh Tasks":
                    //RefreshTasks();
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
            string detailSheetID = e.ExtraParams["DetailSheetID"];
            uxHidDetailSheetID.Text = detailSheetID;

            if (Convert.ToInt64(detailSheetID) == 0)
            {
                long budBidprojectID = Convert.ToInt64(uxHidBudBidID.Text);
                uxComments.Text = BBMonthSummary.Comments.Data(budBidprojectID);
            }
            else
            {
                uxComments.Text = BBDetail.Sheet.MainTabField.Comment(Convert.ToInt64(detailSheetID));
            }

            uxMonthDetailStore.Reload();
        }



        // Main Grid
        protected void deReadMainGridData(object sender, StoreReadDataEventArgs e)
        {
            string orgID = Request.QueryString["orgID"];
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailSheetID.Text;
            long weMonth = 1;  //FIX!

            uxMonthDetailStore.DataSource = BBMonthSummary.MainGrid.Data(yearID, verID, weMonth, orgID, budBidProjectID, detailSheetID);
        }











        protected void deEditSelectedRow(object sender, DirectEventArgs e)
        {
            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailSheetID.Text;


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