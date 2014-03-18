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
    public partial class umEditPermissions : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadPermissions(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var data = (from p in _context.SYS_PERMISSIONS
                            join s in _context.SYS_PERMISSIONS on (decimal)p.PARENT_PERM_ID equals s.PERMISSION_ID
                            select new PermissionDetails { PermissionId = p.PERMISSION_ID, PermissionName = p.PERMISSION_NAME, ParentPermissionId = (p.PARENT_PERM_ID == null ?0:p.PARENT_PERM_ID) , ParentPermissionName = s.PERMISSION_NAME }).ToList();
                int count;
                uxCurrentPermissionsStore.DataSource = GenericData.EnumerableFilterHeader<PermissionDetails>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadParents(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities()){
                var data = _context.SYS_PERMISSIONS.Where(x => (x.PARENT_PERM_ID == null) || (x.PARENT_PERM_ID == 1)).ToList();
                uxParentPermissionStore.DataSource = data;
            }
        }

        protected void deLoadAddPermissionWindow(object sender, DirectEventArgs e)
        {
            uxPermissionFormType.Value = "Add";
            uxUpdatePermissionsWindow.Show();
        }

        protected void deLoadEditPermissionWindow(object sender, DirectEventArgs e)
        {
            decimal PermissionId = decimal.Parse(e.ExtraParams["PermissionId"]);
            uxPermissionFormType.Value = "Edit";

            using (Entities _context = new Entities())
            {
                PermissionDetails PermissionToEdit = (from s in _context.SYS_PERMISSIONS
                                                          join p in _context.SYS_PERMISSIONS on (decimal)s.PARENT_PERM_ID equals p.PERMISSION_ID
                                                          where s.PERMISSION_ID == PermissionId
                                                          select new PermissionDetails{PermissionName = s.PERMISSION_NAME, ParentPermissionName = p.PERMISSION_NAME, ParentPermissionId = p.PERMISSION_ID}).Single();
                uxPermissionName.Value = PermissionToEdit.PermissionName;
                uxParentPermissionName.SelectedItems.Clear();
                uxParentPermissionName.SetValueAndFireSelect(PermissionToEdit.ParentPermissionId.ToString());
                uxParentPermissionName.SelectedItems.Add(new Ext.Net.ListItem(PermissionToEdit.ParentPermissionName, PermissionToEdit.ParentPermissionId.ToString()));
                uxParentPermissionName.UpdateSelectedItems();
            }
            uxUpdatePermissionsWindow.Show();
        }

        protected void deUpdatePermission(object sender, DirectEventArgs e)
        {
            if (uxPermissionFormType.Value.ToString() == "Add")
            {
                AddPermission();
            }
            else
            {
                EditPermission();
            }
        }

        protected void AddPermission()
        {
            SYS_PERMISSIONS PermissionToAdd = new SYS_PERMISSIONS();
            PermissionToAdd.PERMISSION_NAME = uxPermissionName.Value.ToString();
            PermissionToAdd.PARENT_PERM_ID = decimal.Parse(uxParentPermissionName.Value.ToString());
            GenericData.Insert<SYS_PERMISSIONS>(PermissionToAdd);
            uxUpdatePermissionsForm.Reset();
            uxUpdatePermissionsWindow.Hide();
            uxCurrentPermissionsStore.Reload();
        }

        protected void EditPermission()
        {
            decimal PermissionId = decimal.Parse(uxPermissionId.Value.ToString());
            SYS_PERMISSIONS PermissionToEdit;
            using (Entities _context = new Entities())
            {
                PermissionToEdit = _context.SYS_PERMISSIONS.Where(x => x.PERMISSION_ID == PermissionId).Single();
            }
            PermissionToEdit.PERMISSION_NAME = uxPermissionName.Value.ToString();
            PermissionToEdit.PARENT_PERM_ID = decimal.Parse(uxParentPermissionName.Value.ToString());
            GenericData.Update<SYS_PERMISSIONS>(PermissionToEdit);

            uxUpdatePermissionsForm.Reset();
            uxCurrentPermissionsStore.Reload();
        }
    }

    public class PermissionDetails
    {
        public decimal PermissionId { get; set; }
        public string PermissionName { get; set; }
        public decimal? ParentPermissionId { get; set; }
        public string Description { get; set; }
        public string ParentPermissionName { get; set; }
    }
}