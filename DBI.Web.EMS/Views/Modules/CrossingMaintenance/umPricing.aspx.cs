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
    public partial class umPricing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void deReadServiceUnit(object sender, StoreReadDataEventArgs e)
        //{
        //    using (Entities _context = new Entities())
        //    {
        //        List<CROSSING_SERVICE_UNIT> SUData = _context.CROSSING_SERVICE_UNIT.ToList();

        //        uxPricingStore.DataSource = SUData;

        //    }

        //}
        //protected void deSaveServiceUnit(object sender, DirectEventArgs e)
        //{
        //    //long RailroadId = long.Parse(Session["rrType"].ToString());
        //    //CROSSING_SERVICE_UNIT rrId = new CROSSING_SERVICE_UNIT();
        //    //{

        //    //}
        //    ChangeRecords<AddPricing> data = new StoreDataHandler(e.ExtraParams["sudata"]).BatchObjectData<AddPricing>();
        //    foreach (AddPricing CreatedPricing in data.Created)
        //    {
        //        CROSSING_PRICING NewServiceUnit = new CROSSING_PRICING();
        //        NewServiceUnit.SERVICE_UNIT_NAME = CreatedPricing.SERVICE_UNIT_NAME;
        //        NewServiceUnit.RAILROAD_ID = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
        //        NewServiceUnit.CREATE_DATE = DateTime.Now;
        //        NewServiceUnit.MODIFY_DATE = DateTime.Now;
        //        NewServiceUnit.CREATED_BY = User.Identity.Name;
        //        NewServiceUnit.MODIFIED_BY = User.Identity.Name;
        //        GenericData.Insert<CROSSING_SERVICE_UNIT>(NewServiceUnit);
        //        uxPricingStore.Reload();
        //    }
        //}
    }
}