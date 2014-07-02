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
    public partial class umGeneralLedgerAccounts : DBI.Core.Web.BasePage
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

        protected void deLoadActiveOrganizations(object sender, StoreReadDataEventArgs e)
        {

            var data = HR.ActiveOverheadOrganizations();
            int count;
            uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            e.Total = count;
        }

        protected void deViewOrganizationGlAccounts(object sender, DirectEventArgs e)
        {
            long _organizationSelected = long.Parse(uxOrganizationsGridRowSelection.SelectedRecordID);


        }

    }
}