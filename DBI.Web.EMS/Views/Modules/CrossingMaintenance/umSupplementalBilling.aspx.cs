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
using System.Xml.Xsl;
using System.Xml;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umSupplementalBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
            {
                deGetRRType("Add");

            }
        }

        protected void deInvoiceSupplementalGrid(object sender, StoreReadDataEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                IQueryable<CROSSING_MAINTENANCE.CompletedCrossingsSupplemental> allData = CROSSING_MAINTENANCE.GetCompletedCrossingsSupplemental(RailroadId,  _context);
              
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
                if (ServiceUnit != null)
                {
                    allData = allData.Where(x => x.SERVICE_UNIT == ServiceUnit);
                }
                if (SubDiv != null)
                {
                    allData = allData.Where(x => x.SUB_DIVISION == SubDiv);
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

            InvoiceDateTextField.Text = DateTime.Now.ToString("MM/dd/yyyy");
            InvoiceNumTextField.Text = (data.INVOICE_SUPP_NUMBER);

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

              
                uxBillingReportWindow.Show();
            
        }
        protected void deResetInvoice(object sender, DirectEventArgs e)
        {
            uxInvoiceSupplementalStore.Reload();
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
                //IQueryable<CROSSING_MAINTENANCE.InvoicedCrossingsSupplemental> allData = CROSSING_MAINTENANCE.GetInvoicedCrossings(_context).Where(s => ReportList.Contains(s.SUPPLEMENTAL_ID));
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



                }
                uxInvoiceReportStore.DataSource = allData;
                //int count;
                //uxInvoiceSupplementalStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.InvoicedCrossingsSupplemental>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], allData, out count);
                //e.Total = count;
            
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
                    //uxAddServiceUnit.Clear();
                    //uxAddSubDiv.Clear();
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
        protected void deCloseInvoice(object sender, DirectEventArgs e)
        {
            uxBillingReportWindow.Hide();
           
        }
        protected void ToXml(object sender, EventArgs e)
        {
            string json = this.Hidden1.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;

            string strXml = xml.OuterXml;

            this.Response.Clear();
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xml");
            this.Response.AddHeader("Content-Length", strXml.Length.ToString());
            this.Response.ContentType = "application/xml";
            this.Response.Write(strXml);
            this.Response.End();
        }

        protected void ToExcel(object sender, EventArgs e)
        {
            string json = this.Hidden1.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");

            XslCompiledTransform xtExcel = new XslCompiledTransform();

            xtExcel.Load(Server.MapPath("Excel.xsl"));
            xtExcel.Transform(xml, null, this.Response.OutputStream);
            this.Response.End();
        }

        protected void ToCsv(object sender, EventArgs e)
        {
            string json = this.Hidden1.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/octet-stream";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.csv");

            XslCompiledTransform xtCsv = new XslCompiledTransform();

            xtCsv.Load(Server.MapPath("Csv.xsl"));
            xtCsv.Transform(xml, null, this.Response.OutputStream);
            this.Response.End();
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

