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
    public partial class umEditGroups : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadGroups(object sender, StoreReadDataEventArgs e)
        {
            using(Entities _context = new Entities()){
                var data = _context.JOB_TITLE_V.ToList();
                int count;
                uxGroupsStore.DataSource = GenericData.EnumerableFilterHeader<JOB_TITLE_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deLoadPermissions(object sender, DirectEventArgs e)
        {
            uxPermissionsStore.Reload(new Ext.Net.ParameterCollection(){
                new Ext.Net.Parameter{
                    Name = "JobId",
                    Value = e.ExtraParams["JobId"]
                }
            });
        }

        protected void deReadPermissions(object sender, StoreReadDataEventArgs e)
        {
            long JobId = long.Parse(e.Parameters["JobId"]);
            using (Entities _context = new Entities())
            {
                uxPermissionsStore.DataSource = SYS_PERMISSIONS.GetGroupPermissions(JobId).ToList();
                uxPermissionsStore.DataBind();
            }
        }

        protected void deLoadPermissionsWindow(object sender, DirectEventArgs e)
        {
            long JobId = long.Parse(e.ExtraParams["JobId"]);
            //Get All Permissions
            List<SYS_PERMISSIONS> AllPermissions = SYS_PERMISSIONS.GetPermissionsHierarchy();
            //Get Current Group Permissions
            List<SYS_PERMISSIONS> GroupPermissions = SYS_PERMISSIONS.GetGroupPermissions(JobId);

            uxSelectedPermissionsStore.DataSource = GroupPermissions;
            uxSelectedPermissionsStore.DataBind();

            foreach (SYS_PERMISSIONS GroupPermission in GroupPermissions)
            {
                AllPermissions.RemoveAt(AllPermissions.FindIndex(x => x.PERMISSION_ID == GroupPermission.PERMISSION_ID));
            }

            uxAvailablePermissionsStore.DataSource = AllPermissions;
            uxAvailablePermissionsStore.DataBind();

            uxUpdateGroupPermissionWindow.Show();
        }

        protected void deUpdateGroupPermissions(object sender, DirectEventArgs e)
        {
            long JobId = long.Parse(e.ExtraParams["JobId"]);
            List<SYS_PERMISSIONS> ChosenPermissions = JSON.Deserialize<List<SYS_PERMISSIONS>>(e.ExtraParams["SelectedPermissions"]);
            List<SYS_PERMISSIONS> AvailablePermissions = JSON.Deserialize<List<SYS_PERMISSIONS>>(e.ExtraParams["LeftOverPermissions"]);
            SYS_GROUPS Group;
            List<SYS_GROUPS_PERMS> GroupPerms;
            using (Entities _context = new Entities())
            {
                Group = (from g in _context.SYS_GROUPS
                         where g.JOB_ID == JobId
                         select g).Single();
                GroupPerms = _context.SYS_GROUPS_PERMS.Where(x => x.GROUP_ID == Group.GROUP_ID).ToList();
            }
            foreach (SYS_PERMISSIONS Permission in ChosenPermissions)
            {
                if (!GroupPerms.Exists(x => x.PERMISSION_ID == Permission.PERMISSION_ID))
                {
                    SYS_GROUPS_PERMS NewGroupPermission = new SYS_GROUPS_PERMS
                    {
                        GROUP_ID = Group.GROUP_ID,
                        PERMISSION_ID = Permission.PERMISSION_ID,
                    };
                    GenericData.Insert<SYS_GROUPS_PERMS>(NewGroupPermission);
                }
            }
            foreach (SYS_PERMISSIONS Permission in AvailablePermissions)
            {
                if (GroupPerms.Exists(x => x.PERMISSION_ID == Permission.PERMISSION_ID))
                {
                    SYS_GROUPS_PERMS PermToRemove;
                    using (Entities _context = new Entities())
                    {
                        PermToRemove = _context.SYS_GROUPS_PERMS.Where(x => (x.GROUP_ID == Group.GROUP_ID) && (x.PERMISSION_ID == Permission.PERMISSION_ID)).Single();
                    }
                    GenericData.Delete<SYS_GROUPS_PERMS>(PermToRemove);
                }
            }

            uxUpdateGroupPermissionWindow.Hide();
            uxPermissionsGrid.Reload();
        }
    }
}