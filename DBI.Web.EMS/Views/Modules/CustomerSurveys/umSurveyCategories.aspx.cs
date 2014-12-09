using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;
using System.Data.Entity;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umSurveyCategories : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CustomerSurveys.ManageCategories"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }

        protected void deReadCategories(object sender, StoreReadDataEventArgs e)
        {
            int count;
            using (Entities _context = new Entities())
            {
                uxCategoriesStore.DataSource = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyCategoryStore>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], CUSTOMER_SURVEYS.GetCategories(_context), out count);
                e.Total = count;
            }
        }

        protected void deSaveCategory(object sender, DirectEventArgs e)
        {
            ChangeRecords<CUSTOMER_SURVEYS.CustomerSurveyCategoryStore> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEYS.CustomerSurveyCategoryStore>();

            foreach (CUSTOMER_SURVEYS.CustomerSurveyCategoryStore item in data.Created)
            {
                SURVEY_CAT ToBeSaved = new SURVEY_CAT();
                ToBeSaved.NAME = item.NAME;
                ToBeSaved.DESCRIPTION = item.DESCRIPTION;
                ToBeSaved.CREATE_DATE = DateTime.Now;
                ToBeSaved.CREATED_BY = User.Identity.Name;
                ToBeSaved.MODIFIED_BY = User.Identity.Name;
                ToBeSaved.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<SURVEY_CAT>(ToBeSaved);

                ModelProxy Record = uxQuestionCategoryStore.GetByInternalId(item.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(ToBeSaved.CATEGORY_ID);
                Record.Commit();
            }

            foreach (CUSTOMER_SURVEYS.CustomerSurveyCategoryStore item in data.Updated)
            {
                SURVEY_CAT ToBeUpdated;

                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetCategory(item.CATEGORY_ID, _context);
                }
                ToBeUpdated.NAME = item.NAME;
                ToBeUpdated.DESCRIPTION = item.DESCRIPTION;
                ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                ToBeUpdated.MODIFY_DATE = DateTime.Now;

                GenericData.Update<SURVEY_CAT>(ToBeUpdated);
            }
            //dmSubtractFromDirty();
            uxCategoriesStore.CommitChanges();
        }

        protected void deReadQuestionCategories(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                int count;
                IQueryable<CUSTOMER_SURVEYS.CustomerSurveyQuestionCategoryStore> data = CUSTOMER_SURVEYS.GetQuestionCategories(_context);
                var test = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyQuestionCategoryStore>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                uxQuestionCategoryStore.DataSource = test;
                e.Total = count;
            }
        }

        protected void deSaveQuestionCategory(object sender, DirectEventArgs e)
        {
            ChangeRecords<CUSTOMER_SURVEYS.CustomerSurveyQuestionCategoryStore> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEYS.CustomerSurveyQuestionCategoryStore>();

            foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestionCategoryStore item in data.Created)
            {
                SURVEY_QUES_CAT NewCategory = new SURVEY_QUES_CAT();
                NewCategory.CATEGORY_NAME = item.CATEGORY_NAME;
                GenericData.Insert<SURVEY_QUES_CAT>(NewCategory);

                ModelProxy Record = uxQuestionCategoryStore.GetByInternalId(item.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(NewCategory.CATEGORY_ID);
                Record.Commit();
            }

            foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestionCategoryStore item in data.Updated)
            {
                SURVEY_QUES_CAT CategoryToEdit;

                using (Entities _context = new Entities())
                {
                    CategoryToEdit = CUSTOMER_SURVEYS.GetQuestionCategory(item.CATEGORY_ID, _context);
                }

                CategoryToEdit.CATEGORY_NAME = item.CATEGORY_NAME;

                GenericData.Update(CategoryToEdit);
            }
            //dmSubtractFromDirty();
            uxQuestionCategoryStore.CommitChanges();
        }

        [DirectMethod]
        public void dmDeleteCategory(string Id)
        {
            decimal CategoryId = decimal.Parse(Id);
            SURVEY_CAT ToBeDeleted;
            int FormCount;
            using (Entities _context = new Entities())
            {
                ToBeDeleted = CUSTOMER_SURVEYS.GetCategory(CategoryId, _context);
                FormCount = ToBeDeleted.SURVEY_FORMS.Count;
            }
            if (FormCount > 0)
            {
                X.Msg.Alert("Error", "This Category has forms within it.  Please remove any existing forms before deleting the Category").Show();
                uxCategoriesStore.Reload();
            }
            else
            {
                GenericData.Delete<SURVEY_CAT>(ToBeDeleted);
            }
        }

        [DirectMethod]
        public void dmDeleteQuestionCategory(string Id)
        {
            decimal CategoryId = decimal.Parse(Id);
            SURVEY_QUES_CAT ToBeDeleted;
            int FieldsetCount;
            using (Entities _context = new Entities())
            {
                ToBeDeleted = CUSTOMER_SURVEYS.GetQuestionCategory(CategoryId, _context);
                FieldsetCount = (from d in _context.SURVEY_QUES_CAT
                                 join f in _context.SURVEY_FIELDSETS on d.CATEGORY_ID equals f.CATEGORY_ID
                                 where d.CATEGORY_ID == CategoryId
                                 select f).Count();
            }
            if (FieldsetCount > 0)
            {
                X.Msg.Alert("Error", "This Category has fieldsets within it.  Please remove any associated fieldsets, or change the fieldset to another Question Category before deleting").Show();
                uxQuestionCategoryStore.Reload();
            }
            else
            {
                GenericData.Delete<SURVEY_QUES_CAT>(ToBeDeleted);
            }

        }

        //[DirectMethod]
        //public void dmAddToDirty()
        //{
        //    long isDirty = long.Parse(Session["isDirty"].ToString());
        //    isDirty++;
        //    Session["isDirty"] = isDirty;
        //}

        //[DirectMethod]
        //public void dmSubtractFromDirty()
        //{
        //    long isDirty = long.Parse(Session["isDirty"].ToString());
        //    isDirty--;
        //    Session["isDirty"] = isDirty;
        //}
    }
}