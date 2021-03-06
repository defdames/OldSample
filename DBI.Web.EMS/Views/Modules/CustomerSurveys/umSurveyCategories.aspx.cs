﻿using System;
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
                var FilteredData = GenericData.ListFilterHeader<SURVEY_CAT>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], CUSTOMER_SURVEYS.GetCategories(_context), out count);
                foreach (var item in FilteredData)
                {
                    item.NUM_FORMS = item.SURVEY_FORMS.Count;
                }
                uxCategoriesStore.DataSource = FilteredData;
                e.Total = count;
            }
        }

        protected void deSaveCategory(object sender, DirectEventArgs e)
        {
            ChangeRecords<SURVEY_CAT> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<SURVEY_CAT>();

            foreach (SURVEY_CAT item in data.Created)
            {
                SURVEY_CAT ToBeSaved = new SURVEY_CAT();
                ToBeSaved.NAME = item.NAME;
                ToBeSaved.DESCRIPTION = item.DESCRIPTION;
                ToBeSaved.CREATE_DATE = DateTime.Now;
                ToBeSaved.CREATED_BY = User.Identity.Name;
                ToBeSaved.MODIFIED_BY = User.Identity.Name;
                ToBeSaved.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<SURVEY_CAT>(ToBeSaved);

                ModelProxy Record = uxCategoriesStore.GetByInternalId(item.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(ToBeSaved.CATEGORY_ID);
                Record.Set("NUM_FORMS", "0");
                Record.Commit();
            }

            foreach (SURVEY_CAT item in data.Updated)
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
            uxCategorySelection.SetLocked(false);
            uxAddCategoryButton.Enable();
            uxDeleteCategoryButton.Enable();
            X.Js.Call("checkEditing");
        }

        protected void deReadQuestionCategories(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                int count;
                IQueryable<SURVEY_QUES_CAT> data = CUSTOMER_SURVEYS.GetQuestionCategories(_context);
                var FilteredData = GenericData.ListFilterHeader<SURVEY_QUES_CAT>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                foreach (var item in FilteredData)
                {
                    item.NUM_QUESTIONS = (from c in _context.SURVEY_QUES_CAT
                                          join f in _context.SURVEY_FIELDSETS on c.CATEGORY_ID equals f.CATEGORY_ID into cf
                                          from subdata in cf.DefaultIfEmpty()
                                          join r in _context.SURVEY_RELATION on subdata.FIELDSET_ID equals r.FIELDSET_ID into fr
                                          from secondsub in fr.DefaultIfEmpty()
                                          join q in _context.SURVEY_QUESTIONS on secondsub.QUESTION_ID equals q.QUESTION_ID into rq
                                          from thirdsub in rq.DefaultIfEmpty()
                                          group new { c, thirdsub } by new { c.CATEGORY_ID, c.CATEGORY_NAME } into qc
                                          where qc.Key.CATEGORY_ID == item.CATEGORY_ID
                                          select qc.Count(x => x.thirdsub.QUESTION_ID != null)).Single();
                }
                uxQuestionCategoryStore.DataSource = FilteredData;
                e.Total = count;
            }
        }

        protected void deSaveQuestionCategory(object sender, DirectEventArgs e)
        {
            ChangeRecords<SURVEY_QUES_CAT> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<SURVEY_QUES_CAT>();

            foreach (SURVEY_QUES_CAT item in data.Created)
            {
                SURVEY_QUES_CAT NewCategory = new SURVEY_QUES_CAT();
                NewCategory.CATEGORY_NAME = item.CATEGORY_NAME;
                GenericData.Insert<SURVEY_QUES_CAT>(NewCategory);

                ModelProxy Record = uxQuestionCategoryStore.GetByInternalId(item.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(NewCategory.CATEGORY_ID);
                Record.Set("NUM_QUESTIONS", "0");
                Record.Commit();
            }

            foreach (SURVEY_QUES_CAT item in data.Updated)
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
            uxQuestionCategorySelection.SetLocked(false);
            uxDeleteQuestionCategoryButton.Enable();
            uxAddQuestionCategoryButton.Enable();
            X.Js.Call("checkEditing");
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
                
            }
            else
            {
                GenericData.Delete<SURVEY_CAT>(ToBeDeleted);
                uxCategoriesStore.Reload();
                uxDeleteCategoryButton.Disable();
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
                
            }
            else
            {
                GenericData.Delete<SURVEY_QUES_CAT>(ToBeDeleted);
                uxQuestionCategoryStore.Reload();
                uxDeleteQuestionCategoryButton.Disable();
            }

        }
    }
}