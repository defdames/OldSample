using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using DBI.Core.Web;
using Ext.Net;
using System.Text;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umMassUpdateForecast : BasePage
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

        protected void deLoadBudgetNames(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];

            if (_selectedRecordID != null)
            {

                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[1].ToString());
                long _organizationID = long.Parse(_selectedID[0].ToString());


                using (Entities _context = new Entities())
                {
                    var _data = _context.OVERHEAD_BUDGET_TYPE.Where(x => x.LE_ORG_ID == _organizationID).ToList();
                    if (_data.Count() > 0)
                    {
                        uxBudgetNameStore.DataSource = _data;
                        uxOpenPeriod.Enable();
                    }
                }

            }
        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        protected void deExecuteCommand(object sender, DirectEventArgs e)
        {
            string organizationName = e.ExtraParams["Name"];
            string organizationID = e.ExtraParams["ID"];
            string command = e.ExtraParams["command"];

            string _selectedRecordID = Request.QueryString["orgid"];
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);

            if (command == "periods")
            {

                string url = "~/Views/Modules/Overhead/umOverheadBudgetPeriods.aspx?orgID=" + organizationID + "&leID=" + _selectedID[0].ToString();
                Window win = new Window
                {
                    ID = "uxShowAccountRangeWindow",
                    Title = organizationName + " - " + "Budget Periods",
                    Height = 600,
                    Width = 750,
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

                win.Render(this.Form);

                win.Show();


            }

        }

        protected void uxBudgetVersionByOrganizationStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            List<long> OrgsList = new List<long>();

                string _selectedRecordID = Request.QueryString["orgid"];

                if (_selectedRecordID != null)
                {

                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    long _hierarchyID = long.Parse(_selectedID[1].ToString());
                    long _organizationID = long.Parse(_selectedID[0].ToString());

                    OrgsList = HR.ActiveOverheadOrganizations().Select(x => x.ORGANIZATION_ID).ToList();     

                }


            List<OVERHEAD_ORG_BUDGETS_V> _budgetsByOrganizationIDList = new List<OVERHEAD_ORG_BUDGETS_V>();

            StringBuilder _rangeString = new StringBuilder();
            List<OVERHEAD_ORG_BUDGETS_V> _budgetList  = new List<OVERHEAD_ORG_BUDGETS_V>();

            using (Entities _context = new Entities())
            {
                foreach (long _orgID in OrgsList)
                {
                    _rangeString.Clear();

                        List<OVERHEAD_GL_RANGE> _accountRanges = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _orgID).ToList();
                        foreach (OVERHEAD_GL_RANGE _range in _accountRanges)
                        {
                            if (!_rangeString.ToString().Contains(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString()))
                                _rangeString.AppendLine(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString());
                        }

                        OVERHEAD_ORG_BUDGETS_V _data = new OVERHEAD_ORG_BUDGETS_V();
                       _data.ORGANIZATION_ID = _orgID;
                       _data.ORGANIZATION_NAME = HR.Organization(_orgID).ORGANIZATION_NAME;
                       _data.ACCOUNT_RANGE = _rangeString.ToString();
                       _budgetsByOrganizationIDList.Add(_data);
                }

            }

            int count;
            uxBudgetVersionByOrganizationStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;
        }
    }
}