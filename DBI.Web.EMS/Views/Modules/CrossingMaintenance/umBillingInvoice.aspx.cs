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
                if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    deGetRRType("Add");

                }
            }
        }
        protected void deInvoiceGrid(object sender, StoreReadDataEventArgs e)
        {
           
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
            decimal Application = Convert.ToDecimal(uxAddAppReqeusted.SelectedItem.Value);
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            using (Entities _context = new Entities())
            {

                //Get List of all incidents open and closed 
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                IQueryable<CROSSING_MAINTENANCE.CompletedCrossings> allData = CROSSING_MAINTENANCE.GetCompletedCrossings(RailroadId, Application, _context);
                
                //filter down specific information to show the crossings needed for report
                if (ServiceUnit != null)
                {
                    allData = allData.Where(x => x.SERVICE_UNIT == ServiceUnit);
                }
                if (SubDiv != null)
                {
                    allData = allData.Where(x => x.SUB_DIVISION == SubDiv);
                }
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
        protected void deValidationInvoiceButton(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = CheckboxSelectionModel2;

            if (sm.SelectedRows.Count() != 0)
            {
                Button1.Enable();
                Button2.Enable();
            }
            else
            {
                Button1.Disable();
                Button2.Disable();
            }
        }
        protected void deClearFilters(object sender, DirectEventArgs e)
        {
            uxFilterForm.Reset();
        }
        protected void deResetInvoice(object sender, DirectEventArgs e)
        {
            uxInvoiceFormStore.Reload();
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
            InvoiceDateTextField.Text = DateTime.Now.ToString("MM/dd/yyyy");
            InvoiceNumTextField.Text = (data.INVOICE_NUMBER);
           
            decimal InvoiceId = data.INVOICE_ID;

            CROSSING_APPLICATION invoice;
            string json = (e.ExtraParams["selectedApps"]);
            List<ApplicationDetails> appList = JSON.Deserialize<List<ApplicationDetails>>(json);
            foreach (ApplicationDetails app in appList)
            {

                using (Entities _context = new Entities())
                {
                    invoice = _context.CROSSING_APPLICATION.Where(x => x.APPLICATION_ID == app.APPLICATION_ID).SingleOrDefault();

                }

                invoice.INVOICE_ID = InvoiceId;
              
                GenericData.Update<CROSSING_APPLICATION>(invoice);
            }
            //uxFilterForm.Reset();
            //uxInvoiceFormStore.Reload();
            uxBillingReportWindow.Show();    
        }

        protected void deInvoiceReportGrid(object sender, StoreReadDataEventArgs e)
        {
            List<object> allData;
          
            string json = (e.Parameters["selectedApps"]);
            List<ApplicationDetails> appList = JSON.Deserialize<List<ApplicationDetails>>(json);
            List<long> ReportList = new List<long>();
            foreach (ApplicationDetails app in appList)
            {
                ReportList.Add(app.APPLICATION_ID);
            }
                using (Entities _context = new Entities())
                {

                    //Get List of all incidents open and closed 

                    allData = (from a in _context.CROSSING_APPLICATION
                               join d in _context.CROSSINGS on a.CROSSING_ID equals d.CROSSING_ID
                               join v in _context.CROSSING_INVOICE on a.INVOICE_ID equals v.INVOICE_ID
                               where ReportList.Contains(a.APPLICATION_ID)
                               select new
                               {
                                   v.INVOICE_NUMBER,
                                   v.INVOICE_DATE,
                                   d.CROSSING_ID,
                                   a.APPLICATION_ID,
                                   a.APPLICATION_DATE,
                                   a.APPLICATION_REQUESTED,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.MILE_POST,
                                   d.SERVICE_UNIT,

                               }).ToList<object>();

                  
                    //uxInvoiceNumber.Text = allData.INVOICE_NUMBER;
            }
                    uxInvoiceReportStore.DataSource = allData;
                   
            
        }
        protected void deGetRRType(string rrLoad)
        {

            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                var RRdata = (from r in _context.CROSSING_RAILROAD
                              where r.RAILROAD_ID == RailroadId
                              select new
                              {
                                  r

                              }).SingleOrDefault();

                uxRRCI.SetValue(RRdata.r.RAILROAD);

                string rrType = RRdata.r.RAILROAD;
                if (rrLoad == "Add")
                {
                    List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(rrType).ToList();
                    uxAddServiceUnit.Clear();
                    uxAddSubDiv.Clear();
                    uxAddServiceUnitStore.DataSource = units;
                    uxAddServiceUnitStore.DataBind();
                }

            }
        }
        protected void deLoadSubDiv(object sender, DirectEventArgs e)
        {


            if (e.ExtraParams["Type"] == "Add")
            {
                List<ServiceUnitResponse> divisions = ServiceUnitData.ServiceUnitDivisions(uxAddServiceUnit.SelectedItem.Value).ToList();
                uxAddSubDiv.Clear();
                uxAddSubDivStore.DataSource = divisions;
                uxAddSubDivStore.DataBind();
            }
        }
        protected void deCloseInvoice(object sender, DirectEventArgs e)
        {
            uxBillingReportWindow.Hide();
          
        }
        public class ApplicationDetails
        {
            public decimal INVOICE_ID { get; set; }
            public long APPLICATION_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public string APPLICATION_REQUESTED { get; set; }
            public DateTime APPLICATION_DATE { get; set; }
          
        }
        public class ReportDetails
        {
            public decimal INVOICE_ID { get; set; }
            public long APPLICATION_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public string APPLICATION_REQUESTED { get; set; }
            public DateTime APPLICATION_DATE { get; set; }

        }
    }
}
