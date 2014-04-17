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
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }

            GetGridData();
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            int Status = GetStatus(HeaderId);
            if (Status == 4)
            {
                uxAddChemicalButton.Disabled = true;
            }
            if (Status == 3 && !validateComponentSecurity("SYS.DailyActivity.Post"))
            {
                uxAddChemicalButton.Disabled = true;
            }
        }

        /// <summary>
        /// Sets grid store based on existing header's chemical mix
        /// </summary>
        protected void GetGridData()
        {
            //Set header
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            
            //Get Chemical Mix data
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                            where d.HEADER_ID == HeaderId
                            orderby d.CHEMICAL_MIX_NUMBER
                            select d).ToList();
                uxCurrentChemicalStore.DataSource = data;
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

        protected void deEnableEdit(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            int Status = GetStatus(HeaderId);

            if (Status == 4)
            {
                uxEditChemicalButton.Disabled = true;
                uxRemoveChemicalButton.Disabled = true;
            }
            else if (Status == 3 && !validateComponentSecurity("SYS.DailyActivity.Post"))
            {
                uxEditChemicalButton.Disabled = true;
                uxRemoveChemicalButton.Disabled = true;
            }
            else
            {
                uxEditChemicalButton.Disabled = false;
                uxRemoveChemicalButton.Disabled = false;
            }
        }
        /// <summary>
        /// Remove chemical entry from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveChemical(object sender, DirectEventArgs e)
        {
            long ChemicalId = long.Parse(e.ExtraParams["ChemicalId"]);
            //check for existing inven
            DAILY_ACTIVITY_CHEMICAL_MIX data;
            

            //Get record to be deleted.
            using (Entities _context = new Entities())
            {
                data = _context.DAILY_ACTIVITY_CHEMICAL_MIX.Include("DAILY_ACTIVITY_INVENTORY").Where(x => x.CHEMICAL_MIX_ID == ChemicalId).Single();
                
            }
            if (data.DAILY_ACTIVITY_INVENTORY.Count == 0)
            {
                //Log Mix #
                long DeletedMix = data.CHEMICAL_MIX_NUMBER;

                //Delete from db
                GenericData.Delete<DAILY_ACTIVITY_CHEMICAL_MIX>(data);

                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
                //Get all records from this header where mix# is greater than the one that was deleted
                using (Entities _context = new Entities())
                {
                    var Updates = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                   where d.CHEMICAL_MIX_NUMBER > DeletedMix && d.HEADER_ID == HeaderId
                                   select d).ToList();

                    //Loop through and update db
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
            else
            {
                Notification.Show(new NotificationConfig()
                    {
                        Title = "Error",
                        Html = "You must first delete the associated inventory entries before deleting this item",
                        HideDelay = 1000,
                        AlignCfg = new NotificationAlignConfig
                        {
                            ElementAnchor = AnchorPoint.Center,
                            TargetAnchor = AnchorPoint.Center
                        }
                    });
            }
        }

        protected void deLoadChemicalWindow(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (e.ExtraParams["type"] == "Add")
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadChemicalWindow('{0}', '{1}', '{2}')", "Add", HeaderId.ToString(), "None"));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadChemicalWindow('{0}', '{1}', '{2}')", "Edit", HeaderId.ToString(), e.ExtraParams["ChemicalId"]));
            }
        }
    }
}