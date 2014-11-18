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
using System.Text;


namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umManageExisting : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            uxPostMultipleButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Post");
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
                IQueryable<DAILY_ACTIVITY.HeaderData> rawData;

                if (validateComponentSecurity("SYS.DailyActivity.View"))
                {
                    List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();

                    rawData = DAILY_ACTIVITY.GetHeaders(_context, uxToggleInactive.Checked, uxTogglePosted.Checked, OrgsList);
                }
                else
                {
                    string EmployeeName = Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);
                    long PersonId = (from d in _context.EMPLOYEES_V
                                     where d.EMPLOYEE_NAME == EmployeeName
                                     select d.PERSON_ID).Single();

                    rawData = DAILY_ACTIVITY.GetHeaders(_context, uxToggleInactive.Checked, uxTogglePosted.Checked, null, PersonId);
                    uxCreateActivityButton.Disabled = true;

                }

                int count;

                List<DAILY_ACTIVITY.HeaderData> data = GenericData.ListFilterHeader<DAILY_ACTIVITY.HeaderData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], rawData, out count).ToList();

                List<DAILY_ACTIVITY.EmployeeData> HoursOver24 = ValidationChecks.checkEmployeeTime(24);
                List<DAILY_ACTIVITY.EmployeeData> HoursOver14 = ValidationChecks.checkEmployeeTime(14);


                foreach (DAILY_ACTIVITY.HeaderData record in data)
                {
                    string Warning = "Zero";
                    string WarningType = string.Empty;
                    if (record.STATUS != 4 && record.STATUS != 5)
                    {
                        foreach (DAILY_ACTIVITY.EmployeeData OffendingProject in HoursOver14)
                        {
                            if (OffendingProject.HEADER_ID == record.HEADER_ID)
                            {
                                Warning = "Warning";
                                WarningType = "Over 14 hours logged for an employee <br />";
                                break;
                            }

                        }
                        if (ValidationChecks.EquipmentBusinessUnitCheck(record.HEADER_ID, "bool"))
                        {
                            {
                                Warning = "Warning";
                                WarningType += "Contains Equipment outside of Business Unit.<br />";
                            }
                        }
                        if (ValidationChecks.EmployeeBusinessUnitCheck(record.HEADER_ID, "bool"))
                        {

                            Warning = "Warning";
                            WarningType += "Contains Employees outside of Business Unit.<br />";
                        }
                        foreach (DAILY_ACTIVITY.EmployeeData OffendingProject in HoursOver24)
                        {
                            if (OffendingProject.HEADER_ID == record.HEADER_ID)
                            {
                                Warning = "Error";
                                WarningType += "24 or more hours logged for an employee.<br />";
                                break;
                            }
                        }

                        if (ValidationChecks.employeeTimeOverlapCheck(record.HEADER_ID))
                        {
                            Warning = "Error";
                            WarningType += "An employee has overlapping time with another project.<br />";
                        }

                        if (record.ORG_ID == 121)
                        {
                            List<DAILY_ACTIVITY.WarningData> LunchList = ValidationChecks.LunchCheck(record.HEADER_ID);
                            if (LunchList.Count > 0)
                            {
                                Warning = "Error";
                                WarningType += "An employee is missing a lunch entry.<br />";
                            }
                        }

                        DAILY_ACTIVITY.WarningData PerDiems = ValidationChecks.checkPerDiem(record.HEADER_ID);
                        if (PerDiems != null)
                        {
                            Warning = PerDiems.WarningType;
                            WarningType += PerDiems.AdditionalInformation;
                        }

                        record.WARNING = Warning;
                        record.WARNING_TYPE = WarningType;
                    }
                }


                uxManageGridStore.DataSource = data;
                e.Total = count;
                uxTotalRecords.Text = count.ToString() + "Total Records";

            }
        }

        /// <summary>
        /// Update Tab URLs based on selected header and activate buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deUpdateUrlAndButtons(object sender, DirectEventArgs e)
        {
            if (isDirty == 0)
            {
                long OrgId;
                long CoOrgId;
                long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
                string homeUrl = string.Empty;
                using (Entities _context = new Entities())
                {
                    OrgId = DAILY_ACTIVITY.GetHeaderOrgId(_context, HeaderId);
                    CoOrgId = DAILY_ACTIVITY.GetHeaderCoOrgId(_context, HeaderId);
                }

                List<DAILY_ACTIVITY.EmployeeData> HoursOver24 = ValidationChecks.checkEmployeeTime(24);
                bool BadHeader = false;

                if (OrgId == 121)
                {
                    homeUrl = string.Format("umCombinedTab_DBI.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
                }
                else if (OrgId == 123)
                {
                    homeUrl = string.Format("umCombinedTab_IRM.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
                }


                if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), CoOrgId))
                {
                    switch (e.ExtraParams["Status"])
                    {
                        case "PENDING APPROVAL":
                            uxApproveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
                            uxPostActivityButton.Disabled = true;
                            uxMarkAsPostedButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.MarkAsPosted");
                            uxInactiveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
                            uxDeactivate.Value = "Deactivate";
                            uxInactiveActivityButton.Text = "Set Inactive";
                            uxApproveActivityButton.Text = "Approve";
                            uxHiddenApprove.Value = "Approve";
                            break;
                        case "APPROVED":
                            uxInactiveActivityButton.Text = "Set Inactive";
                            uxPostActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Post");
                            uxMarkAsPostedButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.MarkAsPosted");
                            uxInactiveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
                            uxDeactivate.Value = "Deactivate";

                            if (validateComponentSecurity("SYS.DailyActivity.Post") && validateComponentSecurity("SYS.DailyActivity.Approve"))
                            {
                                uxApproveActivityButton.Text = "Unapprove";
                                uxApproveActivityButton.Disabled = false;
                                uxHiddenApprove.Value = "Unapprove";
                            }
                            else
                            {
                                uxApproveActivityButton.Disabled = true;
                            }
                            break;
                        case "POSTED":
                            uxApproveActivityButton.Disabled = true;
                            uxPostActivityButton.Disabled = true;
                            uxMarkAsPostedButton.Disabled = true;
                            uxInactiveActivityButton.Disabled = true;
                            break;
                        case "INACTIVE":
                            uxApproveActivityButton.Disabled = true;
                            uxPostActivityButton.Disabled = true;
                            uxMarkAsPostedButton.Disabled = true;

                            uxInactiveActivityButton.Text = "Activate";
                            uxDeactivate.Value = "Activate";
                            uxInactiveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
                            break;

                    }
                    if (e.ExtraParams["Status"] != "INACTIVE")
                    {
                        List<long> EmployeeOverLap = ValidationChecks.employeeTimeOverlapCheck();

                        if (HoursOver24.Count > 0)
                        {
                            if (HoursOver24.Exists(emp => emp.HEADER_ID == HeaderId))
                            {
                                DAILY_ACTIVITY.EmployeeData HeaderData = HoursOver24.Find(emp => emp.HEADER_ID == HeaderId);
                                BadHeader = true;
                            }
                        }

                        if (EmployeeOverLap.Count > 0)
                        {
                            using (Entities _context = new Entities())
                            {
                                if (EmployeeOverLap.Exists(x => x == HeaderId))
                                {
                                    var HeaderData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                      join emp in _context.EMPLOYEES_V on d.PERSON_ID equals emp.PERSON_ID
                                                      where d.HEADER_ID == HeaderId
                                                      select new { d.DAILY_ACTIVITY_HEADER.DA_DATE, emp.EMPLOYEE_NAME }).First();
                                    BadHeader = true;
                                }
                            }
                        }
                    }
                    if (BadHeader && e.ExtraParams["Status"] == "APPROVED")
                    {

                        uxPostActivityButton.Disabled = true;
                    }
                    else if (BadHeader)
                    {
                        uxApproveActivityButton.Disabled = true;
                        uxPostActivityButton.Disabled = true;
                    }
                }
                else
                {
                    uxApproveActivityButton.Disabled = true;
                    uxPostActivityButton.Disabled = true;
                    uxMarkAsPostedButton.Disabled = true;
                    uxInactiveActivityButton.Disabled = true;
                }

                uxExportToPDF.Disabled = false;
                uxEmailPdf.Disabled = false;
                uxExportPDFCombined.Disabled = false;
                uxEmailPDFCombined.Disabled = false;

                string LoaderURL = (OrgId == 123 ? "umCombinedTab_IRM.aspx" : "umCombinedTab_DBI.aspx") + "?HeaderId=" + HeaderId;
                uxTotalRecords.Text = GetRecordNumber(int.Parse(e.ExtraParams["CurrentPage"])).ToString() + "/" + e.ExtraParams["TotalRecords"];
                uxDetailsPanel.LoadContent(LoaderURL);
            }
            else
            {
                CreateDirtyMessage();
            }

        }

        protected int GetRecordNumber(int CurrentPage)
        {
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;
            return (CurrentPage - 1) * 20 + Index + 1;
        }


        protected void deLoadNextActivity(object sender, DirectEventArgs e)
        {
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;
            int LastRecord = int.Parse(e.ExtraParams["ToRecord"].ToString()) - int.Parse(e.ExtraParams["FromRecord"].ToString());
            if (Index < LastRecord)
            {
                GridModel.SelectedRow.RowIndex = GridModel.SelectedIndex + 1;
                GridModel.Select(GridModel.SelectedIndex);
                GridModel.UpdateSelection();
            }
            else if (LastRecord == 19)
            {

                uxManageGridStore.NextPage(new
                {
                    callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(0)}")
                });

            }
        }

        protected void deLoadPreviousActivity(object sender, DirectEventArgs e)
        {
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;

            if (Index > 0)
            {
                GridModel.SelectedRow.RowIndex = GridModel.SelectedIndex - 1;
                GridModel.Select(GridModel.SelectedIndex);
                GridModel.UpdateSelection();
            }
            else if (int.Parse(e.ExtraParams["CurrentPage"].ToString()) != 1)
            {
                X.Js.Call("App.uxManageGridStore.previousPage({callback: function(){App.uxManageGrid.getSelectionModel().select(19)}})");

            }
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
                data = DAILY_ACTIVITY.GetHeader(_context, HeaderId).Single();
            }
            Ext.Net.Button MyButton = sender as Ext.Net.Button;
            if (uxDeactivate.Value.ToString() == "Deactivate")
            {
                data.STATUS = 5;

            }
            else
            {
                data.STATUS = 2;
            }
            //Update record in DB
            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;


            uxManageGridStore.Reload(new
            {
                callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(" + Index + ")}")
            });

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
            SYS_SECURITY_AUDIT ActionToLog = new SYS_SECURITY_AUDIT();
            //Get record to be updated
            using (Entities _context = new Entities())
            {
                data = DAILY_ACTIVITY.GetHeader(_context, HeaderId).Single();

                ActionToLog.MODULE = "Daily Activity";
                ActionToLog.RECORD_NUMBER = HeaderId;
                ActionToLog.MODIFIED_BY = User.Identity.Name;
                ActionToLog.MODIFIED_DATE = DateTime.Now;
                if (uxHiddenApprove.Value.ToString() == "Approve")
                {
                    data.STATUS = 3;
                    ActionToLog.COMMENTS = "DRS Approved";
                }
                else
                {
                    data.STATUS = 2;
                    ActionToLog.COMMENTS = "DRS Unapproved";
                }
            }



            //Update record in DB
            GenericData.Insert<SYS_SECURITY_AUDIT>(ActionToLog);
            GenericData.Update<DAILY_ACTIVITY_HEADER>(data);
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;


            uxManageGridStore.Reload(new
            {
                callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(" + Index + ")}")
            });

        }

        /// <summary>
        /// Export selected Header to PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deExportToPDF(object sender, DirectEventArgs e)
        {
            //Set header Id
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
            string ProjectName;
            using (Entities _context = new Entities())
            {
                ProjectName = (from d in _context.DAILY_ACTIVITY_HEADER
                               join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                               where d.HEADER_ID == HeaderId
                               select p.LONG_NAME).Single();
            }
            MemoryStream PdfStream = generatePDF(HeaderId);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}{1}-export.pdf", HeaderId.ToString(), RemoveSpecialCharacters(ProjectName)));
            Response.BinaryWrite(PdfStream.ToArray());
            Response.End();
        }

        protected static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        protected void deSendPDF(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);


            using (MemoryStream PdfStream = new MemoryStream(generatePDF(HeaderId).ToArray()))
            {
                string Subject = "Copy of Daily Activity Report";
                bool IsHtml = true;
                string Message = "Please find attached the Daily Activity Report you requested.";

                PdfStream.Position = 0;
                string ProjectName;
                using (Entities _context = new Entities())
                {
                    ProjectName = (from d in _context.DAILY_ACTIVITY_HEADER
                                   join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                                   where d.HEADER_ID == HeaderId
                                   select p.LONG_NAME).Single();
                }

                Attachment MailAttachment = new Attachment(PdfStream, string.Format("{0}{1}-export.pdf", HeaderId.ToString(), RemoveSpecialCharacters(ProjectName)));
                var smtp = new SmtpClient("owa.dbiservices.com");
#if DEBUG
                smtp.Credentials = new System.Net.NetworkCredential("gene.lapointe@dbiservices.com", "password");
#endif
                MailMessage EmailMessage = new MailMessage(User.Identity.Name + "@dbiservices.com", User.Identity.Name + "@dbiservices.com", Subject, Message);
                EmailMessage.Attachments.Add(MailAttachment);
                EmailMessage.IsBodyHtml = true;
                //smtp.SendMessage(User.Identity.Name + "@dbiservices.com", Subject, Message, IsHtml, MailAttachment);
                smtp.Send(EmailMessage);
                X.Msg.Alert("Email sent", string.Format("Message has been sent to {0}@dbiservices.com", User.Identity.Name.ToLower())).Show();

            }
        }

        protected void dePostToOracle(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);

            try
            {
                Interface.PostToOracle(HeaderId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Daily Activity did not post successfully, Error: " + ex.InnerException.ToString(),
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });
            }

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Daily Activity posted successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;


            uxManageGridStore.Reload(new
            {
                callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(" + Index + ")}")
            });
        }

        protected void deMarkAsPosted(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
            DAILY_ACTIVITY_HEADER ToUpdate;
            using (Entities _context = new Entities())
            {
                ToUpdate = DAILY_ACTIVITY.GetHeader(_context, HeaderId).Single();
                ToUpdate.STATUS = 4;
            }
            uxMarkAsPostedButton.Disabled = true;
            GenericData.Update<DAILY_ACTIVITY_HEADER>(ToUpdate);
            RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var Index = GridModel.SelectedIndex;

            uxManageGridStore.Reload(new
            {
                callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(" + Index + ")}")
            });

        }

        protected MemoryStream generatePDF(long HeaderId)
        {
            long OrgId;
            using (Entities _context = new Entities())
            {
                OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
                         join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                         where d.HEADER_ID == HeaderId
                         select (long)p.ORG_ID).Single();
            }

            using (MemoryStream PdfStream = new MemoryStream())
            {
                using (Entities _context = new Entities())
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

                    DAILY_ACTIVITY.HeaderData HeaderData = DAILY_ACTIVITY.GetHeaderData(_context, HeaderId).Single();
                    //Create Header Table
                    PdfPTable HeaderTable = new PdfPTable(4);
                    HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    PdfPCell[] Cells;
                    PdfPRow Row;

                    Paragraph Title = new Paragraph("DAILY ACTIVITY REPORT", FontFactory.GetFont("Verdana", 12, Font.BOLD));
                    Title.Alignment = 1;

                    ExportedPDF.Add(Title);

                    Title = new Paragraph(HeaderData.LONG_NAME, FontFactory.GetFont("Verdana", 12, Font.BOLD));
                    Title.Alignment = 1;
                    ExportedPDF.Add(Title);

                    DateTime HeaderDate = (DateTime)HeaderData.DA_DATE;
                    Title = new Paragraph(HeaderDate.Date.ToString("MM/dd/yyyy"), FontFactory.GetFont("Verdana", 12, Font.BOLD));
                    Title.Alignment = 1;
                    ExportedPDF.Add(Title);
                    ExportedPDF.Add(NewLine);

                    string OracleHeader;
                    try
                    {
                        OracleHeader = HeaderData.DA_HEADER_ID.ToString();
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        OracleHeader = "0";
                    }
                    Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("DRS Number", HeadFootTitleFont )),
						new PdfPCell(new Phrase(HeaderId.ToString(), HeadFootCellFont)),
						new PdfPCell(new Phrase("Oracle DRS Number", HeadFootTitleFont)),
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
					new PdfPCell(new Phrase(HeaderData.SEGMENT1.ToString(), HeadFootCellFont)),
					new PdfPCell(new Phrase("Sub-Division", HeadFootTitleFont)),
					new PdfPCell(new Phrase(HeaderData.SUBDIVISION, HeadFootCellFont))};

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);

                    //Second row
                    Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Business License #", HeadFootTitleFont)),
					new PdfPCell(new Phrase(HeaderData.LICENSE, HeadFootCellFont)),
					new PdfPCell(new Phrase("State", HeadFootTitleFont)),
					new PdfPCell(new Phrase(HeaderData.STATE, HeadFootCellFont))};

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);

                    //Third row
                    Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Type of Application/Work", HeadFootTitleFont)),
					new PdfPCell(new Phrase(HeaderData.APPLICATION_TYPE, HeadFootCellFont)),
					new PdfPCell(new Phrase("Density", HeadFootTitleFont)),
					new PdfPCell(new Phrase(HeaderData.DENSITY, HeadFootCellFont))};

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);

                    //Fourth Row
                    Cells = new PdfPCell[]{
                        new PdfPCell(new Phrase("Supervisor/Area Manager", HeadFootTitleFont)),
                        new PdfPCell(new Phrase(HeaderData.EMPLOYEE_NAME, HeadFootCellFont)),
                        new PdfPCell(new Phrase("Contractor", HeadFootTitleFont)),
                        new PdfPCell(new Phrase(HeaderData.CONTRACTOR, HeadFootCellFont))
                    };

                    foreach (PdfPCell Cell in Cells)
                    {
                        Cell.Border = PdfPCell.NO_BORDER;
                    }
                    Row = new PdfPRow(Cells);
                    HeaderTable.Rows.Add(Row);


                    ExportedPDF.Add(HeaderTable);

                    ExportedPDF.Add(NewLine);

                    try
                    {
                        //Get Equipment/Employee Data
                        List<DAILY_ACTIVITY.EmployeeDetails> EmployeeData = DAILY_ACTIVITY.GetIRMEmployeeData(_context, HeaderId).ToList();

                        if (OrgId == 123)
                        {

                            PdfPTable EmployeeTable;
                            if (DAILY_ACTIVITY.RoleNeeded(_context, HeaderId))
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

                            foreach (DAILY_ACTIVITY.EmployeeDetails Data in EmployeeData)
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
                                if (DAILY_ACTIVITY.RoleNeeded(_context, HeaderId))
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

                            foreach (DAILY_ACTIVITY.EmployeeDetails Data in EmployeeData)
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
                        List<DAILY_ACTIVITY.EquipmentDetails> EquipmentData = DAILY_ACTIVITY.GetEquipmentData(_context, HeaderId).ToList();

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

                        foreach (DAILY_ACTIVITY.EquipmentDetails Equipment in EquipmentData)
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
                            List<DAILY_ACTIVITY.ProductionDetails> ProductionData = DAILY_ACTIVITY.GetDBIProductionData(_context, HeaderId).ToList();

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

                            foreach (DAILY_ACTIVITY.ProductionDetails Data in ProductionData)
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
                            List<DAILY_ACTIVITY.ProductionDetails> ProductionData = DAILY_ACTIVITY.GetIRMProductionData(_context, HeaderId).ToList();

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

                            foreach (DAILY_ACTIVITY.ProductionDetails Data in ProductionData)
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
                        List<DAILY_ACTIVITY.WeatherDetails> WeatherData = DAILY_ACTIVITY.GetWeatherData(_context, HeaderId).ToList();

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

                        foreach (DAILY_ACTIVITY.WeatherDetails Weather in WeatherData)
                        {
                            Cells = new PdfPCell[]{
						new PdfPCell(new Phrase((Weather.WEATHER_DATE.Date + Weather.WEATHER_TIME.TimeOfDay).ToString(), CellFont)),
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
                            List<DAILY_ACTIVITY.ChemicalDetails> ChemicalData = DAILY_ACTIVITY.GetChemicalData(_context, HeaderId).ToList();


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

                            foreach (DAILY_ACTIVITY.ChemicalDetails Data in ChemicalData)
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
                            List<DAILY_ACTIVITY.InventoryDetails> InventoryData = DAILY_ACTIVITY.GetDBIInventoryData(_context, HeaderId).ToList();

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

                            foreach (DAILY_ACTIVITY.InventoryDetails Data in InventoryData)
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
                            List<DAILY_ACTIVITY.InventoryDetails> InventoryData = DAILY_ACTIVITY.GetIRMInventoryData(_context, HeaderId).ToList();

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

                            foreach (DAILY_ACTIVITY.InventoryDetails Data in InventoryData)
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
                        DAILY_ACTIVITY.FooterData FooterData = DAILY_ACTIVITY.GetFooterData(_context, HeaderId).Single();


                        string ForemanName;

                        ForemanName = (from d in _context.DAILY_ACTIVITY_HEADER
                                       join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
                                       where d.HEADER_ID == HeaderId
                                       select e.EMPLOYEE_NAME).Single();



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
                    //Close Stream and start Download
                    ExportWriter.CloseStream = false;
                    ExportedPDF.Close();
                    return PdfStream;
                }
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
            uxPlaceholderWindow.Hide();
            var GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
            var GridIndex = GridModel.SelectedIndex;
            uxManageGridStore.Reload();
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

        protected void deOpenPostMultipleWindow(object sender, DirectEventArgs e)
        {
            CreateWindow("umPostMultipleWindow.aspx", "Post Multiple DRS");
        }

        protected void deOpenExportMultipleWindow(object sender, DirectEventArgs e)
        {
            CreateWindow("umPrintMultipleWindow.aspx", "Export Multiple DRS");
        }

        [DirectMethod]
        public void dmLoadLunchWindow(string HeaderId, string EmployeeId)
        {
            CreateWindow(string.Format("umChooseLunchHeader.aspx?HeaderId={0}&EmployeeId={1}", HeaderId, EmployeeId), "Choose Lunch DRS");
        }

        [DirectMethod]
        public void dmLoadPerDiemWindow(string HeaderId, string EmployeeId)
        {
            CreateWindow(string.Format("umChoosePerDiem.aspx?HeaderId={0}&EmployeeId={1}", HeaderId, EmployeeId), "Choose Per Diem");
        }

        protected void CreateWindow(string LoaderUrl, string Title)
        {
            Window win = new Window()
            {
                ID = "uxPlaceholderWindow",

                Modal = true,
                Resizable = false,
                AutoRender = false,
                Y = 15,
                Title = Title,
                Constrain = false,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = LoaderUrl,
                    DisableCaching = true,
                    Mode = LoadMode.Frame,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            this.Form.Controls.Add(win);
            win.Render(this.Form);
            win.Show();
        }

        [DirectMethod]
        public void dmShowLunchTaskError(string ProjectName)
        {
            X.Msg.Alert("Error", string.Format(" Project {0} has no chargeable task.", ProjectName)).Show();
        }

    }
}