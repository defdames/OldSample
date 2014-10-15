using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MessagingToolkit.Barcode;
using MessagingToolkit.Barcode.Common;

namespace DBI.Web.EMS
{
    public partial class QRCodeGenerator : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                BarcodeEncoder barcodeEncoder = new BarcodeEncoder();

                using (Entities _context = new Entities())
                {
                    var equipment = _context.PROJECTS_V.Where(x => x.PROJECT_TYPE == "TRUCK & EQUIPMENT" & x.PROJECT_STATUS_CODE == "APPROVED").Select(x => new { SEGMENT1 = x.SEGMENT1, NAME = x.NAME }).ToList();

                    iTextSharp.text.Rectangle pgSize = new Rectangle(144f, 72f);
                    Document ExportedPDF = new Document(pgSize, 0f, 0f, 0f, 0f);
                    string _filePath = Request.PhysicalApplicationPath;
                    PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, new FileStream(_filePath + "report.pdf", FileMode.Create));
                    Font cellFont = FontFactory.GetFont("Verdana", 10, Font.BOLD);
                    ExportedPDF.Open();

                    PdfPTable _headerPdfTable = new PdfPTable(2);
                    _headerPdfTable.WidthPercentage = 100;
                    int[] intTblWidth = { 75, 25 };
                    _headerPdfTable.SetWidths(intTblWidth);
                    PdfPCell[] Cells;
                    PdfPRow Row;

                    foreach (var equip in equipment)
                    {

                        System.Drawing.Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, equip.SEGMENT1);
                        iTextSharp.text.Image QRCodeImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Png);
                        QRCodeImage.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                        QRCodeImage.ScaleToFit(140f, 70f);
                        QRCodeImage.SetDpi(300, 300);

                        PdfPCell imageCell = new PdfPCell(QRCodeImage);

                        Cells = new PdfPCell[]{
                     new PdfPCell(imageCell),
                      new PdfPCell(new Phrase(equip.NAME, cellFont ))
                    };

                        int cellCount = 0;

                        foreach (PdfPCell _cell in Cells)
                        {
                            if (cellCount == 0)
                            {
                                _cell.HorizontalAlignment = PdfCell.ALIGN_CENTER;
                                _cell.Border = Rectangle.NO_BORDER;
                            }
                            else
                            {
                                _cell.HorizontalAlignment = PdfCell.ALIGN_CENTER;
                                _cell.Border = Rectangle.NO_BORDER;
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
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
    }
}