using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;

namespace DBI.Web.EMS
{
    public partial class ProcessIRMInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            using(Entities _context = new Entities())
            {
                var data = (from h in _context.DAILY_ACTIVITY_HEADER
                            join i in _context.PROJECTS_V on h.PROJECT_ID equals i.PROJECT_ID
                            where i.ORG_ID == 123 && h.STATUS == 4 && h.HEADER_ID >= 3488
                            select h ).ToList();

                foreach (DAILY_ACTIVITY_HEADER _header in data)
                {
                    DBI.Data.Interface.PostInventory(_header.HEADER_ID, 1154);
                }


            }





        }
    }
}