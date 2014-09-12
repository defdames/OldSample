using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
            
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;

            LocalReport localReport = ReportViewer1.LocalReport;
        }
    }

    //public static DataTable GetReportData( )
}