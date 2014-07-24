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
    }
}