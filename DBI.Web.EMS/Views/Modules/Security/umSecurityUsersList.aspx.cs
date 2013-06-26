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
    public partial class umSecurityUsersList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxSecurityUserStore.DataSource = DBI.Data.DataFactory.Security.Users.UserList();
                uxSecurityUserStore.DataBind();

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

        protected void deCreateUser(object sender, DirectEventArgs e)
        {
            //Need to open via javascript so it renders to the main window.
            X.Js.Call("parent.App.direct.dmLoadModuleIntoWindow", "~/Views/Modules/Security/umSecurityUsers.aspx", "Add User", null);       

        }

        protected void deUserRoleSelect(object sender, DirectEventArgs e)
        {
            string upUserID = e.ExtraParams["upUserID"];

            DBI.Data.SYS_USERS_V User = DBI.Data.DataFactory.Security.Users.UserDetailsByID(decimal.Parse(upUserID));

            this.uxSecurityUserDetails.SetValues(new
            {
                User.USER_NAME,
                User.FIRST_NAME,
                User.MIDDLE_NAMES,
                User.LAST_NAME,
                User.EMPLOYEE_NUMBER,
                User.ORGANIZATION_NAME,
                User.JOB_TITLE,
                CURRENT_EMPLOYEE_FLAG = (User.CURRENT_EMPLOYEE_FLAG == "Y") ? "Yes" : "No",
                ORACLE_ACCOUNT_STATUS = (User.ORACLE_ACCOUNT_STATUS == "1") ? "Active" : "Disabled",
            });

            uxSecurityUserDetails.Collapsed = false;

        }


        protected void deShowActiveEmployees(object sender, DirectEventArgs e)
        {
            ListFilter lf = (ListFilter)uxSecurityGridFilters.Filters[7];
            string[] filter = new string[]{"Y"};
            lf.SetValue(filter);
            lf.SetActive(true);
        }

    }
}
