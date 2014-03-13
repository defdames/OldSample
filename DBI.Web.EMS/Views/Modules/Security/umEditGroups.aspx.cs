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
                uxGroupsStore.DataSource = _context.SYS_GROUPS.ToList();
                uxGroupsStore.DataBind();
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
            using (Entities _context = new Entities())
            {
                uxPermissionsStore.DataSource = SYS_PERMISSIONS.GetGroupPermissions("Programmer").ToList();
                uxPermissionsStore.DataBind();
            }
        }

        protected void deLoadPermissionsWindow(object sender, DirectEventArgs e)
        {
            //Get All Permissions
            List<SYS_PERMISSIONS> AllPermissions = SYS_PERMISSIONS.GetPermissionsHierarchy();
            //Get Current Group Permissions
            List<SYS_PERMISSIONS> GroupPermissions = SYS_PERMISSIONS.GetGroupPermissions("Programmer");

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

        }
    }
}