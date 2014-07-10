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
            //uxInvoiceChoiceStore.Data = StaticLists.InvoiceChoice;
        }

        //protected void deStateCrossingListGrid(object sender, StoreReadDataEventArgs e)
        //{
        //    string Invoice = uxInvoiceChoice.SelectedItem.Value;

        //    List<object> appData;

        //    if(Invoice != "Supplemental")
        //    {
        //        using (Entities _context = new Entities())
        //        {
        //            long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));

                    
        //             appData = (from i in _context.CROSSING_INVOICE
        //                       join a in _context.CROSSING_APPLICATION on i.INVOICE_ID equals a.INVOICE_ID
                               
        //                       //where d.RAILROAD_ID == RailroadId 
        //                       select new
        //                       {
        //                           //d.CROSSING_ID,
        //                           a.APPLICATION_ID,
        //                           a.APPLICATION_DATE,
        //                           a.APPLICATION_REQUESTED,
        //                           //d.CROSSING_NUMBER,
        //                           //d.SUB_DIVISION,
        //                           //d.SERVICE_UNIT,
        //                           //d.STATE,
        //                           //d.MILE_POST,
        //                           //a.REMARKS,
        //                           }).ToList<object>;


        //            int count;
        //            uxInvoiceApplicationStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], appData, out count);
        //            e.Total = count;
        //        }
               
        //    }

        }
    }
