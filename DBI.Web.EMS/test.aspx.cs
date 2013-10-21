using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;

namespace DBI.Web.EMS
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Entities _context = new Entities();
            List<MBL_PROJECT_V> returnList = new List<MBL_PROJECT_V>();

            DateTime checkDate;
            if (DateTime.TryParse("10/01/2013", out checkDate))
            {


                List<PROJECTS_V> pl = _context.PROJECTS_V.Where(p => p.LAST_UPDATE_DATE >= checkDate).ToList();
                foreach (PROJECTS_V item in pl)
                {
                    MBL_PROJECT_V rItem = new MBL_PROJECT_V();
                    rItem.PROJECT_ID = item.PROJECT_ID;
                    rItem.SEGMENT1 = item.SEGMENT1;
                    rItem.LONG_NAME = item.LONG_NAME;
                    rItem.ORG_ID = Double.Parse(item.ORG_ID.ToString());
                    rItem.CARRYING_OUT_ORGANIZATION_NAME = item.ORGANIZATION_NAME;
                    rItem.CARRYING_OUT_ORGANIZATION_ID = item.CARRYING_OUT_ORGANIZATION_ID;
                    rItem.LAST_UPDATED_DATE = item.LAST_UPDATE_DATE;
                    returnList.Add(rItem);
                }


            }
            String test = "test";
        }
    }
}