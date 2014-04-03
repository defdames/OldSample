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
                List<CustomerSurveyForms> data = (from c in _context.CUSTOMER_SURVEY_FORMS
                           from f in c.CUSTOMER_SURVEY_FIELDSETS.DefaultIfEmpty()
                           from r in f.CUSTOMER_SURVEY_RELATION.DefaultIfEmpty()
                           join q in _context.CUSTOMER_SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                           join o in _context.ORG_HIER_V on c.ORG_ID equals o.ORG_ID
                           group new { c, q, o } by new { c.FORM_ID, c.FORMS_NAME, q.QUESTION_ID, o.ORG_HIER} into counter
                           
                           select new CustomerSurveyForms { FORM_ID = counter.Key.FORM_ID, FORMS_NAME = counter.Key.FORMS_NAME, NUM_QUESTIONS = counter.Count(), ORGANIZATION = counter.Key.ORG_HIER }).ToList();
                int count;
                uxFormsStore.DataSource = GenericData.EnumerableFilterHeader<CustomerSurveyForms>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadQuestions(object sender, StoreReadDataEventArgs e)
        {

        }

        protected void deReadOptions(object sender, StoreReadDataEventArgs e)
        {

        }

        protected void deReadFieldsets(object sender, StoreReadDataEventArgs e)
        {

        }
    }

    public class CustomerSurveyForms
    {
        public decimal FORM_ID { get; set; }
        public string FORMS_NAME { get; set; }
        public string ORGANIZATION { get; set; }
        public int NUM_QUESTIONS { get; set; }
    }
}