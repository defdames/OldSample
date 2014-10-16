using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core;
using DBI.Data;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MessagingToolkit.Barcode;
using MessagingToolkit.Barcode.Common;

namespace DBI.Web.EMS.Views.Modules.Fleet
{
    public partial class umViewEquipmentList : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void uxOrganizationEquipmentListStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            long _organizationID = long.Parse(_selectedID[1].ToString());

            //First get a list of organizations
            List<string> _organizationList = HR.OrganizationsByHierarchy(_hierarchyID, _organizationID).Select(x =>x.ORGANIZATION_ID.ToString()).ToList();

            if (_organizationList.Count == 0)
                _organizationList.Add(_organizationID.ToString());

            using (Entities _context = new Entities())
            {
                List<PROJECTS_V> _data = (from _dbData in _context.PROJECTS_V
                                          where (_dbData.PROJECT_STATUS_CODE == "APPROVED" & _dbData.PROJECT_TYPE == "TRUCK & EQUIPMENT")
                                                select _dbData).ToList();

                List<PROJECTS_V> _returnData = new List<PROJECTS_V>();

                foreach (PROJECTS_V _project in _data)
                {
                    if (_organizationList.Contains(_project.CARRYING_OUT_ORGANIZATION_ID.ToString()))
                        _returnData.Add(_project);
                }

                int count;
                uxOrganizationEquipmentListStore.DataSource = GenericData.EnumerableFilterHeader<PROJECTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _returnData, out count);
                e.Total = count;
            }
        }

        public void dePrintLabels(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["orgid"];
            BarcodeEncoder barcodeEncoder = new BarcodeEncoder();

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[0].ToString());
            long _organizationID = long.Parse(_selectedID[1].ToString());

            //First get a list of organizations
            List<string> _organizationList = HR.OrganizationsByHierarchy(_hierarchyID, _organizationID).Select(x => x.ORGANIZATION_ID.ToString()).ToList();

            if (_organizationList.Count == 0)
                _organizationList.Add(_organizationID.ToString());

            using (Entities _context = new Entities())
            {
                List<PROJECTS_V> _data = (from _dbData in _context.PROJECTS_V
                                          where (_dbData.PROJECT_STATUS_CODE == "APPROVED" & _dbData.PROJECT_TYPE == "TRUCK & EQUIPMENT")
                                          select _dbData).ToList();

                List<PROJECTS_V> _returnData = new List<PROJECTS_V>();

                foreach (PROJECTS_V _project in _data)
                {
                    if (_organizationList.Contains(_project.CARRYING_OUT_ORGANIZATION_ID.ToString()))
                    {
                            _returnData.Add(_project);
                    }
                }

                 RowSelectionModel sm = uxEquipmentSelection;
                 if (sm.SelectedRows.Count() > 0)
                 {
                     List<string> _selected = sm.SelectedRows.ToList().Select(x => x.RecordID).ToList();
                     _returnData = _returnData.Where(x => _selected.Contains(x.PROJECT_ID.ToString())).ToList();
                 }

                iTextSharp.text.Rectangle pgSize = new iTextSharp.text.Rectangle(144f, 72f);
                Document ExportedPDF = new Document(pgSize, 0f, 0f, 0f, 0f);
                string _filename = _organizationID + "_report.pdf";
                string _filePath = Request.PhysicalApplicationPath + _filename;
                PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, new FileStream(_filePath, FileMode.Create));
                iTextSharp.text.Font cellFont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Font.BOLD);
                ExportedPDF.Open();

                PdfPTable _headerPdfTable = new PdfPTable(2);
                _headerPdfTable.WidthPercentage = 100;
                int[] intTblWidth = { 75, 25 };
                _headerPdfTable.SetWidths(intTblWidth);
                PdfPCell[] Cells;
                PdfPRow Row;

                foreach (var equip in _returnData)
                {

                    System.Drawing.Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, equip.SEGMENT1);
                    iTextSharp.text.Image QRCodeImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Png);
                    QRCodeImage.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                    QRCodeImage.ScaleToFit(140f, 70f);
                    QRCodeImage.SetDpi(300, 300);

                    PdfPCell imageCell = new PdfPCell(QRCodeImage);

                    Phrase _equipmentName = new Phrase();
                    _equipmentName.Add(new Chunk(equip.NAME, cellFont));
                    _equipmentName.Add(new Chunk("\n"));
                    _equipmentName.Add(new Chunk(equip.DESCRIPTION, cellFont));

                    Cells = new PdfPCell[]{
                     new PdfPCell(imageCell),
                      new PdfPCell(new Phrase(_equipmentName))
                    };

                    int cellCount = 0;

                    foreach (PdfPCell _cell in Cells)
                    {
                        if (cellCount == 0)
                        {
                            _cell.HorizontalAlignment = PdfCell.ALIGN_CENTER;
                            _cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        }
                        else
                        {
                            _cell.HorizontalAlignment = PdfCell.ALIGN_CENTER;
                            _cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            _cell.Rotation = 270;
                        }
                        cellCount = cellCount + 1;
                        _cell.UseAscender = true;

                    }

                    Row = new PdfPRow(Cells);
                    _headerPdfTable.Rows.Add(Row);
                }

                ExportedPDF.Add(_headerPdfTable);
                ExportedPDF.Close();

                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

                string _description = Request.QueryString["desc"];

                X.Js.Call("parent.App.direct.AddTabPanel", "p" + _organizationID, _description, "~/" + _filename);

            }
        }
    }
}