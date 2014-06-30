using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Data.DataFactory;
using DBI.Data.Generic;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umYearBudget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)        
        {
            if (!X.IsAjaxRequest)
            {
                long yearID = long.Parse(Request.QueryString["fiscalYear"]);
                long verID = long.Parse(Request.QueryString["verID"]);

                uxHidPrevYear.Text = CalcPrevYear(yearID, verID).ToString();
                uxHidPrevVer.Text = CalcPrevVer(yearID, verID).ToString();
            }
        }

        protected void deLoadActions(object sender, StoreReadDataEventArgs e)
        {
            uxActionsStore.DataSource = StaticLists.YearBudgetProjectActions();
        }

        protected void deChooseAction(object sender, DirectEventArgs e)
        {
            string selectedAction = uxActions.Text;
            uxActions.Text = null;

            if (selectedAction == "Add a New Project")
            {
                ActionAddNewProject();
            }

            else if (selectedAction == "Delete Selected Project")
            {
                ActionDeleteSelectedProject();
            }
        }

        protected void deAllowFormEditing(object sender, DirectEventArgs e)
        {
            uxProjectDetail.Enable();
        }

        protected void ActionAddNewProject()
        {
            uxGridRowModel.ClearSelection();
            uxProjectDetail.Reset();
            uxProjectDetail.Enable();
            uxHidNewProject.Text = "True";
        }

        protected void ActionDeleteSelectedProject()
        {
            if (uxHidNewProject.Text == "True") { return; }

            else if (uxHidBudBidID.Text == "")
            {
                StandardMsgBox("Delete", "A project must be selected before it can be deleted.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Delete", "Are you sure you want to delete the selected project? Once it's been deleted it cannot be retrieved.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteSelectedProject()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }

        [DirectMethod]
        public void DeleteSelectedProject()
        {
            BUD_BID_PROJECTS prjInfoData = new BUD_BID_PROJECTS();
            BUD_BID_ACTUAL_NUM prjInfoData1 = new BUD_BID_ACTUAL_NUM();
            prjInfoData.BUD_BID_PROJECTS_ID = Convert.ToInt64(uxHidBudBidID.Text);
            prjInfoData1.PROJECT_ID = Convert.ToInt64(uxHidBudBidID.Text);

            GenericData.Delete<BUD_BID_PROJECTS>(prjInfoData);
            GenericData.Delete<BUD_BID_ACTUAL_NUM>(prjInfoData1);
            uxSummaryGridStore.Reload();
        }

        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = Request.QueryString["orgName"];
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);
            uxSummaryGridStore.DataSource = BUD_BID.SummaryProjectsWithLineInfo(orgName, orgID, yearID, verID, prevYearID, prevVerID);
        }

        protected void deLoadOrgProjects(object sender, StoreReadDataEventArgs e)
        {
            //long orgID = long.Parse(Request.QueryString["orgID"]);
            //string orgName = Request.QueryString["orgName"];

            //uxProjectNumStore.DataSource = BUD_BID_STATUS.ProjectList(orgID, orgName);




            long orgID = long.Parse(Request.QueryString["orgID"]);
            string orgName = Request.QueryString["orgName"];

            using (Entities context = new Entities())
            {
                string sql = string.Format(@"SELECT TO_CHAR(SYSDATE, 'YYMMDDHH24MISS') AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '-- OVERRIDE --' AS PROJECT_NAME, 'OVERRIDE' AS TYPE, 'ID1' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT '{1}' AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '{0} (Org)' AS PROJECT_NAME, 'ORG' AS TYPE, 'ID2' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT CAST(PROJECTS_V.PROJECT_ID AS VARCHAR(20)) AS PROJECT_ID, PROJECTS_V.SEGMENT1 AS PROJECT_NUM, PROJECTS_V.LONG_NAME AS PROJECT_NAME, 'PROJECT' AS TYPE, 'ID3' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN PA.PA_PROJECT_CLASSES
                    ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup'
                    AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                        UNION ALL
                    SELECT CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_ID, 'N/A' AS PROJECT_NUM, CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_NAME, 'ROLLUP' AS TYPE, 'ID4' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN PA.PA_PROJECT_CLASSES
                    ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup'
                    AND PA.PA_PROJECT_CLASSES.CLASS_CODE <> 'None' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                    GROUP BY CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) 
                    ORDER BY ORDERKEY, PROJECT_NAME", orgName, orgID);
                List<object> dataSource;
                dataSource = context.Database.SqlQuery<ORG_PROJECTS>(sql).ToList<object>();
                int count;
                uxProjectNumStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
                e.Total = count;
            }
        }

        protected void deLoadStatuses(object sender, StoreReadDataEventArgs e)
        {
            uxStatusStore.DataSource = BUD_BID_STATUS.Statuses();
        }

        protected void deLoadJCDates(object sender, StoreReadDataEventArgs e)
        {
            long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
            uxJCDateStore.DataSource = XXDBI_DW.LoadedJcWeDates(hierID, true, 5);
        }       

        public class ORG_PROJECTS
        {
            public string PROJECT_ID { get; set; }
            public string PROJECT_NUM { get; set; }
            public string PROJECT_NAME { get; set; }
            public string TYPE { get; set; }
            public string ORDERKEY { get; set; }
        }

        protected void deGetFormData(object sender, DirectEventArgs e)
        {
            if (uxProjectDetail.Enabled == true) { uxProjectDetail.Disable(); }

            string projectID = e.ExtraParams["ProjectID"];            
            BUD_BID.BUD_SUMMARY_V data = BUD_BID.SummaryProjectsDetail(Convert.ToInt64(projectID));

            uxHidNewProject.Text = "";
            uxHidBudBidID.Text = projectID;
            uxHidType.Text = data.TYPE;
            //uxPrevOPOverrideCheckbox.Checked = data.COMPARE_PRJ_OVERRIDE == "Y" ? true : false;


        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string projectID = e.ExtraParams["ProjectID"];
            string projectType = e.ExtraParams["Type"];
            string projectNum = projectType == "OVERRIDE" ? "-- OVERRIDE --" : e.ExtraParams["ProjectNum"];
            string projectName = projectType == "OVERRIDE" ? null : e.ExtraParams["ProjectName"];

            uxProjectNum.SetValue(projectID, projectNum);
            uxProjectName.Text = projectName;
            uxHidProjectNumID.Text = projectID;
            uxHidType.Text = projectType;

            if (projectType == "OVERRIDE")
            {
                uxJCDate.Text = "-- OVERRIDE --";
                uxJCDate.Disable();
                uxProjectName.ReadOnly = false;
                uxSGrossRec.ReadOnly = false;
                uxSMatUsage.ReadOnly = false;
                //uxSGrossRev.ReadOnly = false;
                uxSDirects.ReadOnly = false;
                //uxSOP.ReadOnly = false;
            }

            else
            {
                if (uxJCDate.Text == "-- OVERRIDE --") { uxJCDate.Text = ""; }
                uxJCDate.Enable();
                uxProjectName.ReadOnly = true;
                uxSGrossRec.ReadOnly = true;
                uxSMatUsage.ReadOnly = true;
                //uxSGrossRev.ReadOnly = true;
                uxSDirects.ReadOnly = true;
                //uxSOP.ReadOnly = true;
            }

            LoadJCNumbers();
        }

        protected void deSelectJCDate(object sender, DirectEventArgs e)
        {
            if (uxJCDate.Text == "-- OVERRIDE --")
            {
                uxSGrossRec.ReadOnly = false;
                uxSMatUsage.ReadOnly = false;
                //uxSGrossRev.ReadOnly = false;
                uxSDirects.ReadOnly = false;
                //uxSOP.ReadOnly = false;
            }

            else
            {
                uxSGrossRec.ReadOnly = true;
                uxSMatUsage.ReadOnly = true;
                //uxSGrossRev.ReadOnly = true;
                uxSDirects.ReadOnly = true;
                //uxSOP.ReadOnly = true;
                LoadJCNumbers();
            }
        }

        protected void LoadJCNumbers()
        {
            string hierID = Request.QueryString["hierID"];
            string orgID = Request.QueryString["orgID"];
            string projectID = uxHidProjectNumID.Text == null ? "" : uxHidProjectNumID.Text.ToString();
            string type = uxHidType.Text == null ? "" : uxHidType.Text.ToString();
            string jcDate = uxJCDate.Text == null || uxJCDate.Text == "-- OVERRIDE --" ? "" : uxJCDate.Text.ToString();
            XXDBI_DW.JOB_COST_V jcLine = null;

            switch (type)
            {
                case "":
                    return;

                case "OVERRIDE":
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64("0"), jcDate);
                    break;

                case "ORG":
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64(hierID), Convert.ToInt64(projectID), jcDate);
                    break;

                case "PROJECT":
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64(projectID), jcDate);
                    break;

                case "ROLLUP":
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64(orgID), projectID, jcDate);
                    break;
            }

            uxSGrossRec.Text = String.Format("{0:N2}", jcLine.FY_GREC);
            uxSMatUsage.Text = String.Format("{0:N2}", jcLine.FY_MU);
            uxSGrossRev.Text = String.Format("{0:N2}", jcLine.FY_GREV);
            uxSDirects.Text = String.Format("{0:N2}", jcLine.FY_TDE);
            uxSOP.Text = String.Format("{0:N2}", jcLine.FY_TOP);
        }

        protected void deReadDetailGridData(object sender, StoreReadDataEventArgs e)
        {
            List<DetailStruct> list = new List<DetailStruct> 
            {
                    new DetailStruct(1, "Test Sheet 1", 10000, 2000, 8000, 1000, 7000),
                    new DetailStruct(2, "Test Sheet 2", 10000, 2000, 8000, 1000, 7000),
                    new DetailStruct(3, "Test Sheet 3", 10000, 2000, 8000, 1000, 7000),
                    new DetailStruct(4, "Test Sheet 4", 10000, 2000, 8000, 1000, 7000)
            };
            uxSummaryDetailStore.DataSource = list;
        }

        class DetailStruct  // DELETE WHEN GETTING DATA FROM CORRECT SOURCE
        {
            public long DETAIL_SHEET_ID { get; set; }
            public string SHEET_NAME { get; set; }
            public decimal GROSS_REC { get; set; }
            public decimal MAT_USAGE { get; set; }
            public decimal GROSS_REV { get; set; }
            public decimal DIR_EXP { get; set; }
            public decimal OP { get; set; }

            public DetailStruct(long id, string sheet, decimal grRec, decimal mat, decimal grRev, decimal dirs, decimal proOP)
            {
                DETAIL_SHEET_ID = id;
                SHEET_NAME = sheet;
                GROSS_REC = grRec;
                MAT_USAGE = mat;
                GROSS_REV = grRev;
                DIR_EXP = dirs;
                OP = proOP;
            }
        }

        protected void deLiabilityCheck(object sender, DirectEventArgs e)
        {
            if (uxLiabilityCheckbox.Checked == true)
            {
                uxLiabilityAmount.Enable();
            }

            else
            {
                uxLiabilityAmount.Disable();
                uxLiabilityAmount.Text = "0.00";
            }
        }

        protected void deCheckAllowSave(object sender, DirectEventArgs e)
        {
            if (uxProjectName.Text == null || String.IsNullOrWhiteSpace(uxProjectName.Text) || uxStatus.SelectedItem.Value == null)
            {
                uxSave.Disable();
            }

            else
            {
                uxSave.Enable();
            }
        }
   
        protected void deSave(object sender, DirectEventArgs e)
        {
            // Write data to BUD_BID_PROJECTS
            BUD_BID_PROJECTS prjInfoData = new BUD_BID_PROJECTS();
            prjInfoData.PROJECT_ID = uxHidProjectNumID.Text;
            prjInfoData.PRJ_NAME = uxProjectName.Text;
            prjInfoData.ORG_ID = Convert.ToInt64(Request.QueryString["OrgID"]);
            prjInfoData.YEAR_ID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            prjInfoData.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
            prjInfoData.STATUS_ID = Convert.ToInt64(uxStatus.Text);
            prjInfoData.ACRES = Convert.ToDecimal(uxAcres.Text);
            prjInfoData.DAYS = Convert.ToDecimal(uxDays.Text);
            //OH_ID?
            if (uxAppType.Text != null) { prjInfoData.APP_TYPE = uxAppType.Text; }
            if (uxChemMix.Text != null) { prjInfoData.CHEMICAL_MIX = uxChemMix.Text; }
            if (uxComments.Text != null) { prjInfoData.COMMENTS = uxComments.Text; }
            if (uxJCDate.Text != null)
            {
                if (uxJCDate.Text == "-- OVERRIDE --")
                {
                    //prjInfoData.WE_OVERRIDE = "Y";
                }

                else
                {
                    //prjInfoData.WE_OVERRIDE = "N";
                    prjInfoData.WE_DATE = Convert.ToDateTime(uxJCDate.Text);
                }
            }
            prjInfoData.TYPE = uxHidType.Text;
            prjInfoData.LIABILITY = uxLiabilityCheckbox.Checked == true ? "Y" : "N";
            //prjInfoData.LIABILITY_OP = Convert.ToDecimal(uxLiabilityAmount.Text); 
            prjInfoData.COMPARE_PRJ_OVERRIDE = uxCompareOverride.Checked == true ? "Y" : "N";
            //prjInfoData.COMPARE_PRJ_AMOUNT = Convert.ToDecimal(uxCompareOP.Text);  

            if (uxHidNewProject.Text == "True")
            {
                GenericData.Insert<BUD_BID_PROJECTS>(prjInfoData);
            }
            else
            {
                GenericData.Update<BUD_BID_PROJECTS>(prjInfoData);
            }

            // Get created project id from BUD_BID_PROJECTS table
            decimal budBidID = prjInfoData.BUD_BID_PROJECTS_ID;
            uxHidBudBidID.Text = budBidID.ToString();

            // Set line numbers:  GrossRec, MatUsage, GrossRev, Directs, OP
            long[] arrLineNum = { 6, 7, 8, 9, 10 };

            // Write data to BUD_BID_ACTUAL_NUM
            BUD_BID_ACTUAL_NUM startNumsdata = new BUD_BID_ACTUAL_NUM();
            Ext.Net.TextField[] arrSControl = { uxSGrossRec, uxSMatUsage, uxSGrossRev, uxSDirects, uxSOP };
            for (int i = 0; i <= 4; i++)
            {
                startNumsdata.PROJECT_ID = Convert.ToInt64(budBidID);
                startNumsdata.DETAIL_TASK_ID = 0;
                startNumsdata.LINE_ID = arrLineNum[i];
                startNumsdata.NOV = Convert.ToDecimal(arrSControl[i].Text);
                if (uxHidNewProject.Text == "True")
                {
                    GenericData.Insert<BUD_BID_ACTUAL_NUM>(startNumsdata);
                }

                else
                {
                    GenericData.Update<BUD_BID_ACTUAL_NUM>(startNumsdata);
                }
            }

            // Write data to BUD_BID_BUDGET_NUM
            BUD_BID_BUDGET_NUM endNumsdata = new BUD_BID_BUDGET_NUM();
            Ext.Net.TextField[] arrEControl = { uxEGrossRec, uxEMatUsage, uxEGrossRev, uxEDirects, uxEOP };
            for (int i = 0; i <= 4; i++)
            {
                endNumsdata.PROJECT_ID = Convert.ToInt64(budBidID);
                endNumsdata.DETAIL_TASK_ID = 0;
                endNumsdata.LINE_ID = arrLineNum[i];
                endNumsdata.NOV = Convert.ToDecimal(arrEControl[i].Text);
                if (uxHidNewProject.Text == "True")
                {
                    GenericData.Insert<BUD_BID_BUDGET_NUM>(endNumsdata);
                }

                else
                {
                    GenericData.Update<BUD_BID_BUDGET_NUM>(endNumsdata);
                }
            }

            NotificationMsg("Save", "Project has been saved.", Icon.DiskBlack);
            uxProjectDetail.Disable();
            uxSummaryGridStore.Reload();
            uxProjectDetail.Reset();
        }
       
        protected void deCancel(object sender, DirectEventArgs e)
        {            
            if (uxHidNewProject.Text == "True")
            {
                uxProjectDetail.Reset();
            }            
            uxProjectDetail.Disable();
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

        protected void NotificationMsg(string title, string msg, Icon msgIcon)
        {
            Notification.Show(new NotificationConfig()
            {
                Icon = msgIcon,
                Title = title,
                Html = msg,
                HideDelay = 1000,   
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
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

        protected long CalcPrevYear(long curYear, long curVer)
        {
            switch (curVer)
            {
                case 1:  // Bid
                    return curYear;

                case 2:  // First Draft
                    return curYear;

                case 3:  // Final Draft
                    return (curYear - 1);                    

                case 4:  // 1st Reforecast
                    return curYear;

                case 5:  // 2nd Reforecast
                    return curYear;

                case 6:  // 3rd Reforecast
                    return curYear;

                case 7:  // 4th Reforecast
                    return curYear;

                default:
                    return 0;
            }
        }

        protected long CalcPrevVer(long curYear, long curVer)
        {
            switch (curVer)
            {
                case 1:  // Bid
                    return 1;

                case 2:  // First Draft
                    return 1;

                case 3:  // Final Draft
                    return 2;

                case 4:  // 1st Reforecast
                    return 3;

                case 5:  // 2nd Reforecast
                    return 4;

                case 6:  // 3rd Reforecast
                    return 5;

                case 7:  // 4th Reforecast
                    return 6;

                default:
                    return 0;
            }
        }

        protected void Test(object sender, DirectEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long version = long.Parse(Request.QueryString["version"]);
            X.MessageBox.Alert("Title", orgID + " " + version + " " + e.ExtraParams["SheetName"]).Show();
        }

        protected void deCalcGRandOP(object sender, DirectEventArgs e)
        {
            decimal sGrossRec = Convert.ToDecimal(uxSGrossRec.Text);
            decimal sMatUsage = Convert.ToDecimal(uxSMatUsage.Text);            
            decimal sDirects = Convert.ToDecimal(uxSDirects.Text);
            decimal sGrossRev = sGrossRec - sMatUsage;
            decimal sOP = sGrossRev - sDirects;

            uxSGrossRev.Text = String.Format("{0:N2}", sGrossRev);
            uxSOP.Text = String.Format("{0:N2}", sOP);
        }
    }
}