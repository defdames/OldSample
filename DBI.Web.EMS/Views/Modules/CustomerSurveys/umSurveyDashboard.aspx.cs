using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umSurveyDashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CustomerSurveys.Dashboard"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }

        protected long GetOrgFromTree(string _selectedRecordID)
        {
            if (_selectedRecordID.Contains(":"))
            {
                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                return long.Parse(_selectedID[1].ToString());
            }
            else
            {
                return long.Parse(_selectedRecordID);
            }
        }

        protected long GetHierarchyFromTree(string _selectedRecordID)
        {
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            return long.Parse(_selectedID[0].ToString());
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = HR.ActiveOverheadBudgetLegalEntities();
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        e.Nodes.Add(node);
                    }
                }

                else
                {
                    long nodeID = long.Parse(e.NodeID);
                    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID);
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_STRUCTURE_ID.ToString(), view.ORGANIZATION_ID.ToString());
                        e.Nodes.Add(node);
                    }
                }
            }

            // User clicked on org
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());
                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);
                var OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID);
                bool addNode;
                bool leafNode;
                bool colorNode;

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        addNode = false;
                        leafNode = true;
                        colorNode = false;
                        var nextData = HR.ActiveOrganizationsByHierarchy(hierarchyID, view.ORGANIZATION_ID);

                        // In this org?
                        if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in OrgsList)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrgs))
                            {
                                addNode = true;
                                leafNode = false;
                                break;
                            }
                        }

                        if (addNode == true)
                        {
                            Node node = new Node();
                            node.NodeID = string.Format("{0}:{1}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            //if (colorNode == true)
                            //{
                            //    node.Icon = Icon.Tick;
                            //}
                            //else
                            //{
                            //    node.Icon = Icon.FolderGo;
                            //}
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        protected void deReadDashboard(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            List<long> OrgsList;
            if (_selectedRecordID != string.Empty)
            {
                List<long> ProjectList;
                long SelectedOrgId = GetOrgFromTree(_selectedRecordID);
                long HierId = GetHierarchyFromTree(_selectedRecordID);
                //Get Orgs list
                using (Entities _context = new Entities())
                {
                    OrgsList = HR.ActiveOrganizationsByHierarchy(HierId, SelectedOrgId, _context).Select(x => x.ORGANIZATION_ID).ToList();
                    //ProjectList = XXEMS.ProjectsByOrgHierarchy(OrgsList, _context).Select(x => x.PROJECT_ID).ToList();

                    var data = XXDBI_DW.JobCostbyProjectList(OrgsList, HierId, _context);//.Join(_context.CUSTOMER_SURVEY_THRESH_AMT, jc => jc.ORG_ID, tham => tham.ORG_ID, (jc, tham) => new { job_cost = jc, threshold = tham }).Where(x => x.job_cost.LEVEL_SORT == 8).Select(x => new Threshold { PROJECT_NAME = x.job_cost.PROJECT_NAME, PROJECT_NUMBER = x.job_cost.PROJECT_NUMBER, PERCENTAGE = (x.job_cost.BGT_GREC == 0 ? 0 : Math.Round((double)(x.job_cost.FY_GREC / x.job_cost.BGT_GREC * 100))) });
                    int count;

                    var Source = GenericData.ListFilterHeader<XXDBI_DW.Threshold>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    uxDashboardStore.DataSource = Source;
                    e.Total = count;
                }
            }

        }

        protected void deEmailLink(object sender, DirectEventArgs e)
        {
            List<XXDBI_DW.Threshold> RowData = JSON.Deserialize<List<XXDBI_DW.Threshold>>(e.ExtraParams["RowValues"]);
            decimal FormId;
            string ToAddress;
            //Get form for Organization
            using (Entities _context = new Entities())
            {
                ToAddress = CUSTOMER_SURVEYS.GetProjectContacts(RowData[0].PROJECT_ID, _context).Select(x => x.EMAIL_ADDRESS).SingleOrDefault();
            }

            if (ToAddress != null)
            {

                //generate code to tie back to customer
                CUSTOMER_SURVEY_FORMS_COMP NewFormToSubmit = new CUSTOMER_SURVEY_FORMS_COMP();
                using (Entities _context = new Entities())
                {
                    NewFormToSubmit.FORM_ID = CUSTOMER_SURVEYS.GetFormIdByOrg(RowData[0].ORG_ID, _context);
                    NewFormToSubmit.PROJECT_ID = RowData[0].PROJECT_ID;
                    NewFormToSubmit.THRESHOLD_ID = RowData[0].THRESHOLD_ID;
                    NewFormToSubmit.CREATE_DATE = DateTime.Now;
                    NewFormToSubmit.MODIFY_DATE = DateTime.Now;
                    NewFormToSubmit.CREATED_BY = User.Identity.Name;
                    NewFormToSubmit.MODIFIED_BY = User.Identity.Name;
                }

                GenericData.Insert<CUSTOMER_SURVEY_FORMS_COMP>(NewFormToSubmit);

                //generate link

                string QueryString = RSAClass.Encrypt(NewFormToSubmit.COMPLETION_ID.ToString());


                //send email with link
                string Subject = "DBi Services Customer Satisfaction Survey Invitation";
                string Message = string.Format(@"<html><body><p>Dear Valued Customer,</p><p>I would like to take this opportunity to thank you again personally for selecting DBi Services to service your account.  We highly value your feedback, so please 
take a few moments to complete this brief survey to help us help you.</p><p>Please click on the link below to complete this brief survey:</p><p><a href ='http://emsv2.dbiservices.com/Public/umViewSurvey.aspx?FormId={0}'>Start Survey</a></p>
<p>We sincerely appreciate your time to assist us.  Thank you for helping us to serve you better.</p>
<p>Paul D. DeAngelo<br />President</p></body></html>", QueryString);

                var smtp = new SmtpClient("owa.dbiservices.com");
#if DEBUG
                smtp.Credentials = new System.Net.NetworkCredential("gene.lapointe@dbiservices.com", "Monkey1!");
#endif
                MailMessage EmailMessage = new MailMessage(User.Identity.Name + "@dbiservices.com", User.Identity.Name + "@dbiservices.com", Subject, Message);
                EmailMessage.IsBodyHtml = true;
                //smtp.SendMessage(User.Identity.Name + "@dbiservices.com", Subject, Message, IsHtml, MailAttachment);
                smtp.Send(EmailMessage);
                X.Msg.Alert("Email sent", string.Format("Message has been sent to {0}", ToAddress)).Show();
            }
            else
            {
                X.Msg.Alert("No Email Address", "The selected project does not have an associated email address").Show();
            }
        }

        protected void dePrintPDF(object sender, DirectEventArgs e)
        {
            List<XXDBI_DW.Threshold> RowData = JSON.Deserialize<List<XXDBI_DW.Threshold>>(e.ExtraParams["RowValues"]);
            decimal FormId;
            //Get form for Organization
            using (Entities _context = new Entities())
            {
                FormId = CUSTOMER_SURVEYS.GetFormIdByOrg(RowData[0].ORG_ID, _context);
            }

            //generate code to tie back to customer
            CUSTOMER_SURVEY_FORMS_COMP NewFormToSubmit = new CUSTOMER_SURVEY_FORMS_COMP();
            
            NewFormToSubmit.FORM_ID = FormId;
            NewFormToSubmit.PROJECT_ID = RowData[0].PROJECT_ID;
            NewFormToSubmit.THRESHOLD_ID = RowData[0].THRESHOLD_ID;
            NewFormToSubmit.CREATE_DATE = DateTime.Now;
            NewFormToSubmit.MODIFY_DATE = DateTime.Now;
            NewFormToSubmit.CREATED_BY = User.Identity.Name;
            NewFormToSubmit.MODIFIED_BY = User.Identity.Name;
            

            GenericData.Insert<CUSTOMER_SURVEY_FORMS_COMP>(NewFormToSubmit);
            //Get questions
            byte[] PdfStream = generatePDF(FormId, NewFormToSubmit.COMPLETION_ID);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", PdfStream.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment;filename=survey-form.pdf");
            Response.BinaryWrite(PdfStream.ToArray());
            Response.End();

        }

        protected byte[] generatePDF(decimal FormId, decimal CompletionId)
        {
            List<CUSTOMER_SURVEYS.CustomerSurveyQuestions> FormToGenerate;
            List<CUSTOMER_SURVEYS.CustomerSurveyFieldsets> Fieldsets;
            byte[] result;
            using (Entities _context = new Entities())
            {
                Fieldsets = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context).Where(x => x.IS_ACTIVE == true).OrderBy(x => x.SORT_ORDER).ToList();
                
            }
            using (MemoryStream PdfStream = new MemoryStream())
            {

                Document ExportedPDF = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 42f, 42f);
                PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, PdfStream);
                Font TableFont = GetTahoma();
                TableFont.Size = 10;
                Rectangle Page = ExportedPDF.PageSize;
                Font TableHeaderFont = GetTahoma();
                TableHeaderFont.Size = 12;
                TableHeaderFont.IsBold();
                //HeaderFooter Footer = new HeaderFooter(new Phrase("ID:" + CompletionId.ToString(), TableFont), false);
                //Footer.Border = HeaderFooter.NO_BORDER;
                //ExportedPDF.Footer = Footer;

                ExportedPDF.Open();
                
                PdfPTable HeaderTable = new PdfPTable(1);
                HeaderTable.WidthPercentage = 25;
                
                PdfPCell HeaderImage = new PdfPCell(iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images/dbis_black_logo.png")), true);
                HeaderImage.Border = PdfPCell.NO_BORDER;
                HeaderImage.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                HeaderTable.AddCell(HeaderImage);
                ExportedPDF.Add(HeaderTable);

                foreach (CUSTOMER_SURVEYS.CustomerSurveyFieldsets Fieldset in Fieldsets)
                {
                    PdfPTable FieldsetTable = new PdfPTable(1);
                    FieldsetTable.SpacingBefore = 5f;
                    FieldsetTable.SpacingAfter = 5f;
                    FieldsetTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    PdfPCell TitleCell = new PdfPCell(new Phrase(Fieldset.TITLE, TableHeaderFont));
                    TitleCell.BorderWidthLeft = 0f;
                    TitleCell.BorderWidthRight = 0f;
                    TitleCell.BorderWidthBottom = 1f;
                    TitleCell.BorderWidthTop = 0f;
                    FieldsetTable.AddCell(TitleCell);
                    
                    using (Entities _context = new Entities())
                    {
                        FormToGenerate = CUSTOMER_SURVEYS.GetFieldsetQuestionsForGrid(Fieldset.FIELDSET_ID, _context).OrderBy(x => x.SORT_ORDER).ToList();
                    }
                    foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestions Question in FormToGenerate)
                    {
                        PdfPTable Table = new PdfPTable(2);
                        Table.DefaultCell.Border = PdfPCell.NO_BORDER;
                        Table.WidthPercentage = 100;
                        Table.SpacingBefore = 5f;
                        Table.SpacingAfter = 5f;
                        //Table.SetWidths(new float[] { .65f, .35f });
                        PdfPCell Cell;
                        iTextSharp.text.pdf.events.FieldPositioningEvents Events;
                        List<CUSTOMER_SURVEY_OPTIONS> QuestionOptions;
                        string[] Options;
                        int count = 0;
                        switch (Question.QUESTION_TYPE_NAME)
                        {

                            case "singletext":
                                Table.AddCell(new Phrase(Question.TEXT + ":", TableFont));
                                Cell = new PdfPCell();
                                iTextSharp.text.pdf.TextField TextField = new iTextSharp.text.pdf.TextField(ExportWriter, new Rectangle(10, 20), "field" + Question.QUESTION_ID.ToString());
                                Events = new iTextSharp.text.pdf.events.FieldPositioningEvents(ExportWriter, TextField.GetTextField());
                                Cell.CellEvent = Events;

                                Table.AddCell(Cell);
                                break;
                            case "multitext":
                                Table.AddCell(new Phrase(Question.TEXT + ":", TableFont));
                                Cell = new PdfPCell();
                                TextField = new iTextSharp.text.pdf.TextField(ExportWriter, new Rectangle(10, 20), "field" + Question.QUESTION_ID.ToString());
                                TextField.Options = iTextSharp.text.pdf.TextField.MULTILINE;
                                Events = new iTextSharp.text.pdf.events.FieldPositioningEvents(ExportWriter, TextField.GetTextField());
                                Cell.CellEvent = Events;
                                Cell.MinimumHeight = 50;
                                Table.AddCell(Cell);
                                break;
                            case "dropdown":
                                using (Entities _context = new Entities())
                                {
                                    QuestionOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).ToList();
                                }
                                Options = new string[QuestionOptions.Count];

                                foreach (CUSTOMER_SURVEY_OPTIONS Option in QuestionOptions)
                                {
                                    Options[count] = Option.OPTION_NAME;
                                    count++;
                                }
                                Table.AddCell(new Phrase(Question.TEXT + ":", TableFont));
                                PdfFormField DropDown = iTextSharp.text.pdf.PdfFormField.CreateCombo(ExportWriter, true, Options, 0);

                                DropDown.SetWidget(new Rectangle(10, 20), PdfAnnotation.HIGHLIGHT_INVERT);
                                DropDown.FieldName = "field" + Question.QUESTION_ID;
                                Cell = new PdfPCell();
                                Events = new iTextSharp.text.pdf.events.FieldPositioningEvents(ExportWriter, DropDown);
                                Cell.CellEvent = Events;
                                Table.AddCell(Cell);
                                break;
                            case "radio":


                                Table.AddCell(new Phrase(Question.TEXT + ":", TableFont));
                                PdfFormField RadioGroup = PdfFormField.CreateRadioButton(ExportWriter, true);
                                RadioGroup.FieldName = "field" + Question.QUESTION_ID;
                                using (Entities _context = new Entities())
                                {
                                    QuestionOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).ToList();
                                }
                                PdfPTable RadioTable = new PdfPTable(QuestionOptions.Count * 2);
                                RadioTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                                RadioCheckField Radio;
                                PdfFormField RadioField = null;

                                foreach (CUSTOMER_SURVEY_OPTIONS Option in QuestionOptions)
                                {
                                    var root = PdfFormField.CreateEmpty(ExportWriter);
                                    root.FieldName = "root" + Option.OPTION_ID.ToString();

                                    Radio = new RadioCheckField(ExportWriter, new Rectangle(40, 806 - count * 40, 60, 788 - count * 40), Option.OPTION_NAME, "option" + Option.OPTION_ID.ToString());
                                    Radio.BackgroundColor = new GrayColor(0.8f);
                                    Radio.CheckType = RadioCheckField.TYPE_CIRCLE;
                                    RadioField = Radio.CheckField;

                                    RadioGroup.AddKid(RadioField);
                                    var Widths = new int[QuestionOptions.Count * 2];
                                    for (int i = 0; i < QuestionOptions.Count * 2; i++)
                                    {
                                        if (i % 2 == 1)
                                        {
                                            Widths[i] = 10;
                                        }
                                        else
                                        {
                                            Widths[i] = 20;
                                        }
                                    }
                                    RadioTable.SetWidths(Widths);

                                    RadioTable.AddCell(new Phrase(Option.OPTION_NAME + ":", TableFont));

                                    Cell = new PdfPCell();
                                    Cell.Border = PdfPCell.NO_BORDER;

                                    Cell.CellEvent = new ChildFieldEvent(root, RadioField, 0);
                                    RadioTable.AddCell(Cell);
                                }
                                ExportWriter.AddAnnotation(RadioGroup);
                                Table.AddCell(RadioTable);
                                break;
                            case "checkbox":
                                Table.AddCell(new Phrase(Question.TEXT + ":", TableFont));
                                PdfFormField CheckGroup = PdfFormField.CreateRadioButton(ExportWriter, true);
                                CheckGroup.FieldName = "field" + Question.QUESTION_ID;
                                using (Entities _context = new Entities())
                                {
                                    QuestionOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).ToList();
                                }
                                PdfPTable CheckTable = new PdfPTable(QuestionOptions.Count * 2);
                                CheckTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                                RadioCheckField Check;
                                PdfFormField CheckField = null;

                                foreach (CUSTOMER_SURVEY_OPTIONS Option in QuestionOptions)
                                {
                                    var root = PdfFormField.CreateEmpty(ExportWriter);
                                    root.FieldName = "root" + Option.OPTION_ID.ToString();

                                    Check = new RadioCheckField(ExportWriter, new Rectangle(40, 806 - count * 40, 60, 788 - count * 40), Option.OPTION_NAME, "option" + Option.OPTION_ID.ToString());
                                    Check.BackgroundColor = new GrayColor(0.8f);
                                    Check.CheckType = RadioCheckField.TYPE_CROSS;
                                    CheckField = Check.CheckField;
                                    CheckGroup.AddKid(CheckField);
                                    var Widths = new int[QuestionOptions.Count * 2];
                                    for (int i = 0; i < QuestionOptions.Count * 2; i++)
                                    {
                                        if (i % 2 == 1)
                                        {
                                            Widths[i] = 5;
                                        }
                                        else
                                        {
                                            Widths[i] = 20;
                                        }
                                    }
                                    CheckTable.SetWidths(Widths);

                                    CheckTable.AddCell(new Phrase(Option.OPTION_NAME + ":", TableFont));

                                    Cell = new PdfPCell();
                                    Cell.Border = PdfPCell.NO_BORDER;

                                    Cell.CellEvent = new ChildFieldEvent(root, CheckField, 0);
                                    CheckTable.AddCell(Cell);
                                }
                                ExportWriter.AddAnnotation(CheckGroup);
                                Table.AddCell(CheckTable);
                                break;
                        }
                        FieldsetTable.AddCell(Table);
                    }

                    
                    ExportedPDF.Add(FieldsetTable);
                }
                
                ExportedPDF.Close();
                result = PdfStream.GetBuffer();
            }
            return result;
        }

        protected void deEmailPDF(object sender, DirectEventArgs e)
        {
            List<XXDBI_DW.Threshold> RowData = JSON.Deserialize<List<XXDBI_DW.Threshold>>(e.ExtraParams["RowValues"]);
            decimal FormId;
            string ToAddress;
            //Get form for Organization
            using (Entities _context = new Entities())
            {
                FormId = CUSTOMER_SURVEYS.GetFormIdByOrg(RowData[0].ORG_ID, _context);
                ToAddress = CUSTOMER_SURVEYS.GetProjectContacts(RowData[0].PROJECT_ID, _context).Select(x => x.EMAIL_ADDRESS).SingleOrDefault();
            }

            //generate code to tie back to customer
            CUSTOMER_SURVEY_FORMS_COMP NewFormToSubmit = new CUSTOMER_SURVEY_FORMS_COMP();
            
            NewFormToSubmit.FORM_ID = FormId;
            NewFormToSubmit.PROJECT_ID = RowData[0].PROJECT_ID;
            NewFormToSubmit.THRESHOLD_ID = RowData[0].THRESHOLD_ID;
            NewFormToSubmit.CREATE_DATE = DateTime.Now;
            NewFormToSubmit.MODIFY_DATE = DateTime.Now;
            NewFormToSubmit.CREATED_BY = User.Identity.Name;
            NewFormToSubmit.MODIFIED_BY = User.Identity.Name;
            
            GenericData.Insert<CUSTOMER_SURVEY_FORMS_COMP>(NewFormToSubmit);

            if (ToAddress != null)
            {
                //Get questions
                byte[] PdfStream = generatePDF(FormId, NewFormToSubmit.COMPLETION_ID);

                string Subject = "Customer Satisfaction Survey";
                string Message = "Please find attached the following Customer satisfaction survey.";


                Attachment MailAttachment = new Attachment(new MemoryStream(PdfStream), "customer-satisfaction-survey.pdf");
                var smtp = new SmtpClient("owa.dbiservices.com");
#if DEBUG
                smtp.Credentials = new System.Net.NetworkCredential("gene.lapointe@dbiservices.com", "Monkey1!");
#endif
                MailMessage EmailMessage = new MailMessage(User.Identity.Name + "@dbiservices.com", User.Identity.Name + "@dbiservices.com", Subject, Message);
                EmailMessage.Attachments.Add(MailAttachment);
                EmailMessage.IsBodyHtml = true;
                //smtp.SendMessage(User.Identity.Name + "@dbiservices.com", Subject, Message, IsHtml, MailAttachment);
                smtp.Send(EmailMessage);
                X.Msg.Alert("Email sent", string.Format("Message has been sent to {0}", ToAddress)).Show();
            }
            else
            {
                X.Msg.Alert("No Email Address", "The selected project does not have an associated email address").Show();
            }
        }

        public static iTextSharp.text.Font GetTahoma()
        {
            var fontName = "Tahoma";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        protected void deReadProjects(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                int count;
                uxProjectsStore.DataSource = GenericData.ListFilterHeader(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], PA.GetProjectsByOrg(OrgsList, _context), out count);
                e.Total = count;
            }
        }
        protected void deLoadPanel(object sender, DirectEventArgs e)
        {
            //Get Form Id
            long OrgId = long.Parse(e.ExtraParams["OrgId"]);
            decimal FormId;
            using (Entities _context = new Entities())
            {
                FormId = CUSTOMER_SURVEYS.GetFormIdByOrg(OrgId, _context);
            }
            CUSTOMER_SURVEY_FORMS_COMP NewComp =  new CUSTOMER_SURVEY_FORMS_COMP();
            NewComp.FORM_ID = FormId;
            NewComp.CREATE_DATE = DateTime.Now;
            NewComp.CREATED_BY = User.Identity.Name;
            NewComp.MODIFIED_BY = User.Identity.Name;
            NewComp.MODIFY_DATE = DateTime.Now;
            NewComp.PROJECT_ID = long.Parse(e.ExtraParams["ProjectId"]);

            GenericData.Insert<CUSTOMER_SURVEY_FORMS_COMP>(NewComp);
            Ext.Net.Panel uxSurveyPanel = new Ext.Net.Panel();
            uxSurveyPanel.ID = "uxSurveyPanel";
            uxSurveyPanel.Layout = "Fit";
            uxSurveyPanel.Closable = true;
            uxSurveyPanel.Title = "Preview Survey";
            uxSurveyPanel.Loader = new ComponentLoader
            {
                Url = "/Views/Modules/CustomerSurveys/umViewSurvey.aspx?FormId=" + FormId  + "&CompletionId="+ NewComp.COMPLETION_ID,
                Mode = LoadMode.Frame,
                AutoLoad = true
            };

            uxSurveyPanel.AddTo(this.uxTabPanel);
            uxTabPanel.SetActiveTab(uxSurveyPanel);
            uxChooseProjectWindow.Hide();
        }
        protected Window CreateWindow()
        {
            return new Window()
            {
                ID = "uxPlaceholderWindow",

                Modal = true,
                Resizable = false,
                AutoRender = false,
                Y = 15,
                Constrain = false,
                CloseAction = CloseAction.Destroy,
                
            };
        }
    }

    public class ChildFieldEvent : IPdfPCellEvent
    {
        /** A parent field to which a child field has to be added. */
        protected PdfFormField parent;
        /** The child field that has to be added */
        protected PdfFormField kid;
        /** The padding of the field inside the cell */
        protected float padding;

        /**
         * Creates a ChildFieldEvent.
         * @param parent the parent field
         * @param kid the child field
         * @param padding a padding
         */
        public ChildFieldEvent(PdfFormField parent, PdfFormField kid, float padding)
        {
            this.parent = parent;
            this.kid = kid;
            this.padding = padding;
        }

        /**
         * Add the child field to the parent, and sets the coordinates of the child field.
         */
        public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] cb)
        {
            parent.AddKid(kid);
            kid.SetWidget(new iTextSharp.text.Rectangle(
                rect.GetLeft(padding),
                rect.GetBottom(padding),
                rect.GetRight(padding),
                rect.GetTop(padding)
                ),
                PdfAnnotation.HIGHLIGHT_INVERT
                );
        }


    }
}