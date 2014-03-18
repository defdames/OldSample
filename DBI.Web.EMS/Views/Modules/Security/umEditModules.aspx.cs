using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umEditModules : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.uxMenuItemIcon.ToBuilder().ItemsFromEnum(typeof(Icon));
        }

        protected void deReadModules(object sender, StoreReadDataEventArgs e)
        {
            Entities _context = new Entities();
            
                var data = _context.SYS_MODULES.ToList();
                int count;
                uxModuleStore.DataSource = GenericData.EnumerableFilterHeader<SYS_MODULES>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;

            
        }

        protected void deLoadMenuItems(object sender, DirectEventArgs e)
        {
            decimal ModuleId = decimal.Parse(e.ExtraParams["ModuleId"]);
            uxMenuItemsStore.Reload(new Ext.Net.ParameterCollection()
            {
                new Ext.Net.Parameter("ModuleId", ModuleId.ToString())
            });
        }

        protected void deReadMenuItems(object sender, StoreReadDataEventArgs e)
        {
            decimal ModuleId;
            if(decimal.TryParse(e.Parameters["ModuleId"], out ModuleId)){
                Entities _context = new Entities();
                var data = _context.SYS_MENU.Where(x => x.MODULE_ID == ModuleId).ToList();
                int count;
                uxMenuItemsStore.DataSource = GenericData.EnumerableFilterHeader<SYS_MENU>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deLoadAddModuleWindow(object sender, DirectEventArgs e)
        {
            uxModuleFormType.Value = "Add";
            GetPermissionsAtLevel(1);
            uxModulesWindow.Show();
            
        }

        protected void deLoadEditModuleWindow(object sender, DirectEventArgs e)
        {
            uxModuleFormType.Value = "Edit";
            GetPermissionsAtLevel(1);
            //load info
            decimal ModuleId = decimal.Parse(e.ExtraParams["ModuleId"]);
            using (Entities _context = new Entities())
            {
                SYS_MODULES ModuleToEdit = _context.SYS_MODULES.Where(x => x.MODULE_ID == ModuleId).Single();
                uxModuleName.Value = ModuleToEdit.MODULE_NAME;
                uxModulePermission.SelectedItems.Clear();
                uxModulePermission.SetValueAndFireSelect(ModuleToEdit.PERMISSION_ID.ToString());
                uxModulePermission.SelectedItems.Add(new Ext.Net.ListItem(ModuleToEdit.SYS_PERMISSIONS.PERMISSION_NAME.ToString(), ModuleToEdit.PERMISSION_ID));
                uxModulePermission.UpdateSelectedItems();
                uxModuleId.Value = ModuleToEdit.MODULE_ID.ToString();
            }
            uxModulesWindow.Show();
        }

        protected void deLoadAddItemWindow(object sender, DirectEventArgs e)
        {
            uxMenuItemsFormType.Value = "Add";
            GetPermissionsAtLevel(2);
            uxMenuItemModuleStore.DataSource = GetModuleStore();
            uxMenuItemModuleStore.DataBind();
            uxMenuItemsWindow.Show();
            
        }

        protected void deLoadEditItemWindow(object sender, DirectEventArgs e)
        {
            uxMenuItemsFormType.Value = "Edit";
            GetPermissionsAtLevel(2);
            uxMenuItemModuleStore.DataSource = GetModuleStore();
            uxMenuItemModuleStore.DataBind();
            //load info
            decimal ItemId = decimal.Parse(e.ExtraParams["ItemId"]);
            using (Entities _context = new Entities())
            {
                SYS_MENU MenuToEdit = _context.SYS_MENU.Where(x => x.MENU_ID == ItemId).Single();
                uxMenuItemId.Value = MenuToEdit.MENU_ID.ToString();
                uxMenuItemModule.SelectedItems.Clear();
                uxMenuItemModule.SetValueAndFireSelect(MenuToEdit.MODULE_ID.ToString());
                uxMenuItemModule.SelectedItems.Add(new Ext.Net.ListItem(MenuToEdit.SYS_MODULES.MODULE_NAME.ToString(), MenuToEdit.MODULE_ID));
                uxMenuItemModule.UpdateSelectedItems();
                uxMenuItemPermission.SelectedItems.Clear();
                uxMenuItemPermission.SetValueAndFireSelect(MenuToEdit.PERMISSION_ID.ToString());
                uxMenuItemPermission.SelectedItems.Add(new Ext.Net.ListItem(MenuToEdit.SYS_PERMISSIONS.PERMISSION_NAME.ToString(), MenuToEdit.PERMISSION_ID));
                uxMenuItemPermission.UpdateSelectedItems();
                uxMenuItemName.Value = MenuToEdit.ITEM_NAME;
                uxMenuItemURL.Value = MenuToEdit.ITEM_URL;
            }
            uxMenuItemsWindow.Show();
        }

        protected void deUpdateModule(object sender, DirectEventArgs e)
        {
            if (uxModuleFormType.Value.ToString() == "Add")
            {
                AddModule();
            }
            else
            {
                EditModule();
            }
        }

        protected void deUpdateMenuItem(object sender, DirectEventArgs e)
        {
            if (uxMenuItemsFormType.Value.ToString() == "Add")
            {
                AddMenuItem();
            }
            else
            {
                EditMenuItem();
            }
        }

        protected void AddModule()
        {
            SYS_MODULES NewModule = new SYS_MODULES();
            NewModule.MODULE_NAME = uxModuleName.Value.ToString();
            NewModule.PERMISSION_ID = decimal.Parse(uxModulePermission.Value.ToString());

            GenericData.Insert<SYS_MODULES>(NewModule);

            uxModulesWindow.Hide();
            uxModuleStore.Reload();
        }

        protected void EditModule()
        {
            decimal ModuleId = decimal.Parse(uxModuleId.Value.ToString());
            SYS_MODULES ModuleToEdit;

            using (Entities _context = new Entities())
            {
                ModuleToEdit = _context.SYS_MODULES.Where(x => x.MODULE_ID == ModuleId).Single();
                ModuleToEdit.MODULE_NAME = uxModuleName.Value.ToString();
                ModuleToEdit.PERMISSION_ID = decimal.Parse(uxModulePermission.Value.ToString());
            }
            GenericData.Update<SYS_MODULES>(ModuleToEdit);

            uxModulesWindow.Hide();
            uxModuleStore.Reload();
        }

        protected void AddMenuItem()
        {
            SYS_MENU NewMenu = new SYS_MENU();
            NewMenu.ITEM_NAME = uxMenuItemName.Value.ToString();
            NewMenu.ITEM_URL = uxMenuItemURL.Value.ToString();
            NewMenu.MODULE_ID = decimal.Parse(uxMenuItemModule.Value.ToString());
            NewMenu.PERMISSION_ID = decimal.Parse(uxMenuItemPermission.Value.ToString());
            GenericData.Insert<SYS_MENU>(NewMenu);

            uxMenuItemsWindow.Hide();
            uxMenuItemsStore.Reload(new Ext.Net.ParameterCollection()
            {
                new Ext.Net.Parameter("ModuleId", uxMenuItemModule.Value.ToString())
            });
        }

        protected void EditMenuItem()
        {
            SYS_MENU MenuToEdit;
            decimal MenuId = decimal.Parse(uxMenuItemId.Value.ToString());
            using (Entities _context = new Entities())
            {
                MenuToEdit = _context.SYS_MENU.Where(x => x.MENU_ID == MenuId).Single();
                MenuToEdit.ITEM_NAME = uxMenuItemName.Value.ToString();
                MenuToEdit.ITEM_URL = uxMenuItemURL.Value.ToString();
                MenuToEdit.MODULE_ID = decimal.Parse(uxMenuItemModule.Value.ToString());
                MenuToEdit.PERMISSION_ID = decimal.Parse(uxMenuItemPermission.Value.ToString());
            }
            GenericData.Update<SYS_MENU>(MenuToEdit);

            uxMenuItemsWindow.Hide();
            uxMenuItemsStore.Reload(new Ext.Net.ParameterCollection()
            {
                new Ext.Net.Parameter("ModuleId", uxMenuItemModule.Value.ToString())
            });
            
        }

        protected void GetPermissionsAtLevel(int level)
        {
            using (Entities _context = new Entities()){
                List<SYS_PERMISSIONS> data;
                if(level == 1)
                {
                    data = _context.SYS_PERMISSIONS.Where(x => x.PARENT_PERM_ID == 1).ToList();
                    uxModulePermissionStore.DataSource = data;
                    uxModulePermissionStore.DataBind();
                }
                else{
                    data = _context.SYS_PERMISSIONS.Where(x => (x.PARENT_PERM_ID != 1) && (x.PARENT_PERM_ID != null)).ToList();
                    uxMenuItemPermissionStore.DataSource = data;
                    uxMenuItemPermissionStore.DataBind();
                }
                
            }
        }

        protected List<SYS_MODULES> GetModuleStore()
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_MODULES.ToList();
            }
        }
    }
}