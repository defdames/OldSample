using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
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
            long UserId = long.Parse(e.ExtraParams["UserId"]);
            uxEditUserStore.DataSource = GetPermissions();
            uxEditUserStore.DataBind();

            uxEditUserWindow.Show();

            var UserPermissions = SYS_PERMISSIONS.GetPermissions(UserId);
            CheckboxSelectionModel GridModel = uxEditUserGrid.GetSelectionModel() as CheckboxSelectionModel;
            foreach (var UserPermission in UserPermissions)
            {
                GridModel.SelectedRows.Add(new SelectedRow(UserPermission.PERMISSION_NAME));
               
            }
            GridModel.UpdateSelection();
        }

        protected void deUpdateUserPermissions(object sender, DirectEventArgs e)
        {
            long UserId = long.Parse(e.ExtraParams["UserId"]);
            string JobTitle = SYS_USER_INFORMATION.GetJobTitle(UserId);
            List<SYS_PERMISSIONS> SelectedPermissions = JSON.Deserialize<List<SYS_PERMISSIONS>>(e.ExtraParams["Rows"]);
            List<SYS_PERMISSIONS> GroupPermissions = SYS_PERMISSIONS.GetGroupPermissions(JobTitle);
            List<SYS_USER_PERMS> UserPermissions = SYS_USER_PERMS.GetUserPermissions(UserId);
            List<SYS_PERMISSIONS> AllPermissions = GetPermissions();
            foreach (SYS_PERMISSIONS SelectedPermission in SelectedPermissions)
            {
                if (UserPermissions.Exists(x => (x.PERMISSION_ID == SelectedPermission.PERMISSION_ID) && (x.ALLOW_DENY == "D")))
                {
                    SYS_USER_PERMS ToDelete;
                    using (Entities _context = new Entities())
                    {
                        ToDelete = _context.SYS_USER_PERMS.Where(x => (x.PERMISSION_ID == SelectedPermission.PERMISSION_ID) && x.USER_ID == UserId).Single();
                    }
                    GenericData.Delete<SYS_USER_PERMS>(ToDelete);
                }
                else if (!GroupPermissions.Exists(x => x.PERMISSION_ID == SelectedPermission.PERMISSION_ID))
                {
                    //add userpermission entry as it's not in the group
                    SYS_USER_PERMS PermissionToAdd = new SYS_USER_PERMS
                    {
                        PERMISSION_ID = SelectedPermission.PERMISSION_ID,
                        USER_ID = UserId,
                        ALLOW_DENY = "A",
                    };
                    GenericData.Insert<SYS_USER_PERMS>(PermissionToAdd);
                }
                AllPermissions.Remove(AllPermissions.Find(x =>x.PERMISSION_ID == SelectedPermission.PERMISSION_ID));
            }
            foreach (SYS_PERMISSIONS NotSelectedPermission in AllPermissions)
            {
                if (GroupPermissions.Exists(x => x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID))
                {
                    //add deny
                    SYS_USER_PERMS ToDeny = new SYS_USER_PERMS
                    {
                        PERMISSION_ID = NotSelectedPermission.PERMISSION_ID,
                        USER_ID = UserId,
                        ALLOW_DENY = "D"
                    };
                    GenericData.Insert<SYS_USER_PERMS>(ToDeny);
                }
                else if (UserPermissions.Exists(x => x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID))
                {
                    //remove allow
                    SYS_USER_PERMS ToDelete;
                    using (Entities _context = new Entities())
                    {
                        ToDelete = _context.SYS_USER_PERMS.Where(x => (x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID) && (x.USER_ID == UserId)).Single();
                    }
                    GenericData.Delete<SYS_USER_PERMS>(ToDelete);
                }
            }
        }
    }
}