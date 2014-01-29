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
    public partial class umWeatherTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }

            GetGridData();

            
        }

        /// <summary>
        /// Sets grid store with date from this header's existing weather
        /// </summary>
        protected void GetGridData()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_WEATHER
                            where d.HEADER_ID == HeaderId
                            select d).ToList();
                uxCurrentWeatherStore.DataSource = data;
            }
        }
 
        /// <summary>
        /// Remove weather from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveWeather(object sender, DirectEventArgs e)
        {
            long WeatherId = long.Parse(e.ExtraParams["WeatherId"]);
            DAILY_ACTIVITY_WEATHER data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_WEATHER
                        where d.WEATHER_ID == WeatherId
                        select d).Single();
            }
            GenericData.Delete<DAILY_ACTIVITY_WEATHER>(data);

            uxCurrentWeatherStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Weather Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deLoadWeatherWindow(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            if (e.ExtraParams["Type"] == "Add")
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadWeatherWindow('{0}', '{1}', '{2}')", "Add", HeaderId.ToString(), "None"));
            }
            else
            {
                X.Js.Call(string.Format("parent.App.direct.dmLoadWeatherWindow('{0}', '{1}', '{2}')", "Edit", HeaderId.ToString(), e.ExtraParams["WeatherId"]));
            }
        }  
    }
}