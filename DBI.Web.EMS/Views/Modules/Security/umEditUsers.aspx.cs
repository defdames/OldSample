using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umEditUsers : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected List<SYS_PERMISSIONS> GetPermissions()
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PERMISSIONS.ToList();
            }
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

        protected void deLoadEditUserForm(object sender, DirectEventArgs e)
        {

        }

        protected void deUpdateUserPermissions(object sender, DirectEventArgs e)
        {

        }
    }
}