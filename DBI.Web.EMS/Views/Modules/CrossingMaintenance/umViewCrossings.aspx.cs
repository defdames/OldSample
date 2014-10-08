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
using DBI.Core.Security;
using System.Security.Claims;
using System.IO;
using System.Data.Entity;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umViewCrossings : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {
                //deLoadUnit();
                //deLoadType("Edit");
                //uxAddStateList.Data = StaticLists.CrossingStateList;
                //uxEditStateList.Data = StaticLists.CrossingStateList;
                //uxAddPropertyType.Data = StaticLists.PropertyType;
                //uxEditPropertyType.Data = StaticLists.PropertyType;
            }

        }

        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
            using (Entities _context = new Entities())
            {
                //Get List of all new crossings
                IQueryable<CROSSING_MAINTENANCE.CrossingList> data = CROSSING_MAINTENANCE.GetCrossingProjectListIncidents(RailroadId, _context);
              
                int count;
                uxCurrentCrossingStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.CrossingList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;

            }
        }

        //<summary>
        //Get Current Crossing Form Data
        //</summary>

        protected void GetFormData(object sender, DirectEventArgs e)
        {


            using (Entities _context = new Entities())
            {

                long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                var data = (from d in _context.CROSSINGS
                            join r in _context.CROSSING_RAILROAD on d.RAILROAD_ID equals r.RAILROAD_ID
                            where d.CROSSING_ID == CrossingId
                            select new { d, r }).SingleOrDefault();
                try
                {
                    long ContactId = long.Parse(data.d.CONTACT_ID.ToString());
                    var contactdata = (from c in _context.CROSSING_CONTACTS
                                       where c.CONTACT_ID == ContactId

                                       select c.CONTACT_NAME).Single();

                    uxAddManagerCI.SetValue(contactdata);

                }
                catch (Exception)
                {
                    uxAddManagerCI.Value = string.Empty;


                }

                uxServiceUnitCI.SetValue(data.d.SERVICE_UNIT);
                uxSubDivCI.SetValue(data.d.SUB_DIVISION);
                uxRRCI.SetValue(data.r.RAILROAD);
                uxRouteCI.SetValue(data.d.ROUTE);
                uxDOTCI.SetValue(data.d.CROSSING_NUMBER);
                uxStreetCI.SetValue(data.d.STREET);
                uxMPCI.SetValue(data.d.MILE_POST);
                uxStateCI.SetValue(data.d.STATE);
                uxCityCI.SetValue(data.d.CITY);
                uxLatCI.SetValue(data.d.LATITUDE);
                uxSubDivCI.SetValue(data.d.SUB_DIVISION);
                uxCountyCI.SetValue(data.d.COUNTY);
                uxLongCI.SetValue(data.d.LONGITUDE);
                uxNECI.SetValue(data.d.ROWNE);
                uxNEextCI.SetValue(data.d.EXTNE);
                uxRowWidthCI.SetValue(data.d.ROW_WIDTH);
                uxNWCI.SetValue(data.d.ROWNW);
                uxNWextCI.SetValue(data.d.EXTNW);
                uxPropertyTypeCI.SetValue(data.d.PROPERTY_TYPE);
                uxSECI.SetValue(data.d.ROWSE);
                uxSEextCI.SetValue(data.d.EXTSE);
                uxSurfaceCI.SetValue(data.d.SURFACE);
                uxSWCI.SetValue(data.d.ROWSW);
                uxSWextCI.SetValue(data.d.EXTSW);
                uxCrossingWarningDevice.SetValue(data.d.WARNING_DEVICE);
                uxMainTracksCI.SetValue(data.d.MAIN_TRACKS);
                uxOtherTracksCI.SetValue(data.d.OTHER_TRACKS);
                uxMaxSpeedCI.SetValue(data.d.MAX_SPEED);
                uxSpecialInstructCI.SetValue(data.d.SPECIAL_INSTRUCTIONS);
                uxSubConCI.SetValue(data.d.SUB_CONTRACTED);
                uxRestrictedBoxCI.SetValue(data.d.RESTRICTED_COUNTY);
                uxFenceEncroachCI.SetValue(data.d.FENCE_ENCROACHMENT);
                uxOnSpurCI.SetValue(data.d.ON_SPUR);

                if (data.d.SUB_CONTRACTED == "Y")
                {
                    uxSubConCI.Checked = true;
                }
                if (data.d.RESTRICTED_COUNTY == "Y")
                {
                    uxRestrictedBoxCI.Checked = true;
                }
                if (data.d.FENCE_ENCROACHMENT == "Y")
                {
                    uxFenceEncroachCI.Checked = true;
                }
                if (data.d.ON_SPUR == "Y")
                {
                    uxOnSpurCI.Checked = true;
                }


            }

        }
        protected void deDeleteCrossing(object sender, DirectEventArgs e)
        {
            CROSSING data;
            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS
                        where d.CROSSING_ID == CrossingId
                        select d).Single();

                data.STATUS = "DELETED";
                data.DELETED_BY = User.Identity.Name;
                data.DELETED_DATE = DateTime.Now;
            }
            GenericData.Update<CROSSING>(data);

            uxCrossingForm.Reset();
            uxCurrentCrossingStore.Reload();
            uxDeleteCrossingButton.Disable();
            uxReactivateCrossingButton.Disable();
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Deleted Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deReactivateCrossing(object sender, DirectEventArgs e)
        {
            CROSSING data;
            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
            string CutOnly = uxCutOnly.Value.ToString();
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS
                        where d.CROSSING_ID == CrossingId
                        select d).Single();

                data.STATUS = "ACTIVE";
                if (uxCutOnly.Checked)
                {
                    CutOnly = "Y";
                }
                else
                {
                    CutOnly = "N";
                }
                data.CUT_ONLY = CutOnly;
            }
            GenericData.Update<CROSSING>(data);

            uxCutOnlyWindow.Hide();
            uxReactivateForm.Reset();
            uxCrossingForm.Reset();
            uxCurrentCrossingStore.Reload();
            uxReactivateCrossingButton.Disable();
            uxDeleteCrossingButton.Disable();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Reactivated Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
       
    }
}