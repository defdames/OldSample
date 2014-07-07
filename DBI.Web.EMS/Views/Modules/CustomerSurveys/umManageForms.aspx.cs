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
using System.Data.Entity;

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
                IQueryable<CUSTOMER_SURVEY_FORMS> FormData = CUSTOMER_SURVEYS.GetForms(_context);
                int count;
                IQueryable<CUSTOMER_SURVEYS.CustomerSurveyForms> AllData = FormData.Select(x => new CUSTOMER_SURVEYS.CustomerSurveyForms { FORM_ID = x.FORM_ID, FORMS_NAME = x.FORMS_NAME, ORG_ID = x.ORG_ID, CATEGORY_ID = x.CATEGORY_ID });
                List<CUSTOMER_SURVEYS.CustomerSurveyForms> Data = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyForms>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], AllData, out count);
                foreach (CUSTOMER_SURVEYS.CustomerSurveyForms ThisForm in Data)
                {
                    var NumQuestions = (from f in _context.CUSTOMER_SURVEY_FORMS
                                        join fs in _context.CUSTOMER_SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                                        join r in _context.CUSTOMER_SURVEY_RELATION on fs.FIELDSET_ID equals r.FIELDSET_ID
                                        join q in _context.CUSTOMER_SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
                                        where f.FORM_ID == ThisForm.FORM_ID
                                        select q.QUESTION_ID).Count();
                                       
                    ThisForm.NUM_QUESTIONS = NumQuestions;
                }
                uxFormsStore.DataSource = Data;
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

                IQueryable<CUSTOMER_SURVEYS.CustomerSurveyQuestions> data = CUSTOMER_SURVEYS.GetFormQuestions(FormId, _context);
                int count;
                uxQuestionsStore.DataSource = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyQuestions>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadOptions(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                decimal QuestionId = decimal.Parse(e.Parameters["QuestionId"]);
                IQueryable<CUSTOMER_SURVEYS.CustomerSurveyOptions> data = CUSTOMER_SURVEYS.GetQuestionOptions(QuestionId, _context).Select(d => new CUSTOMER_SURVEYS.CustomerSurveyOptions{OPTION_ID = d.OPTION_ID, QUESTION_ID = d.QUESTION_ID, TEXT = d.CUSTOMER_SURVEY_QUESTIONS.TEXT, IS_ACTIVE = (d.IS_ACTIVE == "Y" ? true : false), OPTION_NAME = d.OPTION_NAME, SORT_ORDER = d.SORT_ORDER});
                int count;
                uxOptionsStore.DataSource = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyOptions>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadFieldsets(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long FormId = long.Parse(e.Parameters["FormId"]);
                IQueryable<CUSTOMER_SURVEYS.CustomerSurveyFieldsets> data = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context);

                int count;
                uxFieldsetsStore.DataSource = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyFieldsets>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }

        protected void deReadQuestionFieldsets(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long FormId = long.Parse(e.Parameters["FormId"]);
                uxQuestionFieldsetStore.DataSource = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context).Select(x => new { x.FIELDSET_ID, x.TITLE }).ToList();
                
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
                uxQuestionTypeStore.DataSource = CUSTOMER_SURVEYS.GetQuestionTypes(_context).ToList();
            }
        }

        protected void deReadCategories(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxAddFormCatStore.DataSource = _context.CUSTOMER_SURVEY_CAT.ToList();
            }
        }

        protected void deSaveForms(object sender, DirectEventArgs e)
        {
            ChangeRecords<CUSTOMER_SURVEYS.CustomerSurveyForms> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEYS.CustomerSurveyForms>();
            foreach (CUSTOMER_SURVEYS.CustomerSurveyForms CreatedForm in data.Created)
            {
                CUSTOMER_SURVEY_FORMS NewForm = new CUSTOMER_SURVEY_FORMS();
                NewForm.FORMS_NAME = CreatedForm.FORMS_NAME;
                NewForm.ORG_ID = CreatedForm.ORG_ID;
                NewForm.CATEGORY_ID = CreatedForm.CATEGORY_ID;
                NewForm.CREATE_DATE = DateTime.Now;
                NewForm.MODIFY_DATE = DateTime.Now;
                NewForm.CREATED_BY = User.Identity.Name;
                NewForm.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert<CUSTOMER_SURVEY_FORMS>(NewForm);

            }

            foreach (CUSTOMER_SURVEYS.CustomerSurveyForms UpdatedForm in data.Updated)
            {
                CUSTOMER_SURVEY_FORMS ToBeUpdated;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetForms(_context).Where(x => x.FORM_ID == UpdatedForm.FORM_ID).Single();
                }
                ToBeUpdated.FORMS_NAME = UpdatedForm.FORMS_NAME;
                ToBeUpdated.ORG_ID = UpdatedForm.ORG_ID;
                ToBeUpdated.CATEGORY_ID = UpdatedForm.CATEGORY_ID;
                ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                ToBeUpdated.MODIFY_DATE = DateTime.Now;

                GenericData.Update<CUSTOMER_SURVEY_FORMS>(ToBeUpdated);
            }

            foreach (CUSTOMER_SURVEYS.CustomerSurveyForms DeletedForm in data.Deleted)
            {
                
            }
            uxFormsStore.CommitChanges();
        }

        protected void deSaveFieldsets(object sender, DirectEventArgs e)
        {
            ChangeRecords <CUSTOMER_SURVEYS.CustomerSurveyFieldsets> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEYS.CustomerSurveyFieldsets>();

            foreach (CUSTOMER_SURVEYS.CustomerSurveyFieldsets NewFieldset in data.Created)
            {
                CUSTOMER_SURVEY_FIELDSETS ToBeAdded = new CUSTOMER_SURVEY_FIELDSETS();
                ToBeAdded.CREATE_DATE = DateTime.Now;
                ToBeAdded.MODIFY_DATE = DateTime.Now;
                ToBeAdded.CREATED_BY = User.Identity.Name;
                ToBeAdded.TITLE = NewFieldset.TITLE;
                ToBeAdded.MODIFIED_BY = User.Identity.Name;
                ToBeAdded.FORM_ID = decimal.Parse(e.ExtraParams["FormId"]);
                ToBeAdded.IS_ACTIVE = (NewFieldset.IS_ACTIVE == true ? "Y" : "N");

                GenericData.Insert<CUSTOMER_SURVEY_FIELDSETS>(ToBeAdded);
            }

            foreach (CUSTOMER_SURVEYS.CustomerSurveyFieldsets UpdatedFieldset in data.Updated)
            {
                CUSTOMER_SURVEY_FIELDSETS ToBeUpdated;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetFieldsets(_context).Where(x => x.FIELDSET_ID == UpdatedFieldset.FIELDSET_ID).Single();
                    ToBeUpdated.TITLE = UpdatedFieldset.TITLE;
                    ToBeUpdated.IS_ACTIVE = (UpdatedFieldset.IS_ACTIVE == true ? "Y" : "N");
                    ToBeUpdated.FORM_ID = decimal.Parse(e.ExtraParams["FormId"]);
                    ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                    ToBeUpdated.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Update<CUSTOMER_SURVEY_FIELDSETS>(ToBeUpdated);

            }

            uxFieldsetsStore.CommitChanges();
            uxQuestionFieldsetStore.Reload();
        }

        protected void deSaveQuestions(object sender, DirectEventArgs e)
        {
            ChangeRecords<CUSTOMER_SURVEYS.CustomerSurveyQuestions> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEYS.CustomerSurveyQuestions>();

            foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestions NewQuestion in data.Created)
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

            foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestions UpdatedQuestion in data.Updated)
            {
                CUSTOMER_SURVEY_QUESTIONS ToBeUpdated;
                CUSTOMER_SURVEY_RELATION FieldsetToUpdate;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetQuestion(UpdatedQuestion.QUESTION_ID, _context);
                    ToBeUpdated.IS_REQUIRED = (UpdatedQuestion.IS_REQUIRED == true ? "Y" : "N");
                    ToBeUpdated.TEXT = UpdatedQuestion.TEXT;
                    ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                    ToBeUpdated.MODIFY_DATE = DateTime.Now;
                    ToBeUpdated.TYPE_ID = UpdatedQuestion.TYPE_ID;
                    ToBeUpdated.IS_ACTIVE = "Y";

                    FieldsetToUpdate = CUSTOMER_SURVEYS.GetRelationshipEntry(UpdatedQuestion.QUESTION_ID, _context);
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
            ChangeRecords<CUSTOMER_SURVEYS.CustomerSurveyOptions> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEYS.CustomerSurveyOptions>();
            foreach (CUSTOMER_SURVEYS.CustomerSurveyOptions NewOption in data.Created)
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
            foreach (CUSTOMER_SURVEYS.CustomerSurveyOptions UpdatedOption in data.Updated)
            {
                CUSTOMER_SURVEY_OPTIONS ToUpdate;
                using (Entities _context = new Entities())
                {
                    ToUpdate = CUSTOMER_SURVEYS.GetQuestionOption(UpdatedOption.OPTION_ID, _context);
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
}