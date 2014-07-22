using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;


namespace DBI.Data
{
    public class CROSSING_MAINTENANCE
    {
        //public static List<DoubleComboLongId> BudgetVersions()
        //{
        //    using (Entities context = new Entities())
        //    {
        //        List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
        //        long RailRoadId = long.Parse(e.Parameters["RailroadId"]);
        //        var data = (from v in _context.PROJECTS_V
        //                    where !(from p in _context.CROSSING_PROJECT
        //                            where v.PROJECT_ID == p.PROJECT_ID && p.RAILROAD_ID == RailRoadId
        //                            select p.PROJECT_ID)
        //                        .Contains(v.PROJECT_ID)
        //                    where v.PROJECT_TYPE == "CUSTOMER BILLING" && v.TEMPLATE_FLAG == "N" && v.PROJECT_STATUS_CODE == "APPROVED" && v.ORGANIZATION_NAME.Contains(" RR") && OrgsList.Contains(v.CARRYING_OUT_ORGANIZATION_ID)
        //                    select new { v.PROJECT_ID, v.LONG_NAME, v.ORGANIZATION_NAME, v.SEGMENT1 }).ToList<object>();


        //        //uxProjectGrid.Store.Primary.DataSource = data;
        //        int count;
        //        uxProjectGrid.Store.Primary.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
        //        e.Total = count;
        //        ;
        //    }
        //}
    }
}
