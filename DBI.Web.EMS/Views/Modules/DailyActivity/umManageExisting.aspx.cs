using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects;
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
            if (!X.IsAjaxRequest)
            {
                //employeeHoursCheck();
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
                List<object> rawData;
            
                //Get List of all new headers
                rawData = (from d in _context.DAILY_ACTIVITY_HEADER
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                            join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                        select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE }).ToList<object>();
                List<HeaderData> data = new List<HeaderData>();

                List<long> HoursOver24 = checkEmployeeTime("Hours per day");
                List<long> HoursOver14 = checkEmployeeTime("Hours over 14");
                List<long> OverlapProjects = employeeTimeOverlapCheck();
                foreach (dynamic record in rawData)
                {
                    string Warning = "Green";

                    foreach(long OffendingProject in HoursOver24){
                        if (OffendingProject == record.HEADER_ID)
                        {
                            Warning = "Red";
                        }
                    }
                    foreach(long OffendingProject in HoursOver14){
                        if(OffendingProject == record.HEADER_ID){
                            Warning = "Yellow";
                        }
                    }

                    foreach (long OffendingProject in OverlapProjects)
                    {
                        if (OffendingProject == record.HEADER_ID)
                        {
                            Warning = "Red";
                        }
                    }

                    data.Add(new HeaderData
                    {
                        HEADER_ID = record.HEADER_ID,
                        PROJECT_ID = record.PROJECT_ID,
                        DA_DATE = record.DA_DATE,
                        SEGMENT1 = record.SEGMENT1,
                        LONG_NAME = record.LONG_NAME,
                        STATUS_VALUE = record.STATUS_VALUE,
                        WARNING = Warning
                    });
                }
                
                int count;
                uxManageGridStore.DataSource = GenericData.EnumerableFilterHeader<HeaderData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
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

            uxCombinedTab.Disabled = false;
            uxHeaderTab.Disabled = false;
            uxEquipmentTab.Disabled = false;
            uxProductionTab.Disabled = false;
            uxEmployeeTab.Disabled = false;
            uxChemicalTab.Disabled = false;
            uxWeatherTab.Disabled = false;
            uxInventoryTab.Disabled = false;

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
            uxExportToPDF.Disabled = false;
        }

        /// <summary>
        /// Disables tabs and buttons when row is deselected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deDeselectHeader(object sender, DirectEventArgs e)
        {
            uxApproveActivityButton.Disabled = true;
            uxSubmitActivityButton.Disabled = true;
            uxPostActivityButton.Disabled = true;
            uxInactiveActivityButton.Disabled = true;

            uxHeaderTab.Disabled = true;
            uxCombinedTab.Disabled = true;
            uxEquipmentTab.Disabled = true;
            uxEmployeeTab.Disabled = true;
            uxChemicalTab.Disabled = true;
            uxInventoryTab.Disabled = true;
            uxWeatherTab.Disabled = true;
            uxProductionTab.Disabled = true;
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
        protected List<object> GetHeader(long HeaderId){
            using(Entities _context = new Entities()){
                var returnData = (from d in _context.DAILY_ACTIVITY_HEADER
                                  join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                  where d.HEADER_ID == HeaderId
                                  select new { d.APPLICATION_TYPE, d.CONTRACTOR, d.DA_DATE, d.DENSITY, d.LICENSE, d.STATE, d.STATUS, d.SUBDIVISION, p.SEGMENT1 }).ToList<object>();
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
                            select new { e.EMPLOYEE_NAME, projects.NAME, d.TIME_IN, d.TIME_OUT, d.TRAVEL_TIME, d.DRIVE_TIME, d.PER_DIEM, d.COMMENTS }).ToList<object>();
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
                            select new { t.DESCRIPTION, d.TIME_IN, d.TIME_OUT, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.GALLONS }).ToList<object>();
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
        protected List<object>GetInventory(long HeaderId){
            using(Entities _context = new Entities()){
                List<object> returnData = (from d in _context.DAILY_ACTIVITY_INVENTORY
                            join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                            join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                            join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                            where d.HEADER_ID == HeaderId
                            from j in joined
                            where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                            select new {c.CHEMICAL_MIX_NUMBER, d.SUB_INVENTORY_SECONDARY_NAME, j.DESCRIPTION, d.RATE, u.UNIT_OF_MEASURE, d.EPA_NUMBER }).ToList<object>();

                return returnData;
            }
        }

        /// <summary>
        /// Get footer Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected DAILY_ACTIVITY_FOOTER GetFooter(long HeaderId){
            using(Entities _context = new Entities()){
                DAILY_ACTIVITY_FOOTER returnData = (from d in _context.DAILY_ACTIVITY_FOOTER
                                      where d.HEADER_ID == HeaderId
                                      select d).SingleOrDefault();
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
                //Set header Id
                long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);

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
                var HeaderData = GetHeader(HeaderId);

                //Create Header Table
                PdfPTable HeaderTable = new PdfPTable(4);
                HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                PdfPCell[] Cells;
                PdfPRow Row;
                foreach (dynamic Data in HeaderData)
                {
                    Paragraph Title = new Paragraph(string.Format("DAILY ACTIVITY REPORT FOR {0}", Data.DA_DATE.Date.ToString("MM/dd/yyyy")), FontFactory.GetFont("Verdana", 12, Font.BOLD));
                    Title.Alignment = 1;

                    ExportedPDF.Add(Title);
                    ExportedPDF.Add(NewLine);
                    
                    //First row
                    Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Contract", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(Data.SEGMENT1.ToString(), HeadFootCellFont)),
                    new PdfPCell(new Phrase("Sub-Division", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(Data.SUBDIVISION, HeadFootCellFont))};

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);

                    //Second row
                    Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("License Number", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(Data.LICENSE, HeadFootCellFont)),
                    new PdfPCell(new Phrase("State", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(Data.STATE, HeadFootCellFont))};

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);

                    //Third row
                    Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Type of Application/Work", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(Data.APPLICATION_TYPE, HeadFootCellFont)),
                    new PdfPCell(new Phrase("Density", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(Data.DENSITY, HeadFootCellFont))};

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);
                }
                ExportedPDF.Add(HeaderTable);

                ExportedPDF.Add(NewLine);

                //Get Equipment/Employee Data
                var EmployeeData = GetEmployee(HeaderId);

                PdfPTable EmployeeTable = new PdfPTable(8);

                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Truck/Equipment \n Name", HeaderFont)),
                    new PdfPCell(new Phrase("Operator(s)", HeaderFont)),
                    new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
                    new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
                    new PdfPCell(new Phrase("Total\nHours", HeaderFont)),
                    new PdfPCell(new Phrase("Travel\nTime", HeaderFont)),
                    new PdfPCell(new Phrase("Per\nDiem", HeaderFont)),
                    new PdfPCell(new Phrase("Comments", HeaderFont))};

                Row = new PdfPRow(Cells);
                EmployeeTable.Rows.Add(Row);

                foreach (dynamic Data in EmployeeData)
                {
                    string TravelTime;
                    try
                    {
                        TravelTime = Data.TRAVEL_TIME.ToString();
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        TravelTime = string.Empty;
                    }                   
                    string EquipmentName;
                    try
                    {
                        EquipmentName = Data.NAME.ToString();
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        EquipmentName = String.Empty;
                    }

                    TimeSpan TotalHours = DateTime.Parse(Data.TIME_OUT.ToString()).TimeOfDay - DateTime.Parse(Data.TIME_IN.ToString()).TimeOfDay; 
                    Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase(EquipmentName , CellFont)),
                        new PdfPCell(new Phrase(Data.EMPLOYEE_NAME.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.TIME_IN.TimeOfDay.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.TIME_OUT.TimeOfDay.ToString(), CellFont)),
                        new PdfPCell(new Phrase(TotalHours.ToString(), CellFont)),
                        new PdfPCell(new Phrase(TravelTime, CellFont)),
                        new PdfPCell(new Phrase(Data.PER_DIEM.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.COMMENTS.ToString(), CellFont))
                    };
                    Row = new PdfPRow(Cells);
                    EmployeeTable.Rows.Add(Row);
                }
                ExportedPDF.Add(EmployeeTable);
                ExportedPDF.Add(NewLine);

                //Get Production Data
                var ProductionData = GetProduction(HeaderId);

                PdfPTable ProductionTable = new PdfPTable(7);

                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
                    new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
                    new PdfPCell(new Phrase("Spray/Work Area", HeaderFont)),
                    new PdfPCell(new Phrase("Pole/MP\nFrom", HeaderFont)),
                    new PdfPCell(new Phrase("Pole/MP\nTo", HeaderFont)),
                    new PdfPCell(new Phrase("Acres/Mile", HeaderFont)),
                    new PdfPCell(new Phrase("Gallons", HeaderFont))};

                Row = new PdfPRow(Cells);
                ProductionTable.Rows.Add(Row);

                foreach (dynamic Data in ProductionData)
                {
                    Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase(Data.TIME_IN.TimeOfDay.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.TIME_OUT.TimeOfDay.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.WORK_AREA, CellFont)),
                        new PdfPCell(new Phrase(Data.POLE_FROM, CellFont)),
                        new PdfPCell(new Phrase(Data.POLE_TO, CellFont)),
                        new PdfPCell(new Phrase(Data.ACRES_MILE.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.GALLONS.ToString(), CellFont))
                    };

                    Row = new PdfPRow(Cells);
                    ProductionTable.Rows.Add(Row);
                }
                ExportedPDF.Add(ProductionTable);
                ExportedPDF.Add(NewLine);

                //Get Weather
                var WeatherData = GetWeather(HeaderId);

                PdfPTable WeatherTable = new PdfPTable(6);

                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Time", HeaderFont)),
                    new PdfPCell(new Phrase("Wind\nDirection", HeaderFont)),
                    new PdfPCell(new Phrase("Wind\nVelocity", HeaderFont)),
                    new PdfPCell(new Phrase("Temperature", HeaderFont)),
                    new PdfPCell(new Phrase("Humidity", HeaderFont)),
                    new PdfPCell(new Phrase("Comments", HeaderFont))
                };

                Row = new PdfPRow(Cells);
                WeatherTable.Rows.Add(Row);

                foreach (dynamic Weather in WeatherData)
                {
                    Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase(Weather.WEATHER_DATE_TIME.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Weather.WIND_DIRECTION, CellFont)),
                        new PdfPCell(new Phrase(Weather.WIND_VELOCITY, CellFont)),
                        new PdfPCell(new Phrase(Weather.TEMP, CellFont)),
                        new PdfPCell(new Phrase(Weather.HUMIDITY, CellFont)),
                        new PdfPCell(new Phrase(Weather.COMMENTS, CellFont))
                    };

                    Row = new PdfPRow(Cells);
                    WeatherTable.Rows.Add(Row);
                }
                ExportedPDF.Add(WeatherTable);
                ExportedPDF.Add(NewLine);

                //Get Chemical Mix Data
                var ChemicalData = GetChemicalMix(HeaderId);

                PdfPTable ChemicalTable = new PdfPTable(11);

                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Mix #", HeaderFont)),
                    new PdfPCell(new Phrase("Target\nArea", HeaderFont)),
                    new PdfPCell(new Phrase("Gals/Acre", HeaderFont)),
                    new PdfPCell(new Phrase("Gals\nStarting", HeaderFont)),
                    new PdfPCell(new Phrase("Gals\nMixed", HeaderFont)),
                    new PdfPCell(new Phrase("Total\nGallons", HeaderFont)),
                    new PdfPCell(new Phrase("Gals\nRemaining", HeaderFont)),
                    new PdfPCell(new Phrase("Gals\nUsed", HeaderFont)),
                    new PdfPCell(new Phrase("Acres\nSprayed", HeaderFont)),
                    new PdfPCell(new Phrase("State", HeaderFont)),
                    new PdfPCell(new Phrase("County", HeaderFont))
                };
                Row = new PdfPRow(Cells);
                ChemicalTable.Rows.Add(Row);

                foreach (dynamic Data in ChemicalData)
                {
                    decimal TotalGallons = Data.GALLON_STARTING + Data.GALLON_MIXED;
                    decimal GallonsUsed = TotalGallons - Data.GALLON_REMAINING;

                    Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase(Data.CHEMICAL_MIX_NUMBER != null ? Data.CHEMICAL_MIX_NUMBER.ToString() : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(Data.TARGET_AREA != null ? Data.TARGET_AREA : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(Data.GALLON_ACRE != null ? Data.GALLON_ACRE.ToString() : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(Data.GALLON_STARTING != null ? Data.GALLON_STARTING.ToString() : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(Data.GALLON_MIXED != null ? Data.GALLON_MIXED.ToString() : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(TotalGallons.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.GALLON_REMAINING != null ? Data.GALLON_REMAINING.ToString() : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(GallonsUsed.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.ACRES_SPRAYED != null ? Data.ACRES_SPRAYED.ToString() : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(Data.STATE != null ? Data.STATE : string.Empty, CellFont)),
                        new PdfPCell(new Phrase(Data.COUNTY != null ? Data.COUNTY : string.Empty, CellFont))
                    };
                    Row = new PdfPRow(Cells);
                    ChemicalTable.Rows.Add(Row);
                }

                ExportedPDF.Add(ChemicalTable);
                ExportedPDF.Add(NewLine);

                //Get Inventory Data
                var InventoryData = GetInventory(HeaderId);
                PdfPTable InventoryTable = new PdfPTable(5);

                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Mix #", HeaderFont)),
                    new PdfPCell(new Phrase("Sub-Inventory", HeaderFont)),
                    new PdfPCell(new Phrase("Item Name", HeaderFont)),
                    new PdfPCell(new Phrase("Rate", HeaderFont)),
                    new PdfPCell(new Phrase("EPA \n Number", HeaderFont))
                };
                Row = new PdfPRow(Cells);
                InventoryTable.Rows.Add(Row);

                foreach (dynamic Data in InventoryData)
                {
                    Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase(Data.CHEMICAL_MIX_NUMBER.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.SUB_INVENTORY_SECONDARY_NAME, CellFont)),
                        new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
                        new PdfPCell(new Phrase(string.Format("{0} {1}", Data.RATE.ToString(), Data.UNIT_OF_MEASURE), CellFont)),
                        new PdfPCell(new Phrase(Data.EPA_NUMBER, CellFont))
                    };

                    Row = new PdfPRow(Cells);
                    InventoryTable.Rows.Add(Row);
                }

                ExportedPDF.Add(InventoryTable);
                ExportedPDF.Add(NewLine);

                //Get Footer Data
                var FooterData = GetFooter(HeaderId);

                PdfPTable FooterTable = new PdfPTable(4);
                FooterTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                string ReasonForNoWork;
                string Hotel;
                string City;
                string State;
                string Phone;

                try
                {
                    ReasonForNoWork = FooterData.REASON_FOR_NO_WORK;
                }
                catch (NullReferenceException)
                {
                    ReasonForNoWork = string.Empty;
                }

                try
                {
                    Hotel = FooterData.HOTEL_NAME;
                }
                catch (NullReferenceException)
                {
                    Hotel = string.Empty;
                }

                try
                {
                    City = FooterData.HOTEL_CITY;
                }
                catch (NullReferenceException)
                {
                    City = string.Empty;
                }

                try
                {
                    State = FooterData.HOTEL_STATE;
                }
                catch (NullReferenceException)
                {
                    State = string.Empty;
                }

                try
                {
                    Phone = FooterData.HOTEL_PHONE;
                }
                catch (NullReferenceException)
                {
                    Phone = string.Empty;
                }

                Cells = new PdfPCell[] {
                    new PdfPCell(new Phrase("Reason for no work", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(ReasonForNoWork, HeadFootCellFont)),
                    new PdfPCell(new Phrase("Hotel, City, State, & Phone", HeadFootTitleFont)),
                    new PdfPCell(new Phrase(string.Format("{0} {1} {2} {3}",Hotel, City, State, Phone ), HeadFootCellFont))
                };

                foreach (PdfPCell Cell in Cells)
                {
                    Cell.Border = PdfPCell.NO_BORDER;
                }
                Row = new PdfPRow(Cells);
                FooterTable.Rows.Add(Row);

                iTextSharp.text.Image ForemanImage;
                iTextSharp.text.Image ContractImage;
                try
                {
                    ForemanImage = iTextSharp.text.Image.GetInstance(FooterData.FOREMAN_SIGNATURE.ToArray());
                    ForemanImage.ScaleAbsolute(75f, 25f);
                }
                catch (Exception)
                {
                    ForemanImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                }

                try
                {
                    ContractImage = iTextSharp.text.Image.GetInstance(FooterData.CONTRACT_REP.ToArray());
                    ContractImage.ScaleAbsolute(75f, 25f);
                }
                catch (Exception)
                {
                    ContractImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                }

                
                Cells = new PdfPCell[]{
                    new PdfPCell(new Phrase("Foreman Signature", HeadFootTitleFont)),
                    new PdfPCell(ForemanImage),
                    new PdfPCell(new Phrase("Contract Representative", HeadFootTitleFont)),
                    new PdfPCell(ContractImage),
                };
                foreach (PdfPCell Cell in Cells)
                {
                    Cell.Border = PdfPCell.NO_BORDER;
                }
                Row = new PdfPRow(Cells);
                FooterTable.Rows.Add(Row);

                ExportedPDF.Add(FooterTable);
                //Close Stream and start Download
                ExportWriter.CloseStream = false;
                ExportedPDF.Close();
                PdfStream.Position = 0;

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment;filename=export.pdf");
                Response.BinaryWrite(PdfStream.ToArray());
                Response.End();
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
        /// DirectMethod accessed from umDailyActivity.aspx after it's been submitted
        /// </summary>
        [DirectMethod]
        public void dmHideAddWindow()
        {
            uxCreateActivityWindow.Hide();
            uxManageGridStore.Reload();
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

        [DirectMethod]
        public void dmLoadPerDiemPicker(string HeaderList)
        {
            uxChoosePerDiemWindow.Loader.Url = string.Format("umChoosePerDiem.aspx?HeaderList={0}", HeaderList);
            uxChoosePerDiemWindow.LoadContent();
            uxChoosePerDiemWindow.Show();
            uxSubmitActivityWindow.Hide();
        }
        /// <summary>
        /// Load create activity form and display the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadCreateActivity(object sender, DirectEventArgs e)
        {
            uxCreateActivityWindow.LoadContent();
            uxCreateActivityWindow.Show();
        }

        /// <summary>
        /// Checks data and flags if needed based on below checks
        /// </summary>
        protected void checkData()
        {

        }

        /// <summary>
        /// Checks for more than 24 hours in a day by an employee
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<long> checkEmployeeTime(string CheckType)
        {

            using (Entities _context = new Entities())
            {
                var TotalHoursList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                      where d.DAILY_ACTIVITY_HEADER.STATUS != 4 || d.DAILY_ACTIVITY_HEADER.STATUS != 5
                                     group d by new { d.PERSON_ID, d.DAILY_ACTIVITY_HEADER.DA_DATE } into g
                                     select new { g.Key.PERSON_ID, g.Key.DA_DATE, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value))}).ToList();

                int i = 0;
                List<long> OffendingProjects = new List<long>();
                foreach (var TotalHour in TotalHoursList)
                {
                    if (CheckType == "Hours per day")
                    {
                        if (TotalHour.TotalMinutes / 60 >= 24)
                        {
                            var ProjectsWithEmployeeHoursOver24 = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                   where d.PERSON_ID == TotalHour.PERSON_ID && d.DAILY_ACTIVITY_HEADER.DA_DATE == TotalHour.DA_DATE
                                                                   select d).ToList();
                            foreach (var Project in ProjectsWithEmployeeHoursOver24)
                            {
                                OffendingProjects.Add(Project.HEADER_ID);
                            }
                        }
                    }
                    else
                    {
                        if (TotalHour.TotalMinutes / 60 >= 14  && TotalHour.TotalMinutes / 60 < 24)
                        {
                            var ProjectsWithEmployeeHoursOver14 = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                   where d.PERSON_ID == TotalHour.PERSON_ID && d.DAILY_ACTIVITY_HEADER.DA_DATE == TotalHour.DA_DATE
                                                                   select d).ToList();
                            foreach (var Project in ProjectsWithEmployeeHoursOver14)
                            {
                                OffendingProjects.Add(Project.HEADER_ID);
                            }
                        }
                    }
                }
                return OffendingProjects;

            }
        }

        protected void setProjectError()
        {

        }

        protected void setProjectWarning()
        {
           
        }
        
        protected List<long> employeeTimeOverlapCheck()
        {
            using (Entities _context = new Entities())
            {
                //Get List of Employees
                var PersonIdList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                            select d.PERSON_ID).Distinct().ToList();

                List<long> HeaderIdList = new List<long>();

                foreach (var PersonId in PersonIdList)
                {
                    //Get Headers for that employee
                    List<DAILY_ACTIVITY_EMPLOYEE> EmployeeHeaderList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                                 orderby d.TIME_IN ascending
                                                                 where d.PERSON_ID == PersonId
                                                                 select d).ToList<DAILY_ACTIVITY_EMPLOYEE>();
                    int count = 0;
                    DateTime PreviousTimeIn = DateTime.Parse("1/11/1955");
                    DateTime PreviousTimeOut = DateTime.Parse("1/11/1955");
                    long PreviousHeaderId = 0;
                    foreach (DAILY_ACTIVITY_EMPLOYEE Header in EmployeeHeaderList)
                    {
                        DateTime CurrentTimeIn = (DateTime) Header.TIME_IN;
                        DateTime CurrentTimeOut = (DateTime) Header.TIME_OUT;

                        if (count > 0)
                        {
                            if (CurrentTimeIn < PreviousTimeOut)
                            {
                                HeaderIdList.Add(PreviousHeaderId);
                                HeaderIdList.Add(Header.DAILY_ACTIVITY_HEADER.HEADER_ID);
                            }
                        }
                        PreviousHeaderId = Header.HEADER_ID;
                        PreviousTimeIn = CurrentTimeIn;
                        PreviousTimeOut = CurrentTimeOut;
                        count++;
                    }
                }
                return HeaderIdList;
            }
        }

        //protected bool employeeBusinessUnitCheck(long HeaderId)
        //{

        //}

        //protected bool equipmentBusinessUnitCheck(long HeaderId)
        //{

        //}
    }

    public class HeaderData
    {
        public long HEADER_ID { get; set; }
        public long PROJECT_ID { get; set; }
        public DateTime DA_DATE { get; set; }
        public string SEGMENT1 { get; set; }
        public string LONG_NAME { get; set; }
        public string STATUS_VALUE { get; set; }
        public string WARNING { get; set; }
    }
}