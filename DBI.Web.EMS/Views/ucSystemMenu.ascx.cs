using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views
{
    public partial class ucSystemMenu : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [DirectMethod(ShowMask = true, Msg = "Loading")]
        public void LoadSecurity()
        {
            Ext.Net.Panel centerp = (Ext.Net.Panel)Page.FindControl("uxCenter");
            centerp.Title = "Security Roles";
            LoadUserControl("~/Views/SYS/ucSecurity", centerp, true);
        }
    }
}