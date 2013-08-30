using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views
{
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

                // Get security for menu activities
                uxSecurityUsers.Disabled = DisableActivity("SYS.Users.View");
                uxSecurityActivities.Disabled = DisableActivity("SYS.Activities.View");
                uxSecurityLogs.Disabled = DisableActivity("SYS.Logs.View");

                // Get Impersonating Info/Details
                string user = GetClaimValue("ImpersonatedUser");
                string byuser = GetClaimValue("EmployeeName");

                uxWelcomeTime.Text = string.Format("Today is {0}", DateTime.Now.ToString("D"));

                if (user != string.Empty)
                {
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

        protected void deLoadSecurityUsers(object sender, DirectEventArgs e)
        {
            LoadModule("~/Views/Modules/Security/umSecurityUsersList.aspx", "uxCenter");
        }

        protected void deLoadSecurityActivities(object sender, DirectEventArgs e)
        {
            LoadModule("~/Views/Modules/Security/umSecurityActivityList.aspx", "uxCenter");
        }

        protected void deLoadSecurityLogs(object sender, DirectEventArgs e)
        {
            LoadModule("~/Views/Modules/Security/umSecurityLogList.aspx", "uxCenter");
        }

        protected void deRemoveImpersonate(object sender, DirectEventArgs e)
        {
            if (GetClaimValue("ImpersonatorUsername") != null)
            {

                SYS_USER_INFORMATION userDetails = SYS_USER_INFORMATION.UserByUserName(GetClaimValue("ImpersonatorUsername"));

                List<Claim> claims = DBI.Data.SYS_ACTIVITY.Claims(userDetails.USER_NAME);

                int cnt = claims.Count;
                // Always add the username, this is always required
                claims.Add(new Claim(ClaimTypes.Name, userDetails.USER_NAME));

                // Add full name of user to the claims 
                claims.Add(new Claim("EmployeeName", GetClaimValue("EmployeeName")));

                var id = new ClaimsIdentity(claims, "Forms");
                var cp = new ClaimsPrincipal(id);

                var token = new SessionSecurityToken(cp);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);

                // Break out of frames and redirect user.
                ResourceManager.GetInstance().AddScript("parent.window.location = '{0}';", "uxDefault.aspx");
            }

        }
    }
}