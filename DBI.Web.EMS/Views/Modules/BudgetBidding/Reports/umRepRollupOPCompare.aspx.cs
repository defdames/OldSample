﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    public partial class umRepRollupOPCompare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
                string orgName = Request.QueryString["orgName"];
                long yearID = Convert.ToInt64(Request.QueryString["yearID"]);                
                long verID = Convert.ToInt64(Request.QueryString["verID"]);
                string verName = Request.QueryString["verName"];
                string oh = BBOH.DataSingle(orgID, yearID, verID).OH.ToString();
                long prevYearID = Convert.ToInt64(Request.QueryString["prevYearID"]);
                long prevVerID = Convert.ToInt64(Request.QueryString["prevVerID"]);
                BBOH.Subtotal.Fields prevOHData = BBOH.Subtotal.Data(orgID, prevYearID, prevVerID);
                string preVerName = Request.QueryString["prevVerName"];


                ReportParameter paramOrgName = new ReportParameter("paramOrgName", orgName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramOrgName });

                ReportParameter paramYear = new ReportParameter("paramYear", yearID.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramYear });

                ReportParameter paramVer = new ReportParameter("paramVer", verName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramVer });

                ReportParameter paramOH = new ReportParameter("paramOH", oh);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramOH });

                ReportParameter paramPrevVerName = new ReportParameter("paramPrevVer", preVerName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramPrevVerName });

                ReportParameter paramPrevYear = new ReportParameter("paramPrevYear", prevYearID.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramPrevYear });

                ReportParameter paramPrevOH = new ReportParameter("paramPrevOH", prevOHData.OH.ToString());
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { paramPrevOH });
            }
        } 
    }
}