using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umSecurityRoles : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            uxSecurityRoleStore.DataSource = DBI.Data.DataFactory.SecurityRoles.GetSecurityRoles();
            uxSecurityRoleStore.DataBind();
        }

        protected void deMainTest(object sender, DirectEventArgs e)
        {
            X.Msg.Alert("test", "testing").Show();
        }
    }
}