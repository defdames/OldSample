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
                List<CUSTOMER_SURVEY_FIELDSETS> Fieldsets = CUSTOMER_SURVEY_FORMS.GetFormFieldSets(FormId, _context).Where(x => x.IS_ACTIVE == "Y").ToList();

                foreach (CUSTOMER_SURVEY_FIELDSETS Fieldset in Fieldsets)
                {
                    List<CUSTOMER_SURVEY_QUESTIONS> Questions = CUSTOMER_SURVEY_FORMS.GetFormQuestions(Fieldset.FIELDSET_ID, _context).Where(x => x.IS_ACTIVE == "Y").ToList();

                    foreach (CUSTOMER_SURVEY_QUESTIONS Question in Questions)
                    {
                        switch ((long)Question.TYPE_ID)
                        {
                            case 1:
                                TextField TextField = new TextField();
                                TextField.ID = "question" + Question.QUESTION_ID.ToString();
                                TextField.AllowBlank = Question.IS_REQUIRED == "Y" ? true : false;
                                TextField.FieldLabel = Question.TEXT;
                                uxSurveyDisplay.Items.Add(TextField);
                                break;
                            case 2:
                                TextArea TextArea = new TextArea();
                                TextArea.ID = "question" + Question.QUESTION_ID.ToString();
                                TextArea.AllowBlank = Question.IS_REQUIRED == "Y" ? true : false;
                                TextArea.FieldLabel = Question.TEXT;
                                uxSurveyDisplay.Items.Add(TextArea);
                                break;
                            case 5:
                                ComboBox Combobox = new ComboBox();
                                break;
                            case 6:
                                break;
                            case 7:
                                break;
                        }
                    }
                }
            }
        }
    }
}