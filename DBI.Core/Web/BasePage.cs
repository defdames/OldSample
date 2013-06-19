using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Ext.Net;

namespace DBI.Core.Web
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// Allows user to override the culture for the page, needs to have httpcookie set.
        /// </summary>
        protected override void InitializeCulture()
        {
            // Check to see if culture cookie exists
            string culture;
            if (Request.Cookies["UserSettings"] != null)
            {
                HttpCookie myCookie = new HttpCookie("UserSettings");
                myCookie = Request.Cookies["UserSettings"];
                culture = myCookie["Culture"];

                CultureInfo region = CultureInfo.CreateSpecificCulture(culture);
                CultureInfo.DefaultThreadCurrentCulture = region;
                CultureInfo.DefaultThreadCurrentUICulture = region;

                Thread.CurrentThread.CurrentCulture = region;
                Thread.CurrentThread.CurrentUICulture = region;
            }

            base.InitializeCulture();
        }

        /// <summary>
        /// System logout function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLogout(object sender, DirectEventArgs e)
        {
            FormsAuthentication.SignOut();
            X.Redirect("~/uxLogin.aspx");
        }

        /// <summary>
        /// Load Module Webpage for data contained in other panels
        /// </summary>
        /// <param name="url">Url Address for page you want to load</param>
        /// <param name="panel">Panel name you want to load into</param>
        /// <param name="paramCollection">Collection of parameters if you need any</param>
        protected void deLoadModule(string url, string panel, Ext.Net.ParameterCollection paramCollection = null)
        {
            //Find the panel that's been rendered
            Ext.Net.Panel clPanel = X.GetCmp<Ext.Net.Panel>(panel);

            Ext.Net.ComponentLoader cl = new Ext.Net.ComponentLoader();
            cl.Url = url;
            cl.Mode = LoadMode.Frame;
            cl.Scripts = true;
            cl.LoadMask.ShowMask = true;

            // Only add a param collection if it's not null, otherwise this will cause an error.
            if (paramCollection != null)
            {
                cl.Params.AddRange(paramCollection);
            }
            clPanel.ClearContent();
            clPanel.Loader = cl;
            clPanel.Render();
        }


    }
}
