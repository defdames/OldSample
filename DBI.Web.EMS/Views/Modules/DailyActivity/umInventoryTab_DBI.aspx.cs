﻿using System;
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
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
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
                            select new {j.ENABLED_FLAG, j.ITEM_ID, j.ACTIVE, j.LE, j.LAST_UPDATE_DATE, j.ATTRIBUTE2, j.INV_LOCATION, d.TOTAL, j.INV_NAME, d.INVENTORY_ID, d.CHEMICAL_MIX_ID, c.CHEMICAL_MIX_NUMBER, d.SUB_INVENTORY_SECONDARY_NAME, d.SUB_INVENTORY_ORG_ID, j.SEGMENT1, j.DESCRIPTION, d.RATE, u.UOM_CODE, u.UNIT_OF_MEASURE, d.EPA_NUMBER }).ToList();

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
}