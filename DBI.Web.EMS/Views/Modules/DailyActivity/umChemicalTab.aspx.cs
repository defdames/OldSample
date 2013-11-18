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
                COUNTY = uxAddChemicalCounty.Value.ToString(),
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name,
                HEADER_ID = HeaderId
            };
            GenericData.Insert<DAILY_ACTIVITY_CHEMICAL_MIX>(data);
            uxAddChemicalWindow.Hide();
            uxAddChemicalForm.Reset();
            uxCurrentChemicalStore.Reload();

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

        protected void deEditChemicalForm(object sender, DirectEventArgs e)
        {
            string JsonValues = e.ExtraParams["ChemicalInfo"];
            Dictionary<string, string>[] ChemicalInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);

            foreach (Dictionary<string, string> Chemical in ChemicalInfo)
            {
                uxEditChemicalTargetAre.SetValue(Chemical["TARGET_ARE"]);
                uxEditChemicalGallonAcre.SetValue(Chemical["GALLON_ACRE"]);
                uxEditChemicalGallonStart.SetValue(Chemical["GALLON_STARTING"]);
                uxEditChemicalGallonMixed.SetValue(Chemical["GALLON_MIXED"]);
                uxEditChemicalGallonTotal.SetValue(Chemical["GALLON_TOTAL"]);
                uxEditChemicalGallonRemain.SetValue(Chemical["GALLON_REMAINING"]);
                uxEditChemicalGallonUsed.SetValue(Chemical["GALLON_USED"]);
                uxEditChemicalAcresSprayed.SetValue(Chemical["ACRES_SPRAYED"]);
                uxEditChemicalState.SetValue(Chemical["STATE"]);
                uxEditChemicalCounty.SetValue(Chemical["STATE"]);
            }
        }

        protected void deRemoveChemical(object sender, DirectEventArgs e)
        {
            long ChemicalId = long.Parse(e.ExtraParams["ChemicalId"]);
            DAILY_ACTIVITY_CHEMICAL_MIX data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                        where d.CHEMICAL_MIX_ID == ChemicalId
                        select d).Single();
            }
            long DeletedMix = data.CHEMICAL_MIX_NUMBER;

            GenericData.Delete<DAILY_ACTIVITY_CHEMICAL_MIX>(data);
            
            using (Entities _context = new Entities())
            {
                var Updates = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                               where d.CHEMICAL_MIX_NUMBER > DeletedMix
                               select d).ToList();
                foreach (var ToUpdate in Updates)
                {
                    ToUpdate.CHEMICAL_MIX_NUMBER = ToUpdate.CHEMICAL_MIX_NUMBER - 1;
                    _context.SaveChanges();
                }
                
            }
            uxCurrentChemicalStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Chemical Mix Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deEditChemical(object sender, DirectEventArgs e)
        {
            long ChemicalId = long.Parse(e.ExtraParams["ChemicalId"]);
            decimal GallonAcre = decimal.Parse(uxEditChemicalGallonAcre.Value.ToString());
            decimal GallonStart = decimal.Parse(uxEditChemicalGallonStart.Value.ToString());
            decimal GallonMixed = decimal.Parse(uxEditChemicalGallonMixed.Value.ToString());
            decimal GallonTotal = decimal.Parse(uxEditChemicalGallonTotal.Value.ToString());
            decimal GallonRemain = decimal.Parse(uxEditChemicalGallonRemain.Value.ToString());
            decimal AcresSprayed = decimal.Parse(uxEditChemicalAcresSprayed.Value.ToString());
            decimal GallonUsed = decimal.Parse(uxEditChemicalGallonUsed.Value.ToString());
            DAILY_ACTIVITY_CHEMICAL_MIX data;

            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                        where d.CHEMICAL_MIX_ID == ChemicalId
                        select d).Single();
            }

            data.TARGET_ARE = uxEditChemicalTargetAre.Value.ToString();
            data.GALLON_ACRE = GallonAcre;
            data.GALLON_STARTING = GallonStart;
            data.GALLON_MIXED = GallonMixed;
            data.GALLON_TOTAL = GallonTotal;
            data.GALLON_REMAINING = GallonRemain;
            data.GALLON_USED = GallonUsed;
            data.ACRES_SPRAYED = AcresSprayed;
            data.STATE = uxEditChemicalState.Value.ToString();
            data.COUNTY = uxEditChemicalCounty.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            GenericData.Update<DAILY_ACTIVITY_CHEMICAL_MIX>(data);

            uxEditChemicalWindow.Hide();
            uxCurrentChemicalStore.Reload();

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