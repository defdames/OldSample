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

        protected void deShowBudgetTypes(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[1].ToString());
            long _organizationID = long.Parse(_selectedID[0].ToString());

            HR.ORGANIZATION _organizationDetails = HR.Organization(_organizationID);

            X.Js.Call("parent.App.direct.AddTabPanel", "bt", _organizationDetails.ORGANIZATION_NAME + " - Budget Types", "umOverheadBudgetTypes.aspx?leid=" + _selectedRecordID);
        }


        protected void deLoadOrganizationsByHierarchy(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];

                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[1].ToString());
                long _organizationID = long.Parse(_selectedID[0].ToString());

                var data = HR.OverheadOrganizationStatusByHierarchy(_hierarchyID, _organizationID);

                if (e.Parameters.Count > 4)
                {
                    if (e.Parameters["TOGGLE_ACTIVE"] == "Y")
                    {
                        data = data.Where(x => x.ORGANIZATION_STATUS == "Active").ToList();
                    }
                }
                else
                    {
                        data = data.Where(x => x.ORGANIZATION_STATUS == "Active").ToList();
                    }
            
                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION_V1>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
        }

        protected void deViewPeriods(object sender, DirectEventArgs e)
        {
            string organizationName = e.ExtraParams["Name"];
            string organizationID = e.ExtraParams["ID"];

            string _selectedRecordID = Request.QueryString["orgid"];
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);

            X.Js.Call("parent.App.direct.AddTabPanel", "vp_" + organizationID, organizationName + " - " + "Budget Versions", "~/Views/Modules/Overhead/umOverheadBudgetPeriods.aspx?orgID=" + organizationID + "&leID=" + _selectedID[0].ToString());  
        }

        protected void deViewAccounts(object sender, DirectEventArgs e)
        {
            string organizationName = e.ExtraParams["Name"];
            string organizationID = e.ExtraParams["ID"];

            string _selectedRecordID = Request.QueryString["orgid"];
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);

            X.Js.Call("parent.App.direct.AddTabPanel", "gla_" + organizationID, organizationName + " - " +"General Ledger Accounts", "~/Views/Modules/Overhead/umOverheadGeneralLedger.aspx?orgID=" + organizationID);  
        }

        protected void deSelectOrganization(object sender, DirectEventArgs e)
        {

            if (uxOrganizationsGridSelectionModel.SelectedRows.Count() > 0)
            {
                uxEnableOrganizationButton.Enable();
                uxDisableOrganizationButton.Enable();
                uxGeneralLedger.Enable();
                uxOpenPeriod.Enable();
            }
            else
            {
                uxEnableOrganizationButton.Disable();
                uxDisableOrganizationButton.Disable();
                uxGeneralLedger.Disable();
                uxOpenPeriod.Disable();
            }

        }

        protected void deDeSelectOrganization(object sender, DirectEventArgs e)
        {
            if (uxOrganizationsGridSelectionModel.SelectedRows.Count() > 0)
            {
                uxEnableOrganizationButton.Enable();
                uxDisableOrganizationButton.Enable();
                uxGeneralLedger.Enable();
                uxOpenPeriod.Enable();
            }
            else
            {
                uxEnableOrganizationButton.Disable();
                uxDisableOrganizationButton.Disable();
                uxGeneralLedger.Disable();
                uxOpenPeriod.Disable();
            }
        }

        protected void deShowAllOrganizations(object sender, DirectEventArgs e)
        {
            if (uxShowAllOrganizationsCheckBox.Checked)
            {
                Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

                Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
                _p.Mode = ParameterMode.Value;
                _p.Name = "TOGGLE_ACTIVE";
                _p.Value = "N";
                ps.Add(_p);

                uxOrganizationSecurityStore.Reload(ps);
            }
            else
            {
                Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

                Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
                _p.Mode = ParameterMode.Value;
                _p.Name = "TOGGLE_ACTIVE";
                _p.Value = "Y";
                ps.Add(_p);

                uxOrganizationSecurityStore.Reload(ps);
            }
        }

       

    }
}