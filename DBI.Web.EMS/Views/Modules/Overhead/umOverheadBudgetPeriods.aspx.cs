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
                _budgetsByOrganizationIDList = OVERHEAD_ORG_BUDGETS.BudgetListByOrganizationID(_selectedRowID, _context).OrderBy(x => x.ORG_BUDGET_ID).ToList();
            }

            int count;
            uxForecastPeriodsByOrganization.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;

                uxCreateBudget.Enable();
        }


        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                foreach (SelectedRow row in uxForecastPeriodsByOrganizationSelectionModel.SelectedRows)
                {
                    long _budgetID = long.Parse(row.RecordID);

                    OVERHEAD_ORG_BUDGETS _budget = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetID).SingleOrDefault();

                    if (_budget != null)
                    {
                        if (_budget.STATUS != "O")
                        {
                            _budget.STATUS = "O";
                            GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budget);
                        }
                    }
                }
            }

            uxForecastPeriodsByOrganization.Reload();
            uxForecastPeriodsByOrganizationSelectionModel.DeselectAll(true);
            uxOpenPeriod.Disable();
            uxClosePeriod.Disable();

        }       

        protected void deClosePeriod(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                foreach (SelectedRow row in uxForecastPeriodsByOrganizationSelectionModel.SelectedRows)
                {
                    long _budgetID = long.Parse(row.RecordID);

                    OVERHEAD_ORG_BUDGETS _budget = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetID).SingleOrDefault();

                    if (_budget != null)
                    {
                        if (_budget.STATUS != "C")
                        {
                            _budget.STATUS = "C";
                            GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budget);
                        }
                    }
                }
            }

            uxForecastPeriodsByOrganization.Reload();
            uxForecastPeriodsByOrganizationSelectionModel.DeselectAll(true);
            uxOpenPeriod.Disable();
            uxClosePeriod.Disable();
        }       



        protected void deCreateBudgetPeriod(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];
            string _selectedLeID = Request.QueryString["leID"];

            string url = "umOpenBudgetType.aspx?leID=" + _selectedLeID + "&orgID=" + _selectedRecordID;

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


        protected void deSelectForecast(object sender, DirectEventArgs e)
        {

            if (uxForecastPeriodsByOrganizationSelectionModel.SelectedRows.Count() > 0)
            {
                uxOpenPeriod.Enable();
                uxClosePeriod.Enable();
            }
            else
            {
                uxOpenPeriod.Disable();
                uxClosePeriod.Disable();
            }

        }

        protected void deDeSelectForecast(object sender, DirectEventArgs e)
        {
            if (uxForecastPeriodsByOrganizationSelectionModel.SelectedRows.Count() > 0)
            {
                uxOpenPeriod.Enable();
                uxClosePeriod.Enable();
            }
            else
            {
                uxOpenPeriod.Disable();
                uxClosePeriod.Disable();
            }
        }
    }
}