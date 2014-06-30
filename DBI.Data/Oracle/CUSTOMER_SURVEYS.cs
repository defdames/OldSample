using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class CUSTOMER_SURVEYS
    {
        public static IQueryable<CUSTOMER_SURVEY_FORMS> GetForms(Entities _context)
        {
            return _context.CUSTOMER_SURVEY_FORMS;
        }

        public static int NumberOfQuestionsInForm(decimal FormId, Entities _context)
        {
            return (from f in _context.CUSTOMER_SURVEY_FORMS
                    join fs in _context.CUSTOMER_SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                    join r in _context.CUSTOMER_SURVEY_RELATION on fs.FIELDSET_ID equals r.FIELDSET_ID
                    join q in _context.CUSTOMER_SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                    where f.FORM_ID == FormId
                    select q.QUESTION_ID).Count();
        }

        public static IQueryable<CustomerSurveyFieldsets> GetFormFieldSets(decimal FormId, Entities _context)
        {
            return (from f in _context.CUSTOMER_SURVEY_FIELDSETS
                    orderby f.SORT_ORDER ascending
                    where f.FORM_ID == FormId
                    select new CustomerSurveyFieldsets { FIELDSET_ID = f.FIELDSET_ID, FORM_ID = f.FORM_ID, IS_ACTIVE = (f.IS_ACTIVE == "Y" ? true : false), SORT_ORDER = f.SORT_ORDER, TITLE = f.TITLE });
        }

        public static IQueryable<CustomerSurveyQuestions> GetFieldsetQuestions(decimal FieldSetId, Entities _context)
        {
            return (from q in _context.CUSTOMER_SURVEY_RELATION
                    orderby q.SORT_ORDER ascending
                    where q.FIELDSET_ID == FieldSetId
                    select new CustomerSurveyQuestions { QUESTION_ID = q.CUSTOMER_SURVEY_QUESTIONS.QUESTION_ID, TEXT = q.CUSTOMER_SURVEY_QUESTIONS.TEXT, IS_ACTIVE = (q.CUSTOMER_SURVEY_QUESTIONS.IS_ACTIVE == "Y" ? true : false), IS_REQUIRED = (q.CUSTOMER_SURVEY_QUESTIONS.IS_REQUIRED == "Y" ? true : false), TYPE_ID = q.CUSTOMER_SURVEY_QUESTIONS.TYPE_ID, SORT_ORDER = q.CUSTOMER_SURVEY_QUESTIONS.SORT_ORDER });
        }

        public static IQueryable<CUSTOMER_SURVEY_OPTIONS> GetQuestionOptions(decimal QuestionId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_OPTIONS.Where(x => x.QUESTION_ID == QuestionId);
        }

        public static IQueryable<CustomerSurveyQuestions> GetFormQuestions(decimal FormId, Entities _context){
            return (from f in _context.CUSTOMER_SURVEY_FORMS
                    join fs in _context.CUSTOMER_SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                    join fsr in _context.CUSTOMER_SURVEY_RELATION on fs.FIELDSET_ID equals fsr.FIELDSET_ID
                    join q in _context.CUSTOMER_SURVEY_QUESTIONS on fsr.QUESTION_ID equals q.QUESTION_ID
                    join qt in _context.CUSTOMER_SURVEY_QUES_TYPES on q.TYPE_ID equals qt.TYPE_ID
                    where fs.FORM_ID == FormId
                    select new CustomerSurveyQuestions { QUESTION_ID = q.QUESTION_ID, TEXT = q.TEXT, TYPE_ID = qt.TYPE_ID, QUESTION_TYPE_NAME = qt.QUESTION_TYPE_NAME, IS_REQUIRED = (q.IS_REQUIRED == "Y" ? true : false), FIELDSET_ID = fs.FIELDSET_ID, TITLE = fs.TITLE, IS_ACTIVE = (q.IS_ACTIVE == "Y" ? true: false) });
        }

        public static IQueryable<CUSTOMER_SURVEY_QUES_TYPES> GetQuestionTypes(Entities _context)
        {
            return _context.CUSTOMER_SURVEY_QUES_TYPES;
        }

        public static IQueryable<CUSTOMER_SURVEY_FIELDSETS> GetFieldsets(Entities _context)
        {
            return _context.CUSTOMER_SURVEY_FIELDSETS;
        }

        public static CUSTOMER_SURVEY_QUESTIONS GetQuestion(decimal QuestionId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_QUESTIONS.Where(x => x.QUESTION_ID == QuestionId).Single();
        }

        public static CUSTOMER_SURVEY_RELATION GetRelationshipEntry(decimal QuestionId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_RELATION.Where(x => x.QUESTION_ID == QuestionId).Single();
        }

        public static CUSTOMER_SURVEY_OPTIONS GetQuestionOption(decimal OptionId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_OPTIONS.Where(x => x.OPTION_ID == OptionId).Single();
        }

        public static IQueryable<CUSTOMER_SURVEY_THRESHOLDS> GetOrganizationThreshold(long OrgID, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_THRESHOLDS.Where(x => x.ORG_ID == OrgID);
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
            public decimal TYPE_ID { get; set; }
            public string QUESTION_TYPE_NAME { get; set; }
            public decimal FIELDSET_ID { get; set; }
            public string TITLE { get; set; }
            public bool IS_REQUIRED { get; set; }
            public decimal? SORT_ORDER { get; set; }
            public bool IS_ACTIVE { get; set; }
        }

        public class CustomerSurveyFieldsets
        {
            public decimal FIELDSET_ID { get; set; }
            public decimal FORM_ID { get; set; }
            public string TITLE { get; set; }
            public decimal? SORT_ORDER { get; set; }
            public bool IS_ACTIVE { get; set; }
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
}
