using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadMaintainBudgets : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.ViewAndMaintain"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

            }
        }

        protected void deOrganizationList(object sender, StoreReadDataEventArgs e)
        {
            List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
            List<GL_ACCOUNTS_V> _organizationAccountList = new List<GL_ACCOUNTS_V>();

            foreach(var _org in OrgsList)
            {
                using (Entities _context = new Entities())
                {
                    var _rangeList = OVERHEAD_MODULE.OverheadGLRangeByOrganizationId(_org,_context).ToList();

                    foreach (OVERHEAD_GL_RANGE_V _range in _rangeList)
                    {
                        var _accountList = GL_ACCOUNTS_V.AccountListByRange(_range.GL_RANGE_ID, _context);
                        _organizationAccountList.AddRange(_accountList.ToList());
                    }
                }
            }

            int count;
            Store1.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V>(e.Start, 1000, e.Sort, e.Parameters["filterheader"], _organizationAccountList.AsQueryable(), out count);
            e.Total = count;

        }

     
    }
}