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
using OfficeOpenXml;

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
                if (uxToggleNonSub.Checked)
                {
                    allData = allData.Where(x => x.SUB_CONTRACTED == "N");
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
            uxInvoiceFormStore.RemoveAll();

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
            // InvoiceDateTextField.Text = DateTime.Now.ToString("MM/dd/yyyy");
            // InvoiceNumTextField.Text = (data.INVOICE_NUMBER);

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
            uxInvoiceFormStore.Reload();
        
            string selectedApp = InvoiceId.ToString();
          
            string url = "/Views/Modules/CrossingMaintenance/Reports/MaintenanceInvoiceReport.aspx?selectedApp=" + selectedApp;

            Window win = new Window
            {
                ID = "uxMaintenanceInvoice",
                Height = 600,
                Width = 1120,
                Title = "Maintenance Invoice Report",
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
                Closable = true,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = url,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };
            win.Render(this.Form);
            win.Show();
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
            //uxBillingReportWindow.Hide();

        }

        //protected void deInvoiceReportViewer(object sender, StoreReadDataEventArgs e)
        //{
        //    string json = (e.Parameters["selectedApps"]);
        //    List<ApplicationDetails> appList = JSON.Deserialize<List<ApplicationDetails>>(json);

        //    List<decimal> ReportList = new List<decimal>();
        //    foreach (ApplicationDetails app in appList)
        //    {
        //        ReportList.Add(app.APPLICATION_ID);
        //    }
        //    using (Entities _context = new Entities())
        //    {
        //        IQueryable<CROSSING_MAINTENANCE.ApplicationDetails> data = CROSSING_MAINTENANCE.GetInvoiceReport(_context).Where(s => ReportList.Contains(s.APPLICATION_ID));

        //        int count;
        //        Store1.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.ApplicationDetails>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
        //        e.Total = count;

        //    }
        //}



        public class ApplicationDetails
        {
            public decimal? INVOICE_ID { get; set; }
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
        //protected void deExportSurveys(object sender, DirectEventArgs e)
        //{
        //    //DateTime StartDate = uxStartDate.SelectedDate;
        //    //DateTime EndDate = uxEndDate.SelectedDate;
        //    string _selectedRecordID = CheckboxSelectionModel2.SelectedRecordID;
        //    //List<long> OrgsList;
        //    if (_selectedRecordID != string.Empty)
        //    {
        //        // long SelectedOrgId = GetOrgFromTree(_selectedRecordID);
        //        // long HierId = GetHierarchyFromTree(_selectedRecordID);

        //        string _filename = "MaintenanceInvoiceExport.xlsx";
        //        string _filePath = Request.PhysicalApplicationPath + _filename;

        //        using (Entities _context = new Entities())
        //        {
        //            FileInfo newFile = new FileInfo(_filePath + _filename);

        //            ExcelPackage pck = new ExcelPackage(newFile);

        //           // OrgsList = HR.ActiveOrganizationsByHierarchy(HierId, SelectedOrgId, _context).Select(x => x.ORGANIZATION_ID).ToList();

        //            //List<CUSTOMER_SURVEY_FORMS> FormsList = CUSTOMER_SURVEYS.GetForms(_context).Where(x => OrgsList.Contains((long)x.ORG_ID)).ToList();
        //           var appList = _context.CROSSING_APPLICATION.Select(x => x.APPLICATION_ID).ToList();

        //             foreach (var Invoices in appList)
        //            {
        //                //string OrgName = _context.ORG_HIER_V.Where(x => x.ORG_ID == FormEntry.ORG_ID).Select(x => x.ORG_HIER).Distinct().Single();

        //                List<CROSSING_APPLICATION> results = _context.CROSSING_APPLICATION.OrderBy(x => x.APPLICATION_ID).ToList();
        //                //List<CUSTOMER_SURVEY_FORMS_COMP> Completions = CUSTOMER_SURVEYS.GetCompletionsByDate(StartDate, EndDate, FormEntry.FORM_ID, _context).ToList();
        //                ExcelWorksheet ws;
        //                if (results.Count > 0)
        //                {
        //                   // ws = pck.Workbook.Worksheets.Add(OrgName);
        //                    char letter = 'B';
        //                    int rownumber = 2;
        //                    ws.Cells["A1"].Value = "DOT #";
        //                    ws.Cells["B1"].Value = "MP";
        //                    ws.Cells["C1"].Value = "State";
        //                    ws.Cells["D1"].Value = "Application Date";
        //                    ws.Cells["E1"].Value = "Project #";

        //                    foreach (CUSTOMER_SURVEY_QUESTIONS FormQuestion in FormQuestions)
        //                    {
        //                        ws.Cells[letter + "1"].Value = FormQuestion.TEXT;
        //                        ws.Cells[letter + "1"].Style.Font.Size = 12f;
        //                        letter = GetNextLetter(letter);
        //                    }


        //                    foreach (CROSSING_APPLICATION result in results)
        //                    {
        //                        letter = 'B';
        //                        string ProjectName = _context.PROJECTS_V.Where(x => x.PROJECT_ID == result.PROJECT_ID).Select(x => x.LONG_NAME).Single();

        //                        //Get Answers
        //                        List<CROSSING_APPLICATION> Answers = _context.CROSSING_APPLICATION(result.APPLICATION_ID, _context).OrderBy(x => x.APPLICATION_DATE).ToList();
        //                        if (Answers.Count > 0)
        //                        {
        //                            ws.Cells["A" + rownumber].Value = ProjectName;
        //                        }
        //                        foreach (CUSTOMER_SURVEY_FORMS_ANS Answer in Answers)
        //                        {
        //                            ws.Cells[letter + rownumber.ToString()].Value = Answer.ANSWER;
        //                            letter = GetNextLetter(letter);
        //                        }
        //                        rownumber++;
        //                    }
        //                    ws.Column(1).AutoFit(0);
        //                }

        //            }
        //            Byte[] bin = pck.GetAsByteArray();
        //            File.WriteAllBytes(_filePath, bin);


        //            X.Msg.Confirm("File Download", "Your exported file is now ready to download.", new MessageBoxButtonsConfig
        //            {
        //                No = new MessageBoxButtonConfig
        //                {
        //                    Handler = "App.direct.Download('" + _filename + "','" + Server.UrlEncode(_filePath) + "', { isUpload : true })",
        //                    Text = "Download " + _filename
        //                }
        //            }).Show();
        //            using (FileStream fileStream = File.OpenRead(Server.UrlDecode(_filePath)))
        //            {

        //                //create new MemoryStream object
        //                MemoryStream memStream = new MemoryStream();
        //                memStream.SetLength(fileStream.Length);
        //                //read file to MemoryStream
        //                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

        //                Response.Clear();
        //                Response.ClearContent();
        //                Response.ClearHeaders();
        //                Response.ContentType = "application/octet-stream";
        //                Response.AppendHeader("Content-Disposition", "attachment;filename=" + _filename);
        //                Response.BinaryWrite(memStream.ToArray());
        //                Response.End();
        //            }
        //        }

        //    }

        //}

        //[DirectMethod]
        //public void Download(string filename, string filePath)
        //{
        //    using (FileStream fileStream = File.OpenRead(Server.UrlDecode(filePath)))
        //    {

        //        //create new MemoryStream object
        //        MemoryStream memStream = new MemoryStream();
        //        memStream.SetLength(fileStream.Length);
        //        //read file to MemoryStream
        //        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

        //        Response.Clear();
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.ContentType = "application/octet-stream";
        //        Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
        //        Response.BinaryWrite(memStream.ToArray());
        //        Response.End();
        //    }
        //}
        //protected char GetNextLetter(char letter)
        //{
        //    char nextChar;
        //    if (letter == 'z')
        //    {
        //        nextChar = 'a';
        //    }
        //    else if (letter == 'Z')
        //    {
        //        nextChar = 'A';
        //    }
        //    else
        //    {
        //        nextChar = (char)(((int)letter) + 1);
        //    }
        //    return nextChar;
        //}
    }
}
    

