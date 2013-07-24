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
        /// This method call is run when the user selected a user in the listbox, it will show what roles the user has in the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deUserRoleSelect(object sender, DirectEventArgs e)
        {
            string upUserID = e.ExtraParams["upUserID"];
            //Load up a list of the roles
            uxSecurityRoleStore.DataSource = DBI.Data.DataFactory.Security.Roles.RoleList();
            uxSecurityRoleStore.DataBind();

            CheckboxSelectionModel sm = uxSecurityRoleCheckSelectionModel as CheckboxSelectionModel;
            sm.ClearSelection();

            //Get a list of user roles
            IQueryable<SYS_USER_ROLES> roles = DBI.Data.DataFactory.Security.Roles.RolesByUserID(long.Parse(upUserID));

            foreach(SYS_USER_ROLES role in roles)
            {
                sm.SelectedRows.Add(new SelectedRow(role.ROLE_ID.ToString()));     
            }

            sm.UpdateSelection();

        }


        /// <summary>
        /// This allows you to add a security role to a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddSecurityRole(object sender, DirectEventArgs e)
        {
            string epRecordID = e.ExtraParams["epRecordID"];
            RowSelectionModel rs = uxSecurityUserSelectionModel as RowSelectionModel;
            SYS_USER_ROLES userRole = new SYS_USER_ROLES();
            userRole.ROLE_ID = long.Parse(epRecordID);
            userRole.USER_ID = long.Parse(rs.SelectedRecordID);
            DBI.Data.DataFactory.Security.Roles.AddUserRole(userRole);


        }

        /// <summary>
        /// This removes a security role from a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deDeleteSecurityRole(object sender, DirectEventArgs e)
        {
            string epRecordID = e.ExtraParams["epRecordID"];
            RowSelectionModel rs = uxSecurityUserSelectionModel as RowSelectionModel;
            SYS_USER_ROLES userRole = DBI.Data.DataFactory.Security.Roles.RoleByUserandRoleID(long.Parse(epRecordID),long.Parse(rs.SelectedRecordID));
            DBI.Data.DataFactory.Security.Roles.DeleteUserRole(userRole);

        }

        //protected void deMaintUserRoles(object sender, DirectEventArgs e)
        //{
        //    // Get the record ID of the selected/unselected role
        //    string pRoleID = e.ExtraParams["pRoleID"];

        //    // Get the record System user id of the selected user
        //    RowSelectionModel rm = uxSecurityUserSelectionModel as RowSelectionModel;

        //    // Does the user role exist
        //    SYS_USER_ROLES userRole = DBI.Data.DataFactory.Security.Roles.DoesUserRoleExist(decimal.Parse(rm.SelectedRow.RecordID), decimal.Parse(pRoleID));

        //    if (userRole == null)
        //    {
        //        //First get the user information for the user from Oracle
        //        SYS_USERS_V userInformation = DBI.Data.DataFactory.Security.Users.UserDetailsByID(decimal.Parse(rm.SelectedRow.RecordID));

        //        // we need to set this if we need to insert the role later
        //        SYS_USERS user = new SYS_USERS();

        //        //Do we need to save the user table for the first time? Check the Fnd_user_id if it's null we need to add it first
        //        if (userInformation.FND_USER_ID == null)
        //        {
        //            //user doesn't exist setup user for first time use.
        //            user.FND_USER_ID = userInformation.USER_ID;
        //            GenericData.Insert<SYS_USERS>(user);
        //        }

        //        //Add the user role information
        //        SYS_USER_ROLES role = new SYS_USER_ROLES();
        //        role.ROLE_ID = long.Parse(pRoleID);
        //        role.SYSTEM_USER_ID = user.SYSTEM_USER_ID;
        //        GenericData.Insert<SYS_USER_ROLES>(role);
        //        uxSecurityUserGridPanel.GetStore().Reload();
        //    }
        //    else
        //    {
        //        // Role already exists so remove it
        //        DBI.Data.DataFactory.Security.Roles.DeleteUserRoleByUserID(decimal.Parse(rm.SelectedRow.RecordID), decimal.Parse(pRoleID));
        //        uxSecurityUserGridPanel.GetStore().Reload();
        //    }
        //}

    }
}
