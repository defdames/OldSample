using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umManageExisting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetGridData();
        }

        protected void GetGridData()
        {
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_HEADER
                            select d).ToList();
                uxManageGridStore.DataSource = data;
            }
        }
    }
}