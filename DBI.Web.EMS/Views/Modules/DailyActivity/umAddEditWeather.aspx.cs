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
    public partial class umAddEditWeather : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["type"] == "Add")
                {
                    uxAddWeatherForm.Show();
                    uxAddWeatherWindStore.Data = StaticLists.WindDirection;
                    uxAddWeatherDate.SelectedDate = DateTime.Now.Date;
                }
                else
                {
                    uxEditWeatherForm.Show();
                    LoadEditWeatherForm();
                    uxEditWeatherWindStore.Data = StaticLists.WindDirection;
                }
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
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name
            };

            try
            {
                string Comments = uxAddWeatherComments.Value.ToString();
                data.COMMENTS = Comments;
            }
            catch (NullReferenceException)
            {
                data.COMMENTS = null;
            }

            GenericData.Insert<DAILY_ACTIVITY_WEATHER>(data);

            uxAddWeatherForm.Reset();
            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxWeatherTab.reload()");

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
            long WeatherId = long.Parse(Request.QueryString["WeatherId"]);
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
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            try
            {
                string Comments = uxEditWeatherComments.Value.ToString();
                data.COMMENTS = Comments;
            }
            catch (NullReferenceException)
            {
                data.COMMENTS = null;
            }
            GenericData.Update<DAILY_ACTIVITY_WEATHER>(data);

            X.Js.Call("parent.App.uxPlaceholderWindow.hide(); parent.App.uxWeatherTab.reload()");

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
        /// Fill fields of edit form based on existing data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadEditWeatherForm()
        {
            using (Entities _context = new Entities()){
                
                long WeatherId = long.Parse(Request.QueryString["WeatherId"]);
                DAILY_ACTIVITY_WEATHER Weather = (from d in _context.DAILY_ACTIVITY_WEATHER
                                                  where d.WEATHER_ID == WeatherId
                                                  select d).Single();
                DateTime WeatherDate = (DateTime)Weather.WEATHER_DATE_TIME;
                uxEditWeatherDate.SetValue(WeatherDate.Date);
                uxEditWeatherTime.SetValue(WeatherDate.TimeOfDay);
                uxEditWeatherTemp.SetValue(Weather.TEMP);
                uxEditWeatherWindDirection.SetValue(Weather.WIND_DIRECTION);
                uxEditWeatherWindVelocity.SetValue(Weather.WIND_VELOCITY);
                uxEditWeatherHumidity.SetValue(Weather.HUMIDITY);
                uxEditWeatherComments.SetValue(Weather.COMMENTS);
            }
        }

    }
}