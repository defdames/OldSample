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
    public partial class umRailroadToolbar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
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


                       
                        uxRailRoadCITextField.SetValue(RRdata.r.RAILROAD);
                        //X.Js.Call("parent.App.uxCrossingInfoTab.reload()");
                    }

                }

            }
        

        }
      
        protected void deLoadUnit(object sender, DirectEventArgs e)
        {

            Session.Add("rrType", uxRailRoadCITextField.Value);
            X.Js.Call("parent.App.uxCrossingInfoTab.reload()");
            //uxDataEntryTab.Reload();

        }
     
        //protected void LoadSelectRailroad()
        //{
        //    List<object> data;
        //    using (Entities _context = new Entities())
        //    {
        //        data = (from d in _context.CROSSING_RAILROAD

        //                select new { d.RAILROAD, d.RAILROAD_ID }).ToList<object>();
        //    }
        //    uxRailRoadCI.Store.Primary.DataSource = data;
        //}


       

    }
}