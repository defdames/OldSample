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

        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = Request.QueryString["orgName"];
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);
            uxSummaryGridStore.DataSource = BUD_BID_STATUS.SummaryProjectsWithLineInfo(orgName, orgID, yearID, verID, prevYearID, prevVerID);
        }       

        protected void deGetFormData(object sender, DirectEventArgs e)
        {
            uxProjectDetail.Disable();
            string recordID = e.ExtraParams["RecordID"];
            uxHidProjectID.Text = recordID;
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
        
        protected void deCancel(object sender, DirectEventArgs e)
        {            
            if (uxHidNewProject.Text == "True")
            {
                uxProjectDetail.Reset();
            }            
            uxProjectDetail.Disable();
        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string type = e.ExtraParams["Type"];
            uxHidProjectID.Text = e.ExtraParams["ProjectID"];
            uxHidProjectNum.Text = e.ExtraParams["ProjectNum"];
            uxHidProjectName.Text = e.ExtraParams["ProjectName"];
            uxHidType.Text = type;
            LoadJCNumbers();
        }

        protected void deSelectJCDate(object sender, DirectEventArgs e)
        {
            uxHidDate.Text = uxJCDate.Text;
            if (uxJCDate.Text == "-- OVERRIDE --")
            {
                uxSGrossRec.ReadOnly = false;
                uxSMatUsage.ReadOnly = false;
                uxSGrossRev.ReadOnly = false;
                uxSDirects.ReadOnly = false;
                uxSOP.ReadOnly = false;
            }

            else
            {
                uxSGrossRec.ReadOnly = true;
                uxSMatUsage.ReadOnly = true;
                uxSGrossRev.ReadOnly = true;
                uxSDirects.ReadOnly = true;
                uxSOP.ReadOnly = true;
                LoadJCNumbers();
            }
        }

        protected void LoadJCNumbers()
        {
            string hierID = Request.QueryString["hierID"];
            string orgID = Request.QueryString["orgID"];
            string projectID = uxHidProjectID.Text == null ? "" : uxHidProjectID.Text.ToString();
            string projectNum = uxHidProjectNum.Text == null ? "" : uxHidProjectNum.Text.ToString();
            string projectName = uxHidProjectName.Text == null ? "" : uxHidProjectName.Text.ToString();
            string type = uxHidType.Text == null ? "" : uxHidType.Text.ToString();
            string jcDate = uxHidDate.Text == null ? "" : uxHidDate.Text.ToString();
            XXDBI_DW.JOB_COST_V jcLine = null;

            switch (type)
            {
                case "":
                    return;

                case "OVERRIDE":
                    uxProjectNum.SetValue(projectID, projectName);
                    uxProjectName.SetValue(null);
                    projectOverride(true);
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64("0"), jcDate);
                    break;

                case "ORG":
                    uxProjectNum.SetValue(projectID, projectNum);
                    uxProjectName.SetValue(projectName);
                    projectOverride(false);
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64(hierID), Convert.ToInt64(projectID), jcDate);
                    break;

                case "PROJECT":
                    uxProjectNum.SetValue(projectID, projectNum);
                    uxProjectName.SetValue(projectName);
                    projectOverride(false);
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64(projectID), jcDate);
                    break;

                case "ROLLUP":
                    uxProjectNum.SetValue(projectID, projectNum);
                    uxProjectName.SetValue(projectName);
                    projectOverride(false);
                    jcLine = XXDBI_DW.JcSummaryLineAmounts(Convert.ToInt64(orgID), projectID, jcDate);
                    break;
            }


            uxSGrossRec.Text = String.Format("{0:N2}", jcLine.FY_GREC);
            uxSMatUsage.Text = String.Format("{0:N2}", jcLine.FY_MU);
            uxSGrossRev.Text = String.Format("{0:N2}", jcLine.FY_GREV);
            uxSDirects.Text = String.Format("{0:N2}", jcLine.FY_TDE);
            uxSOP.Text = String.Format("{0:N2}", jcLine.FY_TOP);
        }

        protected void deProjectDropdownDeactivate(object sender, DirectEventArgs e)
        {
            uxProjectFilter.ClearFilter();
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

        public class ORG_PROJECTS
        {
            public string PROJECT_ID { get; set; }
            public string PROJECT_NUM { get; set; }
            public string PROJECT_NAME { get; set; }
            public string TYPE { get; set; }
            public string ORDERKEY { get; set; }
        }

        protected void projectOverride(Boolean projOverride)
        {
            if (projOverride == true)
            {
                uxJCDate.Text = null;
                uxHidDate.Text = null;
                uxJCDate.Disable();
                uxProjectName.ReadOnly = false;
                uxSGrossRec.ReadOnly = false;
                uxSMatUsage.ReadOnly = false;
                uxSGrossRev.ReadOnly = false;
                uxSDirects.ReadOnly = false;
                uxSOP.ReadOnly = false;
            }

            else
            {
                uxJCDate.Enable();
                uxProjectName.ReadOnly = true;
                uxSGrossRec.ReadOnly = true;
                uxSMatUsage.ReadOnly = true;
                uxSGrossRev.ReadOnly = true;
                uxSDirects.ReadOnly = true;
                uxSOP.ReadOnly = true;
            }
        }

        protected void deLoadJCDates(object sender, StoreReadDataEventArgs e)
        {
            long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
            uxJCDateStore.DataSource = XXDBI_DW.LoadedJcWeDates(hierID, true, 5);
        }       

        protected void Test(object sender, DirectEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long version = long.Parse(Request.QueryString["version"]);
            X.MessageBox.Alert("Title", orgID + " " + version + " " + e.ExtraParams["SheetName"]).Show();
        }

        protected void deSave(object sender, DirectEventArgs e)
        {
            // Write project info to DB
            BUD_BID_PROJECTS prjInfoData = new BUD_BID_PROJECTS();
            prjInfoData.PROJECT_ID = Convert.ToInt64(uxHidProjectID.Text);
            prjInfoData.PRJ_NAME = uxProjectName.Text;
            prjInfoData.TYPE = uxHidType.Text;
            prjInfoData.ORG_ID = long.Parse(Request.QueryString["OrgID"]);
            prjInfoData.YEAR_ID = long.Parse(Request.QueryString["fiscalYear"]);
            prjInfoData.VER_ID = long.Parse(Request.QueryString["verID"]);
            prjInfoData.STATUS_ID = Convert.ToInt64(uxStatus.Text);
            prjInfoData.ACRES = Convert.ToDecimal(uxAcres.Text);
            prjInfoData.DAYS = Convert.ToDecimal(uxDays.Text);
            if (uxAppType.Text != null) { prjInfoData.APP_TYPE = uxAppType.Text; }
            if (uxChemMix.Text != null) { prjInfoData.CHEMICAL_MIX = uxChemMix.Text; }
            if (uxComments.Text != null) { prjInfoData.COMMENTS = uxComments.Text; }
            if (uxLiabilityCheckbox.Checked != true) { prjInfoData.LIABILITY = "Y"; } else { prjInfoData.LIABILITY = "N"; }
            //if (liabilityOP != null) { prjInfoData.LIABILITY_OP = liabilityOP; }
            if (uxJCDate.Text != "") { prjInfoData.WE_DATE = Convert.ToDateTime(uxJCDate.Text); }

            if (uxHidNewProject.Text == "True")
            {
                GenericData.Insert<BUD_BID_PROJECTS>(prjInfoData);
            }

            else
            {
                GenericData.Update<BUD_BID_PROJECTS>(prjInfoData);
            }

            // Get created project id from BUD_BID_PROJECTS table
            decimal tableProjectID = prjInfoData.BUD_BID_PROJECTS_ID;

            // Write project start numbers to DB
            BUD_BID_ACTUAL_NUM startNumsdata = new BUD_BID_ACTUAL_NUM();
            long[] arr1 = { 6, 7, 8, 9, 10 };
            Ext.Net.TextField[] arr2 = { uxSGrossRec, uxSMatUsage, uxSGrossRev, uxSDirects, uxSOP };
            for (int i = 0; i <= 4; i++)
            {
                startNumsdata.PROJECT_ID = Convert.ToInt64(tableProjectID);
                startNumsdata.DETAIL_TASK_ID = 7;
                startNumsdata.LINE_ID = arr1[i];
                startNumsdata.NOV = Convert.ToDecimal(arr2[i].Text);
                if (uxHidNewProject.Text == "True")
                {
                    GenericData.Insert<BUD_BID_ACTUAL_NUM>(startNumsdata);
                }

                else
                {
                    GenericData.Update<BUD_BID_ACTUAL_NUM>(startNumsdata);
                }                
            }

            NotificationMsg("Save", "Project has been saved.", Icon.DiskBlack);
            uxProjectDetail.Disable();
            uxSummaryGridStore.Reload();
            uxProjectDetail.Reset();
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

        protected void deLoadStatuses(object sender, StoreReadDataEventArgs e)
        {
            uxStatusStore.DataSource = BUD_BID_STATUS.Statuses();
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

            else if (uxHidProjectID.Text == "")
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
            prjInfoData.BUD_BID_PROJECTS_ID = Convert.ToInt64(uxHidProjectID.Text);
            prjInfoData1.PROJECT_ID = Convert.ToInt64(uxHidProjectID.Text);

            GenericData.Delete<BUD_BID_PROJECTS>(prjInfoData);
            GenericData.Delete<BUD_BID_ACTUAL_NUM>(prjInfoData1);
            uxSummaryGridStore.Reload();        
        }
    }
}