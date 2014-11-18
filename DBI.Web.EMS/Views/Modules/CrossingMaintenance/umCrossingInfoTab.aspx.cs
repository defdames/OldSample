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
    public partial class umCrossings : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CrossingMaintenance.InformationView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {
                //deLoadUnit();
                //deLoadType("Edit");
                uxAddStateList.Data = StaticLists.CrossingStateList;
                uxEditStateList.Data = StaticLists.CrossingStateList;
                uxAddPropertyType.Data = StaticLists.PropertyType;
                uxEditPropertyType.Data = StaticLists.PropertyType;
            }
      
        }

        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {
            
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                using (Entities _context = new Entities())
                {
                //Get List of all new crossings
                     IQueryable<CROSSING_MAINTENANCE.CrossingList> data;
                if (uxToggleClosed.Checked)
                {

                   data = CROSSING_MAINTENANCE.GetCrossingProjectList(RailroadId, _context);
                }
                else
                {
                    
                    data = CROSSING_MAINTENANCE.GetCrossingNoProjectList(RailroadId, _context);
                }
               
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
                        if (data.d.CUT_ONLY == "Y")
                        {
                            uxOnSpurCI.Checked = true;
                        }
              
            }  
        
        }
        protected void deGetRRType(object sender, DirectEventArgs e)
        {
            CROSSING data = new CROSSING();
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                var RRdata = (from r in _context.CROSSING_RAILROAD
                              where r.RAILROAD_ID == RailroadId
                              select new
                              {
                                  r

                              }).Single();


                uxAddRailRoadCITextField.SetValue(RRdata.r.RAILROAD);
                string rrType = RRdata.r.RAILROAD;
                if (e.ExtraParams["Type"] == "Add")
                {
                    List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(rrType).ToList();
                    uxAddServiceUnitCI.Clear();
                    uxAddSubDivCI.Clear();
                    uxAddServiceUnitStore.DataSource = units;
                    uxAddServiceUnitStore.DataBind();
                }
                else
                {
                    List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(rrType).ToList();
                   
                    //uxEditSubDivCIBox.Items.Clear();
                    uxEditServiceUnitStore.DataSource = units;
                    uxEditServiceUnitStore.DataBind();
                   
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
            CheckboxSelectionModel sm = CheckboxSelectionModel2;
            //Do type conversions
            //string CrossingNum = uxAddCrossingNumCI.Value.ToString();
           
            string DotNum = uxAddDotCI.Value.ToString();           
            decimal MP = Convert.ToDecimal(uxAddMPCINumberField.Value);
            string State = uxAddStateComboBox.Value.ToString();         
            string Sub_divisions = uxAddSubDivCI.Value.ToString();          
            string Sub_contracted = uxAddSubConCI.Value.ToString();
            string Restricted = uxAddRestrictedCI.Value.ToString();
            string FenceEncroach = uxAddFenceEnchroachCI.Value.ToString();
            string OnSpur = uxAddOnSpurCI.Value.ToString();
            string CutOnly = uxAddCutOnlyCI.Value.ToString();
            string RailRoad = uxAddRailRoadCITextField.Value.ToString();
            string ServiceUnit = uxAddServiceUnitCI.Value.ToString();
            string SubDiv = uxAddSubDivCI.Value.ToString();
            long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));

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
            if (uxAddCutOnlyCI.Checked)
            {
                CutOnly = "Y";
            }
            else
            {
                CutOnly = "N";
            }
            CROSSING data = new CROSSING()
            {
                
                //CROSSING_NUMBER = CrossingNum,
                MILE_POST = MP,               
                STATE = State,               
                CROSSING_NUMBER = DotNum,
                SUB_CONTRACTED = Sub_contracted,
                RESTRICTED_COUNTY = Restricted,
                FENCE_ENCROACHMENT = FenceEncroach,
                ON_SPUR = OnSpur,
                RAILROAD = RailRoad,
                SERVICE_UNIT = ServiceUnit,
                SUB_DIVISION = SubDiv,
                RAILROAD_ID = RailroadId,
                CREATED_DATE = DateTime.Now,
                MODIFIED_DATE = DateTime.Now,
                CREATED_BY = User.Identity.Name,
                MODIFIED_BY = User.Identity.Name,
                CUT_ONLY = CutOnly,
            };
             try
            {
                string City = uxAddCityCI.Value.ToString();
                data.CITY = City;
            }
            catch(Exception)
            {
                data.CITY = null;
            }
             try
             {
                 string Route = uxAddRouteCI.Value.ToString();
                 data.ROUTE = Route;
             }
             catch (Exception)
             {
                 data.ROUTE = null;
             }
             try
             {
                 string County = uxAddCountyCI.Value.ToString();
                 data.COUNTY = County;
             }
             catch (Exception)
             {
                 data.COUNTY = null;
             }
             try
             {
               string Street = uxAddStreetCI.Value.ToString();
               data.STREET = Street;
             }
             catch (Exception)
             {
                 data.STREET = null;
             }
          
            try
            {
                decimal Latitude = Convert.ToDecimal(uxAddLatCINumberField.Value);
                data.LATITUDE = Latitude;
            }
            catch (FormatException)
            {
                data.LATITUDE = null;
            }
            try
            {
                decimal Longitude = Convert.ToDecimal(uxAddLongCINumberField.Value);
                data.LONGITUDE = Longitude;
            }               
                           
            catch (FormatException)
            {
                data.LONGITUDE = null;
            }
            try
            {
                decimal NE = Convert.ToDecimal(uxAddNECINumberField.Value);
                data.ROWNE = NE;
            }
            catch (FormatException)
            {
                data.ROWNE = null;
            }
            try
            {
                decimal NEext = Convert.ToDecimal(uxAddNEextCINumberField.Value);
               data.EXTNE = NEext;
            }
            catch (FormatException)
            {
                data.EXTNE = null;
            }
            try
            {
                string RowWidth = uxAddRowWidthCI.Value.ToString();
                data.ROW_WIDTH = RowWidth;
            }
            catch (Exception)
            {
                data.ROW_WIDTH = null;
            }
            try
            {
                decimal NW = Convert.ToDecimal(uxAddNWCINumberField.Value);
                data.ROWNW = NW;
            }
            catch (Exception)
            {
                data.ROWNW = null;
            }
              try
            {
                decimal NWext = Convert.ToDecimal(uxAddNWextCINumberField.Value);
                 data.EXTNW = NWext;
            }
            catch (Exception)
            {
                data.EXTNW = null;
            }
              try
            {
                string PropertyType = uxAddPropertyTypeComboBox.Value.ToString();
                data.PROPERTY_TYPE = PropertyType;
            }
            catch (Exception)
            {
                data.PROPERTY_TYPE = null;
            }
              try
            {
                decimal SE = Convert.ToDecimal(uxAddSECINumberField.Value);
                      data.ROWSE = SE;
            }
            catch (Exception)
            {
                data.ROWSE = null;
            }
              try
            {
                decimal SEext = Convert.ToDecimal(uxAddSEextCINumberField.Value);
                data.EXTSE = SEext;
            }
            catch (Exception)
            {
                data.EXTSE = null;
            }
              try
            {
                 string Surface = uxAddSurfaceCI.Value.ToString();
                    data.SURFACE = Surface;
            }
            catch (Exception)
            {
                data.SURFACE = null;
            }
            try
            {
                decimal SW = Convert.ToDecimal(uxAddSWCINumberField.Value);
                    data.ROWSW = SW;
            }
            catch (Exception)
            {
                data.ROWSW = null;
            }
            try
            {
                decimal SWext = Convert.ToDecimal(uxAddSWextCINumberField.Value);
                    data.EXTSW = SWext;
            }
            catch (Exception)
            {
                data.EXTSW = null;
            }
            try
            {
                   string WarningDevice = uxAddWarningDeviceCI.Value.ToString(); 
                   data.WARNING_DEVICE = WarningDevice;
            }
            catch (Exception)
            {
                data.WARNING_DEVICE = null;
            }
            try
            {
                 long MainTracks = Convert.ToInt64(uxAddMainTracksCINumberField.Value);    
                     data.MAIN_TRACKS = MainTracks;
            }
            catch (Exception)
            {
                data.MAIN_TRACKS = null;
            }
            try
            {
                  long OtherTracks = Convert.ToInt64(uxAddOtherTracksCINumberField.Value);  
                  data.OTHER_TRACKS = OtherTracks;
            }
            catch (Exception)
            {
                data.OTHER_TRACKS = null;
            }
            try
            {
            long MaxSpeed = Convert.ToInt64(uxAddMaxSpeedCINumberField.Value);
              data.MAX_SPEED = MaxSpeed;
            }
            catch (Exception)
            {
                data.MAX_SPEED = null;
            }
            try
            {
            string SpecialInstructions = uxAddSpecialInstructCI.Value.ToString();
             data.SPECIAL_INSTRUCTIONS = SpecialInstructions;
            }
            catch(Exception)
            {
                data.SPECIAL_INSTRUCTIONS = null;
            }
            
         
            if (data.STATUS == null)
            {
                data.STATUS = "ACTIVE";
            }

            //if (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue") != null)
            //{
            //    data.RAILROAD_ID.ToString() = (SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
            //}
            //Write to DB
            GenericData.Insert<CROSSING>(data);
           
            long CrossingId = data.CROSSING_ID;
            //do type conversions

            foreach (SelectedRow sr in sm.SelectedRows)
            {
                CROSSING_RELATIONSHIP ProjectToAdd = new CROSSING_RELATIONSHIP
                {
                    PROJECT_ID = long.Parse(sr.RecordID),
                    CROSSING_ID = CrossingId,
                };

                GenericData.Insert<CROSSING_RELATIONSHIP>(ProjectToAdd);
            }
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
        protected void deValidateCancelButton(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = CheckboxSelectionModel2;
            sm.ClearSelection();
        }
        protected void deAddProject(object sender, DirectEventArgs e)
        {
            uxCurrentSecurityProjectStore.Reload();
        //    CheckboxSelectionModel sm = CheckboxSelectionModel2;

        //    //do type conversions
        //     long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
        //    foreach (SelectedRow sr in sm.SelectedRows)
        //    {

                 
        //            CROSSING_RELATIONSHIP ProjectToAdd = new CROSSING_RELATIONSHIP
        //            {
        //                PROJECT_ID = long.Parse(sr.RecordID),
        //                CROSSING_ID = CrossingId,
        //            };

        //            GenericData.Insert<CROSSING_RELATIONSHIP>(ProjectToAdd);
        //        }


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
                            join r in _context.CROSSING_RAILROAD on d.RAILROAD_ID equals r.RAILROAD_ID
                            where d.CROSSING_ID == CrossingId
                            select new
                            {
                                 d, r
                              
                            }).SingleOrDefault();
               
               
                //uxEditCrossingNumCI.SetValue(data.d.CROSSING_NUMBER);
                uxEditRouteCI.SetValue(data.d.ROUTE);
                uxEditDotCI.SetValue(data.d.CROSSING_NUMBER);
                uxEditStreetCI.SetValue(data.d.STREET);
                uxEditMPCINumberField.SetValue(data.d.MILE_POST);
                uxEditStateComboBox.SetValue(data.d.STATE);
                uxEditCityCI.SetValue(data.d.CITY);
                uxEditLatCINumberField.SetValue(data.d.LATITUDE);
                uxEditCountyCI.SetValue(data.d.COUNTY);
                uxEditLongCINumberField.SetValue(data.d.LONGITUDE);
                uxEditNECINumberField.SetValue(data.d.ROWNE);
                uxEditNEextCINumberField.SetValue(data.d.EXTNE);
                uxEditRowWidthCI.SetValue(data.d.ROW_WIDTH);
                uxEditNWCINumberField.SetValue(data.d.ROWNW);
                uxEditNWextCINumberField.SetValue(data.d.EXTNW);
                uxEditPropertyTypeComboBox.SetValue(data.d.PROPERTY_TYPE);
                uxEditSECINumberField.SetValue(data.d.ROWSE);
                uxEditSEextCINumberField.SetValue(data.d.EXTSE);
                uxEditSurfaceCI.SetValue(data.d.SURFACE);
                uxEditSWCINumberField.SetValue(data.d.ROWSW);
                uxEditSWextCINumberField.SetValue(data.d.EXTSW);
                uxEditWarningDeviceCI.SetValue(data.d.WARNING_DEVICE);
                uxEditMainTracksCINumberField.SetValue(data.d.MAIN_TRACKS);
                uxEditOtherTracksCINumberField.SetValue(data.d.OTHER_TRACKS);
                uxEditMaxSpeedCINumberField.SetValue(data.d.MAX_SPEED);
                uxEditSpecialInstructCI.SetValue(data.d.SPECIAL_INSTRUCTIONS);
                uxEditSubConCI.SetValue(data.d.SUB_CONTRACTED);
                uxEditRestrictedCI.SetValue(data.d.RESTRICTED_COUNTY);
                uxEditFenceEnchroachCI.SetValue(data.d.FENCE_ENCROACHMENT);
                uxEditOnSpurCI.SetValue(data.d.ON_SPUR);
                uxEditRRCI.SetValue(data.r.RAILROAD);
                uxEditServiceUnitCI.SetValueAndFireSelect(data.d.SERVICE_UNIT);
                uxEditSubDivCIBox.SetValueAndFireSelect(data.d.SUB_DIVISION);
               

                if (data.d.SUB_CONTRACTED == "Y")
                {
                    uxEditSubConCI.Checked = true;
                }
                if (data.d.RESTRICTED_COUNTY == "Y")
                {
                    uxEditRestrictedCI.Checked = true;
                }
                if (data.d.FENCE_ENCROACHMENT == "Y")
                {
                    uxEditFenceEnchroachCI.Checked = true;
                }
                if (data.d.ON_SPUR == "Y")
                {
                    uxEditOnSpurCI.Checked = true;
                }
                if (data.d.CUT_ONLY == "Y")
                {
                    uxEditCutOnlyCI.Checked = true;
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
            //string CrossingNum = uxEditCrossingNumCI.Value.ToString();
           
            string DotNum = uxEditDotCI.Value.ToString();           
            decimal MP = Convert.ToDecimal(uxEditMPCINumberField.Value);
            string State = uxEditStateComboBox.Value.ToString();          
            string Sub_divisions = uxEditSubDivCIBox.Value.ToString();
            string Sub_contracted = uxEditSubConCI.Value.ToString();
            string Restricted = uxEditRestrictedCI.Value.ToString();
            string FenceEncroach = uxEditFenceEnchroachCI.Value.ToString();
            string OnSpur = uxEditSubConCI.Value.ToString();
            string CutOnly = uxEditCutOnlyCI.Value.ToString();
            string RailRoad = uxEditRRCI.Value.ToString();
            string ServiceUnit = uxEditServiceUnitCI.Value.ToString();
            long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
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
            if (uxEditCutOnlyCI.Checked)
            {
                CutOnly = "Y";
            }
            else
            {
               CutOnly = "N";
            }
            //Get record to be edited
            using (Entities _context = new Entities())
            {
                var CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                data = (from d in _context.CROSSINGS
                        where d.CROSSING_ID == CrossingId
                        select d).Single();
                                     
               
            }
           

                //data.CROSSING_NUMBER = CrossingNum;
                data.MILE_POST = MP;
                data.SUB_DIVISION = Sub_divisions;
               
                data.STATE = State;
               
               
                data.CROSSING_NUMBER = DotNum;
                data.SUB_CONTRACTED = Sub_contracted;
                data.RESTRICTED_COUNTY = Restricted;
                data.FENCE_ENCROACHMENT = FenceEncroach;
                data.ON_SPUR = OnSpur;
                data.RAILROAD_ID = RailroadId;
                data.SERVICE_UNIT = ServiceUnit;
                data.MODIFIED_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
                try
                {
                    string City = uxEditCityCI.Value.ToString();
                    data.CITY = City;
                }
                catch (Exception)
                {
                    data.CITY = null;
                }
                try
                {
                    string Route = uxEditRouteCI.Value.ToString();
                    data.ROUTE = Route;
                }
                catch (Exception)
                {
                    data.ROUTE = null;
                }
                try
                {
                    string County = uxEditCountyCI.Value.ToString();
                    data.COUNTY = County;
                }
                catch (Exception)
                {
                    data.COUNTY = null;
                }
                try
                {
                    string Street = uxEditStreetCI.Value.ToString();
                    data.STREET = Street;
                }
                catch (Exception)
                {
                    data.STREET = null;
                }
                try
                {
                    decimal Latitude = decimal.Parse(uxEditLatCINumberField.Value.ToString());
                    data.LATITUDE = Latitude;
                }
                catch (Exception)
                {
                    data.LATITUDE = null;
                }
                try
                {
                    decimal Longitude = Convert.ToDecimal(uxEditLongCINumberField.Value);
                    data.LONGITUDE = Longitude;
                }
                catch (Exception)
                {
                    data.LONGITUDE = null;
                }
                try
                {
                    decimal NE = Convert.ToDecimal(uxEditNECINumberField.Value);
                data.ROWNE = NE;
                }
             catch (Exception)
                {
                 data.ROWNE = null;
                }
             try
                {
                    decimal NEext = Convert.ToDecimal(uxEditNEextCINumberField.Value);
                data.EXTNE = NEext;
                }
             catch (Exception)
                {
                  data.EXTNE = null;
                }
             try
                {
                string RowWidth = uxEditRowWidthCI.Value.ToString();
                data.ROW_WIDTH = RowWidth;
                }
                catch (Exception)
                {
                    data.ROW_WIDTH = null;
                }
             try
                {
                    decimal NW = Convert.ToDecimal(uxEditNWCINumberField.Value);
                data.ROWNW = NW;
                }
                catch (Exception)
                {
                    data.ROWNW = null;
                }
             try
                {
                    decimal NWext = Convert.ToDecimal(uxEditNWextCINumberField.Value);
                data.EXTNW = NWext;
                }
                catch (Exception)
                {
                    data.EXTNW = null;
                }
             try
                {
                string PropertyType = uxEditPropertyTypeComboBox.Value.ToString();
                data.PROPERTY_TYPE = PropertyType;
                }
                  catch (Exception)
                {
                      data.PROPERTY_TYPE = null;
                }
             try
                {
                    decimal SE = Convert.ToDecimal(uxEditSECINumberField.Value);
                data.ROWSE = SE;
                }
                catch (Exception)
                {
                    data.ROWSE = null;
                }
             try
                {
                    decimal SEext = Convert.ToDecimal(uxEditSEextCINumberField.Value);
                data.EXTSE = SEext;
                }
                catch (Exception)
                {
                    data.EXTSE = null;
                }
             try
                {
                string Surface = uxEditSurfaceCI.Value.ToString();
                data.SURFACE = Surface;
                }  
                catch (Exception)
                {
                    data.SURFACE = null;
                }
             try
                {
                    decimal SW = Convert.ToDecimal(uxEditSWCINumberField.Value);
                data.ROWSW = SW;
                }
                 catch (Exception)
                {
                     data.ROWSW = null;
                }
             try
                {
                    decimal SWext = Convert.ToDecimal(uxEditSWextCINumberField.Value);
                data.EXTSW = SWext;
                }
                 catch (Exception)
                {
                     data.EXTSW = null;
                }
             try
                {
                string WarningDevice = uxEditWarningDeviceCI.Value.ToString();
                data.WARNING_DEVICE = WarningDevice.ToString();
                }
                catch (Exception)
                {
                    data.WARNING_DEVICE = null;
                }
             try
                {
                    long MainTracks = Convert.ToInt64(uxEditMainTracksCINumberField.Value);
                data.MAIN_TRACKS = MainTracks;
                }
                 catch (Exception)
                {
                     data.MAIN_TRACKS = null;
                }
             try
                {
                    long OtherTracks = Convert.ToInt64(uxEditOtherTracksCINumberField.Value);
                data.OTHER_TRACKS = OtherTracks;
                }
                 catch (Exception)
                {
                     data.OTHER_TRACKS = null;
                }
             try
                {
                long MaxSpeed = Convert.ToInt64(uxEditMaxSpeedCINumberField.Value);
                data.MAX_SPEED = MaxSpeed;
                }
                catch (Exception)
                {
                    data.MAX_SPEED = null;
                }
             try
             {
                 string SpecialInstructions = uxEditSpecialInstructCI.Value.ToString();
                 data.SPECIAL_INSTRUCTIONS = SpecialInstructions;
             }

             catch (Exception)
             {
                 data.SPECIAL_INSTRUCTIONS = null;
             }
             //if (Session["rrType"] != null)
             //{
             //    Session["rrType"] = data.RAILROAD_ID;
             //}
               
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
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS
                        where d.CROSSING_ID == CrossingId
                        select d).Single();

                data.STATUS = "ACTIVE";
            }
            GenericData.Update<CROSSING>(data);
           
            uxCrossingForm.Reset();
            uxCurrentCrossingStore.Reload();

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
        //protected void deLoadType(string rrType)
        //{
        //    List<ServiceUnitResponse> types = ServiceUnitData.ServiceUnitTypes().ToList();
        //    if (rrType == "Add")
        //    {

        //        //uxAddRailRoadStore.DataSource = types;
        //        //uxAddRailRoadStore.DataBind();
        //    }
        //    //else
        //    //{
        //    //    uxEditRRStore.DataSource = types;
        //    //    uxEditRRStore.DataBind();

        //    //}

        //}
        //protected void deLoadUnit(object sender, DirectEventArgs e)
        //{
        //    //if (e.ExtraParams["Type"] == "Add")
        //    //{
        //    string rrType = Session["rrType"].ToString();
               
        //        List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(rrType).ToList();
        //        uxAddServiceUnitCI.Clear();
        //        uxAddSubDivCI.Clear();
        //        uxAddServiceUnitStore.DataSource = units;
        //        uxAddServiceUnitStore.DataBind();

        //    //}
        //    //else
        //    //{
        //    //    List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(uxEditRRCI.Value.ToString()).ToList();
        //    //    uxEditServiceUnitStore.DataSource = units;
        //    //    uxEditServiceUnitStore.DataBind();

        //    //}

        //}
        protected void deLoadSubDiv(object sender, DirectEventArgs e)
       {    
            
          
           if (e.ExtraParams["Type"] == "Add")
           {
               List<ServiceUnitResponse> divisions = ServiceUnitData.ServiceUnitDivisions(uxAddServiceUnitCI.SelectedItem.Value).ToList();
               uxAddSubDivCI.Clear();
               uxAddSubDivStore.DataSource = divisions;
               uxAddSubDivStore.DataBind();
           }
           else
           {
                 List<ServiceUnitResponse> divisions = ServiceUnitData.ServiceUnitDivisions(uxEditServiceUnitCI.SelectedItem.Value).ToList();
                
                 uxEditSubDivStore.DataSource = divisions;
                 uxEditSubDivStore.DataBind();
           }
          
       }
        
           
        //protected void deAddManagerGrid(object sender, StoreReadDataEventArgs e)
        //{

        //    //Get Contacts
        //    using (Entities _context = new Entities())
        //    {
        //        long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
        //        List<object> data;
        //        data = (from d in _context.CROSSING_CONTACTS
        //                where d.RAILROAD_ID == RailroadId
        //                select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER }).ToList<object>();
        //        int count;
        //        uxAddManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
        //        e.Total = count;
        //    }
        //}
        //protected void deEditManagerGrid(object sender, StoreReadDataEventArgs e)
        //{

        //    //Get Contacts
        //    using (Entities _context = new Entities())
        //    {
        //        long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
        //        List<object> data;
        //        data = (from d in _context.CROSSING_CONTACTS
        //                where d.RAILROAD_ID == RailroadId
        //                select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER }).ToList<object>();
        //        int count;
        //        uxEditManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
        //        e.Total = count;
        //    }
        //}
        protected void deAddProjectGrid(object sender, StoreReadDataEventArgs e)
        {
            {
                    long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                    List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                    using (Entities _context = new Entities())
                    {
                        IQueryable<CROSSING_MAINTENANCE.ProjectList> data = CROSSING_MAINTENANCE.GetCrossingSecurityProjectList(RailroadId, _context).Where(v => v.PROJECT_TYPE == "CUSTOMER BILLING" && v.TEMPLATE_FLAG == "N" && v.PROJECT_STATUS_CODE == "APPROVED" && v.ORGANIZATION_NAME.Contains(" RR") && OrgsList.Contains(v.CARRYING_OUT_ORGANIZATION_ID) && RailroadId != null);

                        int count;
                        uxCurrentSecurityProjectStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.ProjectList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                        e.Total = count;

                    }
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
        //protected void deStoreEditManagerValue(object sender, DirectEventArgs e)
        //{
        //    switch (e.ExtraParams["Type"])
        //    {
        //        case "EditManager":
        //            uxEditManagerCI.SetValue(e.ExtraParams["ContactId"], e.ExtraParams["ContactName"]);
        //            uxEditManagerFilter.ClearFilter();
        //            break;

        //    }
        //}
        //protected void deStoreAddProjectValue(object sender, DirectEventArgs e)
        //{
        //    switch (e.ExtraParams["Type"])
        //    {
        //        case "AddProject":
        //            uxAddProjectCIDropDownField.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["ProjectName"]);
        //            uxAddProjectFilter.ClearFilter();
        //            break;

        //    }
        //}
        //protected void deStoreEditProjectValue(object sender, DirectEventArgs e)
        //{
        //    switch (e.ExtraParams["Type"])
        //    {
        //        case "EditProject":
        //            uxEditProjectCI.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["ProjectName"]);
        //            uxEditProjectFilter.ClearFilter();
        //            break;

        //    }
        //}
        
        protected void deGetProjectList(object sender, DirectEventArgs e)
        {
                long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

                var data = CROSSING_MAINTENANCE.CrossingsProjectList(CrossingId);

                uxProjectListStore.DataSource = data;
                uxProjectListStore.DataBind();
        }
    }
}

    

            
        
    
