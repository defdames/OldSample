using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views
{
    /// <summary>
    /// Default View with buttons and where modules are loaded
    /// </summary>
    public partial class uxDefault : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!X.IsAjaxRequest)
            {
                // Look for cookie information for language support
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

                //todo Add authentication to the page itself instead of simply disabling the button(Needs DB)
                /// Validate Security Objects ---------------------------------------------------
                validateComponentSecurity<Ext.Net.MenuItem>("SYS.Users.View", "uxSecurityUsers");
                validateComponentSecurity<Ext.Net.MenuItem>("SYS.Activities.View", "uxSecurityActivities");
                validateComponentSecurity<Ext.Net.MenuItem>("SYS.Logs.View", "uxSecurityLogs");
                //-------------------------------------------------------------------------------

                var MyAuth = new Authentication();
                // Get Impersonating Info/Details
                string user = MyAuth.GetClaimValue("ImpersonatedUser", User as ClaimsPrincipal);
                string byuser = MyAuth.GetClaimValue("EmployeeName", User as ClaimsPrincipal);

                uxWelcomeTime.Text = string.Format("Today is {0}", DateTime.Now.ToString("D"));

                if (user != string.Empty)
                {
                    //todo Internationalize(currently only in English) because of string.Format
                    uxWelcomeName.Text = string.Format("Impersonating User {0} by ({1})", user, byuser);
                    uxWelcomeName.CtCls = "header-actions-button-red";
                    uxWelcomeName.Disabled = false;
                }
                else
                {
                    //Display Welcome message to user
                    uxWelcomeName.Text = string.Format("Welcome {0}", byuser);
                    uxWelcomeName.CtCls = "header-actions-button-orange";
                    uxWelcomeName.Disabled = true;
                }
            }
            GenerateMenuItems(User as ClaimsPrincipal);

        }

        /// <summary>
        /// Direct Event that loads a panel given 2 ext.net extra parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Extra Parameters Page(which panel to load), and Location(where on the page to load it)</param>
        protected void deLoadPage(object sender, DirectEventArgs e)
        {
            LoadModule(e.ExtraParams["Page"], e.ExtraParams["Location"]);
        }

        /// <summary>
        /// Removes the impersonate ability from the user, and resets their security back to their user account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveImpersonate(object sender, DirectEventArgs e)
        {
            var MyAuth = new Authentication();
            if (MyAuth.GetClaimValue("ImpersonatorUsername", User as ClaimsPrincipal) != null)
            {

                SYS_USER_INFORMATION userDetails = SYS_USER_INFORMATION.UserByUserName(MyAuth.GetClaimValue("ImpersonatorUsername", User as ClaimsPrincipal));

                List<Claim> claims = DBI.Data.SYS_ACTIVITY.Claims(userDetails.USER_NAME);

                // Add full name of user to the claims 
                claims.Add(new Claim("EmployeeName", MyAuth.GetClaimValue("EmployeeName", User as ClaimsPrincipal)));

                var token = Authentication.GenerateSessionSecurityToken(claims);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);


                // Break out of frames and redirect user.
                ResourceManager.GetInstance().AddScript("parent.window.location = '{0}';", "uxDefault.aspx");
            }

        }

        //todo Eventually move to Role contains default(but overrideable) Activities
        //todo Update to Add Icon and Menu Item Text
        /// <summary>
        /// Generates the Menu Items for a logged in user based on all roles assigned to user in claims
        /// </summary>
        public void GenerateMenuItems(ClaimsPrincipal icp)
        {
            List<SYS_ACTIVITY> userActivities;

            if(!User.IsInRole("SYS.Administrator"))
            {
                //Get all roles from claims
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)icp.Identity;

                List<string> AssignedRoles = (from c in claimsIdentity.Claims
                                              where c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                                              select c.Value).ToList();
                //Get Button config from server
                Entities context = new Entities();
                userActivities = (from s in context.SYS_ACTIVITY
                                    where AssignedRoles.Contains(s.NAME) && s.PATH != null
                                    select s).ToList();
            }
            else
            {
                Entities context = new Entities();
                userActivities = (from s in context.SYS_ACTIVITY
                                    where !(string.IsNullOrEmpty(s.PATH))
                                    select s).ToList();
            }
            //Iterate through allowed activities
            foreach (SYS_ACTIVITY userActivity in userActivities)
            {
                //Create new Menu Item
                Ext.Net.MenuItem NewItem = new Ext.Net.MenuItem()
                {
                    ID = "menu" + userActivity.ACTIVITY_ID.ToString(),
                    Text = "My Button", //userActivity.TITLE,
                    Icon = Icon.Add //(Icon)Enum.Parse(typeof(Icon), userActivity.ICON)
                };

                //Add click DirectEvent
                NewItem.DirectEvents.Click.Event += deLoadPage;
                
                //Add DirectEvent Parameters
                NewItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
                {
                    Name = "Location",
                    Value = userActivity.CONTAINER
                });
                NewItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
                {
                    Name = "Page",
                    Value = userActivity.PATH
                });

                //Add to Menu
                uxMenu.Items.Add(NewItem);                
            }
        }
    }
}