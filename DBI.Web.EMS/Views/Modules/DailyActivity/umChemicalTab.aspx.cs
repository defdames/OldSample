﻿using System;
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
    public partial class umChemicalTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetGridData();
        }

        protected void GetGridData()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                            where d.HEADER_ID == HeaderId
                            orderby d.CHEMICAL_MIX_NUMBER
                            select d).ToList();
                uxCurrentChemicalStore.DataSource = data;
            }
        }
        protected void deAddChemical(object sender, DirectEventArgs e)
        {
            //Convert to correct types
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            decimal GallonAcre = decimal.Parse(uxAddChemicalGallonAcre.Value.ToString());
            decimal GallonStart = decimal.Parse(uxAddChemicalGallonStart.Value.ToString());
            decimal GallonMixed = decimal.Parse(uxAddChemicalGallonMixed.Value.ToString());
            decimal GallonTotal = decimal.Parse(uxAddChemicalGallonTotal.Value.ToString());
            decimal GallonRemain = decimal.Parse(uxAddChemicalGallonRemain.Value.ToString());
            decimal AcresSprayed = decimal.Parse(uxAddChemicalAcresSprayed.Value.ToString());
            decimal GallonUsed = decimal.Parse(uxAddChemicalGallonUsed.Value.ToString());

            //Get Count of current records for this Header
            int count;

            using (Entities _context = new Entities())
            {
                count = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                         where d.HEADER_ID == HeaderId
                         select d).Count();
            }

            DAILY_ACTIVITY_CHEMICAL_MIX data = new DAILY_ACTIVITY_CHEMICAL_MIX()
            {
                CHEMICAL_MIX_NUMBER = count + 1,
                TARGET_ARE = uxAddChemicalTargetAre.Value.ToString(),
                GALLON_ACRE = GallonAcre,
                GALLON_STARTING = GallonStart,
                GALLON_MIXED = GallonMixed,
                GALLON_TOTAL = GallonTotal,
                GALLON_REMAINING = GallonRemain,
                GALLON_USED = GallonUsed,
                ACRES_SPRAYED = AcresSprayed,
                STATE = uxAddChemicalState.Value.ToString(),
                COUNTY = uxAddChemicalCounty.Value.ToString()
            };
            GenericData.Insert<DAILY_ACTIVITY_CHEMICAL_MIX>(data);
            uxAddChemicalWindow.Hide();
            uxCurrentChemicalStore.Reload();
        }

        protected void deEditChemicalForm(object sender, DirectEventArgs e)
        {
            string JsonValues = e.ExtraParams["ChemicalInfo"];
            Dictionary<string, string>[] ChemicalInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);

            foreach (Dictionary<string, string> Chemical in ChemicalInfo)
            {

            }
        }

        protected void deRemoveChemical(object sender, DirectEventArgs e)
        {

        }

        protected void deEditChemical(object sender, DirectEventArgs e)
        {

        }
    }
}