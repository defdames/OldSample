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
            string InvoiceNum = uxInvoiceNumber.Value.ToString();
            DateTime InvoiceDate = (DateTime)uxInvoiceDate.Value;

            CROSSING_SUPP_INVOICE data = new CROSSING_SUPP_INVOICE()
            {
                INVOICE_SUPP_DATE = InvoiceDate,
                INVOICE_SUPP_NUMBER = InvoiceNum,
                CREATED_BY = User.Identity.Name,
                CREATE_DATE = DateTime.Now,
                MODIFIED_BY = User.Identity.Name,
                MODIFY_DATE = DateTime.Now,

            };
            GenericData.Insert<CROSSING_SUPP_INVOICE>(data);

            decimal SuppInvoiceId = data.INVOICE_SUPP_ID;

            CROSSING_SUPPLEMENTAL invoice;
            string json = (e.ExtraParams["selectedSupp"]);
            List<SupplementalDetails> suppList = JSON.Deserialize<List<SupplementalDetails>>(json);
            foreach (SupplementalDetails supp in suppList)
            {

                using (Entities _context = new Entities())
                {

                    invoice = _context.CROSSING_SUPPLEMENTAL.Where(x => x.SUPPLEMENTAL_ID == supp.SUPPLEMENTAL_ID).SingleOrDefault();

                }
                invoice.INVOICE_SUPP_ID = SuppInvoiceId;
                GenericData.Update<CROSSING_SUPPLEMENTAL>(invoice);
            } 
                uxFilterForm.Reset();
                uxInvoiceSupplementalStore.Reload();

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Invoice Added Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });

            
        }
        protected void deInvoiceReportGrid(object sender, StoreReadDataEventArgs e)
        {
            List<object> allData;

            string json = (e.Parameters["selectedSupp"]);
            List<SupplementalDetails> suppList = JSON.Deserialize<List<SupplementalDetails>>(json);
            List<decimal> ReportList = new List<decimal>();
            foreach (SupplementalDetails supp in suppList)
            {
                ReportList.Add(supp.SUPPLEMENTAL_ID);
            }
            using (Entities _context = new Entities())
            {

                //Get List of all incidents open and closed 

                allData = (from a in _context.CROSSING_SUPPLEMENTAL
                           join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                           join v in _context.CROSSING_INVOICE on a.INVOICE_SUPP_ID equals v.INVOICE_ID
                           where ReportList.Contains(a.SUPPLEMENTAL_ID)
                           select new
                           {
                               v.INVOICE_NUMBER,
                               v.INVOICE_DATE,
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
                             

                           }).ToList<object>();


                //uxInvoiceNumber.Text = allData.INVOICE_NUMBER;
            }
            uxInvoiceReportStore.DataSource = allData;
            uxInvoiceDate.SetValue(DateTime.Now);




        }
        public class SupplementalDetails
        {
            public decimal INVOICE_SUPP_ID { get; set; }
            public decimal SUPPLEMENTAL_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public DateTime APPROVED_DATE { get; set; }          
            public string SERVICE_TYPE { get; set; }
            public string TRUCK_NUMBER { get; set; }         
            public long SQUARE_FEET { get; set; }
           

        }
       
    }

}

