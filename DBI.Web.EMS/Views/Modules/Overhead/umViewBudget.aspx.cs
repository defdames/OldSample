using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using System.IO;
using OfficeOpenXml;


namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umViewBudget : DBI.Core.Web.BasePage
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

        protected Field OnCreateFilterableField(object sender, ColumnBase column, Field defaultField)
        {
            if (column.DataIndex == "ACCOUNT_DESCRIPTION")
            {
                ((TextField)defaultField).Icon = Icon.Magnifier;
            }

            return defaultField;
        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        protected void deLoadBudgetNames(object sender, StoreReadDataEventArgs e)
        {

            string _selectedRecordID = Request.QueryString["orgid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            long _organizationID = long.Parse(_selectedID[1].ToString());
            long _leID = 0;

            if (_selectedID.Count() == 2)
            {

                _leID = _organizationID;

            }
            else
            {

                _leID = long.Parse(_selectedID[2].ToString());
            }


            uxBudgetNameStore.DataSource = OVERHEAD_BUDGET_TYPE.BudgetTypes(_leID);
            uxBudgetNameStore.DataBind();

        }

        protected void loadBudgetDetails(object sender, StoreReadDataEventArgs e)
        {

            string _selectedRecordID = Request.QueryString["orgid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            long _organizationID = long.Parse(_selectedID[1].ToString());
            long _leID = 0;

            if (_selectedID.Count() == 2)
            {

                _leID = _organizationID;

            }
            else
            {

                _leID = long.Parse(_selectedID[2].ToString());
            }
                

            using (Entities _context = new Entities())
            {
                    short _fiscal_year = short.Parse(uxFiscalYear.SelectedItem.Value);

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

                    long _budget_type_id = long.Parse(uxBudgetName.SelectedItem.Value);

                List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW> _data = new List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW>();
                OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS _printOptions = new OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS();
                _printOptions.GROUP_ACCOUNTS = uxCollapseAccountTotals.Checked;
                _printOptions.HIDE_BLANK_LINES = true;
                _printOptions.SHOW_NOTES = false;

                string _securityViewValue =  Request.QueryString["securityView"].ToString();                       

                Boolean _securityView = (_securityViewValue == "Y") ? true : false;

                _data = OVERHEAD_BUDGET_FORECAST.BudgetDetailsViewByOrganizationID(_context, _leID, _organizationID, _fiscal_year, _budget_type_id, _printOptions, _securityView, false);
                    
                int count;
                uxOrganizationAccountStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW>(e.Start, 5000, e.Sort, e.Parameters["filterheader"], _data.AsQueryable(), e.Parameters["group"], out count);
                e.Total = count;


            }


        }

        protected void deLoadData(object sender, DirectEventArgs e)
        {

            if (uxFiscalYear.SelectedItem.Value != null & uxBudgetName.SelectedItem.Value != null)
            {
                uxOrganizationAccountStore.Reload();
            }
        }
       

        protected void ExportToExcel(object sender, DirectEventArgs e)
        {

            short _fiscalYear = short.Parse(uxFiscalYear.SelectedItem.Value);
            string _selectedRecordID = Request.QueryString["orgid"];

            OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(long.Parse(uxBudgetName.SelectedItem.Value.ToString()));

            string _description = Request.QueryString["desc"] + " / " + _fiscalYear + " / " + _budgetType.BUDGET_DESCRIPTION;

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            long _organizationID = long.Parse(_selectedID[1].ToString());
            long _leID = 0;

            if (_selectedID.Count() == 2)
            {

                _leID = _organizationID;

            }
            else
            {

                _leID = long.Parse(_selectedID[2].ToString());
            }



            string _filename = _organizationID + "_" + _fiscalYear + "_budget.xlsx";
            string _filePath = Request.PhysicalApplicationPath + _filename;

            FileInfo newFile = new FileInfo(_filePath + _filename);

            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add("Export");

            using (Entities _context = new Entities())
            {
                var _glMonthPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscalYear & x.PERIOD_TYPE == "Month");
                var _glWeekPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscalYear & x.PERIOD_TYPE == "Week");

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

                List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW> _data = new List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW>();
                OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS _printOptions = new OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS();
                _printOptions.GROUP_ACCOUNTS = uxCollapseAccountTotals.Checked;
                _printOptions.HIDE_BLANK_LINES = true;
                _printOptions.SHOW_NOTES = false;

                IEnumerable<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW> _budgetView = OVERHEAD_BUDGET_FORECAST.BudgetDetailsViewByOrganizationID(_context, _leID, _organizationID, _fiscalYear, long.Parse(uxBudgetName.SelectedItem.Value.ToString()), _printOptions, false, false);

                int _cellCount = 2;
                foreach (OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW _row in _budgetView)
                {

                    ws.Cells["A" + _cellCount].Value = _row.ACCOUNT_DESCRIPTION;
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

        public void showPrintWindow(object sender, DirectEventArgs e)
        {
            uxPrintWindow.Show();
        }


        public void printOverheadBudget(object sender, DirectEventArgs e)
        {
            short _fiscalYear = short.Parse(uxFiscalYear.SelectedItem.Value);
            string _selectedRecordID = Request.QueryString["orgid"];

            OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(long.Parse(uxBudgetName.SelectedItem.Value.ToString()));

            string _description = Request.QueryString["desc"] + " / " + _fiscalYear + " / " + _budgetType.BUDGET_DESCRIPTION;

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            long _organizationID = long.Parse(_selectedID[1].ToString());
            long _leID = 0;

            if (_selectedID.Count() == 2)
            {

                _leID = _organizationID;

            }
            else
            {

                _leID = long.Parse(_selectedID[2].ToString());
            }

            using (Entities _context = new Entities())
            {

                List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW> _data = new List<OVERHEAD_BUDGET_FORECAST.OVERHEAD_BUDGET_VIEW>();
                OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS _printOptions = new OVERHEAD_BUDGET_FORECAST.PRINT_OPTIONS();
                _printOptions.GROUP_ACCOUNTS = uxCollapseAccountTotals.Checked;
                _printOptions.HIDE_BLANK_LINES = true;
                _printOptions.SHOW_NOTES = false;

                _data = OVERHEAD_BUDGET_FORECAST.BudgetDetailsViewByOrganizationID(_context, _leID, _organizationID, _fiscalYear, long.Parse(uxBudgetName.SelectedItem.Value.ToString()), _printOptions, false, true);
                    MemoryStream PdfStream = OVERHEAD_BUDGET_FORECAST.GenerateReportByOrganization(_context, _description, _fiscalYear, _data);

                    string _filename = _organizationID + "_" + _fiscalYear + "_budget.pdf";
                    string _filePath = Request.PhysicalApplicationPath + _filename;
                    FileStream fs = new FileStream(_filePath, FileMode.Create);
                    fs.Write(PdfStream.GetBuffer(), 0, PdfStream.GetBuffer().Length);
                    fs.Close();

                    string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

                    X.Js.Call("parent.App.direct.AddTabPanel", "p" + _organizationID + _fiscalYear, _description, "~/" + _filename);
             
            }

        }

    }
}