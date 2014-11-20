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
    public partial class umYearRollupSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!BasePage.validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                long orgID = long.Parse(Request.QueryString["OrgID"]);
                long yearID = long.Parse(Request.QueryString["fiscalYear"]);
                long verID = long.Parse(Request.QueryString["verID"]);

                if (BBAdjustments.Count(orgID, yearID, verID) == 0)
                {
                    BBAdjustments.Create(orgID, yearID, verID);
                }

                CalcSummaryTotals();
            }
        }

        // Load Summary                                     
        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            long hierID = long.Parse(Request.QueryString["hierID"]);
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = BB.CalcPrevYear(yearID, verID);
            long prevVerID = BB.CalcPrevVer(yearID, verID);
            long userID = SYS_USER_INFORMATION.UserID(User.Identity.Name);
            uxSummaryGridStore.DataSource = BBSummaryRollup.Grid.Data(userID, hierID, orgID, yearID, verID, prevYearID, prevVerID);
            CalcSummaryTotals();
        }

        protected void deReadOverheadGridData(object sender, StoreReadDataEventArgs e)        
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxOverheadGridStore.DataSource = BBOH.Data(orgID, yearID, verID);
        }

        protected void deReadAdjustmentGridData(object sender, StoreReadDataEventArgs e)
        {
            //long orgID = long.Parse(Request.QueryString["OrgID"]);
            //long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            //long verID = long.Parse(Request.QueryString["verID"]);

            //uxAdjustmentGridStore.DataSource = BBAdjustments.Data(orgID, yearID, verID);
        }

        [DirectMethod(Namespace = "SaveRecord")]
        public void deSaveAdjustments(long id, string field, string newValue)
        {
            //decimal amount;
            //amount = ForceToDecimal(newValue, -9999999999.99M, 9999999999.99M);

            //BUD_BID_ADJUSTMENT data;

            //using (Entities context = new Entities())
            //{
            //    data = context.BUD_BID_ADJUSTMENT.Where(x => x.ADJ_ID == id).Single();
            //}

            //if (field == "WEATHER_ADJ")
            //{
            //    data.WEATHER_ADJ = amount;
            //}
            //else if (field == "MAT_ADJ")
            //{
            //    data.MAT_ADJ = amount;
            //}


            //GenericData.Update<BUD_BID_ADJUSTMENT>(data);
            //uxAdjustmentGridStore.CommitChanges();
            //CalcSummaryTotals();
            //StandardMsgBox("Adjustment", "The adjustment has been updated.", "INFO");
        }

        protected void CalcSummaryTotals()
        {
            long hierID = long.Parse(Request.QueryString["hierID"]);
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = BB.CalcPrevYear(yearID, verID);
            long prevVerID = BB.CalcPrevVer(yearID, verID);
            long userID = SYS_USER_INFORMATION.UserID(User.Identity.Name);

            BBSummaryRollup.Subtotals.Fields data = BBSummaryRollup.Subtotals.Data(userID, hierID, orgID, yearID, verID, prevYearID, prevVerID);
            decimal tGrossRec = data.GROSS_REC;
            decimal tMatUsage = data.MAT_USAGE;
            decimal tGrossRev = data.GROSS_REV;
            decimal tDirects = data.DIR_EXP;
            decimal tOP = data.OP;
            decimal tOPPerc = tGrossRev == 0 ? 0 : tOP / tGrossRev;
            decimal tOH = data.OH;
            decimal tNetCont = data.NET_CONT;
            decimal tOPPlusMinus = data.OP_VAR;
            decimal tNetContPlusMinus = data.NET_CONT_VAR;

            uxTGrossRec.Text = String.Format("{0:N2}", tGrossRec);
            uxTMatUsage.Text = String.Format("{0:N2}", tMatUsage);
            uxTGrossRev.Text = String.Format("{0:N2}", tGrossRev);
            uxTDirects.Text = String.Format("{0:N2}", tDirects);
            uxTOP.Text = String.Format("{0:N2}", tOP);
            uxTOPPerc.Text = String.Format("{0:#,##0.00%}", tOPPerc);
            uxTOH.Text = String.Format("{0:N2}", tOH);
            uxTNetCont.Text = String.Format("{0:N2}", tNetCont);
            uxTOPPlusMinus.Text = String.Format("{0:N2}", tOPPlusMinus);
            uxTNetContPlusMinus.Text = String.Format("{0:N2}", tNetContPlusMinus);








            // Adjustments
            //BBAdjustments.Subtotal.Fields adjustmentData = BBAdjustments.Subtotal.Data(orgID, yearID, verID);
            //decimal adjMatUsage = adjustmentData.MAT_ADJ ?? 0;
            //decimal adjDirects = adjustmentData.WEATHER_ADJ ?? 0;

            // Previous adjustments for grand total +/-
            //BBAdjustments.Subtotal.Fields prevAdjustmentData = BBAdjustments.Subtotal.Data(orgID, prevYearID, prevVerID);
            //decimal prevAdjMatUsage = prevAdjustmentData.MAT_ADJ ?? 0;
            //decimal prevAdjDirects = prevAdjustmentData.WEATHER_ADJ ?? 0;
            //decimal prevOPIncludingAdj = (tOP - tOPPlusMinus) - (prevAdjMatUsage + prevAdjDirects);

            // Grand total
            //decimal gtGrossRec = tGrossRec;
            //decimal gtMatUsage = tMatUsage + adjMatUsage;
            //decimal gtGrossRev = gtGrossRec - gtMatUsage;
            //decimal gtDirects = tDirects + adjDirects;
            //decimal gtOP = gtGrossRev - gtDirects;
            //decimal gtOPPerc = gtGrossRev == 0 ? 0 : gtOP / gtGrossRev;
            //decimal gtOPPlusMinus = gtOP - prevOPIncludingAdj;
            //uxGTGrossRec.Text = String.Format("{0:N2}", gtGrossRec);
            //uxGTMatUsage.Text = String.Format("{0:N2}", gtMatUsage);
            //uxGTGrossRev.Text = String.Format("{0:N2}", gtGrossRev);
            //uxGTDirects.Text = String.Format("{0:N2}", gtDirects);
            //uxGTOP.Text = String.Format("{0:N2}", gtOP);
            //uxGTOPPerc.Text = String.Format("{0:#,##0.00%}", gtOPPerc);
            //uxGTOPPlusMinus.Text = String.Format("{0:N2}", gtOPPlusMinus);

            // Net contribution
            BBOH.Subtotal.Fields ohData = BBOH.Subtotal.Data(orgID, yearID, verID);
            decimal oh = ohData.OH;
            decimal netCont = tNetCont - oh;
            uxNetCont.Text = String.Format("{0:N2}", netCont);
        }

        // Reports
        protected void deLoadRollupReports(object sender, StoreReadDataEventArgs e)
        {
            uxRollupReportsStore.DataSource = BBReports.RollupSummaryReports();
        }
        protected void deChooseRollupReport(object sender, DirectEventArgs e)
        {
            string selectedReport = uxRollupReports.Text;

            long hierID = long.Parse(Request.QueryString["hierID"]);
            string leOrgID = Request.QueryString["leOrgID"];
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = HttpUtility.UrlEncode(Request.QueryString["orgName"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = BB.CalcPrevYear(yearID, verID);
            long prevVerID = BB.CalcPrevVer(yearID, verID);
            long userID = SYS_USER_INFORMATION.UserID(User.Identity.Name);
            string url = "";

            uxRollupReports.Clear();
            string windowTitle = "";
            Int32 reportHeight = 0;
            Int32 reportWidth = 0;

            switch (selectedReport)
            {
                case "Summary":
                    url = "/Views/Modules/BudgetBidding/Reports/umRepRollupSum.aspx?hierID=" + hierID + "&orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID + "&userID=" + userID;
                    windowTitle = "Report";
                    reportHeight = 600;
                    reportWidth = 1020;
                    break;

                case "Comments & Variances":
                    url = "/Views/Modules/BudgetBidding/Reports/umRepOrgComments.aspx?hierID=" + hierID + "&orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID + "&userID=" + userID;
                    windowTitle = "Report";
                    reportHeight = 600;
                    reportWidth = 1020;
                    break;

                case "Liabilities":
                    url = "/Views/Modules/BudgetBidding/Reports/umRepOrgLiabilities.aspx?hierID=" + hierID + "&orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID + "&userID=" + userID;
                    windowTitle = "Report";
                    reportHeight = 600;
                    reportWidth = 1020;
                    break;

                case "All Projects":
                    url = "/Views/Modules/BudgetBidding/Reports/umRepOrgAllProjects.aspx?hierID=" + hierID + "&orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID + "&userID=" + userID;
                    windowTitle = "Report";
                    reportHeight = 600;
                    reportWidth = 1020;
                    break;

                case "Summary - Budget Year/Version Comparison":
                    url = "/Views/Modules/BudgetBidding/umReportCompareTo.aspx?report=OP_COMPARE";
                    windowTitle = "Compare To:";
                    reportHeight = 210;
                    reportWidth = 400;
                    break;

                case "Overhead Comparison":
                    url = "/Views/Modules/BudgetBidding/umReportCompareTo.aspx?report=OH_COMPARE";
                    windowTitle = "Compare To:";
                    reportHeight = 210;
                    reportWidth = 400;
                    break;
            }

            Window win = new Window
            {
                ID = "uxReport",
                Title = windowTitle,
                Height = reportHeight,
                Width = reportWidth,
                Modal = true,
                Resizable = true,
                CloseAction = CloseAction.Destroy,
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
        public void LoadOHCompareReport(string reportName, string prevYear, string preVer)
        {
            long hierID = long.Parse(Request.QueryString["hierID"]);
            string leOrgID = Request.QueryString["leOrgID"];
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = HttpUtility.UrlEncode(Request.QueryString["orgName"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = long.Parse(prevYear);
            long prevVerID = long.Parse(preVer);
            string prevVerName = HttpUtility.UrlEncode(BB.GetPrevVerName(prevVerID));
            long userID = SYS_USER_INFORMATION.UserID(User.Identity.Name);

            string url = "";

            switch (reportName)
            {
                case "OP_COMPARE":
                    url = "/Views/Modules/BudgetBidding/Reports/umRepRollupOPCompare.aspx?hierID=" + hierID + "&orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID + "&prevVerName=" + prevVerName + "&userID=" + userID;
                    break;

                case "OH_COMPARE":
                    url = "/Views/Modules/BudgetBidding/Reports/umRepRollupOHCompare.aspx?hierID=" + hierID + "&orgID=" + orgID + "&orgName=" + orgName + "&yearID=" + yearID + "&verID=" + verID + "&verName=" + verName + "&prevYearID=" + prevYearID + "&prevVerID=" + prevVerID + "&prevVerName=" + prevVerName + "&userID=" + userID;
                    break;
            }

            Window win = new Window
            {
                ID = "uxReport",
                Title = "Report",
                Height = 600,
                Width = 1020,
                Modal = true,
                Resizable = true,
                CloseAction = CloseAction.Destroy,
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