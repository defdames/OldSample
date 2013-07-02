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


        public void UsersDatabind(object sender, StoreReadDataEventArgs e)
        {
            int total;
            IEnumerable<SYS_USERS_V> data = DBI.Data.DataFactory.Security.Users.UserList(e.Start, e.Limit, e.Sort, e.Parameters["filter"], out total);
            e.Total = total;
            uxSecurityUserGridPanel.GetStore().DataSource = data;
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

            //Load up a list of the roles
            uxSecurityRoleStore.DataSource = DBI.Data.DataFactory.Security.Roles.RoleList();
            uxSecurityRoleStore.DataBind();

            CheckboxSelectionModel sm = uxSecurityRoleCheckSelectionModel as CheckboxSelectionModel;
            sm.SelectedRows.Clear();

            //Get a list of user roles
            IEnumerable<SYS_USER_ROLES_V> roles = DBI.Data.DataFactory.Security.Roles.RolesByUserID(decimal.Parse(upUserID));

            foreach(SYS_USER_ROLES_V role in roles)
            {
                sm.SelectedRows.Add(new SelectedRow(role.ROLE_ID.ToString()));
            }

            sm.UpdateSelection();
        }


        protected void deMaintUserRoles(object sender, DirectEventArgs e)
        {
            // Get the record ID of the selected/unselected role
            string pRoleID = e.ExtraParams["pRoleID"];

            // Get the record System user id of the selected user
            RowSelectionModel rm = uxSecurityUserSelectionModel as RowSelectionModel;

            // Does the user role exist
            SYS_USER_ROLES userRole = DBI.Data.DataFactory.Security.Roles.DoesUserRoleExist(decimal.Parse(rm.SelectedRow.RecordID), decimal.Parse(pRoleID));

            if (userRole == null)
            {
                //First get the user information for the user from Oracle
                SYS_USERS_V userInformation = DBI.Data.DataFactory.Security.Users.UserDetailsByID(decimal.Parse(rm.SelectedRow.RecordID));

                // we need to set this if we need to insert the role later
                SYS_USERS user = new SYS_USERS();

                //Do we need to save the user table for the first time? Check the Fnd_user_id if it's null we need to add it first
                if (userInformation.FND_USER_ID == null)
                {
                    //user doesn't exist setup user for first time use.
                    user.FND_USER_ID = userInformation.USER_ID;
                    GenericData.Insert<SYS_USERS>(user);
                }

                //Add the user role information
                SYS_USER_ROLES role = new SYS_USER_ROLES();
                role.ROLE_ID = long.Parse(pRoleID);
                role.SYSTEM_USER_ID = user.SYSTEM_USER_ID;
                GenericData.Insert<SYS_USER_ROLES>(role);
                uxSecurityUserGridPanel.GetStore().Reload();
            }
            else
            {
                // Role already exists so remove it
                DBI.Data.DataFactory.Security.Roles.DeleteUserRoleByUserID(decimal.Parse(rm.SelectedRow.RecordID), decimal.Parse(pRoleID));
                uxSecurityUserGridPanel.GetStore().Reload();
            }
        }

    }
}
