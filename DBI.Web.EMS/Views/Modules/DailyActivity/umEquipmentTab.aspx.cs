using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umEquipmentTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            GetCurrentEquipment();
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            int Status = GetStatus(HeaderId);
            if (Status == 4)
            {
                uxAddEquipmentButton.Disabled = true;
            }
            if (Status == 3 && !validateComponentSecurity("SYS.DailyActivity.Post"))
            {
                uxAddEquipmentButton.Disabled = true;
            }
        }
        
        /// <summary>
        /// Gets Header's current equipment for gridpanel
        /// </summary>
        protected void GetCurrentEquipment()
        {
            //Query and set datasource for Equipment
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["headerId"]);
                var data = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                            join p in _context.CLASS_CODES_V on e.PROJECT_ID equals p.PROJECT_ID
                            where e.HEADER_ID == HeaderId
                            select new {p.CLASS_CODE, p.ORGANIZATION_NAME, e.ODOMETER_START, e.ODOMETER_END, e.PROJECT_ID, e.EQUIPMENT_ID, p.NAME, e.HEADER_ID }).ToList();
                uxCurrentEquipmentStore.DataSource = data;
            }
        }

        protected void deEnableEdit(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            int Status = GetStatus(HeaderId);

            if (Status == 4)
            {
                uxEditEquipmentButton.Disabled = true;
                uxRemoveEquipmentButton.Disabled = true;
            }
            else if (Status == 3 && !validateComponentSecurity("SYS.DailyActivity.Post"))
            {
                uxEditEquipmentButton.Disabled = true;
                uxRemoveEquipmentButton.Disabled = true;
            }
            else
            {
                uxEditEquipmentButton.Disabled = false;
                uxRemoveEquipmentButton.Disabled = false;
            }
        }



        protected int GetStatus(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                int Status = (from d in _context.DAILY_ACTIVITY_HEADER
                              where d.HEADER_ID == HeaderId
                              select (int)d.STATUS).Single();
                return Status;
            }
        }

        /// <summary>
        /// Remove equipment from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveEquipment(object sender, DirectEventArgs e)
        {
            //Convert EquipmentId to long
            long EquipmentId = long.Parse(e.ExtraParams["EquipmentId"]);
            DAILY_ACTIVITY_EQUIPMENT data;

            //Get record to be deleted
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                        where d.EQUIPMENT_ID == EquipmentId
                        select d).Single();
            }

            GenericData.Delete<DAILY_ACTIVITY_EQUIPMENT>(data);
            
            uxCurrentEquipmentStore.Reload();
            
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Equipment Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deLoadEquipmentWindow(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (e.ExtraParams["type"] == "Add")
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadEquipmentWindow('{0}', '{1}', '{2}')", "Add", HeaderId.ToString(), "None"));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadEquipmentWindow('{0}', '{1}', '{2}')", "Edit", HeaderId.ToString(), e.ExtraParams["EquipmentId"]));
            }
        }

    }
}