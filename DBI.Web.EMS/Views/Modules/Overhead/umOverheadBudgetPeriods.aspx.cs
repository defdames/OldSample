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
            uxForecastPeriodsByOrganization.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList.OrderByDescending(x => x.ORG_BUDGET_ID).ToList(), out count);
            e.Total = count;

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
            uxDelete.Disable();
            uxImportActuals.Disable();
            uxEditBudget.Disable();

        }

        protected void deDeletePeriod(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                foreach (SelectedRow row in uxForecastPeriodsByOrganizationSelectionModel.SelectedRows)
                {
                    long _budgetID = long.Parse(row.RecordID);

                    OVERHEAD_ORG_BUDGETS _budget = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetID).SingleOrDefault();

                    List<OVERHEAD_ACCOUNT_COMMENT> _accountComments = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budgetID).ToList();

                    if (_budget != null)
                    {
                        //First get all the details and remove them, then delete the actual budget
                        List<OVERHEAD_BUDGET_DETAIL> _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetID).ToList();
                        GenericData.Delete<OVERHEAD_BUDGET_DETAIL>(_detail);
                        GenericData.Delete<OVERHEAD_ORG_BUDGETS>(_budget);
                        GenericData.Delete<OVERHEAD_ACCOUNT_COMMENT>(_accountComments);
                    }

                }
            }

            uxForecastPeriodsByOrganization.Reload();
            uxForecastPeriodsByOrganizationSelectionModel.DeselectAll(true);
            uxOpenPeriod.Disable();
            uxClosePeriod.Disable();
            uxDelete.Disable();
            uxImportActuals.Disable();
            uxEditBudget.Disable();

        }

        protected void deImportActuals(object sender, DirectEventArgs e)
        {

            short _fiscal_year = short.Parse(e.ExtraParams["FISCAL_YEAR"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(e.ExtraParams["ORG_BUDGET_ID"]);

            string url = "umImportActualsWindow.aspx?AdminImport=Y&budget_id=" + _budgetid + "&orgid=" + _organizationID + "&fiscalyear=" + _fiscal_year;

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

            //win.Listeners.Close.Handler = "#{uxOrganizationAccountGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();

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
            uxDelete.Disable();
            uxImportActuals.Disable();
            uxEditBudget.Disable();
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

            win.Listeners.Close.Handler = "#{uxForecastPeriodsByOrganizationGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }


        protected void deSelectForecast(object sender, DirectEventArgs e)
        {
            long _budgetid = long.Parse(e.ExtraParams["ORG_BUDGET_ID"]);

            if (uxForecastPeriodsByOrganizationSelectionModel.SelectedRows.Count() > 0)
            {
                using (Entities _context = new Entities())
                {
                    //Return the budget 
                    OVERHEAD_ORG_BUDGETS _budget = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetid);
                    var _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(_budget.OVERHEAD_BUDGET_TYPE_ID);

                    if (_budgetType.IMPORT_ACTUALS_ALLOWED == "N")
                    {
                        uxImportActuals.Disable();
                    }
                    else
                    {
                        uxImportActuals.Enable();
                    }
                }

                uxOpenPeriod.Enable();
                uxClosePeriod.Enable();
                uxDelete.Enable();
                uxEditBudget.Enable();
            }
            else
            {
                uxOpenPeriod.Disable();
                uxClosePeriod.Disable();
                uxDelete.Disable();
                uxImportActuals.Disable();
                uxEditBudget.Disable();
            }

        }

        protected void deDeSelectForecast(object sender, DirectEventArgs e)
        {
            if (uxForecastPeriodsByOrganizationSelectionModel.SelectedRows.Count() > 0)
            {
                uxOpenPeriod.Enable();
                uxClosePeriod.Enable();
                uxDelete.Enable();
                uxImportActuals.Enable();
                uxEditBudget.Enable();
            }
            else
            {
                uxOpenPeriod.Disable();
                uxClosePeriod.Disable();
                uxDelete.Disable();
                uxImportActuals.Disable();
                uxEditBudget.Disable();
            }
        }


        protected void deEditBudget(object sender, DirectEventArgs e)
        {
            long _budgetID = long.Parse(e.ExtraParams["ORG_BUDGET_ID"]);
            using (Entities _context = new Entities())
            {
                OVERHEAD_ORG_BUDGETS _budgetHeader = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetID);

                string _organization_id = _budgetHeader.ORGANIZATION_ID.ToString();
                string _organization_name = HR.Organization(_budgetHeader.ORGANIZATION_ID).ORGANIZATION_NAME;
                string _fiscalYear = _budgetHeader.FISCAL_YEAR.ToString();
                string _description = e.ExtraParams["BUDGET_DESCRIPTION"];
                string _accountRange = OVERHEAD_BUDGET_FORECAST.AccountRangeByOrganizationID(_context, _budgetHeader.ORGANIZATION_ID);
                string _budget_id = _budgetID.ToString();

                X.Js.Call("parent.App.direct.AddTabPanel", "bmw" + _organization_id + _fiscalYear + _budget_id, _organization_name + " - " + "Budget Maintenance / " + _fiscalYear + " / " + _description + " (" + _accountRange + ")", "~/Views/Modules/Overhead/umEditBudgetVersion.aspx?orgid=" + _organization_id + "&fiscalyear=" + _fiscalYear + "&budget_id=" + _budget_id + "&description=" + _organization_name + " / " + _fiscalYear + " / " + _description + " (" + _accountRange + ")");
            }

        }
    }
}