﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umCustomerSurveyAdmin : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void deReadForms(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<CUSTOMER_SURVEY_FORMS> FormData = _context.CUSTOMER_SURVEY_FORMS.ToList();
                List<CustomerSurveyForms> AllData = new List<CustomerSurveyForms>();
                foreach (CUSTOMER_SURVEY_FORMS ThisForm in FormData)
                {
                    CustomerSurveyForms NewForm = new CustomerSurveyForms();
                    NewForm.FORMS_NAME = ThisForm.FORMS_NAME;
                    NewForm.ORG_ID = ThisForm.ORG_ID;
                    NewForm.FORM_ID = ThisForm.FORM_ID;
                    var NumQuestions = (from f in _context.CUSTOMER_SURVEY_FORMS
                                        join fs in _context.CUSTOMER_SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                                        join r in _context.CUSTOMER_SURVEY_RELATION on fs.FIELDSET_ID equals r.FIELDSET_ID
                                        join q in _context.CUSTOMER_SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                                        where f.FORM_ID == ThisForm.FORM_ID
                                        select q.QUESTION_ID).Count();
                                       
                    NewForm.NUM_QUESTIONS = NumQuestions;
                    AllData.Add(NewForm);
                }
                int count;
                uxFormsStore.DataSource = GenericData.EnumerableFilterHeader<CustomerSurveyForms>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], AllData, out count);
                e.Total = count;
            }
        }

        protected void deReadQuestions(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long FormId = -1;
                if (e.Parameters["FormId"] != string.Empty)
                {
                    FormId = long.Parse(e.Parameters["FormId"]);
                }

                List<CustomerSurveyQuestions> data = (from q in _context.CUSTOMER_SURVEY_QUESTIONS
                                                      join fsr in _context.CUSTOMER_SURVEY_RELATION on q.QUESTION_ID equals fsr.QUESTION_ID
                                                      join fs in _context.CUSTOMER_SURVEY_FIELDSETS on fsr.FIELDSET_ID equals fs.FIELDSET_ID
                                                      join qt in _context.CUSTOMER_SURVEY_QUES_TYPES on q.TYPE_ID equals qt.TYPE_ID
                                                      where fs.FORM_ID == FormId
                                                      select new CustomerSurveyQuestions { QUESTION_ID = q.QUESTION_ID, TEXT = q.TEXT, TYPE_ID = qt.TYPE_ID, QUESTION_TYPE_NAME = qt.QUESTION_TYPE_NAME, IS_REQUIRED = (q.IS_REQUIRED == "Y" ? true : false), FIELDSET_ID = fs.FIELDSET_ID, TITLE = fs.TITLE }).ToList();
                int count;
                uxQuestionsStore.DataSource = GenericData.EnumerableFilterHeader<CustomerSurveyQuestions>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadOptions(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                decimal QuestionId = decimal.Parse(e.Parameters["QuestionId"]);
                List<CustomerSurveyOptions> data = (from o in _context.CUSTOMER_SURVEY_OPTIONS
                                                    join q in _context.CUSTOMER_SURVEY_QUESTIONS on o.QUESTION_ID equals q.QUESTION_ID
                                                    where q.QUESTION_ID == QuestionId
                                                    select new CustomerSurveyOptions { OPTION_ID = o.OPTION_ID, QUESTION_ID = o.QUESTION_ID, TEXT = q.TEXT, OPTION_NAME = o.OPTION_NAME, SORT_ORDER = o.SORT_ORDER }).ToList();
                int count;
                uxOptionsStore.DataSource = GenericData.EnumerableFilterHeader<CustomerSurveyOptions>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadFieldsets(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long FormId = long.Parse(e.Parameters["FormId"]);
                List<CUSTOMER_SURVEY_FIELDSETS> data = _context.CUSTOMER_SURVEY_FIELDSETS.Where(x => x.FORM_ID == FormId).ToList();
                int count;
                uxFieldsetsStore.DataSource = GenericData.EnumerableFilterHeader<CUSTOMER_SURVEY_FIELDSETS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadQuestionFieldsets(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long FormId = long.Parse(e.Parameters["FormId"]);
                uxQuestionFieldsetStore.DataSource = _context.CUSTOMER_SURVEY_FIELDSETS.Where(x => x.FORM_ID == FormId).ToList();
                
            }
        }

        protected void deReadOrgs(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxAddFormOrgStore.DataSource = _context.ORG_HIER_V.Distinct().ToList();
            }
        }

        protected void deReadQuestionTypes(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxQuestionTypeStore.DataSource = _context.CUSTOMER_SURVEY_QUES_TYPES.ToList();
            }
        }

        protected void deSaveForms(object sender, DirectEventArgs e)
        {
            ChangeRecords<CustomerSurveyForms> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CustomerSurveyForms>();
            foreach (CustomerSurveyForms CreatedForm in data.Created)
            {
                CUSTOMER_SURVEY_FORMS NewForm = new CUSTOMER_SURVEY_FORMS();
                NewForm.FORMS_NAME = CreatedForm.FORMS_NAME;
                NewForm.ORG_ID = CreatedForm.ORG_ID;
                NewForm.CREATE_DATE = DateTime.Now;
                NewForm.MODIFY_DATE = DateTime.Now;
                NewForm.CREATED_BY = User.Identity.Name;
                NewForm.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert<CUSTOMER_SURVEY_FORMS>(NewForm);

            }

            foreach (CustomerSurveyForms UpdatedForm in data.Updated)
            {
                CUSTOMER_SURVEY_FORMS ToBeUpdated;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = _context.CUSTOMER_SURVEY_FORMS.Where(x => x.FORM_ID == UpdatedForm.FORM_ID).Single();
                }
                ToBeUpdated.FORMS_NAME = UpdatedForm.FORMS_NAME;
                ToBeUpdated.ORG_ID = UpdatedForm.ORG_ID;
                ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                ToBeUpdated.MODIFY_DATE = DateTime.Now;

                GenericData.Update<CUSTOMER_SURVEY_FORMS>(ToBeUpdated);
            }

            foreach (CustomerSurveyForms DeletedForm in data.Deleted)
            {
                
            }
            uxFormsStore.CommitChanges();
        }

        protected void deSaveFieldsets(object sender, DirectEventArgs e)
        {
            ChangeRecords <CUSTOMER_SURVEY_FIELDSETS> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEY_FIELDSETS>();

            foreach (CUSTOMER_SURVEY_FIELDSETS NewFieldset in data.Created)
            {
                NewFieldset.CREATE_DATE = DateTime.Now;
                NewFieldset.MODIFY_DATE = DateTime.Now;
                NewFieldset.CREATED_BY = User.Identity.Name;
                NewFieldset.MODIFIED_BY = User.Identity.Name;
                NewFieldset.FORM_ID = decimal.Parse(e.ExtraParams["FormId"]);

                GenericData.Insert<CUSTOMER_SURVEY_FIELDSETS>(NewFieldset);
            }

            foreach (CUSTOMER_SURVEY_FIELDSETS UpdatedFieldset in data.Updated)
            {
                UpdatedFieldset.FORM_ID = decimal.Parse(e.ExtraParams["FormId"]);
                UpdatedFieldset.MODIFIED_BY = User.Identity.Name;
                UpdatedFieldset.MODIFY_DATE = DateTime.Now;
                GenericData.Update<CUSTOMER_SURVEY_FIELDSETS>(UpdatedFieldset);
            }

            uxFieldsetsStore.CommitChanges();
            uxQuestionFieldsetStore.Reload();
        }

        protected void deSaveQuestions(object sender, DirectEventArgs e)
        {
            ChangeRecords<CustomerSurveyQuestions> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CustomerSurveyQuestions>();

            foreach (CustomerSurveyQuestions NewQuestion in data.Created)
            {
                CUSTOMER_SURVEY_QUESTIONS ToBeAdded = new CUSTOMER_SURVEY_QUESTIONS();
                ToBeAdded.TEXT = NewQuestion.TEXT;
                ToBeAdded.TYPE_ID = NewQuestion.TYPE_ID;
                ToBeAdded.CREATED_BY = User.Identity.Name;
                ToBeAdded.CREATE_DATE = DateTime.Now;
                ToBeAdded.MODIFIED_BY = User.Identity.Name;
                ToBeAdded.MODIFY_DATE = DateTime.Now;
                ToBeAdded.IS_ACTIVE = "Y";
                ToBeAdded.IS_REQUIRED = (NewQuestion.IS_REQUIRED == true ? "Y" : "N");
                GenericData.Insert<CUSTOMER_SURVEY_QUESTIONS>(ToBeAdded);

                CUSTOMER_SURVEY_RELATION FieldsetToAdd = new CUSTOMER_SURVEY_RELATION();
                FieldsetToAdd.QUESTION_ID = ToBeAdded.QUESTION_ID;
                FieldsetToAdd.FIELDSET_ID = NewQuestion.FIELDSET_ID;
                FieldsetToAdd.MODIFIED_BY = User.Identity.Name;
                FieldsetToAdd.CREATED_BY = User.Identity.Name;
                FieldsetToAdd.MODIFY_DATE = DateTime.Now;
                FieldsetToAdd.CREATE_DATE = DateTime.Now;
                GenericData.Insert<CUSTOMER_SURVEY_RELATION>(FieldsetToAdd);

                if(ToBeAdded.TYPE_ID == 5 || ToBeAdded.TYPE_ID == 6 || ToBeAdded.TYPE_ID == 7)
                {
                    uxAddOptionButton.Disabled = false;
                    uxSaveOptionButton.Disabled = false;
                }
            }

            foreach (CustomerSurveyQuestions UpdatedQuestion in data.Updated)
            {
                CUSTOMER_SURVEY_QUESTIONS ToBeUpdated;
                CUSTOMER_SURVEY_RELATION FieldsetToUpdate;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = _context.CUSTOMER_SURVEY_QUESTIONS.Where(x => x.QUESTION_ID == UpdatedQuestion.QUESTION_ID).Single();
                    ToBeUpdated.IS_REQUIRED = (UpdatedQuestion.IS_REQUIRED == true ? "Y" : "N");
                    ToBeUpdated.TEXT = UpdatedQuestion.TEXT;
                    ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                    ToBeUpdated.MODIFY_DATE = DateTime.Now;
                    ToBeUpdated.TYPE_ID = UpdatedQuestion.TYPE_ID;
                    ToBeUpdated.IS_ACTIVE = "Y";

                    FieldsetToUpdate = _context.CUSTOMER_SURVEY_RELATION.Where(x => x.QUESTION_ID == UpdatedQuestion.QUESTION_ID).Single();
                    FieldsetToUpdate.FIELDSET_ID = UpdatedQuestion.FIELDSET_ID;
                    FieldsetToUpdate.MODIFY_DATE = DateTime.Now;
                    FieldsetToUpdate.MODIFIED_BY = User.Identity.Name;
                }
                GenericData.Update<CUSTOMER_SURVEY_QUESTIONS>(ToBeUpdated);
                GenericData.Update<CUSTOMER_SURVEY_RELATION>(FieldsetToUpdate);

                if (ToBeUpdated.TYPE_ID == 5 || ToBeUpdated.TYPE_ID == 6 || ToBeUpdated.TYPE_ID == 7)
                {
                    uxAddOptionButton.Disabled = false;
                    uxAddOptionButton.Disabled = false;
                }
            }
            uxQuestionsStore.CommitChanges();
        }

        protected void deSaveOptions(object sender, DirectEventArgs e)
        {
            ChangeRecords<CustomerSurveyOptions> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CustomerSurveyOptions>();
            foreach (CustomerSurveyOptions NewOption in data.Created)
            {
                CUSTOMER_SURVEY_OPTIONS ToBeAdded = new CUSTOMER_SURVEY_OPTIONS();
                ToBeAdded.QUESTION_ID = decimal.Parse(e.ExtraParams["QuestionId"]);
                ToBeAdded.OPTION_NAME = NewOption.OPTION_NAME;
                ToBeAdded.SORT_ORDER = NewOption.SORT_ORDER;
                ToBeAdded.CREATE_DATE = DateTime.Now;
                ToBeAdded.MODIFY_DATE = DateTime.Now;
                ToBeAdded.CREATED_BY = User.Identity.Name;
                ToBeAdded.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert<CUSTOMER_SURVEY_OPTIONS>(ToBeAdded);
            }
            foreach (CustomerSurveyOptions UpdatedOption in data.Updated)
            {
                CUSTOMER_SURVEY_OPTIONS ToUpdate;
                using (Entities _context = new Entities())
                {
                    ToUpdate = _context.CUSTOMER_SURVEY_OPTIONS.Where(x => x.OPTION_ID == UpdatedOption.OPTION_ID).Single();
                    ToUpdate.OPTION_NAME = ToUpdate.OPTION_NAME;
                    ToUpdate.MODIFIED_BY = User.Identity.Name;
                    ToUpdate.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Update<CUSTOMER_SURVEY_OPTIONS>(ToUpdate);
            }
            uxOptionsStore.CommitChanges();
        }

        protected void deLoadOptions(object sender, DirectEventArgs e)
        {
            int TypeId = int.Parse(e.ExtraParams["QuestionType"]);
            if (TypeId == 5 || TypeId == 6 || TypeId == 7)
            {
                uxAddOptionButton.Disabled = false;
                uxSaveOptionButton.Disabled = false;
            }
            else
            {
                uxAddOptionButton.Disabled = true;
                uxSaveOptionButton.Disabled = true;
            }
            
            uxOptionsStore.Reload();
        }

        protected void deLoadFormDetails(object sender, DirectEventArgs e)
        {
            uxQuestionFieldsetStore.Reload();
            uxQuestionsStore.Reload();
            uxFieldsetsStore.Reload();
            uxAddFieldsetButton.Disabled = false;
            uxAddQuestionButton.Disabled = false;
            uxSaveFieldsetButton.Disabled = false;
            uxSaveQuestionButton.Disabled = false;
            uxOptionsStore.RemoveAll();
        }

        protected void deViewForm(object sender, DirectEventArgs e)
        {
            Ext.Net.Panel uxSurveyPanel = new Ext.Net.Panel();
            uxSurveyPanel.ID = "uxSurveyPanel";
            uxSurveyPanel.Layout = "Fit";
            uxSurveyPanel.Closable = true;
            uxSurveyPanel.Title = "Preview Survey";
            uxSurveyPanel.Loader = new ComponentLoader
            {
                Url = "/Views/Modules/CustomerSurveys/umViewSurvey.aspx?FormId=" + e.ExtraParams["FormId"],
                Mode = LoadMode.Frame,
                AutoLoad = true
            };
            
            uxSurveyPanel.AddTo(this.uxTabPanel);
            uxTabPanel.SetActiveTab(uxSurveyPanel);
        }

        //protected void CreateWindow(string LoaderUrl)
        //{
        //    Window win = new Window()
        //    {
        //        ID = "uxPlaceholderWindow",
        //        Title = "View Survey",
        //        Width = 600,
        //        Modal = true,
        //        Resizable = false,
        //        AutoRender = false,
        //        Y = 15,
        //        Constrain = false,
        //        CloseAction = CloseAction.Destroy,
        //        Loader = new ComponentLoader
        //        {
        //            Url = LoaderUrl,
        //            DisableCaching = true,
        //            Mode = LoadMode.Frame,
        //            AutoLoad = true,
        //            LoadMask =
        //            {
        //                ShowMask = true
        //            }
        //        }
        //    };

        //    this.Form.Controls.Add(win);
        //    win.Render(this.Form);
        //    win.Show();
        //}

    }

    public class CustomerSurveyForms
    {
        public decimal FORM_ID { get; set; }
        public string FORMS_NAME { get; set; }
        public int NUM_QUESTIONS { get; set; }
        public decimal ORG_ID { get; set; }
    }

    public class CustomerSurveyQuestions
    {
        public decimal QUESTION_ID { get; set; }
        public string TEXT { get; set; }
        public decimal TYPE_ID {get; set; }
        public string QUESTION_TYPE_NAME { get; set; }
        public decimal FIELDSET_ID { get; set; }
        public string TITLE { get; set; }
        public bool IS_REQUIRED { get; set; }
        public int SORT_ORDER { get; set; }
    }

    public class CustomerSurveyOptions
    {
        public decimal OPTION_ID { get; set; }
        public string OPTION_NAME { get; set; }
        public decimal QUESTION_ID { get; set; }
        public string TEXT { get; set; }
        public decimal SORT_ORDER { get; set; }
        public bool IS_ACTIVE { get; set; }
    }
}