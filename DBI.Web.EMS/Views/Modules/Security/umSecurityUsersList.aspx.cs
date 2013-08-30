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
using System.Security.Claims;
using System.IdentityModel.Tokens;
using System.IdentityModel.Services;

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
            IEnumerable<SYS_USER_INFORMATION> data = SYS_USER_INFORMATION.Users(e.Start, e.Limit, e.Sort, e.Parameters["filter"], out total);
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
        /// This is to reload the users for the user when the user clicks refresh on the paging toolbar.
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
            try
            {

            CheckboxSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            SYS_USER_INFORMATION user = SYS_USER_INFORMATION.UserByID(UserID);

            this.uxSecurityUserDetails.SetValues(new
            {
                user.USER_ID,
                user.USER_NAME,
                user.EMPLOYEE_NAME,
                user.EMPLOYEE_NUMBER,
                user.CURRENT_ORGANIZATION,
                user.JOB_NAME
            });

            //Load data needed for Gridview and security activities
            List<SYS_USER_ACTIVITY> userActivity = SYS_USER_ACTIVITY.ActivityByUserID(user.USER_ID);

            //Create a list of user roles with descriptions we will databind to the view
            List<SYS_USER_ACTIVITY_CT> data = new List<SYS_USER_ACTIVITY_CT>();

            foreach (SYS_USER_ACTIVITY activity in userActivity)
            {
                SYS_USER_ACTIVITY_CT activityDetails = new SYS_USER_ACTIVITY_CT();
                activityDetails.USER_ACTIVITY_ID = activity.USER_ACTIVITY_ID;
                activityDetails.NAME = activity.SYS_ACTIVITY.NAME;
                activityDetails.DESCRIPTION = activity.SYS_ACTIVITY.DESCRIPTION;
                data.Add(activityDetails);
            }

            uxSecurityActivityGridPanel.GetStore().DataSource = data;
            uxSecurityActivityGridPanel.GetStore().DataBind();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Deletes a user activity by user activity id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deDeleteUserActivity(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = uxSecurityActivityGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long UserActivityID = long.Parse(sm.SelectedRow.RecordID);

            SYS_USER_ACTIVITY.Delete(UserActivityID);

            //Clear the selection
            sm.ClearSelection();

            //Refresh the gridpanel data showing the loading screen
            uxSecurityActivityPaging.DoRefresh();
        }

        public void deShowAddUserActivity(object sender, DirectEventArgs e)
        {
            //clear the selection we will need it later.
            CheckboxSelectionModel csm = uxSecurityActivityList.GetSelectionModel() as CheckboxSelectionModel;
            csm.ClearSelection();

            // Show the window that give a list of user roles to choose from that the user has not already selected.
            uxMaintainSecurityActivities.Show();

            CheckboxSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            //Return a list of current user roles not assigned to the user
            IEnumerable<SYS_ACTIVITY> data = SYS_ACTIVITY.UnassignedActivities(UserID);

            uxSecurityActivityList.GetStore().DataSource = data;
            uxSecurityActivityList.GetStore().DataBind();

            //Refresh the gridpanel data showing the loading screen
            uxSecurityActivityPaging.DoRefresh();
        }


        /// <summary>
        /// This is to reload the security roles view that are free to add for the user when the user clicks refresh on the paging toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deReloadUserActivitySecurity(object sender, StoreReadDataEventArgs e)
        {
            uxSecurityActivityListSelection.ClearSelection();

            CheckboxSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long UserID = long.Parse(sm.SelectedRow.RecordID);

            //Return a list of current user roles not assigned to the user
            IEnumerable<SYS_ACTIVITY> data = SYS_ACTIVITY.UnassignedActivities(UserID);

            uxSecurityActivityList.GetStore().DataSource = data;
            uxSecurityActivityList.GetStore().DataBind();

            //Refresh the gridpanel data showing the loading screen
            uxSecurityActivityPaging.DoRefresh();
        }


        public void deAddUserActivity(object sender, DirectEventArgs e)
        {
            //Get the selected user id
            CheckboxSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long userID = long.Parse(sm.SelectedRow.RecordID);

            //Get the selected role id
            CheckboxSelectionModel csm = uxSecurityActivityList.GetSelectionModel() as CheckboxSelectionModel;
            long activityID = long.Parse(csm.SelectedRow.RecordID);

            //Add role to user and refresh gridpanel after closing window

            SYS_USER_ACTIVITY activity = new SYS_USER_ACTIVITY();
            activity.ACTIVITY_ID = activityID;
            activity.USER_ID = userID;
            activity.CREATED_BY = User.Identity.Name;
            activity.CREATED_DATE = DateTime.Now.InvariantCulture();
            activity.LAST_UPDATED_BY = User.Identity.Name;
            activity.LAST_UPDATED = DateTime.Now.InvariantCulture();

            SYS_USER_ACTIVITY.Add(activity);

            //Close the window
            uxMaintainSecurityActivities.Close();

            //Refresh the user Data for roles.
            uxSecurityActivityPaging.DoRefresh();

        }

        public void deImpersonate(object sender, DirectEventArgs e)
        {
            //Get the user information
            //Get the selected user id
            CheckboxSelectionModel sm = uxSecurityUserGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long userID = long.Parse(sm.SelectedRow.RecordID);

            SYS_USER_INFORMATION userDetails = SYS_USER_INFORMATION.UserByID(userID);

            if (userDetails != null)
            {

             List<Claim> claims = DBI.Data.SYS_ACTIVITY.Claims(userDetails.USER_NAME);

                int cnt = claims.Count;
                // Always add the username, this is always required
                claims.Add(new Claim(ClaimTypes.Name, userDetails.USER_NAME));

                // Add a claim to say they were impersonated and by who impersonated them
                claims.Add(new Claim("ImpersonatedUser", userDetails.EMPLOYEE_NAME));

                if (GetClaimValue("ImpersonatorUsername") == string.Empty)
                {
                    claims.Add(new Claim("ImpersonatorUsername", HttpContext.Current.User.Identity.Name));
                }

                // Add full name of user to the claims 
                claims.Add(new Claim("EmployeeName", GetClaimValue("EmployeeName")));

                var id = new ClaimsIdentity(claims, "Forms");
                var cp = new ClaimsPrincipal(id);

                var token = new SessionSecurityToken(cp);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);

           // Break out of frames and redirect user.
           ResourceManager.GetInstance().AddScript("parent.window.location = '{0}';", "../../uxDefault.aspx");
         }
        }

        public void deUserSelected(object sender, DirectEventArgs e)
        {
                     
             // Setup Security
            uxImpersonate.Disabled = DisableActivity("SYS.Users.Impersonate");
            uxEditUser.Disabled = DisableActivity("SYS.Users.Edit");
            
        }

        public void deUserDeselected(object sender, DirectEventArgs e)
        {

            // Setup Security
            uxImpersonate.Disabled = true;
            uxEditUser.Disabled = true;

        }

                
    }
}
