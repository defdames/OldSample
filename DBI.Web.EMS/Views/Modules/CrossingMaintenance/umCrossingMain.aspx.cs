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
    public partial class umCrossingMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //protected void deMainGridData(object sender, StoreReadDataEventArgs e)
        //{

        //    using (Entities _context = new Entities())
        //    {
        //        List<object> data;

        //        //Get List of all new headers
                
        //        data = (from d in _context.CROSSINGS
        //                select new { d.CROSSING_ID, d.CROSSING_NUMBER }).ToList<object>();                  

        //        int count;
        //        uxCurrentCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
        //    }
        //}
         /// <summary>
        /// Update Tab URLs based on selected header and activate buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void deUpdateUrlAndButtons(object sender, DirectEventArgs e)
        //{
        //    string CrossingSecurityUrl = string.Format("umCrossingSecurityTab.aspx?CrossingId={0}", e.ExtraParams["CrossingId"]);
        //    string CrossingInfoUrl = string.Format("umCrossingInfoTab.aspx?crossingId={0}", e.ExtraParams["CrossingId"]);
        //    string contactsUrl = string.Format("umContactsTab.aspx?CrossingId={0}", e.ExtraParams["CrossingId"]);
        //    string SubDivUrl = string.Format("umSubDivisionsTab.aspx?CrossingId={0}", e.ExtraParams["CrossingId"]);
        //    string ServiceUnitsUrl = string.Format("umServiceUnitsTab.aspx?CrossingId={0}", e.ExtraParams["CrossingId"]);
        //    string DataEntryUrl = string.Format("umDataEntryTab.aspx?CrossingId={0}", e.ExtraParams["CrossingId"]);

        //    uxCrossingSecurity.Disabled = false;
        //    uxCrossingInfoTab.Disabled = false;
        //    uxContactsTab.Disabled = false;
        //    uxSubDivisionsTab.Disabled = false;
        //    uxServiceUnitsTab.Disabled = false;
        //    uxDataEntryTab.Disabled = false;

        //    uxCrossingSecurity.LoadContent(CrossingSecurityUrl);
        //    uxCrossingInfoTab.LoadContent(CrossingInfoUrl);
        //    uxContactsTab.LoadContent(contactsUrl);
        //    uxSubDivisionsTab.LoadContent(SubDivUrl);
        //    uxServiceUnitsTab.LoadContent(ServiceUnitsUrl);
        //    uxDataEntryTab.LoadContent(DataEntryUrl);

        //}
        }

    }
