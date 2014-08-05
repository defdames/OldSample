using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umBudgetMain : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                AddTab("bmaint", "Organization Budget Versions", "umOverheadMaintainBudgets.aspx", false, true);
            }
        }

        /// <summary>
        /// Adds a tab to the tabpanel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="loadContent"></param>
        /// <param name="setActive"></param>
        protected void AddTab(string id, string title, string url, Boolean loadContent = false, Boolean setActive = false)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":", "_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxBudgetTabPanel);

            if (setActive)
                uxBudgetTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));

            if (loadContent)
                pan.LoadContent();
        }

        [DirectMethod]
        public void AddTabPanel(string id, string title, string url)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":", "_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Closable = true;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxBudgetTabPanel);

            uxBudgetTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));
        }
    }
}