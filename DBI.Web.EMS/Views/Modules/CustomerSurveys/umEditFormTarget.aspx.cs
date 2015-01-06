using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umEditFormTarget : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deSaveTarget(object sender, DirectEventArgs e)
        {
            SURVEY_TYPES SurveyType = new SURVEY_TYPES();
            SurveyType.TYPE_NAME = uxTargetName.Text;
            GenericData.Insert(SurveyType);
            X.Js.Call("parent.App.uxFormTypeStore.reload(); parentAutoLoadControl.close()");
        }
    }
}