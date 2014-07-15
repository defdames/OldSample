using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using System.Data.Entity;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umManageOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadDollars(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
            {
                long OrgID = GetOrgFromTree(_selectedRecordID);
                using (Entities _context = new Entities())
                {
                    IQueryable<CUSTOMER_SURVEYS.CustomerSurveyThresholdStore> Amounts = CUSTOMER_SURVEYS.GetOrganizationThresholdAmounts(OrgID, _context);
                    int count;
                    uxDollarStore.DataSource = GenericData.ListFilterHeader(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], Amounts, out count);
                    e.Total = count;
                }
            }
        }

        protected long GetOrgFromTree(string _selectedRecordID)
        {
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            return long.Parse(_selectedID[1].ToString());
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = HR.ActiveOverheadBudgetLegalEntities();
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        e.Nodes.Add(node);
                    }
                }

                else
                {
                    long nodeID = long.Parse(e.NodeID);
                    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID);
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_STRUCTURE_ID.ToString(), view.ORGANIZATION_ID.ToString());
                        e.Nodes.Add(node);
                    }
                }
            }

            // User clicked on org
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());
                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);
                var OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID);
                bool addNode;
                bool leafNode;
                bool colorNode;

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        addNode = false;
                        leafNode = true;
                        colorNode = false;
                        var nextData = HR.ActiveOrganizationsByHierarchy(hierarchyID, view.ORGANIZATION_ID);

                        // In this org?
                        if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in OrgsList)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrgs))
                            {
                                addNode = true;
                                leafNode = false;
                                break;
                            }
                        }

                        if (addNode == true)
                        {
                            Node node = new Node();
                            node.NodeID = string.Format("{0}:{1}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            //if (colorNode == true)
                            //{
                            //    node.Icon = Icon.Tick;
                            //}
                            //else
                            //{
                            //    node.Icon = Icon.FolderGo;
                            //}
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        protected void deReadThresholds(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                decimal AmountId = decimal.Parse(e.Parameters["AmountId"]);
                List<CUSTOMER_SURVEY_THRESHOLDS> Threshold = CUSTOMER_SURVEYS.GetThresholdPercentages(AmountId, _context).Include(x => x.CUSTOMER_SURVEY_THRESH_AMT).ToList();
                uxThresholdStore.DataSource = Threshold;
            }
        }

        protected void deSaveThreshold(object sender, DirectEventArgs e)
        {
            decimal AmountId = decimal.Parse(e.ExtraParams["AmountId"]);
            
            CUSTOMER_SURVEY_THRESHOLDS Threshold;
            if (uxThresholdFormType.Text == "Add")
            {
                Threshold = new CUSTOMER_SURVEY_THRESHOLDS();
                Threshold.THRESHOLD = decimal.Parse(uxThreshold.Text);
                Threshold.AMOUNT_ID = AmountId;
                Threshold.CREATE_DATE = DateTime.Now;
                Threshold.MODIFY_DATE = DateTime.Now;
                Threshold.CREATED_BY = User.Identity.Name;
                Threshold.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert<CUSTOMER_SURVEY_THRESHOLDS>(Threshold);
            }
            else
            {
                using (Entities _context = new Entities())
                {
                    decimal ThresholdId = decimal.Parse(uxThresholdId.Text);
                    Threshold = CUSTOMER_SURVEYS.GetThreshold(ThresholdId, _context);
                    Threshold.THRESHOLD = decimal.Parse(uxThreshold.Text);
                    Threshold.MODIFIED_BY = User.Identity.Name;
                    Threshold.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Update<CUSTOMER_SURVEY_THRESHOLDS>(Threshold);
            }

            uxThresholdStore.Reload();
            uxThresholdForm.Reset();
            uxAddEditThresholdWindow.Hide();

        }

        protected void deSaveDollar(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            long OrgId = GetOrgFromTree(_selectedRecordID);

            CUSTOMER_SURVEY_THRESH_AMT Dollars;
            if (uxDollarFormType.Value.ToString() == "Add")
            {
                Dollars = new CUSTOMER_SURVEY_THRESH_AMT();
                Dollars.LOW_DOLLAR_AMT = decimal.Parse(uxLowDollar.Text);
                Dollars.HIGH_DOLLAR_AMT = decimal.Parse(uxHighDollar.Text);
                Dollars.ORG_ID = OrgId;
                Dollars.CREATED_BY = User.Identity.Name;
                Dollars.MODIFIED_BY = User.Identity.Name;
                Dollars.CREATE_DATE = DateTime.Now;
                Dollars.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<CUSTOMER_SURVEY_THRESH_AMT>(Dollars);

            }
            else
            {
                decimal AmountId = decimal.Parse(uxDollarAmountId.Value.ToString());
                using (Entities _context = new Entities())
                {
                    Dollars = CUSTOMER_SURVEYS.GetOrganizationThresholdAmount(AmountId, _context);
                }
                Dollars.LOW_DOLLAR_AMT = decimal.Parse(uxLowDollar.Text);
                Dollars.HIGH_DOLLAR_AMT = decimal.Parse(uxHighDollar.Text);
                Dollars.MODIFY_DATE = DateTime.Now;
                Dollars.MODIFIED_BY = User.Identity.Name;

                GenericData.Update<CUSTOMER_SURVEY_THRESH_AMT>(Dollars);
            }

            uxDollarStore.Reload();
            uxAddEditDollarWindow.Hide();
            uxDollarForm.Reset();
        }

        protected void deLoadDollarWindow(object sender, DirectEventArgs e)
        {
            uxDollarFormType.Value = "Edit";
            uxDollarAmountId.Value = e.ExtraParams["AmountId"];
            
            CUSTOMER_SURVEY_THRESH_AMT DollarToEdit;
            using (Entities _context = new Entities())
            {
                DollarToEdit = CUSTOMER_SURVEYS.GetOrganizationThresholdAmount(decimal.Parse(e.ExtraParams["AmountId"].ToString()), _context);
            }

            uxLowDollar.Value = DollarToEdit.LOW_DOLLAR_AMT;
            uxHighDollar.Value = DollarToEdit.HIGH_DOLLAR_AMT;

            uxAddEditDollarWindow.Show();
        }

        protected void deDeleteDollar(object sender, DirectEventArgs e)
        {
            decimal AmountId = decimal.Parse(e.ExtraParams["AmountId"]);
            List<CUSTOMER_SURVEY_THRESHOLDS> ThresholdCheck;
            CUSTOMER_SURVEY_THRESH_AMT ToBeDeleted;
            using (Entities _context = new Entities())
            {
                ThresholdCheck = CUSTOMER_SURVEYS.GetThresholdPercentages(AmountId, _context).ToList();
                ToBeDeleted = CUSTOMER_SURVEYS.GetOrganizationThresholdAmount(AmountId, _context);
            }

            if (ThresholdCheck.Count > 0)
            {
                X.Msg.Alert("Contains Percentages", "Please delete all percentage thresholds before deleting the dollar range").Show();
            }
            else
            {
                GenericData.Delete<CUSTOMER_SURVEY_THRESH_AMT>(ToBeDeleted);
                uxDollarStore.Reload();
            }
            
            
        }

        protected void deDeleteThreshold(object sender, DirectEventArgs e)
        {
            decimal ThresholdId = decimal.Parse(e.ExtraParams["ThresholdId"]);
            CUSTOMER_SURVEY_THRESHOLDS ToBeDeleted;
            using (Entities _context = new Entities())
            {
                 ToBeDeleted = CUSTOMER_SURVEYS.GetThreshold(ThresholdId, _context);
            }

            GenericData.Delete<CUSTOMER_SURVEY_THRESHOLDS>(ToBeDeleted);
            uxThresholdStore.Reload();
        }

        protected void deLoadThresholdForm(object sender, DirectEventArgs e)
        {
            uxThresholdFormType.Value = "Edit";
            List<CUSTOMER_SURVEY_THRESHOLDS> Threshold = JSON.Deserialize<List<CUSTOMER_SURVEY_THRESHOLDS>>(e.ExtraParams["RowValues"]);
            foreach (CUSTOMER_SURVEY_THRESHOLDS ThresholdToEdit in Threshold)
            {
                uxThreshold.Value = ThresholdToEdit.THRESHOLD;
                uxThresholdId.Value = ThresholdToEdit.THRESHOLD_ID;
                uxAddEditThresholdWindow.Show();
            }
        }
    }
}