﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
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

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umIncidentReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
             
                uxAddStateList.Data = StaticLists.StateList;

                if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    deGetRRType("Add");

                }
            }
        }
        protected void deIncidentGrid(object sender, StoreReadDataEventArgs e)
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
                var allData = (from d in _context.CROSSINGS
                               join i in _context.CROSSING_INCIDENT on d.CROSSING_ID equals i.CROSSING_ID
                               where d.RAILROAD_ID == RailroadId 
                               select new
                               {
                                   d.CROSSING_ID,
                                   i.INCIDENT_ID,
                                   d.CROSSING_NUMBER,
                                   d.SUB_DIVISION,
                                   d.SERVICE_UNIT,
                                   d.STATE,
                                   d.MILE_POST,
                                   d.DOT,
                                   i.REMARKS,
                                   i.SLOW_ORDER,
                                   i.INCIDENT_NUMBER,
                                   i.DATE_REPORTED,
                                   i.DATE_CLOSED,
                               });

                //filter down specific information to show the incidents needed for report
                if (StartDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.DATE_REPORTED >= StartDate);
                }

                if (EndDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.DATE_REPORTED <= EndDate);
                }

                if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
                {
                    allData = allData.Where(x => x.DATE_REPORTED >= StartDate && x.DATE_REPORTED <= EndDate);
                }

                if (ServiceUnit != null)
                {
                    allData = allData.Where(x => x.SERVICE_UNIT == ServiceUnit);
                }

                if (SubDiv != null)
                {
                    allData = allData.Where(x => x.SUB_DIVISION == SubDiv);
                }

                if (State != null)
                {
                    allData = allData.Where(x => x.STATE == State);
                }
                if(uxOpenIncident.Checked)
                {
                    allData = allData.Where(x => x.DATE_CLOSED == null);
                }
                if(uxClosedIncident.Checked)
                {
                    allData = allData.Where(x => x.DATE_CLOSED != null);
                }
                List<object> _data = allData.ToList<object>();


                int count;
                uxIncidentStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
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