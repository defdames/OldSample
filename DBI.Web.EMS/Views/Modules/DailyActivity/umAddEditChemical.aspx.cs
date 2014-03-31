using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using DBI.Data.DataFactory;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umAddEditChemical : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["type"] == "Add")
                {
                    uxAddChemicalForm.Show();
                    uxAddStateList.Data = StaticLists.StateList;
                }
                else
                {
                    uxEditChemicalForm.Show();
                    uxEditStateList.Data = StaticLists.StateList;
                    LoadEditChemicalForm();
                }
            }
        }

        /// <summary>
        /// Add chemical to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddChemical(object sender, DirectEventArgs e)
        {
            //Convert to correct types
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            decimal GallonAcre = decimal.Parse(uxAddChemicalGallonAcre.Value.ToString());
            decimal GallonStart = decimal.Parse(uxAddChemicalGallonStart.Value.ToString());
            decimal GallonMixed = decimal.Parse(uxAddChemicalGallonMixed.Value.ToString());
            decimal GallonRemain = decimal.Parse(uxAddChemicalGallonRemain.Value.ToString());
            decimal GallonUsed = GallonStart + GallonMixed - GallonRemain;
            decimal AcresSprayed = GallonUsed / GallonAcre;

            //Get Count of current records for this Header
            int count;

            using (Entities _context = new Entities())
            {
                count = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                         where d.HEADER_ID == HeaderId
                         select d).Count();
            }

            //Fill DB fields
            DAILY_ACTIVITY_CHEMICAL_MIX data = new DAILY_ACTIVITY_CHEMICAL_MIX()
            {
                CHEMICAL_MIX_NUMBER = count + 1,
                TARGET_AREA = uxAddChemicalTargetAre.Value.ToString(),
                GALLON_ACRE = GallonAcre,
                GALLON_STARTING = GallonStart,
                GALLON_MIXED = GallonMixed,
                GALLON_REMAINING = GallonRemain,
                ACRES_SPRAYED = AcresSprayed,
                STATE = uxAddChemicalState.Value.ToString(),
                COUNTY = uxAddChemicalCounty.Value.ToString(),
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name,
                HEADER_ID = HeaderId
            };

            //Write to db
            GenericData.Insert<DAILY_ACTIVITY_CHEMICAL_MIX>(data);
            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxChemicalTab.reload()");
            uxAddChemicalForm.Reset();

            //Show notification
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Chemical Mix Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Populate edit form based on existing entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadEditChemicalForm()
        {
            using(Entities _context = new Entities()){
                long ChemicalMixId = long.Parse(Request.QueryString["ChemicalMixId"]);
                DAILY_ACTIVITY_CHEMICAL_MIX data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                                    where d.CHEMICAL_MIX_ID == ChemicalMixId
                                                    select d).Single();
                try
                {
                    uxEditChemicalTargetAre.SetValue(data.TARGET_AREA.ToString());
                }
                catch
                {
                }
                try
                {
                    uxEditChemicalGallonAcre.SetValue(data.GALLON_ACRE.ToString());
                }
                catch 
                { 
                }
                try
                {
                    uxEditChemicalGallonStart.SetValue(data.GALLON_STARTING.ToString());
                }
                catch
                {
                }
                try
                {
                    uxEditChemicalGallonMixed.SetValue(data.GALLON_MIXED.ToString());
                }
                catch
                {
                }
                try
                {
                    uxEditChemicalGallonTotal.Value = int.Parse(data.GALLON_MIXED.ToString()) + int.Parse(data.GALLON_STARTING.ToString());
                }
                catch { }
                try
                {
                    uxEditChemicalGallonRemain.SetValue(data.GALLON_REMAINING.ToString());
                }
                catch
                {
                }
                try
                {
                    uxEditChemicalGallonUsed.Value = int.Parse(data.GALLON_MIXED.ToString()) + int.Parse(data.GALLON_STARTING.ToString()) - int.Parse(data.GALLON_REMAINING.ToString());
                }
                catch { }
                try
                {
                    uxEditChemicalAcresSprayed.Value = int.Parse(uxEditChemicalGallonUsed.Value.ToString()) * decimal.Parse(uxEditChemicalGallonAcre.Value.ToString());
                }
                catch { }
                try
                {
                    uxEditChemicalAcresSprayed.SetValue(data.ACRES_SPRAYED.ToString());
                }
                catch
                {
                }
                try
                {
                    uxEditChemicalState.SetValue(data.STATE);
                }
                catch
                {
                }
                try
                {
                    uxEditChemicalCounty.SetValue(data.COUNTY);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Edit Chemical entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditChemical(object sender, DirectEventArgs e)
        {
            long ChemicalId = long.Parse(Request.QueryString["ChemicalMixId"]);
            decimal GallonAcre = decimal.Parse(uxEditChemicalGallonAcre.Value.ToString());
            decimal GallonStart = decimal.Parse(uxEditChemicalGallonStart.Value.ToString());
            decimal GallonMixed = decimal.Parse(uxEditChemicalGallonMixed.Value.ToString());
            decimal GallonRemain = decimal.Parse(uxEditChemicalGallonRemain.Value.ToString());
            decimal AcresSprayed = decimal.Parse(uxEditChemicalAcresSprayed.Value.ToString());
            DAILY_ACTIVITY_CHEMICAL_MIX data;
            List<DAILY_ACTIVITY_INVENTORY> inventoryData;

            //Get record to update
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                        where d.CHEMICAL_MIX_ID == ChemicalId
                        select d).Single();

                inventoryData = (from i in _context.DAILY_ACTIVITY_INVENTORY
                                 where i.CHEMICAL_MIX_ID == ChemicalId
                                 select i).ToList();
            }

            data.TARGET_AREA = uxEditChemicalTargetAre.Value.ToString();
            data.GALLON_ACRE = GallonAcre;
            data.GALLON_STARTING = GallonStart;
            data.GALLON_MIXED = GallonMixed;
            data.GALLON_REMAINING = GallonRemain;
            data.ACRES_SPRAYED = AcresSprayed;
            data.STATE = uxEditChemicalState.Value.ToString();
            data.COUNTY = uxEditChemicalCounty.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            foreach (DAILY_ACTIVITY_INVENTORY inventoryItem in inventoryData)
            {
                inventoryItem.TOTAL = inventoryItem.RATE * AcresSprayed;
                GenericData.Update<DAILY_ACTIVITY_INVENTORY>(inventoryItem);
            }

            //Set update to database
            GenericData.Update<DAILY_ACTIVITY_CHEMICAL_MIX>(data);
            

            X.MessageBox.Confirm("Inventory Updated", "The associated Inventory item totals have been updated.  Go to the Inventory tab?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "parent.App.uxTabPanel.setActiveTab(parent.App.uxInventoryTab); parent.App.uxPlaceholderWindow.hide();",
                    Text = "Yes"
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "parent.App.uxPlaceholderWindow.hide(); parent.App.uxChemicalTab.reload()",
                    Text = "No"
                }
            }).Show();
            

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Chemical Mix Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
    }
}