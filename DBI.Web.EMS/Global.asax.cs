using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using Ext.Net;


namespace DBI.Web.EMS
{
    public class Global : System.Web.HttpApplication
    {
       #if APPDEV
        // Used to set the default instance of the connection String Values
        public const string serverInstance = "APPDEV";
       #elif PCL05
         // Used to set the default instance of the connection String Values
        public const string serverInstance = "PCL05";
        #else
        // Used to set the default instance of the connection String Values
        public const string serverInstance = "PROD";
        #endif

        protected void Application_Start(object sender, EventArgs e)
        {
            // Set the license key needed to hide "warning" windows when deploying to server
            Application["Ext.Net.LicenseKey"] = "ODUwODg1NjAsMiw5OTk5LTEyLTMx"; 

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Traps any applications errors and changes them to json for nice output for the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            string exceptionText = string.Empty;

            HttpContext ctx = HttpContext.Current;
            Exception exception = ctx.Server.GetLastError();

            // outer exception is always an http exception so we need the inner exception which originally caused the exception
            if (exception.InnerException != null)
                exception = exception.InnerException;

            if (HttpContext.Current.Request.TotalBytes > 0)
            {
                exceptionText = new JavaScriptSerializer().Serialize(new { errorMessage = exception.ToString() });
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 500;
                ctx.Response.Write(exception.ToString());
                ctx.Server.ClearError();
            }
            


        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        ///<summary>
        ///This event occurrs when a session security token has been read from a cookie. 
        ///Sliding expiration is supported by handling this event.  This event is raised by 
        ///the session authentication module. In this event you can create a new session token, 
        ///which will be used for validation and saved as a cookie for subsequent validation. 
        ///Just reset the ValidTo property of the token.                
        ///</summary>
        ///        
        protected void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs e)
        {
            SessionAuthenticationModule sam = FederatedAuthentication.SessionAuthenticationModule;

            var token = e.SessionToken;
            var duration = token.ValidTo.Subtract(token.ValidFrom);
            if (duration <= TimeSpan.Zero) return;

            var diff = token.ValidTo.Add(sam.FederationConfiguration.IdentityConfiguration.MaxClockSkew).Subtract(DateTime.UtcNow);
            if (diff <= TimeSpan.Zero) return;

            var halfWay = duration.TotalMinutes / 2;
            var timeLeft = diff.TotalMinutes;
            if (timeLeft <= halfWay)
            {
                e.ReissueCookie = true;
                e.SessionToken =
                    new SessionSecurityToken(
                        token.ClaimsPrincipal,
                        token.Context,
                        DateTime.UtcNow,
                        DateTime.UtcNow.Add(duration))
                    {
                        IsPersistent = token.IsPersistent,
                        IsReferenceMode = token.IsReferenceMode
                    };
            }
        }
  


    }
}