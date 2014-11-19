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
    public partial class umStateCrossingsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
               uxSprayedNotSprayed.Data = StaticLists.CrossingsType;
               uxAddStateList.Data = StaticLists.CrossingStateList;
               uxAddAppRequestedStore.Data = StaticLists.ApplicationRequested;
               if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
               {
                   deGetRRType("Add");
                  
               }
               
            }
        }

        protected void deStateCrossingListGrid(object sender, DirectEventArgs e)
        {
            decimal Application = Convert.ToDecimal(uxAddAppReqeusted.SelectedItem.Value);
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            string State = uxAddStateComboBox.SelectedItem.Value;
            string SprayNotSpray = uxCrossingSprayed.SelectedItem.Value;
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));

                string selectedRailroad = RailroadId.ToString();
                string selectedServiceUnit = ServiceUnit;
                string selectedSubDiv = SubDiv;
                string selectedState = State;
                string selectedSpray = SprayNotSpray;
                string url = "/Views/Modules/CrossingMaintenance/Reports/StateCrossingListReport.aspx?selectedRailroad=" + selectedRailroad + "&selectedServiceUnit=" + selectedServiceUnit + "&selectedSubDiv=" + selectedSubDiv + "&selectedState=" + selectedState + "&selectedSpray=" + selectedSpray;
                Ext.Net.Panel pan = new Ext.Net.Panel();

                pan.ID = "Tab";
                pan.Title = "State Crossing List Report";
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
            FilterForm.Reset();      
        }
        protected List<object> GetCrossingData(long CrossingId)
        {
            using (Entities _context = new Entities())
            {
                var returnData = (from d in _context.CROSSINGS
                                  select new
                                  {
                                      d.CROSSING_ID,
                                      d.CROSSING_NUMBER,
                                      d.SUB_DIVISION,
                                      d.STATE,
                                      d.COUNTY,
                                      d.CITY,
                                      d.MILE_POST,
                                      d.DOT,
                                      d.ROWNE,
                                      d.ROWNW,
                                      d.ROWSE,
                                      d.ROWSW,
                                      d.STREET,
                                      d.SUB_CONTRACTED,
                                      d.LONGITUDE,
                                      d.LATITUDE,
                                      d.SPECIAL_INSTRUCTIONS
                                  }).ToList<object>();
                return returnData;
            }
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
        protected void GetAdditionalData(object sender, DirectEventArgs e)
        {
            List<object> data;


            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS

                        select new { d.SPECIAL_INSTRUCTIONS }).ToList<object>();
            }

        }

          
        public class CrossingForApplicationDetails
        {
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string DOT { get; set; }
            public string MILE_POST { get; set; }
            public string STATE { get; set; }
            public string CONTACT_ID { get; set; }
        }
                      
    }
}
        

    
