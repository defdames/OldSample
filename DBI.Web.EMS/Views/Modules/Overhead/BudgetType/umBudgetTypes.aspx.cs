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
    public partial class umBudgetTypes : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadLegalEntitiesTreePanel(object sender, NodeLoadEventArgs e)
        {
            try
            {
                //Load LEs
                if (e.NodeID == "0")
                {
                    List<HR.ORGANIZATION> _legalEntities = HR.ActiveOverheadBudgetLegalEntities();

                    //Build the treepanel
                    foreach (HR.ORGANIZATION view in _legalEntities)
                    {
                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
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

        protected void deReadBudgetTypesByOrganization(object sender, StoreReadDataEventArgs e)
        {
            try
            {
                long _organizationID;

                RowSelectionModel selection = uxLegalEntityTreeSelectionModel;
                Boolean check = long.TryParse(selection.SelectedRecordID, out _organizationID);

                if (_organizationID > 0)
                {
                    using (Entities _context = new Entities())
                    {
                        List<OVERHEAD_BUDGET_TYPE> data = OVERHEAD_BUDGET_TYPE.BudgetTypes(_organizationID);

                        int count;
                        uxBudgetTypeStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_TYPE>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                        e.Total = count;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        protected void deAddEditBudgetType(object sender, DirectEventArgs e)
        {
            try
            {
                long _businessUnitID;

                RowSelectionModel selection = uxLegalEntityTreeSelectionModel;
                Boolean checkOrganization = long.TryParse(selection.SelectedRecordID, out _businessUnitID);
                string _editMode = e.ExtraParams["Edit"];


                string url = "/Views/Modules/Overhead/BudgetType/AddEdit/umAddEditBudgetType.aspx?buID=" + _businessUnitID;
                RowSelectionModel _recordID = uxBudgetTypeSelectionModel;

                if (!string.IsNullOrEmpty(_editMode))
                {
                    url = url + "&recordId=" + _recordID.SelectedRecordID.ToString();
                }

                Window win = new Window
                {
                    ID = "uxAddEditBudgetType",
                    Title = "Budget Types",
                    Height = 350,
                    Width = 500,
                    Modal = true,
                    Resizable = false,
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

                win.Listeners.Close.Handler = "#{uxBudgetTypeGridPanel}.getStore().load();";

                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
        }
          
        protected void deShowBudgetTypesByBusinessUnit(object sender, DirectEventArgs e)
        {
            try
            {
                string selectedRecordID = uxLegalEntityTreeSelectionModel.SelectedRecordID;
                if (selectedRecordID != "0")
                {
                    uxBudgetTypeStore.RemoveAll();
                    uxBudgetTypeGridFilter.ClearFilter();
                    uxBudgetTypeGridPanel.Refresh();
                }
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
           
        }

    }
}