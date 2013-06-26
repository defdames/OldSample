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
    public partial class uxDefault : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
       
        }



        protected void deLoadSecurityUsers(object sender, DirectEventArgs e)
        {
            LoadModule("~/Views/Modules/Security/umSecurityUsersList.aspx", "uxCenter");
        }

        protected void deLoadSecurityRoles(object sender, DirectEventArgs e)
        {
            LoadModule("~/Views/Modules/Security/umSecurityRolesList.aspx", "uxCenter");
        }


    }
}