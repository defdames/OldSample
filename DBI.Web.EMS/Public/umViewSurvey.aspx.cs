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

namespace DBI.Web.EMS.PublicPages
{
    public partial class umViewSurvey : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            decimal CompletionId = decimal.Parse(RSAClass.Decrypt(Request.QueryString["FormId"]));
            
            decimal FormId;
            using(Entities _context = new Entities()){
                if (CUSTOMER_SURVEYS.GetFormAnswersByCompletion(CompletionId, _context).ToList().Count == 0)
                {
                    FormId = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.COMPLETION_ID == CompletionId).Select(x => x.FORM_ID).Single();
                    LoadForm(FormId);
                    uxSurveyContainer.Show();
                    uxCompletedContainer.Hide();
                }
                else
                {
                    uxSurveyContainer.Hide();
                    uxCompletedContainer.Show();
                }
            }
            
        }

        protected void LoadForm(decimal FormId)
        {
            using (Entities _context = new Entities())
            {
                List<SURVEY_FIELDSETS> Fieldsets = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();

                foreach (SURVEY_FIELDSETS Fieldset in Fieldsets)
                {
                    var QuestionQuery = CUSTOMER_SURVEYS.GetFieldsetQuestionsForGrid(Fieldset.FIELDSET_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER);
                    List<SURVEY_QUESTIONS> Questions = QuestionQuery.ToList();
                    if (Questions.Count > 0)
                    {
                        FieldSet NewFieldset = new FieldSet();
                        NewFieldset.ID = "fieldset" + Fieldset.FIELDSET_ID;
                        NewFieldset.Title = Fieldset.TITLE;
                        NewFieldset.Margin = 5;
                        
                        uxSurveyDisplay.Items.Add(NewFieldset);
                        foreach (SURVEY_QUESTIONS Question in Questions)
                        {
                            switch ((long)Question.TYPE_ID)
                            {
                                case 1:
                                    TextField TextField = new TextField();
                                    TextField.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextField.AllowBlank = !Question.REQUIRED;
                                    TextField.FieldLabel = Question.TEXT;
                                    TextField.LabelWidth = 150;
                                    NewFieldset.Items.Add(TextField);
                                    break;
                                case 2:
                                    TextArea TextArea = new TextArea();
                                    TextArea.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextArea.AllowBlank = !Question.REQUIRED;
                                    TextArea.FieldLabel = Question.TEXT;
                                    TextArea.LabelWidth = 150;
                                    NewFieldset.Items.Add(TextArea);
                                    break;
                                case 5:
                                    ComboBox Combobox = new ComboBox();
                                    Combobox.ID = "question" + Question.QUESTION_ID;
                                    Combobox.FieldLabel = Question.TEXT;
                                    Combobox.LabelWidth = 150;
                                    Combobox.AllowBlank = !Question.REQUIRED;
                                    Combobox.TypeAhead = true;
                                    Combobox.ForceSelection = true;
                                    Combobox.QueryMode = DataLoadMode.Local;
                                    List<SURVEY_OPTIONS> ComboOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();
                                    foreach (SURVEY_OPTIONS Option in ComboOptions)
                                    {
                                        Combobox.Items.Add(new Ext.Net.ListItem
                                        {
                                            Text = Option.OPTION_NAME,
                                            Value = Option.OPTION_NAME
                                        });
                                    }
                                    NewFieldset.Items.Add(Combobox);
                                    break;
                                case 6:
                                    RadioGroup RadioQuestion = new RadioGroup();
                                    RadioQuestion.GroupName = "question" + Question.QUESTION_ID;
                                    RadioQuestion.Layout = "HBoxLayout";
                                    RadioQuestion.ID = "question" + Question.QUESTION_ID;
                                    RadioQuestion.FieldLabel = Question.TEXT;
                                    RadioQuestion.AllowBlank = !Question.REQUIRED;
                                    RadioQuestion.LabelWidth = 150;
                                    RadioQuestion.ColumnsNumber = 1;
                                    List<SURVEY_OPTIONS> RadioOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();
                                    foreach (SURVEY_OPTIONS Option in RadioOptions)
                                    {
                                        RadioQuestion.Items.Add(new Radio
                                        {
                                            ID = "option" + Option.OPTION_ID,
                                            Name = "question" + Question.QUESTION_ID,
                                            BoxLabelAlign = BoxLabelAlign.After,
                                            BoxLabel = Option.OPTION_NAME,
                                            Value = Option.OPTION_NAME,
                                            InputValue = Option.OPTION_NAME,
                                            Flex = 1
                                        });
                                    }
                                    NewFieldset.Items.Add(RadioQuestion);

                                    break;
                                case 7:
                                    CheckboxGroup CheckQuestion = new CheckboxGroup();
                                    CheckQuestion.ID = "question" + Question.QUESTION_ID;
                                    CheckQuestion.FieldLabel = Question.TEXT;
                                    CheckQuestion.LabelWidth = 150;
                                    CheckQuestion.AllowBlank = !Question.REQUIRED;
                                    CheckQuestion.ColumnsNumber = 1;
                                    List<SURVEY_OPTIONS> CheckOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();
                                    foreach (SURVEY_OPTIONS Option in CheckOptions)
                                    {
                                        CheckQuestion.Add(new Checkbox
                                        {
                                            ID="option" + Option.OPTION_ID,
                                            Name = "question" + Question.QUESTION_ID,
                                            BoxLabelAlign = BoxLabelAlign.After,
                                            BoxLabel = Option.OPTION_NAME,
                                            Value = Option.OPTION_NAME,
                                            InputValue = Option.OPTION_NAME,
                                        });
                                    }
                                    NewFieldset.Items.Add(CheckQuestion);
                                    break;
                                case 21:
                                    DateField DateQuestion = new DateField();
                                    DateQuestion.ID = "question" + Question.QUESTION_ID;
                                    DateQuestion.FieldLabel = Question.TEXT;
                                    DateQuestion.LabelWidth = 150;
                                    DateQuestion.AllowBlank = !Question.REQUIRED;

                                    NewFieldset.Items.Add(DateQuestion);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void deSaveSurvey(object sender, DirectEventArgs e)
        {
            decimal CompletionId = decimal.Parse(RSAClass.Decrypt(Request.QueryString["FormId"]));
            decimal FormId;
            List<SURVEY_QUESTIONS> QuestionList;
            using (Entities _context = new Entities())
            {
                FormId = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.COMPLETION_ID == CompletionId).Select(x => x.FORM_ID).Single();
                QuestionList = CUSTOMER_SURVEYS.GetFormQuestions(FormId, _context).ToList();
            }

            foreach (SURVEY_QUESTIONS Question in QuestionList)
            {
                TextField TextValue;
                TextArea AreaValue;
                ComboBox ComboValue;
                SURVEY_FORMS_ANS AnswerToAdd;

                switch (Question.QUESTION_TYPE_NAME)
                {
                    case "singletext":
                        TextValue = form1.FindControl("question" + Question.QUESTION_ID.ToString()) as TextField;
                        AnswerToAdd = new SURVEY_FORMS_ANS();
                        AnswerToAdd.COMPLETION_ID = CompletionId;
                        AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                        AnswerToAdd.ANSWER = TextValue.Text;
                        AnswerToAdd.CREATE_DATE = DateTime.Now;
                        AnswerToAdd.MODIFY_DATE = DateTime.Now;
                        AnswerToAdd.CREATED_BY = "Customer";
                        AnswerToAdd.MODIFIED_BY = "Customer";
                        break;
                    case "multitext":
                        AreaValue = form1.FindControl("question" + Question.QUESTION_ID.ToString()) as TextArea;
                        AnswerToAdd = new SURVEY_FORMS_ANS();
                        AnswerToAdd.COMPLETION_ID = CompletionId;
                        AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                        AnswerToAdd.ANSWER = AreaValue.Text;
                        AnswerToAdd.CREATE_DATE = DateTime.Now;
                        AnswerToAdd.MODIFY_DATE = DateTime.Now;
                        AnswerToAdd.CREATED_BY = "Customer";
                        AnswerToAdd.MODIFIED_BY = "Customer";
                        break;
                    case "dropdown":
                        ComboValue = form1.FindControl("question" + Question.QUESTION_ID.ToString()) as ComboBox;
                        AnswerToAdd = new SURVEY_FORMS_ANS();
                        AnswerToAdd.COMPLETION_ID = CompletionId;
                        AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                        AnswerToAdd.ANSWER = ComboValue.Text;
                        AnswerToAdd.CREATE_DATE = DateTime.Now;
                        AnswerToAdd.MODIFY_DATE = DateTime.Now;
                        AnswerToAdd.CREATED_BY = "Customer";
                        AnswerToAdd.MODIFIED_BY = "Customer";
                        break;
                    case "radio":
                        AnswerToAdd = new SURVEY_FORMS_ANS();
                        AnswerToAdd.COMPLETION_ID = CompletionId;
                        AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                        AnswerToAdd.ANSWER = Request["question" + Question.QUESTION_ID.ToString()];
                        AnswerToAdd.CREATE_DATE = DateTime.Now;
                        AnswerToAdd.MODIFY_DATE = DateTime.Now;
                        AnswerToAdd.CREATED_BY = "Customer";
                        AnswerToAdd.MODIFIED_BY = "Customer";
                        break;
                    case "checkbox":
                        AnswerToAdd = new SURVEY_FORMS_ANS();
                        AnswerToAdd.COMPLETION_ID = CompletionId;
                        AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                        AnswerToAdd.ANSWER = Request["question" + Question.QUESTION_ID.ToString()];
                        AnswerToAdd.CREATE_DATE = DateTime.Now;
                        AnswerToAdd.MODIFY_DATE = DateTime.Now;
                        AnswerToAdd.CREATED_BY = "Customer";
                        AnswerToAdd.MODIFIED_BY = "Customer";
                        break;
                    default:
                        AnswerToAdd = null;
                        break;
                }

                GenericData.Insert<SURVEY_FORMS_ANS>(AnswerToAdd);
            }
        }
    }
}