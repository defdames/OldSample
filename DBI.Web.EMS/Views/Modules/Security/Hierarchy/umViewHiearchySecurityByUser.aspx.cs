using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security.Hierarchy
{
    public partial class umViewHiearchySecurityByUser : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.Users.Edit") )
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
        }
    }
}