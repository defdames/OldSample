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
    public partial class umInvoiceReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            uxInvoiceChoiceStore.Data = StaticLists.InvoiceChoice;
        }

        protected void deApplicationReviewGrid(object sender, StoreReadDataEventArgs e)
        {
            string Invoice = uxInvoiceChoice.SelectedItem.Value;
            decimal Application = Convert.ToDecimal(uxInvoiceChoice.SelectedItem.Value);

            List<object> appData;

            
                using (Entities _context = new Entities())
                {
                    long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));


                    appData = (from i in _context.CROSSING_INVOICE
                               join a in _context.CROSSING_APPLICATION on i.INVOICE_ID equals a.INVOICE_ID
                               join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                               where a.APPLICATION_REQUESTED == Application && d.RAILROAD_ID == RailroadId
                             
                               select new
                               {
                                  
                                   i.INVOICE_ID,
                                   i.INVOICE_NUMBER,
                                   i.INVOICE_DATE,
                                
                               }).ToList<object>();


                    int count;
                    uxInvoiceApplicationStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], appData, out count);
                    e.Total = count;
                }

            
        }
        protected void deSupplementalReviewGrid(object sender, StoreReadDataEventArgs e)
        {
            string Invoice = uxInvoiceChoice.SelectedItem.Value;
   
            List<object> suppData;

           
                using (Entities _context = new Entities())
                {
                    long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));


                    suppData = (from i in _context.CROSSING_SUPP_INVOICE
                               //join a in _context.CROSSING_SUPPLEMENTAL on i.INVOICE_SUPP_ID equals a.INVOICE_SUPP_ID
                               //join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                               //where d.RAILROAD_ID == RailroadId 
                               select new
                               {
                                   
                                   i.INVOICE_SUPP_ID,
                                   i.INVOICE_SUPP_NUMBER,
                                   i.INVOICE_SUPP_DATE,
                               }).ToList<object>();


                    int count;
                    uxInvoiceSupplementalStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"],suppData, out count);
                    e.Total = count;
                }

            
        }
        protected void deChooseType(object sender, DirectEventArgs e)
        {
            string InvoiceChoice = uxInvoiceChoice.SelectedItem.Value;
            if (InvoiceChoice != "Supplemental")
            {
                uxInvoiceApplicationGrid.Show();
                uxSupplementalInvoiceGrid.Hide();
                uxInvoiceApplicationStore.Reload();

            }
            else 
            {
                uxSupplementalInvoiceGrid.Show();
                uxInvoiceApplicationGrid.Hide();
                uxInvoiceSupplementalStore.Reload();            
            }
        
        }       
      }
    }

