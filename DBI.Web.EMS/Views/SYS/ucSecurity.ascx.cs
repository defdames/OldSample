using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.SYS
{
    public partial class ucSecurity : BaseUserCTL
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [DirectMethod(ShowMask = true, Msg = "Loading")]
        public void Reload()
        {
            
        }
    }
}