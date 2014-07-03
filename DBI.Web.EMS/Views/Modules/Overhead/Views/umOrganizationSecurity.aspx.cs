using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
{
    public partial class umOrganizationSecurity : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        protected void deLoadOracleHierarchy(object sender, NodeLoadEventArgs e)
        {
                //Load Legal Entities
                if (e.NodeID == "0")
                {
                    var data = HR.ActiveOverheadBudgetLegalEntities();

                    //Build the treepanel
                    foreach (var view in data)
                    {
                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        e.Nodes.Add(node);
                    }
                }
                else
                {
                    long nodeID = long.Parse(e.NodeID);

                    //Load Hierarchies for LE
                    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID).ToList();

                    //Build the treepanel
                    foreach (var view in data)
                    {
                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), view.ORGANIZATION_STRUCTURE_ID.ToString());
                        node.Leaf = true;
                        e.Nodes.Add(node);
                    }

                }
        }


        protected void deShowOrganizationsByHierarchy(object sender, DirectEventArgs e)
        {

                string selectedRecordID = uxLegalEntitySelectionModel.SelectedRecordID;
                if (selectedRecordID != "0" && selectedRecordID.Contains(":"))
                {

                    char[] delimiterChars = { ':' };
                    string[] selectedID = selectedRecordID.Split(delimiterChars);
                    long hierarchyID = long.Parse(selectedID[1].ToString());
                    long organizationID = long.Parse(selectedID[0].ToString());

                    uxOrganizationSecurityStore.RemoveAll();
                    uxOrganizationsGridFilter.ClearFilter();
                }

        }


        protected void deEnableOrganization(object sender, DirectEventArgs e)
        {
            RowSelectionModel model = uxOrganizationsGridSelectionModel;

            foreach (SelectedRow row in model.SelectedRows)
            {
               SYS_ORG_PROFILE_OPTIONS.SetOrganizationProfileOption("OverheadBudgetOrganization", "Y", long.Parse(row.RecordID));
            }
            uxOrganizationsGridSelectionModel.ClearSelection();
            uxOrganizationSecurityStore.Reload();
        }

        protected void deDisableOrganization(object sender, DirectEventArgs e)
        {
            RowSelectionModel model = uxOrganizationsGridSelectionModel;

            foreach (SelectedRow row in model.SelectedRows)
            {
                SYS_ORG_PROFILE_OPTIONS.SetOrganizationProfileOption("OverheadBudgetOrganization", "N", long.Parse(row.RecordID));
            }
            uxOrganizationsGridSelectionModel.ClearSelection();
            uxOrganizationSecurityStore.Reload();
        }


        protected void deLoadOrganizationsByHierarchy(object sender, StoreReadDataEventArgs e)
        {
                string _selectedRecordID = uxLegalEntitySelectionModel.SelectedRecordID;

                if (_selectedRecordID != "0")
                {
                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    long _hierarchyID = long.Parse(_selectedID[1].ToString());
                    long _organizationID = long.Parse(_selectedID[0].ToString());

                    var data = HR.OverheadOrganizationStatusByHierarchy(_hierarchyID, _organizationID);

                    int count;
                    uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION_V1>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
        }

       
    }
}