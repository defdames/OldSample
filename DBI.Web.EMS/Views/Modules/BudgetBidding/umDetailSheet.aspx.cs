using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;
using Newtonsoft.Json;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umDetailSheet : BasePage
    {
        // Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                long leOrgID = long.Parse(Request.QueryString["leOrgID"]);
                long orgID = long.Parse(Request.QueryString["orgID"]);
                long yearID = long.Parse(Request.QueryString["yearID"]);
                long verID = long.Parse(Request.QueryString["verID"]);

                if (BB.IsReadOnly(orgID, yearID, verID) == true)
                {
                    uxCloseDetailSheet.Enable();
                    LockDetailSheet();
                }
                else
                {
                    uxCloseDetailSheet.Disable();
                }

                string verName = Request.QueryString["verName"];
                string weDate = Request.QueryString["weDate"];
                long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
                long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
                string projectName = Request.QueryString["projectName"];
                long sheetNum = long.Parse(Request.QueryString["sheetNum"]);
                string detailSheetName = Request.QueryString["detailSheetName"];
                long sheetCount = BBDetail.Sheets.MaxOrder(budBidProjectID);

                uxYearVersion.Text = yearID + " " + verName;
                uxWeekEnding.Text = "Week Ending:  " + weDate;
                uxProjectName.Text = projectName;
                uxDetailNameLabel.Text = "Detail Sheet (" + sheetNum + " of " + sheetCount + "):";
                detailSheetName = (detailSheetName.Length >= 10 && detailSheetName.Substring(0, 10) == "SYS_DETAIL") ? "" : detailSheetName;
                uxDetailName.Text = detailSheetName;
                uxComments.Text = BBDetail.Sheet.MainTabField.Comment(detailSheetID);
                uxLiabilityCheckbox.Checked = BBDetail.Sheet.MainTabField.Liability(detailSheetID) == "Y" ? true : false;

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

                uxSGrossRec.Text = String.Format("{0:N2}", sGrossRec);
                uxSMatUsage.Text = String.Format("{0:N2}", sMatUsage);
                uxSGrossRev.Text = String.Format("{0:N2}", sGrossRev);
                uxSDirects.Text = String.Format("{0:N2}", sDirects);
                uxSOP.Text = String.Format("{0:N2}", sOP);

                BBDetail.Sheet.MainTabField.Fields sheetMainNums = BBDetail.Sheet.MainTabField.NumsData(detailSheetID);
                decimal recRemain = sheetMainNums.RECREMAIN ?? 0;
                decimal daysRemain = sheetMainNums.DAYSREMAIN ?? 0;
                decimal unitsRemain = sheetMainNums.UNITREMAIN ?? 0;
                decimal daysWorked = sheetMainNums.DAYSWORKED ?? 0;
                uxRecRemaining.Text = String.Format("{0:N2}", recRemain);
                uxDaysRemaining.Text = String.Format("{0:N2}", daysRemain);
                uxUnitsRemaining.Text = String.Format("{0:N2}", unitsRemain);
                uxDaysWorked.Text = String.Format("{0:N2}", daysWorked);

                decimal laborBurden = BB.LaborBurdenRate(leOrgID, yearID) * 100;
                uxLaborBurdenLabel.Text = "Labor Burden @ " + (String.Format("{0:N2}", laborBurden)) + "%:";

                uxAddNewBOM.Visible = BB.ShowBOMOrgSetting(orgID);

                CalulateDetailSheet(true);

                CheckAllowDetailSave();
            }
        }



        // Main tab
        public void deSaveMainTabField(object sender, DirectEventArgs e)
        {
            long budBidProjectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
            string recType = e.ExtraParams["RecType"];
            string fieldText = e.ExtraParams["FieldText"];

            if (recType == "COMMENTS")
            {
                BBDetail.Sheet.MainTabField.DBUpdateComments(detailSheetID, fieldText);
            }
            else if (recType == "LIABILITY")
            {
                if (uxLiabilityCheckbox.Checked == true)
                {
                    BBDetail.Sheet.MainTabField.DBUpdateLiability(detailSheetID, "Y");
                }
                else
                {
                    BBDetail.Sheet.MainTabField.DBUpdateLiability(detailSheetID, "N");
                }                
            }
            else
            {
                Ext.Net.TextField myTextField = sender as Ext.Net.TextField;
                decimal retVal = ForceToDecimal(myTextField.Text, -9999999999.99M, 9999999999.99M);
                myTextField.Text = String.Format("{0:N2}", retVal);
                BBDetail.Sheet.MainTabField.DBUpdateNums(budBidProjectID, detailSheetID, recType, retVal);
                CalulateDetailSheet();
            }
        }



        // Sub grid
        protected void deReadGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);
            string recType = e.Parameters["RecordType"];

            switch (recType)
            {
                case "MATERIAL":
                    uxMaterialGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "EQUIPMENT":
                    uxEquipmentGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "PERSONNELR":
                    uxPersonnelRTGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "PERSONNELO":
                    uxPersonnelOTGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "PERDIEM":
                    uxPerDiemGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "TRAVEL":
                    uxTravelGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "MOTELS":
                    uxMotelsGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "MISC":
                    uxMiscGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                case "LUMPSUM":
                    uxLumpSumGridStore.DataSource = BBDetail.SubGrid.Data.Get(orgID, projectID, detailSheetID, recType);
                    break;

                default:
                    break;
            }
        }
        protected void deAddNewRecord(object sender, DirectEventArgs e)
        {            
            long leOrgID = long.Parse(Request.QueryString["leOrgID"]);
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailTaskID = long.Parse(Request.QueryString["detailSheetID"]);
            string detailSheetName = e.ExtraParams["DetailSheetName"];
            string recType = e.ExtraParams["RecordType"];

            bool addPersOTRec;
            long rtDetailSheetID = 0;
            do
            {
                addPersOTRec = false;

                BUD_BID_DETAIL_SHEET data;
                data = new BUD_BID_DETAIL_SHEET();

                switch (recType)
                {
                    case "MATERIAL":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "EQUIPMENT":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 1;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "PERSONNELR":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 1;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "PERSONNELO":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 1;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        data.PARENT_REC_ID = rtDetailSheetID;
                        break;

                    case "PERDIEM":
                        data.DESC_1 = "";
                        data.DESC_2 = "";
                        data.AMT_1 = BB.PerDiemRate(leOrgID);
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "TRAVEL":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 1;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "MOTELS":
                        data.DESC_1 = "";
                        data.DESC_2 = "";
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "MISC":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;

                    case "LUMPSUM":
                        data.DESC_1 = "[NEW]";
                        data.DESC_2 = "";
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = 0;
                        break;
                }
         
                data.PROJECT_ID = projectID;
                data.DETAIL_TASK_ID = detailTaskID;
                data.REC_TYPE = recType;
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now; ;
                data.MODIFIED_BY = "TEMP";
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(data);

                // Get newly created detail sheet id
                rtDetailSheetID = Convert.ToInt64(data.DETAIL_SHEET_ID);

                switch (recType)
                {
                    case "MATERIAL":
                        uxMaterialGridStore.Reload();
                        uxHidSelMatRecID.Text = data.DETAIL_SHEET_ID.ToString();
                        break;

                    case "EQUIPMENT":
                        uxEquipmentGridStore.Reload();
                        uxHidSelEquipRecID.Text = data.DETAIL_SHEET_ID.ToString();
                        break;

                    case "PERSONNELR":
                        uxPersonnelRTGridStore.Reload();
                        uxHidSelPersRTRecID.Text = data.DETAIL_SHEET_ID.ToString();
                        addPersOTRec = true;
                        recType = "PERSONNELO";
                        break;

                    case "PERSONNELO":
                        uxPersonnelOTGridStore.Reload();
                        uxHidSelPersOTRecID.Text = data.DETAIL_SHEET_ID.ToString();
                        break;

                    case "PERDIEM":
                        uxPerDiemGridStore.Reload();
                        break;

                    case "TRAVEL":
                        uxTravelGridStore.Reload();
                        uxHidSelTravelPersRecID.Text = data.DETAIL_SHEET_ID.ToString();
                        break;

                    case "MOTELS":
                        uxMotelsGridStore.Reload();
                        break;

                    case "MISC":
                        uxMiscGridStore.Reload();
                        break;

                    case "LUMPSUM":
                        uxLumpSumGridStore.Reload();
                        break;
                }

            } while (addPersOTRec == true);
        }
        [DirectMethod(Namespace = "DeleteRecord")]
        public void deDeleteRecord(object sender, DirectEventArgs e)
        {
            uxHidDelRecord.Text = e.ExtraParams["RecordID"];

            X.MessageBox.Confirm("Delete", "Are sure you want to delete the selected record?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteRecordContinued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }
        [DirectMethod]
        public void DeleteRecordContinued()
        {
            long id = Convert.ToInt64(uxHidDelRecord.Text);
            BBDetail.SubGrid.DeleteRecord(id);

            long otDetailID = BBDetail.SubGrid.GetOTRecID(id);
            if (otDetailID != 0) { BBDetail.SubGrid.DeleteRecord(otDetailID); }

            uxMaterialGridStore.Reload();
            uxEquipmentGridStore.Reload();
            uxPersonnelRTGridStore.Reload();
            uxPersonnelOTGridStore.Reload();
            uxPerDiemGridStore.Reload();
            uxTravelGridStore.Reload();
            uxMotelsGridStore.Reload();
            uxMiscGridStore.Reload();
            uxLumpSumGridStore.Reload();
            uxHidSelMatRecID.Text = "";
            CalulateDetailSheet();
        }
        [DirectMethod(Namespace = "SaveRecord")]
        public void deSaveSubGridData(long id, string recType, string field, string newValue)
        {
            BUD_BID_DETAIL_SHEET data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == id).Single();
            }

            if (field == "DESC_1") { data.DESC_1 = newValue; }
            if (field == "DESC_2") { data.DESC_2 = newValue; }
            if (field == "AMT_1") { data.AMT_1 = Convert.ToDecimal(newValue); }
            if (field == "AMT_2") { data.AMT_2 = Convert.ToDecimal(newValue); }
            if (field == "AMT_3") { data.AMT_3 = Convert.ToDecimal(newValue); }
            if (field == "AMT_4") { data.AMT_4 = Convert.ToDecimal(newValue); }
            if (field == "AMT_5") { data.AMT_5 = Convert.ToDecimal(newValue); }

            switch (recType)
            {
                case "MATERIAL":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxMaterialGridStore.CommitChanges();
                    uxMaterialGridStore.Reload();
                    break;

                case "EQUIPMENT":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxEquipmentGridStore.CommitChanges();
                    uxEquipmentGridStore.Reload();
                    break;

                case "PERSONNELR":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxPersonnelRTGridStore.CommitChanges();
                    uxPersonnelRTGridStore.Reload();

                    BUD_BID_DETAIL_SHEET otData;
                    using (Entities context = new Entities())
                    {
                        otData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PARENT_REC_ID == id).Single();
                    }

                    try
                    {
                        otData.AMT_1 = data.AMT_1;
                        otData.DESC_1 = data.DESC_1;
                        otData.AMT_3 = data.AMT_3 * 1.5M;
                        otData.TOTAL = otData.AMT_1 * otData.AMT_2 * (data.AMT_3 * 1.5M);
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(otData);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        otData.AMT_1 = 0;
                        otData.AMT_2 = 0;
                        otData.AMT_3 = 0;
                        otData.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(otData);
                    }

                    uxPersonnelOTGridStore.CommitChanges();
                    uxPersonnelOTGridStore.Reload();

                    break;

                case "PERSONNELO":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxPersonnelOTGridStore.CommitChanges();
                    uxPersonnelOTGridStore.Reload();
                    break;

                case "PERDIEM":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxPerDiemGridStore.CommitChanges();
                    uxPerDiemGridStore.Reload();
                    break;

                case "TRAVEL":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxTravelGridStore.CommitChanges();
                    uxTravelGridStore.Reload();
                    break;

                case "MOTELS":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxMotelsGridStore.CommitChanges();
                    uxMotelsGridStore.Reload();
                    break;

                case "MISC":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxMiscGridStore.CommitChanges();
                    uxMiscGridStore.Reload();
                    break;

                case "LUMPSUM":
                    try
                    {
                        data.TOTAL = data.AMT_1 * data.AMT_2;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                    catch
                    {
                        StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.TOTAL = 0;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }

                    uxLumpSumGridStore.CommitChanges();
                    uxLumpSumGridStore.Reload();
                    break;

                default:
                    break;
            }

            CalulateDetailSheet();
        }



        // Material grid - MAKE SURE TO UPDATE 'deSaveSubGridData' method above with same formula as 'deSelect...'
        protected void deLoadMaterialDropdown(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            List<object> dataSource = BBDetail.Sheet.MaterialListing.Data(orgID).ToList<object>();
            int count;

            uxMaterialStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }
        protected void deSelectMaterial(object sender, DirectEventArgs e)
        {
            long recordID = Convert.ToInt64(uxHidSelMatRecID.Text);
            string material = e.ExtraParams["Material"];
            string unitCost = e.ExtraParams["UnitCost"];
            string uom = e.ExtraParams["UOM"];

            uxMaterialPicker.SetValue(material);

            BUD_BID_DETAIL_SHEET data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == recordID).Single();
            }

            try
            {
                data.DESC_1 = material;
                data.AMT_1 = Convert.ToDecimal(unitCost);
                data.DESC_2 = uom;
                data.TOTAL = data.AMT_1 * data.AMT_2;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }
            catch
            {
                StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                data.AMT_1 = 0;
                data.AMT_2 = 0;
                data.TOTAL = 0;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }

            uxMaterialGridStore.CommitChanges();
            uxMaterialGridStore.Reload();

            CalulateDetailSheet();
            uxMaterialFilter.ClearFilter();
        }
        protected void deGetMatRecID(object sender, DirectEventArgs e)
        {
            string recordID = e.ExtraParams["SelRecordID"];
            uxHidSelMatRecID.Text = recordID;
        }



        // Equipment grid - MAKE SURE TO UPDATE 'deSaveSubGridData' method above with same formula as 'deSelect...'
        protected void deLoadEquipmentDropdown(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            List<object> dataSource = BBDetail.Sheet.EquipmentListing.Data(orgID).ToList<object>();
            int count;

            uxEquipmentStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }
        protected void deSelectEquipment(object sender, DirectEventArgs e)
        {
            long recordID = Convert.ToInt64(uxHidSelEquipRecID.Text);
            string equipment = e.ExtraParams["Equipment"];
            string costPerHour = e.ExtraParams["CostPerHour"];

            uxEquipmentPicker.SetValue(equipment);

            BUD_BID_DETAIL_SHEET data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == recordID).Single();
            }

            try
            {
                data.DESC_1 = equipment;
                data.AMT_3 = Convert.ToDecimal(costPerHour);
                data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }
            catch
            {
                StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                data.AMT_1 = 0;
                data.AMT_2 = 0;
                data.AMT_3 = 0;
                data.TOTAL = 0;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }
            
            uxEquipmentGridStore.CommitChanges();
            uxEquipmentGridStore.Reload();

            CalulateDetailSheet();
            uxEquipmentFilter.ClearFilter();
        }
        protected void deGetEquipRecID(object sender, DirectEventArgs e)
        {
            string recordID = e.ExtraParams["SelRecordID"];
            uxHidSelEquipRecID.Text = recordID;
        }



        // Personnel grid - MAKE SURE TO UPDATE 'deSaveSubGridData' method above with same formula as 'deSelect...'
        protected void deLoadPersonnelRTDropdown(object sender, StoreReadDataEventArgs e)
        {
            string company = "DBI";
            List<object> dataSource = BBDetail.Sheet.PersonnelListing.Data(company).ToList<object>();
            int count;

            uxPersonnelRTStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }
        protected void deLoadPersonnelOTDropdown(object sender, StoreReadDataEventArgs e)
        {
            string company = "DBI";
            List<object> dataSource = BBDetail.Sheet.PersonnelListing.Data(company).ToList<object>();
            int count;

            uxPersonnelOTStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }
        protected void deSelectRTPosition(object sender, DirectEventArgs e)
        {
            long recordID = Convert.ToInt64(uxHidSelPersRTRecID.Text);
            string position = e.ExtraParams["Position"];
            string costPerHour = e.ExtraParams["CostPerHour"];

            uxPersonnelRTPicker.SetValue(position);

            BUD_BID_DETAIL_SHEET data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == recordID).Single();
            }

            try
            {
                data.DESC_1 = position;
                data.AMT_3 = Convert.ToDecimal(costPerHour);
                data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }
            catch
            {
                StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                data.AMT_1 = 0;
                data.AMT_2 = 0;
                data.AMT_3 = 0;
                data.TOTAL = 0;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }

            uxPersonnelRTGridStore.CommitChanges();
            uxPersonnelRTGridStore.Reload();

            BUD_BID_DETAIL_SHEET otData;
            using (Entities context = new Entities())
            {
                otData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PARENT_REC_ID == recordID).Single();
            }

            try
            {
                otData.DESC_1 = position;
                otData.AMT_3 = data.AMT_3 * 1.5M;
                otData.TOTAL = otData.AMT_1 * otData.AMT_2 * (data.AMT_3 * 1.5M);
                GenericData.Update<BUD_BID_DETAIL_SHEET>(otData);
            }
            catch
            {
                StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                otData.AMT_1 = 0;
                otData.AMT_2 = 0;
                otData.AMT_3 = 0;
                otData.TOTAL = 0;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(otData);
            }

            uxPersonnelOTGridStore.CommitChanges();
            uxPersonnelOTGridStore.Reload();

            CalulateDetailSheet();
            uxPersonnelRTFilter.ClearFilter();
        }
        protected void deSelectOTPosition(object sender, DirectEventArgs e)
        {
            long recordID = Convert.ToInt64(uxHidSelPersOTRecID.Text);
            string position = e.ExtraParams["Position"];
            string costPerHour = e.ExtraParams["CostPerHour"];

            uxPersonnelOTPicker.SetValue(position);

            BUD_BID_DETAIL_SHEET data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == recordID).Single();
            }

            try
            {
                data.DESC_1 = position;
                data.AMT_3 = Convert.ToDecimal(costPerHour);
                data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }
            catch
            {
                StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                data.AMT_1 = 0;
                data.AMT_2 = 0;
                data.AMT_3 = 0;
                data.TOTAL = 0;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }

            uxPersonnelOTGridStore.CommitChanges();
            uxPersonnelOTGridStore.Reload();

            CalulateDetailSheet();
            uxPersonnelOTFilter.ClearFilter();
        }
        protected void deGetPersRTRecID(object sender, DirectEventArgs e)
        {
            string recordID = e.ExtraParams["SelRecordID"];
            uxHidSelPersRTRecID.Text = recordID;
        }
        protected void deGetPersOTRecID(object sender, DirectEventArgs e)
        {
            string recordID = e.ExtraParams["SelRecordID"];
            uxHidSelPersOTRecID.Text = recordID;
        }


        // Travel grid - MAKE SURE TO UPDATE 'deSaveSubGridData' method above with same formula as 'deSelect...'
        protected void deLoadTravelPersonnelDropdown(object sender, StoreReadDataEventArgs e)
        {
            string company = "DBI";
            List<object> dataSource = BBDetail.Sheet.PersonnelListing.Data(company).ToList<object>();
            int count;

            uxTravelPersonnelStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }
        protected void deSelectTravelPosition(object sender, DirectEventArgs e)
        {
            long recordID = Convert.ToInt64(uxHidSelTravelPersRecID.Text);
            string position = e.ExtraParams["Position"];
            string costPerHour = e.ExtraParams["CostPerHour"];

            uxTravelPersonnelPicker.SetValue(position);

            BUD_BID_DETAIL_SHEET data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == recordID).Single();
            }

            try
            {
                data.DESC_1 = position;
                data.AMT_2 = Convert.ToDecimal(costPerHour);
                data.TOTAL = data.AMT_1 * data.AMT_2 * data.AMT_3;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }
            catch
            {
                StandardMsgBox("Error", "Calculated number to large.", "WARNING");
                data.AMT_1 = 0;
                data.AMT_2 = 0;
                data.AMT_3 = 0;
                data.TOTAL = 0;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }

            uxTravelGridStore.CommitChanges();
            uxTravelGridStore.Reload();

            CalulateDetailSheet();
            uxTravelPersonnelFilter.ClearFilter();
        }
        protected void deGetTravelPersRecID(object sender, DirectEventArgs e)
        {
            string recordID = e.ExtraParams["SelRecordID"];
            uxHidSelTravelPersRecID.Text = recordID;
        }


        // Calculate
        protected void deCalculate(object sender, DirectEventArgs e)
        {
            CalulateDetailSheet();
        }
        protected void CalulateDetailSheet(bool pageLoad = false)
        {
            try
            {
                long leOrgID = long.Parse(Request.QueryString["leOrgID"]);
                long orgID = long.Parse(Request.QueryString["orgID"]);
                long yearID = long.Parse(Request.QueryString["yearID"]);
                long verID = long.Parse(Request.QueryString["verID"]);

                if (pageLoad == false && BB.IsReadOnly(orgID, yearID, verID) == true) { return; }

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
                decimal totalPersonnelRT = subtotal.PERSONNELR ?? 0;
                decimal totalPersonnelOT = subtotal.PERSONNELO ?? 0;
                decimal totalPerDiem = subtotal.PERDIEM ?? 0;
                decimal totalTravel = subtotal.TRAVEL ?? 0;
                decimal totalMotels = subtotal.MOTELS ?? 0;
                decimal totalMisc = subtotal.MISC ?? 0;
                decimal totalLumpSum = subtotal.LUMPSUM ?? 0;
                uxTotalMaterial.Text = String.Format("{0:N2}", totalMaterial);
                uxTotalEquipment.Text = String.Format("{0:N2}", totalEquipment);
                uxTotalPersonnelRT.Text = String.Format("{0:N2}", totalPersonnelRT);
                uxTotalPersonnelOT.Text = String.Format("{0:N2}", totalPersonnelOT);
                uxTotalPerDiem.Text = String.Format("{0:N2}", totalPerDiem);
                uxTotalTravel.Text = String.Format("{0:N2}", totalTravel);
                uxTotalMotels.Text = String.Format("{0:N2}", totalMotels);
                uxTotalMisc.Text = String.Format("{0:N2}", totalMisc);
                uxTotalLumpSum.Text = String.Format("{0:N2}", totalLumpSum);

                // Update Sheet's Bottom Numbers
                BBDetail.Sheet.BottomNumbers.Fields bottomData = BBDetail.Sheet.BottomNumbers.Calculate(leOrgID, yearID, detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP, totalDaysRemain, totalUnitsRemain, totalDaysWorked);
                uxLaborBurden.Text = String.Format("{0:N2}", bottomData.LABOR_BURDEN);
                uxAvgUnitsPerDay.Text = String.Format("{0:N2}", bottomData.AVG_UNITS_PER_DAY);
                uxTotalWklyDirects.Text = String.Format("{0:N2}", bottomData.TOTAL_WKLY_DIRECTS);
                uxTotalDirectsLeft.Text = String.Format("{0:N2}", bottomData.TOTAL_DIRECTS_LEFT);
                uxTotalDirectsPerDay.Text = String.Format("{0:N2}", bottomData.TOTAL_DIRECTS_PER_DAY);
                uxTotalMaterialLeft.Text = String.Format("{0:N2}", bottomData.TOTAL_MATERIAL_LEFT);

                // Update End Numbers
                BBDetail.Sheet.EndNumbers.Fields endNums = BBDetail.Sheet.EndNumbers.Calculate(leOrgID, yearID, detailSheetID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP, recRemaining, totalDaysRemain, totalUnitsRemain, totalDaysWorked);
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
            catch
            {
                X.Js.Call("closeWindowError");
            }
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
        //protected string ConvertSubGridNumToName(string gridNum)
        //{
        //    try
        //    {
        //        long testNum = Convert.ToInt64(gridNum);
        //    }
        //    catch
        //    {
        //        return gridNum;
        //    }

        //    switch (gridNum)
        //    {
        //        case "1":
        //            return "MATERIAL";

        //        case "2":
        //            return "EQUIPMENT";

        //        case "3":
        //            return "PERSONNELR";

        //        case "3":
        //            return "PERSONNELO";

        //        case "4":
        //            return "PERDIEM";

        //        case "5":
        //            return "TRAVEL";

        //        case "6":
        //            return "MOTELS";

        //        case "7":
        //            return "MISC";

        //        case "8":
        //            return "LUMPSUM";

        //        default:
        //            return "";
        //    }
        //}
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
        protected void LockDetailSheet()
        {
            uxAddNewMaterial.Disable();
            uxDeleteMaterial.Disable();
            uxAddNewEquipment.Disable();
            uxDeleteEquipment.Disable();
            uxAddNewPersonnelRT.Disable();
            uxDeletePersonnelOT.Disable();
            uxAddNewPerDiem.Disable();
            uxDeletePerDiem.Disable();
            uxAddNewTravel.Disable();
            uxDeleteTravel.Disable();
            uxAddNewMotel.Disable();
            uxDeleteMotel.Disable();
            uxAddNewMisc.Disable();
            uxDeleteMisc.Disable();
            uxAddNewLumpSum.Disable();
            uxDeleteLumpSum.Disable();

            uxDetailName.ReadOnly = true;
            uxRecRemaining.ReadOnly = true;
            uxDaysWorked.ReadOnly = true;
            uxDaysRemaining.ReadOnly = true;
            uxUnitsRemaining.ReadOnly = true;
            uxComments.ReadOnly = true;

            uxMaterialPicker.ReadOnly = true;
            uxMaterialUnitCost.ReadOnly = true;
            uxMaterialUOM.ReadOnly = true;
            uxMaterialQuantity.ReadOnly = true;

            uxEquipmentQuantity.ReadOnly = true;
            uxEquipmentPicker.ReadOnly = true;
            uxEquipmentHours.ReadOnly = true;
            uxEquipmentCostPerHour.ReadOnly = true;

            uxPersonnelRTQuantity.ReadOnly = true;
            uxPersonnelRTPicker.ReadOnly = true;
            uxPersonnelRTHours.ReadOnly = true;
            uxPersonnelRTCostPerHour.ReadOnly = true;

            uxPerDiemRate.ReadOnly = true;
            uxPerDiemNumOfDays.ReadOnly = true;
            uxPerDiemNumOfPerDiems.ReadOnly = true;

            uxTravelPay.ReadOnly = true;
            uxTravelHours.ReadOnly = true;

            uxMotelRate.ReadOnly = true;
            uxMotelNumOfDays.ReadOnly = true;
            uxMotelNumOfRooms.ReadOnly = true;

            uxMiscDesc.ReadOnly = true;
            uxMiscQuantity.ReadOnly = true;
            uxMiscCost.ReadOnly = true;

            uxLumpSumDesc.ReadOnly = true;
            uxLumpSumQuantity.ReadOnly = true;
            uxLumpSumCost.ReadOnly = true;
        }



        protected void deAddNewBOM(object sender, DirectEventArgs e)
        {
            string hierID = Request.QueryString["hierID"];
            string leOrgID = Request.QueryString["leOrgID"];
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);
            string budBidProjectID = Request.QueryString["projectID"];
            string detailSheetID = Request.QueryString["detailSheetID"];

            string url = "/Views/Modules/BudgetBidding/umBOM.aspx?hierID=" + hierID + "&leOrgID=" + leOrgID + "&orgID=" + orgID + "&yearID=" + yearID + "&verName=" + verName + "&projectID=" + budBidProjectID + "&detailSheetID=" + detailSheetID;

            Window win = new Window
            {
                ID = "uxBOMForm",
                Height = 330,
                Width = 600,
                Title = "BOM",
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
        public void CloseBOMWindow()
        {
            uxMaterialGridStore.Reload();
            CalulateDetailSheet();
        }
    }
}