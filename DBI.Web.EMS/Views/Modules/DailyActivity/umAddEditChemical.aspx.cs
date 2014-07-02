﻿using System;
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
                uxAddStateList.Data = StaticLists.StateList;
                if (Request.QueryString["type"] == "Edit")
                {
                    LoadEditChemicalForm();
                    uxFormType.Value = "Edit";
                }
                else
                {
                    uxFormType.Value = "Add";
                }
            }
        }

        protected void deProcessForm(object sender, DirectEventArgs e)
        {
            if (uxFormType.Value.ToString() == "Add")
            {
                deAddChemical(sender, e);
            }
            else
            {
                deEditChemical(sender, e);
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
            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.hide()");
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
                    uxAddChemicalTargetAre.SetValue(data.TARGET_AREA.ToString());
                }
                catch
                {
                }
                try
                {
                    uxAddChemicalGallonAcre.SetValue(data.GALLON_ACRE.ToString());
                }
                catch 
                { 
                }
                try
                {
                    uxAddChemicalGallonStart.SetValue(data.GALLON_STARTING.ToString());
                }
                catch
                {
                }
                try
                {
                    uxAddChemicalGallonMixed.SetValue(data.GALLON_MIXED.ToString());
                }
                catch
                {
                }
                try
                {
                    uxAddChemicalGallonTotal.Value = int.Parse(data.GALLON_MIXED.ToString()) + int.Parse(data.GALLON_STARTING.ToString());
                }
                catch { }
                try
                {
                    uxAddChemicalGallonRemain.SetValue(data.GALLON_REMAINING.ToString());
                }
                catch
                {
                }
                try
                {
                    uxAddChemicalGallonUsed.Value = int.Parse(data.GALLON_MIXED.ToString()) + int.Parse(data.GALLON_STARTING.ToString()) - int.Parse(data.GALLON_REMAINING.ToString());
                }
                catch { }
                try
                {
                    uxAddChemicalAcresSprayed.Value = int.Parse(uxAddChemicalGallonUsed.Value.ToString()) * decimal.Parse(uxAddChemicalGallonAcre.Value.ToString());
                }
                catch { }
                try
                {
                    uxAddChemicalAcresSprayed.SetValue(data.ACRES_SPRAYED.ToString());
                }
                catch
                {
                }
                try
                {
                    uxAddChemicalState.SetValue(data.STATE);
                }
                catch
                {
                }
                try
                {
                    uxAddChemicalCounty.SetValue(data.COUNTY);
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
            decimal GallonAcre = decimal.Parse(uxAddChemicalGallonAcre.Value.ToString());
            decimal GallonStart = decimal.Parse(uxAddChemicalGallonStart.Value.ToString());
            decimal GallonMixed = decimal.Parse(uxAddChemicalGallonMixed.Value.ToString());
            decimal GallonRemain = decimal.Parse(uxAddChemicalGallonRemain.Value.ToString());
            decimal AcresSprayed = decimal.Parse(uxAddChemicalAcresSprayed.Value.ToString());
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

            data.TARGET_AREA = uxAddChemicalTargetAre.Value.ToString();
            data.GALLON_ACRE = GallonAcre;
            data.GALLON_STARTING = GallonStart;
            data.GALLON_MIXED = GallonMixed;
            data.GALLON_REMAINING = GallonRemain;
            data.ACRES_SPRAYED = AcresSprayed;
            data.STATE = uxAddChemicalState.Value.ToString();
            data.COUNTY = uxAddChemicalCounty.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            foreach (DAILY_ACTIVITY_INVENTORY inventoryItem in inventoryData)
            {
                inventoryItem.TOTAL = inventoryItem.RATE * AcresSprayed;
                GenericData.Update<DAILY_ACTIVITY_INVENTORY>(inventoryItem);
            }

            //Set update to database
            GenericData.Update<DAILY_ACTIVITY_CHEMICAL_MIX>(data);

            X.MessageBox.Alert("Inventory Updated", "The associated Inventory item totals have been updated.", "parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()").Show();
            
        }
    }
}