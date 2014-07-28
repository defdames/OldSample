using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadPeriods : DBI.Core.Web.BasePage
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

        protected void deLoadAllowedBudgetOrganizations(object sender, StoreReadDataEventArgs e)
        {
            long _selectedRecordID = long.Parse(Request.QueryString["leid"]);
            var _hierData = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == _selectedRecordID).ToList();
            List<HR.ORGANIZATION_V1> _returnData = new List<HR.ORGANIZATION_V1>();

            foreach (HR.HIERARCHY _hier in _hierData)
            {
                var data = HR.OverheadOrganizationStatusByHierarchy(_hier.ORGANIZATION_STRUCTURE_ID, _selectedRecordID).Where(x => x.ORGANIZATION_STATUS == "Budgeting Allowed").ToList();
                _returnData.AddRange(data);
            }

            //Clean Up data
            _returnData = _returnData.Distinct().ToList();

            List<OVERHEAD_PERIODS_V> _dataView = new List<OVERHEAD_PERIODS_V>();




            int count;
            uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION_V1>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _returnData, out count);
            e.Total = count;
        }


        public class OVERHEAD_PERIODS_V
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGNAIZATION_NAME { get; set; }
            public string CURRENT_BUDGET_TYPE { get; set; }
            public string STATUS { get; set; }
        }



    }
}