using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class CUSTOMER_SURVEY_FORMS
    {
        public static IQueryable<CUSTOMER_SURVEY_FORMS> GetForms()
        {
            Entities _context = new Entities();
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

        public static IQueryable<CUSTOMER_SURVEY_FIELDSETS> GetFormFieldSets(decimal FormId, Entities _context)
        {
            return (from f in _context.CUSTOMER_SURVEY_FIELDSETS
                    orderby f.SORT_ORDER ascending
                    where f.FORM_ID == FormId
                    select f);
        }

        public static IQueryable<CUSTOMER_SURVEY_QUESTIONS> GetFormQuestions(decimal FieldSetId, Entities _context)
        {
            return (from q in _context.CUSTOMER_SURVEY_RELATION
                    orderby q.SORT_ORDER ascending
                    where q.FIELDSET_ID == FieldSetId
                    select q.CUSTOMER_SURVEY_QUESTIONS);
        }

        public static IQueryable<CUSTOMER_SURVEY_OPTIONS> GetQuestionOptions(decimal QuestionId, Entities _context)
        {
            return _context.CUSTOMER_SURVEY_OPTIONS.Where(x => x.QUESTION_ID == QuestionId);
        }
    }
}
