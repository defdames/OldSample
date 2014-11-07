using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.ReportTemplates
{
    public partial class Landscape : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                long yearID = Convert.ToInt64(Request.QueryString["yearID"]);                
              
                ReportParameter paramYear = new ReportParameter("paramYear", yearID.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramYear });
            }
        } 
    }
}