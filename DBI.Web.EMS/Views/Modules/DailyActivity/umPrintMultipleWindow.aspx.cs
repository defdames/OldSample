using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Core.Web;
using DBI.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umPrintMultipleWindow : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {
            }
        }

        protected void deReadPostableData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                IQueryable<HeaderData> HeaderList = (from d in _context.DAILY_ACTIVITY_HEADER
                                                     join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                                     select new HeaderData { HEADER_ID = d.HEADER_ID, DA_DATE = (DateTime)d.DA_DATE, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME });

                int count;
                uxHeaderPostStore.DataSource = GenericData.ListFilterHeader<HeaderData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], HeaderList, out count);
                e.Total = count;
            }
        }

        /// <summary>
        /// Get Header Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<HeaderData> GetHeader(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                var returnData = (from d in _context.DAILY_ACTIVITY_HEADER
                                  join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                  join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                  where d.HEADER_ID == HeaderId
                                  select new HeaderData { APPLICATION_TYPE = d.APPLICATION_TYPE, CONTRACTOR = d.CONTRACTOR, DA_DATE = d.DA_DATE, DENSITY = d.DENSITY, EMPLOYEE_NAME = e.EMPLOYEE_NAME, LICENSE = d.LICENSE, STATE = d.STATE, STATUS = d.STATUS, SUBDIVISION = d.SUBDIVISION, SEGMENT1 = p.SEGMENT1, LONG_NAME = p.LONG_NAME, DA_HEADER_ID = d.DA_HEADER_ID }).ToList();
                return returnData;
            }
        }

        /// <summary>
        /// Get Employee/Equipment Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<EmployeeDetails> GetEmployee(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                var returnData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                  join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                  join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
                                  from equip in equ.DefaultIfEmpty()
                                  join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
                                  from projects in proj.DefaultIfEmpty()
                                  where d.HEADER_ID == HeaderId
                                  select new EmployeeDetails { EMPLOYEE_NAME = e.EMPLOYEE_NAME, NAME = projects.NAME, LUNCH = d.LUNCH, LUNCH_LENGTH = d.LUNCH_LENGTH, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, FOREMAN_LICENSE = d.FOREMAN_LICENSE, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), PER_DIEM = (d.PER_DIEM == "Y" ? true : false), ROLE_TYPE = d.ROLE_TYPE, COMMENTS = d.COMMENTS }).ToList();
                foreach (var item in returnData)
                {
                    double Hours = Math.Truncate((double)item.TRAVEL_TIME);
                    double Minutes = Math.Round(((double)item.TRAVEL_TIME - Hours) * 60);
                    TimeSpan TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
                    item.TRAVEL_TIME_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                    Hours = Math.Truncate((double)item.DRIVE_TIME);
                    Minutes = Math.Round(((double)item.DRIVE_TIME - Hours) * 60);
                    item.DRIVE_TIME_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                    Hours = Math.Truncate((double)item.SHOPTIME_AM);
                    Minutes = Math.Round(((double)item.SHOPTIME_AM - Hours) * 60);
                    item.SHOPTIME_AM_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                    Hours = Math.Truncate((double)item.SHOPTIME_PM);
                    Minutes = Math.Round(((double)item.SHOPTIME_PM - Hours) * 60);
                    item.SHOPTIME_PM_FORMATTED = DateTime.Now.Date + TotalTimeSpan;
                }
                return returnData;
            }
        }

        protected List<EquipmentDetails> GetEquipment(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                var data = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                            join p in _context.CLASS_CODES_V on e.PROJECT_ID equals p.PROJECT_ID
                            where e.HEADER_ID == HeaderId
                            select new EquipmentDetails { CLASS_CODE = p.CLASS_CODE, ORGANIZATION_NAME = p.ORGANIZATION_NAME, ODOMETER_START = e.ODOMETER_START, ODOMETER_END = e.ODOMETER_END, PROJECT_ID = e.PROJECT_ID, EQUIPMENT_ID = e.EQUIPMENT_ID, NAME = p.NAME, HEADER_ID = e.HEADER_ID, SEGMENT1 = p.SEGMENT1 }).ToList();
                return data;
            }
        }

        /// <summary>
        /// Get Production information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<ProductionDetails> GetProductionDBI(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                var returnData = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                  join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                  join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                                  join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                  where d.HEADER_ID == HeaderId
                                  select new ProductionDetails { TASK_NUMBER = t.TASK_NUMBER, DESCRIPTION = t.DESCRIPTION, TIME_IN = d.TIME_IN, TIME_OUT = d.TIME_OUT, WORK_AREA = d.WORK_AREA, POLE_FROM = d.POLE_FROM, POLE_TO = d.POLE_TO, ACRES_MILE = d.ACRES_MILE, QUANTITY = d.QUANTITY }).ToList();
                return returnData;
            }
        }

        protected List<ProductionDetails> GetProductionIRM(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                var returnData = (from d in _context.DAILY_ACTIVITY_PRODUCTION
                                  join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                  join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
                                  join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                  where d.HEADER_ID == HeaderId
                                  select new ProductionDetails { TASK_NUMBER = t.TASK_NUMBER, DESCRIPTION = t.DESCRIPTION, WORK_AREA = d.WORK_AREA, STATION = d.STATION, EXPENDITURE_TYPE = d.EXPENDITURE_TYPE, BILL_RATE = d.BILL_RATE, UNIT_OF_MEASURE = d.UNIT_OF_MEASURE, SURFACE_TYPE = d.SURFACE_TYPE, COMMENTS = d.COMMENTS, QUANTITY = d.QUANTITY }).ToList();
                return returnData;
            }
        }

        /// <summary>
        /// Get Weather Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected List<DAILY_ACTIVITY_WEATHER> GetWeather(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
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
        protected List<DAILY_ACTIVITY_CHEMICAL_MIX> GetChemicalMix(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
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
        protected List<InventoryDetails> GetInventoryDBI(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<InventoryDetails> returnData = (from d in _context.DAILY_ACTIVITY_INVENTORY
                                                     join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                                                     join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
                                                     join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                                                     where d.HEADER_ID == HeaderId
                                                     from j in joined
                                                     where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                                                     select new InventoryDetails { CHEMICAL_MIX_NUMBER = c.CHEMICAL_MIX_NUMBER, INV_NAME = j.INV_NAME, SUB_INVENTORY_SECONDARY_NAME = d.SUB_INVENTORY_SECONDARY_NAME, SEGMENT1 = j.SEGMENT1, DESCRIPTION = j.DESCRIPTION, TOTAL = d.TOTAL, RATE = d.RATE, UNIT_OF_MEASURE = u.UNIT_OF_MEASURE, EPA_DESCRIPTION = d.EPA_NUMBER, CONTRACTOR_SUPPLIED = (d.CONTRACTOR_SUPPLIED == "Y" ? true : false) }).ToList();

                return returnData;
            }
        }

        protected List<InventoryDetails> GetInventoryIRM(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                List<InventoryDetails> returnData = (from d in _context.DAILY_ACTIVITY_INVENTORY
                                                     join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
                                                     join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
                                                     where d.HEADER_ID == HeaderId
                                                     from j in joined
                                                     where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
                                                     select new InventoryDetails { SUB_INVENTORY_SECONDARY_NAME = d.SUB_INVENTORY_SECONDARY_NAME, INV_NAME = j.INV_NAME, SEGMENT1 = j.SEGMENT1, DESCRIPTION = j.DESCRIPTION, RATE = d.RATE, UNIT_OF_MEASURE = u.UNIT_OF_MEASURE }).ToList();

                return returnData;
            }
        }

        /// <summary>
        /// Get footer Information
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected DAILY_ACTIVITY_FOOTER GetFooter(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                DAILY_ACTIVITY_FOOTER returnData = (from d in _context.DAILY_ACTIVITY_FOOTER
                                                    where d.HEADER_ID == HeaderId
                                                    select d).SingleOrDefault();
                return returnData;
            }
        }

        protected bool roleNeeded(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                string PrevailingWage = (from d in _context.DAILY_ACTIVITY_HEADER
                                         join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                         where d.HEADER_ID == HeaderId
                                         select p.ATTRIBUTE3).Single();
                if (PrevailingWage == "Y")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected MemoryStream generatePDF(List<long> HeaderList)
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
                foreach (long HeaderId in HeaderList)
                {
                    long OrgId;
                    using (Entities _context = new Entities())
                    {
                        OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
                                 join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                 where d.HEADER_ID == HeaderId
                                 select (long)p.ORG_ID).Single();
                    }
                    //Get Header Data
                    var HeaderData = GetHeader(HeaderId);

                    //Create Header Table
                    PdfPTable HeaderTable = new PdfPTable(4);
                    HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    PdfPCell[] Cells;
                    PdfPRow Row;
                    foreach (HeaderData Data in HeaderData)
                    {
                        Paragraph Title = new Paragraph("DAILY ACTIVITY REPORT", FontFactory.GetFont("Verdana", 12, Font.BOLD));
                        Title.Alignment = 1;

                        ExportedPDF.Add(Title);

                        Title = new Paragraph(Data.LONG_NAME, FontFactory.GetFont("Verdana", 12, Font.BOLD));
                        Title.Alignment = 1;
                        ExportedPDF.Add(Title);

                        DateTime DA_DATE = (DateTime)Data.DA_DATE;
                        Title = new Paragraph(DA_DATE.Date.ToString("MM/dd/yyyy"), FontFactory.GetFont("Verdana", 12, Font.BOLD));
                        Title.Alignment = 1;
                        ExportedPDF.Add(Title);
                        ExportedPDF.Add(NewLine);

                        string OracleHeader;
                        try
                        {
                            OracleHeader = Data.DA_HEADER_ID.ToString();
                        }
                        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                        {
                            OracleHeader = "0";
                        }
                        Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("DRS Id", HeadFootTitleFont )),
						new PdfPCell(new Phrase(HeaderId.ToString(), HeadFootCellFont)),
						new PdfPCell(new Phrase("Oracle Header Id", HeadFootTitleFont)),
						new PdfPCell(new Phrase(OracleHeader, HeadFootCellFont))
					};
                        foreach (PdfPCell Cell in Cells)
                        {
                            Cell.Border = PdfPCell.NO_BORDER;
                        }

                        Row = new PdfPRow(Cells);
                        HeaderTable.Rows.Add(Row);

                        //First row
                        Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Project Number", HeadFootTitleFont)),
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
					new PdfPCell(new Phrase("Business License #", HeadFootTitleFont)),
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

                        //Fourth Row
                        Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase("Supervisor/Area Manager", HeadFootTitleFont)),
                        new PdfPCell(new Phrase(Data.EMPLOYEE_NAME, HeadFootCellFont)),
                        new PdfPCell(new Phrase("Contractor", HeadFootTitleFont)),
                        new PdfPCell(new Phrase(Data.CONTRACTOR, HeadFootCellFont))
                    };

                        foreach (PdfPCell Cell in Cells)
                        {
                            Cell.Border = PdfPCell.NO_BORDER;
                        }
                        Row = new PdfPRow(Cells);
                        HeaderTable.Rows.Add(Row);

                    }
                    ExportedPDF.Add(HeaderTable);

                    ExportedPDF.Add(NewLine);

                    try
                    {
                        //Get Equipment/Employee Data
                        var EmployeeData = GetEmployee(HeaderId);
                        if (OrgId == 123)
                        {

                            PdfPTable EmployeeTable;
                            if (roleNeeded(HeaderId))
                            {
                                EmployeeTable = new PdfPTable(13);
                                EmployeeTable.SetWidths(new float[] { 8f, 9f, 6f, 8f, 9f, 7f, 6f, 5f, 7f, 7f, 5f, 10f, 13f });
                                Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Truck/Equipment \n Name", HeaderFont)),
						new PdfPCell(new Phrase("Operator(s)", HeaderFont)),
						new PdfPCell(new Phrase("License #", HeaderFont)),
						new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
						new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
						new PdfPCell(new Phrase("Total\nHours", HeaderFont)),
						new PdfPCell(new Phrase("Travel\nTime", HeaderFont)),
						new PdfPCell(new Phrase("Drive\nTime", HeaderFont)),
						new PdfPCell(new Phrase("ShopTime\nAM", HeaderFont)),
						new PdfPCell(new Phrase("ShopTime\nPM", HeaderFont)),
						new PdfPCell(new Phrase("Per\nDiem", HeaderFont)),
                        new PdfPCell(new Phrase("Role", HeaderFont)),
						new PdfPCell(new Phrase("Comments", HeaderFont))};
                            }
                            else
                            {
                                EmployeeTable = new PdfPTable(12);
                                EmployeeTable.SetWidths(new float[] { 9f, 10f, 7f, 9f, 10f, 8f, 7f, 6f, 8f, 7f, 6f, 13f });
                                Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Truck/Equipment \n Name", HeaderFont)),
						new PdfPCell(new Phrase("Operator(s)", HeaderFont)),
						new PdfPCell(new Phrase("License #", HeaderFont)),
						new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
						new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
						new PdfPCell(new Phrase("Total\nHours", HeaderFont)),
						new PdfPCell(new Phrase("Travel\nTime", HeaderFont)),
						new PdfPCell(new Phrase("Drive\nTime", HeaderFont)),
						new PdfPCell(new Phrase("ShopTime\nAM", HeaderFont)),
						new PdfPCell(new Phrase("ShopTime\nPM", HeaderFont)),
						new PdfPCell(new Phrase("Per\nDiem", HeaderFont)),
						new PdfPCell(new Phrase("Comments", HeaderFont))};
                            }


                            Row = new PdfPRow(Cells);
                            EmployeeTable.Rows.Add(Row);

                            foreach (var Data in EmployeeData)
                            {
                                string TravelTime;
                                try
                                {
                                    TravelTime = Data.TRAVEL_TIME_FORMATTED.ToString();
                                }
                                catch (Exception)
                                {
                                    TravelTime = string.Empty;
                                }
                                string EquipmentName;
                                try
                                {
                                    EquipmentName = Data.NAME.ToString();
                                }
                                catch (Exception)
                                {
                                    EquipmentName = String.Empty;
                                }
                                string Comments;
                                try
                                {
                                    Comments = Data.COMMENTS.ToString();
                                }
                                catch (Exception)
                                {
                                    Comments = String.Empty;
                                }
                                string License;
                                try
                                {
                                    License = Data.FOREMAN_LICENSE;
                                }
                                catch (Exception)
                                {
                                    License = string.Empty;
                                }
                                TimeSpan TotalHours = DateTime.Parse(Data.TIME_OUT.ToString()) - DateTime.Parse(Data.TIME_IN.ToString());
                                if (roleNeeded(HeaderId))
                                {
                                    Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(EquipmentName , CellFont)),
						new PdfPCell(new Phrase(Data.EMPLOYEE_NAME.ToString(), CellFont)),
						new PdfPCell(new Phrase(License, CellFont)),
						new PdfPCell(new Phrase(Data.TIME_IN.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TIME_OUT.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(TotalHours.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TRAVEL_TIME_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.DRIVE_TIME_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.SHOPTIME_AM_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.SHOPTIME_PM_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.PER_DIEM.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.ROLE_TYPE, CellFont)),
						new PdfPCell(new Phrase(Comments, CellFont))
					};
                                }
                                else
                                {
                                    Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(EquipmentName , CellFont)),
						new PdfPCell(new Phrase(Data.EMPLOYEE_NAME.ToString(), CellFont)),
						new PdfPCell(new Phrase(License, CellFont)),
						new PdfPCell(new Phrase(Data.TIME_IN.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TIME_OUT.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(TotalHours.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TRAVEL_TIME_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.DRIVE_TIME_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.SHOPTIME_AM_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.SHOPTIME_PM_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.PER_DIEM.ToString(), CellFont)),
						new PdfPCell(new Phrase(Comments, CellFont))
                            };
                                }
                                Row = new PdfPRow(Cells);
                                EmployeeTable.Rows.Add(Row);
                            }
                            ExportedPDF.Add(EmployeeTable);
                            ExportedPDF.Add(NewLine);
                        }
                        else
                        {
                            PdfPTable EmployeeTable = new PdfPTable(10);
                            EmployeeTable.SetWidths(new float[] { 13f, 13f, 7f, 10f, 10f, 7f, 6f, 6f, 7f, 14f });
                            Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Truck/Equipment \n Name", HeaderFont)),
						new PdfPCell(new Phrase("Operator(s)", HeaderFont)),
						new PdfPCell(new Phrase("License #", HeaderFont)),
						new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
						new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
						new PdfPCell(new Phrase("Total\nHours", HeaderFont)),
						new PdfPCell(new Phrase("Travel\nTime", HeaderFont)),
						new PdfPCell(new Phrase("Per\nDiem", HeaderFont)),
                        new PdfPCell(new Phrase("Lunch\nLength", HeaderFont)),
						new PdfPCell(new Phrase("Comments", HeaderFont))};

                            Row = new PdfPRow(Cells);
                            EmployeeTable.Rows.Add(Row);

                            foreach (var Data in EmployeeData)
                            {
                                string TravelTime;
                                try
                                {
                                    TravelTime = Data.TRAVEL_TIME_FORMATTED.ToString();
                                }
                                catch (Exception)
                                {
                                    TravelTime = string.Empty;
                                }
                                string EquipmentName;
                                try
                                {
                                    EquipmentName = Data.NAME.ToString();
                                }
                                catch (Exception)
                                {
                                    EquipmentName = String.Empty;
                                }
                                string Comments;
                                try
                                {
                                    Comments = Data.COMMENTS.ToString();
                                }
                                catch (Exception)
                                {
                                    Comments = String.Empty;
                                }
                                string License;
                                try
                                {
                                    License = Data.FOREMAN_LICENSE;
                                }
                                catch (Exception)
                                {
                                    License = string.Empty;
                                }
                                TimeSpan TotalHours = DateTime.Parse(Data.TIME_OUT.ToString()) - DateTime.Parse(Data.TIME_IN.ToString());
                                Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(EquipmentName , CellFont)),
						new PdfPCell(new Phrase(Data.EMPLOYEE_NAME.ToString(), CellFont)),
						new PdfPCell(new Phrase(License, CellFont)),
						new PdfPCell(new Phrase(Data.TIME_IN.ToString("hh\\:mm tt"), CellFont)),
						new PdfPCell(new Phrase(Data.TIME_OUT.ToString("hh\\:mm tt"), CellFont)),
						new PdfPCell(new Phrase(TotalHours.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TRAVEL_TIME_FORMATTED.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.PER_DIEM.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.LUNCH_LENGTH.ToString(), CellFont)),
						new PdfPCell(new Phrase(Comments, CellFont))
					};
                                Row = new PdfPRow(Cells);
                                EmployeeTable.Rows.Add(Row);
                            }
                            ExportedPDF.Add(EmployeeTable);
                            ExportedPDF.Add(NewLine);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        //Get Equipment Data
                        var EquipmentData = GetEquipment(HeaderId);
                        PdfPTable EquipmentTable = new PdfPTable(6);
                        EquipmentTable.SetWidths(new int[] { 10, 10, 35, 25, 10, 10 });
                        Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase("Project Number", HeaderFont)),
						new PdfPCell(new Phrase("Equipment Name", HeaderFont)),
						new PdfPCell(new Phrase("Class Code", HeaderFont)),
						new PdfPCell(new Phrase("Organization Name", HeaderFont)),
						new PdfPCell(new Phrase("Starting Units", HeaderFont)),
						new PdfPCell(new Phrase("Ending Units", HeaderFont))
					};

                        Row = new PdfPRow(Cells);
                        EquipmentTable.Rows.Add(Row);

                        foreach (EquipmentDetails Equipment in EquipmentData)
                        {
                            string OdometerStart;
                            string OdometerEnd;
                            string ProjectNumber;
                            try
                            {
                                ProjectNumber = Equipment.SEGMENT1;
                            }
                            catch (Exception)
                            {
                                ProjectNumber = string.Empty;
                            }
                            try
                            {
                                OdometerStart = Equipment.ODOMETER_START.ToString();
                            }
                            catch (Exception)
                            {
                                OdometerStart = string.Empty;
                            }
                            try
                            {
                                OdometerEnd = Equipment.ODOMETER_END.ToString();
                            }
                            catch (Exception)
                            {
                                OdometerEnd = string.Empty;
                            }

                            Cells = new PdfPCell[]{
                            new PdfPCell(new Phrase(ProjectNumber, CellFont)),
							new PdfPCell(new Phrase(Equipment.NAME, CellFont)),
							new PdfPCell(new Phrase(Equipment.CLASS_CODE, CellFont)),
							new PdfPCell(new Phrase(Equipment.ORGANIZATION_NAME, CellFont)),
							new PdfPCell(new Phrase(OdometerStart, CellFont)),
							new PdfPCell(new Phrase(OdometerEnd, CellFont))
						};

                            Row = new PdfPRow(Cells);
                            EquipmentTable.Rows.Add(Row);
                        }
                        ExportedPDF.Add(EquipmentTable);
                        ExportedPDF.Add(NewLine);
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        //Get Production Data
                        if (OrgId == 121)
                        {
                            string WorkArea;
                            var ProductionData = GetProductionDBI(HeaderId);
                            PdfPTable ProductionTable = new PdfPTable(7);
                            ProductionTable.SetWidths(new float[] { 10f, 15f, 40f, 9f, 9f, 9f, 8f });
                            Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("Task Number", HeaderFont)),
							new PdfPCell(new Phrase("Task Name", HeaderFont)),
							new PdfPCell(new Phrase("Spray/Work Area", HeaderFont)),
							new PdfPCell(new Phrase("Pole/MP\nFrom", HeaderFont)),
							new PdfPCell(new Phrase("Pole/MP\nTo", HeaderFont)),
							new PdfPCell(new Phrase("Acres/Mile", HeaderFont)),
							new PdfPCell(new Phrase("Gallons", HeaderFont))
						};

                            Row = new PdfPRow(Cells);
                            ProductionTable.Rows.Add(Row);

                            foreach (ProductionDetails Data in ProductionData)
                            {
                                try
                                {
                                    WorkArea = Data.WORK_AREA.ToString();
                                }
                                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                                {
                                    WorkArea = string.Empty;
                                }
                                string PoleFrom;
                                string PoleTo;
                                try
                                {
                                    PoleFrom = Data.POLE_FROM.ToString();
                                }
                                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                                {
                                    PoleFrom = String.Empty;
                                }
                                try
                                {
                                    PoleTo = Data.POLE_TO.ToString();
                                }
                                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                                {
                                    PoleTo = String.Empty;
                                }
                                Cells = new PdfPCell[]{
								new PdfPCell(new Phrase(Data.TASK_NUMBER, CellFont)),
								new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
								new PdfPCell(new Phrase(WorkArea, CellFont)),
								new PdfPCell(new Phrase(PoleFrom, CellFont)),
								new PdfPCell(new Phrase(PoleTo, CellFont)),
								new PdfPCell(new Phrase(Data.ACRES_MILE.ToString(), CellFont)),
								new PdfPCell(new Phrase(Data.QUANTITY.ToString(), CellFont))
							};

                                Row = new PdfPRow(Cells);
                                ProductionTable.Rows.Add(Row);
                            }
                            ExportedPDF.Add(ProductionTable);
                        }
                        if (OrgId == 123)
                        {
                            var ProductionData = GetProductionIRM(HeaderId);
                            PdfPTable ProductionTable = new PdfPTable(9);

                            Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("Task Number", HeaderFont)),
							new PdfPCell(new Phrase("Task Name", HeaderFont)),
							new PdfPCell(new Phrase("Quantity", HeaderFont)),
							new PdfPCell(new Phrase("Station", HeaderFont)),
							new PdfPCell(new Phrase("Expenditure Type", HeaderFont)),
                            new PdfPCell(new Phrase("Bill Rate", HeaderFont)),
                            new PdfPCell(new Phrase("Units", HeaderFont)),
                            new PdfPCell(new Phrase("Surface Type", HeaderFont)),
							new PdfPCell(new Phrase("Comments", HeaderFont))
						};

                            Row = new PdfPRow(Cells);
                            ProductionTable.Rows.Add(Row);

                            foreach (ProductionDetails Data in ProductionData)
                            {
                                Cells = new PdfPCell[]{
								new PdfPCell(new Phrase(Data.TASK_NUMBER, CellFont)),
								new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
								new PdfPCell(new Phrase(Data.QUANTITY.ToString(), CellFont)),
								new PdfPCell(new Phrase(Data.STATION, CellFont)),
								new PdfPCell(new Phrase(Data.EXPENDITURE_TYPE.ToString(), CellFont)),
                                new PdfPCell(new Phrase(Data.BILL_RATE.ToString(), CellFont)),
                                new PdfPCell(new Phrase(Data.UNIT_OF_MEASURE, CellFont)),
                                new PdfPCell(new Phrase(Data.SURFACE_TYPE, CellFont)),
								new PdfPCell(new Phrase(Data.COMMENTS.ToString(), CellFont))
							};

                                Row = new PdfPRow(Cells);
                                ProductionTable.Rows.Add(Row);
                            }
                            ExportedPDF.Add(ProductionTable);
                        }
                        ExportedPDF.Add(NewLine);
                    }
                    catch (Exception)
                    {

                    }
                    //Get Weather
                    try
                    {
                        var WeatherData = GetWeather(HeaderId);

                        PdfPTable WeatherTable = new PdfPTable(6);
                        WeatherTable.SetWidths(new float[] { 15f, 10f, 10f, 10f, 10f, 45f });
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

                        foreach (DAILY_ACTIVITY_WEATHER Weather in WeatherData)
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
                    }
                    catch (Exception)
                    {

                    }
                    if (OrgId == 121)
                    {
                        try
                        {
                            //Get Chemical Mix Data
                            var ChemicalData = GetChemicalMix(HeaderId);

                            PdfPTable ChemicalTable = new PdfPTable(11);
                            ChemicalTable.SetWidths(new float[] { 4f, 10f, 8f, 10f, 8f, 10f, 10f, 10f, 10f, 10f, 10f });
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

                            foreach (var Data in ChemicalData)
                            {
                                decimal TotalGallons = (decimal)Data.GALLON_STARTING + (decimal)Data.GALLON_MIXED;
                                decimal GallonsUsed = TotalGallons - (decimal)Data.GALLON_REMAINING;

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
                        }
                        catch (Exception)
                        {
                        }
                    }

                    //Get Inventory Data
                    try
                    {
                        if (OrgId == 121)
                        {
                            var InventoryData = GetInventoryDBI(HeaderId);
                            PdfPTable InventoryTable = new PdfPTable(10);
                            InventoryTable.SetWidths(new float[] { 4f, 5f, 13f, 10f, 23f, 5f, 5f, 10f, 10f, 15f });
                            Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Mix #", HeaderFont)),
                    new PdfPCell(new Phrase("Item #", HeaderFont)),
					new PdfPCell(new Phrase("Inventory Org", HeaderFont)),
					new PdfPCell(new Phrase("Sub-Inventory", HeaderFont)),
					new PdfPCell(new Phrase("Item Name", HeaderFont)),
					new PdfPCell(new Phrase("Rate", HeaderFont)),
                    new PdfPCell(new Phrase("Total", HeaderFont)),
                    new PdfPCell(new Phrase("Units", HeaderFont)),                    
					new PdfPCell(new Phrase("EPA \n Number", HeaderFont)),
                    new PdfPCell(new Phrase("Customer Material", HeaderFont))
				};
                            Row = new PdfPRow(Cells);
                            InventoryTable.Rows.Add(Row);

                            foreach (InventoryDetails Data in InventoryData)
                            {
                                string EPANumber;
                                try
                                {
                                    EPANumber = Data.EPA_DESCRIPTION;
                                }
                                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                                {
                                    EPANumber = string.Empty;
                                }
                                Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(Data.CHEMICAL_MIX_NUMBER.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.SEGMENT1, CellFont)),
						new PdfPCell(new Phrase(Data.INV_NAME, CellFont)),
						new PdfPCell(new Phrase(Data.SUB_INVENTORY_SECONDARY_NAME, CellFont)),
						new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
						new PdfPCell(new Phrase(Data.RATE.ToString(), CellFont)),                        
						new PdfPCell(new Phrase(Data.TOTAL.ToString(), CellFont)),
                        new PdfPCell(new Phrase(Data.UNIT_OF_MEASURE, CellFont)),
						new PdfPCell(new Phrase(EPANumber, CellFont)),
                        new PdfPCell(new Phrase((Data.CONTRACTOR_SUPPLIED == true ? "Y" : "N"), CellFont))
					};

                                Row = new PdfPRow(Cells);
                                InventoryTable.Rows.Add(Row);
                            }

                            ExportedPDF.Add(InventoryTable);
                        }
                        if (OrgId == 123)
                        {
                            var InventoryData = GetInventoryIRM(HeaderId);
                            PdfPTable InventoryTable = new PdfPTable(6);
                            InventoryTable.SetWidths(new float[] { 15f, 15f, 15f, 35f, 10f, 10f });

                            Cells = new PdfPCell[]{
                            new PdfPCell(new Phrase("Item #", HeaderFont)),
							new PdfPCell(new Phrase("Inventory Org", HeaderFont)),
							new PdfPCell(new Phrase("Sub-Inventory", HeaderFont)),
							new PdfPCell(new Phrase("Item Name", HeaderFont)),
							new PdfPCell(new Phrase("Quantity", HeaderFont)),
                            new PdfPCell(new Phrase("Units", HeaderFont))
					 };
                            Row = new PdfPRow(Cells);
                            InventoryTable.Rows.Add(Row);

                            foreach (InventoryDetails Data in InventoryData)
                            {
                                Cells = new PdfPCell[]{
                                new PdfPCell(new Phrase(Data.SEGMENT1, CellFont)),
								new PdfPCell(new Phrase(Data.INV_NAME, CellFont)),
								new PdfPCell(new Phrase(Data.SUB_INVENTORY_SECONDARY_NAME, CellFont)),
								new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
								new PdfPCell(new Phrase(Data.RATE.ToString(), CellFont)),
                                new PdfPCell(new Phrase(Data.UNIT_OF_MEASURE, CellFont ))
					};

                                Row = new PdfPRow(Cells);
                                InventoryTable.Rows.Add(Row);
                            }

                            ExportedPDF.Add(InventoryTable);
                        }
                        ExportedPDF.Add(NewLine);
                    }
                    catch (Exception)
                    {

                    }
                    //Get Footer Data
                    try
                    {
                        var FooterData = GetFooter(HeaderId);
                        string ForemanName;
                        using (Entities _context = new Entities())
                        {
                            ForemanName = (from d in _context.DAILY_ACTIVITY_HEADER
                                           join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                           where d.HEADER_ID == HeaderId
                                           select e.EMPLOYEE_NAME).Single();

                        }

                        PdfPTable FooterTable = new PdfPTable(4);
                        FooterTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                        string ReasonForNoWork;
                        string Hotel;
                        string City;
                        string State;
                        string Phone;

                        try
                        {
                            ReasonForNoWork = FooterData.COMMENTS;
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

                        Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Foreman Name", HeadFootTitleFont)),
						new PdfPCell(new Phrase(ForemanName, HeadFootCellFont)),
						new PdfPCell(new Phrase("Contract Rep Name", HeadFootTitleFont)),
						new PdfPCell(new Phrase(FooterData.CONTRACT_REP_NAME, HeadFootCellFont))
					};
                        foreach (PdfPCell Cell in Cells)
                        {
                            Cell.Border = PdfPCell.NO_BORDER;
                        }
                        Row = new PdfPRow(Cells);
                        FooterTable.Rows.Add(Row);


                        ExportedPDF.Add(FooterTable);

                        PdfPTable SignatureTable = new PdfPTable(2);
                        iTextSharp.text.Image ForemanImage;
                        iTextSharp.text.Image ContractImage;
                        try
                        {
                            ForemanImage = iTextSharp.text.Image.GetInstance(FooterData.FOREMAN_SIGNATURE.ToArray());
                            ForemanImage.ScaleAbsolute(250f, 82f);
                        }
                        catch (Exception)
                        {
                            ForemanImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                        }

                        try
                        {
                            ContractImage = iTextSharp.text.Image.GetInstance(FooterData.CONTRACT_REP.ToArray());
                            ContractImage.ScaleAbsolute(250f, 82f);
                        }
                        catch (Exception)
                        {
                            ContractImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                        }


                        Cells = new PdfPCell[]{
					//new PdfPCell(new Phrase("Foreman Signature", HeadFootTitleFont)),
					new PdfPCell(ForemanImage),
					new PdfPCell(ContractImage)
					
				};
                        foreach (PdfPCell Cell in Cells)
                        {
                            Cell.Border = PdfPCell.NO_BORDER;
                        }
                        Row = new PdfPRow(Cells);
                        SignatureTable.Rows.Add(Row);
                        //Cells = new PdfPCell[]{
                        //    new PdfPCell(new Phrase("Contract Representative", HeadFootTitleFont)),
                        //    new PdfPCell(ContractImage)
                        //};
                        //foreach (PdfPCell Cell in Cells)
                        //{
                        //    Cell.Border = PdfPCell.NO_BORDER;
                        //}
                        //Row = new PdfPRow(Cells);
                        //SignatureTable.Rows.Add(Row);
                        if (OrgId == 123)
                        {
                            iTextSharp.text.Image DotRepImage;
                            try
                            {
                                DotRepImage = iTextSharp.text.Image.GetInstance(FooterData.DOT_REP.ToArray());
                                DotRepImage.ScaleAbsolute(300f, 100f);
                            }
                            catch (Exception)
                            {
                                DotRepImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
                            }

                            Cells = new PdfPCell[]{
					
					new PdfPCell(new Phrase("Name", HeadFootTitleFont)),
					new PdfPCell(new Phrase(FooterData.DOT_REP_NAME, HeadFootCellFont))
					};
                            foreach (PdfPCell Cell in Cells)
                            {
                                Cell.Border = PdfPCell.NO_BORDER;
                            }
                            Row = new PdfPRow(Cells);
                            Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("DOT Representative", HeadFootTitleFont)),
							new PdfPCell(DotRepImage)
						};
                            foreach (PdfPCell Cell in Cells)
                            {
                                Cell.Border = PdfPCell.NO_BORDER;
                            }
                            Row = new PdfPRow(Cells);
                            SignatureTable.Rows.Add(Row);

                        }
                        ExportedPDF.Add(SignatureTable);
                    }

                    catch (Exception)
                    {

                    }
                    ExportedPDF.NewPage();
                }
                //Close Stream and start Download
                ExportWriter.CloseStream = false;
                ExportedPDF.Close();
                return PdfStream;
            }
        }

        protected void dePrintMultiple(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["RowsToPrint"];
            List<HeaderData> HeadersToPost = JSON.Deserialize<List<HeaderData>>(json);
            List<long>HeaderList = new List<long>();
            foreach (var Header in HeadersToPost){
                HeaderList.Add(Header.HEADER_ID);
            }

            MemoryStream PdfStream = generatePDF(HeaderList);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment;filename=export.pdf");
            Response.BinaryWrite(PdfStream.ToArray());
            Response.End();

            
        }
    }
}