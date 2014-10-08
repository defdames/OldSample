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
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }

            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["type"] == "Add")
                {
                    uxFormType.Value = "Add";
                    GetInventory();
                }
                else
                {
                    uxFormType.Value = "Edit";
                    LoadEditInventoryForm();
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
        protected void GetInventory()
        {
            //Get inventory regions from db and set datasource for either add or edit
            using (Entities _context = new Entities())
            {
                string ProjectOrg = GetProjectOrg(long.Parse(Request.QueryString["HeaderId"])).ToString();
                var data = (from d in _context.INVENTORY_V
                            where d.LE == ProjectOrg
                            select new { d.ORGANIZATION_ID, d.INV_NAME }).Distinct().OrderBy(x => x.INV_NAME).ToList();
                uxAddInventoryRegionStore.DataSource = data;
                uxAddInventoryRegionStore.DataBind();

            }
        }

        protected void deProcessForm(object sender, DirectEventArgs e)
        {
            if (uxFormType.Value.ToString() == "Add")
            {
                deAddInventory(sender, e);
            }
            else
            {
                deEditInventory(sender, e);
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
            long SubInventoryOrg = long.Parse(uxAddInventorySub.SelectedItem.Value);
            long ItemId = long.Parse(uxAddInventoryItem.Value.ToString());
            long Rate = long.Parse(uxAddInventoryRate.Text);
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
                    UNIT_OF_MEASURE = uxAddInventoryMeasure.SelectedItem.Value,
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
            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");
        }

        /// <summary>
        /// Populate Edit Inventory Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadEditInventoryForm()
        {
            long InventoryID = long.Parse(Request.QueryString["InventoryId"]);
                        
                SUBINVENTORY_V SubData;
                
                using (Entities _context = new Entities())
                {
                    var Inventory = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            where d.INVENTORY_ID == InventoryID
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new { j.ENABLED_FLAG, j.ITEM_ID, j.ACTIVE, j.LE, j.LAST_UPDATE_DATE, j.ATTRIBUTE2, j.INV_LOCATION, j.INV_NAME, j.UOM_CODE, d.INVENTORY_ID, d.UNIT_OF_MEASURE, d.SUB_INVENTORY_SECONDARY_NAME, d.SUB_INVENTORY_ORG_ID, j.SEGMENT1, j.DESCRIPTION, d.RATE}).Single();

                    var OrgId = Inventory.SUB_INVENTORY_ORG_ID;
                    var InvName = Inventory.SUB_INVENTORY_SECONDARY_NAME;

                    SubData = (from s in _context.SUBINVENTORY_V
                               where s.ORG_ID == OrgId && s.SECONDARY_INV_NAME == InvName
                               select s).SingleOrDefault();

                    GetInventory();
                    uxAddInventoryRegion.SelectedItems.Clear();
                    uxAddInventoryRegion.SetRawValue(Inventory.SUB_INVENTORY_ORG_ID);
                    //uxAddInventoryRegion.SetValueAndFireSelect(Inventory.SUB_INVENTORY_ORG_ID);
                    uxAddInventoryRegion.SelectedItems.Add(new Ext.Net.ListItem(Inventory.INV_NAME, Inventory.SUB_INVENTORY_ORG_ID));
                    uxAddInventoryRegion.UpdateSelectedItems();

                    GetSubInventory((decimal)OrgId);
                    uxAddInventorySub.SelectedItems.Clear();
                    uxAddInventorySub.SetRawValue(SubData.ORG_ID);
                    uxAddInventorySub.SetValueAndFireSelect(SubData.ORG_ID);
                    uxAddInventorySub.SelectedItems.Add(new Ext.Net.ListItem(SubData.SECONDARY_INV_NAME, SubData.ORG_ID));
                    uxAddInventorySub.UpdateSelectedItems();

                    GetUnitOfMeasure(Inventory.UOM_CODE);
                    uxAddInventoryMeasure.SelectedItems.Clear();
                    uxAddInventoryMeasure.SelectedItems.Add(new Ext.Net.ListItem(Inventory.UNIT_OF_MEASURE));
                    uxAddInventoryMeasure.UpdateSelectedItems();
                    uxAddInventoryItem.SetValue(Inventory.ITEM_ID.ToString(), Inventory.DESCRIPTION);

                    uxAddInventoryRate.SetValue(Inventory.RATE);
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
            long OrgId = long.Parse(uxAddInventoryRegion.SelectedItem.Value);
            decimal ItemId = decimal.Parse(uxAddInventoryItem.Value.ToString());
            decimal Rate = decimal.Parse(uxAddInventoryRate.Text);

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
            data.UNIT_OF_MEASURE = uxAddInventoryMeasure.SelectedItem.Value;
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            //Write to DB
            GenericData.Update<DAILY_ACTIVITY_INVENTORY>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");

        }

        /// <summary>
        /// Load SubInventories for selected region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadSubinventory(object sender, DirectEventArgs e)
        {
            decimal OrgId;
            
            OrgId = decimal.Parse(uxAddInventoryRegion.Value.ToString());
            GetSubInventory(OrgId);
        }

        protected void GetSubInventory(decimal OrgId)
        {
            //Get list of subinventories
            using (Entities _context = new Entities())
            {
                var data = (from s in _context.SUBINVENTORY_V
                            orderby s.SECONDARY_INV_NAME ascending
                            where s.ORG_ID == OrgId
                            select s).ToList();

                //Set datasource for add/edit
                uxAddInventorySub.Clear();
                uxAddInventoryItem.Clear();
                uxAddInventorySubStore.DataSource = data;
                uxAddInventorySubStore.DataBind();
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
            OrgId = long.Parse(e.Parameters["OrgId"]);

            dataIn = INVENTORY_V.GetActiveInventory(OrgId);

            int count;

            //Get paged, filterable list of inventory
            List<INVENTORY_V> data = GenericData.EnumerableFilterHeader<INVENTORY_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();
            uxAddInventoryItemStore.DataSource = data;
            uxAddInventoryItemStore.DataBind();
            e.Total = count;
        }

        /// <summary>
        /// Updates selection of Items from Add/Edit Forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            uxAddInventoryItem.SetValue(e.ExtraParams["ItemId"], e.ExtraParams["Description"]);
            uxAddInventoryItemStore.ClearFilter();
        }

        protected void deGetUnitOfMeasure(object sender, DirectEventArgs e)
        {
            GetUnitOfMeasure(e.ExtraParams["uomCode"]);
        }

        /// <summary>
        /// Gets Units of Measure from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetUnitOfMeasure(string uomCode)
        {
            //Query Db for units of measure based on uom_code
            using (Entities _context = new Entities())
            {
                List<UNIT_OF_MEASURE_V> data;
                var uomClass = (from u in _context.UNIT_OF_MEASURE_V
                                where u.UOM_CODE == uomCode
                                select u.UOM_CLASS).Single().ToString();

                data = (from u in _context.UNIT_OF_MEASURE_V
                        where u.UOM_CLASS == uomClass
                        select u).ToList();

                //Set datasource for store add/edit
                uxAddInventoryMeasureStore.DataSource = data;
                uxAddInventoryMeasureStore.DataBind();
            }
        }
    }
}