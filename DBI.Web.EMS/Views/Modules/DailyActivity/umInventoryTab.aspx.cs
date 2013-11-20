using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umInventoryTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetChemicalMix();
        }

        protected void GetInventoryData()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
        }

        protected void GetChemicalMix()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                List<DAILY_ACTIVITY_CHEMICAL_MIX> data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                                    where d.HEADER_ID == HeaderId
                                                    select d).ToList();
                uxAddInventoryMixStore.DataSource = data;
                uxEditInventoryMixStore.DataSource = data;
            }
        }

        protected void deAddInventory(object sender, DirectEventArgs e)
        {

        }

        protected void deEditInventoryForm(object sender, DirectEventArgs e)
        {

        }

        protected void deReadChemicalData(object sender, StoreReadDataEventArgs e)
        {

        }

        protected void deEditInventory(object sender, DirectEventArgs e)
        {

        }

        protected void deRemoveInventory(object sender, DirectEventArgs e)
        {

        }
    }
}