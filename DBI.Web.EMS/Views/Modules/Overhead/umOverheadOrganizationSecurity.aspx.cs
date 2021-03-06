﻿using System;
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

                //uxOrganizationsGrid.GetStore().Reload();

            }
        }

        protected void deCloseFiscal(object sender, DirectEventArgs e)
        {

            string _selectedRecordID = Request.QueryString["orgid"];
            string _selectedLeID = Request.QueryString["leID"];

            string url = "umOpenBudgetType.aspx?leID=" + _selectedLeID + "&combinedLEORGID=" + _selectedRecordID + "&closefiscal=Y";

            Window win = new Window
            {
                ID = "uxOpenBudgetTypeWindow",
                Title = "Close Fiscal Year",
                Height = 350,
                Width = 350,
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

            win.Render(this.Form);
            win.Show();
        }

        protected void dePeriodMaintenance(object sender, DirectEventArgs e)
        {

            string _selectedRecordID = Request.QueryString["orgid"];
            string _selectedLeID = Request.QueryString["leID"];

            string url = "umOpenBudgetType.aspx?leID=" + _selectedLeID + "&combinedLEORGID=" + _selectedRecordID;

            Window win = new Window
            {
                ID = "uxOpenBudgetTypeWindow",
                Title = "Create Next Budget Type",
                Height = 250,
                Width = 350,
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

            win.Render(this.Form);
            win.Show();

        }

        protected void deImportActuals(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];
            string _selectedLeID = Request.QueryString["leID"];


            string url = "umImportActualsWindow.aspx?AdminImport=Y" + "&combinedLEORGID=" + _selectedRecordID;

            Window win = new Window
            {
                ID = "uxImportActualsWn",
                Title = "Import Actuals from General Ledger",
                Height = 250,
                Width = 400,
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

            win.Render(this.Form);
            win.Show();

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

            X.Js.Call("parent.App.direct.AddTabPanel", "bt", _organizationDetails.ORGANIZATION_NAME + " - Budget Types", "umOverheadBudgetTypes.aspx?leid=" + _selectedRecordID + "&bulevel=Y");
        }


        protected void deLoadOrganizationsByHierarchy(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];

                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[1].ToString());
                long _organizationID = long.Parse(_selectedID[0].ToString());

                var data = HR.OverheadOrganizationStatusByHierarchy(_hierarchyID, _organizationID);

                if (!uxShowAllOrganizationsCheckBox.Checked)
                {
                    data = data.Where(x => x.ORGANIZATION_STATUS == "Active").ToList();
                }
    
                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION_V1>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
        }

        protected void deExecuteCommand(object sender, DirectEventArgs e)
        {
            string organizationName = e.ExtraParams["Name"];
            string organizationID = e.ExtraParams["ID"];
            string command = e.ExtraParams["command"];

            string _selectedRecordID = Request.QueryString["orgid"];
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);

            if (command == "Budgets")
            {
                X.Js.Call("parent.App.direct.AddTabPanel", "vp_" + organizationID, organizationName + " - " + "Budget Versions", "~/Views/Modules/Overhead/umOverheadBudgetPeriods.aspx?orgID=" + organizationID + "&leID=" + _selectedID[0].ToString());  
            }
            else
            {
                X.Js.Call("parent.App.direct.AddTabPanel", "gla_" + organizationID, organizationName + " - " + "General Ledger Accounts", "~/Views/Modules/Overhead/umOverheadGeneralLedger.aspx?orgID=" + organizationID);  
            }

        }


        protected void deSelectOrganization(object sender, DirectEventArgs e)
        {

            if (uxOrganizationsGridSelectionModel.SelectedRows.Count() > 0)
            {
                uxEnableOrganizationButton.Enable();
                uxDisableOrganizationButton.Enable();
                //uxGeneralLedger.Enable();
                //uxOpenPeriod.Enable();
            }
            else
            {
                uxEnableOrganizationButton.Disable();
                uxDisableOrganizationButton.Disable();
                //uxGeneralLedger.Disable();
                //uxOpenPeriod.Disable();
            }

        }

        protected void deDeSelectOrganization(object sender, DirectEventArgs e)
        {
            if (uxOrganizationsGridSelectionModel.SelectedRows.Count() > 0)
            {
                uxEnableOrganizationButton.Enable();
                uxDisableOrganizationButton.Enable();
                //uxGeneralLedger.Enable();
                //uxOpenPeriod.Enable();
            }
            else
            {
                uxEnableOrganizationButton.Disable();
                uxDisableOrganizationButton.Disable();
                //uxGeneralLedger.Disable();
                //uxOpenPeriod.Disable();
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