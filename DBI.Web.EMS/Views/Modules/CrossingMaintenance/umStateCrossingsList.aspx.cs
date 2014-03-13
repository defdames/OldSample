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
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umStateCrossingsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deStateCrossingListGrid(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new crossings

                data = (from d in _context.CROSSINGS
                        select new
                        {
                            d.CROSSING_ID,
                            d.CROSSING_NUMBER,
                            d.SUB_DIVISION,
                            d.STATE,
                            d.COUNTY,
                            d.CITY,
                            d.MILE_POST,
                            d.DOT,
                            d.ROWNE,
                            d.ROWNW,
                            d.ROWSE,
                            d.ROWSW,
                            d.STREET,
                            d.SUB_CONTRACTED,
                            d.LONGITUDE,
                            d.LATITUDE,
                            d.SPECIAL_INSTRUCTIONS
                        }).ToList<object>();

                int count;
                uxStateCrossingListStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
        protected List<object> GetCrossingData(long CrossingId)
        {
            using (Entities _context = new Entities())
            {
                var returnData = (from d in _context.CROSSINGS
                                  select new
                                  {
                                      d.CROSSING_ID,
                                      d.CROSSING_NUMBER,
                                      d.SUB_DIVISION,
                                      d.STATE,
                                      d.COUNTY,
                                      d.CITY,
                                      d.MILE_POST,
                                      d.DOT,
                                      d.ROWNE,
                                      d.ROWNW,
                                      d.ROWSE,
                                      d.ROWSW,
                                      d.STREET,
                                      d.SUB_CONTRACTED,
                                      d.LONGITUDE,
                                      d.LATITUDE,
                                      d.SPECIAL_INSTRUCTIONS
                                  }).ToList<object>();
                return returnData;
            }
        }
        protected void deExportToPDF(object sender, DirectEventArgs e)
        {
            //Set crossing Id
            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            MemoryStream PdfStream = generatePDF(CrossingId);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment;filename=export.pdf");
            Response.BinaryWrite(PdfStream.ToArray());
            Response.End();
        }

        protected void deSendPDF(object sender, DirectEventArgs e)
        {
            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            using (MemoryStream PdfStream = new MemoryStream(generatePDF(CrossingId).ToArray()))
            {
                string Subject = "Copy of State Crossing List Report";
                bool IsHtml = true;
                string Message = "Please find attached the State Crossing List Report you requested.";

                PdfStream.Position = 0;

                Attachment MailAttachment = new Attachment(PdfStream, "StateCrossingListExport.pdf");

                Mailer.SendMessage(User.Identity.Name + "@dbiservices.com", Subject, Message, IsHtml, MailAttachment);
            }
        }
        protected MemoryStream generatePDF(long CrossingId)
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

                //Get Header Data


                //Create Header Table
                PdfPTable HeaderTable = new PdfPTable(4);
                HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                PdfPCell[] Cells;
                PdfPRow Row;

                Paragraph Title = new Paragraph(string.Format("State Crossing List Report {0}", DateTime.Now.ToString("MM/dd/yyyy")), FontFactory.GetFont("Verdana", 12, Font.BOLD));
                Title.Alignment = 1;

                ExportedPDF.Add(Title);
                ExportedPDF.Add(NewLine);

                //First row
                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Applicator Name", HeadFootTitleFont)),
                   
                    new PdfPCell(new Phrase("Truck Number", HeadFootTitleFont))};
                try
                {

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);

                    ExportedPDF.Add(HeaderTable);
                    ExportedPDF.Add(NewLine);

                    var CrossingData = GetCrossingData(CrossingId);

                    PdfPTable CrossingTable = new PdfPTable(8);

                    Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Sub-Division", HeaderFont)),
                    new PdfPCell(new Phrase("MP", HeaderFont)),
                    new PdfPCell(new Phrase("DOT #", HeaderFont)),
                    new PdfPCell(new Phrase("State", HeaderFont)),
                    new PdfPCell(new Phrase("City", HeaderFont)),
                    new PdfPCell(new Phrase("Street", HeaderFont)),
                    new PdfPCell(new Phrase("County", HeaderFont)),
                    new PdfPCell(new Phrase("NE", HeaderFont)),
                    new PdfPCell(new Phrase("NW", HeaderFont)),
                    new PdfPCell(new Phrase("SE", HeaderFont)),
                    new PdfPCell(new Phrase("SW", HeaderFont)),
                    new PdfPCell(new Phrase("Subcontracted", HeaderFont)),
                    new PdfPCell(new Phrase("Latitude", HeaderFont)),
                    new PdfPCell(new Phrase("Longitude", HeaderFont)),
                    new PdfPCell(new Phrase("Special\nInstructions", HeaderFont))};

                    Row = new PdfPRow(Cells);
                    CrossingTable.Rows.Add(Row);

                    foreach (dynamic Data in CrossingData)
                    {
                        Cells = new PdfPCell[]{
                           new PdfPCell(new Phrase(Data.CROSSING_ID, CellFont)),
                        new PdfPCell(new Phrase(Data.SUB_DIVISION, CellFont)),
                        new PdfPCell(new Phrase(Data.MILE_POST, CellFont)),
                        new PdfPCell(new Phrase(Data.DOT, CellFont)),
                        new PdfPCell(new Phrase(Data.STATE, CellFont)),
                        new PdfPCell(new Phrase(Data.CITY, CellFont)),
                        new PdfPCell(new Phrase(Data.COUNTY, CellFont)),
                        new PdfPCell(new Phrase(Data.STREET, CellFont)),
                        new PdfPCell(new Phrase(Data.NE, CellFont)),
                        new PdfPCell(new Phrase(Data.NW, CellFont)),
                        new PdfPCell(new Phrase(Data.SE, CellFont)),
                        new PdfPCell(new Phrase(Data.SW, CellFont)),
                        new PdfPCell(new Phrase(Data.SUB_CONTRACTED, CellFont)),
                        new PdfPCell(new Phrase(Data.LATITUDE, CellFont)),
                        new PdfPCell(new Phrase(Data.LONGITUDE, CellFont)),
                        new PdfPCell(new Phrase(Data.SPECIAL_INSTRUCTIONS, CellFont)),
                     
                       
                    };
                        Row = new PdfPRow(Cells);
                        CrossingTable.Rows.Add(Row);
                    }
                    ExportedPDF.Add(CrossingTable);
                    ExportedPDF.Add(NewLine);
                }
                catch (Exception)
                { }



                //Get Footer Data
                //try
                //{
                //    var FooterData = GetFooter(HeaderId);

                //    PdfPTable FooterTable = new PdfPTable(4);
                //    FooterTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                //    string ReasonForNoWork;
                //    string Hotel;
                //    string City;
                //    string State;
                //    string Phone;

                //    try
                //    {
                //        ReasonForNoWork = FooterData.COMMENTS;
                //    }
                //    catch (NullReferenceException)
                //    {
                //        ReasonForNoWork = string.Empty;
                //    }

                //    try
                //    {
                //        Hotel = FooterData.HOTEL_NAME;
                //    }
                //    catch (NullReferenceException)
                //    {
                //        Hotel = string.Empty;
                //    }

                //    try
                //    {
                //        City = FooterData.HOTEL_CITY;
                //    }
                //    catch (NullReferenceException)
                //    {
                //        City = string.Empty;
                //    }

                //    try
                //    {
                //        State = FooterData.HOTEL_STATE;
                //    }
                //    catch (NullReferenceException)
                //    {
                //        State = string.Empty;
                //    }

                //    try
                //    {
                //        Phone = FooterData.HOTEL_PHONE;
                //    }
                //    catch (NullReferenceException)
                //    {
                //        Phone = string.Empty;
                //    }

                //    Cells = new PdfPCell[] {
                //    new PdfPCell(new Phrase("Reason for no work", HeadFootTitleFont)),
                //    new PdfPCell(new Phrase(ReasonForNoWork, HeadFootCellFont)),
                //    new PdfPCell(new Phrase("Hotel, City, State, & Phone", HeadFootTitleFont)),
                //    new PdfPCell(new Phrase(string.Format("{0} {1} {2} {3}",Hotel, City, State, Phone ), HeadFootCellFont))
                //};

                //    foreach (PdfPCell Cell in Cells)
                //    {
                //        Cell.Border = PdfPCell.NO_BORDER;
                //    }
                //    Row = new PdfPRow(Cells);
                //    FooterTable.Rows.Add(Row);

                //    iTextSharp.text.Image ForemanImage;
                //    iTextSharp.text.Image ContractImage;
                //    try
                //    {
                //        ForemanImage = iTextSharp.text.Image.GetInstance(FooterData.FOREMAN_SIGNATURE.ToArray());
                //        ForemanImage.ScaleAbsolute(75f, 25f);
                //    }
                //    catch (Exception)
                //    {
                //        ForemanImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                //    }

                //    try
                //    {
                //        ContractImage = iTextSharp.text.Image.GetInstance(FooterData.CONTRACT_REP.ToArray());
                //        ContractImage.ScaleAbsolute(75f, 25f);
                //    }
                //    catch (Exception)
                //    {
                //        ContractImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                //    }


                //    Cells = new PdfPCell[]{
                //    new PdfPCell(new Phrase("Foreman Signature", HeadFootTitleFont)),
                //    new PdfPCell(ForemanImage),
                //    new PdfPCell(new Phrase("Contract Representative", HeadFootTitleFont)),
                //    new PdfPCell(ContractImage),
                //};
                //    foreach (PdfPCell Cell in Cells)
                //    {
                //        Cell.Border = PdfPCell.NO_BORDER;
                //    }
                //    Row = new PdfPRow(Cells);
                //    FooterTable.Rows.Add(Row);
                //    if (OrgId == 123)
                //    {
                //        iTextSharp.text.Image DotRepImage;
                //        try
                //        {
                //            DotRepImage = iTextSharp.text.Image.GetInstance(FooterData.DOT_REP.ToArray());
                //            DotRepImage.ScaleAbsolute(75f, 25f);
                //        }
                //        catch (Exception)
                //        {
                //            DotRepImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                //        }

                //        Cells = new PdfPCell[]{
                //    new PdfPCell(new Phrase("DOT Representative", HeadFootTitleFont)),
                //    new PdfPCell(DotRepImage),
                //    new PdfPCell(new Phrase("Name", HeadFootTitleFont)),
                //    new PdfPCell(new Phrase(FooterData.DOT_REP_NAME, HeadFootCellFont))
                //    };
                //        foreach (PdfPCell Cell in Cells)
                //        {
                //            Cell.Border = PdfPCell.NO_BORDER;
                //        }
                //        Row = new PdfPRow(Cells);
                //        FooterTable.Rows.Add(Row);
                //    }
                //    ExportedPDF.Add(FooterTable);
                //}
                //catch (Exception)
                //{

                //}
                //Close Stream and start Download

                ExportWriter.CloseStream = false;
                ExportedPDF.Close();
                return PdfStream;
            }
        }
    }
}
        

    
