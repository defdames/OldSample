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

                CalulateDetailSheet(); 
                CheckAllowDetailSave();
            }
        }



        // Material Grid
        protected void deReadMaterialGridData(object sender, StoreReadDataEventArgs e)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);

            uxMaterialGridStore.DataSource = BBDetail.SubGrid.Data.Get(projectID, detailSheetID);
            //uxMaterialGridStore.DataSource = BB.MaterialGridData(projectID, detailSheetID);
        }
        [DirectMethod(Namespace = "SaveRecord")]
        public void deSaveMaterial(object recordData, long id)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailTaskID = long.Parse(Request.QueryString["detailSheetID"]);
            string jsonText = recordData.ToString();
            BB.BBSubGridV materialData = JsonConvert.DeserializeObject<BB.BBSubGridV>(jsonText);
            BUD_BID_DETAIL_SHEET data;

            if (id == 0)
            {
                data = new BUD_BID_DETAIL_SHEET();
            }
            else
            {
                using (Entities _context = new Entities())
                {
                    data = _context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == id).Single();
                }
            }

            data.DESC_1 = materialData.DESC_1;
            data.AMT_1 = materialData.AMT_1;
            data.DESC_2 = materialData.DESC_2;
            data.AMT_2 = materialData.AMT_2;
            decimal amount1 = materialData.AMT_1 ?? 0;
            decimal amount2 = materialData.AMT_2 ?? 0;
            data.TOTAL = amount1 * amount2;

            if (id == 0)
            {
                data.PROJECT_ID = projectID;
                data.DETAIL_TASK_ID = detailTaskID;
                data.REC_TYPE = "MATERIAL";
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
            uxMaterialGridStore.CommitChanges();
            uxMaterialGridStore.Reload();
            //uxMaterialGridPanel.GetStore().GetById(id).Commit();
        }
        


        // Other
        protected void CalulateDetailSheet()
        {
            long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
            decimal sGrossRec = Convert.ToDecimal(uxSGrossRec.Text);
            decimal sMatUsage = Convert.ToDecimal(uxSMatUsage.Text);
            decimal sGrossRev = Convert.ToDecimal(uxSGrossRev.Text);
            decimal sDirects = Convert.ToDecimal(uxSDirects.Text);
            decimal sOP = Convert.ToDecimal(uxSOP.Text);

            BBDetail.Sheet.Subtotals.Fields subtotal = BBDetail.Sheet.Subtotals.Get(detailSheetID);
            uxTotalMaterial.Text = String.Format("{0:N2}", subtotal.MATERIAL);

            BBDetail.Sheet.EndNumbers.Fields endNums = BBDetail.Sheet.EndNumbers.Calculate(detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP);
            decimal eGrossRec = endNums.GROSS_REC;
            decimal eMatUsage = endNums.MAT_USAGE;
            decimal eGrossRev = endNums.GROSS_REV;
            decimal eDirects = endNums.DIR_EXP;
            decimal eOP = endNums.OP;
            BBDetail.Sheet.EndNumbers.DBUpdate(detailSheetID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);
            uxEGrossRec.Text = String.Format("{0:N2}", eGrossRec);
            uxEMatUsage.Text = String.Format("{0:N2}", eMatUsage);
            uxEGrossRev.Text = String.Format("{0:N2}", eGrossRev);
            uxEDirects.Text = String.Format("{0:N2}", eDirects);
            uxEOP.Text = String.Format("{0:N2}", eOP);
        }
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
        [DirectMethod(Namespace = "DeleteRecord")]
        public void deDeleteRecord(object sender, DirectEventArgs e)
        {
            long id = Convert.ToInt64(e.ExtraParams["RecordID"]);
            if (id == 0) { return; }

            BBDetail.SubGrid.DeleteRecord(id);
            uxMaterialGridStore.Reload();
        }
    }
}