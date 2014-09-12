using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Web;
using System.Text;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umBudgetVersions : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                //For Admin we need to show all organizations
                if (validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    string _selectedRecordID = Request.QueryString["orgid"];
                    if (_selectedRecordID != null)
                    {
                        uxViewAllBudgets.Checked = true;
                        uxViewAllBudgets.Hidden = true;
                    }
                }
            }
        }


        protected void loadBudgetsForUser(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {

                List<OVERHEAD_BUDGET_FORECAST.BUDGET_VERSION> _data = new List<OVERHEAD_BUDGET_FORECAST.BUDGET_VERSION>();

            //For Admin we need to show all organizations
                if (validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    string _selectedRecordID = Request.QueryString["orgid"];

                    if (_selectedRecordID != null)
                    {

                        char[] _delimiterChars = { ':' };
                        string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                        long _hierarchyID = long.Parse(_selectedID[1].ToString());
                        long _organizationID = long.Parse(_selectedID[0].ToString());

                        _data = OVERHEAD_BUDGET_FORECAST.OrganizationsByHierarchy(_context, _hierarchyID, _organizationID, true);
                    }
                    else
                    {
                        _data = OVERHEAD_BUDGET_FORECAST.OrganizationsByHierarchy(_context);
                    }
                }
                else
                {
                    _data = OVERHEAD_BUDGET_FORECAST.OrganizationsByHierarchy(_context);
                }

            if (uxHideClosedBudgetsCheckbox.Checked)
                _data = _data.Where(x => x.BUDGET_STATUS == "Open" || x.BUDGET_STATUS == "Pending").ToList();

            if (!uxViewAllBudgets.Checked)
            {
                long _baseOrganizationID = SYS_USER_INFORMATION.UserByUserName(User.Identity.Name).CURRENT_ORG_ID;
                _data = _data.Where(x => x.ORGANIZATION_ID == _baseOrganizationID).ToList();
            }


            int count;
            uxBudgetVersionByOrganizationStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_FORECAST.BUDGET_VERSION>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
            e.Total = count;

            }

        }

        protected void selectOrganization(object sender, DirectEventArgs e)
        {
            string _organization_id = e.ExtraParams["ORGANIZATION_ID"];
            string _organization_name = e.ExtraParams["ORGANIZATION_NAME"];
            string _fiscalYear = e.ExtraParams["FISCAL_YEAR"];
            string _description = e.ExtraParams["BUDGET_DESCRIPTION"];
            string _accountRange = e.ExtraParams["ACCOUNT_RANGE"];
            string _budget_id = uxBudgetVersionByOrganizationSelectionModel.SelectedRow.RecordID;

            X.Js.Call("parent.App.direct.AddTabPanel", "bmw" + _organization_id + _fiscalYear + _budget_id, _organization_name + " - " + "Budget Maintenance / " + _fiscalYear + " / " + _description + " (" + _accountRange + ")", "~/Views/Modules/Overhead/umEditBudgetVersion.aspx?orgid=" + _organization_id + "&fiscalyear=" + _fiscalYear + "&budget_id=" + _budget_id + "&description=" + _organization_name + " / " + _fiscalYear + " / " + _description + " (" + _accountRange + ")");
        }


    }

   

}