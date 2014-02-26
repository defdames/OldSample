using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umManageOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadForms(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.CUSTOMER_SURVEY_FORMS
                            select d).ToList();

                uxFormDropStore.DataSource = data;
            }
        }
    }
}