using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class umReport2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

             //string orgName = Request.QueryString["orgName"];
            //long orgID = Convert.ToInt64(Request.QueryString["orgID"]);

            Report2 report = new Report2();
            report.ReportParameters["orgName"].Value = Request.QueryString["orgName"];
            report.ReportParameters["orgID"].Value = Convert.ToInt64(Request.QueryString["orgID"]);

            ReportViewer1.Report = report;

            


            
        }
    }
}