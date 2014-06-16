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
                List<string> equipment = new List<string>();

                using (Entities _context = new Entities())
                {
                    equipment = _context.PROJECTS_V.Where(x => x.PROJECT_TYPE == "TRUCK & EQUIPMENT").Select(x => x.NAME).Take(100).ToList();
                }

                iTextSharp.text.Rectangle pgSize = new Rectangle(144f, 72f);
                Document ExportedPDF = new Document(pgSize, 0f, 0f, 0f, 0f);
                PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, new FileStream("c:\\temp\\truck.pdf", FileMode.Create));
                Font cellFont = FontFactory.GetFont("Verdana", 5);
                ExportedPDF.Open();

                foreach (var equip in equipment)
                {
                    System.Drawing.Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, equip);
                    iTextSharp.text.Image QRCodeImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                    QRCodeImage.Alignment = 1;
                    QRCodeImage.ScaleToFit(100f, 100f);
                    ExportedPDF.Add(QRCodeImage);

                    Paragraph Title = new Paragraph(equip, cellFont);
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