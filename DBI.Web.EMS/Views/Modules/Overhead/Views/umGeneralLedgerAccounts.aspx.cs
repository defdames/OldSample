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
            Entities _context = new Entities();
            IQueryable<GL_ACCOUNTS_V> _data = _context.GL_ACCOUNTS_V.OrderBy(x => x.CODE_COMBINATION_ID);

            string filter = e.Parameters["filterheader"].ToString();

            if (filter != "{}")
            {
                FilterHeaderConditions fhc = new FilterHeaderConditions(filter);

                foreach (FilterHeaderCondition condition in fhc.Conditions)
                {
                    string dataIndex = condition.DataIndex;
                    FilterType type = condition.Type;
                    string op = condition.Operator;
                    object value = condition.Value<string>();
                   _data = _data.AddContainsCondition(dataIndex, value);

                }
            }

            e.Total = _data.Count();
            uxGlAccountSecurityStore.DataSource = _data.Skip(e.Start).Take(10).AsEnumerable();
        }
    
    
    }
}