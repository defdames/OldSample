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
                uxAddWeatherWindStore.Data = StaticLists.WindDirection;
                if (Request.QueryString["type"] == "Add")
                {
                    uxAddWeatherDate.SelectedDate = DateTime.Now.Date;
                    uxFormType.Value = "Add";
                }
                else
                {
                    LoadEditWeatherForm();
                    uxFormType.Value = "Edit";
                }
            }
        }

        protected void deProcessForm(object sender, DirectEventArgs e)
        {
            if (uxFormType.Value.ToString() == "Add")
            {
                deAddWeather(sender, e);
            }
            else
            {
                deEditWeather(sender, e);
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
            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.hide()");
        }

        /// <summary>
        /// Edit selected weather and store to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditWeather(object sender, DirectEventArgs e)
        {
            long WeatherId = long.Parse(Request.QueryString["WeatherId"]);
            DateTime WeatherDate = DateTime.Parse(uxAddWeatherDate.Value.ToString());
            DateTime WeatherTime = DateTime.Parse(uxAddWeatherTime.Value.ToString());

            WeatherDate = WeatherDate + WeatherTime.TimeOfDay;

            DAILY_ACTIVITY_WEATHER data;

            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_WEATHER
                        where d.WEATHER_ID == WeatherId
                        select d).Single();
            }
            data.WEATHER_DATE_TIME = WeatherDate;
            data.TEMP = uxAddWeatherTemp.Value.ToString();
            data.WIND_DIRECTION = uxAddWeatherWindDirection.Value.ToString();
            data.WIND_VELOCITY = uxAddWeatherWindVelocity.Value.ToString();
            data.HUMIDITY = uxAddWeatherHumidity.Value.ToString();
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;

            try
            {
                string Comments = uxAddWeatherComments.Value.ToString();
                data.COMMENTS = Comments;
            }
            catch (NullReferenceException)
            {
                data.COMMENTS = null;
            }
            GenericData.Update<DAILY_ACTIVITY_WEATHER>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.hide()");
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
                uxAddWeatherDate.SetValue(WeatherDate.Date);
                uxAddWeatherTime.SetValue(WeatherDate.TimeOfDay);
                uxAddWeatherTemp.SetValue(Weather.TEMP);
                uxAddWeatherWindDirection.SetValue(Weather.WIND_DIRECTION);
                uxAddWeatherWindVelocity.SetValue(Weather.WIND_VELOCITY);
                uxAddWeatherHumidity.SetValue(Weather.HUMIDITY);
                uxAddWeatherComments.SetValue(Weather.COMMENTS);
            }
        }

    }
}