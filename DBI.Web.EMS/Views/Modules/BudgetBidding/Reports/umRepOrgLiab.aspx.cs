using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class umRepOrgLiab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string orgName = Request.QueryString["orgName"];
                string yearID = Request.QueryString["yearID"];
                string verName = Request.QueryString["verName"];


                ReportParameter paramOrgName = new ReportParameter("paramOrgName", orgName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramOrgName });

                ReportParameter paramYear = new ReportParameter("paramYear", yearID);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramYear });

                ReportParameter paramVer = new ReportParameter("paramVer", verName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramVer });
            }
        } 
    }
}