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
            Entities _context = new Entities();
            var _data = _context.GL_ACCOUNTS_V.OrderBy(x => x.CODE_COMBINATION_ID);
            e.Total = _data.Count();
            var test = _data.Skip(e.Start).Take(10).AsEnumerable();
        }
    
    
    }
}