using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;
using System.Web;

namespace DBI.Data
{
    public class BB
    {
        public static void CleanOldTempRecords(int numOfDaysOld)
        {
            DateTime delDate = DateTime.Today.AddDays(-numOfDaysOld);

            // Delete old temp tasks and associated records
            List<decimal> detailIDs;
            using (Entities context = new Entities())
            {
                detailIDs = context.BUD_BID_DETAIL_TASK.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).Select(x => x.DETAIL_TASK_ID).ToList();
            }

            foreach (long detailID in detailIDs)
            {
                List<BUD_BID_ACTUAL_NUM> actualData1;
                List<BUD_BID_BUDGET_NUM> budgetData1;
                List<BUD_BID_DETAIL_SHEET> detailSheetData1;
                List<BUD_BID_DETAIL_TASK> taskInfoData1;
                using (Entities context = new Entities())
                {
                    actualData1 = context.BUD_BID_ACTUAL_NUM.Where(x => x.DETAIL_TASK_ID == detailID).ToList();
                    budgetData1 = context.BUD_BID_BUDGET_NUM.Where(x => x.DETAIL_TASK_ID == detailID).ToList();
                    detailSheetData1 = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_TASK_ID == detailID).ToList();
                    taskInfoData1 = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailID).ToList();
                }

                GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData1);
                GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData1);
                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData1);
                GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData1);
            }

            // Delete old temp projects and associated records
            List<decimal> projectIDs;
            using (Entities context = new Entities())
            {
                projectIDs = context.BUD_BID_PROJECTS.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).Select(x => x.BUD_BID_PROJECTS_ID).ToList();
            }

            foreach (long projectID in projectIDs)
            {
                List<BUD_BID_ACTUAL_NUM> actualData2;
                List<BUD_BID_BUDGET_NUM> budgetData2;
                List<BUD_BID_DETAIL_SHEET> detailSheetData2;
                List<BUD_BID_DETAIL_TASK> taskInfoData2;
                List<BUD_BID_PROJECTS> projectData2;

                using (Entities context = new Entities())
                {
                    actualData2 = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == projectID).ToList();
                    budgetData2 = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == projectID).ToList();
                    detailSheetData2 = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == projectID).ToList();
                    taskInfoData2 = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == projectID).ToList();
                    taskInfoData2 = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == projectID).ToList();
                    projectData2 = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == projectID).ToList();
                }

                GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData2);
                GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData2);
                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData2);
                GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData2);
                GenericData.Delete<BUD_BID_PROJECTS>(projectData2);
            }


            // Delete remianing lower level old temp records
            List<BUD_BID_ACTUAL_NUM> actualData3;
            List<BUD_BID_BUDGET_NUM> budgetData3;
            List<BUD_BID_DETAIL_SHEET> detailSheetData3;

            using (Entities context = new Entities())
            {
                actualData3 = context.BUD_BID_ACTUAL_NUM.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
                budgetData3 = context.BUD_BID_BUDGET_NUM.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
                detailSheetData3 = context.BUD_BID_DETAIL_SHEET.Where(x => x.MODIFIED_BY == "TEMP" && x.MODIFY_DATE <= delDate).ToList();
            }

            GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData3);
            GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData3);
            GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData3);
        }
        public static List<DoubleComboLongID> BudgetVersions()
        {
            List<DoubleComboLongID> comboItem = new List<DoubleComboLongID>();
            comboItem.Add(new DoubleComboLongID { ID = 1, ID_NAME = "Bid" });
            comboItem.Add(new DoubleComboLongID { ID = 2, ID_NAME = "First Draft" });
            comboItem.Add(new DoubleComboLongID { ID = 3, ID_NAME = "Final Draft" });
            comboItem.Add(new DoubleComboLongID { ID = 4, ID_NAME = "1st Reforecast" });
            comboItem.Add(new DoubleComboLongID { ID = 5, ID_NAME = "2nd Reforecast" });
            comboItem.Add(new DoubleComboLongID { ID = 6, ID_NAME = "3rd Reforecast" });
            comboItem.Add(new DoubleComboLongID { ID = 7, ID_NAME = "4th Reforecast" });
            return comboItem;
        }
        public static bool IsUserOrgAndAllowed(long userID, long orgID)
        {
            bool userOrgAllowed = SYS_USER_ORGS.IsInOrg(userID, orgID);

            bool allowedOrgAllowed;
            using (Entities context = new Entities())
            {
                allowedOrgAllowed = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 48 && x.PROFILE_VALUE == "Y").Select(x => x.ORGANIZATION_ID).Contains(orgID);
            }

            if (userOrgAllowed == true && allowedOrgAllowed == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsBudgetOrg(long orgID)
        {
            using (Entities context = new Entities())
            {
                return context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 48 && x.PROFILE_VALUE == "Y").Select(x => x.ORGANIZATION_ID).Contains(orgID);
            }
        }
        public static bool IsReadOnly(long orgID, long yearID, long verID)
        {
            string lookupValue = orgID + ":" + yearID + ":" + verID + ":Y";

            using (Entities context = new Entities())
            {
                return context.SYS_MODULE_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 49 && x.PROFILE_VALUE.Contains(lookupValue)).Select(x => x.PROFILE_VALUE).Contains(lookupValue);
            }
        }
        public static long CalcPrevYear(long curYear, long curVer)
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
                    return curYear;
            }
        }
        public static long CalcPrevVer(long curYear, long curVer)
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
                    return curVer;
            }
        }
        public static string GetPrevVerName(long curVer)
        {
            switch (curVer)
            {
                case 1:  // Bid
                    return "Bid";

                case 2:  // First Draft
                    return "Bid";

                case 3:  // Final Draft
                    return "First Draft";

                case 4:  // 1st Reforecast
                    return "Final Draft";

                case 5:  // 2nd Reforecast
                    return "1st Reforecast";

                case 6:  // 3rd Reforecast
                    return "2nd Reforecast";

                case 7:  // 4th Reforecast
                    return "3rd Reforecast";

                default:
                    return "";
            }
        }
        public static List<SingleCombo> YearSummaryProjectActions(long orgID, long yearID, long verID)
        {
            List<SingleCombo> comboItems = new List<SingleCombo>();
            bool readOnly = BB.IsReadOnly(orgID, yearID, verID);

            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Add a New Project" }); }

            if (readOnly == false)
            {
                comboItems.Add(new SingleCombo { ID_NAME = "Edit Selected Project" });
            }
            else
            {
                comboItems.Add(new SingleCombo { ID_NAME = "View Selected Project" });
            }

            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Copy Selected Project" }); }

            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Delete Selected Project" }); }

            comboItems.Add(new SingleCombo { ID_NAME = "Refresh Data" });

            return comboItems;
        }
        public static bool ProjectStillExists(long budBidProjectID)
        {
            decimal returnedID;
            using (Entities context = new Entities())
            {
                returnedID = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidProjectID).Select(x => x.BUD_BID_PROJECTS_ID).SingleOrDefault();
            }

            if (returnedID != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
        
        public static decimal LaborBurdenRate(long leOrgID, long yearID)
        {
            decimal laborBurden;
            try
            {
                laborBurden = PA.LaborBurden(leOrgID.ToString(), yearID.ToString());
            }
            catch (DBICustomException ex)
            {
                laborBurden = 0;
            }

            return laborBurden;
        }
        public static string OrgName(long orgID)
        {
            string sql = string.Format(@"
                SELECT NAME
                FROM APPS.HR_ALL_ORGANIZATION_UNITS WHERE ORGANIZATION_ID = {0}", orgID);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<string>(sql).SingleOrDefault();
            }
        }
        public static bool IsRollup(long orgID)
        {
            string profileValue;
            using (Entities context = new Entities())
            {
                profileValue = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 47 && x.ORGANIZATION_ID == orgID).Select(x => x.PROFILE_VALUE).SingleOrDefault();
            }

            if (profileValue == "ROLLUP")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<long> OverallSummaryBudgetOrgsBelowCurrent(long userID, long hierarchyID, long selOrg)
        {
            List<long> testOrgs = new List<long>();
            List<long> showOrgs = new List<long>();
            long rollupCount;
            int leafCount;

            List<long> levelOneOrgsBelow = HR.ActiveOrganizationsByHierarchy(hierarchyID, selOrg).Where(x => x.HIER_LEVEL == 1).Select(x => x.ORGANIZATION_ID).ToList();
            testOrgs.AddRange(levelOneOrgsBelow);

            int i = 0;
            while (i < testOrgs.Count)
            {
                selOrg = testOrgs[i];
                leafCount = HR.ActiveOrganizationsByHierarchy(hierarchyID, selOrg).Count();
                using (Entities context = new Entities())
                {
                    rollupCount = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 47 && x.ORGANIZATION_ID == selOrg && x.PROFILE_VALUE == "ROLLUP").Count();
                }

                if (leafCount == 0 || rollupCount != 0)
                {
                    if (IsBudgetOrg(selOrg) == true)
                    {
                        showOrgs.Add(selOrg);
                    }
                }
                else
                {
                    levelOneOrgsBelow = HR.ActiveOrganizationsByHierarchy(hierarchyID, selOrg).Where(x => x.HIER_LEVEL == 1).Select(x => x.ORGANIZATION_ID).ToList();
                    testOrgs.AddRange(levelOneOrgsBelow);
                }

                i++;
            }

            return showOrgs;
        }
        public static List<long> UserAllowedOrgs(long userID)
        {
            List<long> userOrgs = SYS_USER_ORGS.GetUserOrgs(userID).Select(x => x.ORG_ID).ToList();
            List<long> allowedOrgs;
            using (Entities context = new Entities())
            {
                allowedOrgs = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 48 && x.PROFILE_VALUE == "Y").Select(x => x.ORGANIZATION_ID).ToList();
            }

            return userOrgs.Intersect(allowedOrgs).ToList();
        }

        public static List<SingleCombo> YearSummaryDetailActions(long orgID, long yearID, long verID)
        {
            List<SingleCombo> comboItems = new List<SingleCombo>();
            bool readOnly = BB.IsReadOnly(orgID, yearID, verID);
            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Add a New Sheet" }); }
            if (readOnly == false)
            {
                comboItems.Add(new SingleCombo { ID_NAME = "Edit Selected Sheet" });
            }
            else
            {
                comboItems.Add(new SingleCombo { ID_NAME = "View Selected Sheet" });
            }
            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Copy Selected Sheet" }); }
            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Delete Selected Sheet" }); }
            if (readOnly == false) { comboItems.Add(new SingleCombo { ID_NAME = "Reorder Sheets" }); }
            //comboItems.Add(new SingleCombo { ID_NAME = "Print All Sheets" });
            return comboItems;
        }
        public static long CopyAllProjectDataAsTemp(long budBidID)
        {
            // BBProject
            BUD_BID_PROJECTS projectData;
            using (Entities context = new Entities())
            {
                projectData = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
            }
            projectData.MODIFIED_BY = "TEMP";
            projectData.MODIFY_DATE = DateTime.Now;
            GenericData.Insert<BUD_BID_PROJECTS>(projectData);
            decimal newBudBidID = projectData.BUD_BID_PROJECTS_ID;

            // Tasks
            List<BUD_BID_DETAIL_TASK> taskData;
            using (Entities context = new Entities())
            {
                taskData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            decimal detailTaskID;
            decimal newDetailTaskID;
            foreach (BUD_BID_DETAIL_TASK taskField in taskData)
            {
                detailTaskID = taskField.DETAIL_TASK_ID;
                taskField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                taskField.MODIFIED_BY = "TEMP";
                taskField.MODIFY_DATE = DateTime.Now;
                GenericData.Insert<BUD_BID_DETAIL_TASK>(taskData.Where(x => x.DETAIL_TASK_ID == detailTaskID).Single());
                newDetailTaskID = taskField.DETAIL_TASK_ID;

                // Actuals
                List<BUD_BID_ACTUAL_NUM> actualData;
                using (Entities context = new Entities())
                {
                    actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }

                foreach (BUD_BID_ACTUAL_NUM actualField in actualData)
                {
                    actualField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    actualField.DETAIL_TASK_ID = newDetailTaskID;
                    actualField.MODIFIED_BY = "TEMP";
                    actualField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_ACTUAL_NUM>(actualData);

                // Budgets
                List<BUD_BID_BUDGET_NUM> budgetData;
                using (Entities context = new Entities())
                {
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_BUDGET_NUM budgetField in budgetData)
                {
                    budgetField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    budgetField.DETAIL_TASK_ID = newDetailTaskID;
                    budgetField.MODIFIED_BY = "TEMP";
                    budgetField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_BUDGET_NUM>(budgetData);

                // Detail sheets
                List<BUD_BID_DETAIL_SHEET> sheetData;
                using (Entities context = new Entities())
                {
                    sheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_DETAIL_SHEET detailField in sheetData)
                {
                    detailField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    detailField.DETAIL_TASK_ID = newDetailTaskID;
                    detailField.MODIFIED_BY = "TEMP";
                    detailField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(sheetData);
            }

            return Convert.ToInt64(newBudBidID);
        }
        public static List<SingleCombo> LoadedWEDates(long hierarchyId, bool optionalsortDescending = false, long optionalNumOfReturnRecords = long.MaxValue)
        {
            string sortOrder = optionalsortDescending == false ? sortOrder = "ASC" : sortOrder = "DESC";
            string sql = string.Format(@"
                SELECT TO_CHAR(JC_WK_DATE,'DD-Mon-YYYY') ID_NAME
                FROM (SELECT DISTINCT JC_WK_DATE FROM APPS.XX_JOBCOST_DATES_MV WHERE HIERARCHY_ID = {0} ORDER BY JC_WK_DATE {1}) JC_DATES
                WHERE ROWNUM <= {2}", hierarchyId, sortOrder, optionalNumOfReturnRecords);

            List<SingleCombo> data;
            using (Entities context = new Entities())
            {
                data = context.Database.SqlQuery<SingleCombo>(sql).ToList();
            }

            return data;
        }
        public static long CountAllProjects(long orgID, long yearID, long verID)
        {
            using (Entities context = new Entities())
            {
                return context.BUD_BID_PROJECTS.Where(x => x.ORG_ID == orgID && x.YEAR_ID == yearID && x.VER_ID == verID && x.MODIFIED_BY != "TEMP").Count();
            }
        }
        public static string BudBidVerToOHBudVer(long verID)
        {
            switch (verID)
            {
                case 1:  // Bid
                    return "";  //  FIX???

                case 2:  // First Draft
                    return "1ST DRAFT_11";

                case 3:  // Final Draft
                    return "FINAL DRAFT_11";

                case 4:  // 1st Reforecast
                    return "1ST REFORE_11";

                case 5:  // 2nd Reforecast
                    return "2ND REFORE_11";

                case 6:  // 3rd Reforecast
                    return "3RD REFORE_11";

                case 7:  // 4th Reforecast
                    return "4TH REFORE_11";

                default:
                    return "";
            }
        }
        public static long InventoryOrg(long orgID)
        {
            string profileValue;
            using (Entities context = new Entities())
            {
                profileValue = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 50 && x.ORGANIZATION_ID == orgID).Select(x => x.PROFILE_VALUE).SingleOrDefault();
            }

            
            if (profileValue == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(profileValue);
            }
        }
        public static long EquipmentOrg(long orgID)
        {
            string profileValue;
            using (Entities context = new Entities())
            {
                profileValue = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == 51 && x.ORGANIZATION_ID == orgID).Select(x => x.PROFILE_VALUE).SingleOrDefault();
            }


            if (profileValue == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(profileValue);
            }
        }        
    }

    public class BBSummaryRollup
    {
        public class Grid
        {
            #region Fields
            public class Fields
            {
                public string NAME { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
                public decimal OP_PERC { get; set; }
                public decimal OH { get; set; }
                public decimal NET_CONT { get; set; }
                public decimal OP_VAR { get; set; }
                public decimal NET_CONT_VAR { get; set; }
            }
            #endregion

            public static List<Fields> ORIGData(long userID, long hierarchyID, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string ohVersion = BB.BudBidVerToOHBudVer(verID);
                List<long> summaryOrgs = BB.OverallSummaryBudgetOrgsBelowCurrent(userID, hierarchyID, orgID);
                List<Fields> lineDetail = new List<Fields>();
                foreach (long summaryOrg in summaryOrgs)
                {
                    string orgName = BB.OrgName(summaryOrg);

                    string sql1 = string.Format(@"                          
                        WITH
                            ORG_NAME AS(
                                SELECT ORGANIZATION_ID, NAME
                                FROM APPS.HR_ALL_ORGANIZATION_UNITS      
                            ),
                            BUDGET_LINE_AMOUNTS AS (
                                SELECT * FROM (           
                                    SELECT BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV, BUD_BID_PROJECTS.ORG_ID
                                    FROM BUD_BID_PROJECTS
                                    LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                    LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                    WHERE BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                                PIVOT(
                                    SUM(NOV) FOR (LINE_ID)
                                    IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                            ),
                            PREV_OP AS (                         
                                SELECT BUD_BID_PROJECTS.ORG_ID, SUM(NOV) PREV_OP                                         
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.YEAR_ID = {3} AND BUD_BID_PROJECTS.VER_ID = {4} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10                             
                                GROUP BY BUD_BID_PROJECTS.ORG_ID         
                            ),
                            OVERHEAD AS (
                                SELECT OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID,
                                    NVL(SUM(AMOUNT), 0) OH
                                FROM OVERHEAD_ORG_BUDGETS
                                LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                                LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                                WHERE OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {1} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{5}'
                                GROUP BY OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID
                            ),
                            PREV_OVERHEAD AS (
                                SELECT OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID PREV_ORG_BUDGET_ID,
                                    NVL(SUM(AMOUNT), 0) PREV_OH
                                FROM OVERHEAD_ORG_BUDGETS
                                LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                                LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                                WHERE OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {3} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{5}'
                                GROUP BY OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID
                            )
                            SELECT '{0}' NAME,
                                SUM(NVL(GROSS_REC, 0)) GROSS_REC,                            
                                SUM(NVL(MAT_USAGE, 0)) MAT_USAGE,
                                SUM(NVL(GROSS_REV, 0)) GROSS_REV,
                                SUM(NVL(DIR_EXP, 0)) DIR_EXP,
                                SUM(NVL(OP, 0)) OP,
                                CASE WHEN SUM(GROSS_REV) = 0 OR SUM(GROSS_REV) IS NULL THEN 0 ELSE ROUND((SUM(OP)/SUM(GROSS_REV)) * 100, 2) END OP_PERC,
                                SUM(NVL(OH, 0)) OH,
                                SUM(NVL(OP, 0)) - SUM(NVL(OH, 0)) NET_CONT,                              
                                SUM(NVL(OP, 0)) - SUM(NVL(PREV_OP.PREV_OP, 0)) OP_VAR,
                                (SUM(NVL(OP, 0)) - SUM(NVL(OH, 0))) - (SUM(NVL(PREV_OP.PREV_OP, 0))) - SUM(NVL(PREV_OH, 0)) NET_CONT_VAR                 
                            FROM ORG_NAME
                            LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON ORG_NAME.ORGANIZATION_ID = BUDGET_LINE_AMOUNTS.ORG_ID
                            LEFT OUTER JOIN PREV_OP ON ORG_NAME.ORGANIZATION_ID = PREV_OP.ORG_ID
                            LEFT OUTER JOIN OVERHEAD ON ORG_NAME.ORGANIZATION_ID = OVERHEAD.ORG_BUDGET_ID
                            LEFT OUTER JOIN PREV_OVERHEAD ON ORG_NAME.ORGANIZATION_ID = PREV_OVERHEAD.PREV_ORG_BUDGET_ID", orgName, yearID, verID, prevYearID, prevVerID, ohVersion);                         
                    
                    List<long> whereOrgs = HR.ActiveOrganizationsByHierarchy(hierarchyID, summaryOrg).Select(x => x.ORGANIZATION_ID).ToList();
                    string sql2 = "";
                    if (whereOrgs.Count() == 0)
                    {
                        if (BB.IsUserOrgAndAllowed(userID, summaryOrg) == true)
                        {
                            sql2 = " WHERE ORGANIZATION_ID = " + summaryOrg;
                        }
                        else
                        {
                            sql2 = " WHERE ORGANIZATION_ID = 0";
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (long org in whereOrgs)
                        {
                            if (BB.IsUserOrgAndAllowed(userID, org) == true)
                            {
                                if (i == 0)
                                {
                                    sql2 = " WHERE ORGANIZATION_ID = " + org;
                                    i = 1;
                                }
                                else
                                {
                                    sql2 = sql2 + " OR ORGANIZATION_ID = " + org;
                                }
                            }
                        }
                    }
                    string sql3 = " ORDER BY NAME";
                    string sql = sql1 + sql2 + sql3;

                    List<Fields> lineData;
                    using (Entities context = new Entities())
                    {
                        lineData = context.Database.SqlQuery<Fields>(sql).ToList();
                    }                    
                    
                    lineDetail.AddRange(lineData);
                }

                return lineDetail;
            }

            public static List<Fields> Data(long userID, long hierarchyID, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string ohVersion = BB.BudBidVerToOHBudVer(verID);
                List<long> summaryOrgs = BB.OverallSummaryBudgetOrgsBelowCurrent(userID, hierarchyID, orgID);
                List<Fields> lineDetail = new List<Fields>();
                foreach (long summaryOrg in summaryOrgs)
                {
                    string orgName = BB.OrgName(summaryOrg);

                    string sql1 = string.Format(@"                          
                        WITH
                            ORG_NAME AS(
                                SELECT ORGANIZATION_ID, NAME
                                FROM APPS.HR_ALL_ORGANIZATION_UNITS      
                            ),
                            BUDGET_LINE_AMOUNTS AS (
                                SELECT * FROM (           
                                    SELECT BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV, BUD_BID_PROJECTS.ORG_ID
                                    FROM BUD_BID_PROJECTS
                                    LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                    LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                    WHERE BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                                PIVOT(
                                    SUM(NOV) FOR (LINE_ID)
                                    IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                            ),
                            PREV_OP AS (                         
                                SELECT BUD_BID_PROJECTS.ORG_ID, SUM(NOV) PREV_OP                                         
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.YEAR_ID = {3} AND BUD_BID_PROJECTS.VER_ID = {4} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10                             
                                GROUP BY BUD_BID_PROJECTS.ORG_ID         
                            ),
                            OVERHEAD AS (
                                SELECT OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID,
                                    NVL(SUM(AMOUNT), 0) OH
                                FROM OVERHEAD_ORG_BUDGETS
                                LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                                LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                                WHERE OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {1} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{5}'
                                GROUP BY OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID
                            ),
                            PREV_OVERHEAD AS (
                                SELECT OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID PREV_ORG_BUDGET_ID,
                                    NVL(SUM(AMOUNT), 0) PREV_OH
                                FROM OVERHEAD_ORG_BUDGETS
                                LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                                LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                                WHERE OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {3} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{5}'
                                GROUP BY OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID
                            )
                            SELECT '{0}' NAME,
                                SUM(NVL(GROSS_REC, 0)) GROSS_REC,                            
                                SUM(NVL(MAT_USAGE, 0)) MAT_USAGE,
                                SUM(NVL(GROSS_REV, 0)) GROSS_REV,
                                SUM(NVL(DIR_EXP, 0)) DIR_EXP,
                                SUM(NVL(OP, 0)) OP,
                                CASE WHEN SUM(GROSS_REV) = 0 OR SUM(GROSS_REV) IS NULL THEN 0 ELSE ROUND((SUM(OP)/SUM(GROSS_REV)) * 100, 2) END OP_PERC,
                                SUM(NVL(OH, 0)) OH,
                                SUM(NVL(OP, 0)) - SUM(NVL(OH, 0)) NET_CONT,                              
                                SUM(NVL(OP, 0)) - SUM(NVL(PREV_OP.PREV_OP, 0)) OP_VAR,
                                (SUM(NVL(OP, 0)) - SUM(NVL(OH, 0))) - (SUM(NVL(PREV_OP.PREV_OP, 0))) - SUM(NVL(PREV_OH, 0)) NET_CONT_VAR                 
                            FROM ORG_NAME
                            LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON ORG_NAME.ORGANIZATION_ID = BUDGET_LINE_AMOUNTS.ORG_ID
                            LEFT OUTER JOIN PREV_OP ON ORG_NAME.ORGANIZATION_ID = PREV_OP.ORG_ID
                            LEFT OUTER JOIN OVERHEAD ON ORG_NAME.ORGANIZATION_ID = OVERHEAD.ORG_BUDGET_ID
                            LEFT OUTER JOIN PREV_OVERHEAD ON ORG_NAME.ORGANIZATION_ID = PREV_OVERHEAD.PREV_ORG_BUDGET_ID", orgName, yearID, verID, prevYearID, prevVerID, ohVersion);

                    List<long> whereOrgs = HR.ActiveOrganizationsByHierarchy(hierarchyID, summaryOrg).Select(x => x.ORGANIZATION_ID).ToList();
                    string sql2 = " WHERE ORGANIZATION_ID = 0";
                    if (whereOrgs.Count() == 0)
                    {
                        if (BB.IsUserOrgAndAllowed(userID, summaryOrg) == true)
                        {
                            sql2 = sql2 + " OR ORGANIZATION_ID = " + summaryOrg;
                        }
                    }
                    else
                    {
                        foreach (long org in whereOrgs)
                        {
                            if (BB.IsUserOrgAndAllowed(userID, org) == true)
                            {
                                sql2 = sql2 + " OR ORGANIZATION_ID = " + org;                                
                            }
                        }
                    }
                    string sql3 = " ORDER BY NAME";
                    string sql = sql1 + sql2 + sql3;

                    List<Fields> lineData;
                    using (Entities context = new Entities())
                    {
                        lineData = context.Database.SqlQuery<Fields>(sql).ToList();
                    }

                    lineDetail.AddRange(lineData);
                }

                return lineDetail;
            }
        }

        public class Subtotals
        {
            #region Fields
            public class Fields
            {
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
                public decimal OP_PERC { get; set; }
                public decimal OH { get; set; }
                public decimal NET_CONT { get; set; }
                public decimal OP_VAR { get; set; }
                public decimal NET_CONT_VAR { get; set; }
            }
            #endregion 

            public static Fields Data(long userID, long hierarchyID, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string ohVersion = BB.BudBidVerToOHBudVer(verID);
                string sql1 = string.Format(@"
                    WITH
                        ORG_NAME AS(
                            SELECT ORGANIZATION_ID, NAME
                            FROM APPS.HR_ALL_ORGANIZATION_UNITS      
                        ),
                        BUDGET_LINE_AMOUNTS AS (
                            SELECT * FROM (           
                                SELECT BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV, BUD_BID_PROJECTS.ORG_ID
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.YEAR_ID = {0} AND BUD_BID_PROJECTS.VER_ID = {1} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                            PIVOT(
                                SUM(NOV) FOR (LINE_ID)
                                IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                        ),
                        PREV_OP AS (                        
                            SELECT BUD_BID_PROJECTS.ORG_ID, SUM(NOV) PREV_OP                                         
                            FROM BUD_BID_PROJECTS
                            LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                            LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                            WHERE BUD_BID_PROJECTS.YEAR_ID = {2} AND BUD_BID_PROJECTS.VER_ID = {3} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10
                            GROUP BY BUD_BID_PROJECTS.ORG_ID 
                        ),
                        OVERHEAD AS (
                            SELECT OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID,
                                NVL(SUM(AMOUNT), 0) OH
                            FROM OVERHEAD_ORG_BUDGETS
                            LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                            LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                            WHERE OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {0} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{4}'
                            GROUP BY OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID
                        ),
                        PREV_OVERHEAD AS (
                            SELECT OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID PREV_ORG_BUDGET_ID,
                                NVL(SUM(AMOUNT), 0) PREV_OH
                            FROM OVERHEAD_ORG_BUDGETS
                            LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                            LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                            WHERE OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {2} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{4}'
                            GROUP BY OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID
                        )
                        SELECT SUM(NVL(GROSS_REC, 0)) GROSS_REC,                            
                            SUM(NVL(MAT_USAGE, 0)) MAT_USAGE,
                            SUM(NVL(GROSS_REV, 0)) GROSS_REV,
                            SUM(NVL(DIR_EXP, 0)) DIR_EXP,
                            SUM(NVL(OP, 0)) OP,
                            CASE WHEN SUM(GROSS_REV) = 0 OR SUM(GROSS_REV) IS NULL THEN 0 ELSE ROUND((SUM(OP)/SUM(GROSS_REV)) * 100, 2) END OP_PERC,
                            SUM(NVL(OH, 0)) OH,
                            SUM(NVL(OP, 0)) - SUM(NVL(OH, 0)) NET_CONT,                                
                            SUM(NVL(OP, 0)) - SUM(NVL(PREV_OP.PREV_OP, 0)) OP_VAR,
                            (SUM(NVL(OP, 0)) - SUM(NVL(OH, 0))) - (SUM(NVL(PREV_OP.PREV_OP, 0))) - SUM(NVL(PREV_OH, 0)) NET_CONT_VAR                          
                        FROM ORG_NAME
                        LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON ORG_NAME.ORGANIZATION_ID = BUDGET_LINE_AMOUNTS.ORG_ID
                        LEFT OUTER JOIN PREV_OP ON ORG_NAME.ORGANIZATION_ID = PREV_OP.ORG_ID
                        LEFT OUTER JOIN OVERHEAD ON ORG_NAME.ORGANIZATION_ID = OVERHEAD.ORG_BUDGET_ID
                        LEFT OUTER JOIN PREV_OVERHEAD ON ORG_NAME.ORGANIZATION_ID = PREV_OVERHEAD.PREV_ORG_BUDGET_ID", yearID, verID, prevYearID, prevVerID, ohVersion);

                List<long> summaryOrgs = BB.OverallSummaryBudgetOrgsBelowCurrent(userID, hierarchyID, orgID);
                string sql2 = " WHERE ORGANIZATION_ID = 0";
                foreach (long summaryOrg in summaryOrgs)
                {
                    List<long> whereOrgs = HR.ActiveOrganizationsByHierarchy(hierarchyID, summaryOrg).Select(x => x.ORGANIZATION_ID).ToList();
                    if (whereOrgs.Count() == 0)
                    {
                        if (BB.IsUserOrgAndAllowed(userID, summaryOrg) == true)
                        {
                            sql2 = sql2 + " OR ORGANIZATION_ID = " + summaryOrg;
                        }
                    }
                    else
                    {
                        foreach (long org in whereOrgs)
                        {
                            if (BB.IsUserOrgAndAllowed(userID, org) == true)
                            {
                                sql2 = sql2 + " OR ORGANIZATION_ID = " + org;
                            }
                        }
                    }
                }

                string sql = sql1 + sql2;
                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }  
            }
        } 
    }

    public class BBSummary
    {
        public static void DBUpdateAllJCNums(string hierID, long orgID, long leOrgID, long yearID, string jcDate)
        {
            // Get org's project list
            List<BUD_BID_PROJECTS> data;
            using (Entities context = new Entities())
            {
                data = context.BUD_BID_PROJECTS.Where(x => x.ORG_ID == orgID && (x.STATUS_ID != 45 || x.WE_DATE != null)).ToList();
            }          
            
            long budBidProjectID;
            string projectID;            
            decimal sGrossRec;
            decimal sMaterial;
            decimal sGrossRev;
            decimal sDirects;
            decimal sOP;
        
            // Do for each project
            foreach (BUD_BID_PROJECTS field in data)
            {
                budBidProjectID = Convert.ToInt64(field.BUD_BID_PROJECTS_ID);
                projectID = field.PROJECT_ID;
                string type = field.TYPE;

                // Get new start nums
                XXDBI_DW.JOB_COST_V jcLine = null;
                bool proceed = false;
                switch (type)
                {
                    case "":
                        proceed = false;
                        break;

                    case "OVERRIDE":
                        proceed = false;
                        break;

                    case "ORG":
                        jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64(hierID), Convert.ToInt64(projectID), jcDate);
                        proceed = true;
                        break;

                    case "PROJECT":
                        jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64(projectID), jcDate);
                        proceed = true;
                        break;

                    case "ROLLUP":
                        jcLine = XXDBI_DW.JCSummaryLineAmounts(Convert.ToInt64(orgID), projectID, jcDate);
                        proceed = true;
                        break;
                }

                if (proceed == true)
                {
                    sGrossRec = jcLine.FY_GREC;
                    sMaterial = jcLine.FY_MU;
                    sGrossRev = jcLine.FY_GREV;
                    sDirects = jcLine.FY_TDE;
                    sOP = jcLine.FY_TOP;

                    // Update jc date
                    BBProject.WEDate.Update(budBidProjectID, jcDate);
                    
                    // Update start nums
                    BBProject.StartNumbers.DBUpdate(budBidProjectID, sGrossRec, sMaterial, sGrossRev, sDirects, sOP);

                    // Update end nums
                    BBDetail.Sheets.EndNumbers.DBCalculate(leOrgID, yearID, budBidProjectID, sGrossRec, sMaterial, sGrossRev, sDirects, sOP);
                }
            }
        }

        public class Grid
        {
            #region Fields
            public class Fields
            {
                public long BUD_BID_PROJECTS_ID { get; set; }
                public string PROJECT_ID { get; set; }
                public string PROJECT_NUM { get; set; }
                public string TYPE { get; set; }
                public string PROJECT_NAME { get; set; }
                public string STATUS { get; set; }
                public decimal ACRES { get; set; }
                public decimal DAYS { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
                public decimal PREV_OP { get; set; }
                public decimal MAT_PERC { get; set; }
                public decimal GR_PERC { get; set; }
                public decimal DIRECTS_PERC { get; set; }
                public decimal OP_PERC { get; set; }
                public decimal OP_VAR { get; set; }
                public string COMPARE_PRJ_OVERRIDE { get; set; }
                public string WE_OVERRIDE { get; set; }
            }
            #endregion

            public static List<Fields> Data(string orgName, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string sql = string.Format(@"
                    WITH
                        CUR_PROJECT_INFO_WITH_STATUS AS(
                            SELECT BUD_BID_STATUS.STATUS_ID, BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.TYPE, BUD_BID_PROJECTS.PRJ_NAME, BUD_BID_STATUS.STATUS,
                                BUD_BID_PROJECTS.ACRES, BUD_BID_PROJECTS.DAYS, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT, BUD_BID_PROJECTS.WE_OVERRIDE 
                            FROM BUD_BID_PROJECTS
                            INNER JOIN BUD_BID_STATUS
                            ON BUD_BID_PROJECTS.STATUS_ID = BUD_BID_STATUS.STATUS_ID
                            WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {2} AND BUD_BID_PROJECTS.VER_ID = {3} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP'
                        ),     
                        ORACLE_PROJECT_NAMES AS (
                            SELECT '{1}' AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '{0} (Org)' AS PROJECT_NAME, 'ORG' AS TYPE
                            FROM DUAL
                                UNION ALL
                            SELECT CAST(PROJECTS_V.PROJECT_ID AS varchar(20)) AS PROJECT_ID, PROJECTS_V.SEGMENT1 AS PROJECT_NUM, PROJECTS_V.LONG_NAME AS PROJECT_NAME, 'PROJECT' AS TYPE
                            FROM PROJECTS_V
                            LEFT JOIN PA.PA_PROJECT_CLASSES
                            ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                            WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                                UNION ALL
                            SELECT CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_ID, 'N/A' AS PROJECT_NUM, CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_NAME, 'ROLLUP' AS TYPE
                            FROM PROJECTS_V
                            LEFT JOIN PA.PA_PROJECT_CLASSES
                            ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                            WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup'
                            AND PA.PA_PROJECT_CLASSES.CLASS_CODE <> 'None' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                            GROUP BY CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) 
                        ),
                        BUDGET_LINE_AMOUNTS AS (
                            SELECT * FROM (           
                                SELECT BUD_BID_BUDGET_NUM.PROJECT_ID, BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {2} AND BUD_BID_PROJECTS.VER_ID = {3} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                            PIVOT(
                                SUM(NOV) FOR (LINE_ID)
                                IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                        ),
                        PREV_OP AS (                        
                            SELECT BUD_BID_PROJECTS.PROJECT_ID, NOV PREV_OP  
                            FROM BUD_BID_PROJECTS
                            LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                            LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                            WHERE BUD_BID_PROJECTS.ORG_ID = {1} AND BUD_BID_PROJECTS.YEAR_ID = {4} AND BUD_BID_PROJECTS.VER_ID = {5} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10        
                        ) 
                    SELECT CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.STATUS_ID = 45 THEN 'Z' ELSE 'A' END PROJECT_SORT,
                        CUR_PROJECT_INFO_WITH_STATUS.BUD_BID_PROJECTS_ID,
                        CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID, 
                        CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'OVERRIDE' THEN '-- OVERRIDE --'
                            WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'ORG' THEN 'N/A'
                            WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'ROLLUP' THEN 'N/A'
                            ELSE ORACLE_PROJECT_NAMES.PROJECT_NUM END PROJECT_NUM,                       
                        CUR_PROJECT_INFO_WITH_STATUS.TYPE,
                        CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.TYPE = 'OVERRIDE' THEN CUR_PROJECT_INFO_WITH_STATUS.PRJ_NAME ELSE ORACLE_PROJECT_NAMES.PROJECT_NAME END PROJECT_NAME,
                        CUR_PROJECT_INFO_WITH_STATUS.STATUS, 
                        CUR_PROJECT_INFO_WITH_STATUS.ACRES, 
                        CUR_PROJECT_INFO_WITH_STATUS.DAYS,
                        BUDGET_LINE_AMOUNTS.GROSS_REC,
                        BUDGET_LINE_AMOUNTS.MAT_USAGE,
                        BUDGET_LINE_AMOUNTS.GROSS_REV,
                        BUDGET_LINE_AMOUNTS.DIR_EXP,
                        BUDGET_LINE_AMOUNTS.OP,                        
                        CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_AMOUNT ELSE (CASE WHEN PREV_OP.PREV_OP IS NULL THEN 0 ELSE PREV_OP.PREV_OP END) END PREV_OP,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REC = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.MAT_USAGE/BUDGET_LINE_AMOUNTS.GROSS_REC) * 100, 2) END MAT_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.GROSS_REC/BUDGET_LINE_AMOUNTS.GROSS_REV) * 100, 2) END GR_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.DIR_EXP/BUDGET_LINE_AMOUNTS.GROSS_REV) * 100, 2) END DIRECTS_PERC,
                        CASE WHEN BUDGET_LINE_AMOUNTS.GROSS_REV = 0 THEN 0 ELSE ROUND((BUDGET_LINE_AMOUNTS.OP/BUDGET_LINE_AMOUNTS.GROSS_REV) * 100, 2) END OP_PERC,       
                        BUDGET_LINE_AMOUNTS.OP - (CASE WHEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_AMOUNT ELSE (CASE WHEN PREV_OP.PREV_OP IS NULL THEN 0 ELSE PREV_OP.PREV_OP END) END) OP_VAR,
CUR_PROJECT_INFO_WITH_STATUS.COMPARE_PRJ_OVERRIDE,
CUR_PROJECT_INFO_WITH_STATUS.WE_OVERRIDE
                    FROM CUR_PROJECT_INFO_WITH_STATUS
                    LEFT OUTER JOIN ORACLE_PROJECT_NAMES ON CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID = ORACLE_PROJECT_NAMES.PROJECT_ID AND CUR_PROJECT_INFO_WITH_STATUS.TYPE = ORACLE_PROJECT_NAMES.TYPE
                    LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON CUR_PROJECT_INFO_WITH_STATUS.BUD_BID_PROJECTS_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID
                    LEFT OUTER JOIN PREV_OP ON CUR_PROJECT_INFO_WITH_STATUS.PROJECT_ID = PREV_OP.PROJECT_ID
                    ORDER BY PROJECT_SORT, LOWER(PROJECT_NAME)", orgName, orgID, yearID, verID, prevYearID, prevVerID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).ToList();                    
                }
            }
        }

        public class Subtotals
        {
            #region Fields
            public class Fields
            {
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
                public decimal COMPARE_PRJ_AMOUNT { get; set; }
                public decimal PREV_OP { get; set; }
                public decimal OP_VAR { get; set; }
            }
            #endregion

            public static Fields Data(bool active, long orgID, long yearID, long verID, long prevYearID, long prevVerID)
            {
                string statusID = active == true ? "<>" : "=";
                string sql = string.Format(@"
                        WITH
                            CUR_PROJECT_INFO AS(
                                SELECT BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.STATUS_ID, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT
                                FROM BUD_BID_PROJECTS
                                WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP'
                            ),     
                            BUDGET_LINE_AMOUNTS AS (
                                SELECT * FROM (           
                                    SELECT BUD_BID_BUDGET_NUM.PROJECT_ID, BUD_BID_BUDGET_NUM.LINE_ID, BUD_BID_BUDGET_NUM.NOV
                                    FROM BUD_BID_PROJECTS
                                    LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                    LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                    WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')       
                                PIVOT(
                                    SUM(NOV) FOR (LINE_ID)
                                    IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                            ),
                            PREV_OP AS (                       
                              SELECT BUD_BID_PROJECTS.PROJECT_ID, NOV PREV_OP  
                                FROM BUD_BID_PROJECTS
                                LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                                LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                                WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {3} AND BUD_BID_PROJECTS.VER_ID = {4} AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10        
                            )    
                            SELECT NVL(SUM(BUDGET_LINE_AMOUNTS.GROSS_REC),0) GROSS_REC,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.MAT_USAGE),0) MAT_USAGE,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.GROSS_REV),0) GROSS_REV,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.DIR_EXP),0) DIR_EXP,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.OP),0) OP,
                                NVL(SUM(CUR_PROJECT_INFO.COMPARE_PRJ_AMOUNT),0) COMPARE_PRJ_AMOUNT,     
                                NVL(SUM(CASE WHEN CUR_PROJECT_INFO.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO.COMPARE_PRJ_AMOUNT ELSE PREV_OP.PREV_OP END),0) PREV_OP,
                                NVL(SUM(BUDGET_LINE_AMOUNTS.OP),0) - NVL(SUM(CASE WHEN CUR_PROJECT_INFO.COMPARE_PRJ_OVERRIDE = 'Y' THEN CUR_PROJECT_INFO.COMPARE_PRJ_AMOUNT ELSE PREV_OP.PREV_OP END),0) OP_VAR 
                            FROM CUR_PROJECT_INFO
                            LEFT OUTER JOIN BUDGET_LINE_AMOUNTS ON CUR_PROJECT_INFO.BUD_BID_PROJECTS_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID
                            LEFT OUTER JOIN PREV_OP ON CUR_PROJECT_INFO.PROJECT_ID = PREV_OP.PROJECT_ID
                            WHERE CUR_PROJECT_INFO.STATUS_ID " + statusID + " 45", orgID, yearID, verID, prevYearID, prevVerID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        } 
    }
    
    public class BBAdjustments
    {
        #region Fields
        public class Fields
        {
            public long ADJ_ID { get; set; }
            public string ADJUSTMENT { get; set; }
            public decimal? MAT_ADJ { get; set; }
            public decimal? WEATHER_ADJ { get; set; }
        }
        #endregion

        public static long Count(long orgID, long yearID, long verID)
        {
            using (Entities context = new Entities())
            {
                return context.BUD_BID_ADJUSTMENT.Where(x => x.ORG_ID == orgID && x.YEAR_ID == yearID && x.VER_ID == verID).Count();
            }
        }
        public static void Create(long orgID, long yearID, long verID)
        {
            BUD_BID_ADJUSTMENT data = new BUD_BID_ADJUSTMENT();

            // Material
            data.ORG_ID = orgID;
            data.YEAR_ID = yearID;
            data.VER_ID = verID;
            data.MAT_ADJ = 0;
            data.WEATHER_ADJ = 12345678910;
            data.CREATE_DATE = DateTime.Now;
            data.CREATED_BY = HttpContext.Current.User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            data.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
            GenericData.Insert<BUD_BID_ADJUSTMENT>(data);

            // Weather
            data.MAT_ADJ = 12345678910;
            data.WEATHER_ADJ = 0;
            GenericData.Insert<BUD_BID_ADJUSTMENT>(data);
        }
        public static List<Fields> Data(long orgID, long yearID, long verID)
        {
            string sql = string.Format(@"
                SELECT ADJ_ID,
                    CASE WHEN MAT_ADJ <> 12345678910 THEN 'Material Adjustment' ELSE 'Weather Adjustment' END ADJUSTMENT,
                    12345678910 BLANKCOL1,
                    12345678910 BLANKCOL2,
                    12345678910 BLANKCOL3,
                    12345678910 BLANKCOL4,
                    MAT_ADJ,
                    12345678910 BLANKCOL5,
                    WEATHER_ADJ,                    
                    12345678910 BLANKCOL6,
                    12345678910 BLANKCOL7,
                    12345678910 BLANKCOL8
                FROM BUD_BID_ADJUSTMENT
                WHERE ORG_ID = {0} AND YEAR_ID = {1} AND VER_ID = {2}
                ORDER BY ADJUSTMENT", orgID, yearID, verID);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<Fields>(sql).ToList();
            }
        }

        public class Subtotal
        {
            #region Fields
            public class Fields
            {
                public decimal? MAT_ADJ { get; set; }
                public decimal? WEATHER_ADJ { get; set; }
            }
            #endregion

            public static Fields Data(long orgID, long yearID, long verID)
            {
                string sql = string.Format(@"
                    SELECT SUM(MAT_ADJ) - 12345678910 MAT_ADJ,
                        SUM(WEATHER_ADJ) - 12345678910 WEATHER_ADJ
                    FROM 
                    (
                        SELECT MAT_ADJ,
                            WEATHER_ADJ
                        FROM BUD_BID_ADJUSTMENT
                        WHERE ORG_ID = {0} AND YEAR_ID = {1} AND VER_ID = {2}
                    )", orgID, yearID, verID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }  
    }

    public class BBOH
    {
        #region Fields
        public class Fields
        {
            public string ADJUSTMENT { get; set; }
            public decimal OH { get; set; }
        }
        #endregion

        public static List<Fields> Data(long orgID, long yearID, long verID)
        {
            string ohVer = BB.BudBidVerToOHBudVer(verID);
            string sql = string.Format(@"
                SELECT 'Overhead' ADJUSTMENT,
                    NVL(SUM(AMOUNT), 0) OH
                FROM OVERHEAD_ORG_BUDGETS
                LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                WHERE OVERHEAD_ORG_BUDGETS.ORGANIZATION_ID = {0} AND OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {1} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{2}'", orgID, yearID, ohVer);

            using (Entities context = new Entities())
            {
                return context.Database.SqlQuery<Fields>(sql).ToList();
            }
        }

        public class Subtotal
        {
            #region Fields
            public class Fields
            {
                public string ADJUSTMENT { get; set; }
                public decimal OH { get; set; }
            }
            #endregion

            public static Fields Data(long orgID, long yearID, long verID)
            {
                string ohVer = BB.BudBidVerToOHBudVer(verID);
                string sql = string.Format(@"
                    SELECT 'Overhead' ADJUSTMENT,
                        NVL(SUM(AMOUNT), 0) OH
                    FROM OVERHEAD_ORG_BUDGETS
                    LEFT JOIN OVERHEAD_BUDGET_TYPE ON OVERHEAD_ORG_BUDGETS.OVERHEAD_BUDGET_TYPE_ID = OVERHEAD_BUDGET_TYPE.OVERHEAD_BUDGET_TYPE_ID
                    LEFT JOIN OVERHEAD_BUDGET_DETAIL ON OVERHEAD_ORG_BUDGETS.ORG_BUDGET_ID = OVERHEAD_BUDGET_DETAIL.ORG_BUDGET_ID 
                    WHERE OVERHEAD_ORG_BUDGETS.ORGANIZATION_ID = {0} AND OVERHEAD_ORG_BUDGETS.FISCAL_YEAR = {1} AND OVERHEAD_BUDGET_TYPE.BUDGET_NAME = '{2}'", orgID, yearID, ohVer);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }  
    }

    public class BBProject
    {
        public static long Count(long orgID, long yearID, long verID, string projectID, long curBudBidID)
        {
            using (Entities context = new Entities())
            {
                return context.BUD_BID_PROJECTS.Where(x => x.MODIFIED_BY != "TEMP" && x.BUD_BID_PROJECTS_ID != curBudBidID && x.ORG_ID == orgID && x.YEAR_ID == yearID && x.VER_ID == verID && x.PROJECT_ID == projectID).Count();
            }
        }
        public static void DBDelete(long budBidID)
        {
            BUD_BID_PROJECTS projectData;
            List<BUD_BID_ACTUAL_NUM> actualData;
            List<BUD_BID_BUDGET_NUM> budgetData;
            List<BUD_BID_DETAIL_SHEET> detailSheetData;
            List<BUD_BID_DETAIL_TASK> taskInfoData;

            using (Entities context = new Entities())
            {
                projectData = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
                actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID).ToList();
                detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID).ToList();
                taskInfoData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }

            GenericData.Delete<BUD_BID_PROJECTS>(projectData);
            GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
            GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);
            GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
            GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData);
        }
        public static long DBCopy(long budBidID, string origProjectName)
        {
            // BBProject
            BUD_BID_PROJECTS projectData;
            using (Entities context = new Entities())
            {
                projectData = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidID).Single();
            }
            projectData.PROJECT_ID = DateTime.Now.ToString("yyMMddHHmmss");
            projectData.WE_OVERRIDE = "Y";
            projectData.WE_DATE = null;
            projectData.PRJ_NAME = "* " + (origProjectName.Length >= 20 ? origProjectName.Substring(0, 20) : origProjectName) + " (COPIED: " + DateTime.Now + ")";
            projectData.TYPE = "OVERRIDE";
            projectData.CREATE_DATE = DateTime.Now;
            projectData.CREATED_BY = HttpContext.Current.User.Identity.Name;
            projectData.MODIFY_DATE = DateTime.Now;
            projectData.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
            GenericData.Insert<BUD_BID_PROJECTS>(projectData);
            decimal newBudBidID = projectData.BUD_BID_PROJECTS_ID;

            // Tasks
            List<BUD_BID_DETAIL_TASK> taskData;
            using (Entities context = new Entities())
            {
                taskData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidID).ToList();
            }
            decimal detailTaskID;
            decimal newDetailTaskID;
            foreach (BUD_BID_DETAIL_TASK taskField in taskData)
            {
                detailTaskID = taskField.DETAIL_TASK_ID;
                taskField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                taskField.CREATE_DATE = DateTime.Now;
                taskField.CREATED_BY = HttpContext.Current.User.Identity.Name;
                taskField.MODIFY_DATE = DateTime.Now;
                taskField.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                GenericData.Insert<BUD_BID_DETAIL_TASK>(taskData.Where(x => x.DETAIL_TASK_ID == detailTaskID).Single());
                newDetailTaskID = taskField.DETAIL_TASK_ID;

                // Actuals
                List<BUD_BID_ACTUAL_NUM> actualData;
                using (Entities context = new Entities())
                {
                    actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }

                foreach (BUD_BID_ACTUAL_NUM actualField in actualData)
                {
                    actualField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    actualField.DETAIL_TASK_ID = newDetailTaskID;
                    actualField.CREATE_DATE = DateTime.Now;
                    actualField.CREATED_BY = HttpContext.Current.User.Identity.Name;
                    actualField.MODIFY_DATE = DateTime.Now;
                    actualField.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                }
                GenericData.Insert<BUD_BID_ACTUAL_NUM>(actualData);

                // Budgets
                List<BUD_BID_BUDGET_NUM> budgetData;
                using (Entities context = new Entities())
                {
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_BUDGET_NUM budgetField in budgetData)
                {
                    budgetField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    budgetField.DETAIL_TASK_ID = newDetailTaskID;
                    budgetField.CREATE_DATE = DateTime.Now;
                    budgetField.CREATED_BY = HttpContext.Current.User.Identity.Name;
                    budgetField.MODIFY_DATE = DateTime.Now;
                    budgetField.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                }
                GenericData.Insert<BUD_BID_BUDGET_NUM>(budgetData);

                // Detail sheets
                List<BUD_BID_DETAIL_SHEET> sheetData;
                using (Entities context = new Entities())
                {
                    sheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_DETAIL_SHEET detailField in sheetData)
                {
                    detailField.PROJECT_ID = Convert.ToInt64(newBudBidID);
                    detailField.DETAIL_TASK_ID = newDetailTaskID;
                    detailField.CREATE_DATE = DateTime.Now;
                    detailField.CREATED_BY = HttpContext.Current.User.Identity.Name;
                    detailField.MODIFY_DATE = DateTime.Now;
                    detailField.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                }
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(sheetData);
            }

            return Convert.ToInt64(newBudBidID);
        }
        public static List<DoubleComboLongID> Statuses()
        {
            string sql = "SELECT STATUS_ID ID, STATUS ID_NAME FROM BUD_BID_STATUS ORDER BY STATUS";

            using (Entities context = new Entities())
            {     
                return context.Database.SqlQuery<DoubleComboLongID>(sql).ToList();
            }
        }

        public class StartNumbers
        {
            #region Fields
            public class Fields
            {
                public long PROJECT_ID { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static Fields Data(long projectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_ACTUAL_NUM.PROJECT_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_ACTUAL_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_ACTUAL_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'  )
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", projectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
            public static void DBUpdate(long budBidProjectID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP)
            {
                string sql = string.Format(@"
                    SELECT *
                    FROM BUD_BID_DETAIL_TASK
                    LEFT JOIN BUD_BID_ACTUAL_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_ACTUAL_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_ACTUAL_NUM.DETAIL_TASK_ID
                    WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
                    ORDER BY LINE_ID", budBidProjectID);
                long[] lineNum = { 6, 7, 8, 9, 10 };
                decimal[] startNum = { sGrossRec, sMaterial, sGrossRev, sDirects, sOP };

                List<BUD_BID_ACTUAL_NUM> data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<BUD_BID_ACTUAL_NUM>(sql).OrderBy(x => x.LINE_ID).ToList();
                }

                int a = 0;
                foreach (BUD_BID_ACTUAL_NUM field in data)
                {
                    field.NOV = startNum[a];
                    field.MODIFY_DATE = DateTime.Now;
                    field.MODIFIED_BY = HttpContext.Current.User.Identity.Name;
                    a++;
                }

                GenericData.Update<BUD_BID_ACTUAL_NUM>(data);
            }
        }

        public class EndNumbers
        {
            #region Fields
            public class Fields
            {
                public long PROJECT_ID { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static Fields Data(long projectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_BUDGET_NUM.PROJECT_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'  )
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", projectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }  

        public class EndNumbersW0
        {
            #region Fields
            public class Fields
            {
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static Fields Data(long budBidProjectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_DETAIL_TASK.PROJECT_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT')
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", budBidProjectID);

                Fields data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }

                if (data == null)
                {
                    Fields nullData = new Fields();
                    nullData.GROSS_REC = 0;
                    nullData.MAT_USAGE = 0;
                    nullData.GROSS_REV = 0;
                    nullData.DIR_EXP = 0;
                    nullData.OP = 0;
                    data = nullData;
                }

                return data;
            }
            public static void DBUpdate(long budBidProjectID, decimal eGrossRec, decimal eMaterial, decimal eGrossRev, decimal eDirects, decimal eOP)
            {
                string sql = string.Format(@"
                    SELECT *
                    FROM BUD_BID_DETAIL_TASK
                    LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                    WHERE BUD_BID_DETAIL_TASK.PROJECT_ID = {0} AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT'
                    ORDER BY LINE_ID", budBidProjectID);
                long[] lineNum = { 6, 7, 8, 9, 10 };
                decimal[] endNum = { eGrossRec, eMaterial, eGrossRev, eDirects, eOP };

                List<BUD_BID_BUDGET_NUM> data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<BUD_BID_BUDGET_NUM>(sql).OrderBy(x => x.LINE_ID).ToList();
                }

                int a = 0;
                foreach (BUD_BID_BUDGET_NUM field in data)
                {
                    field.NOV = endNum[a];
                    a++;
                }

                GenericData.Update<BUD_BID_BUDGET_NUM>(data);
            }
        }

        public class Listing
        {
            #region Fields
            public class Fields
            {
                public string PROJECT_ID { get; set; }
                public string PROJECT_NUM { get; set; }
                public string PROJECT_NAME { get; set; }
                public string TYPE { get; set; }
                public string ORDERKEY { get; set; }
            }
            #endregion

            public static List<Fields> Data(long orgID, string orgName)
            {
                string sql = string.Format(@"
                    SELECT TO_CHAR(SYSDATE, 'YYMMDDHH24MISS') AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '-- OVERRIDE --' AS PROJECT_NAME, 'OVERRIDE' AS TYPE, 'ID1' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT '{1}' AS PROJECT_ID, 'N/A' AS PROJECT_NUM, '{0} (Org)' AS PROJECT_NAME, 'ORG' AS TYPE, 'ID2' AS ORDERKEY
                    FROM DUAL
                        UNION ALL
                    SELECT CAST(PROJECTS_V.PROJECT_ID AS VARCHAR(20)) AS PROJECT_ID, PROJECTS_V.SEGMENT1 AS PROJECT_NUM, PROJECTS_V.LONG_NAME AS PROJECT_NAME, 'PROJECT' AS TYPE, 'ID3' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN PA.PA_PROJECT_CLASSES
                    ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                        UNION ALL
                    SELECT CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_ID, 'N/A' AS PROJECT_NUM, CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) AS PROJECT_NAME, 'ROLLUP' AS TYPE, 'ID4' AS ORDERKEY
                    FROM PROJECTS_V
                    LEFT JOIN PA.PA_PROJECT_CLASSES
                    ON PROJECTS_V.PROJECT_ID = PA.PA_PROJECT_CLASSES.PROJECT_ID
                    WHERE PROJECTS_V.PROJECT_STATUS_CODE = 'APPROVED' AND PROJECTS_V.PROJECT_TYPE <> 'TRUCK ' || CHR(38) || ' EQUIPMENT' AND PA.PA_PROJECT_CLASSES.CLASS_CATEGORY = 'Job Cost Rollup' AND PA.PA_PROJECT_CLASSES.CLASS_CODE <> 'None' AND PROJECTS_V.CARRYING_OUT_ORGANIZATION_ID = {1}
                    GROUP BY CONCAT('Various - ', PA.PA_PROJECT_CLASSES.CLASS_CODE) 
                    ORDER BY ORDERKEY, PROJECT_NAME", orgName, orgID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).ToList();
                }
            }        
        }

        public class OP
        {
            #region Fields
            public class Fields
            {
                public decimal? OP { get; set; }
            }
            #endregion

            public static Fields Data(long orgID, long prevYearID, long prevVerID, string projectID)
            {
                string sql = string.Format(@"
                    SELECT NOV OP 
                    FROM BUD_BID_PROJECTS
                    LEFT OUTER JOIN BUD_BID_DETAIL_TASK ON BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = BUD_BID_DETAIL_TASK.PROJECT_ID
                    LEFT OUTER JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.PROJECT_ID = BUD_BID_BUDGET_NUM.PROJECT_ID AND BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID 
                    WHERE BUD_BID_PROJECTS.ORG_ID = {0} AND BUD_BID_PROJECTS.YEAR_ID = {1} AND BUD_BID_PROJECTS.VER_ID = {2} AND BUD_BID_PROJECTS.PROJECT_ID = '{3}' AND BUD_BID_PROJECTS.MODIFIED_BY <> 'TEMP' AND BUD_BID_DETAIL_TASK.DETAIL_NAME = 'SYS_PROJECT' AND BUD_BID_BUDGET_NUM.LINE_ID = 10", orgID, prevYearID, prevVerID, projectID);

                Fields data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }

                if (data == null)
                {
                    Fields nullData = new Fields();
                    nullData.OP = 0;
                    data = nullData;
                }

                return data;
            }
            public static Fields OverridenOP(long budBidprojectID)
            {
                string sql = string.Format(@"
                    SELECT COMPARE_PRJ_AMOUNT OP
                    FROM BUD_BID_PROJECTS
                    WHERE BUD_BID_PROJECTS_ID = {0}", budBidprojectID);

                Fields data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }

                if (data == null)
                {
                    Fields nullData = new Fields();
                    nullData.OP = 0;
                    data = nullData;
                }
                    
                return data;                    
            }
        }

        public class Detail
        {
            #region Fields
            public class Fields
            {
                public string PROJECT_ID { get; set; }
                public long BUD_BID_PROJECTS_ID { get; set; }
                public string TYPE { get; set; }
                public string PROJECT_NAME { get; set; }
                public long STATUS_ID { get; set; }
                public string STATUS { get; set; }
                public decimal ACRES { get; set; }
                public decimal DAYS { get; set; }
                public string APP_TYPE { get; set; }
                public string CHEMICAL_MIX { get; set; }
                public string COMMENTS { get; set; }
                public string LIABILITY { get; set; }
                public decimal LIABILITY_OP { get; set; }
                public string COMPARE_PRJ_OVERRIDE { get; set; }
                public decimal COMPARE_PRJ_AMOUNT { get; set; }
                public DateTime? WE_DATE { get; set; }
                public string WE_OVERRIDE { get; set; }
            }
            #endregion

            public static Fields Data(long budBidProjectID)
            {
                string sql = string.Format(@"
                    SELECT BUD_BID_PROJECTS.PROJECT_ID, BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID, BUD_BID_PROJECTS.TYPE, BUD_BID_PROJECTS.PRJ_NAME PROJECT_NAME, BUD_BID_STATUS.STATUS_ID,
                        BUD_BID_STATUS.STATUS, BUD_BID_PROJECTS.ACRES, BUD_BID_PROJECTS.DAYS, BUD_BID_PROJECTS.APP_TYPE, BUD_BID_PROJECTS.CHEMICAL_MIX, BUD_BID_PROJECTS.COMMENTS,
                        BUD_BID_PROJECTS.LIABILITY, BUD_BID_PROJECTS.LIABILITY_OP, BUD_BID_PROJECTS.COMPARE_PRJ_OVERRIDE, BUD_BID_PROJECTS.COMPARE_PRJ_AMOUNT, BUD_BID_PROJECTS.WE_DATE,
                        BUD_BID_PROJECTS.WE_OVERRIDE
                    FROM BUD_BID_PROJECTS
                    INNER JOIN BUD_BID_STATUS
                    ON BUD_BID_PROJECTS.STATUS_ID = BUD_BID_STATUS.STATUS_ID
                    WHERE BUD_BID_PROJECTS.BUD_BID_PROJECTS_ID = {0}", budBidProjectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                }
            }
        }

        public class WEDate
        {
            public static void Update(long budBidProjectID, string jcDate)
            {
                BUD_BID_PROJECTS jcDateData;
                using (Entities context = new Entities())
                {
                    jcDateData = context.BUD_BID_PROJECTS.Where(x => x.BUD_BID_PROJECTS_ID == budBidProjectID).SingleOrDefault();
                }

                if (jcDate == "")
                {
                    jcDateData.WE_DATE = null;
                }
                else
                {
                    jcDateData.WE_DATE = Convert.ToDateTime(jcDate);
                }

                GenericData.Update<BUD_BID_PROJECTS>(jcDateData);
            }
        }
    }
    
    public class BBDetail
    {
        public class MainGrid
        {
            #region Fields
            public class Fields
            {
                public long DETAIL_TASK_ID { get; set; }
                public string DETAIL_NAME { get; set; }
                public long SHEET_ORDER { get; set; }
                public decimal GROSS_REC { get; set; }
                public decimal MAT_USAGE { get; set; }
                public decimal GROSS_REV { get; set; }
                public decimal DIR_EXP { get; set; }
                public decimal OP { get; set; }
            }
            #endregion

            public static List<Fields> Data(long projectID)
            {
                string sql = string.Format(@"
                    WITH
                        TASKS AS (            
                            SELECT PROJECT_ID, DETAIL_TASK_ID, DETAIL_NAME, SHEET_ORDER, MODIFIED_BY
                            FROM BUD_BID_DETAIL_TASK
                            WHERE PROJECT_ID = {0}
                        ),          
                        BUDGET_LINE_AMOUNTS AS (
                            SELECT * FROM (
                                SELECT PROJECT_ID, DETAIL_TASK_ID, LINE_ID, NOV, MODIFIED_BY
                                FROM BUD_BID_BUDGET_NUM
                                WHERE PROJECT_ID = {0})
                            PIVOT(
                                SUM(NOV) FOR (LINE_ID)
                                IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))
                        )         
                    SELECT TASKS.PROJECT_ID,
                        TASKS.DETAIL_TASK_ID,
                        TASKS.DETAIL_NAME,
                        TASKS.SHEET_ORDER,
                        BUDGET_LINE_AMOUNTS.GROSS_REC,
                        BUDGET_LINE_AMOUNTS.MAT_USAGE,
                        BUDGET_LINE_AMOUNTS. GROSS_REV,
                        BUDGET_LINE_AMOUNTS.DIR_EXP,
                        BUDGET_LINE_AMOUNTS.OP
                    FROM TASKS 
                    INNER JOIN BUDGET_LINE_AMOUNTS ON TASKS.PROJECT_ID = BUDGET_LINE_AMOUNTS.PROJECT_ID AND TASKS.DETAIL_TASK_ID = BUDGET_LINE_AMOUNTS.DETAIL_TASK_ID
                    WHERE TASKS.DETAIL_NAME <> 'SYS_PROJECT'
                    ORDER BY SHEET_ORDER", projectID);

                using (Entities context = new Entities())
                {
                    return context.Database.SqlQuery<Fields>(sql).ToList();
                }
            }
        }

        public class Sheet
        {
            public static long ID(long budBidProjectID, long order)
            {
                BUD_BID_DETAIL_TASK data;
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.SHEET_ORDER == (order)).SingleOrDefault();
                }

                if (data == null)
                {
                    return 0;
                }

                return Convert.ToInt64(data.DETAIL_TASK_ID);
            }
            public static void DBDelete(long detailSheetID)
            {
                List<BUD_BID_ACTUAL_NUM> actualData;
                List<BUD_BID_BUDGET_NUM> budgetData;
                List<BUD_BID_DETAIL_SHEET> detailSheetData;
                List<BUD_BID_DETAIL_TASK> taskInfoData;

                using (Entities context = new Entities())
                {
                    actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                    taskInfoData = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailSheetID).ToList();
                }

                GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
                GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);
                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
                GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData);
            }
            public static long DBAdd(long budBidID, long sheetOrder)
            {
                BUD_BID_DETAIL_TASK detailData = new BUD_BID_DETAIL_TASK();
                detailData.PROJECT_ID = budBidID;
                detailData.DETAIL_NAME = "SYS_DETAIL_" + sheetOrder;
                detailData.SHEET_ORDER = sheetOrder;
                detailData.CREATE_DATE = DateTime.Now;
                detailData.CREATED_BY = HttpContext.Current.User.Identity.Name;
                detailData.MODIFY_DATE = DateTime.Now;
                detailData.MODIFIED_BY = "TEMP";
                GenericData.Insert<BUD_BID_DETAIL_TASK>(detailData);

                long newDetailID = Convert.ToInt64(detailData.DETAIL_TASK_ID);

                long[] arrLineNum = { 6, 7, 8, 9, 10 };
                List<BUD_BID_BUDGET_NUM> budNumsData = new List<BUD_BID_BUDGET_NUM>();
                for (int i = 0; i <= 4; i++)
                {
                    budNumsData.Add(new BUD_BID_BUDGET_NUM
                    {
                        PROJECT_ID = budBidID,
                        DETAIL_TASK_ID = newDetailID,
                        LINE_ID = arrLineNum[i],
                        NOV = 0,
                        CREATE_DATE = DateTime.Now,
                        CREATED_BY = HttpContext.Current.User.Identity.Name,
                        MODIFY_DATE = DateTime.Now,
                        MODIFIED_BY = "TEMP"
                    });
                }
                GenericData.Insert<BUD_BID_BUDGET_NUM>(budNumsData);

                return newDetailID;
            }
            public static void DBUpdateName(long detailSheetID, string sheetName)
            {
                BUD_BID_DETAIL_TASK data;

                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailSheetID).Single();
                }

                data.DETAIL_NAME = sheetName;

                GenericData.Update<BUD_BID_DETAIL_TASK>(data);
            }
            public static long DBCopy(long detailTaskID)
            {             
                // Task
                BUD_BID_DETAIL_TASK taskData;
                using (Entities context = new Entities())
                {
                    taskData = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailTaskID).SingleOrDefault();
                }

                long budBidID = taskData.PROJECT_ID;
                taskData.SHEET_ORDER = BBDetail.Sheets.MaxOrder(budBidID) + 1;
                string origTaskName = taskData.DETAIL_NAME;
                taskData.DETAIL_NAME = "* " + (origTaskName.Length >= 20 ? origTaskName.Substring(0, 20) : origTaskName) + " (COPIED: " + DateTime.Now + ")";
                taskData.CREATE_DATE = DateTime.Now;
                taskData.CREATED_BY = HttpContext.Current.User.Identity.Name;
                taskData.MODIFY_DATE = DateTime.Now;
                GenericData.Insert<BUD_BID_DETAIL_TASK>(taskData);
                decimal newDetailTaskID = taskData.DETAIL_TASK_ID;

                // Budgets
                List<BUD_BID_BUDGET_NUM> budgetData;
                using (Entities context = new Entities())
                {
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_BUDGET_NUM budgetField in budgetData)
                {
                    budgetField.PROJECT_ID = Convert.ToInt64(budBidID);
                    budgetField.DETAIL_TASK_ID = newDetailTaskID;
                    budgetField.CREATE_DATE = DateTime.Now;
                    budgetField.CREATED_BY = HttpContext.Current.User.Identity.Name;
                    budgetField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_BUDGET_NUM>(budgetData);

                // Detail sheets
                List<BUD_BID_DETAIL_SHEET> sheetData;
                using (Entities context = new Entities())
                {
                    sheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidID && x.DETAIL_TASK_ID == detailTaskID).ToList();
                }
                foreach (BUD_BID_DETAIL_SHEET detailField in sheetData)
                {
                    detailField.PROJECT_ID = Convert.ToInt64(budBidID);
                    detailField.DETAIL_TASK_ID = newDetailTaskID;
                    detailField.CREATE_DATE = DateTime.Now;
                    detailField.CREATED_BY = HttpContext.Current.User.Identity.Name;
                    detailField.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(sheetData);

                return Convert.ToInt64(newDetailTaskID);
            }

            public class MainTabField
            {
                #region Fields
                public class Fields
                {
                    public decimal? RECREMAIN { get; set; }
                    public decimal? DAYSREMAIN { get; set; }
                    public decimal? UNITREMAIN { get; set; }
                    public decimal? DAYSWORKED { get; set; }
                }
                #endregion

                public static Fields NumsData(long detailSheetID)
                {
                    string sql = string.Format(@"
                        SELECT * FROM (
                            SELECT DETAIL_TASK_ID, REC_TYPE, TOTAL
                            FROM BUD_BID_DETAIL_SHEET
                            WHERE DETAIL_TASK_ID = {0})
                        PIVOT(
                            SUM(TOTAL) FOR (REC_TYPE)
                            IN ('RECREMAIN' RECREMAIN, 'DAYSREMAIN' DAYSREMAIN, 'UNITREMAIN' UNITREMAIN, 'DAYSWORKED' DAYSWORKED))", detailSheetID);

                    Fields data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        Fields nullData = new Fields();
                        nullData.RECREMAIN = 0;
                        nullData.DAYSREMAIN = 0;
                        nullData.UNITREMAIN = 0;
                        nullData.DAYSWORKED = 0;
                        return nullData;
                    }

                    return data;
                }
                public static string Comment(long detailSheetID)
                {
                    BUD_BID_DETAIL_TASK data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailSheetID).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        return "";
                    }

                    return data.COMMENTS;
                }
                public static void DBUpdateNums(long budBidProjectID, long detailSheetID, string recType, decimal amount)
                {
                    BUD_BID_DETAIL_SHEET data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_TASK_ID == detailSheetID && x.REC_TYPE == recType).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        data = new BUD_BID_DETAIL_SHEET();
                        data.PROJECT_ID = budBidProjectID;
                        data.REC_TYPE = recType;
                        data.AMT_1 = 0;
                        data.AMT_2 = 0;
                        data.AMT_3 = 0;
                        data.AMT_4 = 0;
                        data.AMT_5 = 0;
                        data.TOTAL = amount;
                        data.CREATE_DATE = DateTime.Now;
                        data.CREATED_BY = HttpContext.Current.User.Identity.Name;
                        data.MODIFY_DATE = DateTime.Now;
                        data.MODIFIED_BY = "TEMP";
                        data.DETAIL_TASK_ID = detailSheetID;
                        GenericData.Insert<BUD_BID_DETAIL_SHEET>(data);
                    }

                    else
                    {
                        data.TOTAL = amount;
                        GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
                    }
                }
                public static void DBUpdateComments(long detailSheetID, string comment)
                {
                    BUD_BID_DETAIL_TASK data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_DETAIL_TASK.Where(x => x.DETAIL_TASK_ID == detailSheetID).SingleOrDefault();
                    }
                    
                    data.COMMENTS = comment;
                    GenericData.Update<BUD_BID_DETAIL_TASK>(data);
                }
            }

            public class Subtotals
            {
                #region Fields
                public class Fields
                {
                    public decimal? MATERIAL { get; set; }
                    public decimal? EQUIPMENT { get; set; }
                    public decimal? PERSONNEL { get; set; }
                    public decimal? PERDIEM { get; set; }
                    public decimal? TRAVEL { get; set; }
                    public decimal? MOTELS { get; set; }
                    public decimal? MISC { get; set; }
                    public decimal? LUMPSUM { get; set; }
                }
                #endregion

                public static Fields Get(long detailSheetID)
                {
                    string sql = string.Format(@"
                        SELECT * FROM (
                            SELECT DETAIL_TASK_ID, REC_TYPE, TOTAL
                            FROM BUD_BID_DETAIL_SHEET
                            WHERE DETAIL_TASK_ID = {0})
                        PIVOT(
                            SUM(TOTAL) FOR (REC_TYPE)
                            IN ('MATERIAL' MATERIAL, 'EQUIPMENT' EQUIPMENT, 'PERSONNEL' PERSONNEL, 'PERDIEM' PERDIEM,
                                'TRAVEL' TRAVEL, 'MOTELS' MOTELS, 'MISC' MISC, 'LUMPSUM' LUMPSUM))", detailSheetID);

                    Fields data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        Fields nullData = new Fields();
                        nullData.MATERIAL = 0;
                        nullData.EQUIPMENT = 0;
                        nullData.PERSONNEL = 0;
                        nullData.PERDIEM = 0;
                        nullData.TRAVEL = 0;
                        nullData.MOTELS = 0;
                        nullData.MISC = 0;
                        nullData.LUMPSUM = 0;
                        return nullData;
                    }

                    return data;
                }
            }

            public class BottomNumbers
            {
                #region Fields
                public class Fields
                {
                    public decimal LABOR_BURDEN { get; set; }
                    public decimal TOTAL_WKLY_DIRECTS { get; set; }
                    public decimal TOTAL_DIRECTS_PER_DAY { get; set; }
                    public decimal AVG_UNITS_PER_DAY { get; set; }
                    public decimal TOTAL_DIRECTS_LEFT { get; set; }
                    public decimal TOTAL_MATERIAL_LEFT { get; set; }
                }
                #endregion

                public static Fields Calculate(long leOrgID, long yearID, long detailSheetID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP, decimal totalDaysRemain, 
                    decimal totalUnitsRemain, decimal totalDaysWorked)
                {
                    BBDetail.Sheet.Subtotals.Fields subtotal = BBDetail.Sheet.Subtotals.Get(detailSheetID);
                    decimal totalMaterial = subtotal.MATERIAL ?? 0;
                    decimal totalEquipment = subtotal.EQUIPMENT ?? 0;
                    decimal totalPersonnel = subtotal.PERSONNEL ?? 0;
                    decimal totalPerDiem = subtotal.PERDIEM ?? 0;
                    decimal totalTravel = subtotal.TRAVEL ?? 0;
                    decimal totalMotels = subtotal.MOTELS ?? 0;
                    decimal totalMisc = subtotal.MISC ?? 0;
                    decimal totalLumpSum = subtotal.LUMPSUM ?? 0;

                    decimal laborBurdenRate = BB.LaborBurdenRate(leOrgID, yearID);
                    decimal laborBurden = (totalPersonnel + totalTravel) * laborBurdenRate;
                    decimal totalWklyDirects = totalEquipment + totalPersonnel + totalPerDiem + totalTravel + totalMotels + totalMisc + laborBurden;
                    decimal totalDirectsPerDay = totalDaysWorked == 0 ? 0 : totalWklyDirects / totalDaysWorked;
                    decimal avgUnitsPerDay = totalDaysRemain == 0 ? 0 : totalUnitsRemain / totalDaysRemain;
                    decimal totalDirectsLeft = (totalDirectsPerDay * totalDaysRemain) + totalLumpSum;
                    decimal totalMaterialLeft = totalMaterial * totalUnitsRemain;

                    Fields returnData = new Fields();
                    returnData.LABOR_BURDEN = laborBurden;
                    returnData.TOTAL_WKLY_DIRECTS = totalWklyDirects;
                    returnData.TOTAL_DIRECTS_PER_DAY = totalDirectsPerDay;
                    returnData.AVG_UNITS_PER_DAY = avgUnitsPerDay;
                    returnData.TOTAL_DIRECTS_LEFT = totalDirectsLeft;
                    returnData.TOTAL_MATERIAL_LEFT = totalMaterialLeft;
                    return returnData;
                }
            }

            public class EndNumbers
            {
                #region Fields
                public class Fields
                {
                    public decimal GROSS_REC { get; set; }
                    public decimal MAT_USAGE { get; set; }
                    public decimal GROSS_REV { get; set; }
                    public decimal DIR_EXP { get; set; }
                    public decimal OP { get; set; }
                }
                #endregion

                public static Fields Get(long detailSheetID)
                {
                    string sql = string.Format(@"
                    SELECT * FROM (
                        SELECT BUD_BID_DETAIL_TASK.DETAIL_TASK_ID, LINE_ID, NOV
                        FROM BUD_BID_DETAIL_TASK
                        LEFT JOIN BUD_BID_BUDGET_NUM ON BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = BUD_BID_BUDGET_NUM.DETAIL_TASK_ID
                        WHERE BUD_BID_DETAIL_TASK.DETAIL_TASK_ID = {0})
                    PIVOT(
                        SUM(NOV) FOR (LINE_ID)
                        IN (6 GROSS_REC, 7 MAT_USAGE, 8 GROSS_REV, 9 DIR_EXP, 10 OP))", detailSheetID);

                    Fields data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).SingleOrDefault();
                    }

                    if (data == null)
                    {
                        Fields nullData = new Fields();
                        nullData.GROSS_REC = 0;
                        nullData.MAT_USAGE = 0;
                        nullData.GROSS_REV = 0;
                        nullData.DIR_EXP = 0;
                        nullData.OP = 0;
                        return nullData;
                    }

                    return data;
                }
                public static void DBUpdate(long detailSheetID, decimal eGrossRec, decimal eMaterial, decimal eGrossRev, decimal eDirects, decimal eOP)
                {
                    long[] lineNum = { 6, 7, 8, 9, 10 };
                    decimal[] endNum = { eGrossRec, eMaterial, eGrossRev, eDirects, eOP };

                    List<BUD_BID_BUDGET_NUM> data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_BUDGET_NUM.Where(x => x.DETAIL_TASK_ID == detailSheetID).OrderBy(x => x.LINE_ID).ToList();
                    }

                    int a = 0;
                    foreach (BUD_BID_BUDGET_NUM field in data)
                    {
                        field.NOV = endNum[a];
                        a++;
                    }

                    GenericData.Update<BUD_BID_BUDGET_NUM>(data);
                }
                public static Fields Calculate(long leOrgID, long yearID, long detailSheetID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP,  decimal totalReceiptsRemain, 
                    decimal totalDaysRemain, decimal totalUnitsRemain, decimal totalDaysWorked)
                {
                    BBDetail.Sheet.BottomNumbers.Fields data = BBDetail.Sheet.BottomNumbers.Calculate(leOrgID, yearID, detailSheetID, sGrossRec, sMaterial, sGrossRev, sDirects, sOP, 
                        totalDaysRemain, totalUnitsRemain, totalDaysWorked);
                    decimal totalMatLeft = data.TOTAL_MATERIAL_LEFT;
                    decimal totalDirectsLeft = data.TOTAL_DIRECTS_LEFT;

                    decimal eGrossRec = sGrossRec + totalReceiptsRemain;
                    decimal eMaterial = sMaterial + totalMatLeft;
                    decimal eGrossRev = eGrossRec - eMaterial;
                    decimal eDirects = sDirects + totalDirectsLeft;
                    decimal eOP = eGrossRev - eDirects;

                    Fields endNums = new Fields();
                    endNums.GROSS_REC = eGrossRec;
                    endNums.MAT_USAGE = eMaterial;
                    endNums.GROSS_REV = eGrossRev;
                    endNums.DIR_EXP = eDirects;
                    endNums.OP = eOP;
                    return endNums;
                }              
            }

            public class MaterialListing
            {
                #region Fields
                public class Fields
                {
                    public string DESCRIPTION { get; set; }
                    public string UOM_CODE { get; set; }  
                    public decimal? ITEM_COST { get; set; }                                      
                }
                #endregion

                public static List<Fields> Data(long orgID)
                {
                    long invOrgID = BB.InventoryOrg(orgID);
                    string sql = string.Format(@"
                        SELECT DESCRIPTION,
                            UOM_CODE,
                            ITEM_COST 
                        FROM XXEMS.INVENTORY_V
                        WHERE INV_LOCATION = {0}
                        ORDER BY DESCRIPTION", invOrgID);             

                    using (Entities context = new Entities())
                    {
                        return context.Database.SqlQuery<Fields>(sql).ToList();
                    }
                }
            }

            public class EquipmentListing
            {
                #region Fields
                public class Fields
                {
                    public string EXPENDITURE_TYPE { get; set; }
                    public decimal? COST_RATE { get; set; }
                }
                #endregion

                public static List<Fields> Data(long orgID)
                {
                    long equipOrgID = BB.EquipmentOrg(orgID);
                    string sql = string.Format(@"                           
                        SELECT EXPENDITURE_TYPE,
                            COST_RATE                        
                        FROM APPS.PA_EXPENDITURE_COST_RATES_ALL
                        WHERE ORG_ID = {0} AND 
                            SYSDATE BETWEEN START_DATE_ACTIVE
                            AND NVL (END_DATE_ACTIVE, '31-DEC-4712')
                            GROUP BY EXPENDITURE_TYPE, COST_RATE
                        ORDER BY EXPENDITURE_TYPE", equipOrgID);

                    using (Entities context = new Entities())
                    {
                        return context.Database.SqlQuery<Fields>(sql).ToList();
                    }
                }
            }

            public class PersonnelListing
            {
                #region Fields
                public class Fields
                {
                    public string COMPANY { get; set; }
                    public string POSITION { get; set; }
                    public decimal? COST_PER_HR { get; set; }
                }
                #endregion

                public static List<Fields> Data(string company)
                {
                    List<Fields> data = new List<Fields>();
                    data.Add(new Fields { COMPANY = "DBI", POSITION = "Foreman", COST_PER_HR = 0 });
                    data.Add(new Fields { COMPANY = "DBI", POSITION = "Laborer", COST_PER_HR = 0 });
                    data.Add(new Fields { COMPANY = "DBI", POSITION = "Supervisor", COST_PER_HR = 0 });
                    data.Add(new Fields { COMPANY = "DBI", POSITION = "Technician", COST_PER_HR = 0 });
                    data.Add(new Fields { COMPANY = "DBI", POSITION = "Truck Driver", COST_PER_HR = 0 });
                    return data;
                }
            }
        }

        public class Sheets
        {
            public static long MaxOrder(long budBidProjectID)
            {
                BUD_BID_DETAIL_TASK data;
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID).OrderByDescending(y => y.SHEET_ORDER).FirstOrDefault();
                }

                if (data == null)
                {
                    return 0;
                }

                return Convert.ToInt64(data.SHEET_ORDER);
            }
            public static void DBResetOrder(long budBidProjectID)
            {
                string sql = string.Format(@"
                    SELECT * FROM BUD_BID_DETAIL_TASK
                    WHERE PROJECT_ID = {0} AND DETAIL_NAME <> 'SYS_PROJECT'
                    ORDER BY SHEET_ORDER", budBidProjectID);

                List<BUD_BID_DETAIL_TASK> data;
                using (Entities context = new Entities())
                {
                    data = context.Database.SqlQuery<BUD_BID_DETAIL_TASK>(sql).ToList();
                }

                int i = 1;
                foreach (BUD_BID_DETAIL_TASK field in data)
                {
                    field.SHEET_ORDER = i;
                    i++;
                    GenericData.Update<BUD_BID_DETAIL_TASK>(data);
                }                
            }
            public static long SYS_PROJECTSheetID(long budBidProjectID)
            {
                BUD_BID_DETAIL_TASK data;
                using (Entities context = new Entities())
                {
                    data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_NAME == "SYS_PROJECT").SingleOrDefault();
                }

                return Convert.ToInt64(data.DETAIL_TASK_ID);
            }
            public static void DBDelete(long budBidProjectID, bool includeSYS_PROJECT)
            {
                long sysProjectDetailSheetID;

                if (includeSYS_PROJECT == true)
                {
                    sysProjectDetailSheetID = 0;
                }
                else
                {
                    sysProjectDetailSheetID = BBDetail.Sheets.SYS_PROJECTSheetID(budBidProjectID);
                }

                List<BUD_BID_ACTUAL_NUM> actualData;
                List<BUD_BID_BUDGET_NUM> budgetData;
                List<BUD_BID_DETAIL_SHEET> detailSheetData;
                List<BUD_BID_DETAIL_TASK> taskInfoData;

                using (Entities context = new Entities())
                {
                    actualData = context.BUD_BID_ACTUAL_NUM.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_TASK_ID != sysProjectDetailSheetID).ToList();
                    budgetData = context.BUD_BID_BUDGET_NUM.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_TASK_ID != sysProjectDetailSheetID).ToList();
                    detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_TASK_ID != sysProjectDetailSheetID).ToList();
                    taskInfoData = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_TASK_ID != sysProjectDetailSheetID).ToList();
                }

                GenericData.Delete<BUD_BID_ACTUAL_NUM>(actualData);
                GenericData.Delete<BUD_BID_BUDGET_NUM>(budgetData);
                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
                GenericData.Delete<BUD_BID_DETAIL_TASK>(taskInfoData);
            }

            public class SheetOrder
            {
                public static List<BUD_BID_DETAIL_TASK> Data(long budBidProjectID)
                {
                    using (Entities context = new Entities())
                    {
                        return context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.SHEET_ORDER != 0).OrderBy(x => x.SHEET_ORDER).ToList();
                    }
                }
            }

            public class EndNumbers
            {
                #region Fields
                public class Fields
                {
                    public decimal GROSS_REC { get; set; }
                    public decimal MAT_USAGE { get; set; }
                    public decimal GROSS_REV { get; set; }
                    public decimal DIR_EXP { get; set; }
                    public decimal OP { get; set; }
                }
                #endregion

                public static void DBCalculate(long leOrgID, long yearID, long budBidProjectID, decimal sGrossRec, decimal sMaterial, decimal sGrossRev, decimal sDirects, decimal sOP)
                {
                    List<BUD_BID_DETAIL_TASK> data;
                    using (Entities context = new Entities())
                    {
                        data = context.BUD_BID_DETAIL_TASK.Where(x => x.PROJECT_ID == budBidProjectID && x.DETAIL_NAME != "SYS_PROJECT").OrderBy(y => y.SHEET_ORDER).ToList();
                    }

                    long detailSheetID;
                    decimal eGrossRec = sGrossRec;
                    decimal eMatUsage = sMaterial;
                    decimal eGrossRev = sGrossRev;
                    decimal eDirects = sDirects;
                    decimal eOP = sOP;
                    foreach (BUD_BID_DETAIL_TASK field in data)
                    {
                        detailSheetID = Convert.ToInt64(field.DETAIL_TASK_ID);

                        BBDetail.Sheet.MainTabField.Fields mainTabData = BBDetail.Sheet.MainTabField.NumsData(detailSheetID);
                        decimal totalReceiptsRemain = mainTabData.RECREMAIN ?? 0;
                        decimal totalDaysRemain = mainTabData.DAYSREMAIN ?? 0;
                        decimal totalUnitsRemain = mainTabData.UNITREMAIN ?? 0;
                        decimal totalDaysWorked = mainTabData.DAYSWORKED ?? 0;

                        BBDetail.Sheet.EndNumbers.Fields endNums = BBDetail.Sheet.EndNumbers.Calculate(leOrgID, yearID, detailSheetID, sGrossRec, sMaterial, sGrossRev, sDirects, sOP,  
                            totalReceiptsRemain, totalDaysRemain,  totalUnitsRemain,  totalDaysWorked);
                        eGrossRec = endNums.GROSS_REC;
                        eMatUsage = endNums.MAT_USAGE;
                        eGrossRev = endNums.GROSS_REV;
                        eDirects = endNums.DIR_EXP;
                        eOP = endNums.OP;
                        sGrossRec = eGrossRec;
                        sMaterial = eMatUsage;
                        sGrossRev = eGrossRev;
                        sDirects = eDirects;
                        sOP = eOP;

                        BBDetail.Sheet.EndNumbers.DBUpdate(detailSheetID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);
                    }

                    BBProject.EndNumbersW0.DBUpdate(budBidProjectID, eGrossRec, eMatUsage, eGrossRev, eDirects, eOP);
                }
            }
        }

        public class SubGrid
        {
            public static void DeleteRecord(long detailSheetID)
            {
                BUD_BID_DETAIL_SHEET detailSheetData;

                using (Entities context = new Entities())
                {
                    detailSheetData = context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == detailSheetID).SingleOrDefault();
                }

                GenericData.Delete<BUD_BID_DETAIL_SHEET>(detailSheetData);
            }

            public class BOM 
            {
                public static void AddItems(long projectID, long detailTaskID)
                {
                    BUD_BID_DETAIL_SHEET data = new BUD_BID_DETAIL_SHEET();

                    data.PROJECT_ID = projectID;
                    data.DETAIL_TASK_ID = detailTaskID;
                    data.REC_TYPE = "MATERIAL";
                    data.DESC_1 = "TEST";
                    data.DESC_2 = "";
                    data.AMT_1 = 0;
                    data.AMT_2 = 0;
                    data.AMT_3 = 0;
                    data.AMT_4 = 0;
                    data.AMT_5 = 0;
                    data.TOTAL = 0;
                    data.CREATE_DATE = DateTime.Now;
                    data.CREATED_BY = HttpContext.Current.User.Identity.Name;
                    data.MODIFY_DATE = DateTime.Now;
                    data.MODIFIED_BY = "TEMP";
                    GenericData.Insert<BUD_BID_DETAIL_SHEET>(data);
                }

                public class Listing
                {
                    #region Fields
                    public class Fields
                    {
                        public long ORGANIZATION_ID { get; set; }
                        public long BILL_SEQUENCE_ID { get; set; }
                        public string DESCRIPTION { get; set; }
                        public string SEGMENT1 { get; set; }
                        public string ATTRIBUTE15 { get; set; }
                    }
                    #endregion

                    public static List<Fields> Data(long orgID)
                    {
                        long invOrgID = BB.InventoryOrg(orgID);
                        string sql = string.Format(@"
                        SELECT APPS.MTL_SYSTEM_ITEMS.ORGANIZATION_ID,
                            APPS.BOM_BILL_OF_MATERIALS_V.BILL_SEQUENCE_ID,
                            APPS.BOM_BILL_OF_MATERIALS_V.DESCRIPTION,
                            APPS.MTL_SYSTEM_ITEMS.SEGMENT1,
                            APPS.BOM_BILL_OF_MATERIALS_V.ATTRIBUTE15
                        FROM APPS.BOM_BILL_OF_MATERIALS_V
                        LEFT JOIN APPS.MTL_SYSTEM_ITEMS ON APPS.BOM_BILL_OF_MATERIALS_V.ASSEMBLY_ITEM_ID = APPS.MTL_SYSTEM_ITEMS.INVENTORY_ITEM_ID
                        WHERE APPS.MTL_SYSTEM_ITEMS.ORGANIZATION_ID = {0}
                        ORDER BY APPS.BOM_BILL_OF_MATERIALS_V.DESCRIPTION", invOrgID);

                        List<Fields> data;
                        using (Entities context = new Entities())
                        {
                            data = context.Database.SqlQuery<Fields>(sql).ToList();
                        }

                        return data;
                    }
                }

                public class MaterialItems
                {
                    #region Fields
                    public class Fields
                    {
                        public long COMPONENT_ITEM_ID { get; set; }
                        public string DESCRIPTION { get; set; }
                        public string PRIMARY_UOM_CODE { get; set; }
                        public decimal COMPONENT_QUANTITY { get; set; }
                        public decimal ITEM_COST { get; set; }
                    }
                    #endregion

                    public static List<Fields> Data(long orgID, long billSeqID)
                    {
                        long invOrgID = BB.InventoryOrg(orgID);
                        string sql = string.Format(@"
                            SELECT APPS.BOM_INVENTORY_COMPONENTS_V.COMPONENT_ITEM_ID,
                                APPS.BOM_INVENTORY_COMPONENTS_V.DESCRIPTION,
                                APPS.BOM_INVENTORY_COMPONENTS_V.PRIMARY_UOM_CODE,
                                APPS.BOM_INVENTORY_COMPONENTS_V.COMPONENT_QUANTITY,
                                XXEMS.INVENTORY_V.ITEM_COST
                            FROM APPS.BOM_INVENTORY_COMPONENTS_V 
                            LEFT JOIN XXEMS.INVENTORY_V ON APPS.BOM_INVENTORY_COMPONENTS_V.COMPONENT_ITEM_ID = XXEMS.INVENTORY_V.ITEM_ID
                            WHERE XXEMS.INVENTORY_V.ORGANIZATION_ID = {0} AND BILL_SEQUENCE_ID = {1}", invOrgID, billSeqID);

                        List<Fields> data;
                        using (Entities context = new Entities())
                        {
                            data = context.Database.SqlQuery<Fields>(sql).ToList();
                        }

                        return data;
                    }
                }
            }

            public class Data
            {
                #region Fields
                public class Fields
                {
                    public long DETAIL_SHEET_ID { get; set; }
                    public long PROJECT_ID { get; set; }
                    public long DETAIL_TASK_ID { get; set; }
                    public string REC_TYPE { get; set; }
                    public string DESC_1 { get; set; }
                    public string DESC_2 { get; set; }
                    public decimal? AMT_1 { get; set; }
                    public decimal? AMT_2 { get; set; }
                    public decimal? AMT_3 { get; set; }
                    public decimal? AMT_4 { get; set; }
                    public decimal? AMT_5 { get; set; }
                    public decimal? TOTAL { get; set; }
                    public decimal? OVERRIDDEN { get; set; }
                }
                #endregion

                public static List<Fields> Get(long orgID, long projectID, long detailSheetID, string recType)
                {
                    string sql;
                    switch (recType)
                    {
                        case "MATERIAL":
                            long invOrgID = BB.InventoryOrg(orgID);
                            sql = string.Format(@"
                                WITH
                                    MY_LISTING AS (
                                        SELECT DETAIL_SHEET_ID,
                                            PROJECT_ID,
                                            DETAIL_TASK_ID,
                                            REC_TYPE,
                                            DESC_1,
                                            DESC_2,
                                            AMT_1,
                                            AMT_2,
                                            AMT_3,
                                            AMT_4,
                                            AMT_5,
                                            TOTAL
                                        FROM BUD_BID_DETAIL_SHEET 
                                        WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND REC_TYPE = '{2}'                                        
                                    ),
                                    ORACLE_LISTING AS (
                                        SELECT DESCRIPTION,
                                            UOM_CODE,
                                            ITEM_COST 
                                        FROM XXEMS.INVENTORY_V
                                        WHERE INV_LOCATION = {3}
                                    )
                                SELECT MY_LISTING.DETAIL_SHEET_ID,
                                    MY_LISTING.PROJECT_ID,
                                    MY_LISTING.DETAIL_TASK_ID,
                                    MY_LISTING.REC_TYPE,
                                    MY_LISTING.DESC_1,
                                    MY_LISTING.DESC_2,
                                    MY_LISTING.AMT_1,
                                    MY_LISTING.AMT_2,
                                    MY_LISTING.AMT_3,
                                    MY_LISTING.AMT_4,
                                    MY_LISTING.AMT_5,
                                    MY_LISTING.TOTAL,
                                    CASE WHEN (ORACLE_LISTING.DESCRIPTION IS NULL
                                    OR MY_LISTING.DESC_2 <> ORACLE_LISTING.UOM_CODE
                                    OR MY_LISTING.AMT_1 <> ORACLE_LISTING.ITEM_COST) THEN 1 ELSE 0 END OVERRIDDEN  
                                FROM MY_LISTING
                                LEFT JOIN ORACLE_LISTING ON MY_LISTING.DESC_1 = ORACLE_LISTING.DESCRIPTION
                                ORDER BY DETAIL_SHEET_ID", projectID, detailSheetID, recType, invOrgID);
                            break;

                        case "EQUIPMENT":
                            long equipOrgID = BB.EquipmentOrg(orgID);
                            sql = string.Format(@"
                                WITH
                                    MY_LISTING AS (
                                        SELECT DETAIL_SHEET_ID,
                                            PROJECT_ID,
                                            DETAIL_TASK_ID,
                                            REC_TYPE,
                                            DESC_1,
                                            DESC_2,
                                            AMT_1,
                                            AMT_2,
                                            AMT_3,
                                            AMT_4,
                                            AMT_5,
                                            TOTAL
                                        FROM BUD_BID_DETAIL_SHEET 
                                        WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND REC_TYPE = '{2}'
                                    ),
                                    ORACLE_LISTING AS (
                                        SELECT EXPENDITURE_TYPE,
                                            COST_RATE                        
                                        FROM APPS.PA_EXPENDITURE_COST_RATES_ALL
                                        WHERE ORG_ID = {3} AND 
                                            SYSDATE BETWEEN START_DATE_ACTIVE
                                            AND NVL (END_DATE_ACTIVE, '31-DEC-4712')
                                        GROUP BY EXPENDITURE_TYPE, COST_RATE
                                    )
                                    SELECT MY_LISTING.DETAIL_SHEET_ID,
                                        MY_LISTING.PROJECT_ID,
                                        MY_LISTING.DETAIL_TASK_ID,
                                        MY_LISTING.REC_TYPE,
                                        MY_LISTING.DESC_1,
                                        MY_LISTING.DESC_2,
                                        MY_LISTING.AMT_1,
                                        MY_LISTING.AMT_2,
                                        MY_LISTING.AMT_3,
                                        MY_LISTING.AMT_4,
                                        MY_LISTING.AMT_5,
                                        MY_LISTING.TOTAL,
                                        CASE WHEN (ORACLE_LISTING.EXPENDITURE_TYPE IS NULL
                                            OR MY_LISTING.AMT_3 <> ORACLE_LISTING.COST_RATE) THEN 1 ELSE 0 END OVERRIDDEN  
                                    FROM MY_LISTING
                                    LEFT JOIN ORACLE_LISTING ON MY_LISTING.DESC_1 = ORACLE_LISTING.EXPENDITURE_TYPE
                                    ORDER BY DETAIL_SHEET_ID", projectID, detailSheetID, recType, equipOrgID);
                            break;

                        default:
                            sql = string.Format(@"
                                SELECT DETAIL_SHEET_ID,
                                    PROJECT_ID,
                                    DETAIL_TASK_ID,
                                    REC_TYPE,
                                    DESC_1,
                                    DESC_2,
                                    AMT_1,
                                    AMT_2,
                                    AMT_3,
                                    AMT_4,
                                    AMT_5,
                                    TOTAL,
                                    0 OVERRIDDEN
                                FROM BUD_BID_DETAIL_SHEET 
                                WHERE PROJECT_ID = {0} AND DETAIL_TASK_ID = {1} AND REC_TYPE = '{2}'
                                ORDER BY DETAIL_SHEET_ID", projectID, detailSheetID, recType);
                            break;
                    }

                    List<Fields> data;
                    using (Entities context = new Entities())
                    {
                        data = context.Database.SqlQuery<Fields>(sql).ToList();                        
                    }

                    return data;
                }
            }
        }
    }
}
