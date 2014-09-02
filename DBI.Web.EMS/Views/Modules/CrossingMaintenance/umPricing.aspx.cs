using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using DBI.Data.GMS;
using DBI.Data.DataFactory;
using DBI.Core.Security;
using System.Security.Claims;
using System.IO;
using System.Data.Entity;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umPricing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSelectRailroad();
            uxAddStateList.Data = StaticLists.CrossingStateList;
        }

        protected void deReadPricing(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<CROSSING_PRICING> PRIData = _context.CROSSING_PRICING.ToList();

                uxPriceStore.DataSource = PRIData;

            }

        }
        protected void deSavePricing(object sender, DirectEventArgs e)
        {
           
            ChangeRecords<AddPricing> data = new StoreDataHandler(e.ExtraParams["PRIdata"]).BatchObjectData<AddPricing>();
            foreach (AddPricing CreatedPricing in data.Created)
            {
                CROSSING_PRICING NewPricing = new CROSSING_PRICING();
                NewPricing.SERVICE_CATEGORY = CreatedPricing.SERVICE_CATEGORY;
                NewPricing.STATE = CreatedPricing.STATE;
                NewPricing.RAILROAD = CreatedPricing.RAILROAD;
                NewPricing.PRICE = CreatedPricing.PRICE;
                NewPricing.CREATE_DATE = DateTime.Now;
                NewPricing.MODIFY_DATE = DateTime.Now;
                NewPricing.CREATED_BY = User.Identity.Name;
                NewPricing.MODIFIED_BY = User.Identity.Name;
                GenericData.Insert<CROSSING_PRICING>(NewPricing);
                uxPriceStore.Reload();
            }
        }
        protected void deRemovePricing(object sender, DirectEventArgs e)
        {

            string json = e.ExtraParams["PricingInfo"];

            List<RemovePricing> PricingList = JSON.Deserialize<List<RemovePricing>>(json);
            foreach (RemovePricing Pricing in PricingList)
            {

                CROSSING_PRICING data = new CROSSING_PRICING();

                using (Entities _context = new Entities())
                {
                    data = _context.CROSSING_PRICING.Where(x => x.PRICING_ID == Pricing.PRICING_ID).SingleOrDefault();

                    uxPriceStore.Reload();
                }

                GenericData.Delete<CROSSING_PRICING>(data);

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Pricing Removed Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });


            }

        }
        protected void LoadSelectRailroad()
        {
            List<object> data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSING_RAILROAD

                        select new { d.RAILROAD, d.RAILROAD_ID }).ToList<object>();
            }
            uxRailRoadCI.Store.Primary.DataSource = data;
        }
        public class AddPricing
        {
            public decimal? PRICING_ID { get; set; }
            public string SERVICE_CATEGORY { get; set; }
            public string STATE { get; set; }
            public string RAILROAD { get; set; }
            public decimal PRICE { get; set; }
        }
        public class RemovePricing
        {
            public decimal? PRICING_ID { get; set; }
            public string SERVICE_CATEGORY { get; set; }
            public string STATE { get; set; }
            public string RAILROAD { get; set; }
            public decimal PRICE { get; set; }
        }
    }
}