using System;
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
    public partial class umWeatherTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetGridData();
            uxAddWeatherWindStore.Data = StaticLists.WindDirection;
            uxEditWeatherWindStore.Data = StaticLists.WindDirection;
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
        /// Add Weather to db from form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddWeather(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            DateTime WeatherDate = DateTime.Parse(uxAddWeatherDate.Value.ToString());
            DateTime WeatherTime = DateTime.Parse(uxAddWeatherTime.Value.ToString());
            WeatherDate = WeatherDate + WeatherTime.TimeOfDay;

            DAILY_ACTIVITY_WEATHER data = new DAILY_ACTIVITY_WEATHER()
            {
                WEATHER_DATE_TIME = WeatherDate,
                HEADER_ID = HeaderId,
                TEMP = uxAddWeatherTemp.Value.ToString(),
                WIND_DIRECTION = uxAddWeatherWindDirection.Value.ToString(),
                WIND_VELOCITY = uxAddWeatherWindVelocity.Value.ToString(),
                HUMIDITY = uxAddWeatherHumidity.Value.ToString(),
                COMMENTS = uxAddWeatherComments.Value.ToString(),
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name
            };
            GenericData.Insert<DAILY_ACTIVITY_WEATHER>(data);
            
            uxAddWeatherWindow.Hide();
            uxCurrentWeatherStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Weather Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Edit selected weather and store to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditWeather(object sender, DirectEventArgs e)
        {           
            long WeatherId = long.Parse(e.ExtraParams["WeatherId"]);
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            DateTime WeatherDate = DateTime.Parse(uxEditWeatherDate.Value.ToString());
            DateTime WeatherTime = DateTime.Parse(uxEditWeatherTime.Value.ToString());

            WeatherDate = WeatherDate + WeatherTime.TimeOfDay;

            DAILY_ACTIVITY_WEATHER data;

            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_WEATHER
                        where d.WEATHER_ID == WeatherId
                        select d).Single();
            }
            data.WEATHER_DATE_TIME = WeatherDate;
            data.TEMP = uxEditWeatherTemp.Value.ToString();
            data.WIND_DIRECTION = uxEditWeatherWindDirection.Value.ToString();
            data.WIND_VELOCITY = uxEditWeatherWindVelocity.Value.ToString();
            data.HUMIDITY = uxEditWeatherHumidity.Value.ToString();
            data.COMMENTS = uxEditWeatherComments.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            GenericData.Update<DAILY_ACTIVITY_WEATHER>(data);

            uxEditWeatherWindow.Hide();
            uxCurrentWeatherStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Weather Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
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

        /// <summary>
        /// Fill fields of edit form based on existing data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditWeatherForm(object sender, DirectEventArgs e)
        {
            string JsonValues = e.ExtraParams["WeatherInfo"];
            Dictionary<string, string>[]WeatherInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);
            
            
            foreach (Dictionary<string, string> Weather in WeatherInfo)
            {
                uxEditWeatherDate.SetValue(DateTime.Parse(Weather["WEATHER_DATE_TIME"]).Date);
                uxEditWeatherTime.SetValue(DateTime.Parse(Weather["WEATHER_DATE_TIME"]).TimeOfDay);
                uxEditWeatherTemp.SetValue(Weather["TEMP"]);
                uxEditWeatherWindDirection.SetValue(Weather["WIND_DIRECTION"]);
                uxEditWeatherWindVelocity.SetValue(Weather["WIND_VELOCITY"]);
                uxEditWeatherHumidity.SetValue(Weather["HUMIDITY"]);
                uxEditWeatherComments.SetValue(Weather["COMMENTS"]);
            }
        }
    }
}