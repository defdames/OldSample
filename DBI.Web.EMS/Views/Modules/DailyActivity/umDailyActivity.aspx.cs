using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data.DataFactory;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umDailyActivity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void deReadData(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            Entities _context = new Entities();
            var data = (from p in _context.PROJECTS_V
                       where p.PROJECT_TYPE == "CUSTOMER BILLING" && p.TEMPLATE_FLAG == "N" && p.PROJECT_STATUS_CODE == "APPROVED"
                       select new(p.NAME).ToList();
            uxFormProject.GetStore().DataSource = data;
        }
        //todo Finish DirectEvent and uncomment Comboboxes
        /// <summary>
        /// Direct Event that stores the Daily Activity form data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreActivity(object sender, Ext.Net.DirectEventArgs e)
        {

        }


        protected object StateList
        {
            get
            {
                return new object[]
            {
                new object[] { "AL", "Alabama" },
                new object[] { "AK", "Alaska" },
                new object[] { "AZ", "Arizona" },
                new object[] { "AR", "Arkansas" },
                new object[] { "CA", "California" },
                new object[] { "CO", "Colorado" },
                new object[] { "CT", "Connecticut" },
                new object[] { "DE", "Delaware" },
                new object[] { "DC", "District of Columbia" },
                new object[] { "FL", "Florida" },
                new object[] { "GA", "Georgia" },
                new object[] { "HI", "Hawaii" },
                new object[] { "ID", "Idaho" },
                new object[] { "IL", "Illinois" },
                new object[] { "IN", "Indiana" },
                new object[] { "IA", "Iowa" },
                new object[] { "KS", "Kansas" },
                new object[] { "KY", "Kentucky" },
                new object[] { "LA", "Louisiana" },
                new object[] { "ME", "Maine" },
                new object[] { "MD", "Maryland" },
                new object[] { "MA", "Massachusetts" },
                new object[] { "MI", "Michigan" },
                new object[] { "MN", "Minnesota" },
                new object[] { "MS", "Mississippi" },
                new object[] { "MO", "Missouri" },
                new object[] { "MT", "Montana" },
                new object[] { "NE", "Nebraska" },
                new object[] { "NV", "Nevada" },
                new object[] { "NH", "New Hampshire" },
                new object[] { "NJ", "New Jersey" },
                new object[] { "NM", "New Mexico" },
                new object[] { "NY", "New York" },
                new object[] { "NC", "North Carolina" },
                new object[] { "ND", "North Dakota" },
                new object[] { "OH", "Ohio" },
                new object[] { "OK", "Oklahoma" },
                new object[] { "OR", "Oregon" },
                new object[] { "PA", "Pennsylvania" },
                new object[] { "RI", "Rhode Island" },
                new object[] { "SC", "South Carolina" },
                new object[] { "SD", "South Dakota" },
                new object[] { "TN", "Tennessee" },
                new object[] { "TX", "Texas" },
                new object[] { "UT", "Utah" },
                new object[] { "VT", "Vermont" },
                new object[] { "VA", "Virginia" },
                new object[] { "WA", "Washington" },
                new object[] { "WV", "West Virginia" },
                new object[] { "WI", "Wisconsin" },
                new object[] { "WY", "Wyoming" } 
            };
            }
        }

    }
}