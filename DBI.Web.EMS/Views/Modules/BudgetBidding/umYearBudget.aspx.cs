using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Data.DataFactory;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umYearBudget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)        
        {

        }

        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            //using (Entities _context = new Entities())
            //{
            //    List<object> dataSource;
            //    dataSource = (from d in _context.CROSSINGS
            //                  join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID into pn
            //                  from proj in pn.DefaultIfEmpty()
            //                  select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID, proj.LONG_NAME }).ToList<object>();
            //    int count;
            //    uxSummaryGridStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            //    e.Total = count;
            //}

            List<YearSummaryStruct> list = new List<YearSummaryStruct> 
            {
                    new YearSummaryStruct(1, "Test Project 1", "Expected", 2, 5, 10000, 2000, 8000, 1000, 7000, 12.50m, 1500),
                    new YearSummaryStruct(2, "Test Project 2", "Renewal", 2, 5, 15000, 2000, 8000, 1000, 12000, -12.5m, 1500),
                    new YearSummaryStruct(3, "Test Project 3", "Expected", 2, 5, 20000, 2000, 8000, 1000, 17000, 12.5m, 1500),
                    new YearSummaryStruct(4, "Test Project 4", "Renewal", 2, 5, 25000, 2000, 8000, 1000, 23000, 12.5m, 1500),
                    new YearSummaryStruct(5, "Test Project 5", "Renewal", 2, 5, 30000, 2000, 8000, 1000, 28000, 12.5m, 1500),
                    new YearSummaryStruct(6, "Test Project 6", "New Sale", 2, 5, 35000, 2000, 8000, 1000, 33000, 12.5m, 1500),
                    new YearSummaryStruct(7, "Test Project 7", "Expected", 2, 5, 40000, 2000, 8000, 1000, 38000, 12.5m, 1500),
                    new YearSummaryStruct(8, "Test Project 8", "Expected", 2, 5, 45000, 2000, 8000, 1000, 43000, 12.5m, 1500),
                    new YearSummaryStruct(9, "Test Project 9", "New Sale", 2, 5, 50000, 2000, 8000, 1000, 48000, 12.5m, 1500),
                    new YearSummaryStruct(10, "Test Project 10", "New Sale", 2, 5, 55000, 2000, 8000, 1000, 53000, 12.5m, 1500)
            };
            uxSummaryGridStore.DataSource = list;
        }

        class YearSummaryStruct  // DELETE WHEN GETTING DATA FROM CORRECT SOURCE
        {
            public long PROJ_ID { get; set; }
            public string PROJECT_NAME { get; set; }
            public string STATUS { get; set; }
            public decimal ACRES { get; set; }
            public decimal DAYS { get; set; }
            public decimal GROSS_REC { get; set; }
            public decimal MAT_USAGE { get; set; }
            public decimal GROSS_REV { get; set; }
            public decimal DIR_EXP { get; set; }
            public decimal OP { get; set; }
            public decimal OP_PERC { get; set; }
            public decimal OP_VAR { get; set; }

            public YearSummaryStruct(long id, string project, string proStatus, decimal proAcres, decimal proDays,
                decimal grRec, decimal mat, decimal grRev, decimal dirs, decimal proOP, decimal proOPPerc, decimal proOPVar)
            {
                PROJ_ID = id;
                PROJECT_NAME = project;
                STATUS = proStatus;
                ACRES = proAcres;
                DAYS = proDays;
                GROSS_REC = grRec;
                MAT_USAGE = mat;
                GROSS_REV = grRev;
                DIR_EXP = dirs;
                OP = proOP;
                OP_PERC = proOPPerc;
                OP_VAR = proOPVar;
            }
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
            }
        }
        
        protected void deCancel(object sender, DirectEventArgs e)
        {
            uxProjectDetail.Reset();
            uxProjectDetail.Disable();
        }

        protected void deSave(object sender, DirectEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long verID = long.Parse(Request.QueryString["verID"]);
            long fiscalYear = long.Parse(Request.QueryString["fiscalYear"]);
            long projectNum = Convert.ToInt64(uxProjectNum.Value);
            string projectName = uxProjectName.Value.ToString();

            BUD_BID_PROJECTS data = new BUD_BID_PROJECTS()
            {
                ORG_ID = orgID,
                VER_ID = verID,
                YEAR_ID = fiscalYear,
                PROJECT_ID = projectNum.ToString(),
                PRJ_NAME = projectName
            };

            GenericData.Insert<BUD_BID_PROJECTS>(data);

            BUD_BID_BUDGET_NUM data1 = new BUD_BID_BUDGET_NUM()
            {
                PROJECT_ID = projectNum,

            };











            NotificationMsg("Save", "Project has been saved.", Icon.DiskBlack);         
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
            string projectID = uxHidProjectID.Value.ToString();
            string projectNum = uxHidProjectNum.Value.ToString();
            string projectName = uxHidProjectName.Value.ToString();
            string type = uxHidType.Value.ToString();
            string jcDate = uxHidDate.Value.ToString();
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
    }
}