using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class umReport2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

             //string orgName = Request.QueryString["orgName"];
            //long orgID = Convert.ToInt64(Request.QueryString["orgID"]);

            
            Report rareport = new Report2(Request.QueryString["orgName"], Convert.ToInt64(Request.QueryString["orgID"]), Convert.ToInt64(Request.QueryString["yeargID"]),Convert.ToInt64(Request.QueryString["verID"]), Convert.ToInt64(Request.QueryString["prevYearID"]), Convert.ToInt64(Request.QueryString["prevVerID"])) ;
            //report.ReportParameters["orgID"].Value = Convert.ToInt64(Request.QueryString["orgID"]);
            //report.ReportParameters["yearID"].Value = Convert.ToInt64(Request.QueryString["yeargID"]);
            //report.ReportParameters["verID"].Value = Convert.ToInt64(Request.QueryString["verID"]);
            //report.ReportParameters["prevYearID"].Value = Convert.ToInt64(Request.QueryString["prevYearID"]);
            //report.ReportParameters["prevVerID"].Value = Convert.ToInt64(Request.QueryString["prevVerID"]);

            ReportViewer1.Report = rareport;

            


            
        }
    }
}