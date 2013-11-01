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

                //todo Switch to generating buttons based off of permissions vs disabling based off of permissions.(Needs DB)
                //todo Add authentication to the page itself instead of simply disabling the button(Needs DB)
                /// Validate Security Objects ---------------------------------------------------
                validateComponentSecurity<Ext.Net.MenuItem>("SYS.Users.View", "uxSecurityUsers");
                validateComponentSecurity<Ext.Net.MenuItem>("SYS.Activities.View", "uxSecurityActivities");
                validateComponentSecurity<Ext.Net.MenuItem>("SYS.Logs.View", "uxSecurityLogs");
                //-------------------------------------------------------------------------------

                // Get Impersonating Info/Details
                string user = GetClaimValue("ImpersonatedUser");
                string byuser = GetClaimValue("EmployeeName");

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
            if (GetClaimValue("ImpersonatorUsername") != null)
            {

                SYS_USER_INFORMATION userDetails = SYS_USER_INFORMATION.UserByUserName(GetClaimValue("ImpersonatorUsername"));

                List<Claim> claims = DBI.Data.SYS_ACTIVITY.Claims(userDetails.USER_NAME);

                // Add full name of user to the claims 
                claims.Add(new Claim("EmployeeName", GetClaimValue("EmployeeName")));

                var token = Authentication.GenerateSessionSecurityToken(claims);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);


                // Break out of frames and redirect user.
                ResourceManager.GetInstance().AddScript("parent.window.location = '{0}';", "uxDefault.aspx");
            }

        }
    }
}