using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
{
    public partial class umBudgetTypesByLegalEntity : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }

        protected void deLoadOverheadLegalEntities(object sender, NodeLoadEventArgs e)
        {
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

        protected void deReadBudgetTypesByLegalEntity(object sender, StoreReadDataEventArgs e)
        {

                long _organizationID;

                RowSelectionModel selection = uxLegalEntityTreeSelectionModel;
                Boolean check = long.TryParse(selection.SelectedRecordID, out _organizationID);

                if (_organizationID > 0) 
                {
                        List<OVERHEAD_BUDGET_TYPE> data = OVERHEAD_BUDGET_TYPE.BudgetTypes(_organizationID);
                        uxBudgetTypeStore.DataSource = data;
                        uxAssignBudgetType.Disabled = false;
                }
           

        }

        protected void deShowBudgetTypesByLegalEntity(object sender, DirectEventArgs e)
        {
           
                string selectedRecordID = uxLegalEntityTreeSelectionModel.SelectedRecordID;
                if (selectedRecordID != "0")
                {
                    uxBudgetTypeStore.Reload();
                }
           
        }

        protected void deUnassignBudgetType(object sender, DirectEventArgs e)
        {
                long _budgetTypeIDSelected;

                RowSelectionModel _rsm = uxBudgetTypeSelectionModel;
                Boolean _check = long.TryParse(_rsm.SelectedRecordID, out _budgetTypeIDSelected);

                OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(_budgetTypeIDSelected);
                GenericData.Delete<OVERHEAD_BUDGET_TYPE>(_budgetType);

                uxBudgetTypeStore.Reload();
        }

        protected void deAddEditBudgetType(object sender, DirectEventArgs e)
        {
                long _businessUnitID;

                RowSelectionModel selection = uxLegalEntityTreeSelectionModel;
                Boolean checkOrganization = long.TryParse(selection.SelectedRecordID, out _businessUnitID);
                string _editMode = e.ExtraParams["Edit"];


                string url = "/Views/Modules/Overhead/Views/umAddRemoveBudgetType.aspx?buID=" + _businessUnitID;
                RowSelectionModel _recordID = uxBudgetTypeSelectionModel;

                if (!string.IsNullOrEmpty(_editMode))
                {
                    url = url + "&recordId=" + _recordID.SelectedRecordID.ToString();
                }

                Window win = new Window
                {
                    ID = "uxAddEditBudgetType",
                    Title = "Budget Types",
                    Height = 200,
                    Width = 550,
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

    }
}