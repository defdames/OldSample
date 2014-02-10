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
                    //Create header record
                    XXDBI_DAILY_ACTIVITY_HEADER header;
                    DBI.Data.Interface.createHeaderRecords(138, out header);
                  
                    //Create Labor Records
                    DBI.Data.Interface.createLaborRecords(138, header);

             }
        }

    }
}

