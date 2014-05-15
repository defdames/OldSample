using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using DBI.Data.GMS;
using DBI.Data.DataFactory;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umCrossingHome : BasePage
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


                         uxRailRoadCI.SetValue(RRdata.r.RAILROAD);

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


            RowSelectionModel rrSelection = RowSelectionModel2;


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


                uxRailRoadCI.SetValueAndFireSelect(RRdata.r.RAILROAD);

            }


            uxChangeRailroadWindow.Close();
            uxCrossingHomeStore.Reload();
          
        }
        protected void deLoadUnit(object sender, DirectEventArgs e)
        {

            Session.Add("rrType", uxRailRoadCI.SelectedItem.Value);

            uxCrossingHomeStore.Reload();
           

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
       
        protected void deCrossingHomeGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers
                long RailroadId = long.Parse(Session["rrType"].ToString());
                data = (from d in _context.CROSSINGS
                        join r in _context.CROSSING_RAILROAD on d.RAILROAD_ID equals r.RAILROAD_ID
                        where d.RAILROAD_ID == RailroadId
                        select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, r.RAILROAD, d.SERVICE_UNIT, d.SUB_DIVISION, d.STATE, d.CROSSING_CONTACTS.CONTACT_NAME }).ToList<object>();


                int count;
                uxCrossingHomeStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void deGetProjectList(object sender, DirectEventArgs e)
        {
            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
            List<object> data;
            using (Entities _context = new Entities())
            {
                data = (from r in _context.CROSSING_RELATIONSHIP
                        join p in _context.PROJECTS_V on r.PROJECT_ID equals p.PROJECT_ID
                        where r.CROSSING_ID == CrossingId
                        select new { r.PROJECT_ID, p.LONG_NAME, p.SEGMENT1, p.ORGANIZATION_NAME }).ToList<object>();
                uxProjectListStore.DataSource = data;
                uxProjectListStore.DataBind();


            }
        }
    }
}