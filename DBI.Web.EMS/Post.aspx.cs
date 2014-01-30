using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;

namespace DBI.Web.EMS
{
    public partial class Post : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Post to Oracle

            Entities _context = new Entities();

            int pHeaderID = 148; // passed in from method

            var daHeader = _context.DAILY_ACTIVITY_HEADER.Where(s => s.HEADER_ID  == pHeaderID)
                            .Join(_context.PROJECTS_V, h => h.PROJECT_ID, p => p.PROJECT_ID, (h, p) =>
                                new {
                                    HEADER_ID = h.HEADER_ID,
                                    ACTIVITY_DATE = h.DA_DATE,
                                    STATE = h.STATE,
                                    PROJECT_NUMBER = p.SEGMENT1,
                                    PROJECT_NAME = p.NAME,
                                    ORG_ID = p.ORG_ID,
                                    }).SingleOrDefault();


            XXDBI_DAILY_ACTIVITY_HEADER header = new XXDBI_DAILY_ACTIVITY_HEADER();
            header.STATE = daHeader.STATE;
            header.COUNTY = "None";
            header.ACTIVITY_DATE = (DateTime)daHeader.ACTIVITY_DATE;
            header.ORG_ID = (Decimal)daHeader.ORG_ID;
            header.PROJECT_NUMBER = daHeader.PROJECT_NUMBER;
            header.PROJECT_NAME = daHeader.PROJECT_NAME;
            GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header);


        }
    }
}