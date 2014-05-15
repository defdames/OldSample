using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umYearBudget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadSummaryGridData(object sender, StoreReadDataEventArgs e)
        {
            //using (Entities _context = new Entities())
            //{
            //    List<object> dataSource;
            //    dataSource = (from d in _context.CROSSINGS
            //                  join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID into pn
            //                  from proj in pn.DefaultIfEmpty()
            //                  select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID, proj.LONG_NAME }).ToList<object>();
            //    int count;
            //    uxSummaryGridStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
            //    e.Total = count;
            //}

            List<YearSummaryStruct> list = new List<YearSummaryStruct> 
            {
                    new YearSummaryStruct(1, "Test Project 1", "Expected", 2, 5, 10000, 2000, 8000, 1000, 7000, 12.50m, 1500),
                    new YearSummaryStruct(2, "Test Project 2", "Renewal", 2, 5, 15000, 2000, 8000, 1000, 12000, -12.5m, 1500),
                    new YearSummaryStruct(3, "Test Project 3", "Expected", 2, 5, 20000, 2000, 8000, 1000, 17000, 12.5m, 1500),
                    new YearSummaryStruct(4, "Test Project 4", "Renewal", 2, 5, 25000, 2000, 8000, 1000, 23000, 12.5m, 1500),
                    new YearSummaryStruct(5, "Test Project 5", "Renewal", 2, 5, 30000, 2000, 8000, 1000, 28000, 12.5m, 1500),
                    new YearSummaryStruct(6, "Test Project 6", "New Sale", 2, 5, 35000, 2000, 8000, 1000, 33000, 12.5m, 1500),
                    new YearSummaryStruct(7, "Test Project 7", "Expected", 2, 5, 40000, 2000, 8000, 1000, 38000, 12.5m, 1500),
                    new YearSummaryStruct(8, "Test Project 8", "Expected", 2, 5, 45000, 2000, 8000, 1000, 43000, 12.5m, 1500),
                    new YearSummaryStruct(9, "Test Project 9", "New Sale", 2, 5, 50000, 2000, 8000, 1000, 48000, 12.5m, 1500),
                    new YearSummaryStruct(10, "Test Project 10", "New Sale", 2, 5, 55000, 2000, 8000, 1000, 53000, 12.5m, 1500)
            };
            uxSummaryGridStore.DataSource = list;
        }

        class YearSummaryStruct  // DELETE WHEN GETTING DATA FROM CORRECT SOURCE
        {
            public long PROJ_ID { get; set; }
            public string PROJECT_NAME { get; set; }
            public string STATUS { get; set; }
            public decimal ACRES { get; set; }
            public decimal DAYS { get; set; }
            public decimal GROSS_REC { get; set; }
            public decimal MAT_USAGE { get; set; }
            public decimal GROSS_REV { get; set; }
            public decimal DIR_EXP { get; set; }
            public decimal OP { get; set; }
            public decimal OP_PERC { get; set; }
            public decimal OP_VAR { get; set; }

            public YearSummaryStruct(long id, string project, string proStatus, decimal proAcres, decimal proDays,
                decimal grRec, decimal mat, decimal grRev, decimal dirs, decimal proOP, decimal proOPPerc, decimal proOPVar)
            {
                PROJ_ID = id;
                PROJECT_NAME = project;
                STATUS = proStatus;
                ACRES = proAcres;
                DAYS = proDays;
                GROSS_REC = grRec;
                MAT_USAGE = mat;
                GROSS_REV = grRev;
                DIR_EXP = dirs;
                OP = proOP;
                OP_PERC = proOPPerc;
                OP_VAR = proOPVar;
            }
        }

        protected void deReadDetailGridData(object sender, StoreReadDataEventArgs e)
        {
            List<DetailStruct> list = new List<DetailStruct> 
            {
                    new DetailStruct(1, "Test Sheet 1", 10000, 2000, 8000, 1000, 7000),
                    new DetailStruct(2, "Test Sheet 2", 10000, 2000, 8000, 1000, 7000),
                    new DetailStruct(3, "Test Sheet 3", 10000, 2000, 8000, 1000, 7000),
                    new DetailStruct(4, "Test Sheet 4", 10000, 2000, 8000, 1000, 7000)
            };
            uxSummaryDetailStore.DataSource = list;
        }

        class DetailStruct  // DELETE WHEN GETTING DATA FROM CORRECT SOURCE
        {
            public long DETAIL_SHEET_ID { get; set; }
            public string SHEET_NAME { get; set; }
            public decimal GROSS_REC { get; set; }
            public decimal MAT_USAGE { get; set; }
            public decimal GROSS_REV { get; set; }
            public decimal DIR_EXP { get; set; }
            public decimal OP { get; set; }

            public DetailStruct(long id, string sheet, decimal grRec, decimal mat, decimal grRev, decimal dirs, decimal proOP)
            {
                DETAIL_SHEET_ID = id;
                SHEET_NAME = sheet;
                GROSS_REC = grRec;
                MAT_USAGE = mat;
                GROSS_REV = grRev;
                DIR_EXP = dirs;
                OP = proOP;
            }
        }

        public void GetFormData(object sender, DirectEventArgs e)
        {
            uxProjectNum.SetValue("2001517");//e.ExtraParams["CrossingId"]);
        }

        public void Test(object sender, DirectEventArgs e)
        {
            long orgID = long.Parse(Request.QueryString["OrgID"]);
            long version = long.Parse(Request.QueryString["version"]);
            X.MessageBox.Alert("Title", orgID + " " + version + " " + e.ExtraParams["SheetName"]).Show();
        }

        public void CheckFormat(object sender, DirectEventArgs e)
        {
            decimal moneyvalue;
            try
            {
                moneyvalue = Convert.ToDecimal(TextField4.Text);
            }
            catch
            {
                moneyvalue = 0;
            }
                        
            string converted = String.Format("{0:C}", moneyvalue);
            TextField4.Text = converted;
        }

    }
}