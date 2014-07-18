using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOrganizationBudgetSecurity : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadOrganizations(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
               {
                   string sql = @"select distinct a.organization_id,a.name as organization_name from apps.hr_all_organization_units a
                                  inner join XXEMS.OVERHEAD_GL_ACCOUNT b on B.OVERHEAD_ORG_ID = a.organization_id order by 2";

                    var data = _context.Database.SqlQuery<ORGANIZATION_VIEW>(sql).Select(a => new ORGANIZATION_VIEW { ORGANIZATION_ID = a.ORGANIZATION_ID, ORGANIZATION_NAME = a.ORGANIZATION_NAME }).ToList();

                    int count;
                    uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<ORGANIZATION_VIEW>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
         }
    }

    public class ORGANIZATION_VIEW
    {
        public long ORGANIZATION_ID { get; set; }
        public string ORGANIZATION_NAME { get; set; }
    }
}