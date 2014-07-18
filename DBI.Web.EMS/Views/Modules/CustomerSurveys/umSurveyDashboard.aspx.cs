using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umSurveyDashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected long GetOrgFromTree(string _selectedRecordID)
        {
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            return long.Parse(_selectedID[1].ToString());
        }

        protected long GetHierarchyFromTree(string _selectedRecordID)
        {
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            return long.Parse(_selectedID[0].ToString());
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

        protected void deReadDashboard(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            List<long> OrgsList;
            if (_selectedRecordID != string.Empty)
            {
                List<long> ProjectList;
                long SelectedOrgId = GetOrgFromTree(_selectedRecordID);
                long HierId = GetHierarchyFromTree(_selectedRecordID);
                //Get Orgs list
                using (Entities _context = new Entities())
                {
                    OrgsList = HR.ActiveOrganizationsByHierarchy(HierId, SelectedOrgId, _context).Select(x => x.ORGANIZATION_ID).ToList();
                    //ProjectList = XXEMS.ProjectsByOrgHierarchy(OrgsList, _context).Select(x => x.PROJECT_ID).ToList();

                    var data = XXDBI_DW.JobCostbyProjectList(OrgsList, HierId, _context);//.Join(_context.CUSTOMER_SURVEY_THRESH_AMT, jc => jc.ORG_ID, tham => tham.ORG_ID, (jc, tham) => new { job_cost = jc, threshold = tham }).Where(x => x.job_cost.LEVEL_SORT == 8).Select(x => new Threshold { PROJECT_NAME = x.job_cost.PROJECT_NAME, PROJECT_NUMBER = x.job_cost.PROJECT_NUMBER, PERCENTAGE = (x.job_cost.BGT_GREC == 0 ? 0 : Math.Round((double)(x.job_cost.FY_GREC / x.job_cost.BGT_GREC * 100))) });
                    int count;

                    var Source = GenericData.ListFilterHeader<XXDBI_DW.Threshold>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    uxDashboardStore.DataSource = Source;
                    e.Total = count;
                }
            }

        }

        protected void deEmailLink(object sender, DirectEventArgs e)
        {
            List<XXDBI_DW.Threshold> RowData = JSON.Deserialize<List<XXDBI_DW.Threshold>>(e.ExtraParams["RowValues"]);

            //generate code to tie back to customer
            CUSTOMER_SURVEY_FORMS_COMP NewFormToSubmit = new CUSTOMER_SURVEY_FORMS_COMP();
            using (Entities _context = new Entities())
            {
                NewFormToSubmit.FORM_ID = CUSTOMER_SURVEYS.GetFormIdByOrg(RowData[0].ORG_ID, _context);
                NewFormToSubmit.CREATE_DATE = DateTime.Now;
                NewFormToSubmit.MODIFY_DATE = DateTime.Now;
                NewFormToSubmit.CREATED_BY = User.Identity.Name;
                NewFormToSubmit.MODIFIED_BY = User.Identity.Name;
            }

            GenericData.Insert<CUSTOMER_SURVEY_FORMS_COMP>(NewFormToSubmit);

            //generate link

            string QueryString = RSAClass.Encrypt(NewFormToSubmit.COMPLETION_ID.ToString());

            //get contact to email the link to

            //send email with link
            
        }

        protected void deEmailPDF(object sender, DirectEventArgs e)
        {
            //Get form for Organization
            

            //Get questions
            
        }

        protected void dePrintPDF(object sender, DirectEventArgs e)
        {

        }
    }

    
}