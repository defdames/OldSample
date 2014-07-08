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

                CalcSummaryTotals();
            }
        }

        protected void deLoadSummaryActions(object sender, StoreReadDataEventArgs e)
        {
            uxActionsStore.DataSource = BUDGETBIDDING.YearBudgetSummaryProjectActions();
        }

        protected void deChooseSummaryAction(object sender, DirectEventArgs e)
        {
            string selectedAction = uxActions.Text;

            switch (selectedAction)
            {
                case "Add a New Project":
                    ActionAddNewProject();
                    break;

                case "Delete Selected Project":
                    ActionDeleteSelectedProject();                    
                    break;

                case "Edit Selected Project":
                    EditSelectedProject(false);
                    break;    
            }

            uxActions.Text = null;
        }

        protected void ActionAddNewProject()
        {
            if (uxHidNewProject.Text == "True") { return; }

            uxGridRowModel.ClearSelection();
            uxProjectDetail.Reset();            
            uxHidNewProject.Text = "True";
            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxProjectDetail.Enable();
            uxSave.Disable();
        }

        protected void ActionDeleteSelectedProject()
        {
            if (uxHidNewProject.Text == "True") { return; }

            if (uxHidBudBidID.Text == "")
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
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            BUD_BID_PROJECTS projectData;
            List<BUD_BID_ACTUAL_NUM> actualData;
            List<BUD_BID_BUDGET_NUM> budgetData;

            using (Entities _context = new Entities())
            {
                projectData = _context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
                actualData = _context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                budgetData = _context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
            }

            GenericData.Delete<BUD_BID_PROJECTS>(projectData);
            GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
            GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);

            uxProjectDetail.Disable();
            uxProjectDetail.Reset();
            uxHidNewProject.Text = "";
            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxSummaryGridStore.Reload();
            CalcSummaryTotals();
        }

        protected void deAllowFormEditing(object sender, DirectEventArgs e)
        {
            string budBidprojectID = e.ExtraParams["BudBidProjectID"];

            EditSelectedProject(true);
        }

        protected void EditSelectedProject(bool doubleClick)
        {
            if (uxHidNewProject.Text == "True") { return; }

            if (doubleClick == false && uxHidBudBidID.Text == "")
            {
                StandardMsgBox("Edit", "A project must be selected before it can be edited.", "INFO");
                return;
            }

            uxProjectDetail.Enable();
            uxSave.Enable();
        }

        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = Request.QueryString["orgName"];
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);
            uxSummaryGridStore.DataSource = BUDGETBIDDING.SummaryGridData(orgName, orgID, yearID, verID, prevYearID, prevVerID);
        }

        protected void deReadAdjustmentGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxAdjustmentGridStore.DataSource = BUDGETBIDDING.AdjustmentGridData(orgID, yearID, verID);
        }

        protected void deLoadOrgProjects(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            string orgName = Request.QueryString["orgName"];
            List<object> dataSource = BUDGETBIDDING.ProjectList(orgID, orgName).ToList<object>();
            int count;

            uxProjectNumStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }

        protected void deLoadStatuses(object sender, StoreReadDataEventArgs e)
        {
            uxStatusStore.DataSource = BUDGETBIDDING.Statuses();
        }

        protected void deLoadJCDates(object sender, StoreReadDataEventArgs e)
        {
            long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
            uxJCDateStore.DataSource = XXDBI_DW.LoadedJCWeDates(hierID, true, 5);
        }       

        protected void deGetFormData(object sender, DirectEventArgs e)
        {
            if (uxProjectDetail.Enabled == true) { uxProjectDetail.Disable(); }

            string budBidprojectID = e.ExtraParams["BudBidProjectID"];
            string projectNumID = e.ExtraParams["ProjectNumID"];
            string type = e.ExtraParams["Type"];
            string projectNum = e.ExtraParams["ProjectNum"];            
            string projectName = e.ExtraParams["ProjectName"];
            BUDGETBIDDING.BUD_SUMMARY_V data = BUDGETBIDDING.SummaryProjectsDetail(Convert.ToInt64(budBidprojectID));

            uxHidNewProject.Text = "";
            uxHidBudBidID.Text = budBidprojectID;
            uxHidProjectNumID.Text = projectNumID;
            uxHidType.Text = type;
            uxHidStatusID.Text = data.STATUS_ID.ToString();

            uxProjectNum.SetValue(projectNumID, projectNum);
            uxProjectName.Text = projectName;
            uxStatus.Text = data.STATUS;
            uxComments.Text = data.COMMENTS;
            uxCompareOverride.Checked = data.COMPARE_PRJ_OVERRIDE == "Y" ? true : false;
            uxCompareOP.Text = String.Format("{0:N2}", data.COMPARE_PRJ_AMOUNT);
            uxCompareVar.Text = "100";
            uxAcres.Text = String.Format("{0:N2}", data.ACRES);
            uxDays.Text = String.Format("{0:N2}", data.DAYS);
            uxLiabilityCheckbox.Checked = data.LIABILITY == "Y" ? true : false;
            uxLiabilityAmount.Text = String.Format("{0:N2}", data.LIABILITY_OP);
            uxAppType.Text = data.APP_TYPE;
            uxChemMix.Text = data.CHEMICAL_MIX;       
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
                uxHidProjectNumID.Text = DateTime.Now.ToString("yyMMddHHmmss");
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

        protected void deSelectStatus(object sender, DirectEventArgs e)
        {
            uxHidStatusID.Text = uxStatus.Value.ToString();            
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
                    jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64("0"), jcDate);
                    break;

                case "ORG":
                    jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64(hierID), Convert.ToInt64(projectID), jcDate);
                    break;

                case "PROJECT":
                    jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64(projectID), jcDate);
                    break;

                case "ROLLUP":
                    jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64(orgID), projectID, jcDate);
                    break;
            }

            uxSGrossRec.Text = String.Format("{0:N2}", jcLine.FY_GREC);
            uxSMatUsage.Text = String.Format("{0:N2}", jcLine.FY_MU);
            uxSGrossRev.Text = String.Format("{0:N2}", jcLine.FY_GREV);
            uxSDirects.Text = String.Format("{0:N2}", jcLine.FY_TDE);
            uxSOP.Text = String.Format("{0:N2}", jcLine.FY_TOP);

            //  FIX
            uxEGrossRec.Text = String.Format("{0:N2}", jcLine.FY_GREC);
            uxEMatUsage.Text = String.Format("{0:N2}", jcLine.FY_MU);
            uxEGrossRev.Text = String.Format("{0:N2}", jcLine.FY_GREV);
            uxEDirects.Text = String.Format("{0:N2}", jcLine.FY_TDE);
            uxEOP.Text = String.Format("{0:N2}", jcLine.FY_TOP);
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
            if (uxProjectName.Text == "" || uxHidStatusID.Text == "")
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
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long projectID = Convert.ToInt64(uxHidProjectNumID.Text);

            if (uxHidNewProject.Text == "True")
            {
                if (BUDGETBIDDING.ProjectExists(orgID, yearID, verID, projectID) == false)
                {
                    SaveInsertNewRecord();
                }

                else
                {
                    StandardMsgBox("Exists", "This project already exists.  Please select a different project or edit/delete the existing one.", "INFO");
                    return;
                }
            }

            else
            {
                SaveUpdateExistingRecord();
            }

            NotificationMsg("Save", "Project has been saved.", Icon.DiskBlack);
            //uxProjectFilter.ClearFilter();  //  FIX
            uxProjectDetail.Disable();
            uxSummaryGridStore.Reload();
            CalcSummaryTotals();
            uxHidNewProject.Text = "";
            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxProjectDetail.Reset();
        }

        protected void SaveInsertNewRecord()
        {
            BUD_BID_PROJECTS projectData = ProjectFormDetailData(true);

            GenericData.Insert<BUD_BID_PROJECTS>(projectData);            

            // Get created project id from BUD_BID_PROJECTS table
            long budBidID = Convert.ToInt64(projectData.BUD_BID_PROJECTS_ID);
            uxHidBudBidID.Text = budBidID.ToString();

            GenericData.Insert<BUD_BID_DETAIL_TASK>(ProjectDetailSheetData(budBidID));
            GenericData.Insert<BUD_BID_ACTUAL_NUM>(ProjectFormStartData(true, budBidID));
            GenericData.Insert<BUD_BID_BUDGET_NUM>(ProjectFormEndData(true, budBidID));
        }

        protected void SaveUpdateExistingRecord()
        {
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);
            BUD_BID_PROJECTS projectData = ProjectFormDetailData(false, budBidID);
            List<BUD_BID_ACTUAL_NUM> startData = ProjectFormStartData(false, budBidID);
            List<BUD_BID_BUDGET_NUM> endData = ProjectFormEndData(false, budBidID);

            GenericData.Update<BUD_BID_PROJECTS>(projectData);
            GenericData.Update<BUD_BID_ACTUAL_NUM>(startData);
            GenericData.Update<BUD_BID_BUDGET_NUM>(endData);          
        }

        protected BUD_BID_PROJECTS ProjectFormDetailData(bool insert, long budBidID = 0)
        {
            BUD_BID_PROJECTS data;

            if (insert == true)
            {
                data = new BUD_BID_PROJECTS();
            }

            else
            {
                using (Entities _context = new Entities())
                {
                    data = _context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
                }
            }

            data.PROJECT_ID = uxHidProjectNumID.Text;
            data.ORG_ID = Convert.ToInt64(Request.QueryString["OrgID"]);
            data.YEAR_ID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            data.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
            data.STATUS_ID = Convert.ToInt64(uxHidStatusID.Text);
            data.ACRES = Convert.ToDecimal(uxAcres.Text);
            data.DAYS = Convert.ToDecimal(uxDays.Text);
            data.OH_ID = 0;
            data.APP_TYPE = uxAppType.Text;
            data.CHEMICAL_MIX = uxChemMix.Text;
            data.COMMENTS = uxComments.Text;
            if (uxJCDate.Text != null)
            {
                if (uxJCDate.Text == "-- OVERRIDE --")
                {
                    data.PRJ_NAME = uxProjectName.Text;
                    data.WE_OVERRIDE = "Y";
                }
                else
                {
                    data.WE_OVERRIDE = "N";
                    if (uxJCDate.Text != "")
                    {
                        data.WE_DATE = Convert.ToDateTime(uxJCDate.Text);
                    }
                }
            }
            data.TYPE = uxHidType.Text;
            data.LIABILITY = uxLiabilityCheckbox.Checked == true ? "Y" : "N";
            data.LIABILITY_OP = Convert.ToDecimal(uxLiabilityAmount.Text);
            data.COMPARE_PRJ_OVERRIDE = uxCompareOverride.Checked == true ? "Y" : "N";
            data.COMPARE_PRJ_AMOUNT = Convert.ToDecimal(uxCompareOP.Text);

            return data;
        }

        protected BUD_BID_DETAIL_TASK ProjectDetailSheetData(long budBidID)
        {
            BUD_BID_DETAIL_TASK data = new BUD_BID_DETAIL_TASK();
            
            data.PROJECT_ID = Convert.ToInt64(uxHidBudBidID.Text);
            data.DETAIL_NAME = "SYS_PROJECT";
            data.SHEET_ORDER = 0;

            return data;
        }

        protected List<BUD_BID_ACTUAL_NUM> ProjectFormStartData(bool insert, long budBidID)
        {
            List<BUD_BID_ACTUAL_NUM> data;

            if (insert == true)
            {
                data = new List<BUD_BID_ACTUAL_NUM>();
            }

            else
            {
                using (Entities _context = new Entities())
                {
                    data = _context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                }
            }

            long[] arrLineNum = { 6, 7, 8, 9, 10 };
            Ext.Net.TextField[] arrControl = { uxSGrossRec, uxSMatUsage, uxSGrossRev, uxSDirects, uxSOP };

            if (insert == true)
            {
                for (int i = 0; i <= 4; i++)
                {
                    data.Add(new BUD_BID_ACTUAL_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = 9999,
                        LINE_ID = arrLineNum[i],
                        NOV = Convert.ToDecimal(arrControl[i].Text)
                    });
                }
            }

            else
            {
                int a = 0;
                foreach (BUD_BID_ACTUAL_NUM field in data)
                {
                    field.NOV = Convert.ToDecimal(arrControl[a].Text);
                    a++;
                }
            }

            return data;
        }  //FIX DETAIL ID

        protected List<BUD_BID_BUDGET_NUM> ProjectFormEndData(bool insert, long budBidID)
        {
            List<BUD_BID_BUDGET_NUM> data;

            if (insert == true)
            {
                data = new List<BUD_BID_BUDGET_NUM>();
            }

            else
            {
                using (Entities _context = new Entities())
                {
                    data = _context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                }
            }

            long[] arrLineNum = { 6, 7, 8, 9, 10 };
            Ext.Net.TextField[] arrControl = { uxEGrossRec, uxEMatUsage, uxEGrossRev, uxEDirects, uxEOP };

            if (insert == true)
            {
                for (int i = 0; i <= 4; i++)
                {
                    data.Add(new BUD_BID_BUDGET_NUM
                        {
                            PROJECT_ID = budBidID,
                            DETAIL_TASK_ID = 9999,
                            LINE_ID = arrLineNum[i],
                            NOV = Convert.ToDecimal(arrControl[i].Text)
                        });
                }
            }

            else
            {
                int a = 0;
                foreach (BUD_BID_BUDGET_NUM field in data)
                {
                    field.NOV = Convert.ToDecimal(arrControl[a].Text);
                    a++;
                }
            }

            return data;
        }     //FIX DETAIL ID

        protected void deCancel(object sender, DirectEventArgs e)
        {            
            uxProjectDetail.Disable();           
            uxProjectDetail.Reset();
            uxGridRowModel.ClearSelection();
            uxHidNewProject.Text = "";
            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";

            //uxProjectFilter.ClearFilter();  // FIX
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

        protected void CalcSummaryTotals()
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);


            BUDGETBIDDING.BUD_SUMMARY_V activeData = BUDGETBIDDING.SummaryGridSubtotals(true, orgID, yearID, verID, prevYearID, prevVerID);
            decimal aGrossRec = activeData.GROSS_REC;
            decimal aMatUsage = activeData.MAT_USAGE;
            decimal aGrossRev = activeData.GROSS_REV;
            decimal aDirects = activeData.DIR_EXP;
            decimal aOP = activeData.OP;
            decimal aOPPerc = activeData.OP_PERC;
            decimal aOPPlusMinus = activeData.OP_VAR;

            uxAGrossRec.Text = String.Format("{0:N2}", aGrossRec);
            uxAMatUsage.Text = String.Format("{0:N2}", aMatUsage);
            uxAGrossRev.Text = String.Format("{0:N2}", aGrossRev);
            uxADirects.Text = String.Format("{0:N2}", aDirects);
            uxAOP.Text = String.Format("{0:N2}", aOP);
            uxAOPPerc.Text = String.Format("{0:P2}", aOPPerc);  //  FIX
            uxAOPPlusMinus.Text = String.Format("{0:N2}", aOPPlusMinus);

            BUDGETBIDDING.BUD_SUMMARY_V inactiveData = BUDGETBIDDING.SummaryGridSubtotals(false, orgID, yearID, verID, prevYearID, prevVerID);
            decimal iGrossRec = inactiveData.GROSS_REC;
            decimal iMatUsage = inactiveData.MAT_USAGE;
            decimal iGrossRev = inactiveData.GROSS_REV;
            decimal iDirects = inactiveData.DIR_EXP;
            decimal iOP = inactiveData.OP;
            decimal iOPPerc = inactiveData.OP_PERC;
            decimal iOPPlusMinus = inactiveData.OP_VAR;

            uxIGrossRec.Text = String.Format("{0:N2}", iGrossRec);
            uxIMatUsage.Text = String.Format("{0:N2}", iMatUsage);
            uxIGrossRev.Text = String.Format("{0:N2}", iGrossRev);
            uxIDirects.Text = String.Format("{0:N2}", iDirects);
            uxIOP.Text = String.Format("{0:N2}", iOP);
            uxIOPPerc.Text = String.Format("{0:P2}", iOPPerc);   //  FIX
            uxIOPPlusMinus.Text = String.Format("{0:N2}", iOPPlusMinus);

            //BUDGETBIDDING.BUD_SUMMARY_V adjustmentData = BUDGETBIDDING.AdjustmentGridSubtotals(orgID, yearID, verID);
            //decimal adjGrossRec = 0;
            //decimal adjMatUsage = adjustmentData.MAT_ADJ ?? 0;
            //decimal adjGrossRev = adjGrossRec - adjMatUsage;
            //decimal adjDirects = adjustmentData.DIRECT_ADJ ?? 0;
            //decimal adjOP = adjGrossRev - adjDirects;
            //decimal adjOPPerc = 0;   //  FIX
            //decimal adjOPPlusMinus = 0;   //  FIX

            decimal tGrossRec = aGrossRec + iGrossRec; //+ adjGrossRec;
            decimal tMatUsage = aMatUsage + iMatUsage; //+ adjMatUsage;
            decimal tGrossRev = aGrossRev + iGrossRev; //+ adjGrossRev;
            decimal tDirects = aDirects + iDirects; //+ adjDirects;
            decimal tOP = aOP + iOP; //+ adjOP;
            decimal tOPPerc = aOPPerc + iOPPerc; //+ adjOPPerc;
            decimal tOPPlusMinus = aOPPlusMinus + iOPPlusMinus; //+ adjOPPlusMinus;

            uxTGrossRec.Text = String.Format("{0:N2}", tGrossRec);
            uxTMatUsage.Text = String.Format("{0:N2}", tMatUsage);
            uxTGrossRev.Text = String.Format("{0:N2}", tGrossRev);
            uxTDirects.Text = String.Format("{0:N2}", tDirects);
            uxTOP.Text = String.Format("{0:N2}", tOP);
            uxTOPPerc.Text = String.Format("{0:P2}", tOPPerc);   //  FIX
            uxTOPPlusMinus.Text = String.Format("{0:N2}", tOPPlusMinus);          
        }

        //[DirectMethod(Namespace = "SaveRecord")]
        //public void deSaveAdjustments(long id, string field, decimal newValue)
        //{
        //    BUD_BID_ADJUSTMENT data;

        //    using (Entities _context = new Entities())
        //    {
        //        data = _context.BUD_BID_ADJUSTMENT.Where(x => x.ADJ_ID == id).Single();
        //    }

        //    if (field == "WEATHER_ADJ")
        //    {
        //        data.WEATHER_ADJ = newValue;
        //    }

        //    else if (field == "MAT_ADJ")
        //    {
        //        data.MAT_ADJ = newValue;
        //    }
            

        //    GenericData.Update<BUD_BID_ADJUSTMENT>(data);
        //    uxAdjustmentGridStore.CommitChanges();
        //    StandardMsgBox("Updated", "The adjustment has been updated.", "INFO");
        //}
    }
}