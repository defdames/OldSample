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
    public partial class umReorderDetailSheets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!BasePage.validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        protected void deLoadDetailSheetNames(object sender, StoreReadDataEventArgs e)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            uxDetailSheetOrderStore.DataSource = BBDetail.Sheets.SheetOrder.Data(projectID);
        }

        protected void deUpdate(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            List<BUD_BID_DETAIL_TASK> gridValues = JSON.Deserialize<List<BUD_BID_DETAIL_TASK>>(json);

            int i = 1;
            foreach (BUD_BID_DETAIL_TASK data in gridValues)
            {
                data.SHEET_ORDER = i;
                i++;
            }

            GenericData.Update<BUD_BID_DETAIL_TASK>(gridValues);
            
            X.Js.Call("closeUpdate");
        }
    }
}