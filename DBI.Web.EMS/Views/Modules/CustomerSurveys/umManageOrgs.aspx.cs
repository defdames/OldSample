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

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umManageOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadForms(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
            {
                long _organizationID = GetOrgFromTree(_selectedRecordID);
                using (Entities _context = new Entities())
                {
                    List<CUSTOMER_SURVEY_FORMS> FormData = CUSTOMER_SURVEYS.GetForms(_context).Where(x => x.ORG_ID == _organizationID).ToList();
                    List<CUSTOMER_SURVEYS.CustomerSurveyForms> AllData = new List<CUSTOMER_SURVEYS.CustomerSurveyForms>();
                    foreach (CUSTOMER_SURVEY_FORMS ThisForm in FormData)
                    {
                        CUSTOMER_SURVEYS.CustomerSurveyForms NewForm = new CUSTOMER_SURVEYS.CustomerSurveyForms();
                        NewForm.FORMS_NAME = ThisForm.FORMS_NAME;
                        NewForm.ORG_ID = ThisForm.ORG_ID;
                        NewForm.FORM_ID = ThisForm.FORM_ID;
                        //var NumQuestions = CUSTOMER_SURVEYS.NumberOfQuestionsInForm(ThisForm.FORM_ID);

                        // NewForm.NUM_QUESTIONS = NumQuestions;
                        AllData.Add(NewForm);
                    }
                }
                //  uxFormsGridStore.DataSource = AllData;
            }
        }

        protected void deReadDollars(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
            {
                long OrgID = GetOrgFromTree(_selectedRecordID);
                using (Entities _context = new Entities())
                {
                    IQueryable<CUSTOMER_SURVEY_THRESH_AMT> Amounts = CUSTOMER_SURVEYS.GetOrganizationThresholdAmounts(OrgID, _context);
                    int count;
                    uxDollarStore.DataSource = GenericData.ListFilterHeader(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], Amounts, out count);
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

        protected void deLoadFormThreshold(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                decimal AmountId = decimal.Parse(e.ExtraParams["AmountId"]);
                CUSTOMER_SURVEY_THRESHOLDS Threshold = CUSTOMER_SURVEYS.GetThresholdPercentages(AmountId, _context).Single();
                if (Threshold != null)
                {
                    //uxSmallThreshold.Value = Threshold
                    //uxFirstLargeThreshold.Value = Threshold.LARGE_THRESHOLD1;
                    //uxSecondLargeThreshold.Value = Threshold.LARGE_THRESHOLD2;
                    //uxFormType.Value = "Edit";
                }
                else
                {
                    uxFormType.Value = "Add";
                    uxOrganizationForm.Reset();
                }
            }
        }

        protected void deSubmitThreshold(object sender, DirectEventArgs e)
        {
            if (uxFormType.Value.ToString() == "Add")
            {
                CUSTOMER_SURVEY_THRESHOLDS NewThreshold = new CUSTOMER_SURVEY_THRESHOLDS();
                //NewThreshold.SMALL_THRESHOLD = decimal.Parse(uxSmallThreshold.Text);
                //NewThreshold.LARGE_THRESHOLD1 = decimal.Parse(uxFirstLargeThreshold.Text);
                //NewThreshold.LARGE_THRESHOLD2 = decimal.Parse(uxSecondLargeThreshold.Text);
                NewThreshold.MODIFIED_BY = User.Identity.Name;
                NewThreshold.CREATED_BY = User.Identity.Name;
                NewThreshold.MODIFY_DATE = DateTime.Now;
                NewThreshold.CREATE_DATE = DateTime.Now;

                GenericData.Insert<CUSTOMER_SURVEY_THRESHOLDS>(NewThreshold);
            }
            else
            {
                decimal ThresholdID = decimal.Parse(e.ExtraParams["ThresholdId"]);
                CUSTOMER_SURVEY_THRESHOLDS UpdatedThreshold;
                using (Entities _context = new Entities())
                {
                    UpdatedThreshold = CUSTOMER_SURVEYS.GetThresholdPercentages(ThresholdID, _context).Single();
                }
                //UpdatedThreshold.SMALL_THRESHOLD = decimal.Parse(uxSmallThreshold.Text);
                //UpdatedThreshold.LARGE_THRESHOLD1 = decimal.Parse(uxFirstLargeThreshold.Text);
                //UpdatedThreshold.LARGE_THRESHOLD2 = decimal.Parse(uxSecondLargeThreshold.Text);
                UpdatedThreshold.MODIFY_DATE = DateTime.Now;
                UpdatedThreshold.MODIFIED_BY = User.Identity.Name;

                GenericData.Update<CUSTOMER_SURVEY_THRESHOLDS>(UpdatedThreshold);
            }
        }
    }
}