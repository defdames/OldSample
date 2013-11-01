using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;

using DBI.Core.Web;
using DBI.Data.DataFactory;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Web.Security;
using DBI.Core.Security;
using System.Security.Claims;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using DBI.Data;

namespace DBI.Web.EMS
{
    /// <summary>
    /// Login class that controls all login for Enterprise Management System.
    /// </summary>
    public partial class uxLogin : BasePage
    {

        /// <summary>
        /// Loads the login page
        /// </summary>
        /// <remarks>This area loads the cookie for UserSettings, if it finds one it sets the language equal to the cookie language. It also checks for RTL and sets that if needed.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                Store store = uxRegion.GetStore();
                store.DataSource = StaticLists.GetSupportedLanguages();
                store.DataBind();

                this.uxResourceManager.RegisterIcon(Icon.FlagCa);
                this.uxResourceManager.RegisterIcon(Icon.FlagFr);
                this.uxResourceManager.RegisterIcon(Icon.FlagGb);
                this.uxResourceManager.RegisterIcon(Icon.FlagUs);
                this.uxResourceManager.RegisterIcon(Icon.FlagAr);


                // Get the language from the user and set the browser region.

                string[] languages = HttpContext.Current.Request.UserLanguages;
                string browserLanguage = languages[0].Trim();

                if (Request.Cookies["UserSettings"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("UserSettings");
                    myCookie = Request.Cookies["UserSettings"];
                    browserLanguage = myCookie["Culture"];
                }


                switch (browserLanguage)
                {
                    case "en-GB":
                        uxRegion.SelectedItem.Index = 2;
                        break;
                    case "en-US":
                        uxRegion.SelectedItem.Index = 3;
                        break;
                    case "fr-CA":
                        uxRegion.SelectedItem.Index = 1;
                        break;
                    case "ar-AE":
                        uxRegion.SelectedItem.Index = 4;
                        break;
                    case "en-CA":
                        uxRegion.SelectedItem.Index = 0;
                        break;
                    default:
                        uxRegion.SelectedItem.Index = 3;
                        break;
                }

                CultureInfo region = CultureInfo.CreateSpecificCulture(browserLanguage);
                CultureInfo.DefaultThreadCurrentCulture = region;
                CultureInfo.DefaultThreadCurrentUICulture = region;

                Thread.CurrentThread.CurrentCulture = region;
                Thread.CurrentThread.CurrentUICulture = region;

                FileVersionInfo buildInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/DBI.Web.EMS.dll"));
                uxDatabaseVer.Text = string.Format("{0}: {1} - {2}: {3}", (string)GetLocalResourceObject("loginDatabase"), Global.serverInstance, (string)GetLocalResourceObject("loginVersion"), buildInfo.FileVersion);
            }

            // Check for valid oracle connection
            if (!GenericData.IsContextValid())
            {
                X.Msg.Alert((string)GetLocalResourceObject("loginErrorTitle"), (string)GetLocalResourceObject("maintenance")).Show();
                uxLoginButton.Disabled = true;

            }
        }

        /// <summary>
        /// Verifies the login is correct for the user and redirects or messages the user depending on the outcome.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLogin(object sender, DirectEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(this.uxUsername.Text) || String.IsNullOrWhiteSpace(this.uxPassword.Text))
                {
                    var focusBtn = new JFunction { Handler = "#{uxUsername}.focus(false,250);"};
                    X.Msg.Alert((string)GetLocalResourceObject("loginErrorTitle"), (string)GetLocalResourceObject("loginErrorMessage"), focusBtn).Show();
                    uxUsername.Reset();
                    return;
                }

                if (Authentication.Authenticate(this.uxUsername.Text, this.uxPassword.Text))
                {
                    //todo Move Claims building into Authenticate method?
                    List<Claim> claims = DBI.Data.SYS_ACTIVITY.Claims(this.uxUsername.Text.ToUpper());

                    int cnt = claims.Count;

                    //Check if user has any roles, if not then exit now
                    if (cnt == 0)
                    {
                        var focusBtn = new JFunction { Handler = "#{uxPassword}.focus(false,250);"};
                        X.Msg.Alert((string)GetLocalResourceObject("loginErrorTitle"), (string)GetLocalResourceObject("loginErrorNoRoles"), focusBtn).Show();
                        uxPassword.Reset();
                        return;
                    }
                    else
                    {
                        var token = Authentication.GenerateSessionSecurityToken(claims);
                        var sam = FederatedAuthentication.SessionAuthenticationModule;
                        sam.WriteSessionTokenToCookie(token);

                        //Disable login button
                        uxLoginButton.Disabled = true;

                        // Redirect:
                        Ext.Net.ExtNet.Redirect("Views/uxDefault.aspx");
                    }
                }

                else
                {
                    var focusBtn = new JFunction { Handler = "#{uxPassword}.focus(false,250);" };
                    X.Msg.Alert((string)GetLocalResourceObject("loginErrorTitle"), (string)GetLocalResourceObject("loginInvalid"), focusBtn).Show();
                    uxPassword.Reset();
                    return;
                }
            }
            catch (Exception ex)
            {
                string eCode = String.Empty;
                DBI.Data.SYS_LOG.LogToDatabase(ex, out eCode);
            }
        }

        /// <summary>
        /// Switches the language on the page depending on the value selected.
        /// </summary>
        /// <param name="selectedLanguage"></param>
        [DirectMethod(ShowMask = true, Msg = "Translating...")]
        public void dmChangeRegion(string selectedLanguage, string iconValue)
        {
            string culture;
            if (iconValue == "icon-flagus")
            {
                culture = "en-US";
            }
            else if (iconValue == "icon-flaggb")
            {
                culture = "en-GB";
            }
            else if (iconValue == "icon-flagca")
            {
                culture = "en-CA";
            }
            else if (iconValue == "icon-flagfr")
            {
                culture = "fr-CA";
            }
            else if (iconValue == "icon-flagar")
            {
                culture = "ar-AE";
            }
            else
            {
                culture = "en-US";
            }

            HttpCookie myCookie = new HttpCookie("UserSettings");
            myCookie["Culture"] = culture;
            //Check for RTL Support
            if (culture == "ar-AE")
            {
                myCookie["RTL"] = "True";
            }
            myCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(myCookie);

            X.Redirect("uxLogin.aspx");
        }
    }
}