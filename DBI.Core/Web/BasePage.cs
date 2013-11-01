using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using Ext.Net;
using DBI.Core;
using System.Web.Script.Serialization;
using System.Security.Claims;
using DBI.Core.Security;

namespace DBI.Core.Web
{
    public class BasePage : System.Web.UI.Page
    {

        /// <summary>
        /// Code the checks for Activity, returns false if user is in role to disable the button (setDisabled)
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static void validateComponentSecurity<T>(string activity, string componentName) where T: Ext.Net.ComponentBase, new()
        {
            bool result = false;
            //Check for Administrator override, give access to everything
            if (!HttpContext.Current.User.IsInRole("SYS.Administrator"))
            {
                result = (HttpContext.Current.User.IsInRole(activity)) ? false : true;
                //Find the obect and validate it
                Ext.Net.X.GetCmp<T>(componentName).Disabled = result;
            }
        }



        /// <summary>
        /// Returns the system time as Invariant, needed to store all data in tables that require datetimes.
        /// </summary>
        /// <param name="Invariant"></param>
        /// <returns>DateTime</returns>
        public static DateTime SystemTime()
        {
            DateTime dt = DateTime.Now;
            return dt.InvariantCulture();
        }

        /// <summary>
        /// Override the onload to register the exception override script.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            ResourceManager.GetInstance(this).Listeners.AjaxRequestException.Handler = GetExceptionHandlerScript(String.Empty);
            ResourceManager.GetInstance(this).RegisterClientScriptBlock("Localization", "Ext.override({showFailure: " + GetExceptionHandlerScript(String.Empty) + "});");

            base.OnLoad(e);
        }

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
            Authentication MyAuth = new Authentication();
            MyAuth.Logout();
            X.Redirect("~/uxLogin.aspx");
        }

        /// <summary>
        /// Load Module Webpage for data contained in other panels
        /// </summary>
        /// <param name="url">Url Address for page you want to load</param>
        /// <param name="panel">Panel name you want to load into</param>
        /// <param name="paramCollection">Collection of parameters if you need any</param>
        protected void LoadModule(string url, string panel, Ext.Net.ParameterCollection paramCollection = null)
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


        /// <summary>
        /// Load Module Webpage for data contained in a new Window
        /// </summary>
        /// <param name="url">Url Address for page you want to load</param>
        /// <param name="panel">Panel name you want to load into</param>
        /// <param name="paramCollection">Collection of parameters if you need any</param>
        [DirectMethod]
        public void dmLoadModuleIntoWindow(string url, string windowTitle, string iconName = null, int pWidth = 500, int pHeight = 500, Ext.Net.ParameterCollection paramCollection = null)
        {
            //Find the panel that's been rendered
            Ext.Net.Window clWindow = new Ext.Net.Window();
            clWindow.Closable = true;
            clWindow.CloseAction = CloseAction.Destroy;
            clWindow.Title = windowTitle;
            clWindow.Width = pWidth;
            clWindow.Height = pHeight;
            clWindow.Resizable = true;
            clWindow.Layout = "FitLayout";
            clWindow.Title = windowTitle;
            if (iconName != null)
            {
                clWindow.Icon = (Icon)Enum.Parse(typeof(Icon), iconName);
            }
            clWindow.Modal = true;

            Random random = new Random();
            clWindow.ID = "gw" + random.Next(1000).ToString();

            int num = random.Next(1000);

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
            clWindow.Loader = cl;
            clWindow.Render(this.Form);
        }

        /// <summary>
        /// This displays a nice error message for all exceptions, handled and unhandled
        /// </summary>
        /// <param name="onRequestFailureScript"></param>
        /// <returns>string</returns>
        protected string GetExceptionHandlerScript(string onRequestFailureScript)
        {
            StringBuilder script = new StringBuilder();

            // overriding default exception handler here (AjaxEvent, AjaxMethod and Server-side event) to display a custom "beautified" error dialog

            script.Append(@"function(response,errorMsg) {");
            script.Append("errorMsg = response.responseText;");
            script.Append("if(errorMsg.charAt(0) == '{') errorMsg = Ext.decode(errorMsg);");
            script.Append("if(!Ext.isEmpty(errorMsg.errorMessage)) errorMsg = errorMsg.errorMessage; else if(!Ext.isEmpty(errorMsg.serviceResponse)) errorMsg = errorMsg.serviceResponse.Msg;");
            script.Append("var stack0 = errorMsg.indexOf('\\r\\n');");
            script.Append("var stack1 = errorMsg.length;");
            script.Append("var desc0 = errorMsg.indexOf(':') + 1;");
            script.Append("var desc1 = stack0;");
            script.Append("parent.Ext.MessageBox.show({");
            script.Append("title: 'System Error',");
            script.Append("msg: errorMsg.substring(desc0, desc1),"); // error message
            script.Append("value: errorMsg.substring(stack0, stack1).trim(),"); // display stack
            script.Append("buttons: Ext.MessageBox.OK,");

#if (DEBUG)
            script.Append("multiline: true,");
#else
                script.Append("multiline: false,"); // ==> "value" textfield gets hidden
#endif

            script.Append("width: 400,");
            script.Append("icon: Ext.MessageBox.ERROR");
            script.Append("});");


            if (!String.IsNullOrEmpty(onRequestFailureScript))
                script.Append(onRequestFailureScript);

            script.Append("return false;");

            script.Append(@"}");

            return script.ToString();
        }
    }
}