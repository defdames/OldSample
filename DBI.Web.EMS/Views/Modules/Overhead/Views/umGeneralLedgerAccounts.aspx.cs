using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using System.Reflection;
using System.Linq.Expressions;
using DBI.Core.Web;
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
{
    public partial class umGeneralLedgerAccounts : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }


        protected void deReadGLSecurityCodes(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                int count;
                IQueryable<GL_ACCOUNTS_V> _data = _context.GL_ACCOUNTS_V;
                uxGlAccountSecurityStore.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
            }
        }  
    }
}