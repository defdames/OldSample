using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umSecurityRolesList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxSecurityRoleStore.DataSource = DBI.Data.DataFactory.Security.Roles.RoleList();
                uxSecurityRoleStore.DataBind();

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


        protected void deAddRole(object sender, DirectEventArgs e)
        {
            //Need to open via javascript so it renders to the main window.
            X.Js.Call("App.direct.dmLoadModuleIntoWindow", "~/Views/Modules/Security/umSecurityAddRole.aspx", "Add Role", Ext.Net.Icon.UserAdd, 330, 150, null);
        }

        protected void deSecurityRoleRefresh(object sender, StoreReadDataEventArgs e)
        {
            uxSecurityRoleStore.DataSource = DBI.Data.DataFactory.Security.Roles.RoleList();
            uxSecurityRoleStore.DataBind();
        }

        [DirectMethod(ShowMask=true)]
        public void dmRefreshRoles()
        {
            uxSecurityRoleStore.DataSource = DBI.Data.DataFactory.Security.Roles.RoleList();
            uxSecurityRoleStore.DataBind();
        }

        [DirectMethod]
        public void dmShowAlert(string title, string message)
        {
            X.Msg.Alert(title, message).Show();
        }

    }
}