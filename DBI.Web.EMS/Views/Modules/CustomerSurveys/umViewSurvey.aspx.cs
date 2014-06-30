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
            LoadForm(long.Parse(Request.QueryString["FormId"]));
        }

        protected void LoadForm(long FormId)
        {
            using (Entities _context = new Entities())
            {
                List<CUSTOMER_SURVEYS.CustomerSurveyFieldsets> Fieldsets = CUSTOMER_SURVEYS.GetFormFieldSets(FormId, _context).Where(x => x.IS_ACTIVE == true).ToList();

                foreach (CUSTOMER_SURVEYS.CustomerSurveyFieldsets Fieldset in Fieldsets)
                {
                    var QuestionQuery = CUSTOMER_SURVEYS.GetFieldsetQuestions(Fieldset.FIELDSET_ID, _context).Where(x => x.IS_ACTIVE == true);
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
                                    TextField.LabelWidth = 150;
                                    NewFieldset.Items.Add(TextField);
                                    break;
                                case 2:
                                    TextArea TextArea = new TextArea();
                                    TextArea.ID = "question" + Question.QUESTION_ID.ToString();
                                    TextArea.AllowBlank = !Question.IS_REQUIRED;
                                    TextArea.FieldLabel = Question.TEXT;
                                    TextArea.LabelWidth = 150;
                                    NewFieldset.Items.Add(TextArea);
                                    break;
                                case 5:
                                    ComboBox Combobox = new ComboBox();
                                    Combobox.ID = "question" + Question.QUESTION_ID;
                                    Combobox.FieldLabel = Question.TEXT;
                                    Combobox.LabelWidth = 150;
                                    Combobox.AllowBlank = !Question.IS_REQUIRED;
                                    Combobox.TypeAhead = true;
                                    Combobox.ForceSelection = true;
                                    Combobox.QueryMode = DataLoadMode.Local;
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
                                    RadioQuestion.ColumnsNumber = 1;
                                    List<CUSTOMER_SURVEY_OPTIONS> RadioOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).OrderBy(x => x.SORT_ORDER).ToList();
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
                                    CheckQuestion.LabelWidth = 150;
                                    CheckQuestion.AllowBlank = !Question.IS_REQUIRED;
                                    CheckQuestion.ColumnsNumber = 1;
                                    List<CUSTOMER_SURVEY_OPTIONS> CheckOptions = CUSTOMER_SURVEYS.GetQuestionOptions(Question.QUESTION_ID, _context).OrderBy(x => x.SORT_ORDER).ToList();
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
                            }
                        }
                    }
                }
            }
        }
    }
}