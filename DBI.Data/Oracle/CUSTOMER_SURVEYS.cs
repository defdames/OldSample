using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class CUSTOMER_SURVEYS
    {
        public static IQueryable<SURVEY_FORMS> GetForms(Entities _context)
        {
            return _context.SURVEY_FORMS;
        }

        public static int NumberOfQuestionsInForm(decimal FormId, Entities _context)
        {
            return (from f in _context.SURVEY_FORMS
                    join fs in _context.SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                    join r in _context.SURVEY_RELATION on fs.FIELDSET_ID equals r.FIELDSET_ID
                    join q in _context.SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                    where f.FORM_ID == FormId
                    select q.QUESTION_ID).Count();
        }

        public static IQueryable<SURVEY_FIELDSETS> GetFormFieldSets(decimal FormId, Entities _context)
        {
            return (from f in _context.SURVEY_FIELDSETS
                    orderby f.SORT_ORDER ascending
                    where f.FORM_ID == FormId
                    select f);
        }

        public static IQueryable<SURVEY_QUESTIONS> GetFieldsetQuestionsForGrid(decimal FieldSetId, Entities _context)
        {
            return (from q in _context.SURVEY_RELATION
                    where q.FIELDSET_ID == FieldSetId
                    select q.SURVEY_QUESTIONS);
        }

        public static IQueryable<SURVEY_QUESTIONS> GetFieldsetQuestions(decimal FieldsetId, Entities _context)
        {
            return (from q in _context.SURVEY_RELATION
                    where q.FIELDSET_ID == FieldsetId
                    select q.SURVEY_QUESTIONS);
        }

        public static IQueryable<SURVEY_OPTIONS> GetQuestionOptions(decimal QuestionId, Entities _context)
        {
            return _context.SURVEY_OPTIONS.Where(x => x.QUESTION_ID == QuestionId);
        }

        public static IQueryable<SURVEY_QUESTIONS> GetFormQuestions(decimal FormId, Entities _context){
            return (from f in _context.SURVEY_FORMS
                    join fs in _context.SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                    join fsr in _context.SURVEY_RELATION on fs.FIELDSET_ID equals fsr.FIELDSET_ID
                    join q in _context.SURVEY_QUESTIONS on fsr.QUESTION_ID equals q.QUESTION_ID
                    join qt in _context.SURVEY_QUES_TYPES on q.TYPE_ID equals qt.TYPE_ID
                    where fs.FORM_ID == FormId
                    select q);
        }

        public static IQueryable<SURVEY_QUESTIONS> GetFormQuestion2(decimal FormId, Entities _context)
        {
            return (from f in _context.SURVEY_FORMS
                    join fs in _context.SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                    join fsr in _context.SURVEY_RELATION on fs.FIELDSET_ID equals fsr.FIELDSET_ID
                    join q in _context.SURVEY_QUESTIONS on fsr.QUESTION_ID equals q.QUESTION_ID
                    where fs.FORM_ID == FormId
                    select q);
        }

        public static IQueryable<SURVEY_QUES_TYPES> GetQuestionTypes(Entities _context)
        {
            return _context.SURVEY_QUES_TYPES;
        }

        public static IQueryable<SURVEY_FIELDSETS> GetFieldsets(Entities _context)
        {
            return _context.SURVEY_FIELDSETS;
        }

        public static SURVEY_QUESTIONS GetQuestion(decimal QuestionId, Entities _context)
        {
            return _context.SURVEY_QUESTIONS.Where(x => x.QUESTION_ID == QuestionId).Single();
        }

        public static SURVEY_RELATION GetRelationshipEntry(decimal QuestionId, Entities _context)
        {
            return _context.SURVEY_RELATION.Where(x => x.QUESTION_ID == QuestionId).Single();
        }

        public static SURVEY_OPTIONS GetQuestionOption(decimal OptionId, Entities _context)
        {
            return _context.SURVEY_OPTIONS.Where(x => x.OPTION_ID == OptionId).Single();
        }

        public static IQueryable<CUSTOMER_SURVEY_THRESH_AMT> GetOrganizationThresholdAmounts(long OrgID, Entities _context)
        {
            return (from c in _context.CUSTOMER_SURVEY_THRESH_AMT
                    join o in _context.ORG_HIER_V on c.ORG_ID equals o.ORG_ID
                    join f in _context.SURVEY_TYPES on c.TYPE_ID equals f.TYPE_ID
                    where c.ORG_ID == OrgID
                    select c).Distinct();
        }

        public static CUSTOMER_SURVEY_THRESH_AMT GetOrganizationThresholdAmount(decimal AmountId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_THRESH_AMT.Where(x => x.AMOUNT_ID == AmountId).Single();
        }

        public static IQueryable<CUSTOMER_SURVEY_THRESHOLDS> GetThresholdPercentages(decimal AmountID, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_THRESHOLDS.Where(x => x.AMOUNT_ID == AmountID);
        }

        public static CUSTOMER_SURVEY_THRESHOLDS GetThreshold(decimal ThresholdId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_THRESHOLDS.Where(x => x.THRESHOLD_ID == ThresholdId).Single();
        }

        public static decimal GetFormIdByThreshold(decimal OrgId, decimal Threshold, Entities _context)
        {
            return (from d in _context.SURVEY_FORMS
                    join a in _context.CUSTOMER_SURVEY_THRESH_AMT on d.TYPE_ID equals a.TYPE_ID
                    join t in _context.CUSTOMER_SURVEY_THRESHOLDS on a.AMOUNT_ID equals t.AMOUNT_ID
                    where d.ORG_ID == OrgId && t.THRESHOLD_ID == Threshold
                    select d.FORM_ID).Single();
        }
        public static IQueryable<SURVEY_CAT> GetCategories(Entities _context)
        {
            return _context.SURVEY_CAT;
        }

        public static SURVEY_CAT GetCategory(decimal CategoryId, Entities _context)
        {
            return _context.SURVEY_CAT.Where(x => x.CATEGORY_ID == CategoryId).Single();
        }

        public static decimal GetFormIdByOrg(long OrgId, Entities _context)
        {
            return _context.SURVEY_FORMS.Where(x => x.ORG_ID == OrgId).Select(x => x.FORM_ID).SingleOrDefault();
        }

        public static IQueryable<PROJECT_CONTACTS_V> GetProjectContacts(long ProjectId, Entities _context)
        {
            return _context.PROJECT_CONTACTS_V.Where(x => x.CUST_SURVEY_PROJECT_ID == ProjectId && x.CUST_SURVEY == "Y");
        }

        public static IQueryable<SURVEY_FORMS_COMP> GetFormCompletion(Entities _context)
        {
            return _context.SURVEY_FORMS_COMP;
        }

        public static IQueryable<SURVEY_FORMS_ANS> GetFormAnswersByCompletion(decimal CompletionId, Entities _context)
        {
            return _context.SURVEY_FORMS_ANS.Where(x => x.COMPLETION_ID == CompletionId);
        }

        public static IQueryable<SURVEY_FORMS_ANS> GetFormAnswerByQuestion(decimal QuestionId, Entities _context)
        {
            return _context.SURVEY_FORMS_ANS.Where(x => x.QUESTION_ID == QuestionId);
        }

        public static IQueryable<SURVEY_RELATION> GetRelationEntries(Entities _context)
        {
            return _context.SURVEY_RELATION;
        }

        public static bool HasQuestionBeenFilledOut(decimal QuestionId, Entities _context)
        {
            List<SURVEY_FORMS_ANS> Answers = _context.SURVEY_FORMS_ANS.Where(x => x.QUESTION_ID == QuestionId).ToList();
            if (Answers.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static IQueryable<SURVEY_FORMS_COMP> GetCompletionStore(long ProjectId, Entities _context)
        {
            return (from d in _context.SURVEY_FORMS_COMP
                    join f in _context.SURVEY_FORMS on d.FORM_ID equals f.FORM_ID
                    join a in _context.SURVEY_FORMS_ANS on d.COMPLETION_ID equals a.COMPLETION_ID
                    join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                    where d.PROJECT_ID == ProjectId
                    select d).Distinct();

            //new CustomerSurveyCompletions { COMPLETION_ID = d.COMPLETION_ID, FORM_ID = f.FORM_ID, LONG_NAME = p.LONG_NAME, FILLED_BY = d.FILLED_BY, FILLED_ON = d.FILLED_ON, FORMS_NAME = f.FORMS_NAME }
        }

        public static IQueryable<SURVEY_QUES_CAT> GetQuestionCategories(Entities _context)
        {
            return _context.SURVEY_QUES_CAT;
            //return(from c in _context.SURVEY_QUES_CAT
            //       join f in _context.SURVEY_FIELDSETS on c.CATEGORY_ID equals f.CATEGORY_ID into cf
            //       from subdata in cf.DefaultIfEmpty()
            //       join r in _context.SURVEY_RELATION on subdata.FIELDSET_ID equals r.FIELDSET_ID into fr
            //       from secondsub in fr.DefaultIfEmpty()
            //       join q in _context.SURVEY_QUESTIONS on secondsub.QUESTION_ID equals q.QUESTION_ID into rq
            //       from thirdsub in rq.DefaultIfEmpty()
            //       group new {c, thirdsub} by new{c.CATEGORY_ID, c.CATEGORY_NAME} into qc
            //       select new SURVEY_QUES_CAT{CATEGORY_ID = qc.Key.CATEGORY_ID, CATEGORY_NAME = qc.Key.CATEGORY_NAME, NUM_QUESTIONS = qc.Count(x => x.thirdsub.QUESTION_ID != null)});
        }

        public static SURVEY_QUES_CAT GetQuestionCategory(decimal CategoryId, Entities _context)
        {
            return _context.SURVEY_QUES_CAT.Where(x => x.CATEGORY_ID == CategoryId).Single();
        }

        public static IQueryable<SURVEY_FORMS_COMP> GetCompletionsByDate(DateTime StartDate, DateTime EndDate, decimal FormId, Entities _context)
        {
            EndDate = EndDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            return (from c in _context.SURVEY_FORMS_COMP
                    join f in _context.SURVEY_FORMS on c.FORM_ID equals f.FORM_ID
                    where c.FORM_ID == FormId && c.FILLED_ON >= StartDate && c.FILLED_ON <= EndDate
                    select c);
                        
        }
    }
    public partial class SURVEY_FORMS
    {
        public int NUM_QUESTIONS { get; set; }
        public string PhantomId { get; set; }
    }

    public partial class SURVEY_QUESTIONS
    {
        public string QUESTION_TYPE_NAME { get; set; }
        public decimal FIELDSET_ID { get; set; }
        public string TITLE { get; set; }
        public bool REQUIRED { get; set; }
        public bool ACTIVE { get; set; }
        public string PhantomId { get; set; }
    }

    public partial class SURVEY_FIELDSETS
    {
        public bool ACTIVE { get; set; }
        public string PhantomId { get; set; }
    }

    public partial class SURVEY_OPTIONS
    {
        public bool ACTIVE { get; set; }
        public string TEXT { get; set; }
        public string PhantomId { get; set; }
    }

    public partial class CUSTOMER_SURVEY_THRESH_AMT
    {
        public string ORG_HIER { get; set; }
        public string TYPE_NAME { get; set; }
        public string PhantomId { get; set; }
    }

    public partial class CUSTOMER_SURVEY_THRESHOLDS
    {
        public string PhantomId { get; set; }
    }
    public partial class SURVEY_CAT
    {
        public int? NUM_FORMS { get; set; }
        public string PhantomId { get; set; }
    }

    public partial class SURVEY_FORMS_COMP
    {
        public string FORMS_NAME { get; set; }
        public string LONG_NAME { get; set; }
    }

    public partial class SURVEY_QUES_CAT
    {
        public int? NUM_QUESTIONS { get; set; }
        public string PhantomId { get; set; }
    }
}
