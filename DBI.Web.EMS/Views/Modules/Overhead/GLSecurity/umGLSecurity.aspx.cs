using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Data.Oracle;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umGLSecurity : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void deReadOrganizationsByHierarchy(object sender, StoreReadDataEventArgs e)
        {
            try
            {
                string _selectedRecordID = uxHierarchyTreeSelectionModel.SelectedRecordID;

                if (_selectedRecordID != "0")
                {
                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    long _hierarchyID = long.Parse(_selectedID[1].ToString());
                    long _organizationID = long.Parse(_selectedID[0].ToString());

                    var data = HR.ActiveOverheadOrganizationsByHierarchy(_hierarchyID, _organizationID);

                    int count;
                    uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION_V1>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
            catch (Exception)
            {
                throw;
            }
                
        }

        protected void LoadHierarchyTree(object sender, NodeLoadEventArgs e)
        {
            try
            {
                //Load LEs
                if (e.NodeID == "0")
                {
                    var data = HR.LegalEntityHierarchies().Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();

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
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
           
        }

        protected void deShowOrganizationsByHierarchy(object sender, DirectEventArgs e)
        {
            try
            {
                string selectedRecordID = uxHierarchyTreeSelectionModel.SelectedRecordID;
                if (selectedRecordID != "0" && selectedRecordID.Contains(":"))
                {

                    char[] delimiterChars = { ':' };
                    string[] selectedID = selectedRecordID.Split(delimiterChars);
                    long hierarchyID = long.Parse(selectedID[1].ToString());
                    long organizationID = long.Parse(selectedID[0].ToString());

                    uxOrganizationSecurityStore.RemoveAll();
                    uxOrganizationsGridFilter.ClearFilter();

                    uxGlAccountSecurityStore.RemoveAll();
                    uxGlAccountSecurityGridFilter.ClearFilter();
                    uxGlAccountSecurityGrid.Refresh();
                }
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
            
        }

        protected void deReadGLSecurityByOrganization(object sender, StoreReadDataEventArgs e)
        {
            try
            {
                long _organizationID;

                RowSelectionModel selection = uxOrganizationSelectionModel;
                Boolean check = long.TryParse(selection.SelectedRecordID, out _organizationID);

                if (_organizationID > 0)
                {
                    var data = OVERHEAD_GL_ACCOUNT.AccountsByLegalEntity(_organizationID);
                    int count;
                    uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_GL_ACCOUNT.GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

            

        protected void deOrganizationSelect(object sender, DirectEventArgs e)
        {
            try
            {
                uxGlAccountSecurityStore.RemoveAll();
                uxGlAccountSecurityGridFilter.ClearFilter();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
          
        }

        protected void deShowGLAccounts(object sender, DirectEventArgs e)
        {
            try
            {
                long OrganizationID;

                RowSelectionModel selection = uxOrganizationSelectionModel;
                Boolean checkOrganization = long.TryParse(selection.SelectedRecordID, out OrganizationID);

                Window win = new Window
                {
                    ID = "uxGlAccounts",
                    Title = "GL Accounts",
                    Height = 650,
                    Width = 750,
                    Modal = true,
                    CloseAction = CloseAction.Destroy,
                    Loader = new ComponentLoader
                    {
                        Mode = LoadMode.Frame,
                        DisableCaching = true,
                        Url = "/Views/Modules/Overhead/GLSecurity/Add/umAddGlAccount.aspx?orgID=" + OrganizationID,
                        AutoLoad = true,
                        LoadMask =
                        {
                            ShowMask = true
                        }
                    }
                };

                win.Listeners.Close.Handler = "#{uxGlAccountSecurityGrid}.getStore().load();#{uxOrganizationsGrid}.getStore().load();";

                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
          
        }

        protected void deDeleteGLAccounts(object sender, DirectEventArgs e)
        {

            try
            {
                long _organizationID;

                RowSelectionModel _selection = uxOrganizationSelectionModel;
                Boolean _checkOrganization = long.TryParse(_selection.SelectedRecordID, out _organizationID);

                RowSelectionModel _model = uxGlAccountSecurityGridSelectionModel;

                foreach (SelectedRow _row in _model.SelectedRows)
                {
                    long _recordID = long.Parse(_row.RecordID);
                    OVERHEAD_GL_ACCOUNT.Delete(_recordID);
                }

                int recordCount = OVERHEAD_GL_ACCOUNT.GetCount(_organizationID);

                if (recordCount == 0)
                {
                    uxOrganizationSecurityStore.Reload();
                }

                uxGlAccountSecurityStore.RemoveAll();
                uxGlAccountSecurityStore.ClearFilter();
                uxGlAccountSecurityStore.Reload();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
           
        }

    }
}