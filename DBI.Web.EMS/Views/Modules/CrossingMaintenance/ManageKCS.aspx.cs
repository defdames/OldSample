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
    public partial class ManageKCS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deReadServiceUnit(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<CROSSING_SERVICE_SUB_DIV> SUData = _context.CROSSING_SERVICE_SUB_DIV.ToList();


                uxServiceUnitStore.DataSource = SUData;

            }

        }
        protected void deSaveServiceUnit(object sender, DirectEventArgs e)
        {
            //long RailroadId = long.Parse(Session["rrType"].ToString());
            //CROSSING_SERVICE_SUB_DIV rrId = new CROSSING_SERVICE_SUB_DIV();
            //{
            //    RailroadId = rrId.RR_ID;
            //}
            //if (Session["rrType"] != null)
            //{
            //    Session["rrType"] = rrId.RR_ID;
            //}
            ChangeRecords<AddServiceUnit> data = new StoreDataHandler(e.ExtraParams["sudata"]).BatchObjectData<AddServiceUnit>();
            foreach (AddServiceUnit CreatedServiceUnit in data.Created)
            {
                CROSSING_SERVICE_SUB_DIV NewServiceUnit = new CROSSING_SERVICE_SUB_DIV();
                NewServiceUnit.SERVICE_UNIT_NAME = CreatedServiceUnit.SERVICE_UNIT_NAME;

                GenericData.Insert<CROSSING_SERVICE_SUB_DIV>(NewServiceUnit);
                uxServiceUnitStore.Reload();
            }
        }
        protected void deSaveSubDiv(object sender, DirectEventArgs e)
        {
            ChangeRecords<AddSubDiv> data = new StoreDataHandler(e.ExtraParams["subdivdata"]).BatchObjectData<AddSubDiv>();
            foreach (AddSubDiv CreatedSubDiv in data.Created)
            {
                CROSSING_SERVICE_SUB_DIV NewSubDiv = new CROSSING_SERVICE_SUB_DIV();
                NewSubDiv.SUB_DIVISION_NAME = CreatedSubDiv.SUB_DIVISION_NAME;

                GenericData.Insert<CROSSING_SERVICE_SUB_DIV>(NewSubDiv);
                uxServiceUnitStore.Reload();
            }
        }
        //protected void deLoadSubDiv(object sender, DirectEventArgs e)
        //{
        //    using (Entities _context = new Entities())
        //    {
        //        List<object> data;
        //        long ServiceUnitid = long.Parse(e.ExtraParams["ServiceUnitId"].ToString());

        //        data = (from d in _context.SUB_DIVISION
        //                where d.SERVICE_UNIT_ID == ServiceUnitId
        //                select new { d.SUB_DIVISION_ID, SUB_DIVISION_NAME }).ToList<object>();
        //    }
        //}
        public class AddServiceUnit
        {
            public decimal RAILROAD_ID { get; set; }
            public int SERVICE_UNIT_ID { get; set; }
            public string SERVICE_UNIT_NAME { get; set; }
        }
        public class AddSubDiv
        {
            public int SUB_DIVISION_ID { get; set; }
            public string SUB_DIVISION_NAME { get; set; }
        }
    }
}