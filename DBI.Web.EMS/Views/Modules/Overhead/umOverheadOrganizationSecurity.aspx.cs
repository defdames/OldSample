using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Core.Web;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadOrganizationSecurity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                uxOrganizationsGrid.GetStore().Reload();

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
            string _selectedRecordID = Request.QueryString["orgid"];

                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[1].ToString());
                long _organizationID = long.Parse(_selectedID[0].ToString());

                var data = HR.OverheadOrganizationStatusByHierarchy(_hierarchyID, _organizationID);

                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION_V1>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
        }

        protected void deViewAccounts(object sender, DirectEventArgs e)
        {
            string organizationName = e.ExtraParams["Name"];
            string organizationID = e.ExtraParams["ID"];

            X.Js.Call("parent.App.direct.AddTabPanel", "gla_" + organizationID, organizationName + " - " +"General Ledger Accounts", "~/Views/Modules/Overhead/umOverheadGeneralLedger.aspx?orgID=" + organizationID);         
        }

        protected void deLoadForcastPeriodsByOrganization(object sender, StoreReadDataEventArgs e)
        {

            List<OVERHEAD_ORG_BUDGETS_V> _budgetsByOrganizationIDList = new List<OVERHEAD_ORG_BUDGETS_V>();

            using (Entities _context = new Entities())
            {
                RowSelectionModel model = uxOrganizationsGridSelectionModel;
                foreach (SelectedRow row in model.SelectedRows)
                {
                long _selectedRowID = long.Parse(row.RecordID);
                _budgetsByOrganizationIDList = OVERHEAD_ORG_BUDGETS.BudgetListByOrganizationID(_selectedRowID, _context).ToList();
                }
            }

            int count;
            uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;

            if (_budgetsByOrganizationIDList.Count == 0)
            {
                uxOpenPeriod.Enable();
            }
            else
            {
                uxOpenPeriod.Disable();
            }
        }


        protected void deSelectOrganization(object sender, DirectEventArgs e)
        {
           
            if(uxOrganizationsGridSelectionModel.SelectedRows.Count() > 0)
            {
                uxEnableOrganizationButton.Enable();
                uxDisableOrganizationButton.Enable();
            }
            else
            {
                uxEnableOrganizationButton.Disable();
                uxDisableOrganizationButton.Disable();
            }

            uxForecastPeriodsByOrganization.Reload();
           
        }

        protected void deDeSelectOrganization(object sender, DirectEventArgs e)
        {
            if (uxOrganizationsGridSelectionModel.SelectedRows.Count() > 0)
            {
                uxEnableOrganizationButton.Enable();
                uxDisableOrganizationButton.Enable();
            }
            else
            {
                uxEnableOrganizationButton.Disable();
                uxDisableOrganizationButton.Disable();
            }

            uxForecastPeriodsByOrganization.Reload();
        }

        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);

            string url = "umOpenBudgetType.aspx?leID=" + _selectedID[0].ToString();

            Window win = new Window
            {
                ID = "uxOpenBudgetTypeWindow",
                Title = "Open Budget Type",
                Height = 250,
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

            win.Listeners.Close.Handler = "#{uxForecastPeriodsByOrganizationGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }

    }
}