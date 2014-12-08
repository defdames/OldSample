using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;
using System.IO;
using OfficeOpenXml;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umViewCompleted : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CustomerSurveys.ViewCompleted"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest || !IsPostBack)
            {
                uxStartDate.MaxDate = DateTime.Now;
                uxEndDate.MaxDate = DateTime.Now;
                uxStartDate.SelectedDate = DateTime.Now;
                uxEndDate.SelectedDate = DateTime.Now;
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

        protected void deReadCompletions(object sender, StoreReadDataEventArgs e)
        {
            IQueryable<CUSTOMER_SURVEYS.CustomerSurveyCompletions> Completions;
            using (Entities _context = new Entities())
            {
                long ProjectId = long.Parse(e.Parameters["ProjectId"]);
                Completions = CUSTOMER_SURVEYS.GetCompletionStore(ProjectId, _context);
                int count;
                var data = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyCompletions>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], Completions, out count);
                uxCompletedStore.DataSource = data;
                e.Total = count;
            }
        }

        protected void deReadProjects(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            List<long> OrgsList;
            if (_selectedRecordID != string.Empty)
            {
                long SelectedOrgId = GetOrgFromTree(_selectedRecordID);
                long HierId = GetHierarchyFromTree(_selectedRecordID);
                using (Entities _context = new Entities())
                {
                    OrgsList = HR.ActiveOrganizationsByHierarchy(HierId, SelectedOrgId, _context).Select(x => x.ORGANIZATION_ID).ToList();
                    IQueryable<PROJECTS_V> ProjectList = PA.GetProjectsByOrg(OrgsList, _context);
                    int count;
                    var data = GenericData.ListFilterHeader<PROJECTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], ProjectList, out count);
                    uxProjectStore.DataSource = data;
                    e.Total = count;
                }
            }
        }

        protected void deLoadForm(object sender, DirectEventArgs e)
        {
            decimal CompletionId = decimal.Parse(e.ExtraParams["CompletionId"]);
            decimal FormId;
            using (Entities _context = new Entities())
            {
                FormId = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.COMPLETION_ID == CompletionId).Select(x => x.FORM_ID).Single();
            }
            //LoadForm(FormId, CompletionId);
            uxCompletedSurveyPanel.LoadContent(string.Format("umViewSurvey.aspx?CompletionId={0}&FormId={1}", CompletionId, FormId));
            
        }

        protected void ValidateDate(object sender, RemoteValidationEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;

            DateTime EndDate = uxEndDate.SelectedDate;

            if (StartDate > EndDate)
            {
                e.Success = false;
                e.ErrorMessage = "End Date and Time must be later than Start Date and Time";
                uxStartDate.MarkInvalid();
                uxEndDate.MarkInvalid();
                
            }
            else if (StartDate > DateTime.Now.Date || EndDate > DateTime.Now.Date)
            {
                e.Success = false;
                e.ErrorMessage = "Date Cannot be later than today";
                uxStartDate.MarkInvalid();
                uxEndDate.MarkInvalid();
            }
            else
            {
                e.Success = true;
                uxStartDate.ClearInvalid();
                uxStartDate.MarkAsValid();
                uxEndDate.ClearInvalid();
                uxEndDate.MarkAsValid();
                
            }
        }
        protected void deExportSurveys(object sender, DirectEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            List<long> OrgsList;
            if (_selectedRecordID != string.Empty)
            {
                long SelectedOrgId = GetOrgFromTree(_selectedRecordID);
                long HierId = GetHierarchyFromTree(_selectedRecordID);
                
                string _filename = "SurveyExport.xlsx";
                string _filePath = Request.PhysicalApplicationPath + _filename;

                using (Entities _context = new Entities())
                {
                    FileInfo newFile = new FileInfo(_filePath + _filename);

                    ExcelPackage pck = new ExcelPackage(newFile);

                    OrgsList = HR.ActiveOrganizationsByHierarchy(HierId, SelectedOrgId, _context).Select(x => x.ORGANIZATION_ID).ToList();

                    List<SURVEY_FORMS> FormsList = CUSTOMER_SURVEYS.GetForms(_context).Where(x => OrgsList.Contains((long)x.ORG_ID)).ToList();
                    
                    
                    foreach (SURVEY_FORMS FormEntry in FormsList)
                    {
                        string OrgName = _context.ORG_HIER_V.Where(x => x.ORG_ID == FormEntry.ORG_ID).Select(x => x.ORG_HIER).Distinct().Single();
                        
                        List<SURVEY_QUESTIONS> FormQuestions = CUSTOMER_SURVEYS.GetFormQuestion2(FormEntry.FORM_ID, _context).OrderBy(x => x.QUESTION_ID).ToList();
                        List<SURVEY_FORMS_COMP> Completions = CUSTOMER_SURVEYS.GetCompletionsByDate(StartDate, EndDate, FormEntry.FORM_ID, _context).ToList();
                        ExcelWorksheet ws;
                        if (Completions.Count > 0)
                        {
                            ws = pck.Workbook.Worksheets.Add(OrgName);
                            char letter = 'B';
                            int rownumber = 2;
                            ws.Cells["A1"].Value = "Project Name";
                            foreach (SURVEY_QUESTIONS FormQuestion in FormQuestions)
                            {
                                ws.Cells[letter + "1"].Value = FormQuestion.TEXT;
                                ws.Cells[letter + "1"].Style.Font.Size = 12f;
                                letter = GetNextLetter(letter);
                            }


                            foreach (SURVEY_FORMS_COMP Completion in Completions)
                            {
                                letter = 'B';
                                string ProjectName = _context.PROJECTS_V.Where(x => x.PROJECT_ID == Completion.PROJECT_ID).Select(x => x.LONG_NAME).Single();

                                //Get Answers
                                List<SURVEY_FORMS_ANS> Answers = CUSTOMER_SURVEYS.GetFormAnswersByCompletion(Completion.COMPLETION_ID, _context).OrderBy(x => x.QUESTION_ID).ToList();
                                if (Answers.Count > 0)
                                {
                                    ws.Cells["A" + rownumber].Value = ProjectName;
                                }
                                foreach (SURVEY_FORMS_ANS Answer in Answers)
                                {
                                    ws.Cells[letter + rownumber.ToString()].Value = Answer.ANSWER;
                                    letter = GetNextLetter(letter);
                                }
                                rownumber++;
                            }
                            ws.Column(1).AutoFit(0);
                        }
                        
                    }
                    Byte[] bin = pck.GetAsByteArray();
                    File.WriteAllBytes(_filePath, bin);
                    uxExportWindow.Hide();
                    uxDatesForm.Reset();

                    X.Msg.Confirm("File Download", "Your exported file is now ready to download.", new MessageBoxButtonsConfig
                    {
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.Download('" + _filename + "','" + Server.UrlEncode(_filePath) + "', { isUpload : true })",
                            Text = "Download " + _filename
                        }
                    }).Show();
                    //using (FileStream fileStream = File.OpenRead(Server.UrlDecode(_filePath)))
                    //{

                    //    //create new MemoryStream object
                    //    MemoryStream memStream = new MemoryStream();
                    //    memStream.SetLength(fileStream.Length);
                    //    //read file to MemoryStream
                    //    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    //    Response.Clear();
                    //    Response.ClearContent();
                    //    Response.ClearHeaders();
                    //    Response.ContentType = "application/octet-stream";
                    //    Response.AppendHeader("Content-Disposition", "attachment;filename=" + _filename);
                    //    Response.BinaryWrite(memStream.ToArray());
                    //    Response.End();
                    //}
                }
                
            }

        }

        [DirectMethod]
        public void Download(string filename, string filePath)
        {
            using (FileStream fileStream = File.OpenRead(Server.UrlDecode(filePath)))
            {

                //create new MemoryStream object
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                //read file to MemoryStream
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.BinaryWrite(memStream.ToArray());
                Response.End();
            }
        }
        protected char GetNextLetter(char letter)
        {
            char nextChar;
            if (letter == 'z')
            {
                nextChar = 'a';
            }
            else if (letter == 'Z')
            {
                nextChar = 'A';
            }
            else
            {
                nextChar = (char)(((int)letter) + 1);
            }
            return nextChar;
        }
    }
}