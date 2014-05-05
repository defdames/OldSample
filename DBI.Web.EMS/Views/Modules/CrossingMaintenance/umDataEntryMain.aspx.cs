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
    public partial class umDataEntryMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSelectRailroad();

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


        //protected void deSaveRRSelection(object sender, DirectEventArgs e)
        //{
        //    long RailroadId = long.Parse(Request.QueryString["RailroadId"]);
        //    List<object> data;
        //    using (Entities _context = new Entities())
        //    {
        //        data = (from d in _context.CROSSING_RAILROAD
        //                where d.RAILROAD_ID == RailroadId
        //                select new { d.RAILROAD, d.RAILROAD_ID }).ToList<object>();
        //    }
        //}
    }
}