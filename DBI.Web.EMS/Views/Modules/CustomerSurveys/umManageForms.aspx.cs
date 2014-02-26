using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umManageForms : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadCurrentForms(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var data = (from c in _context.CUSTOMER_SURVEY_FORMS
                            from f in c.CUSTOMER_SURVEY_FIELDSETS.DefaultIfEmpty()
                            from r in f.CUSTOMER_SURVEY_RELATION.DefaultIfEmpty()
                            join q in _context.CUSTOMER_SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                            group new { c, q } by new { c.FORM_ID, c.FORMS_NAME, q.QUESTION_ID } into counter
                            select new { FORM_ID = counter.Key.FORM_ID, FORMS_NAME = counter.Key.FORMS_NAME, NUM_QUESTIONS = counter.Count() }).ToList();

                uxCurrentFormsStore.DataSource = data;
            }
        }

        protected void deLoadFieldSets(object sender, DirectEventArgs e)
        {
            decimal FormId = decimal.Parse(e.ExtraParams["FormId"]);
            using (Entities _context = new Entities())
            {
                var data = (from f in _context.CUSTOMER_SURVEY_FIELDSETS
                            where f.FORM_ID == FormId
                            select f).ToList();
                uxCurrentFieldSetsStore.DataSource = data;
            }
        }

        protected void deAddForm(object sender, DirectEventArgs e)
        {
            CUSTOMER_SURVEY_FORMS NewForm;
            decimal OrgId;
            try
            {
                OrgId = decimal.Parse(uxAddFormOrg.Value.ToString());
            }
            catch (Exception)
            {
                OrgId = 0;
            }
            using (Entities _context = new Entities())
            {
                NewForm = new CUSTOMER_SURVEY_FORMS
                {
                    FORMS_NAME = uxAddFormName.Value.ToString(),
                    ORG_ID = OrgId,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name
                };
            }
            GenericData.Insert<CUSTOMER_SURVEY_FORMS>(NewForm);
            uxCurrentFormsStore.Reload();
            uxAddFormWindow.Hide();
            uxAddFormPanel.Reset();
        }

        protected void deEditForm(object sender, DirectEventArgs e)
        {
            decimal FormId = decimal.Parse(e.ExtraParams["FormId"]);
            decimal OrgId = decimal.Parse(uxAddFormOrg.Value.ToString());
            CUSTOMER_SURVEY_FORMS FormToEdit;

            using (Entities _context = new Entities())
            {
                FormToEdit = (from c in _context.CUSTOMER_SURVEY_FORMS
                              where c.FORM_ID == FormId
                              select c).Single();
                FormToEdit.FORMS_NAME = uxAddFormName.Value.ToString();
                FormToEdit.ORG_ID = OrgId;
                FormToEdit.MODIFIED_BY = User.Identity.Name;
                FormToEdit.MODIFY_DATE = DateTime.Now;
            }
            GenericData.Update<CUSTOMER_SURVEY_FORMS>(FormToEdit);
            
        }

        protected void deAddFieldSet(object sender, DirectEventArgs e)
        {
            CUSTOMER_SURVEY_FIELDSETS NewFieldSet;

            using (Entities _context = new Entities())
            {
                NewFieldSet = new CUSTOMER_SURVEY_FIELDSETS
                {
                    TITLE = uxAddFieldSetTitle.Value.ToString(),
                    FORM_ID = 0
                };
            }
            GenericData.Insert<CUSTOMER_SURVEY_FIELDSETS>(NewFieldSet);
            uxCurrentFieldSetsStore.Reload();
            uxAddFieldSetWindow.Hide();
            uxAddFieldSetPanel.Reset();
        }
        
        protected void deEditFieldSet(object sender, DirectEventArgs e)
        {
            decimal FieldSetId = decimal.Parse(e.ExtraParams["FieldSetId"]);
            CUSTOMER_SURVEY_FIELDSETS FieldSetToEdit;

            using (Entities _context = new Entities())
            {
                FieldSetToEdit = (from c in _context.CUSTOMER_SURVEY_FIELDSETS
                                  where c.FIELDSET_ID == FieldSetId
                                  select c).Single();
                FieldSetToEdit.TITLE = uxAddFieldSetTitle.Value.ToString();
                FieldSetToEdit.MODIFIED_BY = User.Identity.Name;
                FieldSetToEdit.MODIFY_DATE = DateTime.Now;
            }
            GenericData.Update<CUSTOMER_SURVEY_FIELDSETS>(FieldSetToEdit);
        }

        
        
    }
}