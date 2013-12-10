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
    public partial class umManageExisting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Gets filterable list of header data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadHeaderData(object sender, StoreReadDataEventArgs e)
        {
            
            using (Entities _context = new Entities())
            {
                List<object> data;
            
                //Get List of all new headers
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                            select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE }).ToList<object>();
                
                int count;
                uxManageGridStore.DataSource = GenericData.EnumerableFilter<object>(e.Start, e.Limit, e.Sort, e.Parameters["filter"], data, out count);
            }
        }

        /// <summary>
        /// Update Tab URLs based on selected header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deSelectHeader(object sender, DirectEventArgs e)
        {
            string homeUrl = string.Format("umCombinedTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string headerUrl = string.Format("umHeaderTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string equipUrl = string.Format("umEquipmentTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string prodUrl = string.Format("umProductionTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string emplUrl = string.Format("umEmployeesTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string chemUrl = string.Format("umChemicalTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string weatherUrl = string.Format("umWeatherTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string invUrl = string.Format("umInventoryTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);

            uxCombinedTab.LoadContent(homeUrl);
            uxHeaderTab.LoadContent(headerUrl);
            uxEquipmentTab.LoadContent(equipUrl);
            uxProductionTab.LoadContent(prodUrl);
            uxEmployeeTab.LoadContent(emplUrl);
            uxChemicalTab.LoadContent(chemUrl);
            uxWeatherTab.LoadContent(weatherUrl);
            uxInventoryTab.LoadContent(invUrl);
        }

        /// <summary>
        /// Shows Submit activity Window/Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deSubmitActivity(object sender, DirectEventArgs e)
        {
            string WindowUrl = string.Format("umSubmitActivity.aspx?headerId={0}", e.ExtraParams["HeaderId"]);

            uxSubmitActivityWindow.LoadContent(WindowUrl);
            uxSubmitActivityWindow.Show();
        }

        /// <summary>
        /// Set Header to Inactive status(5)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deSetHeaderInactive(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
            DAILY_ACTIVITY_HEADER data;
            
            //Get Record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                        where d.HEADER_ID == HeaderId
                        select d).Single();
                data.STATUS = 5;
            }
            //Update record in DB
            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);

            uxManageGridStore.Reload();

        }

        /// <summary>
        /// Approve Activity(set status to 3)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deApproveActivity(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
            DAILY_ACTIVITY_HEADER data;

            //Get record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                        where d.HEADER_ID == HeaderId
                        select d).Single();
                data.STATUS = 3;
            }

            //Update record in DB
            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);

            uxManageGridStore.Reload();
        }

        /// <summary>
        /// DirectMethod accessed from umSubmitActivity.aspx when signature is missing on SubmitActivity form
        /// </summary>
        [DirectMethod]
        public void dmSubmitNotification()
        {
            Notification.Show(new NotificationConfig()
            {
                Title = "Signature Missing",
                Html = "Unable to submit, signature missing.  Please provide the foreman signature.",
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