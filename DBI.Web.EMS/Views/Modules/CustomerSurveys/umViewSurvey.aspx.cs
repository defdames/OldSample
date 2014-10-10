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
    public partial class umViewSurvey : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            decimal FormId = decimal.Parse(Request.QueryString["FormId"]);
            if (Request.QueryString["CompletionId"] == null)
            {
                LoadForm(FormId);
            }
            else
            {
                decimal CompletionId = decimal.Parse(Request.QueryString["CompletionId"]);
                LoadForm(FormId, CompletionId);
                uxCodeFieldset.Hide();
                uxFormCode.Value = CompletionId;
                uxLogoImage.Hide();
                uxLogoContainer.Hide();
                

                if (Request.QueryString["Print"] != "print")
                {
                    uxSurveyContainer.LayoutConfig.Add(new HBoxLayoutConfig { Pack = BoxPack.Center, ReserveScrollbar = true });
                }
                else
                {
                    uxSurveyDisplay.MaxWidth = 600;
                    uxSubmitSurveyButton.Hide();
                    uxCancelSurveyButton.Hide();
                   
                }
            }
            uxSurveyContainer.Show();

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Request.QueryString["Print"] == "print")
            {
                if (!IsPostBack)
                {
                    X.Js.Call("window.print()");
                }
            }
        }

        protected void LoadForm(decimal FormId)
        {
            using (Entities _context = new Entities())
            {
                List<CUSTOMER_SURVEYS.CustomerSurveyFieldsets> Fieldsets = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context).Where(x => x.IS_ACTIVE == true).OrderBy(x => x.SORT_ORDER).ToList();

                foreach (CUSTOMER_SURVEYS.CustomerSurveyFieldsets Fieldset in Fieldsets)
                {
                    var QuestionQuery = CUSTOMER_SURVEYS.GetFieldsetQuestionsForGrid(Fieldset.FIELDSET_ID, _context).Where(x => x.IS_ACTIVE == true).OrderBy(x => x.SORT_ORDER);
                    List<CUSTOMER_SURVEYS.CustomerSurveyQuestions> Questions = QuestionQuery.ToList();
                    if (Questions.Count > 0)
                    {
                        FieldSet NewFieldset = new FieldSet();
                        NewFieldset.ID = "fieldset" + Fieldset.FIELDSET_ID;
                        NewFieldset.Title = Fieldset.TITLE;
                        NewFieldset.Margin = 5;
                        uxSurveyDisplay.Items.Add(NewFieldset);
                        foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestions Question in Questions)
                        {
                            switch ((long)Question.TYPE_ID)
                            {
                                case 1:
                                    TextField TextField = new TextField();
                                    TextField.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextField.AllowBlank = !Question.IS_REQUIRED;
                                    TextField.FieldLabel = Question.TEXT;
                                    TextField.LabelWidth = 250;
                                    TextField.Width = 600;
                                    TextField.InvalidCls = "empty";
                                    if (Question.IS_REQUIRED)
                                    {
                                        TextField.MsgTarget = MessageTarget.Side;
                                        TextField.FieldCls = "allowBlank";
                                    }
                                    TextField.ValidateBlank = true;
                                    NewFieldset.Items.Add(TextField);
                                    break;
                                case 2:
                                    TextArea TextArea = new TextArea();
                                    TextArea.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextArea.AllowBlank = !Question.IS_REQUIRED;
                                    TextArea.FieldLabel = Question.TEXT;
                                    TextArea.LabelWidth = 250;
                                    if (Question.IS_REQUIRED)
                                    {
                                        TextArea.IndicatorIcon = Icon.BulletRed;
                                        TextArea.MsgTarget = MessageTarget.Side;
                                    }
                                    TextArea.Width = 600;
                                    TextArea.InvalidCls = "allowBlank";
                                    NewFieldset.Items.Add(TextArea);
                                    break;
                                case 5:
                                    ComboBox Combobox = new ComboBox();
                                    Combobox.ID = "question" + Question.QUESTION_ID;
                                    Combobox.FieldLabel = Question.TEXT;
                                    Combobox.LabelWidth = 250;
                                    Combobox.Width = 600;
                                    Combobox.AllowBlank = !Question.IS_REQUIRED;
                                    Combobox.InvalidCls = "allowBlank";
                                    if (Question.IS_REQUIRED)
                                    {
                                        Combobox.IndicatorIcon = Icon.BulletRed;
                                        Combobox.MsgTarget = MessageTarget.Side;
                                    }
                                    Combobox.TypeAhead = true;
                                    Combobox.ForceSelection = true;
                                    Combobox.QueryMode = DataLoadMode.Local;
                                    List<CUSTOMER_SURVEY_OPTIONS> ComboOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();
                                    foreach (CUSTOMER_SURVEY_OPTIONS Option in ComboOptions)
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
                                    RadioQuestion.ID = "question" + Question.QUESTION_ID;
                                    RadioQuestion.FieldLabel = Question.TEXT;
                                    RadioQuestion.AllowBlank = !Question.IS_REQUIRED;
                                    RadioQuestion.InvalidCls = "allowBlank";
                                    if (Question.IS_REQUIRED)
                                    {
                                        RadioQuestion.IndicatorIcon = Icon.BulletRed;
                                        RadioQuestion.MsgTarget = MessageTarget.Side;
                                    }
                                    RadioQuestion.LabelWidth = 250;
                                    
                                    List<CUSTOMER_SURVEY_OPTIONS> RadioOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();
                                    RadioQuestion.ColumnsNumber = RadioOptions.Count;
                                    foreach (CUSTOMER_SURVEY_OPTIONS Option in RadioOptions)
                                    {
                                        RadioQuestion.Items.Add(new Radio
                                        {
                                            BoxLabelAlign = BoxLabelAlign.After,
                                            BoxLabel = Option.OPTION_NAME,
                                            Value = Option.OPTION_NAME,
                                        });
                                    }
                                    
                                    NewFieldset.Items.Add(RadioQuestion);

                                    break;
                                case 7:
                                    CheckboxGroup CheckQuestion = new CheckboxGroup();
                                    CheckQuestion.ID = "question" + Question.QUESTION_ID;
                                    CheckQuestion.FieldLabel = Question.TEXT;
                                    CheckQuestion.LabelWidth = 250;
                                    CheckQuestion.InvalidCls = "allowBlank";
                                    if (Question.IS_REQUIRED)
                                    {
                                        CheckQuestion.IndicatorIcon = Icon.BulletRed;
                                        CheckQuestion.MsgTarget = MessageTarget.Side;
                                    }
                                    CheckQuestion.AllowBlank = !Question.IS_REQUIRED;

                                    List<CUSTOMER_SURVEY_OPTIONS> CheckOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).Where(x => x.IS_ACTIVE == "Y").OrderBy(x => x.SORT_ORDER).ToList();
                                    CheckQuestion.ColumnsNumber = CheckOptions.Count;

                                    foreach (CUSTOMER_SURVEY_OPTIONS Option in CheckOptions)
                                    {
                                        CheckQuestion.Items.Add(new Checkbox
                                        {
                                            BoxLabelAlign = BoxLabelAlign.After,
                                            BoxLabel = Option.OPTION_NAME,
                                            Value = Option.OPTION_NAME
                                        });
                                    }
                                    NewFieldset.Items.Add(CheckQuestion);
                                    break;
                                case 21:
                                    DateField DateQuestion = new DateField();
                                    DateQuestion.ID = "question" + Question.QUESTION_ID;
                                    DateQuestion.FieldLabel = Question.TEXT;
                                    DateQuestion.LabelWidth = 250;
                                    DateQuestion.AllowBlank = !Question.IS_REQUIRED;
                                    DateQuestion.InvalidCls = "allowBlank";
                                    if (Question.IS_REQUIRED)
                                    {
                                        DateQuestion.IndicatorIcon = Icon.BulletRed;
                                        DateQuestion.MsgTarget = MessageTarget.Side;
                                    }
                                    NewFieldset.Items.Add(DateQuestion);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void LoadForm(decimal FormId, decimal CompletionId)
        {
            using (Entities _context = new Entities())
            {
                List<CUSTOMER_SURVEYS.CustomerSurveyFieldsets> Fieldsets = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context).Where(x => x.IS_ACTIVE == true).OrderBy(x => x.SORT_ORDER).ToList();

                string FilledBy = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.COMPLETION_ID == CompletionId && x.FILLED_ON == null).Select(x => x.FILLED_BY).SingleOrDefault();
                if (!string.IsNullOrEmpty(FilledBy))
                {
                    uxSubmitSurveyButton.Hide();
                    uxCancelSurveyButton.Hide();
                }
                foreach (CUSTOMER_SURVEYS.CustomerSurveyFieldsets Fieldset in Fieldsets)
                {
                    var QuestionQuery = CUSTOMER_SURVEYS.GetFieldsetQuestionsForGrid(Fieldset.FIELDSET_ID, _context).OrderBy(x => x.SORT_ORDER);
                    List<CUSTOMER_SURVEYS.CustomerSurveyQuestions> Questions = QuestionQuery.ToList();
                    uxFormCode.Value = CompletionId;
                    if (Questions.Count > 0)
                    {
                        FieldSet NewFieldset = new FieldSet();
                        NewFieldset.ID = "fieldset" + Fieldset.FIELDSET_ID;
                        NewFieldset.Title = Fieldset.TITLE;
                        NewFieldset.Margin = 5;
                        uxSurveyDisplay.Items.Add(NewFieldset);
                        foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestions Question in Questions)
                        {

                            CUSTOMER_SURVEY_FORMS_ANS Answer = CUSTOMER_SURVEYS.GetFormAnswerByQuestion(Question.QUESTION_ID, _context).Where(x => x.COMPLETION_ID == CompletionId).SingleOrDefault();
                            switch ((long)Question.TYPE_ID)
                            {
                                case 1:
                                    TextField TextField = new TextField();
                                    TextField.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextField.AllowBlank = !Question.IS_REQUIRED;
                                    TextField.FieldLabel = Question.TEXT;
                                    if (!string.IsNullOrEmpty(FilledBy))
                                    {
                                        TextField.ReadOnly = true;
                                    }
                                    TextField.LabelWidth = 250;
                                    TextField.Width = 600;
                                    try
                                    {
                                        TextField.Value = Answer.ANSWER;
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    NewFieldset.Items.Add(TextField);
                                    break;


                                case 2:
                                    TextArea TextArea = new TextArea();
                                    TextArea.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextArea.AllowBlank = !Question.IS_REQUIRED;
                                    TextArea.FieldLabel = Question.TEXT;
                                    if (!string.IsNullOrEmpty(FilledBy))
                                    {
                                        TextArea.ReadOnly = true;
                                    }
                                    TextArea.LabelWidth = 150;
                                    TextArea.Width = 600;
                                    try
                                    {
                                        TextArea.Value = Answer.ANSWER;
                                    }
                                    catch (Exception e) { }
                                    NewFieldset.Items.Add(TextArea);
                                    break;
                                case 5:
                                    ComboBox Combobox = new ComboBox();
                                    Combobox.ID = "question" + Question.QUESTION_ID;
                                    Combobox.FieldLabel = Question.TEXT;
                                    Combobox.LabelWidth = 150;
                                    Combobox.Width = 600;
                                    Combobox.AllowBlank = !Question.IS_REQUIRED;
                                    Combobox.TypeAhead = true;
                                    Combobox.ForceSelection = true;
                                    if (!string.IsNullOrEmpty(FilledBy))
                                    {
                                        Combobox.ReadOnly = true;
                                    }
                                    Combobox.QueryMode = DataLoadMode.Local;
                                    try
                                    {
                                        Combobox.Select(Answer.ANSWER);
                                    }
                                    catch (Exception e) { }
                                    List<CUSTOMER_SURVEY_OPTIONS> ComboOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).OrderBy(x => x.SORT_ORDER).ToList();
                                    foreach (CUSTOMER_SURVEY_OPTIONS Option in ComboOptions)
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
                                    RadioQuestion.ID = "question" + Question.QUESTION_ID;
                                    RadioQuestion.FieldLabel = Question.TEXT;
                                    RadioQuestion.AllowBlank = !Question.IS_REQUIRED;
                                    RadioQuestion.LabelWidth = 150;
                                    RadioQuestion.LabelAlign = LabelAlign.Top;
                                    
                                    List<CUSTOMER_SURVEY_OPTIONS> RadioOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).OrderBy(x => x.SORT_ORDER).ToList();
                                    RadioQuestion.ColumnsNumber = RadioOptions.Count;
                                    foreach (CUSTOMER_SURVEY_OPTIONS Option in RadioOptions)
                                    {
                                        try
                                        {
                                            RadioQuestion.Items.Add(new Radio
                                            {
                                                BoxLabelAlign = BoxLabelAlign.After,
                                                BoxLabel = Option.OPTION_NAME,
                                                Value = Option.OPTION_NAME,
                                                Checked = (Answer.ANSWER == Option.OPTION_NAME ? true : false),
                                                Disabled = !string.IsNullOrEmpty(FilledBy)
                                            });
                                        }
                                        catch (Exception e)
                                        {
                                            RadioQuestion.Items.Add(new Radio
                                            {
                                                BoxLabelAlign = BoxLabelAlign.After,
                                                BoxLabel = Option.OPTION_NAME,
                                                Value = Option.OPTION_NAME,
                                                Checked = false,
                                                Disabled = !string.IsNullOrEmpty(FilledBy)
                                            });
                                        }
                                    }
                                    
                                    NewFieldset.Items.Add(RadioQuestion);

                                    break;
                                case 7:
                                    CheckboxGroup CheckQuestion = new CheckboxGroup();
                                    CheckQuestion.ID = "question" + Question.QUESTION_ID;
                                    CheckQuestion.FieldLabel = Question.TEXT;
                                    CheckQuestion.LabelWidth = 150;
                                    CheckQuestion.AllowBlank = !Question.IS_REQUIRED;
                                    CheckQuestion.LabelAlign = LabelAlign.Top;
                                    
                                    List<CUSTOMER_SURVEY_OPTIONS> CheckOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).OrderBy(x => x.SORT_ORDER).ToList();
                                    CheckQuestion.ColumnsNumber = CheckOptions.Count;
                                    foreach (CUSTOMER_SURVEY_OPTIONS Option in CheckOptions)
                                    {
                                        try
                                        {
                                            CheckQuestion.Items.Add(new Checkbox
                                            {
                                                BoxLabelAlign = BoxLabelAlign.After,
                                                BoxLabel = Option.OPTION_NAME,
                                                Value = Option.OPTION_NAME,
                                                Checked = (Answer.ANSWER == Option.OPTION_NAME ? true : false),
                                                Disabled = !string.IsNullOrEmpty(FilledBy)
                                            });
                                        }
                                        catch (Exception e)
                                        {
                                            CheckQuestion.Items.Add(new Checkbox
                                            {
                                                BoxLabelAlign = BoxLabelAlign.After,
                                                BoxLabel = Option.OPTION_NAME,
                                                Value = Option.OPTION_NAME,
                                                Checked = false,
                                                Disabled = !string.IsNullOrEmpty(FilledBy)
                                            });
                                        }
                                    }
                                    NewFieldset.Items.Add(CheckQuestion);
                                    break;
                                case 21:
                                    DateField DateQuestion = new DateField();
                                    DateQuestion.ID = "question" + Question.QUESTION_ID;
                                    DateQuestion.FieldLabel = Question.TEXT;
                                    DateQuestion.LabelWidth = 150;
                                    DateQuestion.Width = 600;
                                    DateQuestion.AllowBlank = !Question.IS_REQUIRED;
                                    try
                                    {
                                        DateQuestion.Value = Answer.ANSWER;
                                    }
                                    catch (Exception e) { }
                                    DateQuestion.ReadOnly = !string.IsNullOrEmpty(FilledBy);
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
            decimal CompletionId = decimal.Parse(uxFormCode.Value.ToString());
            decimal FormId;
            List<CUSTOMER_SURVEYS.CustomerSurveyQuestions> QuestionList;
            CUSTOMER_SURVEY_FORMS_COMP Completion;
            using (Entities _context = new Entities())
            {
                FormId = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.COMPLETION_ID == CompletionId).Select(x => x.FORM_ID).Single();
                QuestionList = CUSTOMER_SURVEYS.GetFormQuestions(FormId, _context).ToList();
                Completion = CUSTOMER_SURVEYS.GetFormCompletion(_context).Where(x => x.COMPLETION_ID == CompletionId).SingleOrDefault();
            }

            if (Completion != null)
            {
                Completion.FILLED_BY = User.Identity.Name;
                Completion.FILLED_ON = DateTime.Now;
                GenericData.Update<CUSTOMER_SURVEY_FORMS_COMP>(Completion);

                foreach (CUSTOMER_SURVEYS.CustomerSurveyQuestions Question in QuestionList)
                {
                    TextField TextValue;
                    TextArea AreaValue;
                    ComboBox ComboValue;
                    CUSTOMER_SURVEY_FORMS_ANS AnswerToAdd;

                    switch (Question.QUESTION_TYPE_NAME)
                    {
                        case "singletext":
                            TextValue = form1.FindControl("question" + Question.QUESTION_ID.ToString()) as TextField;
                            AnswerToAdd = new CUSTOMER_SURVEY_FORMS_ANS();
                            AnswerToAdd.COMPLETION_ID = CompletionId;
                            AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                            AnswerToAdd.ANSWER = TextValue.Text;
                            AnswerToAdd.CREATE_DATE = DateTime.Now;
                            AnswerToAdd.MODIFY_DATE = DateTime.Now;
                            AnswerToAdd.CREATED_BY = User.Identity.Name;
                            AnswerToAdd.MODIFIED_BY = User.Identity.Name;
                            break;
                        case "multitext":
                            AreaValue = form1.FindControl("question" + Question.QUESTION_ID.ToString()) as TextArea;
                            AnswerToAdd = new CUSTOMER_SURVEY_FORMS_ANS();
                            AnswerToAdd.COMPLETION_ID = CompletionId;
                            AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                            AnswerToAdd.ANSWER = AreaValue.Text;
                            AnswerToAdd.CREATE_DATE = DateTime.Now;
                            AnswerToAdd.MODIFY_DATE = DateTime.Now;
                            AnswerToAdd.CREATED_BY = User.Identity.Name;
                            AnswerToAdd.MODIFIED_BY = User.Identity.Name;
                            break;
                        case "dropdown":
                            ComboValue = form1.FindControl("question" + Question.QUESTION_ID.ToString()) as ComboBox;
                            AnswerToAdd = new CUSTOMER_SURVEY_FORMS_ANS();
                            AnswerToAdd.COMPLETION_ID = CompletionId;
                            AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                            AnswerToAdd.ANSWER = ComboValue.Text;
                            AnswerToAdd.CREATE_DATE = DateTime.Now;
                            AnswerToAdd.MODIFY_DATE = DateTime.Now;
                            AnswerToAdd.CREATED_BY = User.Identity.Name;
                            AnswerToAdd.MODIFIED_BY = User.Identity.Name;
                            break;
                        case "radio":
                            AnswerToAdd = new CUSTOMER_SURVEY_FORMS_ANS();
                            AnswerToAdd.COMPLETION_ID = CompletionId;
                            AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                            AnswerToAdd.ANSWER = Request["question" + Question.QUESTION_ID.ToString()];
                            AnswerToAdd.CREATE_DATE = DateTime.Now;
                            AnswerToAdd.MODIFY_DATE = DateTime.Now;
                            AnswerToAdd.CREATED_BY = User.Identity.Name;
                            AnswerToAdd.MODIFIED_BY = User.Identity.Name;
                            break;
                        case "checkbox":
                            AnswerToAdd = new CUSTOMER_SURVEY_FORMS_ANS();
                            AnswerToAdd.COMPLETION_ID = CompletionId;
                            AnswerToAdd.QUESTION_ID = Question.QUESTION_ID;
                            AnswerToAdd.ANSWER = Request["question" + Question.QUESTION_ID.ToString()];
                            AnswerToAdd.CREATE_DATE = DateTime.Now;
                            AnswerToAdd.MODIFY_DATE = DateTime.Now;
                            AnswerToAdd.CREATED_BY = User.Identity.Name;
                            AnswerToAdd.MODIFIED_BY = User.Identity.Name;
                            break;
                        default:
                            AnswerToAdd = null;
                            break;
                    }

                    GenericData.Insert<CUSTOMER_SURVEY_FORMS_ANS>(AnswerToAdd);
                }
            }
            else
            {
                X.Msg.Alert("Form does not exist", "The Form Code you've entered does not exist");
            }
        }

        protected void deLoadCustomer(object sender, DirectEventArgs e)
        {
            
            decimal CompletionId = 0;
            if (uxFormCode.Value != null)
            {
                try
                {
                    CompletionId = decimal.Parse(uxFormCode.Value.ToString());
                }
                catch (Exception ex)
                {
                    X.Msg.Alert("Error", "Invalid Form Code entered, please try again.").Show();
                    uxFormCode.Clear();
                }
            }
            PROJECT_CONTACTS_V Contact;
            string ProjectName = null;
            using (Entities _context = new Entities()){
                long ProjectID = _context.CUSTOMER_SURVEY_FORMS_COMP.Where(x => x.COMPLETION_ID == CompletionId).Select(x => (long)x.PROJECT_ID).SingleOrDefault();
                if (ProjectID != null)
                {
                    ProjectName = _context.PROJECTS_V.Where(x => x.PROJECT_ID == ProjectID).Select(x => x.LONG_NAME).SingleOrDefault();
                }
            }
            if (ProjectName != null)
            {
                uxCustomerField.Text = ProjectName;
            }
            else
            {
                uxCustomerField.Text = "";
            }
        }
    }
}