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

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umPrivateCrossingReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {

                uxAddStateList.Data = StaticLists.StateList;
           
                if (SYS_USER_PROFILE_OPTIONS.userProfileOption("UserCrossingSelectedValue") != string.Empty)
                {
                    deGetRRType("Add");

                }
            }
        }
        protected void dePrivateCrossingListGrid(object sender, StoreReadDataEventArgs e)
        {
            
            string ServiceUnit = uxAddServiceUnit.SelectedItem.Value;
            string SubDiv = uxAddSubDiv.SelectedItem.Value;
            string State = uxAddStateComboBox.SelectedItem.Value;
            using (Entities _context = new Entities())
            {
                

                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.userProfileOption("UserCrossingSelectedValue"));
                var allData = (from d in _context.CROSSINGS
                        where d.RAILROAD_ID == RailroadId && d.PROPERTY_TYPE == "Private"
                        select new
                        {
                            d.CROSSING_ID,
                            d.CROSSING_NUMBER,
                            d.SUB_DIVISION,
                            d.STATE,
                            d.SERVICE_UNIT,
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
                            d.SPECIAL_INSTRUCTIONS,
                        
                        });

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

                List<object> _data = allData.ToList<object>();

                int count;
                uxPrivateCrossingListStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
              
                //uxStateCrossingListStore.Reload();

            }

        }
        protected void deClearFilters(object sender, DirectEventArgs e)
        {
            FilterForm.Reset();
        }
        protected void deGetRRType(string rrLoad)
        {

            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.userProfileOption("UserCrossingSelectedValue"));
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
        //protected void GetAdditionalData(object sender, DirectEventArgs e)
        //{
        //    List<object> data;


        //    using (Entities _context = new Entities())
        //    {
        //        data = (from d in _context.CROSSINGS

        //                select new { d.SPECIAL_INSTRUCTIONS }).ToList<object>();
        //    }

        //}
    }
}