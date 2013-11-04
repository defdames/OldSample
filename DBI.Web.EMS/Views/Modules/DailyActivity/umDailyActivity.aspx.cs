using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umDailyActivity : uxDefault
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateSubMenuItems();
        }

        protected void GenerateSubMenuItems()
        {
            Ext.Net.Menu SubMenu = new Ext.Net.Menu();
            SubMenu.Items.Add(new Ext.Net.MenuItem()
            {
                Text = "Create",
                Icon = Ext.Net.Icon.Add
            });
            SubMenu.Items.Add(new Ext.Net.MenuItem()
            {
                Text = "Edit",
                Icon = Ext.Net.Icon.ApplicationEdit
            });
            uxMenu.Items.Add(SubMenu);
        }
    }
}