using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umManageExisting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
        }

        /// <summary>
        /// Gets filterable list of header data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadHeaderData(object sender, StoreReadDataEventArgs e)
        {
            
            using (Entities _context = new Entities())
            {
                List<object> data;
            
                //Get List of all new headers
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                            select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE }).ToList<object>();
                
                int count;
                uxManageGridStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }

        /// <summary>
        /// Update Tab URLs based on selected header and activate buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deUpdateUrlAndButtons(object sender, DirectEventArgs e)
        {
            string homeUrl = string.Format("umCombinedTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string headerUrl = string.Format("umHeaderTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string equipUrl = string.Format("umEquipmentTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string prodUrl = string.Format("umProductionTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string emplUrl = string.Format("umEmployeesTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string chemUrl = string.Format("umChemicalTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string weatherUrl = string.Format("umWeatherTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
            string invUrl = string.Format("umInventoryTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);

            uxCombinedTab.LoadContent(homeUrl);
            uxHeaderTab.LoadContent(headerUrl);
            uxEquipmentTab.LoadContent(equipUrl);
            uxProductionTab.LoadContent(prodUrl);
            uxEmployeeTab.LoadContent(emplUrl);
            uxChemicalTab.LoadContent(chemUrl);
            uxWeatherTab.LoadContent(weatherUrl);
            uxInventoryTab.LoadContent(invUrl);

            uxApproveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
            uxPostActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Post");
            uxInactiveActivityButton.Disabled = false;
            uxSubmitActivityButton.Disabled = false;
        }

        protected void deDeselectHeader(object sender, DirectEventArgs e)
        {
            uxApproveActivityButton.Disabled = true;
            uxSubmitActivityButton.Disabled = true;
            uxPostActivityButton.Disabled = true;
            uxInactiveActivityButton.Disabled = true;
        }
        /// <summary>
        /// Shows Submit activity Window/Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deSubmitActivity(object sender, DirectEventArgs e)
        {
            string WindowUrl = string.Format("umSubmitActivity.aspx?headerId={0}", e.ExtraParams["HeaderId"]);

            uxSubmitActivityWindow.LoadContent(WindowUrl);
            uxSubmitActivityWindow.Show();
        }

        /// <summary>
        /// Set Header to Inactive status(5)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deSetHeaderInactive(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
            DAILY_ACTIVITY_HEADER data;
            
            //Get Record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                        where d.HEADER_ID == HeaderId
                        select d).Single();
                data.STATUS = 5;
            }
            //Update record in DB
            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);

            uxManageGridStore.Reload();

        }

        /// <summary>
        /// Approve Activity(set status to 3)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deApproveActivity(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
            DAILY_ACTIVITY_HEADER data;

            //Get record to be updated
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                        where d.HEADER_ID == HeaderId
                        select d).Single();
                data.STATUS = 3;
            }

            //Update record in DB
            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);

            uxManageGridStore.Reload();
        }

        /// <summary>
        /// Get Header Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected DAILY_ACTIVITY_HEADER GetHeader(long HeaderId){
            using(Entities _context = new Entities()){
                DAILY_ACTIVITY_HEADER returnData = (from d in _context.DAILY_ACTIVITY_HEADER
                                                    where d.HEADER_ID == HeaderId
                                                    select d).Single();
                return returnData;
            }
        }

        /// <summary>
        /// Get Employee/Equipment Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<object> GetEmployee(long HeaderId){
            using(Entities _context = new Entities()){
                var returnData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                            join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                            from equip in equ.DefaultIfEmpty()
                            join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                            from projects in proj.DefaultIfEmpty()
                            where d.HEADER_ID == HeaderId
                            select new { e.EMPLOYEE_NAME, projects.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS }).ToList();
                return returnData;
            }
        }

        /// <summary>
        /// Get Production information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<object> GetProduction(long HeaderId){
            using(Entities _context = new Entities()){
                var returnData = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                            join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                            join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                            join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                            where d.HEADER_ID == HeaderId
                            select new { d.PRODUCTION_ID, h.PROJECT_ID, p.LONG_NAME, t.TASK_ID, t.DESCRIPTION, d.TIME_IN, d.TIME_OUT, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.GALLONS }).ToList();
                return returnData;
            }
        }

        /// <summary>
        /// Get Weather Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<DAILY_ACTIVITY_WEATHER> GetWeather(long HeaderId){
            using(Entities _context = new Entities()){
                var returnData = (from d in _context.DAILY_ACTIVITY_WEATHER
                                      where d.HEADER_ID == HeaderId
                                      select d).ToList();
                return returnData;
            }
        }

        /// <summary>
        /// Get Chemical Mix Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<DAILY_ACTIVITY_CHEMICAL_MIX> GetChemicalMix(long HeaderId){
            using(Entities _context = new Entities()){
                var returnData = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                                      where d.HEADER_ID == HeaderId
                                      select d).ToList();
                return returnData;
            }
        }

        /// <summary>
        /// Get Inventory Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected DAILY_ACTIVITY_INVENTORY GetInventory(long HeaderId){
            using(Entities _context = new Entities()){
            
            }
        }

        /// <summary>
        /// Get footer Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected DAILY_ACTIVITY_FOOTER GetFooter(long HeaderId){
            using(Entities _context = new Entities()){
                var returnData = (from d in _context.DAILY_ACTIVITY_FOOTER
                                      where d.HEADER_ID == HeaderId
                                      select d).Single();
                return returnData;
            }
        }

        /// <summary>
        /// Export selected Header to PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deExportToPDF(object sender, DirectEventArgs e)
        {
            using (MemoryStream PdfStream = new MemoryStream())
            {
                //Create the document
                Document ExportedPDF = new Document(iTextSharp.text.PageSize.LETTER);
                PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, PdfStream);

                //Get Header Data

                //Create Header Table
                PdfPTable HeaderTabel = new PdfPTable(10);


            }
                
                //Create the table
                PdfPTable AnswerTable = new PdfPTable(2);
                foreach (Dictionary<string, string> Answer in AnswerInfo)
                {
                    if (Answer["QuestionText"] == "Show Name")
                    {
                        int MyFormId = Convert.ToInt32(Answer["FormInfoId"]);
                        PostForm MyConn = new PostForm();
                        List<FormInfo> FormsInfo = (from f in MyConn.FormInfo
                                                   where f.FormInfoId == MyFormId
                                                   select f).ToList();
                        DateTime StartDate = DateTime.Now ;
                        DateTime EndDate = DateTime.Now;
                        foreach (FormInfo MyFormInfo in FormsInfo)
                        {
                            StartDate = MyFormInfo.DateFrom;
                            EndDate = MyFormInfo.DateTo;
                        }
                        Paragraph MyHeader = new Paragraph(Answer["Answer"], FontFactory.GetFont("Verdana", 20, Font.BOLD)); 
                        MyHeader.Alignment = 1;
                        Paragraph SubHeader = new Paragraph(StartDate.ToShortDateString() + " to " + EndDate.ToShortDateString(), FontFactory.GetFont("Verdana", 18, Font.BOLD));
                        SubHeader.Alignment = 1;
                        SubHeader.SpacingAfter = 10;
                        GeneratedPDF.Open();
                        GeneratedPDF.Add(MyHeader);
                        GeneratedPDF.Add(SubHeader);
                    }
                    else
                    {
                        AnswerTable.AddCell(new PdfPCell(new Phrase(Answer["QuestionText"])));
                        AnswerTable.AddCell(new PdfPCell(new Phrase(Answer["Answer"])));
                    }
                }
                Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD);
                GeneratedPDF.Add(AnswerTable);
                PdfWriter.CloseStream = false;
                GeneratedPDF.Close();
                myMemoryStream.Position = 0;

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment;filename=export.pdf");
                Response.BinaryWrite(myMemoryStream.ToArray());
                Response.End();

                myMemoryStream.Close();

            }
        }

        /// <summary>
        /// DirectMethod accessed from umSubmitActivity.aspx when signature is missing on SubmitActivity form
        /// </summary>
        [DirectMethod]
        public void dmSubmitNotification()
        {
            Notification.Show(new NotificationConfig()
            {
                Title = "Signature Missing",
                Html = "Unable to submit, signature missing.  Please provide the foreman signature.",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Direct Method accessed from umSubmitActivity.aspx after it's submitted
        /// </summary>
        [DirectMethod]
        public void dmHideWindowUpdateGrid()
        {
            uxSubmitActivityWindow.Hide();
            uxManageGridStore.Reload();
        }

        protected void deLoadCreateActivity(object sender, DirectEventArgs e)
        {
            uxCreateActivityWindow.LoadContent();
            uxCreateActivityWindow.Show();
        }
        

        
    }
}