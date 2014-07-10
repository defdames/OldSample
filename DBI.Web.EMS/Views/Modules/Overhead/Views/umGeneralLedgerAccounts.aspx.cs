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
            uxGLAccountRangeStore.Reload();
            uxShowGLRangeWindow.Enable();
        }

        protected void deLoadGLAccountRange(object sender, StoreReadDataEventArgs e)
        {
            long _organizationSelected = long.Parse(uxOrganizationsGridRowSelection.SelectedRecordID);

            using(Entities _context = new Entities())
            {
                var data = OVERHEAD_GL_ACCOUNT.OverheadGLAccountsByOrganization(_context, _organizationSelected);
                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_GL_ACCOUNT>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;

            }

        }

        protected void deShowRangeWindow(object sender, DirectEventArgs e)
        {
            string url = "/Views/Modules/Overhead/Views/umAddGlAccountRange.aspx";
            Window win = new Window
            {
                ID = "uxShowAccountRangeWindow",
                Title = "General Ledger Account Range Filter",
                Height = 750,
                Width = 900,
                Modal = true,
                Resizable = true,
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

            win.Listeners.Close.Handler = "#{uxGLAccountRangeGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }



    }
}