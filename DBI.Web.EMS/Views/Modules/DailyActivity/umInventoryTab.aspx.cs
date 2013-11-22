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

        protected void dePopulateInventory(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.INVENTORY_V
                            select new { d.ORGANIZATION_ID, d.INV_NAME }).Distinct().ToList();
                if (e.ExtraParams["Type"] == "Add")
                {
                    uxAddInventoryRegionStore.DataSource = data;
                    uxAddInventoryRegionStore.DataBind();
                }
                else
                {
                    uxEditInventoryRegionStore.DataSource = data;
                    uxEditInventoryRegionStore.DataBind();
                }
            }
        }

        protected void deAddInventory(object sender, DirectEventArgs e)
        {

        }

        protected void deEditInventoryForm(object sender, DirectEventArgs e)
        {
            dePopulateInventory(sender, e);
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

        protected void deLoadSubinventory(object sender, DirectEventArgs e)
        {
            decimal OrgId;
            if (e.ExtraParams["Type"] == "Add")
            {
                OrgId = decimal.Parse(uxAddInventoryRegion.Value.ToString());
            }
            else
            {
                OrgId = decimal.Parse(uxEditInventoryRegion.Value.ToString());
            }
            using (Entities _context = new Entities())
            {
                var data = (from s in _context.SUBINVENTORY_V
                            where s.ORG_ID == OrgId
                            select s).ToList();
                if (e.ExtraParams["Type"] == "Add")
                {
                    uxAddInventorySub.Clear();
                    uxAddInventorySubStore.DataSource = data;
                    uxAddInventorySubStore.DataBind();
                }
                else
                {
                    uxEditInventorySub.Clear();
                    uxEditInventorySubStore.DataSource = data;
                    uxEditInventorySubStore.DataBind();
                    
                }
            }
        }

        protected void deReadItems(object sender, StoreReadDataEventArgs e)
        {
            long OrgId;
            List<INVENTORY_V> dataIn;
            if (e.Parameters["Type"] == "Add")
            {
                OrgId = long.Parse(uxAddInventoryRegion.Value.ToString());
            }
            else
            {
                OrgId = long.Parse(uxEditInventoryRegion.Value.ToString());
            }
            dataIn = INVENTORY_V.GetActiveInventory(OrgId);    

            int count;
            
            List<INVENTORY_V> data = GenericData.EnumerableFilterHeader<INVENTORY_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();
            if (e.Parameters["Type"] == "Add")
            {
                uxAddInventoryItemStore.DataSource = data;
                uxAddInventoryItemStore.DataBind();
                e.Total = count;
            }
            else
            {
                uxEditInventoryItemStore.DataSource = data;
                uxEditInventoryItemStore.DataBind();
                e.Total = count;
            }
        }

        protected void deGetUnitOfMeasure(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                string uomClass = (from u in _context.UNIT_OF_MEASURE_V
                            where u.UOM_CODE == e.ExtraParams["uomCode"]
                            select u.UOM_CLASS).Single();
                var data = (from u in _context.UNIT_OF_MEASURE_V
                            where u.UOM_CLASS == uomClass
                            select u).ToList();
                if (e.ExtraParams["Type"] == "Add")
                {
                    uxAddInventoryMeasureStore.DataSource = data;
                    uxAddInventoryMeasureStore.DataBind();
                }
                else
                {
                    uxEditInventoryMeasureStore.DataSource = data;
                    uxEditInventoryMeasureStore.DataBind();
                }
            }
        }

    }
}