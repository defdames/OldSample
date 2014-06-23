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

        }

        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = Request.QueryString["orgName"];
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            CalcPrevYearAndVersion(yearID, verID);
            long prevYearID = Convert.ToInt64(uxHidPrevYear.Value);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Value);
            uxSummaryGridStore.DataSource = BUD_BID_STATUS.OrgSummaryProjects(orgName, orgID, yearID, verID, prevYearID, prevVerID);
        }       

        protected void deGetFormData(object sender, DirectEventArgs e)
        {
            uxProjectNum.SetValue("2001517");//e.ExtraParams["ProjectId"]);
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
                uxGridRowModel.ClearSelection();
                uxProjectDetail.Reset();
                uxProjectDetail.Enable();
                uxHidNewProject.Value = "True";
            }
        }
        
        protected void deCancel(object sender, DirectEventArgs e)
        {
            uxProjectDetail.Reset();
            uxProjectDetail.Disable();
        }

        protected void deSelectProject(object sender, DirectEventArgs e)
        {
            string type = e.ExtraParams["Type"];
            uxHidProjectID.Value = e.ExtraParams["ProjectID"];
            uxHidProjectNum.Value = e.ExtraParams["ProjectNum"];
            uxHidProjectName.Value = e.ExtraParams["ProjectName"];
            uxHidType.Value = type;
            LoadJCNumbers();
        }

        protected void deSelectJCDate(object sender, DirectEventArgs e)
        {
            uxHidDate.Value = uxJCDate.Value;
            LoadJCNumbers();
        }

        protected void LoadJCNumbers()
        {
            string hierID = Request.QueryString["hierID"];
            string orgID = Request.QueryString["orgID"];
            string projectID = uxHidProjectID.Value == null ? "" : uxHidProjectID.Value.ToString();
            string projectNum = uxHidProjectNum.Value == null ? "" : uxHidProjectNum.Value.ToString();
            string projectName = uxHidProjectName.Value == null ? "" : uxHidProjectName.Value.ToString();
            string type = uxHidType.Value == null ? "" : uxHidType.Value.ToString();
            string jcDate = uxHidDate.Value == null ? "" : uxHidDate.Value.ToString();
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


            uxSGrossRec.Value = String.Format("{0:N2}", jcLine.FY_GREC);
            uxSMatUsage.Value = String.Format("{0:N2}", jcLine.FY_MU);
            uxSGrossRev.Value = String.Format("{0:N2}", jcLine.FY_GREV);
            uxSDirects.Value = String.Format("{0:N2}", jcLine.FY_TDE);
            uxSOP.Value = String.Format("{0:N2}", jcLine.FY_TOP);
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
        
        protected void deLoadOrgProjects(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            string orgName = Request.QueryString["orgName"];

            using (Entities context = new Entities())
            {
                string sql = string.Format(@"SELECT TO_CHAR(sysdate, 'YYMMDDHH24MISS') as PROJECT_ID, 'N/A' as PROJECT_NUM, '-- OVERRIDE --' as PROJECT_NAME, 'OVERRIDE' as TYPE, 'ID1' as ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT '{1}' as PROJECT_ID, 'N/A' as PROJECT_NUM, '{0} (Org)' as PROJECT_NAME, 'ORG' as TYPE, 'ID2' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT CAST(PROJECTS_V.PROJECT_ID as varchar(20)) as PROJECT_ID, PROJECTS_V.SEGMENT1 as PROJECT_NUM, PROJECTS_V.LONG_NAME as PROJECT_NAME, 'PROJECT' as TYPE, 'ID3' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN pa.pa_project_classes
                    ON PROJECTS_V.PROJECT_ID = pa.pa_project_classes.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' and PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || chr(38) || ' EQUIPMENT' and pa.pa_project_classes.CLASS_CATEGORY = 'Job Cost Rollup'
                    and PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                        UNION ALL
                    SELECT CONCAT('Various - ', pa.pa_project_classes.CLASS_CODE) as PROJECT_ID, 'N/A' as PROJECT_NUM, CONCAT('Various - ', pa.pa_project_classes.CLASS_CODE) as PROJECT_NAME, 'ROLLUP' as TYPE, 'ID4' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN pa.pa_project_classes
                    ON PROJECTS_V.PROJECT_ID = pa.pa_project_classes.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' and PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || chr(38) || ' EQUIPMENT' and pa.pa_project_classes.CLASS_CATEGORY = 'Job Cost Rollup'
                    and pa.pa_project_classes.CLASS_CODE <> 'None' and PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                    GROUP BY  CONCAT('Various - ', pa.pa_project_classes.CLASS_CODE) 
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
                uxJCDate.Value = null;
                uxHidDate.Value = null;
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

        protected void deLoadCompareToProjects(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            long fiscalYear = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            using (Entities context = new Entities())
            {
                string sql = string.Format("SELECT OVERRIDE_PROJ_NAME FROM BUD_BID_PROJECTS WHERE ORG_ID = {0} AND YEAR_ID = {1} AND VER_ID = {2} ORDER BY OVERRIDE_PROJ_NAME", orgID, fiscalYear, verID);
                List<object> dataSource;
                dataSource = context.Database.SqlQuery<ORG_PROJECTS>(sql).ToList<object>();
                int count;
                uxProjectCompareStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
                e.Total = count;
            }
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
            prjInfoData.PROJECT_ID = Convert.ToInt64(uxHidProjectID.Value);
            //prjInfoData.PROJ_NUM = uxProjectNum.Value.ToString();
            prjInfoData.PRJ_NAME = uxProjectName.Value.ToString();
            prjInfoData.TYPE = uxHidType.Value.ToString();
            prjInfoData.ORG_ID = long.Parse(Request.QueryString["OrgID"]);
            prjInfoData.YEAR_ID = long.Parse(Request.QueryString["fiscalYear"]);
            prjInfoData.VER_ID = long.Parse(Request.QueryString["verID"]);
            prjInfoData.STATUS_ID = Convert.ToInt64(uxStatus.Value);
            prjInfoData.ACRES = Convert.ToDecimal(uxAcres.Value);
            prjInfoData.DAYS = Convert.ToDecimal(uxDays.Value);
            if (uxAppType.Value != null) { prjInfoData.APP_TYPE = uxAppType.Value.ToString(); }
            if (uxChemMix.Value != null) { prjInfoData.CHEMICAL_MIX = uxChemMix.Value.ToString(); }
            if (uxComments.Value != null) { prjInfoData.COMMENTS = uxComments.Value.ToString(); }
            if (uxLiabilityCheckbox.Checked != true) { prjInfoData.LIABILITY = "Y"; } else { prjInfoData.LIABILITY = "N"; }
            //if (liabilityOP != null) { prjInfoData.LIABILITY_OP = liabilityOP; }
            if (uxJCDate.Value != "") { prjInfoData.WE_DATE = Convert.ToDateTime(uxJCDate.Value); }

            if (uxHidNewProject.Value.ToString() == "True")
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
                startNumsdata.NOV = Convert.ToDecimal(arr2[i].Value);
                if (uxHidNewProject.Value.ToString() == "True")
                {
                    GenericData.Insert<BUD_BID_ACTUAL_NUM>(startNumsdata);
                }

                else
                {
                    GenericData.Update<BUD_BID_ACTUAL_NUM>(startNumsdata);
                }                
            }

            //BUD_BID_BUDGET_NUM startNumsdata = new BUD_BID_BUDGET_NUM();
            //startNumsdata.PROJECT_ID = 1;
            //startNumsdata.DETAIL_TASK_ID = 7;
            //startNumsdata.LINE_ID = 6;
            //startNumsdata.NOV = 4;
            //GenericData.Insert<BUD_BID_BUDGET_NUM>(startNumsdata);

            NotificationMsg("Save", "Project has been saved.", Icon.DiskBlack);
            uxProjectDetail.Disable();
        }

        protected void deCheckAllowSave(object sender, DirectEventArgs e)
        {
            if (uxProjectName.Value == null || String.IsNullOrWhiteSpace(uxProjectName.Value.ToString()) || uxStatus.SelectedItem.Value == null)
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

        protected void CalcPrevYearAndVersion(long curYear, long curVer)
        {
            switch (curVer)
            {
                case 1:  // Bid
                    uxHidPrevYear.Value = curYear;
                    uxHidPrevVer.Value = 1;
                    break;

                case 2:  // First Draft
                    uxHidPrevYear.Value = curYear;
                    uxHidPrevVer.Value = 1;
                    break;

                case 3:  // Final Draft
                    uxHidPrevYear.Value = curYear - 1;
                    uxHidPrevVer.Value = 3;
                    break;

                case 4:  // 1st Reforecast
                    uxHidPrevYear.Value = curYear;
                    uxHidPrevVer.Value = 3;
                    break;

                case 5:  // 2nd Reforecast
                    uxHidPrevYear.Value = curYear;
                    uxHidPrevVer.Value = 4;
                    break;

                case 6:  // 3rd Reforecast
                    uxHidPrevYear.Value = curYear;
                    uxHidPrevVer.Value = 5;
                    break;

                case 7:  // 4th Reforecast
                    uxHidPrevYear.Value = curYear;
                    uxHidPrevVer.Value = 6;
                    break;
            }

        }
    }
}