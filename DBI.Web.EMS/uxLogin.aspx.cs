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
                if (browserLanguage == "en-GB")
                {
                    uxRegion.SelectedItem.Index = 2;
                }
                else if (browserLanguage == "en-US")
                {
                    uxRegion.SelectedItem.Index = 3;
                }
                else if (browserLanguage == "fr-CA")
                {
                    uxRegion.SelectedItem.Index = 1;
                }
                else if (browserLanguage == "ar-AE")
                {
                    uxRegion.SelectedItem.Index = 4;
                }
                else if (browserLanguage == "en-CA")
                {
                    uxRegion.SelectedItem.Index = 0;
                }
                else
                {
                    uxRegion.SelectedItem.Index = 3;
                }

                CultureInfo region = CultureInfo.CreateSpecificCulture(browserLanguage);
                CultureInfo.DefaultThreadCurrentCulture = region;
                CultureInfo.DefaultThreadCurrentUICulture = region;

                Thread.CurrentThread.CurrentCulture = region;
                Thread.CurrentThread.CurrentUICulture = region;

                FileVersionInfo buildInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/DBI.Web.EMS.dll"));
                uxStatus.Text = string.Format("{0}: {1} - {2}: {3}", (string)GetLocalResourceObject("loginDatabase"), Global.serverInstance, (string)GetLocalResourceObject("loginVersion"), buildInfo.FileVersion);
                uxDatabaseVer.Text = uxStatus.Text;
            }


        }

        /// <summary>
        /// Verifies the login is correct for the user and redirects or messages the user depending on the outcome.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLogin(object sender, DirectEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.uxUsername.Text) || String.IsNullOrWhiteSpace(this.uxPassword.Text))
            {
                X.Msg.Alert((string)GetLocalResourceObject("loginErrorTitle"), (string)GetLocalResourceObject("loginErrorMessage")).Show();
                return;
            }

            /*
             * Check authenticated (updates last logon).
             */
            CustomPrincipal principal = new CustomPrincipal(this.uxUsername.Text, this.uxPassword.Text);
            if (principal.Identity.IsAuthenticated)
            {
                // Issue Cookie:
                FormsAuthenticationTicket ticket = ticket = new FormsAuthenticationTicket(2,
                    this.uxUsername.Text.ToUpper(),
                    DateTime.Now,
                    DateTime.Now.Add(FormsAuthentication.Timeout),
                    true,
                    FormsAuthentication.FormsCookiePath);
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Current.Response.Cookies.Add(authCookie);

                //Disable login button
                uxLoginButton.Disabled = true;

                // Redirect:
                Ext.Net.ExtNet.Redirect("Views/uxDefault.aspx");

            }
            else
            {
                X.Msg.Alert((string)GetLocalResourceObject("loginErrorTitle"), (string)GetLocalResourceObject("loginInvalid")).Show();
                uxPassword.Reset();
            }
        }

        /// <summary>
        /// Switches the language on the page depending on the value selected.
        /// </summary>
        /// <param name="selectedLanguage"></param>
        [DirectMethod(ShowMask = true, Msg = "Translating...")]
        public void dmChangeRegion(string selectedLanguage)
        {
            string culture;
            if (selectedLanguage == "United States")
            {
                culture = "en-US";
            }
            else if (selectedLanguage == "Great Britain")
            {
                culture = "en-GB";
            }
            else if (selectedLanguage == "Canada")
            {
                culture = "en-CA";
            }
            else if (selectedLanguage == "French Canadian")
            {
                culture = "fr-CA";
            }
            else if (selectedLanguage == "United Arab Emirates")
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