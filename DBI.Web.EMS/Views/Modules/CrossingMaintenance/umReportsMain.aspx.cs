using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umReportsMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSelectRailroad();
            if (!X.IsAjaxRequest)
            {

                if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    using (Entities _context = new Entities())
                    {
                        long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                        var RRdata = (from r in _context.CROSSING_RAILROAD
                                      where r.RAILROAD_ID == RailroadId
                                      select new
                                      {
                                          r

                                      }).Single();


                        uxRailRoadCI.SetValue(RRdata.r.RAILROAD);

                    }

                }
                else if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") == string.Empty)
                {
                    uxChangeRailroadWindow.Show();
                }

            }
        }
        protected void deLoadReportList(object sender, StoreReadDataEventArgs e)
        {
            List<ReportListStruct> list = new List<ReportListStruct> 
                {
                    new ReportListStruct(1, "State Crossing List"),
                    new ReportListStruct(7, "Crossing Summary"),
                    new ReportListStruct(2, "Application Date"),
                    new ReportListStruct(3, "Inspection Date"),
                    new ReportListStruct(4, "Incidents"),
                    new ReportListStruct(5, "Supplemental Billing"),                  
                    new ReportListStruct(8, "Weekly Work Activity"),
                    new ReportListStruct(9, "Private Crossing List"),
                    new ReportListStruct(10, "ROW Missing") 
                };
            uxReportListStore.DataSource = list;
        }
        class ReportListStruct  // DELETE WHEN GETTING DATA FROM CORRECT SOURCE
        {
            public long REPORT_ID { get; set; }
            public string REPORT_NAME { get; set; }
    

            public ReportListStruct(long id, string reportName)
            {
                REPORT_ID = id;
                REPORT_NAME = reportName;
         
            }
        }
        protected void deLoadReport(object sender, DirectEventArgs e)
        {
            
            switch (e.ExtraParams["selectedReport"])
            {
            case "State Crossing List":
            string StateListUrl = string.Format("umStateCrossingsList.aspx?");
            uxReportMainPanel.LoadContent(StateListUrl);
            break;

            case "Application Date":
            string AppDateUrl = string.Format("umAppDate.aspx?");
            uxReportMainPanel.LoadContent(AppDateUrl);
            break;

            case "Incidents":
            string IncidentUrl = string.Format("umIncidentReports.aspx?");
            uxReportMainPanel.LoadContent(IncidentUrl);
            break;

            case "Inspection Date":
            string InspectUrl = string.Format("umInspectionReport.aspx?");
            uxReportMainPanel.LoadContent(InspectUrl);
            break;

            case "Private Crossing List":
            string PriUrl = string.Format("umPrivateCrossingReport.aspx?");
            uxReportMainPanel.LoadContent(PriUrl);
            break;

            case "Crossing Summary":
            string CSUrl = string.Format("umSummaryReport.aspx?");
            uxReportMainPanel.LoadContent(CSUrl);
            break;

            case "ROW Missing":
            string ROWUrl = string.Format("umMissingROW.aspx?");
            uxReportMainPanel.LoadContent(ROWUrl);
            break;

            case "Weekly Work Activity":
            string WWAUrl = string.Format("umWeeklyWorkActivity.aspx?");
            uxReportMainPanel.LoadContent(WWAUrl);
            break;

            case "Supplemental Billing":
            string SUPUrl = string.Format("umSuppBillingReport.aspx?");
            uxReportMainPanel.LoadContent(SUPUrl);
            break;
            } 
            
        }
        protected void deReadRRTypes(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> RRdata;

                RRdata = (from r in _context.CROSSING_RAILROAD
                          select new
                          {
                              r.RAILROAD_ID,
                              r.RAILROAD

                          }).ToList<object>();


                int count;
                uxRailRoadStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], RRdata, out count);
                e.Total = count;

                //X.Js.Call("parent.App.uxCrossingInfoTab.reload()");
            }
        }
        protected void deLoadRR(object sender, DirectEventArgs e)
        {


            RowSelectionModel rrSelection = RowSelectionModel1;


            SYS_USER_PROFILE_OPTIONS.SetProfileOption("UserCrossingSelectedValue", rrSelection.SelectedRecordID.ToString());

            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                var RRdata = (from r in _context.CROSSING_RAILROAD
                              where r.RAILROAD_ID == RailroadId
                              select new
                              {
                                  r

                              }).Single();


                uxRailRoadCI.SetValue(RRdata.r.RAILROAD);

            }


            uxChangeRailroadWindow.Close();
            uxReportMainPanel.Reload();
            uxReportListGrid.Reload();
           
        }


        protected void deLoadUnit(object sender, DirectEventArgs e)
        {

            SYS_USER_PROFILE_OPTIONS.SetProfileOption("UserCrossingSelectedValue", uxRailRoadCI.SelectedItem.Value);

            uxReportListStore.Reload();
            uxReportMainPanel.ClearContent();
            

        }

        protected void LoadSelectRailroad()
        {
            List<object> data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSING_RAILROAD

                        select new { d.RAILROAD, d.RAILROAD_ID }).ToList<object>();
            }
            uxRailRoadCI.Store.Primary.DataSource = data;
        }
    }
}