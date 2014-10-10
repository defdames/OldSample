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
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
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
            DateTime WeatherDate = uxAddWeatherDate.SelectedDate;
            TimeSpan WeatherTime = uxAddWeatherTime.SelectedTime;
            WeatherDate = WeatherDate + WeatherTime;

            DAILY_ACTIVITY_WEATHER data = new DAILY_ACTIVITY_WEATHER()
            {
                WEATHER_DATE_TIME = WeatherDate,
                HEADER_ID = HeaderId,
                TEMP = uxAddWeatherTemp.Value.ToString(),
                WIND_DIRECTION = uxAddWeatherWindDirection.SelectedItem.Value,
                WIND_VELOCITY = uxAddWeatherWindVelocity.Text,
                HUMIDITY = uxAddWeatherHumidity.Text,
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name,
                COMMENTS = uxAddWeatherComments.Text
            };
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
            DateTime WeatherDate = uxAddWeatherDate.SelectedDate;
            TimeSpan WeatherTime = uxAddWeatherTime.SelectedTime;
            WeatherDate = WeatherDate + WeatherTime;

            DAILY_ACTIVITY_WEATHER data;

            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_WEATHER
                        where d.WEATHER_ID == WeatherId
                        select d).Single();
            }
            data.WEATHER_DATE_TIME = WeatherDate;
            data.TEMP = uxAddWeatherTemp.Text;
            data.WIND_DIRECTION = uxAddWeatherWindDirection.SelectedItem.Value;
            data.WIND_VELOCITY = uxAddWeatherWindVelocity.Text;
            data.HUMIDITY = uxAddWeatherHumidity.Text;
            data.MODIFIED_BY = User.Identity.Name;
            data.MODIFY_DATE = DateTime.Now;
            data.COMMENTS = uxAddWeatherComments.Text;
            
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