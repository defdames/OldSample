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
            try
            {
                if (!Page.IsPostBack)
                {
                    string orgName = Request.QueryString["orgName"];
                    long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
                    long yearID = Convert.ToInt64(Request.QueryString["yearID"]);
                    long verID = Convert.ToInt64(Request.QueryString["verID"]);
                    long prevYearID = Convert.ToInt64(Request.QueryString["prevYearID"]);
                    long prevVerID = Convert.ToInt64(Request.QueryString["prevVerID"]);
                    string oh = Request.QueryString["oh"];


                    string iyearID = yearID.ToString();

                    //ReportParameter[] para = new ReportParameter[1];
                    //para[0] = new ReportParameter("orgName", orgName);
                    //para[1] = new ReportParameter("yearID", iyearID);


                    //this.ReportViewer1.LocalReport.SetParameters(para);

                    ReportParameter pyID = new ReportParameter("pyearID", iyearID);
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { pyID });
                    //ReportParameter pohead = new ReportParameter("poh", oh);
                    //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { pohead });
                    ReportParameter poName = new ReportParameter("porgName", orgName);
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { poName });
                    if (verID == 1)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "Bid");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                    else if (verID == 2)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "First Draft");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                    else if (verID == 3)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "Final Draft");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                    else if (verID == 4)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "1st Reforecast");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                    else if (verID == 5)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "2nd Reforecast");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                    else if (verID == 6)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "3rd Reforecast");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                    else if (verID == 7)
                    {
                        ReportParameter bver = new ReportParameter("pbudVer", "4th Reforecast");
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { bver });
                    }
                }
            }
            catch (Exception err)
            {

            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

        }     
    }
}