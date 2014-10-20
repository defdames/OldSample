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
             
               uxAddStateList.Data = StaticLists.CrossingStateList;
               uxAddAppRequestedStore.Data = StaticLists.ApplicationRequested;
               if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
               {
                   deGetRRType("Add");
                  
               }
               
            //   deLoadUnit("Add");
            }
        }

        protected void deStateCrossingListGrid(object sender, DirectEventArgs e)
        {
            decimal Application = Convert.ToDecimal(uxAddAppReqeusted.SelectedItem.Value);
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            string State = uxAddStateComboBox.SelectedItem.Value;
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));


                //IQueryable<CROSSING_MAINTENANCE.StateCrossingList> allData = CROSSING_MAINTENANCE.GetStateCrossingList(RailroadId, _context);

                //if (ServiceUnit != null)
                //{
                //    allData = allData.Where(x => x.SERVICE_UNIT == ServiceUnit);
                //}
                //if (SubDiv != null)
                //{
                //    allData = allData.Where(x => x.SUB_DIVISION == SubDiv);
                //}
                //if (State != null)
                //{
                //    allData = allData.Where(x => x.STATE == State);
                //}
                //List<object> _data = allData.ToList<object>();



                //int count;
                //uxStateCrossingListStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                //e.Total = count;

                string selectedRailroad = RailroadId.ToString();
                string selectedServiceUnit = ServiceUnit.ToString();
                string selectedSubDiv = SubDiv.ToString();
                string selectedState = State.ToString();

                string url = "/Views/Modules/CrossingMaintenance/Reports/StateCrossingListReport.aspx?selectedRailroad=" + selectedRailroad + "&selectedServiceUnit=" + selectedServiceUnit + "&selectedSubDiv=" + selectedSubDiv + "&selectedState=" + selectedState;
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
        //protected void deLaunchGrid(object sender, DirectEventArgs e)
        //{
        //    GridPanel1.Show();
        //}
       protected void ToXml(object sender, EventArgs e)
        {
            string json = this.Hidden1.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;

            string strXml = xml.OuterXml;

            this.Response.Clear();
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xml");
            this.Response.AddHeader("Content-Length", strXml.Length.ToString());
            this.Response.ContentType = "application/xml";
            this.Response.Write(strXml);
            this.Response.End();
        }

        protected void ToExcel(object sender, EventArgs e)
        {
            string json = this.Hidden1.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");

            XslCompiledTransform xtExcel = new XslCompiledTransform();

            xtExcel.Load(Server.MapPath("Excel.xsl"));
            xtExcel.Transform(xml, null, this.Response.OutputStream);
            this.Response.End();
        }

        protected void ToCsv(object sender, EventArgs e)
        {
            string json = this.Hidden1.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/octet-stream";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.csv");

            XslCompiledTransform xtCsv = new XslCompiledTransform();

            xtCsv.Load(Server.MapPath("Csv.xsl"));
            xtCsv.Transform(xml, null, this.Response.OutputStream);
            this.Response.End();
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
        

    
