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
             using (Entities _context = new Entities())
                {
                    DAILY_ACTIVITY_HEADER header = _context.DAILY_ACTIVITY_HEADER.Where(h => h.HEADER_ID == 61).SingleOrDefault();
                    List<DAILY_ACTIVITY_EMPLOYEE.LABOR_HEADER_RECORD> employees = DAILY_ACTIVITY_EMPLOYEE.interfaceRecords(header.HEADER_ID, (DateTime)header.DA_DATE);

             }
        }

    }
}