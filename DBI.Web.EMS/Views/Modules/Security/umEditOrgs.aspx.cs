using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umEditOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void deReadUsers(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<SYS_USER_INFORMATION> UserList = _context.SYS_USER_INFORMATION.ToList();
                int count;
                uxUsersStore.DataSource = GenericData.EnumerableFilterHeader<SYS_USER_INFORMATION>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], UserList, out count);
                e.Total = count;
            }
        }
        protected void deLoadUpdateOrgWindow(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<ORG_HIER_V> AllOrgs = _context.ORG_HIER_V.ToList();
                long UserId = long.Parse(e.ExtraParams["UserId"]);
                
                List<ORG_HIER_V> SelectedOrgs = (from s in _context.SYS_USER_ORGS
                                                 join o in _context.ORG_HIER_V on s.ORG_ID equals o.ORG_ID
                                                 where s.USER_ID == UserId
                                                 select o).ToList();

                foreach (ORG_HIER_V SelectedOrg in SelectedOrgs)
                {
                    AllOrgs.RemoveAt(AllOrgs.FindIndex(x => x.ORG_ID == SelectedOrg.ORG_ID));
                }


                this.uxAvailableOrgsStore.DataSource = AllOrgs;
                this.uxAvailableOrgsStore.DataBind();
                uxSelectedOrgsStore.DataSource = SelectedOrgs;
                uxSelectedOrgsStore.DataBind();

                uxTwoGridWindow.Show();
            }
        }
    }
}