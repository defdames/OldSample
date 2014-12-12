﻿using System;
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
                uxHidDetailSheetID.Text = "0";
            }
        }

        protected void deLoadSummaryActions(object sender, StoreReadDataEventArgs e)
        {
            //long orgID = long.Parse(Request.QueryString["OrgID"]);
            //long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            //long verID = long.Parse(Request.QueryString["verID"]);

            //uxActionsStore.DataSource = BB.YearSummaryProjectActions(orgID, yearID, verID);
        }

        protected void deChooseSummaryAction(object sender, DirectEventArgs e)
        {
            //string selectedAction = uxActions.Text;

            //switch (selectedAction)
            //{
            //    case "Add a New Project":
            //        AddNewProject();
            //        break;

            //    case "View Selected Project":
            //        EditSelectedProject();
            //        break;

            //    case "Edit Selected Project":
            //        EditSelectedProject();
            //        break;

            //    case "Copy Selected Project":
            //        CopySelectedProject();
            //        break;

            //    case "Delete Selected Project":
            //        DeleteSelectedProject();
            //        break;

            //    case "Refresh Data":
            //        RefreshData();
            //        break;
            //}

            //uxActions.Clear();
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
            //string hierID = Request.QueryString["hierID"];
            //string leOrgID = Request.QueryString["leOrgID"];
            //long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            //long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            //long verID = long.Parse(Request.QueryString["verID"]);
            //string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);

            //if (BB.CountAllProjects(orgID, yearID, verID) == 0)
            //{
            //    StandardMsgBox("Update All Projects", "There are no projects to update.", "INFO");
            //    return;
            //}

            //string url = "/Views/Modules/BudgetBidding/umUpdateAllActuals.aspx?hierID=" + hierID + "&leOrgID=" + leOrgID + "&orgID=" + orgID + "&yearID=" + yearID + "&verName=" + verName;

            //Window win = new Window
            //{
            //    ID = "uxUpdateAllActualsForm",
            //    Height = 210,
            //    Width = 400,
            //    Title = "Update All Actuals",
            //    Modal = true,
            //    Resizable = false,
            //    CloseAction = CloseAction.Destroy,
            //    Closable = false,
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

        protected void deReadProjectGridData(object sender, StoreReadDataEventArgs e)
        {
            //if (uxHidBudBidID.Text == "") { return; }

            //long orgID = Convert.ToInt64(1210);
            long orgID = Convert.ToInt64(425);

            uxSummaryProjectStore.DataSource = BBMonthSummary.ProjectGrid.Data(orgID, 2014, 4);
        }

        protected void deReadTaskDetailGridData(object sender, StoreReadDataEventArgs e)
        {
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);
            uxSummaryTaskDetailStore.DataSource = BBMonthSummary.TaskDetailGrid.Data(budBidID);
        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string budBidprojectID = e.ExtraParams["BudBidProjectID"];
            uxHidBudBidID.Text = budBidprojectID;
            uxSummaryTaskDetailStore.Reload();
            uxComments.Text = "";
        }

        protected void deSelectTaskDetail(object sender, DirectEventArgs e)
        {
            string detailSheetID = e.ExtraParams["DetailSheetID"];
            uxHidDetailSheetID.Text = detailSheetID;

            if (Convert.ToInt64(detailSheetID) == 9999)
            {
                long budBidprojectID = Convert.ToInt64(uxHidBudBidID.Text);
                uxComments.Text = BBMonthSummary.Project.Comments(budBidprojectID); 
            }
            else
            {
                uxComments.Text = BBDetail.Sheet.MainTabField.Comment(Convert.ToInt64(detailSheetID)); 
            }
            uxMonthDetailStore.Reload();
        }

        protected void deReadMonthDetailGridData(object sender, StoreReadDataEventArgs e)
        {
            //long detailSheetID = 48497;
            long detailSheetID = Convert.ToInt64(uxHidDetailSheetID.Text);
            uxMonthDetailStore.DataSource = BBMonthSummary.TaskDetail.Data(detailSheetID, 1);
        }
    }
}