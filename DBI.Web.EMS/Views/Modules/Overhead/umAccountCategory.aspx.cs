using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAccountCategory : BasePage
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

        protected void deLoadAccounts(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var _data = _context.GL_ACCOUNTS_V.Select(x => new GL_ACCOUNTS_V2 { CATEGORY_NAME = "", CATEGORY_ID = 0, SEGMENT5 = x.SEGMENT5, SEGMENT5_DESC = x.SEGMENT5_DESC }).Distinct().OrderBy(x => x.SEGMENT5_DESC).ToList();

                int count;
                uxAccountCategoryStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;

            }
        }

        public class GL_ACCOUNTS_V2 : GL_ACCOUNTS_V
        {
            public string CATEGORY_NAME {get; set;}
            public long CATEGORY_ID {get; set;}
        }
    }
}