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
    public partial class umInventoryTab_DBI : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }

            GetInventoryData();
        }

        /// <summary>
        /// Get current inventory info
        /// </summary>
        protected void GetInventoryData()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Get Inventory for current project
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                            join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                            where d.HEADER_ID == HeaderId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new InventoryDetails{ENABLED_FLAG = j.ENABLED_FLAG, ITEM_ID = j.ITEM_ID, ACTIVE = j.ACTIVE, LE = j.LE, SEGMENT1 =  j.SEGMENT1, LAST_UPDATE_DATE = j.LAST_UPDATE_DATE, ATTRIBUTE2 =  j.ATTRIBUTE2, INV_LOCATION = j.INV_LOCATION, CONTRACTOR_SUPPLIED =(d.CONTRACTOR_SUPPLIED == "Y" ? true : false), TOTAL = d.TOTAL, INV_NAME = j.INV_NAME, INVENTORY_ID = d.INVENTORY_ID, CHEMICAL_MIX_ID = d.CHEMICAL_MIX_ID, CHEMICAL_MIX_NUMBER = c.CHEMICAL_MIX_NUMBER, SUB_INVENTORY_SECONDARY_NAME = d.SUB_INVENTORY_SECONDARY_NAME, SUB_INVENTORY_ORG_ID = d.SUB_INVENTORY_ORG_ID, DESCRIPTION = j.DESCRIPTION, RATE =  d.RATE, UOM_CODE = u.UOM_CODE, UNIT_OF_MEASURE = u.UNIT_OF_MEASURE, EPA_NUMBER = d.EPA_NUMBER }).ToList();

                uxCurrentInventoryStore.DataSource = data;
            }
        }

        /// <summary>
        /// Remove inventory entry from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveInventory(object sender, DirectEventArgs e)
        {
            long InventoryId = long.Parse(e.ExtraParams["InventoryId"]);
            DAILY_ACTIVITY_INVENTORY data;

            //Get record to be deleted
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            where d.INVENTORY_ID == InventoryId
                            select d).Single();
            }

            //Delete from DB
            GenericData.Delete<DAILY_ACTIVITY_INVENTORY>(data);

            uxCurrentInventoryStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Inventory Entry Deleted Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deLoadInventoryWindow(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (e.ExtraParams["type"] == "Add")
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadInventoryWindow_DBI('{0}', '{1}', '{2}')", "Add", HeaderId.ToString(), "None"));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadInventoryWindow_DBI('{0}', '{1}', '{2}')", "Edit", HeaderId.ToString(), e.ExtraParams["InventoryId"]));
            }
        }
    }

    public class InventoryDetails
    {
        public string ENABLED_FLAG { get; set; }
        public decimal ITEM_ID { get; set; }
        public string ACTIVE { get; set; }
        public string LE { get; set; }
        public string SEGMENT1 { get; set; }
        public DateTime? LAST_UPDATE_DATE { get; set; }
        public string ATTRIBUTE2 { get; set; }
        public long? INV_LOCATION { get; set; }
        public bool CONTRACTOR_SUPPLIED { get; set; }
        public decimal? TOTAL { get; set; }
        public string INV_NAME { get; set; }
        public long INVENTORY_ID { get; set; }
        public long? CHEMICAL_MIX_ID { get; set; }
        public long CHEMICAL_MIX_NUMBER { get; set; }
        public string SUB_INVENTORY_SECONDARY_NAME { get; set; }
        public decimal? SUB_INVENTORY_ORG_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? RATE { get; set; }
        public string UOM_CODE { get; set; }
        public string UNIT_OF_MEASURE { get; set; }
        public string EPA_NUMBER { get; set; }

    } 
}