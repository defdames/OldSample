using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using DBI.Data.GMS;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DBI.Data.DataFactory;


namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umSupplementalBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

         protected void deInvoiceSupplementalGrid(object sender, StoreReadDataEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
         
            using (Entities _context = new Entities())
            {

                //Get List of all incidents open and closed 
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                var allData = (from a in _context.CROSSING_SUPPLEMENTAL
                               join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                               where d.RAILROAD_ID == RailroadId
                               select new
                               {
                                   d.CROSSING_ID,
                                   a.SUPPLEMENTAL_ID,
                                   a.APPROVED_DATE,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.SERVICE_UNIT,
                                   d.STATE,
                                   a.SERVICE_TYPE,
                                   d.MILE_POST,
                                   a.TRUCK_NUMBER,
                                   a.SQUARE_FEET,
                                   
                                   a.REMARKS,
                               });

                //filter down specific information to show the incidents needed for report
                
                if (StartDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.APPROVED_DATE >= StartDate);
                }

                if (EndDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.APPROVED_DATE <= EndDate);
                }

                if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.APPROVED_DATE >= StartDate && x.APPROVED_DATE <= EndDate);
                }

                List<object> _data = allData.ToList<object>();


                int count;
                uxInvoiceSupplementalStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
            }
        }
         protected void deClearFilters(object sender, DirectEventArgs e)
         {
             uxFilterForm.Reset();
         }
         protected void deAddInvoice(object sender, DirectEventArgs e)
         {

         }
        }
    }
