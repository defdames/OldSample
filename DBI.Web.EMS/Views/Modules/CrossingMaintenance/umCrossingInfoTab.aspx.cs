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
            //if (!X.IsAjaxRequest)
            //{
            //    GetFormData();
            //}
        }
        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers

                data = (from d in _context.CROSSINGS
                        select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.SUB_CONTRACTED }).ToList<object>();

                int count;
                uxCurrentCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
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
                            where d.CROSSING_ID == CrossingId
                            select new { d.CROSSING_NUMBER,d.ROUTE, d.DOT, d.STREET, d.MILE_POST, d.STATE, d.CITY, d.LATITUDE,
                            d.SUB_DIVISION, d.COUNTY, d.LONGITUDE, d.ROWNE, d.EXTNE, d.ROW_WIDTH, d.ROWNW, d.EXTNW, d.PROPERTY_TYPE,
                            d.ROWSE, d.EXTSE, d.SURFACE, d.ROWSW, d.EXTSW, d.WARNING_DEVICE, d.MTM, d.MTM_OFFICE_NUMBER, d.MTM_PHONE_NUMBER,
                            d.MAIN_TRACKS, d.OTHER_TRACKS, d.MAX_SPEED, d.SPECIAL_INSTRUCTIONS, d.FENCE_ENCROACHMENT, d.RESTRICTED_COUNTY, 
                            d.SUB_CONTRACTED, d.ON_SPUR}).SingleOrDefault();
                uxCrossingNumCI.SetValue(data.CROSSING_NUMBER);
                uxRouteCI.SetValue(data.ROUTE);
                uxDOTNumCI.SetValue(data.DOT);
                uxStreetCI.SetValue(data.STREET);
                uxMPCI.SetValue(data.MILE_POST);
                uxStateCI.SetValue(data.STATE);
                uxCityCI.SetValue(data.CITY);
                uxLatCI.SetValue(data.LATITUDE);
                uxSubDivCI.SetValue(data.SUB_DIVISION);
                uxCountyCI.SetValue(data.COUNTY);
                uxLongCI.SetValue(data.LONGITUDE);
                uxNECI.SetValue(data.ROWNE);
                uxNEextCI.SetValue(data.EXTNE);
                uxRowWidthCI.SetValue(data.ROW_WIDTH);
                uxNWCI.SetValue(data.ROWNW);
                uxNWextCI.SetValue(data.EXTNW);
                uxPropertyTypeCI.SetValue(data.PROPERTY_TYPE);
                uxSECI.SetValue(data.ROWSE);
                uxSEextCI.SetValue(data.EXTSE);
                uxSurfaceCI.SetValue(data.SURFACE);
                uxSWCI.SetValue(data.ROWSW);
                uxSWextCI.SetValue(data.EXTSW);
                uxCrossingWarningDevice.SetValue(data.WARNING_DEVICE);
                uxMTMCI.SetValue(data.MTM);
                uxMTMOfficeCI.SetValue(data.MTM_OFFICE_NUMBER);
                uxMTMCellCI.SetValue(data.MTM_PHONE_NUMBER);
                uxMainTracksCI.SetValue(data.MAIN_TRACKS);
                uxOtherTracksCI.SetValue(data.OTHER_TRACKS);
                uxMaxSpeedCI.SetValue(data.MAX_SPEED);
                uxSpecialInstructCI.SetValue(data.SPECIAL_INSTRUCTIONS);
               
               
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
            string Route= uxAddRouteCI.Value.ToString();
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
            string Mtm = uxAddMTMCI.Value.ToString();
            long MainTracks = Convert.ToInt64(uxAddMainTracksCI.Value);          
            string MtmCell = uxAddMTMCellCI.Value.ToString();
            long OtherTracks = Convert.ToInt64(uxAddOtherTracksCI.Value);          
            string MtmOffice = uxAddMTMOfficeCI.Value.ToString();
            long MaxSpeed = Convert.ToInt64(uxAddMaxSpeedCI.Value);
            string SpecialInstructions = uxAddSpecialInstructCI.Value.ToString();
            string Sub_contracted = uxAddSubConCI.Value.ToString();
            
                    if (uxAddSubConCI.Checked)
                    {
                        Sub_contracted = "Y";
                    }
                    else
                    {
                        Sub_contracted = "N";
                    }
                   
                    //if (uxAddRestrictedCI.Checked)
                    //{
                    //    Restricted = "Y";
                    //}
                    //else
                    //{
                    //    Restricted = "N";
                    //}  
                    // if (uxAddFenceEnchroachCI.Checked)
                    //{
                    //    FenceEncroach= "Y";
                    //}
                    //else
                    //{
                    //    FenceEncroach = "N";
                    //}
                    // if (uxAddOnSpurCI.Checked)
                    //{
                    //    Sub_contracted = "Y";
                    //}
                    //else
                    //{
                    //    Sub_contracted = "N";
                    //}

                    CROSSING data = new CROSSING()
                    {
                 //using (Entities _context = new Entities())
                 //{

                 //      data = new CROSSING()

                 
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
                     MTM = Mtm,
                     MTM_PHONE_NUMBER = MtmCell,
                     MTM_OFFICE_NUMBER = MtmOffice,


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
                uxEditMTMCI.SetValue(data.MTM);
                uxEditMTMOfficeCI.SetValue(data.MTM_OFFICE_NUMBER);
                uxEditMTMCellCI.SetValue(data.MTM_PHONE_NUMBER);
                uxEditMainTracksCI.SetValue(data.MAIN_TRACKS);
                uxEditOtherTracksCI.SetValue(data.OTHER_TRACKS);
                uxEditMaxSpeedCI.SetValue(data.MAX_SPEED);
                uxEditSpecialInstructCI.SetValue(data.SPECIAL_INSTRUCTIONS);
                //uxEditOnSpurCI.SetValue(data.ON_SPUR);
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
            string Sub_divisions = uxEditDOTNumCI.Value.ToString();
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
            string Mtm = uxEditMTMCI.Value.ToString();
            long MainTracks = Convert.ToInt64(uxEditMainTracksCI.Value);          
            string MtmCell = uxEditMTMCellCI.Value.ToString();
            long OtherTracks = Convert.ToInt64(uxEditOtherTracksCI.Value);           
            string MtmOffice = uxEditMTMOfficeCI.Value.ToString();
            long MaxSpeed = Convert.ToInt64(uxEditMaxSpeedCI.Value);
            string SpecialInstructions = uxEditSpecialInstructCI.Value.ToString();

           
           
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
                    data.MTM = Mtm; data.MTM_PHONE_NUMBER = MtmCell; data.MTM_OFFICE_NUMBER = MtmOffice;
                    //RESTRICTED_COUNTY = Restricted,           
                    //ON_SPUR = OnSpur,
                    //SUB_CONTRACTED = Sub_contracted,
                    //FENCE_ENCROACHMENT = FenceEncroach,
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
            
            
        }
    }
