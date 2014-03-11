using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;
using DBI.Mobile.EMS.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace DBI.Mobile.EMS.Controllers
{
    public class ProductionController : ApiController
    {
        [Authorize]
        [HttpPost]
        public void Post(HttpRequestMessage req)
        {
            string jsonString = req.Content.ReadAsStringAsync().Result;

            var jsonObj = new DailyActivityResponse.RootObject();
            jsonObj = JsonConvert.DeserializeObject<DailyActivityResponse.RootObject>(jsonString);

            MemoryStream objectPDF = generateProductionPDF(jsonObj);

        }

        protected MemoryStream generateProductionPDF(DailyActivityResponse.RootObject response)
        {

            using (MemoryStream PdfStream = new MemoryStream())
            {
                //Create the document
                Document ExportedPDF = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 42f, 42f);
                PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, PdfStream);
                Paragraph NewLine = new Paragraph("\n");
                Font HeaderFont = FontFactory.GetFont("Verdana", 6, Font.BOLD);
                Font HeadFootTitleFont = FontFactory.GetFont("Verdana", 7, Font.BOLD);
                Font HeadFootCellFont = FontFactory.GetFont("Verdana", 7);
                Font CellFont = FontFactory.GetFont("Verdana", 6);
                //Open Document
                ExportedPDF.Open();


                //Create Header Table
                PdfPTable HeaderTable = new PdfPTable(4);
                HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                PdfPCell[] Cells;
                PdfPRow Row;

                DailyActivityResponse.DailyActivityHeader header = response.daily_activity_header.SingleOrDefault();

                using(Entities _context = new Entities())
                {
                    var data = from p in _context.PROJECTS_V
                               where p.PROJECT_ID == header.project_id
                               select new { p.SEGMENT1, p.LONG_NAME };

                Paragraph Title = new Paragraph(string.Format("DAILY WORK LOG {0}", header.da_date), FontFactory.GetFont("Verdana", 12, Font.BOLD));
                Paragraph SubTitle = new Paragraph(string.Format("{0} - {1}", data.SingleOrDefault().SEGMENT1, data.SingleOrDefault().LONG_NAME), FontFactory.GetFont("Verdana", 12, Font.BOLD));
                Title.Alignment = 1;
                SubTitle.Alignment = 1;

                ExportedPDF.Add(Title);
                ExportedPDF.Add(SubTitle);

                }
                
              
                //Close Stream and start Download
                ExportWriter.CloseStream = false;
                ExportedPDF.Close();
                return PdfStream;
            }
        }
    }
}
