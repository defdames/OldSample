using System;
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
                    int NumQuestions = (from f in _context.CUSTOMER_SURVEY_FIELDSETS
                                        join r in _context.CUSTOMER_SURVEY_RELATION on f.FIELDSET_ID equals r.FIELDSET_ID
                                        join q in _context.CUSTOMER_SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                                        where f.FORM_ID == ThisForm.FORM_ID
                                        group q by f.FORM_ID into counter
                                        select counter).Count();
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
                                                      join fsr in _context.CUSTOMER_SURVEY_RELATION on q.QUESTION_ID equals fsr.FIELDSET_ID
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
                long QuestionId = long.Parse(e.Parameters["QuestionId"]);
                List<CustomerSurveyOptions> data = (from o in _context.CUSTOMER_SURVEY_OPTIONS
                                                    join q in _context.CUSTOMER_SURVEY_QUESTIONS on o.QUESTION_ID equals q.QUESTION_ID
                                                    where q.QUESTION_ID == QuestionId
                                                    select new CustomerSurveyOptions { TEXT = q.TEXT, OPTION_NAME = o.OPTION_NAME, SORT_ORDER = o.SORT_ORDER }).ToList();
                int count;
                uxOptionsStore.DataSource = GenericData.EnumerableFilterHeader<CustomerSurveyOptions>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadQuestionFieldSets(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxQuestionFieldSetStore.DataSource = _context.CUSTOMER_SURVEY_FIELDSETS.ToList();
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
            uxFormsStore.Reload();
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

            uxFieldsetsStore.Reload();
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

                CUSTOMER_SURVEY_RELATION FieldsetRelationship = new CUSTOMER_SURVEY_RELATION
                {
                    FIELDSET_ID = NewQuestion.FIELDSET_ID,
                    QUESTION_ID = ToBeAdded.QUESTION_ID
                };
                GenericData.Insert<CUSTOMER_SURVEY_RELATION>(FieldsetRelationship);
            }

            foreach (CustomerSurveyQuestions UpdatedQuestion in data.Updated)
            {
                CUSTOMER_SURVEY_QUESTIONS ToBeUpdated;
                CUSTOMER_SURVEY_RELATION FieldsetRelationship;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = _context.CUSTOMER_SURVEY_QUESTIONS.Where(x => x.QUESTION_ID == UpdatedQuestion.QUESTION_ID).Single();
                    ToBeUpdated.IS_REQUIRED = (UpdatedQuestion.IS_REQUIRED == true ? "Y" : "N");
                    ToBeUpdated.TEXT = UpdatedQuestion.TEXT;
                    ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                    ToBeUpdated.MODIFY_DATE = DateTime.Now;
                    ToBeUpdated.TYPE_ID = UpdatedQuestion.TYPE_ID;
                    ToBeUpdated.IS_ACTIVE = "Y";
                    FieldsetRelationship = _context.CUSTOMER_SURVEY_RELATION.Where(x => x.QUESTION_ID == UpdatedQuestion.QUESTION_ID).Single();
                    FieldsetRelationship.FIELDSET_ID = UpdatedQuestion.FIELDSET_ID;
                }
                GenericData.Update<CUSTOMER_SURVEY_QUESTIONS>(ToBeUpdated);
                GenericData.Update<CUSTOMER_SURVEY_RELATION>(FieldsetRelationship);
            }
            uxQuestionsStore.Reload();
        }
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
        public string TEXT { get; set; }
        public string OPTION_NAME { get; set; }
        public decimal SORT_ORDER { get; set; }
        public bool IS_ACTIVE { get; set; }
    }
}