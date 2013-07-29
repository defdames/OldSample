using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using DBI.Data.DataFactory.Utilities;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umSecurityUsersList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
        /// This loads up the data for the User List page proxy and is used whenever the user filters data, changes a page or reloads data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UsersDatabind(object sender, StoreReadDataEventArgs e)
        {
            int total;
            IEnumerable<SYS_USER_INFORMATION> data = DBI.Data.DataFactory.Security.Users.UserList(e.Start, e.Limit, e.Sort, e.Parameters["filter"], out total);
            e.Total = total;
            uxSecurityUserGridPanel.GetStore().DataSource = data;
        }


        /// <summary>
        /// Edits a system user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deEditUser(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as RowSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            //Show the Window
            uxSecurityUserDetailsWindow.Show();

        }

    }
}
