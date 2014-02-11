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

                    //DAILY_ACTIVITY_HEADER headerQuery = _context.DAILY_ACTIVITY_HEADER.Where(h => h.HEADER_ID == 137).SingleOrDefault();

                    //XXDBI_DAILY_ACTIVITY_HEADER header = DBI.Data.Interface.headerInterfaceRecords(headerQuery.HEADER_ID);
                    //var columns = new[] { "DA_HEADER_ID", "STATE", "COUNTY", "ACTIVITY_DATE", "ORG_ID", "PROJECT_NUMBER", "PROJECT_NAME", "CREATED_BY", "CREATION_DATE", "LAST_UPDATED_BY", "LAST_UPDATE_DATE" };
                    //GenericData.Insert<XXDBI_DAILY_ACTIVITY_HEADER>(header, columns, "XXDBI.XXDBI_DAILY_ACTIVITY_HEADER");

                    //List<XXDBI_LABOR_HEADER> employees = DBI.Data.Interface.laborInterfaceRecords(headerQuery.HEADER_ID, header.DA_HEADER_ID, header.ACTIVITY_DATE);

                    //foreach (XXDBI_LABOR_HEADER record in employees)
                    //{
                    //    var laborColumns = new[] { "LABOR_HEADER_ID", "DA_HEADER_ID", "PROJECT_NUMBER", "TASK_NUMBER", "EMPLOYEE_NUMBER", "EMP_FULL_NAME", "ROLE", "STATE", "COUNTY", "LAB_HEADER_DATE", "QUANTITY", "ELEMENT", "ADJUSTMENT", "STATUS", "ORG_ID", "CREATED_BY", "CREATION_DATE", "LAST_UPDATE_DATE", "LAST_UPDATED_BY" };
                    //    GenericData.Insert<XXDBI_LABOR_HEADER>(record, laborColumns, "XXDBI.XXDBI_LABOR_HEADER");
                    //}

                    //Create header record
                    XXDBI_DAILY_ACTIVITY_HEADER header;
                    DBI.Data.Interface.createHeaderRecords(61, out header);
                  
                    List<XXDBI_LABOR_HEADER> laborRecords;
                    DBI.Data.Interface.createLaborRecords(61, header, out laborRecords);

                    //Create truck records
                    DBI.Data.Interface.createTruckUsageRecords(61, header, laborRecords);

                    //Create perdiem
                    DBI.Data.Interface.createPerDiemRecords(61, header);


                    //Interface.PostInventory(61);

                    //Interface.PostProduction(170);
             }
        }

    }
}

