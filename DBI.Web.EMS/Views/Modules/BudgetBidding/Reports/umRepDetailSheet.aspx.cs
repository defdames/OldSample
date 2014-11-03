using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class umRepDetailSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                long leOrgID = long.Parse(Request.QueryString["leOrgID"]);
                long orgID = long.Parse(Request.QueryString["orgID"]);
                long yearID = long.Parse(Request.QueryString["yearID"]);
                long verID = long.Parse(Request.QueryString["verID"]);
                string verName = Request.QueryString["verName"];
                string weDate = Request.QueryString["weDate"];
                long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
                long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
                string projectName = Request.QueryString["projectName"];
                long sheetNum = long.Parse(Request.QueryString["sheetNum"]);
                string detailSheetName = Request.QueryString["detailSheetName"];
                long sheetCount = BBDetail.Sheets.MaxOrder(budBidProjectID);
                string sheetComments = BBDetail.Sheet.MainTabField.Comment(detailSheetID);

                ReportParameter paramYear = new ReportParameter("paramYear", yearID.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramYear });
                ReportParameter paramVer = new ReportParameter("paramVer", verName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramVer });
                ReportParameter paramWEData = new ReportParameter("paramWEData", weDate);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramWEData });
                ReportParameter paramProjName = new ReportParameter("paramProjName", projectName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramProjName });
                ReportParameter paramSheetNum = new ReportParameter("paramSheetNum", sheetNum.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSheetNum });
                ReportParameter paramSheetCount = new ReportParameter("paramSheetCount", sheetCount.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSheetCount });
                detailSheetName = (detailSheetName.Length >= 10 && detailSheetName.Substring(0, 10) == "SYS_DETAIL") ? "" : detailSheetName;
                ReportParameter paramSheetName = new ReportParameter("paramSheetName", detailSheetName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSheetName });
                ReportParameter paramSheetComments = new ReportParameter("paramSheetComments", sheetComments);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSheetComments });

                decimal sGrossRec;
                decimal sMatUsage;
                decimal sGrossRev;
                decimal sDirects;
                decimal sOP;

                if (sheetNum == 1)
                {
                    BBProject.StartNumbers.Fields dataStart = BBProject.StartNumbers.Data(budBidProjectID);
                    sGrossRec = dataStart.GROSS_REC;
                    sMatUsage = dataStart.MAT_USAGE;
                    sGrossRev = dataStart.GROSS_REV;
                    sDirects = dataStart.DIR_EXP;
                    sOP = dataStart.OP;
                }
                else
                {
                    long prevSheetID = BBDetail.Sheet.ID(budBidProjectID, sheetNum - 1);
                    BBDetail.Sheet.EndNumbers.Fields sheetStartNums = BBDetail.Sheet.EndNumbers.Get(prevSheetID);
                    sGrossRec = sheetStartNums.GROSS_REC;
                    sMatUsage = sheetStartNums.MAT_USAGE;
                    sGrossRev = sheetStartNums.GROSS_REV;
                    sDirects = sheetStartNums.DIR_EXP;
                    sOP = sheetStartNums.OP;
                }

                ReportParameter paramSGrossRec = new ReportParameter("paramSGrossRec", sGrossRec.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSGrossRec });
                ReportParameter paramSMatUsage = new ReportParameter("paramSMatUsage", sMatUsage.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSMatUsage });
                ReportParameter paramSGrossRev = new ReportParameter("paramSGrossRev", sGrossRev.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSGrossRev });
                ReportParameter paramSDirects = new ReportParameter("paramSDirects", sDirects.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSDirects });
                ReportParameter paramSOP = new ReportParameter("paramSOP", sOP.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramSOP });

                BBDetail.Sheet.MainTabField.Fields sheetMainNums = BBDetail.Sheet.MainTabField.NumsData(detailSheetID);
                decimal recRemain = sheetMainNums.RECREMAIN ?? 0;
                decimal daysRemain = sheetMainNums.DAYSREMAIN ?? 0;
                decimal unitsRemain = sheetMainNums.UNITREMAIN ?? 0;
                decimal daysWorked = sheetMainNums.DAYSWORKED ?? 0;
                ReportParameter paramRecRemain = new ReportParameter("paramRecRemain", recRemain.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramRecRemain });
                ReportParameter paramDaysRemain = new ReportParameter("paramDaysRemain", daysRemain.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramDaysRemain });
                ReportParameter paramUnitsRemain = new ReportParameter("paramUnitsRemain", unitsRemain.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramUnitsRemain });
                ReportParameter paramDaysWorked = new ReportParameter("paramDaysWorked", daysWorked.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramDaysWorked });
                decimal laborBurden = BB.LaborBurdenRate(leOrgID, yearID) * 100;
                ReportParameter paramLaborBurden = new ReportParameter("paramLaborBurden", laborBurden.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramLaborBurden });

                CalulateDetailSheet();
            }
        }

        protected void CalulateDetailSheet()
        {
            long leOrgID = long.Parse(Request.QueryString["leOrgID"]);
            long orgID = long.Parse(Request.QueryString["orgID"]);
            long yearID = long.Parse(Request.QueryString["yearID"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
            long sheetNum = long.Parse(Request.QueryString["sheetNum"]);

            // Get Start Numbers
            decimal sGrossRec;
            decimal sMatUsage;
            decimal sGrossRev;
            decimal sDirects;
            decimal sOP;

            if (sheetNum == 1)
            {
                BBProject.StartNumbers.Fields dataStart = BBProject.StartNumbers.Data(budBidProjectID);
                sGrossRec = dataStart.GROSS_REC;
                sMatUsage = dataStart.MAT_USAGE;
                sGrossRev = dataStart.GROSS_REV;
                sDirects = dataStart.DIR_EXP;
                sOP = dataStart.OP;
            }
            else
            {
                long prevSheetID = BBDetail.Sheet.ID(budBidProjectID, sheetNum - 1);
                BBDetail.Sheet.EndNumbers.Fields sheetStartNums = BBDetail.Sheet.EndNumbers.Get(prevSheetID);
                sGrossRec = sheetStartNums.GROSS_REC;
                sMatUsage = sheetStartNums.MAT_USAGE;
                sGrossRev = sheetStartNums.GROSS_REV;
                sDirects = sheetStartNums.DIR_EXP;
                sOP = sheetStartNums.OP;
            }

            // Get Main Tab Numbers
            BBDetail.Sheet.MainTabField.Fields sheetMainNums = BBDetail.Sheet.MainTabField.NumsData(detailSheetID);
            decimal recRemaining = sheetMainNums.RECREMAIN ?? 0;
            decimal totalDaysRemain = sheetMainNums.DAYSREMAIN ?? 0;
            decimal totalUnitsRemain = sheetMainNums.UNITREMAIN ?? 0;
            decimal totalDaysWorked = sheetMainNums.DAYSWORKED ?? 0;

            // Update Sheet's Bottom Numbers
            BBDetail.Sheet.BottomNumbers.Fields bottomData = BBDetail.Sheet.BottomNumbers.Calculate(leOrgID, yearID, detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP, totalDaysRemain, totalUnitsRemain, totalDaysWorked);
            ReportParameter paramLABOR_BURDEN = new ReportParameter("paramLABOR_BURDEN", bottomData.LABOR_BURDEN.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramLABOR_BURDEN });
            ReportParameter paramAVG_UNITS_PER_DAY = new ReportParameter("paramAVG_UNITS_PER_DAY", bottomData.AVG_UNITS_PER_DAY.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramAVG_UNITS_PER_DAY });
            ReportParameter paramTOTAL_WKLY_DIRECTS = new ReportParameter("paramTOTAL_WKLY_DIRECTS", bottomData.TOTAL_WKLY_DIRECTS.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramTOTAL_WKLY_DIRECTS });
            ReportParameter paramTOTAL_DIRECTS_LEFT = new ReportParameter("paramTOTAL_DIRECTS_LEFT", bottomData.TOTAL_DIRECTS_LEFT.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramTOTAL_DIRECTS_LEFT });
            ReportParameter paramTOTAL_DIRECTS_PER_DAY = new ReportParameter("paramTOTAL_DIRECTS_PER_DAY", bottomData.TOTAL_DIRECTS_PER_DAY.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramTOTAL_DIRECTS_PER_DAY });
            ReportParameter paramTOTAL_MATERIAL_LEFT = new ReportParameter("paramTOTAL_MATERIAL_LEFT", bottomData.TOTAL_MATERIAL_LEFT.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramTOTAL_MATERIAL_LEFT });

            // Update End Numbers
            BBDetail.Sheet.EndNumbers.Fields endNums = BBDetail.Sheet.EndNumbers.Calculate(leOrgID, yearID, detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP, recRemaining, totalDaysRemain, totalUnitsRemain, totalDaysWorked);
            decimal eGrossRec = endNums.GROSS_REC;
            decimal eMatUsage = endNums.MAT_USAGE;
            decimal eGrossRev = endNums.GROSS_REV;
            decimal eDirects = endNums.DIR_EXP;
            decimal eOP = endNums.OP;
            ReportParameter paramEGrossRec = new ReportParameter("paramEGrossRec", eGrossRec.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramEGrossRec });
            ReportParameter paramEMatUsage = new ReportParameter("paramEMatUsage", eMatUsage.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramEMatUsage });
            ReportParameter paramEGrossRev = new ReportParameter("paramEGrossRev", eGrossRev.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramEGrossRev });
            ReportParameter paramEDirects = new ReportParameter("paramEDirects", eDirects.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramEDirects });
            ReportParameter paramEOP = new ReportParameter("paramEOP", eOP.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramEOP });
        }
    }
}