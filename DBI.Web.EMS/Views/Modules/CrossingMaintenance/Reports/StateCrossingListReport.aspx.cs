using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports
{
    public partial class StateCrossingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    string selectedServiceUnit = Request.QueryString["selectedServiceUnit"];
            //    string selectedSubDiv = Request.QueryString["selectedSubDiv"];
            //    string selectedState = Request.QueryString["selectedState"];
            //    long selectedRailroad = Convert.ToInt64(Request.QueryString["selectedRailroad"]);

            //    ReportParameter sselectedServiceUnit = new ReportParameter("sselectedServiceUnit", selectedServiceUnit);
            //    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { sselectedServiceUnit });

            //    ReportParameter sselectedSubDiv = new ReportParameter("sselectedSubDiv", selectedSubDiv);
            //    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { sselectedSubDiv });

            //    ReportParameter sselectedState = new ReportParameter("sselectedState", selectedState);
            //    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { sselectedState });

            //    ReportParameter sselectedRailroad = new ReportParameter("sselectedRailroad", selectedRailroad);
            //    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { sselectedRailroad });


            //}
        }
    }
}
