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
            if (!validateComponentSecurity("SYS.CustomerSurveys.ManageForms"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest || !IsPostBack)
            {
                Session["isDirty"] = "0";
                uxAddFormCatStore.Reload();
                uxAddFormOrgStore.Reload();
            }
            
        }


        protected void deReadForms(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                IQueryable<SURVEY_FORMS> FormData = CUSTOMER_SURVEYS.GetForms(_context);
                int count;
                List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                IQueryable<SURVEY_FORMS> AllData = FormData.Where(x => OrgsList.Contains((long)x.ORG_ID));
                List<SURVEY_FORMS> Data = GenericData.ListFilterHeader<SURVEY_FORMS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], AllData, out count);
                foreach (SURVEY_FORMS ThisForm in Data)
                {
                    var NumQuestions = (from f in _context.SURVEY_FORMS
                                        join fs in _context.SURVEY_FIELDSETS on f.FORM_ID equals fs.FORM_ID
                                        join r in _context.SURVEY_RELATION on fs.FIELDSET_ID equals r.FIELDSET_ID
                                        join q in _context.SURVEY_QUESTIONS on r.QUESTION_ID equals q.QUESTION_ID
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

                IQueryable<SURVEY_QUESTIONS> data = CUSTOMER_SURVEYS.GetFormQuestions(FormId, _context);
                int count;
                var FilteredData = GenericData.ListFilterHeader<SURVEY_QUESTIONS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                foreach (var item in FilteredData)
                {
                    item.QUESTION_TYPE_NAME = item.SURVEY_QUES_TYPES.QUESTION_TYPE_NAME;
                    item.TITLE = item.SURVEY_RELATION.Where(x => x.QUESTION_ID == item.QUESTION_ID).Select(x => x.SURVEY_FIELDSETS.TITLE).Single();
                    item.FIELDSET_ID = item.SURVEY_RELATION.Where(x => x.QUESTION_ID == item.QUESTION_ID).Select(x => x.SURVEY_FIELDSETS.FIELDSET_ID).Single();
                    item.ACTIVE = (item.IS_ACTIVE == "Y" ? true : false);
                    item.REQUIRED = (item.IS_REQUIRED == "Y" ? true : false);
                }
                uxQuestionsStore.DataSource = FilteredData;
                e.Total = count;
            }
        }

        protected void deReadOptions(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                decimal QuestionId = decimal.Parse(e.Parameters["QuestionId"]);
                IQueryable<SURVEY_OPTIONS> data = CUSTOMER_SURVEYS.GetQuestionOptions(QuestionId, _context);
                int count;
                var FilteredData = GenericData.ListFilterHeader<SURVEY_OPTIONS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                foreach (var item in FilteredData)
                {
                    item.ACTIVE = (item.IS_ACTIVE == "Y" ? true : false);
                    item.TEXT = item.SURVEY_QUESTIONS.TEXT;
                }
                uxOptionsStore.DataSource = FilteredData;
                e.Total = count;
            }
        }

        protected void deReadFieldsets(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long FormId = long.Parse(e.Parameters["FormId"]);
                IQueryable<SURVEY_FIELDSETS> data = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context);
                
                int count;
                var FilteredData = GenericData.ListFilterHeader<SURVEY_FIELDSETS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                foreach (var item in FilteredData)
                {
                    item.ACTIVE = (item.IS_ACTIVE == "Y" ? true : false);
                }
                uxFieldsetsStore.DataSource = FilteredData;
                e.Total = count;
            }
        }

        protected void deReadFieldsetCategories(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxQuestionCategoryStore.DataSource = _context.SURVEY_QUES_CAT.ToList();
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
            List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
            using (Entities _context = new Entities())
            {
                uxAddFormOrgStore.DataSource = _context.ORG_HIER_V.Distinct().Where(x => OrgsList.Contains(x.ORG_ID)).ToList();
                uxCopyFormOrgStore.DataSource = _context.ORG_HIER_V.Distinct().Where(x => OrgsList.Contains(x.ORG_ID)).ToList();
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
                uxAddFormCatStore.DataSource = _context.SURVEY_CAT.ToList();
                uxCopyFormCategoryStore.DataSource = _context.SURVEY_CAT.ToList();
            }
        }
        
        protected void deSaveForm(object sender, DirectEventArgs e)
        {
            
            ChangeRecords<SURVEY_FORMS> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<SURVEY_FORMS>();
            foreach (SURVEY_FORMS CreatedForm in data.Created)
            {
                SURVEY_FORMS NewForm = new SURVEY_FORMS();
                NewForm.FORMS_NAME = CreatedForm.FORMS_NAME;
                NewForm.ORG_ID = CreatedForm.ORG_ID;
                NewForm.CATEGORY_ID = CreatedForm.CATEGORY_ID;
                NewForm.CREATE_DATE = DateTime.Now;
                NewForm.MODIFY_DATE = DateTime.Now;
                NewForm.CREATED_BY = User.Identity.Name;
                NewForm.MODIFIED_BY = User.Identity.Name;

                if (CreatedForm.TYPE_ID == null)
                {
                    SURVEY_TYPES FormTypes = new SURVEY_TYPES();
                    FormTypes.TYPE_NAME = e.ExtraParams["TypeName"];
                    GenericData.Insert<SURVEY_TYPES>(FormTypes);
                    NewForm.TYPE_ID = FormTypes.TYPE_ID;
                }
                else
                {
                    NewForm.TYPE_ID = (decimal)CreatedForm.TYPE_ID;
                }
                GenericData.Insert<SURVEY_FORMS>(NewForm);

                ModelProxy Record = uxFormsStore.GetByInternalId(CreatedForm.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(NewForm.FORM_ID);
                Record.Set("TYPE_ID", NewForm.TYPE_ID);
                Record.Commit();
                //uxFormTypeStore.Reload();
                uxAddFieldsetButton.Enable();
                uxAddQuestionButton.Enable();
            }

            foreach (SURVEY_FORMS UpdatedForm in data.Updated)
            {
                SURVEY_FORMS ToBeUpdated;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetForms(_context).Where(x => x.FORM_ID == UpdatedForm.FORM_ID).Single();
                }

                if (UpdatedForm.TYPE_ID == null)
                {
                    SURVEY_TYPES FormTypes = new SURVEY_TYPES();
                    FormTypes.TYPE_NAME = e.ExtraParams["TypeName"];
                    GenericData.Insert<SURVEY_TYPES>(FormTypes);
                    ToBeUpdated.TYPE_ID = FormTypes.TYPE_ID;
                }
                else
                {
                    ToBeUpdated.TYPE_ID = (decimal)UpdatedForm.TYPE_ID;
                }
                ToBeUpdated.FORMS_NAME = UpdatedForm.FORMS_NAME;
                ToBeUpdated.ORG_ID = UpdatedForm.ORG_ID;
                ToBeUpdated.CATEGORY_ID = UpdatedForm.CATEGORY_ID;
                ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                ToBeUpdated.MODIFY_DATE = DateTime.Now;

                GenericData.Update<SURVEY_FORMS>(ToBeUpdated);
                uxFormsStore.GetById(ToBeUpdated.FORM_ID).Commit();

                ModelProxy Record = uxFormsStore.GetById(ToBeUpdated.FORM_ID);
                Record.Set("TYPE_ID", ToBeUpdated.TYPE_ID);
                Record.Commit();
                uxFormTypeStore.Reload();
            }
            uxFormsStore.CommitChanges();
            uxAddFormButton.Enable();
            X.Js.Call("checkEditing");
            uxFormSelection.SetLocked(false);
            uxDeleteFormButton.Enable();
            uxCopyFormButton.Enable();
            uxViewFormButton.Enable();
            uxFieldsetsStore.Reload();
            uxQuestionsStore.Reload();
            uxOptionsStore.Reload();
        }

        protected void deSaveFieldsets(object sender, DirectEventArgs e)
        {
            ChangeRecords <SURVEY_FIELDSETS> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<SURVEY_FIELDSETS>();

            foreach (SURVEY_FIELDSETS NewFieldset in data.Created)
            {
                SURVEY_QUES_CAT NewCategory;
                SURVEY_FIELDSETS ToBeAdded = new SURVEY_FIELDSETS();
                ToBeAdded.CREATE_DATE = DateTime.Now;
                ToBeAdded.MODIFY_DATE = DateTime.Now;
                ToBeAdded.CREATED_BY = User.Identity.Name;
                ToBeAdded.TITLE = NewFieldset.TITLE;
                ToBeAdded.MODIFIED_BY = User.Identity.Name;
                ToBeAdded.SORT_ORDER = NewFieldset.SORT_ORDER;
                ToBeAdded.FORM_ID = decimal.Parse(e.ExtraParams["FormId"]);
                ToBeAdded.IS_ACTIVE = (NewFieldset.ACTIVE == true ? "Y" : "N");

                if (NewFieldset.CATEGORY_ID == null)
                {
                    NewCategory = new SURVEY_QUES_CAT();
                    NewCategory.CATEGORY_NAME = e.ExtraParams["CategoryName"];
                    GenericData.Insert<SURVEY_QUES_CAT>(NewCategory);
                    ToBeAdded.CATEGORY_ID = NewCategory.CATEGORY_ID;
                }
                else
                {
                    ToBeAdded.CATEGORY_ID = (decimal)NewFieldset.CATEGORY_ID;
                    using (Entities _context = new Entities())
                    {
                        NewCategory = _context.SURVEY_QUES_CAT.Where(x => x.CATEGORY_ID == NewFieldset.CATEGORY_ID).Single();
                    }
                }
                GenericData.Insert<SURVEY_FIELDSETS>(ToBeAdded);
                

                ModelProxy Record = uxFieldsetsStore.GetByInternalId(NewFieldset.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(ToBeAdded.FIELDSET_ID);
                Record.Set("TITLE", ToBeAdded.TITLE);
                Record.Commit();
                uxQuestionCategoryStore.Reload();
            }

            foreach (SURVEY_FIELDSETS UpdatedFieldset in data.Updated)
            {
                SURVEY_QUES_CAT NewCategory;
                SURVEY_FIELDSETS ToBeUpdated;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetFieldsets(_context).Where(x => x.FIELDSET_ID == UpdatedFieldset.FIELDSET_ID).Single();
                    ToBeUpdated.TITLE = UpdatedFieldset.TITLE;
                    ToBeUpdated.IS_ACTIVE = (UpdatedFieldset.ACTIVE == true ? "Y" : "N");
                    ToBeUpdated.SORT_ORDER = UpdatedFieldset.SORT_ORDER;
                    ToBeUpdated.FORM_ID = decimal.Parse(e.ExtraParams["FormId"]);
                    ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                    ToBeUpdated.MODIFY_DATE = DateTime.Now;
                }
                if (UpdatedFieldset.CATEGORY_ID == null)
                {
                    NewCategory = new SURVEY_QUES_CAT();
                    NewCategory.CATEGORY_NAME = e.ExtraParams["CategoryName"];
                    GenericData.Insert<SURVEY_QUES_CAT>(NewCategory);
                    ToBeUpdated.CATEGORY_ID = NewCategory.CATEGORY_ID;
                }
                else
                {
                    ToBeUpdated.CATEGORY_ID = (decimal)UpdatedFieldset.CATEGORY_ID;
                    using (Entities _context = new Entities())
                    {
                        NewCategory = _context.SURVEY_QUES_CAT.Where(x => x.CATEGORY_ID == ToBeUpdated.CATEGORY_ID).Single();
                    }
                }
                GenericData.Update<SURVEY_FIELDSETS>(ToBeUpdated);
                uxQuestionCategoryStore.Reload();
                ModelProxy Record = uxFieldsetsStore.GetById(ToBeUpdated.FIELDSET_ID);
                Record.Set("TITLE", ToBeUpdated.TITLE);
                Record.Commit();
                uxQuestionCategoryStore.Reload();
            }
            
            uxFieldsetsStore.CommitChanges();
            uxDeleteFieldsetButton.Enable();
            uxAddFieldsetButton.Enable();
            X.Js.Call("checkEditing");
            uxFieldsetSelection.SetLocked(false);
            uxQuestionFieldsetStore.Reload();
            uxFieldsetsStore.Reload();
            uxAddFormButton.Enable();
            uxCopyFormButton.Enable();
            uxViewFormButton.Enable();
            uxDeleteFormButton.Enable();
            uxFormSelection.SetLocked(false);
        }

        protected void deSaveQuestions(object sender, DirectEventArgs e)
        {
            ChangeRecords<SURVEY_QUESTIONS> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<SURVEY_QUESTIONS>();

            foreach (SURVEY_QUESTIONS NewQuestion in data.Created)
            {
                SURVEY_QUESTIONS ToBeAdded = new SURVEY_QUESTIONS();
                ToBeAdded.TEXT = NewQuestion.TEXT;
                ToBeAdded.TYPE_ID = NewQuestion.TYPE_ID;
                ToBeAdded.SORT_ORDER = NewQuestion.SORT_ORDER;
                ToBeAdded.CREATED_BY = User.Identity.Name;
                ToBeAdded.CREATE_DATE = DateTime.Now;
                ToBeAdded.MODIFIED_BY = User.Identity.Name;
                ToBeAdded.MODIFY_DATE = DateTime.Now;
                ToBeAdded.IS_ACTIVE = (NewQuestion.ACTIVE == true ? "Y" : "N");
                ToBeAdded.IS_REQUIRED = (NewQuestion.REQUIRED == true ? "Y" : "N");
                GenericData.Insert<SURVEY_QUESTIONS>(ToBeAdded);

                SURVEY_RELATION FieldsetToAdd = new SURVEY_RELATION();
                FieldsetToAdd.QUESTION_ID = ToBeAdded.QUESTION_ID;
                FieldsetToAdd.FIELDSET_ID = NewQuestion.FIELDSET_ID;
                FieldsetToAdd.MODIFIED_BY = User.Identity.Name;
                FieldsetToAdd.CREATED_BY = User.Identity.Name;
                FieldsetToAdd.MODIFY_DATE = DateTime.Now;
                FieldsetToAdd.CREATE_DATE = DateTime.Now;
                GenericData.Insert<SURVEY_RELATION>(FieldsetToAdd);

                ModelProxy Record = uxQuestionsStore.GetByInternalId(NewQuestion.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(ToBeAdded.QUESTION_ID);
                Record.Commit();

                if(ToBeAdded.TYPE_ID == 3 || ToBeAdded.TYPE_ID == 4 || ToBeAdded.TYPE_ID == 5)
                {
                    uxAddOptionButton.Enable();
                }
            }

            foreach (SURVEY_QUESTIONS UpdatedQuestion in data.Updated)
            {
                SURVEY_QUESTIONS ToBeUpdated;
                SURVEY_RELATION FieldsetToUpdate;
                using (Entities _context = new Entities())
                {
                    ToBeUpdated = CUSTOMER_SURVEYS.GetQuestion(UpdatedQuestion.QUESTION_ID, _context);
                    ToBeUpdated.IS_REQUIRED = (UpdatedQuestion.REQUIRED == true ? "Y" : "N");
                    ToBeUpdated.SORT_ORDER = UpdatedQuestion.SORT_ORDER;
                    ToBeUpdated.TEXT = UpdatedQuestion.TEXT;
                    ToBeUpdated.MODIFIED_BY = User.Identity.Name;
                    ToBeUpdated.MODIFY_DATE = DateTime.Now;
                    ToBeUpdated.TYPE_ID = UpdatedQuestion.TYPE_ID;
                    ToBeUpdated.IS_ACTIVE = (UpdatedQuestion.ACTIVE == true ? "Y" : "N");

                    FieldsetToUpdate = CUSTOMER_SURVEYS.GetRelationshipEntry(UpdatedQuestion.QUESTION_ID, _context);
                    FieldsetToUpdate.FIELDSET_ID = UpdatedQuestion.FIELDSET_ID;
                    FieldsetToUpdate.MODIFY_DATE = DateTime.Now;
                    FieldsetToUpdate.MODIFIED_BY = User.Identity.Name;
                }
                GenericData.Update<SURVEY_QUESTIONS>(ToBeUpdated);
                GenericData.Update<SURVEY_RELATION>(FieldsetToUpdate);

                if (ToBeUpdated.TYPE_ID == 3 || ToBeUpdated.TYPE_ID == 4 || ToBeUpdated.TYPE_ID == 5)
                {
                    uxAddOptionButton.Enable();
                    uxAddOptionButton.Enable();
                }
            }
            uxQuestionsStore.CommitChanges();
            uxQuestionsStore.Reload();
            uxAddQuestionButton.Enable();
            X.Js.Call("checkEditing");
            uxQuestionSelection.SetLocked(false);
            uxDeleteQuestionButton.Enable();
        }

        protected void deSaveOptions(object sender, DirectEventArgs e)
        {
            ChangeRecords<SURVEY_OPTIONS> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<SURVEY_OPTIONS>();
            foreach (SURVEY_OPTIONS NewOption in data.Created)
            {
                SURVEY_OPTIONS ToBeAdded = new SURVEY_OPTIONS();
                ToBeAdded.QUESTION_ID = decimal.Parse(e.ExtraParams["QuestionId"]);
                ToBeAdded.OPTION_NAME = NewOption.OPTION_NAME;
                ToBeAdded.SORT_ORDER = NewOption.SORT_ORDER;
                ToBeAdded.CREATE_DATE = DateTime.Now;
                ToBeAdded.IS_ACTIVE = (NewOption.ACTIVE == true ? "Y" : "N");
                ToBeAdded.MODIFY_DATE = DateTime.Now;
                ToBeAdded.CREATED_BY = User.Identity.Name;
                ToBeAdded.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert<SURVEY_OPTIONS>(ToBeAdded);

                ModelProxy Record = uxOptionsStore.GetByInternalId(NewOption.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(ToBeAdded.OPTION_ID);
                Record.Commit();
            }
            foreach (SURVEY_OPTIONS UpdatedOption in data.Updated)
            {
                SURVEY_OPTIONS ToUpdate;
                using (Entities _context = new Entities())
                {
                    ToUpdate = CUSTOMER_SURVEYS.GetQuestionOption(UpdatedOption.OPTION_ID, _context);
                    ToUpdate.IS_ACTIVE = (UpdatedOption.ACTIVE == true ? "Y" : "N");
                    ToUpdate.OPTION_NAME = UpdatedOption.OPTION_NAME;
                    ToUpdate.SORT_ORDER = UpdatedOption.SORT_ORDER;
                    ToUpdate.MODIFIED_BY = User.Identity.Name;
                    ToUpdate.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Update<SURVEY_OPTIONS>(ToUpdate);
            }
            uxOptionsStore.CommitChanges();
            uxOptionsStore.Reload();
            uxAddOptionButton.Enable();
            X.Js.Call("checkEditing");
            uxOptionSelection.SetLocked(false);
            uxDeleteOptionButton.Enable();
        }

        protected void deLoadOptions(object sender, DirectEventArgs e)
        {
            int TypeId = int.Parse(e.ExtraParams["QuestionType"]);
            if (TypeId == 3 || TypeId == 4 || TypeId == 5)
            {
                uxAddOptionButton.Enable();
            }
            else
            {
                uxAddOptionButton.Disable();
            }
            if (TypeId != 0)
            {
                uxOptionsStore.Reload();
            }
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
                Url = "/Views/Modules/FormGenerator/umViewSurvey.aspx?FormId=" + e.ExtraParams["FormId"],
                Mode = LoadMode.Frame,
                AutoLoad = true
            };
            
            uxSurveyPanel.AddTo(this.uxTabPanel);
            uxTabPanel.SetActiveTab(uxSurveyPanel);
        }

        protected void deDeleteForm(object sender, DirectEventArgs e)
        {
            decimal FormId = decimal.Parse(e.ExtraParams["FormId"]);
            List<SURVEY_FORMS_COMP> FormCompletions;
            List<SURVEY_FORMS_ANS> FormAnswers;
            SURVEY_FORMS FormToDelete;
            bool CannotDelete = false;
            using (Entities _context = new Entities())
            {
                FormCompletions = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.FORM_ID == FormId).ToList();
                FormToDelete = CUSTOMER_SURVEYS.GetForms(_context).Where(x => x.FORM_ID == FormId).Single();
            }
            int count = 1;
            if (FormCompletions.Count > 0)
            {
                foreach (SURVEY_FORMS_COMP FormCompletion in FormCompletions)
                {
                    using (Entities _context = new Entities())
                    {
                        FormAnswers = CUSTOMER_SURVEYS.GetFormAnswersByCompletion(FormCompletion.COMPLETION_ID, _context).ToList();
                    }
                    if (FormAnswers.Count > 0)
                    {
                        X.Msg.Alert("Unable to delete", "This form has already been completed and cannot be deleted.").Show();
                        CannotDelete = true;
                        break;
                    }
                    else if (count == FormCompletions.Count)
                    {
                        List<SURVEY_FIELDSETS> Fieldsets;
                        List<SURVEY_QUESTIONS> Questions;
                        List<SURVEY_OPTIONS> Options;
                        List<SURVEY_RELATION> RelationEntries;


                        using (Entities _context = new Entities())
                        {
                            //Get Fieldsets
                            Fieldsets = CUSTOMER_SURVEYS.GetFieldsets(_context).Where(x => x.FORM_ID == FormId).ToList();
                        }
                        foreach (SURVEY_FIELDSETS Fieldset in Fieldsets)
                        {
                            //Get Questions
                            using (Entities _context = new Entities())
                            {
                                Questions = CUSTOMER_SURVEYS.GetFieldsetQuestions(Fieldset.FIELDSET_ID, _context).ToList();
                            }
                            foreach (SURVEY_QUESTIONS Question in Questions)
                            {
                                //Get Options
                                using (Entities _context = new Entities())
                                {
                                    Options = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).ToList();
                                }
                                //Delete Options
                                GenericData.Delete<SURVEY_OPTIONS>(Options);
                            }
                            using (Entities _context = new Entities())
                            {
                                RelationEntries = CUSTOMER_SURVEYS.GetRelationEntries(_context).Where(x => x.FIELDSET_ID == Fieldset.FIELDSET_ID).ToList();
                            }
                            GenericData.Delete<SURVEY_RELATION>(RelationEntries);
                            GenericData.Delete<SURVEY_QUESTIONS>(Questions);


                        }
                        GenericData.Delete<SURVEY_FIELDSETS>(Fieldsets);

                    }
                    count++;
                }
                if (CannotDelete == false)
                {
                    GenericData.Delete<SURVEY_FORMS>(FormToDelete);

                    uxFormsStore.Reload();
                    uxQuestionsStore.Reload();
                    uxFieldsetsStore.Reload();
                    uxOptionsStore.Reload();
                }
            }
            else
            {
                List<SURVEY_FIELDSETS> Fieldsets;
                List<SURVEY_QUESTIONS> Questions;
                List<SURVEY_OPTIONS> Options;
                List<SURVEY_RELATION> RelationEntries;

                using (Entities _context = new Entities())
                {
                    Fieldsets = CUSTOMER_SURVEYS.GetFieldsets(_context).Where(x => x.FORM_ID == FormId).ToList();
                    Questions = CUSTOMER_SURVEYS.GetFormQuestion2(FormId, _context).ToList();
                }    
                
                foreach (SURVEY_QUESTIONS Question in Questions)
                {
                    using (Entities _context = new Entities())
                    {
                        Options = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).ToList();
                    }
                    GenericData.Delete<SURVEY_OPTIONS>(Options);
                }
                foreach (SURVEY_FIELDSETS Fieldset in Fieldsets)
                {
                    using (Entities _context = new Entities())
                    {
                        RelationEntries = CUSTOMER_SURVEYS.GetRelationEntries(_context).Where(x => x.FIELDSET_ID == Fieldset.FIELDSET_ID).ToList();
                    }
                    GenericData.Delete<SURVEY_RELATION>(RelationEntries);
                }
                GenericData.Delete<SURVEY_QUESTIONS>(Questions);
                GenericData.Delete<SURVEY_FIELDSETS>(Fieldsets);
                GenericData.Delete<SURVEY_FORMS>(FormToDelete);

                uxFormsStore.Reload();
                uxQuestionsStore.RemoveAll();
                uxFieldsetsStore.RemoveAll();
                uxOptionsStore.RemoveAll();

                uxDeleteFormButton.Disable();
                uxCopyFormButton.Disable();
                uxViewFormButton.Disable();
                uxAddFieldsetButton.Disable();
                uxDeleteFieldsetButton.Disable();
                uxAddQuestionButton.Disable();
                uxDeleteQuestionButton.Disable();
                uxAddOptionButton.Disable();
                uxDeleteOptionButton.Disable();
            }
            
        }

        protected void deDeleteFieldset(object sender, DirectEventArgs e)
        {
            List<SURVEY_QUESTIONS> Questions;
            
            decimal FieldsetId = decimal.Parse(e.ExtraParams["FieldsetId"]);
            using (Entities _context = new Entities())
            {
                Questions = CUSTOMER_SURVEYS.GetFieldsetQuestions(FieldsetId, _context).ToList();
            }
            int count = 1;
            foreach (SURVEY_QUESTIONS Question in Questions)
            {
                bool QuestionHasBeenFilled;
                using (Entities _context = new Entities())
                {
                    QuestionHasBeenFilled = CUSTOMER_SURVEYS.HasQuestionBeenFilledOut(Question.QUESTION_ID, _context);
                }
                if (QuestionHasBeenFilled)
                {
                    X.Msg.Alert("Unable to delete", "This fieldset has questions that have already been completed.").Show();
                    break;
                }
                else if (count == Questions.Count)
                {
                    List<SURVEY_QUESTIONS> QuestionsToDelete = Questions;
                    List<SURVEY_RELATION> RelationEntries;
                    List<SURVEY_OPTIONS> Options;
                    
                    foreach (SURVEY_QUESTIONS QuestionToDelete in QuestionsToDelete)
                    {
                        //Get Options
                        using (Entities _context = new Entities())
                        {
                            
                            Options = CUSTOMER_SURVEYS.GetQuestionOptions(QuestionToDelete.QUESTION_ID, _context).ToList();
                        }
                        GenericData.Delete<SURVEY_OPTIONS>(Options);
                    }

                    using (Entities _context = new Entities())
                    {
                        RelationEntries = _context.SURVEY_RELATION.Where(x => x.FIELDSET_ID == FieldsetId).ToList();
                    }

                    GenericData.Delete<SURVEY_RELATION>(RelationEntries);
                    GenericData.Delete<SURVEY_QUESTIONS>(QuestionsToDelete);
                    SURVEY_FIELDSETS FieldsetToDelete;
                    using (Entities _context = new Entities())
                    {
                        FieldsetToDelete = CUSTOMER_SURVEYS.GetFieldsets(_context).Where(x => x.FIELDSET_ID == FieldsetId).Single();
                    }
                    GenericData.Delete<SURVEY_FIELDSETS>(FieldsetToDelete);
                }
                count++;
            }
            uxFieldsetsStore.RemoveAt(uxFieldsetSelection.SelectedIndex);
            uxQuestionsStore.Reload();
            uxQuestionFieldsetStore.Reload();
            uxOptionsStore.Reload();
            uxDeleteFieldsetButton.Disable();
            uxDeleteOptionButton.Disable();
            uxDeleteQuestionButton.Disable();
        }

        protected void deDeleteQuestion(object sender, DirectEventArgs e)
        {
            decimal QuestionId = decimal.Parse(e.ExtraParams["QuestionId"]);
            SURVEY_QUESTIONS Question;
            List<SURVEY_OPTIONS> Options;
            SURVEY_RELATION RelationEntry;
            bool QuestionHasBeenAnswered;
            using (Entities _context = new Entities())
            {
                Question = CUSTOMER_SURVEYS.GetQuestion(QuestionId, _context);
                QuestionHasBeenAnswered = CUSTOMER_SURVEYS.HasQuestionBeenFilledOut(QuestionId, _context);
            }
            if (QuestionHasBeenAnswered)
            {
                X.Msg.Alert("Unable to delete", "This question has already been completed and cannot be deleted").Show();
            }
            else
            {
                using (Entities _context = new Entities())
                {
                    Options = CUSTOMER_SURVEYS.GetQuestionOptions(QuestionId, _context).ToList();
                    RelationEntry = CUSTOMER_SURVEYS.GetRelationEntries(_context).Where(x => x.QUESTION_ID == QuestionId).Single();
                }

                GenericData.Delete<SURVEY_OPTIONS>(Options);
                GenericData.Delete<SURVEY_RELATION>(RelationEntry);
                GenericData.Delete<SURVEY_QUESTIONS>(Question);
            }

            uxDeleteQuestionButton.Disable();
            uxDeleteOptionButton.Disable();
            uxQuestionsStore.RemoveAt(uxQuestionSelection.SelectedIndex);
            uxOptionsStore.RemoveAll();
            uxAddOptionButton.Disable();

        }

        protected void deDeleteOption(object sender, DirectEventArgs e)
        {
            decimal OptionId = decimal.Parse(e.ExtraParams["OptionId"]);
            SURVEY_OPTIONS Option;
            using (Entities _context = new Entities())
            {
                Option = CUSTOMER_SURVEYS.GetQuestionOption(OptionId, _context);
            }
            GenericData.Delete<SURVEY_OPTIONS>(Option);
            uxDeleteOptionButton.Disable();
            uxOptionsStore.RemoveAt(uxOptionSelection.SelectedIndex);
        }

        protected void deCopyForm(object sender, DirectEventArgs e)
        {
            decimal FormId = decimal.Parse(e.ExtraParams["FormId"]);
            List<SURVEY_FIELDSETS> ExistingFieldsets;
            List<SURVEY_QUESTIONS> ExistingQuestions;
            List<SURVEY_OPTIONS> ExistingOptions;

            SURVEY_FORMS Form = new SURVEY_FORMS();
            Form.FORMS_NAME = uxCopyFormName.Text;
            Form.ORG_ID = decimal.Parse(uxCopyFormOrg.Value.ToString());
            Form.CATEGORY_ID = decimal.Parse(uxCopyFormCategory.Value.ToString());
            Form.CREATE_DATE = DateTime.Now;
            Form.MODIFY_DATE = DateTime.Now;
            Form.TYPE_ID = decimal.Parse(uxCopyFormType.Value.ToString());
            Form.MODIFIED_BY = User.Identity.Name;
            Form.CREATED_BY = User.Identity.Name;
            GenericData.Insert<SURVEY_FORMS>(Form);

            using (Entities _context = new Entities())
            {
                ExistingFieldsets = CUSTOMER_SURVEYS.GetFieldsets(_context).Where(x => x.FORM_ID == FormId).ToList();
            }

            foreach (SURVEY_FIELDSETS ExistingFieldset in ExistingFieldsets)
            {
                SURVEY_FIELDSETS Fieldset = new SURVEY_FIELDSETS();
                Fieldset.CREATE_DATE = DateTime.Now;
                Fieldset.MODIFY_DATE = DateTime.Now;
                Fieldset.CREATED_BY = User.Identity.Name;
                Fieldset.MODIFIED_BY = User.Identity.Name;
                Fieldset.FORM_ID = Form.FORM_ID;
                Fieldset.IS_ACTIVE = ExistingFieldset.IS_ACTIVE;
                Fieldset.SORT_ORDER = ExistingFieldset.SORT_ORDER;
                Fieldset.TITLE = ExistingFieldset.TITLE;
                Fieldset.CATEGORY_ID = ExistingFieldset.CATEGORY_ID;

                GenericData.Insert<SURVEY_FIELDSETS>(Fieldset);

                using (Entities _context = new Entities())
                {
                    ExistingQuestions = CUSTOMER_SURVEYS.GetFieldsetQuestions(ExistingFieldset.FIELDSET_ID, _context).ToList();
                }

                foreach (SURVEY_QUESTIONS ExistingQuestion in ExistingQuestions)
                {
                    SURVEY_QUESTIONS Question = new SURVEY_QUESTIONS();
                    Question.CREATE_DATE = DateTime.Now;
                    Question.MODIFY_DATE = DateTime.Now;
                    Question.CREATED_BY = User.Identity.Name;
                    Question.MODIFIED_BY = User.Identity.Name;
                    Question.IS_ACTIVE = ExistingQuestion.IS_ACTIVE;
                    Question.IS_REQUIRED = ExistingQuestion.IS_REQUIRED;
                    Question.SORT_ORDER = ExistingQuestion.SORT_ORDER;
                    Question.TEXT = ExistingQuestion.TEXT;
                    Question.TYPE_ID = ExistingQuestion.TYPE_ID;
                    Question.SORT_ORDER = ExistingQuestion.SORT_ORDER;

                    GenericData.Insert<SURVEY_QUESTIONS>(Question);

                    SURVEY_RELATION Relation = new SURVEY_RELATION();
                    Relation.FIELDSET_ID = Fieldset.FIELDSET_ID;
                    Relation.QUESTION_ID = Question.QUESTION_ID;
                    Relation.CREATE_DATE = DateTime.Now;
                    Relation.MODIFY_DATE = DateTime.Now;
                    Relation.CREATED_BY = User.Identity.Name;
                    Relation.MODIFIED_BY = User.Identity.Name;

                    GenericData.Insert<SURVEY_RELATION>(Relation);

                    using (Entities _context = new Entities())
                    {
                        ExistingOptions = CUSTOMER_SURVEYS.GetQuestionOptions(ExistingQuestion.QUESTION_ID, _context).ToList();
                    }
                    foreach (SURVEY_OPTIONS ExistingOption in ExistingOptions)
                    {
                        SURVEY_OPTIONS Option = new SURVEY_OPTIONS();
                        Option.CREATE_DATE = DateTime.Now;
                        Option.MODIFY_DATE = DateTime.Now;
                        Option.CREATED_BY = User.Identity.Name;
                        Option.MODIFIED_BY = User.Identity.Name;
                        Option.IS_ACTIVE = ExistingOption.IS_ACTIVE;
                        Option.QUESTION_ID = Question.QUESTION_ID;
                        Option.OPTION_NAME = ExistingOption.OPTION_NAME;
                        Option.SORT_ORDER = ExistingOption.SORT_ORDER;

                        GenericData.Insert<SURVEY_OPTIONS>(Option);
                    }
                }
            }

            uxCopyFormWindow.Hide();
            uxCopyForm.Reset();
        }

        protected void deCreateTargetWindow(object sender, DirectEventArgs e)
        {
            CreateWindow("/Views/Modules/CustomerSurveys/umEditFormTarget.aspx", "Add Form Target");
        }

        protected void CreateWindow(string LoaderUrl, string Title)
        {
            Window win = new Window()
            {
                ID = "uxPlaceholderWindow",
                Title = Title,
                Width = 600,
                Modal = true,
                Resizable = false,
                AutoRender = false,
                Y = 15,
                Constrain = false,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = LoaderUrl,
                    DisableCaching = true,
                    Mode = LoadMode.Frame,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            this.Form.Controls.Add(win);
            win.Render(this.Form);
            win.Show();
        }

        protected void deReadQuestionCategories(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxQuestionCategoryStore.DataSource = _context.SURVEY_QUES_CAT.ToList();
            }
        }

        protected void deReadFormTypes(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<SURVEY_TYPES> FormTypes = _context.SURVEY_TYPES.ToList();
                uxFormTypeStore.DataSource = FormTypes;
                uxCopyFormTypeStore.DataSource = FormTypes;
            }
        }
    }
}