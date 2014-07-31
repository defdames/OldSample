using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using Newtonsoft.Json;
using DBI.Core.Security;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umDetailSheet : System.Web.UI.Page
    {
        // Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxCloseDetailSheet.Disable();

                string yearID = Request.QueryString["yearID"];
                string verName = Request.QueryString["verName"];
                string weDate = Request.QueryString["weDate"];
                long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
                long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
                string projectName = RSAClass.Decrypt(Request.QueryString["projectName"]);
                long sheetNum = long.Parse(Request.QueryString["sheetNum"]);
                string detailSheetName = RSAClass.Decrypt(Request.QueryString["detailSheetName"]);
                decimal sGrossRec = Convert.ToDecimal(Request.QueryString["sGrossRec"]);
                decimal sMatUsage = Convert.ToDecimal(Request.QueryString["sMatUsage"]);
                decimal sGrossRev = Convert.ToDecimal(Request.QueryString["sGrossRev"]);
                decimal sDirects = Convert.ToDecimal(Request.QueryString["sDirects"]);
                decimal sOP = Convert.ToDecimal(Request.QueryString["sOP"]);
                long sheetCount = BBDetail.Sheets.MaxOrder(budBidProjectID);

                uxYearVersion.Text = yearID + " " + verName;
                uxWeekEnding.Text = "Week Ending: " + weDate;
                uxProjectName.Text = projectName;
                uxDetailNameLabel.Text = "Detail Sheet (" + sheetNum + " of " + sheetCount + "): ";
                detailSheetName = (detailSheetName.Length >= 10 && detailSheetName.Substring(0, 10) == "SYS_DETAIL") ? "" : detailSheetName;
                uxDetailName.Text = detailSheetName;
                
                if (sheetNum > 1)
                {
                    long prevSheetID = BBDetail.Sheet.ID(budBidProjectID, sheetNum - 1);
                    BBDetail.Sheet.EndNumbers.Fields sheetStartNums = BBDetail.Sheet.EndNumbers.Get(prevSheetID);
                    sGrossRec = sheetStartNums.GROSS_REC;
                    sMatUsage = sheetStartNums.MAT_USAGE;
                    sGrossRev = sheetStartNums.GROSS_REV;
                    sDirects = sheetStartNums.DIR_EXP;
                    sOP = sheetStartNums.OP;
                }
                uxSGrossRec.Text = String.Format("{0:N2}", sGrossRec);
                uxSMatUsage.Text = String.Format("{0:N2}", sMatUsage);
                uxSGrossRev.Text = String.Format("{0:N2}", sGrossRev);
                uxSDirects.Text = String.Format("{0:N2}", sDirects);
                uxSOP.Text = String.Format("{0:N2}", sOP);

                BBDetail.Sheet.MainTabField.Fields sheetMainNums = BBDetail.Sheet.MainTabField.Data(detailSheetID);
                decimal recRemain = sheetMainNums.RECREMAIN ?? 0;
                decimal daysRemain = sheetMainNums.DAYSREMAIN ?? 0;
                decimal unitsRemain = sheetMainNums.UNITREMAIN ?? 0;
                decimal daysWorked = sheetMainNums.DAYSWORKED ?? 0;
                uxRecRemaining.Text = String.Format("{0:N2}", recRemain);
                uxDaysRemaining.Text = String.Format("{0:N2}", daysRemain);
                uxUnitsRemaining.Text = String.Format("{0:N2}", unitsRemain);
                uxDaysWorked.Text = String.Format("{0:N2}", daysWorked);

                CalulateDetailSheet(); 
                CheckAllowDetailSave();
            }
        }

        public void deSaveMainTabField(object sender, DirectEventArgs e)
        {
            long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
            string recType = e.ExtraParams["RecType"];
            decimal fieldText = Convert.ToDecimal(e.ExtraParams["FieldText"]);
            BBDetail.Sheet.MainTabField.DBUpdate(budBidProjectID, detailSheetID, recType, fieldText);
            deFormatNumber(sender, e);
            CalulateDetailSheet();
        }

 
        
        // Sub grid
        protected void deReadGridData(object sender, StoreReadDataEventArgs e)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
            string recType = e.Parameters["RecordType"];

            switch (recType)
            {
                case "MATERIAL":
                    uxMaterialGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "EQUIPMENT":
                    uxEquipmentGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "PERSONNEL":
                    uxPersonnelGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "PERDIEM":
                    uxPerDiemGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "TRAVEL":
                    uxTravelGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "MOTELS":
                    uxMotelsGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "MISC":
                    uxMiscGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                case "LUMPSUM":
                    uxLumpSumGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID, recType);
                    break;

                default:
                    break;
            }                
        }
        [DirectMethod(Namespace = "SaveRecord")]
        public void deSaveSubGridData(object recordData, long id)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailTaskID = long.Parse(Request.QueryString["detailSheetID"]);
            string jsonText = recordData.ToString();
            BB.BBSubGridV gridData = JsonConvert.DeserializeObject<BB.BBSubGridV>(jsonText);
            string recType = ConvertSubGridNumToName(gridData.REC_TYPE);
            string desc1 = gridData.DESC_1;
            string desc2 = gridData.DESC_2;
            decimal amt_1 = ConvertToDecimal(gridData.AMT_1);
            decimal amt_2 = ConvertToDecimal(gridData.AMT_2);
            decimal amt_3 = ConvertToDecimal(gridData.AMT_3);
            decimal amt_4 = ConvertToDecimal(gridData.AMT_4);
            decimal amt_5 = ConvertToDecimal(gridData.AMT_5);


            BUD_BID_DETAIL_SHEET data;
            if (id == 0)
            {
                data = new BUD_BID_DETAIL_SHEET();
            }
            else
            {
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == id).Single();
                }
            }

            data.DESC_1 = desc1;
            data.DESC_2 = desc2;
            data.AMT_1 = amt_1;
            data.AMT_2 = amt_2;
            data.AMT_3 = amt_3;
            data.AMT_4 = amt_4;
            data.AMT_5 = amt_5;
            data.TOTAL = amt_1 * amt_2;

            if (id == 0)
            {
                data.PROJECT_ID = projectID;
                data.DETAIL_TASK_ID = detailTaskID;
                data.REC_TYPE = recType;
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now; ;
                data.MODIFIED_BY = "TEMP";
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(data);
            }
            else
            {
                data.MODIFY_DATE = DateTime.Now; ;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }

            CalulateDetailSheet();

            switch (recType)
            {
                case "MATERIAL":
                    uxMaterialGridStore.CommitChanges();
                    uxMaterialGridStore.Reload();
                    break;

                case "EQUIPMENT":
                    uxEquipmentGridStore.CommitChanges();
                    uxEquipmentGridStore.Reload();
                    break;

                case "PERSONNEL":
                    uxPersonnelGridStore.CommitChanges();
                    uxPersonnelGridStore.Reload();
                    break;

                case "PERDIEM":
                    uxPerDiemGridStore.CommitChanges();
                    uxPerDiemGridStore.Reload();
                    break;

                case "TRAVEL":
                    uxTravelGridStore.CommitChanges();
                    uxTravelGridStore.Reload();
                    break;

                case "MOTELS":
                    uxMotelsGridStore.CommitChanges();
                    uxMotelsGridStore.Reload();
                    break;

                case "MISC":
                    uxMiscGridStore.CommitChanges();
                    uxMiscGridStore.Reload();
                    break;

                case "LUMPSUM":
                    uxLumpSumGridStore.CommitChanges();
                    uxLumpSumGridStore.Reload();
                    break;

                default:
                    break;
            }           
        }
        [DirectMethod(Namespace = "DeleteRecord")]
        public void deDeleteRecord(object sender, DirectEventArgs e)
        {
            long id = Convert.ToInt64(e.ExtraParams["RecordID"]);
            if (id == 0) { return; }

            BBDetail.SubGrid.DeleteRecord(id);
            uxMaterialGridStore.Reload();   // FIX THSE WITH SWITCH!
            uxEquipmentGridStore.Reload();
            uxPersonnelGridStore.Reload();
            uxPerDiemGridStore.Reload();
            uxTravelGridStore.Reload();
            uxMotelsGridStore.Reload();
            uxMiscGridStore.Reload();
            uxLumpSumGridStore.Reload();
        }

        

        // Calculate
        protected void deCalculate(object sender, DirectEventArgs e)
        {
            CalulateDetailSheet();
        }
        protected void CalulateDetailSheet()
        {
            long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);

            // Get Start Numbers
            decimal sGrossRec = Convert.ToDecimal(uxSGrossRec.Text);
            decimal sMatUsage = Convert.ToDecimal(uxSMatUsage.Text);
            decimal sGrossRev = Convert.ToDecimal(uxSGrossRev.Text);
            decimal sDirects = Convert.ToDecimal(uxSDirects.Text);
            decimal sOP = Convert.ToDecimal(uxSOP.Text);

            // Get Main Tab Numbers
            decimal recRemaining = Convert.ToDecimal(uxRecRemaining.Text);
            decimal totalDaysRemain = Convert.ToDecimal(uxDaysRemaining.Text);
            decimal totalUnitsRemain = Convert.ToDecimal(uxUnitsRemaining.Text);
            decimal totalDaysWorked = Convert.ToDecimal(uxDaysWorked.Text);

            // Update Sheet's Sub Grid Totals
            BBDetail.Sheet.Subtotals.Fields subtotal = BBDetail.Sheet.Subtotals.Get(detailSheetID);
            decimal totalMaterial = subtotal.MATERIAL ?? 0;
            decimal totalEquipment = subtotal.EQUIPMENT ?? 0;
            decimal totalPersonnel = subtotal.PERSONNEL ?? 0;
            decimal totalPerDiem = subtotal.PERDIEM ?? 0;
            decimal totalTravel = subtotal.TRAVEL ?? 0;
            decimal totalMotels = subtotal.MOTELS ?? 0;
            decimal totalMisc = subtotal.MISC ?? 0;
            decimal totalLumpSum = subtotal.LUMPSUM ?? 0;
            uxTotalMaterial.Text = String.Format("{0:N2}", totalMaterial);
            uxTotalEquipment.Text = String.Format("{0:N2}", totalEquipment);
            uxTotalPersonnel.Text = String.Format("{0:N2}", totalPersonnel);
            uxTotalPerDiem.Text = String.Format("{0:N2}", totalPerDiem);
            uxTotalTravel.Text = String.Format("{0:N2}", totalTravel);
            uxTotalMotels.Text = String.Format("{0:N2}", totalMotels);
            uxTotalMisc.Text = String.Format("{0:N2}", totalMisc);
            uxTotalLumpSum.Text = String.Format("{0:N2}", totalLumpSum);

            // Update Sheet's Bottom Numbers
            BBDetail.Sheet.BottomNumbers.Fields bottomData = BBDetail.Sheet.BottomNumbers.Calculate(detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP, totalDaysRemain, totalUnitsRemain, totalDaysWorked);
            uxLaborBurden.Text = String.Format("{0:N2}", bottomData.LABOR_BURDEN.ToString());
            uxAvgUnitsPerDay.Text = String.Format("{0:N2}", bottomData.AVG_UNITS_PER_DAY.ToString());
            uxTotalWklyDirects.Text = String.Format("{0:N2}", bottomData.TOTAL_WKLY_DIRECTS.ToString());
            uxTotalDirectsLeft.Text = String.Format("{0:N2}", bottomData.TOTAL_DIRECTS_LEFT.ToString());
            uxTotalDirectsPerDay.Text = String.Format("{0:N2}", bottomData.TOTAL_DIRECTS_PER_DAY.ToString());
            uxTotalMaterialLeft.Text = String.Format("{0:N2}", bottomData.TOTAL_MATERIAL_LEFT.ToString()); 

            // Update End Numbers
            BBDetail.Sheet.EndNumbers.Fields endNums = BBDetail.Sheet.EndNumbers.Calculate(detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP, recRemaining, totalDaysRemain, totalUnitsRemain, totalDaysWorked);
            decimal eGrossRec = endNums.GROSS_REC;
            decimal eMatUsage = endNums.MAT_USAGE;
            decimal eGrossRev = endNums.GROSS_REV;
            decimal eDirects = endNums.DIR_EXP;
            decimal eOP = endNums.OP;
            uxEGrossRec.Text = String.Format("{0:N2}", eGrossRec);
            uxEMatUsage.Text = String.Format("{0:N2}", eMatUsage);
            uxEGrossRev.Text = String.Format("{0:N2}", eGrossRev);
            uxEDirects.Text = String.Format("{0:N2}", eDirects);
            uxEOP.Text = String.Format("{0:N2}", eOP);

            // Also Update End Numbers in DB
            BBDetail.Sheet.EndNumbers.DBUpdate(detailSheetID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);
        }



        // Other
        protected void deCheckAllowDetailSave(object sender, DirectEventArgs e)
        {
            CheckAllowDetailSave();
        }
        protected void CheckAllowDetailSave()
        {
            char[] charsToTrim = { ' ', '\t' };
            string detailName = uxDetailName.Text.Trim(charsToTrim);

            if (detailName == "")
            {
                uxCloseDetailSheet.Disable();
            }
            else
            {
                uxCloseDetailSheet.Enable();
            }
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
        protected string ConvertSubGridNumToName(string gridNum)
        {
            try
            {
                long testNum = Convert.ToInt64(gridNum);
            }
            catch
            {
                return gridNum;
            }

            switch (gridNum)
            {
                case "1":
                    return "MATERIAL";

                case "2":
                    return "EQUIPMENT";

                case "3":
                    return "PERSONNEL";

                case "4":
                    return "PERDIEM";

                case "5":
                    return "TRAVEL";

                case "6":
                    return "MOTELS";

                case "7":
                    return "MISC";

                case "8":
                    return "LUMPSUM";

                default:
                    return "";
            }
        }
        protected decimal ConvertToDecimal(string strNumber)
        {
            decimal converted;

            try
            {
                converted = Convert.ToDecimal(strNumber);
            }

            catch
            {
                converted = 0;
            }

            return converted;
        }
        protected void deFormatNumber(object sender, DirectEventArgs e)
        {
            Ext.Net.TextField myTextField = sender as Ext.Net.TextField;
            decimal amount;

            try
            {
                amount = Convert.ToDecimal(myTextField.Text);
            }

            catch
            {
                amount = 0;
            }

            string converted = String.Format("{0:N2}", amount);
            myTextField.Text = converted;
        }
    }
}