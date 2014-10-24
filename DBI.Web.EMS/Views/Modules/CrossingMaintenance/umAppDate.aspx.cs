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
    public partial class umAppDate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxAddAppRequestedStore.Data = StaticLists.ApplicationRequested;
                uxAddStateList.Data = StaticLists.CrossingStateList;

                if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    deGetRRType("Add");

                }
            }
        }
        protected void deAppDateGrid(object sender, DirectEventArgs e)
        {
            
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
            decimal Application = Convert.ToDecimal(uxAddAppReqeusted.SelectedItem.Value);
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            string State = uxAddStateComboBox.SelectedItem.Value;
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
               
                DateTime selectedStart = StartDate;
                DateTime selectedEnd = EndDate;
                decimal selectedApplication = Application;
                string selectedRailroad = RailroadId.ToString();
                string selectedServiceUnit = ServiceUnit;
                string selectedSubDiv = SubDiv;
                string selectedState = State;

                string url = "/Views/Modules/CrossingMaintenance/Reports/AppDateReport.aspx?selectedRailroad=" + selectedRailroad + "&selectedServiceUnit=" + selectedServiceUnit + "&selectedSubDiv=" + selectedSubDiv + "&selectedState=" + selectedState + "&selectedStart=" + selectedStart + "&selectedEnd=" + selectedEnd + "&selectedApplication=" + selectedApplication;
                Ext.Net.Panel pan = new Ext.Net.Panel();

                pan.ID = "Tab";
                pan.Title = "Application Date Report";
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
                if (rrLoad == "Add")
                {
                    List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(rrType).ToList();
                    uxAddServiceUnit.Clear();
                    uxAddSubDiv.Clear();
                    uxAddServiceUnitStore.DataSource = units;
                    uxAddServiceUnitStore.DataBind();
                }

            }
        }
        protected void deLoadSubDiv(object sender, DirectEventArgs e)
        {


            if (e.ExtraParams["Type"] == "Add")
            {
                List<ServiceUnitResponse> divisions = ServiceUnitData.ServiceUnitDivisions(uxAddServiceUnit.SelectedItem.Value).ToList();
                uxAddSubDiv.Clear();
                uxAddSubDivStore.DataSource = divisions;
                uxAddSubDivStore.DataBind();
            }
        }
      
    }
}