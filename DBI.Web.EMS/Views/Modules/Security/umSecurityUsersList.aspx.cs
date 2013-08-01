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
        public void deUsersDataBind(object sender, StoreReadDataEventArgs e)
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
            //Show the Window
            uxSecurityUserDetailsWindow.Show();
            DataBindSecurityGrid();
        }

        /// <summary>
        /// This is to reload the security roles view for the user when the user clicks refresh on the paging toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deReloadUserSecurity(object sender, StoreReadDataEventArgs e)
        {
        DataBindSecurityGrid();
        }


        /// <summary>
        /// This is the generic code that performs the data needed for the reload of the security users gridpanel.
        /// </summary>
        public void DataBindSecurityGrid()
        {
            RowSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as RowSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            SYS_USER_INFORMATION user = DBI.Data.DataFactory.Security.Users.UserByID(UserID);

            this.uxSecurityUserDetails.SetValues(new
            {
                user.USER_ID,
                user.USER_NAME,
                user.EMPLOYEE_NAME,
                user.EMPLOYEE_NUMBER,
                user.CURRENT_ORGANIZATION,
                user.JOB_NAME
            });

            //Load data needed for Gridview and security user roles
            List<SYS_USER_ROLES> userRoles = Data.DataFactory.Security.Roles.RolesByUserID(user.USER_ID);

            //Create a list of user roles with descriptions we will databind to the view
            List<SYS_USER_ROLES_CT> data = new List<SYS_USER_ROLES_CT>();

            foreach (SYS_USER_ROLES role in userRoles)
            {
                SYS_USER_ROLES_CT roleDetails = new SYS_USER_ROLES_CT();
                roleDetails.USER_ROLE_ID = role.USER_ROLE_ID;
                roleDetails.NAME = role.SYS_ROLES.NAME;
                roleDetails.DESCRIPTION = role.SYS_ROLES.DESCRIPTION;
                data.Add(roleDetails);
            }

            uxSecurityRoleGridPanel.GetStore().DataSource = data;
            uxSecurityRoleGridPanel.GetStore().DataBind();
        }

        /// <summary>
        /// Deletes a user role by user role id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deDeleteUserRole(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = uxSecurityRoleGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long UserRoleID = long.Parse(sm.SelectedRow.RecordID);

            DBI.Data.DataFactory.Security.Roles.DeleteUserRoleByID(UserRoleID);

            //Clear the selection
            sm.ClearSelection();

            //Refresh the gridpanel data showing the loading screen
            uxSecurityRolePaging.DoRefresh();
        }

        public void deShowAddUserRole(object sender, DirectEventArgs e)
        {
            //clear the selection we will need it later.
            CheckboxSelectionModel csm = uxSecurityRoleList.GetSelectionModel() as CheckboxSelectionModel;
            csm.ClearSelection();

            // Show the window that give a list of user roles to choose from that the user has not already selected.
            uxMaintainSecurityRoles.Show();

            RowSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as RowSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            //Return a list of current user roles
            IEnumerable<SYS_ROLES> data = Data.DataFactory.Security.Roles.FreeRolesByUserID(UserID);

            uxSecurityRoleList.GetStore().DataSource = data;
            uxSecurityRoleList.GetStore().DataBind();

            //Refresh the gridpanel data showing the loading screen
            uxSecurityRolePaging.DoRefresh();
        }


        /// <summary>
        /// This is to reload the security roles view that are free to add for the user when the user clicks refresh on the paging toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deReloadUserRoleSecurity(object sender, StoreReadDataEventArgs e)
        {
            uxSecurityRoleListSelection.ClearSelection();

            RowSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as RowSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            //Return a list of current user roles
            IEnumerable<SYS_ROLES> data = Data.DataFactory.Security.Roles.FreeRolesByUserID(UserID);

            uxSecurityRoleList.GetStore().DataSource = data;
            uxSecurityRoleList.GetStore().DataBind();

            //Refresh the gridpanel data showing the loading screen
            uxSecurityRolePaging.DoRefresh();
        }


        public void deAddUserRole(object sender, DirectEventArgs e)
        {
            //Get the selected user id
            RowSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as RowSelectionModel;
            long userID = long.Parse(sm.SelectedRow.RecordID);

            //Get the selected role id
            CheckboxSelectionModel csm = uxSecurityRoleList.GetSelectionModel() as CheckboxSelectionModel;
            long roleID = long.Parse(csm.SelectedRow.RecordID);

            //Add role to user and refresh gridpanel after closing window
            DBI.Data.DataFactory.Security.Roles.AddRoleToUserID(roleID, userID);

            //Close the window
            uxMaintainSecurityRoles.Close();

            //Refresh the user Data for roles.
            uxSecurityRolePaging.DoRefresh();

        }

                
    }
}
