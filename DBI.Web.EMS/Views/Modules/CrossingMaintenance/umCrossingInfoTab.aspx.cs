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
    public partial class umCrossings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers

                data = (from d in _context.CROSSINGS
                      
                        select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.SUB_DIVISION, }).ToList<object>();

                int count;
                uxCurrentCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
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
                            join c in _context.CROSSING_CONTACTS on d.CONTACT_ID equals c.CONTACT_ID
                            where d.CROSSING_ID == CrossingId
                           
                            select new
                            { 
                                d,
                                c.CONTACT_NAME,
                                
                            }).Single();
                uxAddManagerCI.SetValue(data.CONTACT_NAME);
                uxCrossingNumCI.SetValue(data.d.CROSSING_NUMBER);
                uxRouteCI.SetValue(data.d.ROUTE);
                uxDOTNumCI.SetValue(data.d.DOT);
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

        /// <summary>
        /// Add Crossing to Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddCrossings(object sender, DirectEventArgs e)
        {

            //Do type conversions
            string CrossingNum = uxAddCrossingNumCI.Value.ToString();
            string Route = uxAddRouteCI.Value.ToString();
            string DotNum = uxAddDOTNumCI.Value.ToString();
            string Street = uxAddStreetCI.Value.ToString();
            decimal MP = Convert.ToDecimal(uxAddMPCI.Value);
            string State = uxAddStateCI.Value.ToString();
            string City = uxAddCityCI.Value.ToString();
            decimal Latitude = Convert.ToDecimal(uxAddLatCI.Value);
            string Sub_divisions = uxAddSubDivCI.Value.ToString();
            string County = uxAddCountyCI.Value.ToString();
            decimal Longitude = Convert.ToDecimal(uxAddLongCI.Value);
            decimal NE = Convert.ToDecimal(uxAddNECI.Value);
            decimal NEext = Convert.ToDecimal(uxAddNEextCI.Value);
            string RowWidth = uxAddRowWidthCI.Value.ToString();
            decimal NW = Convert.ToDecimal(uxAddNWCI.Value);
            decimal NWext = Convert.ToDecimal(uxAddNWextCI.Value);
            string PropertyType = uxAddPropertyTypeCI.Value.ToString();
            decimal SE = Convert.ToDecimal(uxAddSECI.Value);
            decimal SEext = Convert.ToDecimal(uxAddSEextCI.Value);
            string Surface = uxAddSurfaceCI.Value.ToString();
            decimal SW = Convert.ToDecimal(uxAddSWCI.Value);
            decimal SWext = Convert.ToDecimal(uxAddSWextCI.Value);
            string WarningDevice = uxAddWarningDeviceCI.Value.ToString();         
            long MainTracks = Convert.ToInt64(uxAddMainTracksCI.Value);         
            long OtherTracks = Convert.ToInt64(uxAddOtherTracksCI.Value);         
            long MaxSpeed = Convert.ToInt64(uxAddMaxSpeedCINumberField.Value);
            string SpecialInstructions = uxAddSpecialInstructCI.Value.ToString();
            string Sub_contracted = uxAddSubConCI.Value.ToString();
            string Restricted = uxAddRestrictedCI.Value.ToString();
            string FenceEncroach = uxAddFenceEnchroachCI.Value.ToString();
            string OnSpur = uxAddOnSpurCI.Value.ToString();

            if (uxAddSubConCI.Checked)
            {
                Sub_contracted = "Y";
            }
            else
            {
                Sub_contracted = "N";
            }

            if (uxAddRestrictedCI.Checked)
            {
                Restricted = "Y";
            }
            else
            {
                Restricted = "N";
            }
            if (uxAddFenceEnchroachCI.Checked)
            {
                FenceEncroach = "Y";
            }
            else
            {
                FenceEncroach = "N";
            }
            if (uxAddOnSpurCI.Checked)
            {
                OnSpur = "Y";
            }
            else
            {
                OnSpur = "N";
            }

            CROSSING data = new CROSSING()
            {

                CROSSING_NUMBER = CrossingNum,
                DOT = DotNum,
                MILE_POST = MP,
                SUB_DIVISION = Sub_divisions,
                CITY = City,
                STREET = Street,
                STATE = State,
                COUNTY = County,
                SPECIAL_INSTRUCTIONS = SpecialInstructions,
                ROUTE = Route,
                MAIN_TRACKS = MainTracks,
                OTHER_TRACKS = OtherTracks,
                MAX_SPEED = MaxSpeed,
                LONGITUDE = Longitude,
                LATITUDE = Latitude,
                PROPERTY_TYPE = PropertyType,
                WARNING_DEVICE = WarningDevice,
                SURFACE = Surface,
                ROWNE = NE,
                EXTNE = NEext,
                ROWNW = NW,
                EXTNW = NWext,
                ROWSE = SE,
                EXTSE = SEext,
                ROWSW = SW,
                EXTSW = SWext,
                ROW_WIDTH = RowWidth,
                SUB_CONTRACTED = Sub_contracted,
                RESTRICTED_COUNTY = Restricted,
                FENCE_ENCROACHMENT = FenceEncroach,
                ON_SPUR = OnSpur,
                
            };

            //Write to DB
            GenericData.Insert<CROSSING>(data);

            uxAddCrossingWindow.Hide();
            uxAddCrossingForm.Reset();
            uxCurrentCrossingStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Load current values into Edit Crossing Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditCrossingForm(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                var data = (from d in _context.CROSSINGS
                            where d.CROSSING_ID == CrossingId
                            select new
                            {
                                d.CROSSING_NUMBER,
                                d.ROUTE,
                                d.DOT,
                                d.STREET,
                                d.MILE_POST,
                                d.STATE,
                                d.CITY,
                                d.LATITUDE,
                                d.SUB_DIVISION,
                                d.COUNTY,
                                d.LONGITUDE,
                                d.ROWNE,
                                d.EXTNE,
                                d.ROW_WIDTH,
                                d.ROWNW,
                                d.EXTNW,
                                d.PROPERTY_TYPE,
                                d.ROWSE,
                                d.EXTSE,
                                d.SURFACE,
                                d.ROWSW,
                                d.EXTSW,
                                d.WARNING_DEVICE,
                                d.MTM,
                                d.MTM_OFFICE_NUMBER,
                                d.MTM_PHONE_NUMBER,
                                d.MAIN_TRACKS,
                                d.OTHER_TRACKS,
                                d.MAX_SPEED,
                                d.SPECIAL_INSTRUCTIONS,
                                d.FENCE_ENCROACHMENT,
                                d.RESTRICTED_COUNTY,
                                d.SUB_CONTRACTED,
                                d.ON_SPUR
                            }).SingleOrDefault();
                uxEditCrossingNumCI.SetValue(data.CROSSING_NUMBER);
                uxEditRouteCI.SetValue(data.ROUTE);
                uxEditDOTNumCI.SetValue(data.DOT);
                uxEditStreetCI.SetValue(data.STREET);
                uxEditMPCI.SetValue(data.MILE_POST);
                uxEditStateCI.SetValue(data.STATE);
                uxEditCityCI.SetValue(data.CITY);
                uxEditLatCI.SetValue(data.LATITUDE);
                uxEditSubDivCI.SetValue(data.SUB_DIVISION);
                uxEditCountyCI.SetValue(data.COUNTY);
                uxEditLongCI.SetValue(data.LONGITUDE);
                uxEditNECI.SetValue(data.ROWNE);
                uxEditNEextCI.SetValue(data.EXTNE);
                uxEditRowWidthCI.SetValue(data.ROW_WIDTH);
                uxEditNWCI.SetValue(data.ROWNW);
                uxEditNWextCI.SetValue(data.EXTNW);
                uxEditPropertyTypeCI.SetValue(data.PROPERTY_TYPE);
                uxEditSECI.SetValue(data.ROWSE);
                uxEditSEextCI.SetValue(data.EXTSE);
                uxEditSurfaceCI.SetValue(data.SURFACE);
                uxEditSWCI.SetValue(data.ROWSW);
                uxEditSWextCI.SetValue(data.EXTSW);
                uxEditWarningDeviceCI.SetValue(data.WARNING_DEVICE);
                uxEditMainTracksCI.SetValue(data.MAIN_TRACKS);
                uxEditOtherTracksCI.SetValue(data.OTHER_TRACKS);
                uxEditMaxSpeedCINumberField.SetValue(data.MAX_SPEED);
                uxEditSpecialInstructCI.SetValue(data.SPECIAL_INSTRUCTIONS);
                uxEditSubConCI.SetValue(data.SUB_CONTRACTED);
                uxEditRestrictedCI.SetValue(data.RESTRICTED_COUNTY);
                uxEditFenceEnchroachCI.SetValue(data.FENCE_ENCROACHMENT);
                uxEditOnSpurCI.SetValue(data.ON_SPUR);

                if (data.SUB_CONTRACTED == "Y")
                {
                    uxEditSubConCI.Checked = true;
                }
                if (data.RESTRICTED_COUNTY == "Y")
                {
                    uxEditRestrictedCI.Checked = true;
                }
                if (data.FENCE_ENCROACHMENT == "Y")
                {
                    uxEditFenceEnchroachCI.Checked = true;
                }
                if (data.ON_SPUR == "Y")
                {
                    uxEditOnSpurCI.Checked = true;
                }

            }
        }

        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditCrossings(object sender, DirectEventArgs e)
        {
            CROSSING data;

            //Do type conversions
            string CrossingNum = uxEditCrossingNumCI.Value.ToString();
            string Route = uxEditRouteCI.Value.ToString();
            string DotNum = uxEditDOTNumCI.Value.ToString();
            string Street = uxEditStreetCI.Value.ToString();
            decimal MP = Convert.ToDecimal(uxEditMPCI.Value);
            string State = uxEditStateCI.Value.ToString();
            string City = uxEditCityCI.Value.ToString();
            decimal Latitude = Convert.ToDecimal(uxEditLatCI.Value);
            string Sub_divisions = uxEditSubDivCI.Value.ToString();
            string County = uxEditCountyCI.Value.ToString();
            decimal Longitude = Convert.ToDecimal(uxEditLongCI.Value);
            decimal NE = Convert.ToDecimal(uxEditNECI.Value);
            decimal NEext = Convert.ToDecimal(uxEditNEextCI.Value);
            string RowWidth = uxEditRowWidthCI.Value.ToString();
            decimal NW = Convert.ToDecimal(uxEditNWCI.Value);
            decimal NWext = Convert.ToDecimal(uxEditNWextCI.Value);
            string PropertyType = uxEditPropertyTypeCI.Value.ToString();
            decimal SE = Convert.ToDecimal(uxEditSECI.Value);
            decimal SEext = Convert.ToDecimal(uxEditSEextCI.Value);
            string Surface = uxEditSurfaceCI.Value.ToString();
            decimal SW = Convert.ToDecimal(uxEditSWCI.Value);
            decimal SWext = Convert.ToDecimal(uxEditSWextCI.Value);
            string WarningDevice = uxEditWarningDeviceCI.Value.ToString();           
            long MainTracks = Convert.ToInt64(uxEditMainTracksCI.Value);           
            long OtherTracks = Convert.ToInt64(uxEditOtherTracksCI.Value);            
            long MaxSpeed = Convert.ToInt64(uxEditMaxSpeedCINumberField.Value);
            string SpecialInstructions = uxEditSpecialInstructCI.Value.ToString();
            string Sub_contracted = uxEditSubConCI.Value.ToString();
            string Restricted = uxEditRestrictedCI.Value.ToString();
            string FenceEncroach = uxEditFenceEnchroachCI.Value.ToString();
            string OnSpur = uxEditSubConCI.Value.ToString();

            if (uxEditSubConCI.Checked)
            {
                Sub_contracted = "Y";
            }
            else
            {
                Sub_contracted = "N";
            }
            if (uxEditRestrictedCI.Checked)
            {
                Restricted = "Y";
            }
            else
            {
                Restricted = "N";
            }
            if (uxEditFenceEnchroachCI.Checked)
            {
                FenceEncroach = "Y";
            }
            else
            {
                FenceEncroach = "N";
            }
            if (uxEditOnSpurCI.Checked)
            {
                OnSpur = "Y";
            }
            else
            {
                OnSpur = "N";
            }


            //Get record to be edited
            using (Entities _context = new Entities())
            {
                var CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                data = (from d in _context.CROSSINGS
                        where d.CROSSING_ID == CrossingId
                        select d).Single();
            }
            data.CROSSING_NUMBER = CrossingNum;
            data.DOT = DotNum;
            data.MILE_POST = MP;
            data.SUB_DIVISION = Sub_divisions;
            data.CITY = City;
            data.STREET = Street;
            data.STATE = State;
            data.COUNTY = County;
            data.SPECIAL_INSTRUCTIONS = SpecialInstructions;
            data.ROUTE = Route;
            data.MAIN_TRACKS = MainTracks;
            data.OTHER_TRACKS = OtherTracks;
            data.MAX_SPEED = MaxSpeed;
            data.LONGITUDE = Longitude;
            data.LATITUDE = Latitude;
            data.PROPERTY_TYPE = PropertyType;
            data.WARNING_DEVICE = WarningDevice.ToString();
            data.SURFACE = Surface;
            data.ROWNE = NE; data.EXTNE = NEext;
            data.ROWNW = NW; data.EXTNW = NWext;
            data.ROWSE = SE; data.EXTSE = SEext;
            data.ROWSW = SW; data.EXTSW = SWext;
            data.ROW_WIDTH = RowWidth;
            data.SUB_CONTRACTED = Sub_contracted;
            data.RESTRICTED_COUNTY = Restricted;
            data.FENCE_ENCROACHMENT = FenceEncroach;
            data.ON_SPUR = OnSpur;

            //Write to DB
            GenericData.Update<CROSSING>(data);

            uxEditCrossingWindow.Hide();
            uxCrossingForm.Reset();
            uxCurrentCrossingStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });

        }
        protected void deRemoveCrossing(object sender, DirectEventArgs e)
        {
            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
            CROSSING data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS
                        where d.CROSSING_ID == CrossingId
                        select d).Single();
            }
            GenericData.Delete<CROSSING>(data);

            uxCurrentCrossingStore.Reload();
            uxCrossingForm.Reset();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        //protected void deAddManagerGrid(object sender, StoreReadDataEventArgs e)
        //{

        //    //Get Contacts
        //    using (Entities _context = new Entities())
        //    {
        //        List<object> data;
        //        data = (from d in _context.CROSSING_CONTACTS
                        
        //                select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER }).ToList<object>();
        //        int count;
        //        uxAddManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
        //        e.Total = count;
        //    }
        //}
        protected void deEditManagerGrid(object sender, StoreReadDataEventArgs e)
        {

            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                data = (from d in _context.CROSSING_CONTACTS

                        select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER }).ToList<object>();
                int count;
                uxEditManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        //protected void deStoreAddManagerValue(object sender, DirectEventArgs e)
        //{
        //    switch (e.ExtraParams["Type"])
        //    {
        //        case "AddManager":
        //            uxAddManagerCIDropDownField.SetValue(e.ExtraParams["ContactId"], e.ExtraParams["ContactName"]);
        //            uxAddManagerFilter.ClearFilter();
        //            break;

        //    }
        //}
        protected void deStoreEditManagerValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "EditManager":
                    uxEditManagerCI.SetValue(e.ExtraParams["ContactName"], e.ExtraParams["Name"]);
                    uxEditManagerFilter.ClearFilter();
                    break;

            }
        }
    }
}
    

            
        
    
