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
                if (Session["rrType"] != null)
                {
                    using (Entities _context = new Entities())
                    {
                        long RailroadId = long.Parse(Session["rrType"].ToString());
                        var RRdata = (from r in _context.CROSSING_RAILROAD
                                      where r.RAILROAD_ID == RailroadId
                                      select new
                                      {
                                          r

                                      }).Single();


                        uxRailRoadCI.SetValueAndFireSelect(RRdata.r.RAILROAD);

                    }

                }
                else if (Session["rrType"] == null)
                {
                    uxChangeRailroadWindow.Show();
                }

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


            Session.Add("rrType", rrSelection.SelectedRecordID.ToString());

            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(Session["rrType"].ToString());
                var RRdata = (from r in _context.CROSSING_RAILROAD
                              where r.RAILROAD_ID == RailroadId
                              select new
                              {
                                  r

                              }).Single();


                uxRailRoadCI.SetValue(RRdata.r.RAILROAD);

            }


            uxChangeRailroadWindow.Close();
            uxStateCrossingsList.Reload();
            uxAppDate.Reload();
            uxIncidents.Reload();
        }


        protected void deLoadUnit(object sender, DirectEventArgs e)
        {

            Session.Add("rrType", uxRailRoadCI.SelectedItem.Value);

            uxStateCrossingsList.Reload();
            uxAppDate.Reload();
            uxIncidents.Reload();

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