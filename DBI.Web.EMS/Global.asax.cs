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
        // Used to set the default instance of the connection String Values
        public const string serverInstance = "Production";

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


        protected void Application_Error(object sender, EventArgs e)
        {
           // do not forget to register ExtHelper`s exception handler on every page (ExtHelper.Exception.SetExceptionHandling)!
            bool transmitStack = false;
            string exceptionText = string.Empty;
 
            HttpContext ctx = HttpContext.Current;
            Exception exception = ctx.Server.GetLastError();
 
            // outer exception is always an http exception so we need the inner exception which originally caused the exception
            if (exception.InnerException != null)
                exception = exception.InnerException;
 
            // if an exception occured before page was rendered (response.Length == 0) we need to return plain html including 
            // information about the error as response because Coolite`s error handler displays this html response within it`s error window by default(=> response is
            // content of error window, which itself can`t be changed)
            string plainTemplate =
                @"<table>
                <tr>
                    <td colspan='2'>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan='2' style='color: #FF0000; font-weight:bold; font-size:18px;'>{0}</td>
                </tr>
                <tr>
                    <td colspan='2' style='font-weight:bold; font-size: 16px;' >{1}</td>
                </tr>
                <tr>
                    <td colspan='2'>&nbsp;</td>
                </tr>
                <tr>
                    <td style='font-weight:bold; font-size:12px; visibility:{2}'>Stack:</td>
                    <td style='font-size: 11px; visbility:{2}'>{3}</td>
                <tr/>
                <tr>
                    <td style='font-weight:bold; font-size:12px;'>Adresse:</td>
                    <td style='font-size: 11px;'>{4}</td>
                <tr/>
            </table>";
 
#if DEBUG
                        transmitStack = true; // only show stack if in debug mode
#endif

                        if (HttpContext.Current.Request.TotalBytes > 0)
                        //exceptionText = String.Format(plainTemplate, "Error processing your request: ", exception.Message, transmitStack ? "visible" : "hidden",
                        //exception.StackTrace.Length > 400 ? exception.StackTrace.Substring(0, 400) : exception.StackTrace, ctx.Request.Url); // send plain html to display within`s Coolite default error window
                        //else
                            // if page was already rendered, we need to return a json response with full error message
                            // the error message then gets parsed in client-side ExtJs error handler (AjaxFailure listener of ScriptManager) and is displayed (formatted) within a message box
                            // the reason why we have to serialize FULL error message (including stack etc.) here is that there are two sorts of how an exception is handled by ExtJs/ASP
                            // 1) exceptions which are thrown within an AjaxMethod do not trigger global applications OnError-Handler so this part of code is NEVER executed in this case -> ExtJs transmits the full exception message 
                            //    to the client side error handler as parameter automatically then
                            // 2) an exception is thrown within an AjaxEvent, this part of code is executed
                            // Because both cases may happen we need to "emulate" case 1) here if case 2) happened
                        exceptionText = new JavaScriptSerializer().Serialize(new { errorMessage = exception.ToString() });

                        if (HttpContext.Current.Request.TotalBytes > 0)
                        {
                            ctx.Response.Clear();
                            ctx.Response.ContentType = "application/json";
                            ctx.Response.StatusCode = 500;
                            ctx.Response.Write(exceptionText);
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