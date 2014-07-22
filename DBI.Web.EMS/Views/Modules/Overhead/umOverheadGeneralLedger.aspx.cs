using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadGeneralLedger : BasePage
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

            protected void deAddGLRange(object sender, DirectEventArgs e)
            {

                string organizationID = Request.QueryString["orgID"];

                string url = "/Views/Modules/Overhead/umAddGlAccountRange.aspx?orgID=" + organizationID;
                Window win = new Window
                {
                    ID = "uxShowAccountRangeWindow",
                    Title = "General Ledger Account Range Filter",
                    Height = 700,
                    Width = 800,
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