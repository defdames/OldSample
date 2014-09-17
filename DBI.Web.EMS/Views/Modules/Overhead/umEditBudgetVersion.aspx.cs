using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;
using System.IO;
using OfficeOpenXml;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umEditBudgetVersion : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                using (Entities _context = new Entities())
                {

                    long _organizationID;
                    bool checkOrgId = long.TryParse(Request.QueryString["orgid"], out _organizationID);
                    short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);

                        var _glMonthPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month");
                        var _glWeekPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Week");

                        foreach (OVERHEAD_BUDGET_FORECAST.GL_PERIOD _period in _glMonthPeriods)
                        {
                            if (_period.PERIOD_NUM == 1)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column1.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 2)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column2.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 3)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column3.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 4)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column4.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 5)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column5.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 6)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column6.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 7)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column7.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 8)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column8.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 9)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column9.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 10)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column10.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 11)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column11.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                            if (_period.PERIOD_NUM == 12)
                            {
                                var _weekCount = _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                                Column12.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                            }
                    }
                }

            }
        }



        protected void editBudgetNotes(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {

                long _budgetid = long.Parse(Request.QueryString["budget_id"]);

                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetid);
                uxBudgetComments.Text = _budgetDetail.COMMENTS;

                //pull budget detail data
                var _detail = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetid);

                if (_budgetDetail.STATUS == "C")
                {
                    uxSaveBudgetNote.Disable();
                }

            
            }
            uxBudgetNotesWindow.Center();
            uxBudgetNotesWindow.Show();



        }

        protected Field OnCreateFilterableField(object sender, ColumnBase column, Field defaultField)
        {
            if (column.DataIndex == "ACCOUNT_DESCRIPTION")
            {
                ((TextField)defaultField).Icon = Icon.Magnifier;
            }

            return defaultField;
        }

        protected void viewActuals(object sender, DirectEventArgs e)
        {


            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);
            string _accountDescription = e.ExtraParams["ACCOUNT_DESCRIPTION"];
            string _AccountSelectedID = uxOrganizationAccountSelectionModel.SelectedRow.RecordID;

            string url = "umViewActualsWindow.aspx?budget_id=" + _budgetid + "&orgid=" + _organizationID + "&fiscalyear=" + _fiscal_year + "&accountID=" + _AccountSelectedID;

            Window win = new Window
            {
                ID = "uxViewActualsWn",
                Title = "View Account Actuals - " + _accountDescription + " / Fiscal Year " + _fiscal_year,
                Height = 550,
                Width = 900,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
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

            //win.Listeners.Close.Handler = "#{uxOrganizationAccountGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();


        }

        protected void importActuals(object sender, DirectEventArgs e)
        {


            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);

            string url = "umImportActualsWindow.aspx?budget_id=" + _budgetid + "&orgid=" + _organizationID + "&fiscalyear=" + _fiscal_year;

            Window win = new Window
            {
                ID = "uxImportActualsWn",
                Title = "Import Actuals For Selected Periods",
                Height = 400,
                Width = 800,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
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

            //win.Listeners.Close.Handler = "#{uxOrganizationAccountGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();


        }

        protected void saveBudgetNotes(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {

                long _budgetid = long.Parse(Request.QueryString["budget_id"]);

                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetid);
                _budgetDetail.COMMENTS = uxBudgetComments.Text;
                GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budgetDetail);
            }
            uxBudgetNotesWindow.Close();

        }

        protected void loadBudgetDetails(object sender, StoreReadDataEventArgs e)
        {

            long _organizationID;
            bool checkOrgId = long.TryParse(Request.QueryString["orgid"], out _organizationID);
            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);


            using (Entities _context = new Entities())
            {
                var _validAccounts = OVERHEAD_BUDGET_FORECAST.AccountListValidByOrganizationID(_context, _organizationID);

                //pull budget detail data
                var _budgetDetail = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetid);

                if (_budgetDetail.STATUS == "C")
                {
                    uxImportActualsButton.Disable();
                }

                List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW> _data = new List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW>();
   
                            //Check toggle button if button is active, hide zero lines (zero total)
                    if (uxHideBlankLinesCheckbox.Checked)
                    {
                         _data = OVERHEAD_BUDGET_FORECAST.BudgetDetailsViewByBudgetID(_context, _budgetid,false,true);
                    }
                    else
                    {
                       _data = OVERHEAD_BUDGET_FORECAST.BudgetDetailsViewByBudgetID(_context, _budgetid);
                    }

                int count;
                uxOrganizationAccountStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW>(e.Start, 5000, e.Sort, e.Parameters["filterheader"], _data.AsQueryable(), e.Parameters["group"], out count);
                e.Total = count;


            }


        }

        protected void deHideBlankLines(object sender, DirectEventArgs e)
        {
            Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

            Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
            _p.Mode = ParameterMode.Value;
            _p.Name = "TOGGLE_ACTIVE";
            _p.Value = "N";

            if (uxHideBlankLinesCheckbox.Checked)
                _p.Value = "Y";

            ps.Add(_p);

            uxOrganizationAccountStore.Reload(ps);

        }


        protected void deItemMaintenance(object sender, DirectEventArgs e)
        {
            string _budgetSelectedID = Request.QueryString["budget_id"];
            string _AccountSelectedID = uxOrganizationAccountSelectionModel.SelectedRow.RecordID;
            string _fiscal_year = Request.QueryString["fiscalyear"];
            string _accountDescription = e.ExtraParams["ACCOUNT_DESCRIPTION"];

            long _budgetID = long.Parse(_budgetSelectedID);


            using (Entities _context = new Entities())
            {
                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = OVERHEAD_BUDGET_FORECAST.BudgetByID(_context, _budgetID);

                if (_budgetDetail.STATUS == "C")
                {
                    return;
                }

            }
            string url = "umAddOverheadDetailLine.aspx?budgetID=" + _budgetSelectedID + "&accountID=" + _AccountSelectedID + "&fiscalyear=" + _fiscal_year;

            Window win = new Window
            {
                ID = "uxDetailLineMaintenance",
                Title = "Account Details - " + _accountDescription,
                Height = 465,
                Width = 850,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
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

            //win.Listeners.Close.Handler = "#{uxOrganizationAccountGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();

        }

        protected void deExportData(object sender, StoreSubmitDataEventArgs e)
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


        protected void ExportToExcel(object sender, DirectEventArgs e)
        {

            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);
            string _description = Request.QueryString["description"];

            string _filename = _organizationID + "_" + _fiscal_year + "_budget.xlsx";
            string _filePath = Request.PhysicalApplicationPath + _filename;

            FileInfo newFile = new FileInfo(_filePath + _filename);

            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add("Export");

            using (Entities _context = new Entities())
            {
                var _glMonthPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month");
                var _glWeekPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Week");

                ws.Cells["A1"].Value = "Account Name";
                ws.Cells["B1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 1).Single().ENTERED_PERIOD_NAME);
                ws.Cells["C1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 2).Single().ENTERED_PERIOD_NAME);
                ws.Cells["D1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 3).Single().ENTERED_PERIOD_NAME);
                ws.Cells["E1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 4).Single().ENTERED_PERIOD_NAME);
                ws.Cells["F1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 5).Single().ENTERED_PERIOD_NAME);
                ws.Cells["G1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 6).Single().ENTERED_PERIOD_NAME);
                ws.Cells["H1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 7).Single().ENTERED_PERIOD_NAME);
                ws.Cells["I1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 8).Single().ENTERED_PERIOD_NAME);
                ws.Cells["J1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 9).Single().ENTERED_PERIOD_NAME);
                ws.Cells["K1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 10).Single().ENTERED_PERIOD_NAME);
                ws.Cells["L1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 11).Single().ENTERED_PERIOD_NAME);
                ws.Cells["M1"].Value = string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 12).Single().ENTERED_PERIOD_NAME);
                ws.Cells["N1"].Value = "Total";
                //ws.Cells["E2"].Value = new decimal(98222.50);
                //ws.Cells["E2"].Style.Numberformat.Format = "#,##0.00";

                OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS _printOptions = new OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS();
                _printOptions.HIDE_BLANK_LINES = uxHideBlankLinesCheckbox.Checked;

                IEnumerable<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW> _budgetView = OVERHEAD_BUDGET_FORECAST.BudgetDetailsViewByBudgetID(_context, _budgetid, true, _printOptions.HIDE_BLANK_LINES);

                int _cellCount = 2;
                foreach (OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW _row in _budgetView)
                {

                    ws.Cells["A" + _cellCount].Value = _row.ACCOUNT_DESCRIPTION + " - " + _row.ACCOUNT_DESCRIPTION2;
                    ws.Cells["B" + _cellCount].Value = _row.AMOUNT1;
                    ws.Cells["B" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["C" + _cellCount].Value = _row.AMOUNT2;
                    ws.Cells["c" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["D" + _cellCount].Value = _row.AMOUNT3;
                    ws.Cells["D" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["E" + _cellCount].Value = _row.AMOUNT4;
                    ws.Cells["E" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["F" + _cellCount].Value = _row.AMOUNT5;
                    ws.Cells["F" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["G" + _cellCount].Value = _row.AMOUNT6;
                    ws.Cells["G" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["H" + _cellCount].Value = _row.AMOUNT7;
                    ws.Cells["H" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["I" + _cellCount].Value = _row.AMOUNT8;
                    ws.Cells["I" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["J" + _cellCount].Value = _row.AMOUNT9;
                    ws.Cells["J" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["K" + _cellCount].Value = _row.AMOUNT10;
                    ws.Cells["K" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["L" + _cellCount].Value = _row.AMOUNT11;
                    ws.Cells["L" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["M" + _cellCount].Value = _row.AMOUNT12;
                    ws.Cells["M" + _cellCount].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells["N" + _cellCount].Value = _row.TOTAL;
                    ws.Cells["N" + _cellCount].Style.Numberformat.Format = "#,##0.00";
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


        public void printOverheadBudget(object sender, DirectEventArgs e)
        {

            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);
            string _description = Request.QueryString["description"];

            using(Entities _context = new Entities())
            {

                OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS _printOptions = new OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS();
                _printOptions.HIDE_BLANK_LINES = uxHideBlankLinesCheckbox.Checked;

                MemoryStream PdfStream = OVERHEAD_BUDGET_FORECAST.GenerateReport(_context, _organizationID, _fiscal_year, _budgetid, _description, _printOptions);

                
                string _filename = _organizationID + "_" + _fiscal_year + "_budget.pdf";
                string _filePath = Request.PhysicalApplicationPath + _filename;
                FileStream fs = new FileStream(_filePath,FileMode.Create);
                fs.Write(PdfStream.GetBuffer(), 0, PdfStream.GetBuffer().Length);
                fs.Close();

                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

                X.Js.Call("parent.App.direct.AddTabPanel", "p" + _organizationID + _fiscal_year + _budgetid, _description, "~/" + _filename);
            }

        }

        public class OVERHEAD_BUDGET_DETAIL_V
        {
            public long CATEGORY_ID { get; set; }
            public long CATEGORY_SORT_ORDER { get; set; }
            public long? SORT_ORDER { get; set; }
            public string ACCOUNT_SEGMENT { get; set; }
            public string CATEGORY_NAME { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public string ACCOUNT_DESCRIPTION { get; set; }
            public decimal TOTAL { get; set; }
            public decimal AMOUNT1 { get; set; }
            public decimal AMOUNT2 { get; set; }
            public decimal AMOUNT3 { get; set; }
            public decimal AMOUNT4 { get; set; }
            public decimal AMOUNT5 { get; set; }
            public decimal AMOUNT6 { get; set; }
            public decimal AMOUNT7 { get; set; }
            public decimal AMOUNT8 { get; set; }
            public decimal AMOUNT9 { get; set; }
            public decimal AMOUNT10 { get; set; }
            public decimal AMOUNT11 { get; set; }
            public decimal AMOUNT12 { get; set; }
        }

        public class GL_PERIOD_LIST
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime END_DATE { get; set; }
        }

        public class ACTUAL_BALANCES
        {
            public decimal PERIOD_NET_DR { get; set; }
            public long PERIOD_YEAR { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public long PERIOD_NUM { get; set; }
        }
    }
}


