namespace DBI.Web.EMS.Views.Modules.BudgetBidding.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for Report2.
    /// </summary>
    public partial class Report2 : Telerik.Reporting.Report
    {
        public Report2(string orgName, Int64 orgID,  Int64 yearID,  Int64 verID,  Int64 prevYearID,  Int64 prevVerID)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.ReportParameters["orgName"].Value = orgName;
            this.ReportParameters["orgID"].Value = orgID;
            this.ReportParameters["yearID"].Value = yearID;
            this.ReportParameters["verID"].Value = verID;
            this.ReportParameters["prevYearID"].Value = prevYearID;
            this.ReportParameters["prevVerID"].Value = prevVerID;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}