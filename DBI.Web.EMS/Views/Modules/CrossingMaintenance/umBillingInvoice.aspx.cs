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
    public partial class umBillingInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxAddAppRequestedStore.Data = StaticLists.ApplicationRequested;

            }
        }
        protected void deInvoiceGrid(object sender, StoreReadDataEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
            string Application = uxAddAppReqeusted.SelectedItem.Value;
            using (Entities _context = new Entities())
            {

                //Get List of all incidents open and closed 
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                var allData = (from a in _context.CROSSING_APPLICATION
                               join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                               where d.RAILROAD_ID == RailroadId && a.APPLICATION_REQUESTED == Application
                               select new
                               {
                                   d.CROSSING_ID,
                                   a.APPLICATION_ID,
                                   a.APPLICATION_DATE,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.SERVICE_UNIT,
                                   d.STATE,
                                   d.MILE_POST,
                                   a.REMARKS,
                               });

                //filter down specific information to show the incidents needed for report
                if (StartDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.APPLICATION_DATE >= StartDate);
                }

                if (EndDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.APPLICATION_DATE <= EndDate);
                }

                if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.APPLICATION_DATE >= StartDate && x.APPLICATION_DATE <= EndDate);
                }

                List<object> _data = allData.ToList<object>();


                int count;
                uxInvoiceFormStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
            }
        }
        protected void deClearFilters(object sender, DirectEventArgs e)
        {
            uxFilterForm.Reset();
        }
        protected void deAddInvoice(object sender, DirectEventArgs e)
        {
            string InvoiceNum = uxInvoiceNumber.Value.ToString();
            DateTime InvoiceDate = (DateTime)uxInvoiceDate.Value;

            CROSSING_INVOICE data = new CROSSING_INVOICE()
            {
                INVOICE_DATE = InvoiceDate,
                INVOICE_NUMBER = InvoiceNum,
                CREATED_BY = User.Identity.Name,
                CREATE_DATE = DateTime.Now,
                MODIFIED_BY = User.Identity.Name,
                MODIFY_DATE = DateTime.Now,
                
            };
            GenericData.Insert<CROSSING_INVOICE>(data);

            //  decimal SuppInvoiceId = data.INVOICE_SUPP_ID;

            //CROSSING_APPLICATION invoice;
            //string json = (e.ExtraParams["selectedSupps"]);
            //List<SupplementalDetails> suppList = JSON.Deserialize<List<SupplementalDetails>>(json);
            //foreach (SupplementalDetails supp in suppList)
            //{

            //    using (Entities _context = new Entities())
            //    {
            //        invoice = _context.CROSSING_APPLICATION.Where(x => x.APPLICATION_ID == supp.APPLICATION_ID).SingleOrDefault();
            //        invoice.INVOICE_SUPP_ID = Convert.ToInt64(SuppInvoiceId);
            //    }
            //        GenericData.Update<CROSSING_SUPPLEMENTAL>(invoice);
                
            //    uxFilterForm.Reset();
            //    uxInvoiceSupplementalStore.Reload();

            //    Notification.Show(new NotificationConfig()
            //    {
            //        Title = "Success",
            //        Html = "Invoice Added Successfully",
            //        HideDelay = 1000,
            //        AlignCfg = new NotificationAlignConfig
            //        {
            //            ElementAnchor = AnchorPoint.Center,
            //            TargetAnchor = AnchorPoint.Center
            //        }
            //    });

        
        }
        public class ApplicationDetails
        {
            public long APPLICATION_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public Int64 INVOICE_SUPP_ID { get; set; }
            public Int64 APPLICATION_NUMBER { get; set; }
            public string APPLICATION_REQUESTED { get; set; }
            public DateTime APPLICATION_DATE { get; set; }
            public string TRUCK_NUMBER { get; set; }
            public long FISCAL_YEAR { get; set; }
            public string SPRAY { get; set; }
            public string CUT { get; set; }
            public string INSPECT { get; set; }
            public string REMARKS { get; set; }

        }
        }
    }
