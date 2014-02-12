using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;


namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umPostMultipleWindow : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
            }
        }

        protected void deReadPostableData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<HeaderData> HeaderList = (from d in _context.DAILY_ACTIVITY_HEADER
                                  join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                  where d.STATUS == 3
                                  select new HeaderData{HEADER_ID=d.HEADER_ID, DA_DATE =  (DateTime)d.DA_DATE, SEGMENT1 =  p.SEGMENT1, LONG_NAME =  p.LONG_NAME }).ToList();

                int count;
                uxHeaderPostStore.DataSource = GenericData.EnumerableFilterHeader<HeaderData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], HeaderList, out count);
                e.Total = count;
            }
        }

        protected void dePostData(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["RowsToPost"];
            List<HeaderData> HeadersToPost = JSON.Deserialize<List<HeaderData>>(json);
            foreach (HeaderData HeaderToPost in HeadersToPost)
            {
                Interface.PostToOracle(HeaderToPost.HEADER_ID, User.Identity.Name);
            }
        }
    }
}