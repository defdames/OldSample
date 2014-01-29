using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umSecurityLogList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.Logs.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            if (!X.IsAjaxRequest)
            {

                if (Request.Cookies["UserSettings"] != null)
                {
                    string RTL;
                    HttpCookie myCookie = new HttpCookie("UserSettings");
                    myCookie = Request.Cookies["UserSettings"];
                    RTL = myCookie["RTL"];
                    //Check for RTL support
                    if (RTL == "True")
                    {
                        uxViewPort.RTL = true;
                    }
                }
            }
        }

        /// <summary>
        /// Data bind system Logs to the gridpanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deLogsDatabind(object sender, StoreReadDataEventArgs e)
        {
                int total;
                IEnumerable<SYS_LOG_CT> data = SYS_LOG.Logs(e.Start, e.Limit, e.Sort, e.Parameters["filter"], out total);
                e.Total = total;
                uxSecurityLogGridPanel.GetStore().DataSource = data;
        }

    }
}