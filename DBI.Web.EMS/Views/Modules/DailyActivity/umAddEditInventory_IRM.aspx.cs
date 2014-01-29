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
    public partial class umAddEditInventory_IRM : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["type"] == "Add")
                {
                    uxAddInventoryForm.Show();
                    GetInventory("Add");
                }
                else
                {
                    uxEditInventoryForm.Show();
                    LoadEditInventoryForm();
                    GetInventory("Edit");
                }
            }
        }

        /// <summary>
        /// Gets the project org ID of the Header's Project
        /// </summary>
        /// <param name="HeaderId"></param>
        protected long? GetProjectOrg(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                long? ProjectId = (from d in _context.DAILY_ACTIVITY_HEADER
                                   where d.HEADER_ID == HeaderId
                                   select d.PROJECT_ID).Single();
                long? OrgId = (from d in _context.PROJECTS_V
                               where d.PROJECT_ID == ProjectId
                               select d.ORG_ID).Single();
                return OrgId;
            }
        }

        /// <summary>
        /// Get List of Inventory Regions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetInventory(string FormType)
        {
            //Get inventory regions from db and set datasource for either add or edit
            using (Entities _context = new Entities())
            {
                string ProjectOrg = GetProjectOrg(long.Parse(Request.QueryString["HeaderId"])).ToString();
                var data = (from d in _context.INVENTORY_V
                            where d.LE == ProjectOrg
                            select new { d.ORGANIZATION_ID, d.INV_NAME }).Distinct().OrderBy(x => x.INV_NAME).ToList();
                if (FormType == "Add")
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

        /// <summary>
        /// Store Inventory entry to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddInventory(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_INVENTORY data;

            //do type conversions
            long SubInventoryOrg = long.Parse(uxAddInventorySub.Value.ToString());
            long ItemId = long.Parse(uxAddInventoryItem.Value.ToString());
            long Rate = long.Parse(uxAddInventoryRate.Value.ToString());
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new DAILY_ACTIVITY_INVENTORY()
                {
                    SUB_INVENTORY_SECONDARY_NAME = e.ExtraParams["SecondaryInvName"],
                    SUB_INVENTORY_ORG_ID = SubInventoryOrg,
                    ITEM_ID = ItemId,
                    RATE = Rate,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name,
                    HEADER_ID = HeaderId
                };
            }

            //Process addition
            GenericData.Insert<DAILY_ACTIVITY_INVENTORY>(data);

            uxAddInventoryForm.Reset();
            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxInventoryTab.reload()");

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

        /// <summary>
        /// Populate Edit Inventory Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadEditInventoryForm()
        {
            uxEditInventoryForm.Reset();
            long InventoryID = long.Parse(Request.QueryString["InventoryId"]);
                        
                SUBINVENTORY_V SubData;
                
                using (Entities _context = new Entities())
                {
                    var Inventory = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            where d.INVENTORY_ID == InventoryID
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new { j.ENABLED_FLAG, j.ITEM_ID, j.ACTIVE, j.LE, j.LAST_UPDATE_DATE, j.ATTRIBUTE2, j.INV_LOCATION, j.INV_NAME, d.INVENTORY_ID, d.SUB_INVENTORY_SECONDARY_NAME, d.SUB_INVENTORY_ORG_ID, j.SEGMENT1, j.DESCRIPTION, d.RATE}).Single();

                    var OrgId = Inventory.SUB_INVENTORY_ORG_ID;
                    var InvName = Inventory.SUB_INVENTORY_SECONDARY_NAME;

                    SubData = (from s in _context.SUBINVENTORY_V
                               where s.ORG_ID == OrgId && s.SECONDARY_INV_NAME == InvName
                               select s).SingleOrDefault();

                    GetInventory("Edit");
                    uxEditInventoryRegion.SelectedItems.Clear();
                    uxEditInventoryRegion.SetValueAndFireSelect(Inventory.SUB_INVENTORY_ORG_ID);
                    uxEditInventoryRegion.SelectedItems.Add(new Ext.Net.ListItem(Inventory.INV_NAME, Inventory.SUB_INVENTORY_ORG_ID));
                    uxEditInventoryRegion.UpdateSelectedItems();
                    uxEditInventorySub.SetValueAndFireSelect(SubData.DESCRIPTION);
                    uxEditInventoryItem.SetValue(Inventory.ITEM_ID.ToString(), Inventory.DESCRIPTION);
                    uxEditInventoryRate.SetValue(Inventory.RATE);
                }

                
            }

        /// <summary>
        /// Store edit inventory to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditInventory(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_INVENTORY data;

            //Do type conversions
            long InventoryId = long.Parse(Request.QueryString["InventoryId"]);
            long OrgId = long.Parse(uxEditInventoryRegion.Value.ToString());
            decimal ItemId = decimal.Parse(uxEditInventoryItem.Value.ToString());
            decimal Rate = decimal.Parse(uxEditInventoryRate.Value.ToString());

            //Get record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                        where d.INVENTORY_ID == InventoryId
                        select d).Single();
            }
            data.SUB_INVENTORY_SECONDARY_NAME = e.ExtraParams["SecondaryInvName"];
            data.SUB_INVENTORY_ORG_ID = OrgId;
            data.ITEM_ID = ItemId;
            data.RATE = Rate;
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            //Write to DB
            GenericData.Update<DAILY_ACTIVITY_INVENTORY>(data);

            uxEditInventoryForm.Reset();
            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxInventoryTab.reload()");

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Inventory Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Load SubInventories for selected region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Get list of subinventories
            using (Entities _context = new Entities())
            {
                var data = (from s in _context.SUBINVENTORY_V
                            orderby s.SECONDARY_INV_NAME ascending
                            where s.ORG_ID == OrgId
                            select s).ToList();

                //Set datasource for add/edit
                if (e.ExtraParams["Type"] == "Add")
                {
                    uxAddInventorySub.Clear();
                    uxAddInventoryItem.Clear();
                    uxAddInventorySubStore.DataSource = data;
                    uxAddInventorySubStore.DataBind();
                }
                else
                {
                    uxEditInventorySubStore.DataSource = data;
                    uxEditInventorySubStore.DataBind();

                }
            }
        }

        /// <summary>
        /// Get List of Inventory Items for OrgId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Get paged, filterable list of inventory
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

        /// <summary>
        /// Updates selection of Items from Add/Edit Forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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