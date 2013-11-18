using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Data.DataFactory;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umWeatherTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            uxAddWeatherWindStore.Data = StaticLists.WindDirection;
            uxEditWeatherWindStore.Data = StaticLists.WindDirection;
        }

        protected void deAddWeather(object sender, DirectEventArgs e)
        {
            
        }

        protected void deEditWeather(object sender, DirectEventArgs e)
        {

        }

        protected void deRemoveWeather(object sender, DirectEventArgs e)
        {

        }

        protected void deEditWeatherForm(object sender, DirectEventArgs e)
        {

        }
    }
}