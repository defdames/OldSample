using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Security;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umYearBudget : System.Web.UI.Page
    {                        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                long orgID = long.Parse(Request.QueryString["OrgID"]);
                long yearID = long.Parse(Request.QueryString["fiscalYear"]);
                long verID = long.Parse(Request.QueryString["verID"]);

                if (BB.IsReadOnly(orgID, yearID, verID) == true)
                {
                    uxUpdateAllActuals.Disable();
                    StandardMsgBox("Read Only", "This budget is currently read only.  No changes can be made.", "INFO");
                }
                else
                {
                    uxUpdateAllActuals.Enable();
                }

                uxHidPrevYear.Text = BB.CalcPrevYear(yearID, verID).ToString();
                uxHidPrevVer.Text = BB.CalcPrevVer(yearID, verID).ToString();
                uxVersionLabel.Text = BB.GetPrevVerName(verID) + " OP:";

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
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            string orgName = Request.QueryString["orgName"];
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);

            uxSummaryGridStore.DataSource = BBSummary.Grid.Data(orgName, orgID, yearID, verID, prevYearID, prevVerID);
        }                         
        protected void deReadAdjustmentGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxAdjustmentGridStore.DataSource = BBAdjustments.Data(orgID, yearID, verID);
        }              
        protected void deReadOverheadGridData(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxOverheadGridStore.DataSource = BBOH.Data(orgID, yearID, verID);
        }


        
        // Summary Actions                          
        protected void deLoadSummaryActions(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxActionsStore.DataSource = BB.YearSummaryProjectActions(orgID, yearID, verID);
        }
        protected void deChooseSummaryAction(object sender, DirectEventArgs e)
        {
            string selectedAction = uxActions.Text;

            switch (selectedAction)
            {
                case "Add a New Project":
                    AddNewProject();
                    break;

                case "View Selected Project":
                    EditSelectedProject();
                    break;

                case "Edit Selected Project":
                    EditSelectedProject();
                    break;

                case "Copy Selected Project":
                    CopySelectedProject();
                    break;

                case "Delete Selected Project":
                    DeleteSelectedProject();
                    break;

                case "Refresh Data":
                    RefreshData();
                    break;
            }

            uxActions.Clear();            
        }
        protected void AddNewProject()
        {
            LockTopSection();
            ResetProjectForm();         

            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxHidFormEnabled.Text = "True";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            SaveInsertNewRecord();

            uxProjectInfo.Enable();
        }                                    
        protected void EditSelectedProject()
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            if (uxHidBudBidID.Text == "")
            {
                if (BB.IsReadOnly(orgID, yearID, verID) == true)
                {
                    StandardMsgBox("View", "A project must be selected before it can be viewed.", "INFO");
                    return;
                }
                else
                {
                    StandardMsgBox("Edit", "A project must be selected before it can be edited.", "INFO");
                    return;
                }
            }

            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);            

            if (BB.ProjectStillExists(budBidID) == false)
            {
                StandardMsgBox("Edit", "Project has been deleted or has changed.  Please refresh summary", "INFO");
                return;
            }

            LockTopSection();

            long newBudBidID = BB.CopyAllProjectDataAsTemp(budBidID);

            uxHidBudBidID.Text = newBudBidID.ToString();
            uxHidOldBudBidID.Text = budBidID.ToString();
            uxHidFormEnabled.Text = "True";

            uxSummaryDetailStore.Reload();  

            uxProjectInfo.Enable();

            if (BB.IsReadOnly(orgID, yearID, verID) == true)
            {
                LockProjectInfo();
            }
            else
            {
                if (uxJCDate.Text != "" && uxJCDate.Text != "-- OVERRIDE --")
                {
                    X.MessageBox.Confirm("Actuals", "Would you like to refresh the job cost start numbers in case they have changed?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig { Handler = "App.direct.EditSelectedProjectContinued()", Text = "Yes" },
                        No = new MessageBoxButtonConfig { Text = "No" }
                    }).Show();
                }
            }
        }
        [DirectMethod]
        public void EditSelectedProjectContinued()
        {
            LoadJCNumbers();
            UpdateCompareNums(uxCompareOverride.Checked);

            StandardMsgBox("Actuals", "Job cost numbers have been refreshed.", "INFO");
        }
        protected void CopySelectedProject()
        {
            if (uxHidBudBidID.Text == "")
            {
                StandardMsgBox("Copy", "A project must be selected before it can be copied.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Copy", "Are you sure you want to copy the selected project?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.CopySelectedProjectContiued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }
        [DirectMethod]
        public void CopySelectedProjectContiued()
        {
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);
            string origProjectName = uxProjectName.Text;
            long test = BBProject.DBCopy(budBidID, origProjectName);

            uxProjectInfo.Disable();
            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxCompareVar.Text = "0.00";

            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxHidFormEnabled.Text = "";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            uxSummaryGridStore.Reload();
            UnlockTopSection();
            CalcSummaryTotals();
        }
        protected void DeleteSelectedProject()
        {
            if (uxHidBudBidID.Text == "")
            {
                StandardMsgBox("Delete", "A project must be selected before it can be deleted.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Delete", "Are you sure you want to delete the selected project? Once it's been deleted it cannot be retrieved.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteSelectedProjectContiued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }                                                    
        [DirectMethod]
        public void DeleteSelectedProjectContiued()
        {
            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            BBProject.DBDelete(budBidID);

            uxProjectInfo.Disable();
            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxCompareVar.Text = "0.00";

            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxHidFormEnabled.Text = "";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            uxSummaryGridStore.Reload();
            UnlockTopSection();
            CalcSummaryTotals();
        }
        protected void RefreshData()
        {
            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxCompareVar.Text = "0.00";

            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxHidFormEnabled.Text = "";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            uxSummaryGridStore.Reload();
            UnlockTopSection();
            CalcSummaryTotals();
        }
        protected void deUpdateAllActuals(object sender, DirectEventArgs e)
        {
            string hierID = Request.QueryString["hierID"];
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            long yearID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);

            if (BB.CountAllProjects(orgID, yearID, verID) == 0)
            {
                StandardMsgBox("Update All Projects", "There are no projects to update.", "INFO");
                return;
            }
            
            string url = "/Views/Modules/BudgetBidding/umUpdateAllActuals.aspx?hierID=" + hierID + "&orgID=" + orgID + "&yearID=" + yearID + "&verName=" + verName;

            Window win = new Window
            {
                ID = "uxUpdateAllActualsForm",
                Height = 210,
                Width = 400,
                Title = "Update All Actuals",
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
        public void CloseUpdateAllActualsWindow()
        {       
            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxCompareVar.Text = "0.00";

            uxSummaryGridStore.Reload();

            CalcSummaryTotals();
        }


        // Reports
        protected void deLoadReports(object sender, StoreReadDataEventArgs e)
        {

                long orgID = long.Parse(Request.QueryString["OrgID"]);
                string strOrgID = Request.QueryString["OrgID"];
                string orgName = Request.QueryString["orgName"];
                long yearID = long.Parse(Request.QueryString["fiscalYear"]);
                string strYearID = Request.QueryString["fiscalYear"];
                long verID = long.Parse(Request.QueryString["verID"]);
                string strVerID = Request.QueryString["verID"];

                long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
                string strPrevYearID = uxHidPrevYear.Text;
                long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);
                string strPrevVerID = uxHidPrevVer.Text;

                string url = "/Views/Modules/BudgetBidding/Reports/umReport1.aspx?orgName=" + orgName + "strOrgID=" + strOrgID + "strYearID=" + strYearID + "strVerID=" + strVerID + "strPrevYearID=" + strPrevYearID + "strPrevVerID=" + strPrevVerID;

                Window win = new Window
                {
                    ID = "uxReport",
                    Title = "Report",
                    Height = 350,
                    Width = 500,
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
                //win.Listeners.Close.Handler = "#{uxPayrollAuditGrid}.getStore().load();";

                win.Render(this.Form);
                win.Show();
        
            
           
        }
        protected void deChooseReport(object sender, DirectEventArgs e)
        {

        }



        // Load Project Info                        
        protected void deLoadProjectDropdown(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            string orgName = Request.QueryString["orgName"];
            List<object> dataSource = BBProject.Listing.Data(orgID, orgName).ToList<object>();
            int count;

            uxProjectNumStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }
        protected void deLoadStatusDropdown(object sender, StoreReadDataEventArgs e)
        {
            uxStatusStore.DataSource = BBProject.Statuses();
        }
        protected void deLoadJCDateDropdown(object sender, StoreReadDataEventArgs e)
        {
            long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
            uxJCDateStore.DataSource = XXDBI_DW.LoadedJCWeDates(hierID, true, 5);
        }
        protected void deGetFormData(object sender, DirectEventArgs e)
        {
            string budBidprojectID = e.ExtraParams["BudBidProjectID"];
            string projectNumID = e.ExtraParams["ProjectNumID"];
            string type = e.ExtraParams["Type"];
            string projectNum = e.ExtraParams["ProjectNum"];
            string projectName = e.ExtraParams["ProjectName"];

            if (BB.ProjectStillExists(Convert.ToInt64(budBidprojectID)) == false)
            {
                StandardMsgBox("Select", "Project has been deleted or has changed.  Please refresh summary", "INFO");
                return;
            }

            BBProject.Detail.Fields data = BBProject.Detail.Data(Convert.ToInt64(budBidprojectID));
            string weOverride = data.WE_OVERRIDE;

            // Enable or disable fields based on loaded field values
            if (projectNum == "-- OVERRIDE --")
            {
                uxProjectName.ReadOnly = false;
                uxJCDate.Disable();
                uxSGrossRec.ReadOnly = false;
                uxSMatUsage.ReadOnly = false;
                uxSDirects.ReadOnly = false;
            }
            else
            {
                uxProjectName.ReadOnly = true;
                uxJCDate.Enable();
                if (weOverride == "Y")
                {
                    uxSGrossRec.ReadOnly = false;
                    uxSMatUsage.ReadOnly = false;
                    uxSDirects.ReadOnly = false;
                }
                else
                {
                    uxSGrossRec.ReadOnly = true;
                    uxSMatUsage.ReadOnly = true;
                    uxSDirects.ReadOnly = true;
                }
            }

            uxHidBudBidID.Text = budBidprojectID;
            uxHidProjectNumID.Text = projectNumID;
            uxHidType.Text = type;
            uxHidStatusID.Text = data.STATUS_ID.ToString();
            uxHidFormEnabled.Text = "";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            uxProjectNum.SetValue(projectNumID, projectNum);
            uxProjectName.Text = projectName;
            uxStatus.Text = data.STATUS;
            uxComments.Text = data.COMMENTS;
            uxAcres.Text = String.Format("{0:N2}", data.ACRES);
            uxDays.Text = String.Format("{0:N2}", data.DAYS);
            uxLiabilityCheckbox.Checked = data.LIABILITY == "Y" ? true : false;
            uxLiabilityAmount.Text = String.Format("{0:N2}", data.LIABILITY_OP);
            uxAppType.Text = data.APP_TYPE;
            uxChemMix.Text = data.CHEMICAL_MIX;
            if (weOverride == "Y")
            {
                uxJCDate.Text = "-- OVERRIDE --";
            }
            else
            {
                uxJCDate.Text = data.WE_DATE == null ? null : data.WE_DATE.Value.ToString("dd-MMM-yyyy");
            }

            BBProject.StartNumbers.Fields dataStart = BBProject.StartNumbers.Data(Convert.ToInt64(budBidprojectID));
            uxSGrossRec.Text = String.Format("{0:N2}", dataStart.GROSS_REC);
            uxSMatUsage.Text = String.Format("{0:N2}", dataStart.MAT_USAGE);
            uxSGrossRev.Text = String.Format("{0:N2}", dataStart.GROSS_REV);
            uxSDirects.Text = String.Format("{0:N2}", dataStart.DIR_EXP);
            uxSOP.Text = String.Format("{0:N2}", dataStart.OP);

            uxSummaryDetailStore.Reload();

            BBProject.EndNumbers.Fields dataEnd = BBProject.EndNumbers.Data(Convert.ToInt64(budBidprojectID));
            uxEGrossRec.Text = String.Format("{0:N2}", dataEnd.GROSS_REC);
            uxEMatUsage.Text = String.Format("{0:N2}", dataEnd.MAT_USAGE);
            uxEGrossRev.Text = String.Format("{0:N2}", dataEnd.GROSS_REV);
            uxEDirects.Text = String.Format("{0:N2}", dataEnd.DIR_EXP);
            uxEOP.Text = String.Format("{0:N2}", dataEnd.OP);
            
            if (data.COMPARE_PRJ_OVERRIDE == "Y")
            {
                uxCompareOverride.Checked = true;
                UpdateCompareNums(true);
            }
            else
            {
                uxCompareOverride.Checked = false;
                UpdateCompareNums(false);
            }
        }
        protected void deUpdateCompareNums(object sender, DirectEventArgs e)
        {
            UpdateCompareNums(uxCompareOverride.Checked, true);
        }
        protected void UpdateCompareNums(bool overriden, bool manuallyEdited = false)
        {
            decimal curOP = ForceToDecimal(uxEOP.Text);
            decimal prevOP;

            if (overriden == true)
            {
                if (manuallyEdited == false)
                {
                    long budBidprojectID = Convert.ToInt64(uxHidBudBidID.Text);
                    BBProject.OP.Fields dataOverridenOP = BBProject.OP.OverridenOP(budBidprojectID);
                    prevOP = dataOverridenOP.OP ?? 0;
                }
                else
                {
                    prevOP = ForceToDecimal(uxCompareOP.Text);
                }
            }
            else
            {
                if (uxHidProjectNumID.Text == "")
                {
                    prevOP = 0;
                }
                else
                {
                    long orgID = long.Parse(Request.QueryString["orgID"]);
                    long prevYear = Convert.ToInt64(uxHidPrevYear.Text);
                    long prevVer = Convert.ToInt64(uxHidPrevVer.Text);
                    string projectNumID = uxHidProjectNumID.Text;

                    BBProject.OP.Fields dataPrevOP = BBProject.OP.Data(orgID, prevYear, prevVer, projectNumID);
                    prevOP = dataPrevOP.OP ?? 0;
                }
            }

            uxCompareOP.Text = String.Format("{0:N2}", prevOP);
            uxCompareVar.Text = String.Format("{0:N2}", (curOP - prevOP));
        }        
        protected void deReadDetailGridData(object sender, StoreReadDataEventArgs e)
        {
            if (uxHidBudBidID.Text == "") { return; }

            long budBidID = Convert.ToInt64(uxHidBudBidID.Text);

            uxSummaryDetailStore.DataSource = BBDetail.MainGrid.Data(budBidID);
        }



        // Project Form Other                       
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
                uxSDirects.ReadOnly = false;
                uxHidProjectNumID.Text = DateTime.Now.ToString("yyMMddHHmmss");
            }
            else
            {
                if (uxJCDate.Text == "-- OVERRIDE --") { uxJCDate.Text = ""; }
                uxJCDate.Enable();
                uxProjectName.ReadOnly = true;
                uxSGrossRec.ReadOnly = true;
                uxSMatUsage.ReadOnly = true;
                uxSDirects.ReadOnly = true;
            }

            LoadJCNumbers();
            UpdateCompareNums(uxCompareOverride.Checked);
        }
        protected void deSelectStatus(object sender, DirectEventArgs e)
        {
            long statusID = Convert.ToInt64(uxStatus.Value);
            uxHidStatusID.Text = statusID.ToString();

            if (statusID == 45)
            {
                X.MessageBox.Confirm("Missing", "Would you like to clear out the project's starting numbers, detail sheets and ending numbers?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig { Handler = "App.direct.ClearMissingStatusNums()", Text = "Yes" },
                    No = new MessageBoxButtonConfig { Text = "No" }
                }).Show();
            }           
        }    
        [DirectMethod]
        public void ClearMissingStatusNums()
        {
            long budBidProjectID = Convert.ToInt64(uxHidBudBidID.Text);

            uxJCDate.Clear();
            BBProject.WEDate.Update(budBidProjectID, "");
            // update we date in db to null

            BBProject.StartNumbers.DBUpdate(budBidProjectID, 0, 0, 0, 0, 0);
            uxSGrossRec.Text = "0.00";
            uxSMatUsage.Text = "0.00";
            uxSGrossRev.Text = "0.00";
            uxSDirects.Text = "0.00";
            uxSOP.Text = "0.00";

            BBDetail.Sheets.DBDelete(budBidProjectID, false);
            uxSummaryDetailStore.Reload();

            BBProject.EndNumbersW0.DBUpdate(budBidProjectID, 0, 0, 0, 0, 0);
            uxEGrossRec.Text = "0.00";
            uxEMatUsage.Text = "0.00";
            uxEGrossRev.Text = "0.00";
            uxEDirects.Text = "0.00";
            uxEOP.Text = "0.00";
        }
        protected void deSelectJCDate(object sender, DirectEventArgs e)
        {
            if (uxJCDate.Text == "-- OVERRIDE --")
            {
                uxSGrossRec.ReadOnly = false;
                uxSMatUsage.ReadOnly = false;
                uxSDirects.ReadOnly = false;
            }
            else
            {
                uxSGrossRec.ReadOnly = true;
                uxSMatUsage.ReadOnly = true;
                uxSDirects.ReadOnly = true;

                LoadJCNumbers();
            }

            UpdateCompareNums(uxCompareOverride.Checked);
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

            PopulateProjectEndNumbers();
            uxSummaryDetailStore.Reload();  
        }
        protected void deCompareCheck(object sender, DirectEventArgs e)
        {
            if (uxCompareOverride.Checked == true)
            {
                if (uxHidFormEnabled.Text == "True") 
                {
                    UpdateCompareNums(true);
                }                
                uxCompareOP.Enable();                
            }
            else
            {
                uxCompareOP.Disable();
                if (uxHidFormEnabled.Text == "True")
                {
                    UpdateCompareNums(false);
                }                         
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
            char[] charsToTrim = { ' ', '\t' };
            string projectName = uxProjectName.Text.Trim(charsToTrim);

            if (projectName == "" || uxHidStatusID.Text == "")
            {
                uxSave.Disable();
            }
            else
            {
                uxSave.Enable();
            }
        }



        // Save Project       
        protected void deSave(object sender, DirectEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            string projectID = uxHidProjectNumID.Text;
            long curBudBidID = uxHidOldBudBidID.Text == "" ? 0 : Convert.ToInt64(uxHidOldBudBidID.Text);

            if (BBProject.Count(orgID, yearID, verID, projectID, curBudBidID) == 0)
            {
                if (curBudBidID != 0)
                {
                    BBProject.DBDelete(Convert.ToInt64(uxHidOldBudBidID.Text));
                }
                SaveUpdateExistingRecord();
            }
            else
            {
                StandardMsgBox("Exists", "This project already exists.  Please select a different project or edit/delete the existing one.", "INFO");
                return;
            }

            uxProjectInfo.Disable();            
            
            uxSummaryGridStore.Reload();
            UnlockTopSection();

            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxHidFormEnabled.Text = "";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxCompareVar.Text = "0.00";

            CalcSummaryTotals();
        }
        protected void SaveInsertNewRecord()
        {
            BUD_BID_PROJECTS projectData = ProjectFormDetailData(true);
            GenericData.Insert<BUD_BID_PROJECTS>(projectData);

            // Get newly created project id from BUD_BID_PROJECTS table
            long budBidID = Convert.ToInt64(projectData.BUD_BID_PROJECTS_ID);
            uxHidBudBidID.Text = budBidID.ToString();

            BUD_BID_DETAIL_TASK taskData = CreateProjectLevelDetailSheet(budBidID);
            GenericData.Insert<BUD_BID_DETAIL_TASK>(taskData);

            // Get newly created detail sheet id from BUD_BID_DETAIL_TASK table
            long detailTaskID = Convert.ToInt64(taskData.DETAIL_TASK_ID);

            List<BUD_BID_ACTUAL_NUM> startData = ProjectFormStartData(true, budBidID, detailTaskID);
            GenericData.Insert<BUD_BID_ACTUAL_NUM>(startData);

            List<BUD_BID_BUDGET_NUM> endData = ProjectFormEndData(true, budBidID, detailTaskID);
            GenericData.Insert<BUD_BID_BUDGET_NUM>(endData);
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

            UpdateDetailSheetsModifiedDateAndBy(budBidID);
        }
        protected BUD_BID_PROJECTS ProjectFormDetailData(bool insert, long budBidID = 0)
        {
            BUD_BID_PROJECTS data;

            if (insert == true)
            {
                data = new BUD_BID_PROJECTS();
                data.PROJECT_ID = "0";
                data.ORG_ID = Convert.ToInt64(Request.QueryString["OrgID"]);
                data.YEAR_ID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
                data.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = "TEMP";
            }
            else
            {
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
                }

                data.PROJECT_ID = uxHidProjectNumID.Text;
                data.ORG_ID = Convert.ToInt64(Request.QueryString["OrgID"]);
                data.YEAR_ID = Convert.ToInt64(Request.QueryString["fiscalYear"]);
                data.VER_ID = Convert.ToInt64(Request.QueryString["verID"]);
                data.STATUS_ID = Convert.ToInt64(uxHidStatusID.Text);
                data.ACRES = ForceToDecimal(uxAcres.Text, -9999999999.99M, 9999999999.99M);
                data.DAYS = ForceToDecimal(uxDays.Text, -9999999999.99M, 9999999999.99M);
                data.APP_TYPE = uxAppType.Text;
                data.CHEMICAL_MIX = uxChemMix.Text;
                data.COMMENTS = uxComments.Text;

                if (uxProjectNum.Text == "-- OVERRIDE --")
                {
                    data.PRJ_NAME = uxProjectName.Text;
                }
                else
                {
                    data.PRJ_NAME = null;
                }

                if (uxJCDate.Text == "")
                {
                    data.WE_OVERRIDE = null;
                    data.WE_DATE = null;
                }
                else
                {
                    if (uxJCDate.Text == "-- OVERRIDE --")
                    {
                        data.WE_OVERRIDE = "Y";
                        data.WE_DATE = null;
                    }
                    else
                    {
                        data.WE_OVERRIDE = "N";
                        data.WE_DATE = Convert.ToDateTime(uxJCDate.Text);
                    }
                }


                data.TYPE = uxHidType.Text;
                data.LIABILITY = uxLiabilityCheckbox.Checked == true ? "Y" : "N";
                data.LIABILITY_OP = ForceToDecimal(uxLiabilityAmount.Text, -9999999999.99M, 9999999999.99M);
                bool overridenOP = uxCompareOverride.Checked;
                data.COMPARE_PRJ_OVERRIDE = overridenOP == true ? "Y" : "N";

                if (overridenOP == true)
                {
                    data.COMPARE_PRJ_AMOUNT = ForceToDecimal(uxCompareOP.Text);
                }
                else
                {
                    data.COMPARE_PRJ_AMOUNT = 0;
                }

                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            }
            return data;
        }
        protected BUD_BID_DETAIL_TASK CreateProjectLevelDetailSheet(long budBidID)
        {
            BUD_BID_DETAIL_TASK data = new BUD_BID_DETAIL_TASK();

            data.PROJECT_ID = Convert.ToInt64(uxHidBudBidID.Text);
            data.DETAIL_NAME = "SYS_PROJECT";
            data.SHEET_ORDER = 0;
            data.CREATE_DATE = DateTime.Now;
            data.CREATED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = "TEMP";

            return data;
        }
        protected List<BUD_BID_ACTUAL_NUM> ProjectFormStartData(bool insert, long budBidID, long detailTaskID = 0)
        {
            long[] arrLineNum = { 6, 7, 8, 9, 10 };
            Ext.Net.TextField[] arrControl = { uxSGrossRec, uxSMatUsage, uxSGrossRev, uxSDirects, uxSOP };
            List<BUD_BID_ACTUAL_NUM> data;

            if (insert == true)
            {
                data = new List<BUD_BID_ACTUAL_NUM>();

                for (int i = 0; i <= 4; i++)
                {
                    data.Add(new BUD_BID_ACTUAL_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = detailTaskID,
                        LINE_ID = arrLineNum[i],
                        NOV = ForceToDecimal(arrControl[i].Text, -9999999999.99M, 9999999999.99M),
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = "TEMP"
                    });
                }
            }
            else
            {
                using (Entities context = new Entities())
                {
                    string sql = string.Format(@"
                        SELECT BUD_BID_ACTUAL_NUM.ACTUAL_NUM_ID,
                            BUD_BID_ACTUAL_NUM.PROJECT_ID,
                            BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID,
                            BUD_BID_ACTUAL_NUM.LINE_ID,
                            BUD_BID_ACTUAL_NUM.NOV,
                            BUD_BID_ACTUAL_NUM.DEC,
                            BUD_BID_ACTUAL_NUM.JAN,
                            BUD_BID_ACTUAL_NUM.FEB,
                            BUD_BID_ACTUAL_NUM.MAR,
                            BUD_BID_ACTUAL_NUM.APR,
                            BUD_BID_ACTUAL_NUM.MAY,
                            BUD_BID_ACTUAL_NUM.JUN,
                            BUD_BID_ACTUAL_NUM.JUL,
                            BUD_BID_ACTUAL_NUM.AUG,
                            BUD_BID_ACTUAL_NUM.SEP,
                            BUD_BID_ACTUAL_NUM.OCT,
                            BUD_BID_ACTUAL_NUM.CREATE_DATE,
                            BUD_BID_ACTUAL_NUM.CREATED_BY,
                            BUD_BID_ACTUAL_NUM.MODIFY_DATE,
                            BUD_BID_ACTUAL_NUM.MODIFIED_BY
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_ACTUAL_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_ACTUAL_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
                        ORDER BY LINE_ID", budBidID);
                    data = context.Database.SqlQuery<BUD_BID_ACTUAL_NUM>(sql).ToList();
                }

                int a = 0;
                foreach (BUD_BID_ACTUAL_NUM field in data)
                {
                    field.NOV = ForceToDecimal(arrControl[a].Text, -9999999999.99M, 9999999999.99M);
                    field.MODIFY_DATE = DateTime.Now;
                    field.MODIFIED_BY = User.Identity.Name;
                    a++;
                }
            }

            return data;
        }
        protected List<BUD_BID_BUDGET_NUM> ProjectFormEndData(bool insert, long budBidID, long detailTaskID = 0)
        {
            long[] arrLineNum = { 6, 7, 8, 9, 10 };
            Ext.Net.TextField[] arrControl = { uxEGrossRec, uxEMatUsage, uxEGrossRev, uxEDirects, uxEOP };
            List<BUD_BID_BUDGET_NUM> data;

            if (insert == true)
            {
                data = new List<BUD_BID_BUDGET_NUM>();

                for (int i = 0; i <= 4; i++)
                {
                    data.Add(new BUD_BID_BUDGET_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = detailTaskID,
                        LINE_ID = arrLineNum[i],
                        NOV = ForceToDecimal(arrControl[i].Text, -9999999999.99M, 9999999999.99M),
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = "TEMP"
                    });
                }
            }
            else
            {
                using (Entities context = new Entities())
                {
                    string sql = string.Format(@"
                        SELECT BUD_BID_BUDGET_NUM.BUDGET_NUM_ID,
                            BUD_BID_BUDGET_NUM.PROJECT_ID,
                            BUD_BID_BUDGET_NUM.DETAIL_TASK_ID,
                            BUD_BID_BUDGET_NUM.LINE_ID,
                            BUD_BID_BUDGET_NUM.NOV,
                            BUD_BID_BUDGET_NUM.DEC,
                            BUD_BID_BUDGET_NUM.JAN,
                            BUD_BID_BUDGET_NUM.FEB,
                            BUD_BID_BUDGET_NUM.MAR,
                            BUD_BID_BUDGET_NUM.APR,
                            BUD_BID_BUDGET_NUM.MAY,
                            BUD_BID_BUDGET_NUM.JUN,
                            BUD_BID_BUDGET_NUM.JUL,
                            BUD_BID_BUDGET_NUM.AUG,
                            BUD_BID_BUDGET_NUM.SEP,
                            BUD_BID_BUDGET_NUM.OCT,
                            BUD_BID_BUDGET_NUM.CREATE_DATE,
                            BUD_BID_BUDGET_NUM.CREATED_BY,
                            BUD_BID_BUDGET_NUM.MODIFY_DATE,
                            BUD_BID_BUDGET_NUM.MODIFIED_BY
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
                        ORDER BY LINE_ID", budBidID);
                    data = context.Database.SqlQuery<BUD_BID_BUDGET_NUM>(sql).ToList();

                    int a = 0;
                    foreach (BUD_BID_BUDGET_NUM field in data)
                    {
                        field.NOV = ForceToDecimal(arrControl[a].Text, -9999999999.99M, 9999999999.99M);
                        field.MODIFY_DATE = DateTime.Now;
                        field.MODIFIED_BY = User.Identity.Name;
                        a++;
                    }
                }
            }
            return data;
        }
        protected void UpdateDetailSheetsModifiedDateAndBy(long budBidID)
        {
            List<BUD_BID_DETAIL_TASK> taskData;            
            using (Entities context = new Entities())
            {
                taskData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            foreach (BUD_BID_DETAIL_TASK taskField in taskData)
            {
                taskField.MODIFY_DATE = DateTime.Now;
                taskField.MODIFIED_BY = User.Identity.Name;
            } 
            GenericData.Update<BUD_BID_DETAIL_TASK>(taskData);

            List<BUD_BID_BUDGET_NUM> budgetData;
            using (Entities context = new Entities())
            {
                budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            foreach (BUD_BID_BUDGET_NUM budgetField in budgetData)
            {
                budgetField.MODIFY_DATE = DateTime.Now;
                budgetField.MODIFIED_BY = User.Identity.Name;
            }
            GenericData.Update<BUD_BID_BUDGET_NUM>(budgetData);

            List<BUD_BID_DETAIL_SHEET> detailData;
            using (Entities context = new Entities())
            {
                detailData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            foreach (BUD_BID_DETAIL_SHEET detailField in detailData)
            {
                detailField.MODIFY_DATE = DateTime.Now;
                detailField.MODIFIED_BY = User.Identity.Name;
            } 
            GenericData.Update<BUD_BID_DETAIL_SHEET>(detailData);
        }



        // Detail                                   
        protected void deLoadDetailActions(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxDetailActionsStore.DataSource = BB.YearSummaryDetailActions(orgID, yearID, verID);
        }
        protected void deChooseDetailAction(object sender, DirectEventArgs e)
        {
            string selectedAction = uxDetailActions.Text;

            switch (selectedAction)
            {
                case "Add a New Sheet":
                    AddNewDetailSheet();
                    break;

                case "View Selected Sheet":
                    LoadDetailSheet();
                    break;

                case "Edit Selected Sheet":
                    LoadDetailSheet();
                    break;

                case "Copy Selected Sheet":
                    CopyDetailSheet();
                    break;

                case "Delete Selected Sheet":
                    DeleteDetailSheet();
                    break;

                case "Reorder Sheets":
                    ReorderSheets();
                    break;

                case "Print All Sheets":
                    break;
            }

            uxDetailActions.Clear();
        }
        protected void AddNewDetailSheet()
        {
            if (uxHidBudBidID.Text == "")
            {
                StandardMsgBox("Save", "The project must be saved first before you can create detail sheets.  Please save the project to continue.", "INFO");
                return;
            }

            long budBidProjectID = Convert.ToInt64(uxHidBudBidID.Text);
            long newOrder = BBDetail.Sheets.MaxOrder(budBidProjectID) + 1;
            uxHidDetailSheetID.Text = BBDetail.Sheet.DBAdd(budBidProjectID, newOrder).ToString();
            uxHidDetailSheetOrder.Text = newOrder.ToString();
            uxHidDetailSheetName.Text = "SYS_DETAIL_" + newOrder;
            LoadDetailSheet();
        }
        protected void DeleteDetailSheet()
        {
            if (uxHidDetailSheetID.Text == "")
            {
                StandardMsgBox("Delete", "A detail sheet must be selected before it can be deleted.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Delete", "Are you sure you want to delete the selected detail sheet? Once it's been deleted it cannot be retrieved.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.DeleteDetailSheetContiued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }
        [DirectMethod]
        public void DeleteDetailSheetContiued()
        {
            long detailSheetID = Convert.ToInt64(uxHidDetailSheetID.Text);
            long budBidProjectID = Convert.ToInt64(uxHidBudBidID.Text);

            BBDetail.Sheet.DBDelete(detailSheetID);
            BBDetail.Sheets.DBResetOrder(budBidProjectID);

            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            PopulateProjectEndNumbers();
            uxSummaryDetailStore.Reload();
        }
        protected void CopyDetailSheet()
        {
            if (uxHidDetailSheetID.Text == "")
            {
                StandardMsgBox("Copy", "A detail sheet must be selected before it can be copied.", "INFO");
                return;
            }

            X.MessageBox.Confirm("Copy", "Are you sure you want to copy the selected detail sheet?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "App.direct.CopyDetailSheetContinued()", Text = "Yes" },
                No = new MessageBoxButtonConfig { Text = "No" }
            }).Show();
        }
        [DirectMethod]
        public void CopyDetailSheetContinued()
        {
            long detailSheetID = Convert.ToInt64(uxHidDetailSheetID.Text);
            long copiedSheetID = BBDetail.Sheet.DBCopy(detailSheetID);

            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            PopulateProjectEndNumbers();
            uxSummaryDetailStore.Reload();
        }
        protected void deSelectDetailSheet(object sender, DirectEventArgs e)
        {
            string detailSheetID = e.ExtraParams["DetailSheetID"];
            string detailSheetOrder = e.ExtraParams["DetailSheetOrder"];
            string detailSheetName = e.ExtraParams["DetailSheetName"];

            uxHidDetailSheetID.Text = detailSheetID;
            uxHidDetailSheetOrder.Text = detailSheetOrder;
            uxHidDetailSheetName.Text = detailSheetName;
        }
        protected void LoadDetailSheet()
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            if (uxHidDetailSheetID.Text == "")
            {
                if (BB.IsReadOnly(orgID, yearID, verID) == true)
                {
                    StandardMsgBox("View", "A detail sheet must be selected before it can be viewed.", "INFO");
                    return;
                }
                else
                {
                    StandardMsgBox("Edit", "A detail sheet must be selected before it can be edited.", "INFO");
                    return;
                }
            }

            string budBidProjectID = uxHidBudBidID.Text;
            string detailSheetID = uxHidDetailSheetID.Text;
            string verName = HttpUtility.UrlEncode(Request.QueryString["verName"]);
            string weDate = uxJCDate.Text;
            string projectName = HttpUtility.UrlEncode(uxProjectName.Text);
            string sheetNum = uxHidDetailSheetOrder.Text;
            string detailSheetName = HttpUtility.UrlEncode(uxHidDetailSheetName.Text);
            string sGrossRec = uxSGrossRec.Text;
            string sMatUsage = uxSMatUsage.Text;
            string sGrossRev = uxSGrossRev.Text;
            string sDirects = uxSDirects.Text;
            string sOP = uxSOP.Text;

            string url = "/Views/Modules/BudgetBidding/umDetailSheet.aspx?projectID=" + budBidProjectID + "&detailSheetID=" + detailSheetID + "&orgID=" + orgID + "&yearID=" + yearID + "&verID=" + verID +
                "&verName=" + verName + "&weDate=" + weDate + "&projectName=" + projectName + "&sheetNum=" + sheetNum + "&detailSheetName=" + detailSheetName +
                "&sGrossRec=" + sGrossRec + "&sMatUsage=" + sMatUsage + "&sGrossRev=" + sGrossRev + "&sDirects=" + sDirects + "&sOP=" + sOP;

            Window win = new Window
            {
                ID = "uxAddEditDetailSheet",
                Height = 700,
                Width = 700,
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
        public void CloseDetailWindow(string sheetName)
        {
            long detailSheetID = Convert.ToInt64(uxHidDetailSheetID.Text);
            BBDetail.Sheet.DBUpdateName(detailSheetID, sheetName);

            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            PopulateProjectEndNumbers();
            uxSummaryDetailStore.Reload();
        }
        protected void PopulateProjectEndNumbers()
        {
            long budBidProjectID = uxHidBudBidID.Text == "" ? 0 : Convert.ToInt64(uxHidBudBidID.Text);

            decimal sGrossRec = ForceToDecimal(uxSGrossRec.Text, -9999999999.99M, 9999999999.99M);
            decimal sMatUsage = ForceToDecimal(uxSMatUsage.Text, -9999999999.99M, 9999999999.99M);
            decimal sGrossRev = ForceToDecimal(uxSGrossRev.Text);
            decimal sDirects = ForceToDecimal(uxSDirects.Text, -9999999999.99M, 9999999999.99M);
            decimal sOP = ForceToDecimal(uxSOP.Text);


            BBDetail.Sheets.EndNumbers.DBCalculate(budBidProjectID, sGrossRec, sMatUsage, sGrossRev, sDirects, sOP);

            long maxOrder = BBDetail.Sheets.MaxOrder(budBidProjectID);
            decimal eGrossRec;
            decimal eMatUsage;
            decimal eGrossRev;
            decimal eDirects;
            decimal eOP;
            if (maxOrder == 0)
            {
                eGrossRec = sGrossRec;
                eMatUsage = sMatUsage;
                eGrossRev = sGrossRev;
                eDirects = sDirects;
                eOP = sOP;
                uxEGrossRec.Text = String.Format("{0:N2}", eGrossRec);
                uxEMatUsage.Text = String.Format("{0:N2}", eMatUsage);
                uxEGrossRev.Text = String.Format("{0:N2}", eGrossRev);
                uxEDirects.Text = String.Format("{0:N2}", eDirects);
                uxEOP.Text = String.Format("{0:N2}", eOP);
            }
            else
            {
                long lastID = BBDetail.Sheet.ID(budBidProjectID, maxOrder);
                BBDetail.Sheet.EndNumbers.Fields data = BBDetail.Sheet.EndNumbers.Get(lastID);
                eGrossRec = data.GROSS_REC;
                eMatUsage = data.MAT_USAGE;
                eGrossRev = data.GROSS_REV;
                eDirects = data.DIR_EXP;
                eOP = data.OP;

                BBProject.EndNumbersW0.DBUpdate(budBidProjectID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);

                BBProject.EndNumbersW0.Fields dataEnd = BBProject.EndNumbersW0.Data(budBidProjectID);
                uxEGrossRec.Text = String.Format("{0:N2}", dataEnd.GROSS_REC);
                uxEMatUsage.Text = String.Format("{0:N2}", dataEnd.MAT_USAGE);
                uxEGrossRev.Text = String.Format("{0:N2}", dataEnd.GROSS_REV);
                uxEDirects.Text = String.Format("{0:N2}", dataEnd.DIR_EXP);
                uxEOP.Text = String.Format("{0:N2}", dataEnd.OP);
            }
        }
        protected void ReorderSheets()
        {
            long budBidProjectID = Convert.ToInt64(uxHidBudBidID.Text);

            if (BBDetail.Sheets.MaxOrder(budBidProjectID) == 0)
            {
                StandardMsgBox("Reorder Detail Sheets", "There are no detail sheets to reorder.", "INFO");
                return;
            }

            string url = "/Views/Modules/BudgetBidding/umReorderDetailSheets.aspx?projectID=" + budBidProjectID;

            Window win = new Window
            {
                ID = "uxReorderDetailSheetsForm",
                Height = 400,
                Width = 400,
                Title = "Reorder Sheets",
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

        // Other     
        protected void LockTopSection()
        {
            uxActions.Disable();
            uxSummaryReports.Disable();
            uxUpdateAllActuals.Disable();
            uxGridRowModel.ClearSelection();
            uxSummaryGrid.Disable();
            uxAdjustmentGridRowModel.ClearSelection();
            uxAdjustmentsGrid.Disable();
            uxOverheadGridRowModel.ClearSelection();
            uxOverheadGrid.Disable();
        }
        protected void UnlockTopSection()
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);

            uxActions.Enable();
            uxSummaryReports.Enable();
            if (BB.IsReadOnly(orgID, yearID, verID) == true)
            {
                uxUpdateAllActuals.Disable();
            }
            else
            {
                uxUpdateAllActuals.Enable();
            }
            uxSummaryGrid.Enable();
            uxAdjustmentsGrid.Enable();
            uxOverheadGrid.Enable();
        }
        protected void ResetProjectForm()
        {
            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxProjectName.ReadOnly = true;
            uxJCDate.Enable();
            uxSGrossRec.ReadOnly = true;
            uxSMatUsage.ReadOnly = true;
            uxSDirects.ReadOnly = true;
            uxSave.Disable();
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
<<<<<<< HEAD
        
        private void LoadReport()
        {
            
        }
=======
        protected void deCancel(object sender, DirectEventArgs e)
        {
            uxProjectInfo.Disable();
            if (uxHidBudBidID.Text != "")
            {
                BBProject.DBDelete(Convert.ToInt64(uxHidBudBidID.Text));
            }
            uxProjectInfo.Reset();
            uxSummaryDetailStore.RemoveAll();
            uxCompareVar.Text = "0.00";

            uxHidBudBidID.Text = "";
            uxHidProjectNumID.Text = "";
            uxHidType.Text = "";
            uxHidStatusID.Text = "";
            uxHidFormEnabled.Text = "";
            uxHidOldBudBidID.Text = "";
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";
            uxHidDetailSheetOrder.Text = "";

            UnlockTopSection();
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
            decimal retVal = ForceToDecimal(myTextField.Text, -9999999999.99M, 9999999999.99M);
            myTextField.Text = String.Format("{0:N2}", retVal);
        }
        protected void deCalcGRandOP(object sender, DirectEventArgs e)
        {
            deFormatNumber(sender, e);

            decimal sGrossRec = ForceToDecimal(uxSGrossRec.Text, -9999999999.99M, 9999999999.99M);
            decimal sMatUsage = ForceToDecimal(uxSMatUsage.Text, -9999999999.99M, 9999999999.99M);
            decimal sGrossRev = sGrossRec - sMatUsage;
            decimal sDirects = ForceToDecimal(uxSDirects.Text, -9999999999.99M, 9999999999.99M); 
            decimal sOP = sGrossRev - sDirects;

            uxSGrossRev.Text = String.Format("{0:N2}", sGrossRev);
            uxSOP.Text = String.Format("{0:N2}", sOP);
>>>>>>> develop

            UpdateCompareNums(uxCompareOverride.Checked);
            PopulateProjectEndNumbers();
            uxSummaryDetailStore.Reload(); 
        }
        protected void CalcSummaryTotals()
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long yearID = long.Parse(Request.QueryString["fiscalYear"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long prevYearID = Convert.ToInt64(uxHidPrevYear.Text);
            long prevVerID = Convert.ToInt64(uxHidPrevVer.Text);

            // Active
            BBSummary.Subtotals.Fields activeData = BBSummary.Subtotals.Data(true, orgID, yearID, verID, prevYearID, prevVerID);
            decimal aGrossRec = activeData.GROSS_REC;
            decimal aMatUsage = activeData.MAT_USAGE;
            decimal aGrossRev = activeData.GROSS_REV;
            decimal aDirects = activeData.DIR_EXP;
            decimal aOP = activeData.OP;
            decimal aOPPerc = aGrossRev == 0 ? 0 : aOP / aGrossRev;
            decimal aOPPlusMinus = activeData.OP_VAR;
            uxAGrossRec.Text = String.Format("{0:N2}", aGrossRec);
            uxAMatUsage.Text = String.Format("{0:N2}", aMatUsage);
            uxAGrossRev.Text = String.Format("{0:N2}", aGrossRev);
            uxADirects.Text = String.Format("{0:N2}", aDirects);
            uxAOP.Text = String.Format("{0:N2}", aOP);
            uxAOPPerc.Text = String.Format("{0:#,##0.00%}", aOPPerc);
            uxAOPPlusMinus.Text = String.Format("{0:N2}", aOPPlusMinus);
            
            // Inactive
            BBSummary.Subtotals.Fields inactiveData = BBSummary.Subtotals.Data(false, orgID, yearID, verID, prevYearID, prevVerID);
            decimal iGrossRec = inactiveData.GROSS_REC;
            decimal iMatUsage = inactiveData.MAT_USAGE;
            decimal iGrossRev = inactiveData.GROSS_REV;
            decimal iDirects = inactiveData.DIR_EXP;
            decimal iOP = inactiveData.OP;
            decimal iOPPerc = iGrossRev == 0 ? 0 : iOP / iGrossRev;
            decimal iOPPlusMinus = inactiveData.OP_VAR;
            uxIGrossRec.Text = String.Format("{0:N2}", iGrossRec);
            uxIMatUsage.Text = String.Format("{0:N2}", iMatUsage);
            uxIGrossRev.Text = String.Format("{0:N2}", iGrossRev);
            uxIDirects.Text = String.Format("{0:N2}", iDirects);
            uxIOP.Text = String.Format("{0:N2}", iOP);
            uxIOPPerc.Text = String.Format("{0:#,##0.00%}", iOPPerc);
            uxIOPPlusMinus.Text = String.Format("{0:N2}", iOPPlusMinus);

            // Total
            decimal tGrossRec = aGrossRec + iGrossRec;
            decimal tMatUsage = aMatUsage + iMatUsage;
            decimal tGrossRev = aGrossRev + iGrossRev;
            decimal tDirects = aDirects + iDirects;
            decimal tOP = aOP + iOP;
            decimal tOPPerc = tGrossRev == 0 ? 0 : tOP / tGrossRev;
            decimal tOPPlusMinus = aOPPlusMinus + iOPPlusMinus;
            uxTGrossRec.Text = String.Format("{0:N2}", tGrossRec);
            uxTMatUsage.Text = String.Format("{0:N2}", tMatUsage);
            uxTGrossRev.Text = String.Format("{0:N2}", tGrossRev);
            uxTDirects.Text = String.Format("{0:N2}", tDirects);
            uxTOP.Text = String.Format("{0:N2}", tOP);
            uxTOPPerc.Text = String.Format("{0:#,##0.00%}", tOPPerc);
            uxTOPPlusMinus.Text = String.Format("{0:N2}", tOPPlusMinus);

            // Adjustments
            BBAdjustments.Subtotal.Fields adjustmentData = BBAdjustments.Subtotal.Data(orgID, yearID, verID);
            decimal adjMatUsage = adjustmentData.MAT_ADJ ?? 0;          
            decimal adjDirects = adjustmentData.WEATHER_ADJ ?? 0;

            // Previous adjustments for grand total +/-
            BBAdjustments.Subtotal.Fields prevAdjustmentData = BBAdjustments.Subtotal.Data(orgID, prevYearID, prevVerID);
            decimal prevAdjMatUsage = prevAdjustmentData.MAT_ADJ ?? 0;
            decimal prevAdjDirects = prevAdjustmentData.WEATHER_ADJ ?? 0;
            decimal prevOPIncludingAdj = (tOP - tOPPlusMinus) - (prevAdjMatUsage + prevAdjDirects);

            // Grand total
            decimal gtGrossRec = tGrossRec;
            decimal gtMatUsage = tMatUsage + adjMatUsage;
            decimal gtGrossRev = gtGrossRec - gtMatUsage;
            decimal gtDirects = tDirects + adjDirects;
            decimal gtOP = gtGrossRev - gtDirects;
            decimal gtOPPerc = gtGrossRev == 0 ? 0 : gtOP / gtGrossRev;
            decimal gtOPPlusMinus = gtOP - prevOPIncludingAdj;
            uxGTGrossRec.Text = String.Format("{0:N2}", gtGrossRec);
            uxGTMatUsage.Text = String.Format("{0:N2}", gtMatUsage);
            uxGTGrossRev.Text = String.Format("{0:N2}", gtGrossRev);
            uxGTDirects.Text = String.Format("{0:N2}", gtDirects);
            uxGTOP.Text = String.Format("{0:N2}", gtOP);
            uxGTOPPerc.Text = String.Format("{0:#,##0.00%}", gtOPPerc);
            uxGTOPPlusMinus.Text = String.Format("{0:N2}", gtOPPlusMinus);

            // Net contribution
            BBOH.Subtotal.Fields ohData = BBOH.Subtotal.Data(orgID, yearID, verID);
            decimal oh = ohData.OH;
            decimal netCont = gtOP - oh;
            uxNetCont.Text = String.Format("{0:N2}", netCont);
        }
        [DirectMethod(Namespace = "SaveRecord")]
        public void deSaveAdjustments(long id, string field, string newValue)
        {
            decimal amount;
            amount = ForceToDecimal(newValue, -9999999999.99M, 9999999999.99M);

            BUD_BID_ADJUSTMENT data;

            using (Entities context = new Entities())
            {
                data = context.BUD_BID_ADJUSTMENT.Where(x => x.ADJ_ID == id).Single();
            }

            if (field == "WEATHER_ADJ")
            {
                data.WEATHER_ADJ = amount;
            }
            else if (field == "MAT_ADJ")
            {
                data.MAT_ADJ = amount;
            }


            GenericData.Update<BUD_BID_ADJUSTMENT>(data);
            uxAdjustmentGridStore.CommitChanges();
            CalcSummaryTotals();
            StandardMsgBox("Adjustment", "The adjustment has been updated.", "INFO");
        }
        protected void LockProjectInfo()
        {
            uxProjectNum.ReadOnly = true;
            uxProjectName.ReadOnly = true;
            uxStatus.ReadOnly = true;
            uxComments.ReadOnly = true;
            uxCompareOverride.ReadOnly = true;
            uxCompareOP.ReadOnly = true;
            uxAcres.ReadOnly = true;
            uxDays.ReadOnly = true;
            uxLiabilityCheckbox.ReadOnly = true;
            uxLiabilityAmount.ReadOnly = true;
            uxAppType.ReadOnly = true;
            uxChemMix.ReadOnly = true;
            uxJCDate.ReadOnly = true;
            uxSGrossRec.ReadOnly = true;
            uxSMatUsage.ReadOnly = true;
            uxSDirects.ReadOnly = true;
            uxSave.Disable();
        }

        [DirectMethod]
        public void CloseReorderDetailSheetsWindow()
        {
            uxHidDetailSheetID.Text = "";
            uxHidDetailSheetOrder.Text = "";
            uxHidDetailSheetName.Text = "";

            PopulateProjectEndNumbers();
            uxSummaryDetailStore.Reload();
        }
    }
}