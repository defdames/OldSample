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
    public partial class umOverheadBudgetPeriods : BasePage
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

        protected void deLoadForcastPeriodsByOrganization(object sender, StoreReadDataEventArgs e)
        {
            string _selectedOrganizationId = Request.QueryString["orgid"];

            List<OVERHEAD_ORG_BUDGETS_V> _budgetsByOrganizationIDList = new List<OVERHEAD_ORG_BUDGETS_V>();

            using (Entities _context = new Entities())
            {
                long _selectedRowID = long.Parse(_selectedOrganizationId);
                _budgetsByOrganizationIDList = OVERHEAD_ORG_BUDGETS.BudgetListByOrganizationID(_selectedRowID, _context).ToList();
            }

            int count;
            uxForecastPeriodsByOrganization.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;
            uxOpenPeriod.Enable();

        }


        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];
            string _selectedLeID = Request.QueryString["leID"];

            string url = "umOpenBudgetType.aspx?leID=" + _selectedLeID + "&orgID=" + _selectedRecordID;

            Window win = new Window
            {
                ID = "uxOpenBudgetTypeWindow",
                Title = "Open Budget Type",
                Height = 300,
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