using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using DBI.Data.GMS;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DBI.Data.DataFactory;
using System.Xml.Xsl;
using System.Xml;


namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umSummaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
                if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    deGetRRType("Add");

                }
            }
        
        protected void deSupplementalReportGrid(object sender, DirectEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;

         
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));

                DateTime selectedStart = StartDate;
                DateTime selectedEnd = EndDate;
                string selectedRailroad = RailroadId.ToString();
       
                string url = "/Views/Modules/CrossingMaintenance/Reports/SupplementalBillingReport.aspx?selectedRailroad=" + selectedRailroad + "&selectedStart=" + selectedStart + "&selectedEnd=" + selectedEnd;
                Ext.Net.Panel pan = new Ext.Net.Panel();

                pan.ID = "Panel";
                pan.Title = "Crossing Summary Report";
                pan.CloseAction = CloseAction.Destroy;
                pan.Loader = new ComponentLoader();
                pan.Loader.ID = "loader";
                pan.Loader.Url = url;
                pan.Loader.Mode = LoadMode.Frame;
                pan.Loader.LoadMask.ShowMask = true;
                pan.Loader.DisableCaching = true;
                pan.AddTo(uxCenterPanel);
                
            }
        }
        protected void deClearFilters(object sender, DirectEventArgs e)
        {
            uxFilterForm.Reset();
        }
        protected void deGetRRType(string rrLoad)
        {

            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                var RRdata = (from r in _context.CROSSING_RAILROAD
                              where r.RAILROAD_ID == RailroadId
                              select new
                              {
                                  r

                              }).SingleOrDefault();

                uxRRCI.SetValue(RRdata.r.RAILROAD);

                string rrType = RRdata.r.RAILROAD;
               
            }
        }
        protected void GetAdditionalData(object sender, DirectEventArgs e)
        {
            List<object> data;


            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS

                        select new { d.SPECIAL_INSTRUCTIONS }).ToList<object>();
            }

        }
     
        }
    }

    
