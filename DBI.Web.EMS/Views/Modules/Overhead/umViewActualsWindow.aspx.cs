using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using OfficeOpenXml;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umViewActualsWindow : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        protected void uxDetailStore_ReadData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
                long _accountID = long.Parse(Request.QueryString["accountID"]);
                
                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date, 0 as PERIOD_DR, 0 as PERIOD_CR, 0 as PERIOD_TOTAL from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                string sql = string.Format("select period_net_dr, period_net_cr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, _accountID);
                    List<ACTUAL_BALANCES> _balance = _context.Database.SqlQuery<ACTUAL_BALANCES>(sql).ToList();

                foreach(GL_PERIODS _period in _periodMonthList)
                {
                    ACTUAL_BALANCES _actualTotalLine = _balance.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                    if (_actualTotalLine != null)
                    {
                        _period.PERIOD_DR = _actualTotalLine.PERIOD_NET_DR;
                        _period.PERIOD_CR = _actualTotalLine.PERIOD_NET_CR;
                        _period.PERIOD_TOTAL = _period.PERIOD_DR + Decimal.Negate(_period.PERIOD_CR);
                    }
                    else
                    {
                        _period.PERIOD_DR = 0;
                        _period.PERIOD_CR = 0;
                        _period.PERIOD_TOTAL = 0;
                    }
                }

                int count;
                uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<GL_PERIODS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _periodMonthList, out count);
                e.Total = count;
            }
        }

        protected void ExportToExcel(object sender, DirectEventArgs e)
        {

            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _accountID = long.Parse(Request.QueryString["accountID"]);

            string _filename = _accountID + "_" + _fiscal_year + "_actuals.xlsx";
            string _filePath = Request.PhysicalApplicationPath + _filename;

            FileInfo newFile = new FileInfo(_filePath + _filename);

            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add("Export");

            using (Entities _context = new Entities())
            {
                ws.Cells["A1"].Value = "Account Description";
                ws.Cells["B1"].Value = "Fiscal Year";
                ws.Cells["C1"].Value = "Entered Period Name";
                ws.Cells["D1"].Value = "Transaction Date";
                ws.Cells["E1"].Value = "Posted Date";
                ws.Cells["F1"].Value = "Line Reference";
                ws.Cells["G1"].Value = "Line Description";
                ws.Cells["H1"].Value = "Debit";
                ws.Cells["I1"].Value = "Credit";
                ws.Cells["J1"].Value = "Total";

                string _accountDescription = Request.QueryString["description"];


                string sql2 = string.Format("select ROW_ID as ROW_ID, Line_reference_1 as LINE_REFERENCE, Line_description as LINE_DESCRIPTION, nvl(line_entered_dr,0) AS DEBIT, nvl(line_entered_cr,0) AS CREDIT, je_category AS CATEGORY, header_effective_date AS TRANSACTION_DATE,header_posted_date as POSTED_DATE, 0 as TOTAL, PERIOD_NAME from APPS.GL_JE_JOURNAL_LINES_V where period_year = {0} and period_num = {1} and line_code_combination_id = {2} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, uxPeriodSelectionModel.SelectedRow.RecordID, _accountID);
                List<BALANCE_DETAILS> _details = _context.Database.SqlQuery<BALANCE_DETAILS>(sql2).ToList();

                int _cellCount = 2;
                foreach (BALANCE_DETAILS _detail in _details)
                {

                    ws.Cells["A" + _cellCount].Value = _accountDescription;
                    ws.Cells["B" + _cellCount].Value = _fiscal_year;
                    ws.Cells["C" + _cellCount].Value = _detail.PERIOD_NAME;
                    ws.Cells["D" + _cellCount].Formula = "=DATE(" + _detail.TRANSACTION_DATE.Year.ToString() + "," + _detail.TRANSACTION_DATE.Month.ToString() + "," + _detail.TRANSACTION_DATE.Day.ToString() + ")";
                    ws.Cells["D" + _cellCount].Style.Numberformat.Format = "DD/MM/YYYY";
                    if (_detail.POSTED_DATE != null)
                    {
                        DateTime _convertedDate = (DateTime)_detail.POSTED_DATE;
                        ws.Cells["E" + _cellCount].Formula = "=DATE(" + _convertedDate.Year.ToString() + "," + _convertedDate.Month.ToString() + "," + _convertedDate.Day.ToString() + ")";
                    }
                    ws.Cells["E" + _cellCount].Style.Numberformat.Format = "DD/MM/YYYY";
                    ws.Cells["F" + _cellCount].Value = _detail.LINE_REFERENCE;
                    ws.Cells["G" + _cellCount].Value = _detail.LINE_DESCRIPTION;
                    ws.Cells["H" + _cellCount].Value = _detail.DEBIT;
                    ws.Cells["H" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["I" + _cellCount].Value = _detail.CREDIT;
                    ws.Cells["I" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["J" + _cellCount].Value = (_detail.DEBIT + Decimal.Negate(_detail.CREDIT));
                    ws.Cells["J" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    _cellCount = _cellCount + 1;
                }
                                
            }

            Byte[] bin = pck.GetAsByteArray();
            File.WriteAllBytes(_filePath, bin);

            X.Msg.Confirm("File Download", "Your exported file is now ready to download.", new MessageBoxButtonsConfig
            {
                No = new MessageBoxButtonConfig
                {
                    Handler = "App.direct.Download('" + _filename + "','" + Server.UrlEncode(_filePath) + "', { isUpload : true })",
                    Text = "Download " + _filename
                }
            }).Show();

        }

        [DirectMethod]
        public void Download(string filename, string filePath)
        {
            using (FileStream fileStream = File.OpenRead(Server.UrlDecode(filePath)))
            {

                //create new MemoryStream object
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                //read file to MemoryStream
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.BinaryWrite(memStream.ToArray());
                Response.End();
            }
        }

        protected void deLoadDetails(object sender, DirectEventArgs e)
        {
            Store1.Reload();
        }

        public class BALANCE_DETAILS
        {
            public string ROW_ID { get; set; }
            public string LINE_REFERENCE { get; set; }
            public string LINE_DESCRIPTION { get; set; }
            public decimal DEBIT { get; set; }
            public decimal CREDIT { get; set; }
            public string CATEGORY { get; set; }
            public DateTime TRANSACTION_DATE { get; set; }
            public DateTime? POSTED_DATE { get; set; }
            public decimal TOTAL { get; set; }
            public string PERIOD_NAME { get; set; }
        }

        public class ACTUAL_BALANCES
        {
            public decimal PERIOD_NET_DR { get; set; }
            public decimal PERIOD_NET_CR { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public long PERIOD_NUM { get; set; }
        }

        public class GL_PERIODS
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime? END_DATE { get; set; }
            public Decimal PERIOD_DR { get; set; }
            public Decimal PERIOD_CR { get; set; }
            public Decimal PERIOD_TOTAL { get; set; }
        }

        protected void Store1_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                try
                {
                    short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
                    long _accountID = long.Parse(Request.QueryString["accountID"]);
                    string sql2 = string.Format("select ROW_ID as ROW_ID, Line_reference_1 as LINE_REFERENCE, Line_description as LINE_DESCRIPTION, nvl(line_entered_dr,0) AS DEBIT, nvl(line_entered_cr,0) AS CREDIT, je_category AS CATEGORY, header_effective_date AS TRANSACTION_DATE,header_posted_date as POSTED_DATE, 0 as TOTAL, PERIOD_NAME from APPS.GL_JE_JOURNAL_LINES_V where period_year = {0} and period_num = {1} and line_code_combination_id = {2} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, uxPeriodSelectionModel.SelectedRow.RecordID, _accountID);
                    List<BALANCE_DETAILS> _details = _context.Database.SqlQuery<BALANCE_DETAILS>(sql2).ToList();

                    foreach (BALANCE_DETAILS _detail in _details)
                    {
                        _detail.TOTAL = (_detail.DEBIT + Decimal.Negate(_detail.CREDIT));
                    }

                    int count;
                    Store1.DataSource = GenericData.EnumerableFilterHeader<BALANCE_DETAILS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _details, out count);
                    e.Total = count;

                    GridPanel1.Enable();
                }
                catch (Exception ex)
                {
                    X.Msg.Alert("Error", ex.ToString()).Show();
                }
               
            }
        }


        protected void Store1_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.FormatType.Value.ToString();

            XmlNode xml = e.Xml;

            this.Response.Clear();

            switch (format)
            {
                case "xml":
                    string strXml = xml.OuterXml;
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xml");
                    this.Response.AddHeader("Content-Length", strXml.Length.ToString());
                    this.Response.ContentType = "application/xml";
                    this.Response.Write(strXml);
                    break;

                case "xls":
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "csv":
                    this.Response.ContentType = "application/octet-stream";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.csv");
                    XslCompiledTransform xtCsv = new XslCompiledTransform();
                    xtCsv.Load(Server.MapPath("Csv.xsl"));
                    xtCsv.Transform(xml, null, Response.OutputStream);

                    break;
            }
            this.Response.End();
        }
    }
}