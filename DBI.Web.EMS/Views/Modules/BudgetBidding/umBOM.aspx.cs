using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umBOM : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        protected void deLoadBOMDropdown(object sender, StoreReadDataEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["orgID"]);
            List<object> dataSource = BBDetail.SubGrid.BOM.Listing.Data(orgID).ToList<object>();
            int count;

            uxBOMStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            e.Total = count;
        }

        protected void deReadBOMGridData(object sender, StoreReadDataEventArgs e)
        {
            if (uxHidBOMBillSeqID.Text == "") { return; }

            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            long bomBillSeqID = Convert.ToInt64(uxHidBOMBillSeqID.Text);

            uxBOMGridStore.DataSource = BBDetail.SubGrid.BOM.MaterialItems.Data(orgID, bomBillSeqID);
        }

        protected void deSelectBOM(object sender, DirectEventArgs e)
        {
            string billSeqID = e.ExtraParams["BillSeqID"];
            string bomDesc = e.ExtraParams["BOMDesc"];

            uxBOM.SetValue(billSeqID, bomDesc);
            uxHidBOMBillSeqID.Text = billSeqID;
            uxBOMGridStore.Reload();
            uxUpdate.Enable();
        }

        protected void deAddBOM(object sender, DirectEventArgs e)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailTaskID = long.Parse(Request.QueryString["detailSheetID"]);
            long orgID = long.Parse(Request.QueryString["orgID"]);
            long billSeqID = Convert.ToInt64(uxHidBOMBillSeqID.Text);

            BBDetail.SubGrid.BOM.MaterialItems.AddItems(projectID, detailTaskID, orgID, billSeqID);

            X.Js.Call("closeUpdate");
        }
    }
}