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
    public partial class umManageQuestions : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetQuestions();
            GetQuestionTypes();
        }

        protected void GetQuestions()
        {
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.CUSTOMER_SURVEY_QUESTIONS
                            select d).ToList();
                uxOptionQuestionStore.DataSource = data;
            }
        }

        protected void GetQuestionTypes()
        {
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.CUSTOMER_SURVEY_QUES_TYPES
                            select d).ToList();
                uxQuestionTypeStore.DataSource = data;
            }
        }

        protected void deReadQuestions(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var data = (from q in _context.CUSTOMER_SURVEY_QUESTIONS
                            select new { q.TEXT, q.CUSTOMER_SURVEY_QUES_TYPES.QUESTION_TYPE_NAME }).ToList();
                uxCurrentQuestionsStore.DataSource = data;
            }
        }

        protected void deDeactivateOption(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var ToBeUpdated = (from d in _context.CUSTOMER_SURVEY_QUES_TYPES
                                       select d);
            }
        }

        protected void deAddEditQuestion(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {

            }
        }

        protected void deAddOption(object sender, DirectEventArgs e)
        {

        }
    }
}