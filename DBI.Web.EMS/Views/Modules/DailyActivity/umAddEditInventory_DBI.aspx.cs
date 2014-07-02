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
    public partial class umAddEditInventory_DBI : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!X.IsAjaxRequest)
            {
                GetChemicalMix();
                GetInventory();
                if (Request.QueryString["type"] == "Add")
                {
                    uxFormType.Value = "Add";
                }
                else
                {
                    LoadEditInventoryForm();
                    uxFormType.Value = "Edit";
                }
            }
        }

        /// <summary>
        /// Get Chemical Mixes entered on Chemical Mix page
        /// </summary>
        protected void GetChemicalMix()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Get Chemical mixes for current project
            using (Entities _context = new Entities())
            {
                List<DAILY_ACTIVITY_CHEMICAL_MIX> data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                                          where d.HEADER_ID == HeaderId
                                                          select d).ToList();
                uxAddInventoryMixStore.DataSource = data;
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

        /// <summary>
        /// Store Inventory entry to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddInventory(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_INVENTORY data;

            //do type conversions
            long ChemicalMix = long.Parse(uxAddInventoryMix.Value.ToString());
            long SubInventoryOrg = long.Parse(uxAddInventorySub.SelectedItem.Value);
            long ItemId = long.Parse(uxAddInventoryItem.Value.ToString());
            long Rate = long.Parse(uxAddInventoryRate.Text);
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            decimal Total = decimal.Parse(uxAddInventoryTotal.Text);
            string ContractorSupplied;
            if (uxAddInventoryContractor.Checked)
            {
                ContractorSupplied = "Y";
            }
            else
            {
                ContractorSupplied = "N";
            }
            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new DAILY_ACTIVITY_INVENTORY()
                {
                    CHEMICAL_MIX_ID = ChemicalMix,
                    SUB_INVENTORY_SECONDARY_NAME = e.ExtraParams["SecondaryInvName"],
                    SUB_INVENTORY_ORG_ID = SubInventoryOrg,
                    ITEM_ID = ItemId,
                    RATE = Rate,
                    UNIT_OF_MEASURE = uxAddInventoryMeasure.SelectedItem.Value,
                    EPA_NUMBER = uxAddInventoryEPA.Text,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name,
                    HEADER_ID = HeaderId,
                    TOTAL = Total,
                    CONTRACTOR_SUPPLIED = ContractorSupplied
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
            using (Entities _context = new Entities())
            {
                long InventoryId = long.Parse(Request.QueryString["InventoryId"]);
                var Inventory = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                            join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                            where d.INVENTORY_ID == InventoryId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new { j.ENABLED_FLAG, j.ITEM_ID, j.ACTIVE, j.LE, j.LAST_UPDATE_DATE, j.ATTRIBUTE2, j.INV_LOCATION, d.CONTRACTOR_SUPPLIED, d.TOTAL, j.INV_NAME, d.INVENTORY_ID, d.CHEMICAL_MIX_ID, c.CHEMICAL_MIX_NUMBER, d.SUB_INVENTORY_SECONDARY_NAME, d.SUB_INVENTORY_ORG_ID, j.SEGMENT1, j.DESCRIPTION, d.RATE, u.UOM_CODE, u.UNIT_OF_MEASURE, d.EPA_NUMBER }).Single();
                SUBINVENTORY_V SubData;
                var OrgId = Inventory.SUB_INVENTORY_ORG_ID;
                var InvName = Inventory.SUB_INVENTORY_SECONDARY_NAME;

                SubData = (from s in _context.SUBINVENTORY_V
                           where s.ORG_ID == OrgId && s.SECONDARY_INV_NAME == InvName
                           select s).SingleOrDefault();
                
                uxAddInventoryMix.SetValue(Inventory.CHEMICAL_MIX_ID.ToString(), Inventory.CHEMICAL_MIX_NUMBER.ToString());
                
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
                uxAddInventoryItem.SetValue(Inventory.ITEM_ID.ToString(), Inventory.DESCRIPTION);
                
                GetUnitOfMeasure(Inventory.UOM_CODE);
                uxAddInventoryMeasure.SelectedItems.Clear();
                uxAddInventoryMeasure.SelectedItems.Add(new Ext.Net.ListItem(Inventory.UNIT_OF_MEASURE, Inventory.UOM_CODE));
                uxAddInventoryMeasure.UpdateSelectedItems();
                uxAddInventoryEPA.SetValue(Inventory.EPA_NUMBER);
                uxAddInventoryRate.SetValue(Inventory.RATE);
                uxAddInventoryTotal.SetValue(Inventory.TOTAL.ToString());

                if (Inventory.CONTRACTOR_SUPPLIED == "Y")
                {
                    uxAddInventoryContractor.Checked = true;
                }
                else
                {
                    uxAddInventoryContractor.Checked = false;
                }
            }



        }

        /// <summary>
        /// Set value of chemical drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreChemicalData(object sender, DirectEventArgs e)
        {
            uxAddInventoryMix.SetValue(e.ExtraParams["MixId"], e.ExtraParams["MixNumber"]);
            uxAddInventoryMixStore.ClearFilter();
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
        /// Store edit inventory to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditInventory(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_INVENTORY data;

            //Do type conversions
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long MixNumber = long.Parse(uxAddInventoryMix.Value.ToString());
            long InventoryId = long.Parse(Request.QueryString["InventoryId"]);
            long OrgId = long.Parse(uxAddInventoryRegion.SelectedItem.Value);
            decimal ItemId = decimal.Parse(uxAddInventoryItem.Value.ToString());
            decimal Rate = decimal.Parse(uxAddInventoryRate.Text);
            string ContractorSupplied;
            if (uxAddInventoryContractor.Checked)
            {
                ContractorSupplied = "Y";
            }
            else
            {
                ContractorSupplied = "N";
            }

            //Get record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                        where d.INVENTORY_ID == InventoryId
                        select d).Single();
            }

            data.CONTRACTOR_SUPPLIED = ContractorSupplied;
            data.SUB_INVENTORY_SECONDARY_NAME = e.ExtraParams["SecondaryInvName"];
            data.SUB_INVENTORY_ORG_ID = OrgId;
            data.ITEM_ID = ItemId;
            data.RATE = Rate;
            data.UNIT_OF_MEASURE = uxAddInventoryMeasure.SelectedItem.Value;
            data.EPA_NUMBER = uxAddInventoryEPA.Text;
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            
            decimal Total;
            if(decimal.TryParse(uxAddInventoryTotal.Text, out Total)){
                data.TOTAL = Total;
            }
            else
            {
                data.TOTAL = null;
            }

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
            uxAddInventoryItem.Clear();
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

                //uxAddInventorySub.Clear();
                //uxAddInventoryItem.Clear();
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

        /// <summary>
        /// Do math on the Edit page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditMath(object sender, DirectEventArgs e)
        {
            try
            {
                long MixId = long.Parse(uxAddInventoryMix.Value.ToString());
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                DAILY_ACTIVITY_CHEMICAL_MIX ChemData;

                using (Entities _context = new Entities())
                {
                    ChemData = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                where d.CHEMICAL_MIX_NUMBER == MixId && d.HEADER_ID == HeaderId
                                select d).Single();
                }
                var GallonsUsed = ChemData.GALLON_STARTING + ChemData.GALLON_MIXED - ChemData.GALLON_REMAINING;
                var GallonAcre = ChemData.GALLON_ACRE;

                var AcresSprayed = GallonsUsed / GallonAcre;
                var Total = AcresSprayed * decimal.Parse(uxAddInventoryRate.Value.ToString());
                uxAddInventoryTotal.SetValue(Total);
            }
            catch (NullReferenceException)
            { }
        }

    }
}