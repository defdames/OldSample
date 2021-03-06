﻿using System;
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
    public partial class umIncidentReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
             
                uxAddStateList.Data = StaticLists.CrossingStateList;

                if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    deGetRRType("Add");

                }
            }
        }
        protected void deIncidentGrid(object sender, DirectEventArgs e)
        {
            DateTime StartDate = uxStartDate.SelectedDate;
            DateTime EndDate = uxEndDate.SelectedDate;
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            string State = uxAddStateComboBox.SelectedItem.Value;
            using (Entities _context = new Entities())
            {

                //Get List of all incidents open and closed 
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));

                //IQueryable<CROSSING_MAINTENANCE.IncidentReportList> allData = CROSSING_MAINTENANCE.GetIncidentReport(RailroadId, _context);

                //filter down specific information to show the incidents needed for report
                //if (StartDate != DateTime.MinValue)
                //{
                //    allData = allData.Where(x => x.DATE_REPORTED >= StartDate);
                //}

                //if (EndDate != DateTime.MinValue)
                //{
                //    allData = allData.Where(x => x.DATE_REPORTED <= EndDate);
                //}

                //if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
                //{
                //    allData = allData.Where(x => x.DATE_REPORTED >= StartDate && x.DATE_REPORTED <= EndDate);
                //}

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
                string Open = "";
                string Closed = "";
                if (uxOpenIncident.Checked)
                {
                    Open = "Y";
                }
                if (uxClosedIncident.Checked)
                {
                    Closed = "Y";
                }
                //List<object> _data = allData.ToList<object>();


                //int count;
                //uxIncidentStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                //e.Total = count;

          
                string selectedOpen = Open;
                string selectedClosed = Closed;
                DateTime selectedStart = StartDate;
                DateTime selectedEnd = EndDate;
                string selectedRailroad = RailroadId.ToString();
                string selectedServiceUnit = ServiceUnit;
                string selectedSubDiv = SubDiv;
                string selectedState = State;

                string url = "/Views/Modules/CrossingMaintenance/Reports/IncidentReport.aspx?selectedRailroad=" + selectedRailroad + "&selectedServiceUnit=" + selectedServiceUnit + "&selectedSubDiv=" + selectedSubDiv + "&selectedState=" + selectedState + "&selectedStart=" + selectedStart + "&selectedEnd=" + selectedEnd + "&selectedOpen=" + selectedOpen + "&selectedClosed=" + selectedClosed;
                Ext.Net.Panel pan = new Ext.Net.Panel();

                pan.ID = "Tab";
                pan.Title = " RMCC Incident Report";
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
    }
}