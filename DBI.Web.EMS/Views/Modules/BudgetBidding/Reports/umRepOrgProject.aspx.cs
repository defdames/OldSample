using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class umRepProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string orgName = Request.QueryString["orgName"];
                string yearID = Request.QueryString["yearID"];
                long verID = Convert.ToInt64(Request.QueryString["verID"]);
                string verName = Request.QueryString["verName"];
                string projectNumber = Request.QueryString["projectNum"];
                string projectName = Request.QueryString["projectName"];
                string prevVerName = BB.GetPrevVerName(verID);


                ReportParameter paramOrgName = new ReportParameter("paramOrgName", orgName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramOrgName });

                ReportParameter paramYear = new ReportParameter("paramYear", yearID);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramYear });

                ReportParameter paramVer = new ReportParameter("paramVer", verName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramVer });

                ReportParameter paramProjNum = new ReportParameter("paramProjNum", projectNumber);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramProjNum });

                ReportParameter paramProjName = new ReportParameter("paramProjName", projectName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramProjName });

                ReportParameter paramPrevVerName = new ReportParameter("paramPrevVerName", prevVerName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramPrevVerName });                    
            }
        }   
    }
}