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

namespace DBI.Web.EMS
{
    public partial class QRCodeGenerator : System.Web.UI.Page
    {

        

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                BarcodeEncoder barcodeEncoder = new BarcodeEncoder();
                List<PROJECTS_V> equipment = new List<PROJECTS_V>();

                using(Entities _context = new Entities())
                {
                    equipment = _context.PROJECTS_V.Where(x => x.PROJECT_TYPE == "TRUCK & EQUIPMENT" && x.PROJECT_ID == 94).ToList();
                }

                    iTextSharp.text.Rectangle pgSize = new Rectangle(144f,72f);
                    Document ExportedPDF = new Document(pgSize, 0f, 0f, 0f, 0f);
                    PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, new FileStream("c:\\temp\\truck.pdf",FileMode.Create));
                    Paragraph NewLine = new Paragraph("\n");
                    Font cellFont = FontFactory.GetFont("Verdana", 6);
                    ExportedPDF.Open();

                    foreach (var equip in equipment)
                    {
                        System.Drawing.Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, equip.NAME);
                        iTextSharp.text.Image QRCodeImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Png);
                        QRCodeImage.Alignment = 1;
                        ExportedPDF.Add(QRCodeImage);

                        Paragraph Title = new Paragraph(equip.NAME, cellFont);
                        Title.Alignment = 1;
                        ExportedPDF.Add(Title);
                        ExportedPDF.NewPage();
                    }

                    ExportedPDF.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
    }
}