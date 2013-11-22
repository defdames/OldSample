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

            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID
                            join s in _context.SUBINVENTORY_V on d.SUB_INVENTORY_ORG_ID equals s.ORG_ID
                            join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                            where d.HEADER_ID == HeaderId
                            select d).ToList();

                uxCurrentInventoryStore.DataSource = data;
            }
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
            DAILY_ACTIVITY_INVENTORY data;
            long ChemicalMix = long.Parse(uxAddInventoryMix.Value.ToString());
            long SubInventoryOrg = long.Parse(uxAddInventorySub.Value.ToString());
            long ItemId = long.Parse(uxAddInventoryItem.Value.ToString());
            long Rate = long.Parse(uxAddInventoryRate.Value.ToString());
            
            using (Entities _context = new Entities())
            {
                data = new DAILY_ACTIVITY_INVENTORY()
                {
                    CHEMICAL_MIX_ID = ChemicalMix,
                    SUB_INVENTORY_SECONDARY_NAME = e.ExtraParams["SecondaryInvName"],
                    SUB_INVENTORY_ORG_ID = SubInventoryOrg,
                    ITEM_ID = ItemId,
                    RATE = Rate,
                    UNIT_OF_MEASURE = uxAddInventoryMeasure.Value.ToString(),
                    EPA_NUMBER = uxAddInventoryEPA.Value.ToString(),
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name
                };
            }
            GenericData.Insert<DAILY_ACTIVITY_INVENTORY>(data);

            uxAddInventoryWindow.Hide();
            uxCurrentInventoryStore.Reload();
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Inventory Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deEditInventoryForm(object sender, DirectEventArgs e)
        {
            //JSON Decode Row and assign to variables
            string JsonValues = e.ExtraParams["InventoryInfo"];
            Dictionary<string, string>[] InventoryInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);

            foreach (Dictionary<string, string> Inventory in InventoryInfo)
            {
                uxEditInventoryMix.SetValue(Inventory["CHEMICAL_MIX_ID"], Inventory["CHEMICAL_MIX_NUMBER"]);
                uxEditInventorySub.SetValue(Inventory["SUB_INVENTORY_SECONDARY_NAME"]);
                uxEditInventorySub.SetRawValue(Inventory["SUB_INVENTORY_ORG_ID"]);
                uxEditInventoryItem.SetValue(Inventory["SEGMENT1"], Inventory["DESCRIPTION"] );
                uxEditInventoryRate.SetValue(Inventory["RATE"]);
                uxEditInventoryMeasure.SetValue(Inventory["UNIT_OF_MEASURE"]);
                uxEditInventoryMeasure.SetRawValue(Inventory["UOM_CODE"]);
                uxEditInventoryEPA.SetValue(Inventory["EPA_NUMBER"]);
            }
            
            dePopulateInventory(sender, e);
        }

        protected void deStoreChemicalData(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Type"] == "Add")
            {
                uxAddInventoryMix.SetValue(e.ExtraParams["MixId"], e.ExtraParams["MixNumber"]);
                uxAddInventoryMixStore.ClearFilter();
            }
            else
            {
                uxAddInventoryMix.SetValue(e.ExtraParams["MixId"], e.ExtraParams["MixNumber"]);
                uxEditInventoryMixStore.ClearFilter();
            }
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
                    uxAddInventoryItem.Clear();
                    uxAddInventorySubStore.DataSource = data;
                    uxAddInventorySubStore.DataBind();
                }
                else
                {
                    uxEditInventorySub.Clear();
                    uxEditInventoryItem.Clear();
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
                string uomCode = e.ExtraParams["uomCode"];
                List<UNIT_OF_MEASURE_V> data;
                var uomClass = (from u in _context.UNIT_OF_MEASURE_V
                                    where u.UOM_CODE == uomCode
                                    select u.UOM_CLASS).Single().ToString();

                data = (from u in _context.UNIT_OF_MEASURE_V
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

        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Type"] == "Add")
            {
                uxAddInventoryItem.SetValue(e.ExtraParams["ItemId"], e.ExtraParams["Description"]);
                uxAddInventoryItemStore.ClearFilter();
                
            }
            else
            {
                uxEditInventoryItem.SetValue(e.ExtraParams["ItemId"], e.ExtraParams["Description"]);
                uxEditInventoryItemStore.ClearFilter();
            }
        }

    }
}