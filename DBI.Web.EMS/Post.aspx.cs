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


            int pHeaderID = 148; // passed in from method

            using (Entities _context = new Entities())
            {
                var query = from h in _context.DAILY_ACTIVITY_HEADER
                                join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                join l in _context.PA_LOCATIONS_V on p.LOCATION_ID equals (long)l.LOCATION_ID
                                join u in _context.SYS_USER_INFORMATION on h.CREATED_BY equals u.USER_NAME
                                where h.HEADER_ID == pHeaderID
                                select new {h, p, l, u};

                var data  = query.Single();

                Decimal generatedHeaderID = DBI.Data.Interface.nextHeaderID();

                XXDBI_DAILY_ACTIVITY_HEADER header = new XXDBI_DAILY_ACTIVITY_HEADER();
                header.STATE = data.l.REGION;
                header.COUNTY = "NONE";
                header.ACTIVITY_DATE = (DateTime)data.h.DA_DATE;
                header.ORG_ID = (Decimal)data.p.ORG_ID;
                header.PROJECT_NUMBER = data.p.SEGMENT1;
                header.PROJECT_NAME = data.p.NAME;
                header.CREATED_BY = data.u.USER_ID;
                header.CREATION_DATE = DateTime.Now;
                header.LAST_UPDATED_BY = data.u.USER_ID;
                header.LAST_UPDATE_DATE = DateTime.Now;

                var included = new[] { "STATE", "COUNTY", "ACTIVITY_DATE" };

                GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header, included, "XXDBI.XXDBI_DAILY_ACTIVITY_HEADER");


                
  
            }

        }
    }
}