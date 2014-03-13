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
    public partial class umCrossings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                
                deLoadType("Add");
                deLoadType("Edit");
                uxAddStateList.Data = StaticLists.StateList;
                uxEditStateList.Data = StaticLists.StateList;
            }
          
        }

        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {
           
            using (Entities _context = new Entities())
            {
                List<object> data;

                //GeList of all new crossings
                
                    data = (from d in _context.CROSSINGS
                            join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID into pn
                            from proj in pn.DefaultIfEmpty()
                            select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID, proj.LONG_NAME}).ToList<object>();
              

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

                            where d.CROSSING_ID == CrossingId

                            select new
                            {
                                d
                            }).SingleOrDefault();
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
                uxRRCI.SetValue(data.d.RAILROAD);
                uxCrossingNumCI.SetValue(data.d.CROSSING_NUMBER);
                uxRouteCI.SetValue(data.d.ROUTE);
                uxDOTCI.SetValue(data.d.DOT);
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
            string DotNum = uxAddDotCI.Value.ToString();
            string Street = uxAddStreetCI.Value.ToString();
            decimal MP = Convert.ToDecimal(uxAddMPCI.Value);
            string State = uxAddStateComboBox.Value.ToString();
            string City = uxAddCityCI.Value.ToString();
            string Sub_divisions = uxAddSubDivCI.Value.ToString();
            string County = uxAddCountyCI.Value.ToString();
            string Sub_contracted = uxAddSubConCI.Value.ToString();
            string Restricted = uxAddRestrictedCI.Value.ToString();
            string FenceEncroach = uxAddFenceEnchroachCI.Value.ToString();
            string OnSpur = uxAddOnSpurCI.Value.ToString();
            string RailRoad = uxAddRailRoadCI.Value.ToString();
            string ServiceUnit = uxAddServiceUnitCI.Value.ToString();
            string SubDiv = uxAddSubDivCI.Value.ToString();

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
                MILE_POST = MP,
                CITY = City,
                STREET = Street,
                STATE = State,
                COUNTY = County,
                ROUTE = Route,   
                DOT = DotNum,
                SUB_CONTRACTED = Sub_contracted,
                RESTRICTED_COUNTY = Restricted,
                FENCE_ENCROACHMENT = FenceEncroach,
                ON_SPUR = OnSpur,
                RAILROAD = RailRoad,
                SERVICE_UNIT = ServiceUnit,
                SUB_DIVISION = SubDiv,
                
            };

            try
            {
                long ContactName = Convert.ToInt64(uxAddManagerCIDropDownField.Value);
                data.CONTACT_ID = ContactName;
            }
            catch (FormatException)
            {
                data.CONTACT_ID = null;
            }
            try
            {
                decimal Latitude = Convert.ToDecimal(uxAddLatCI.Value);
                data.LATITUDE = Latitude;
            }
            catch (FormatException)
            {
                data.LATITUDE = null;
            }
            try
            {
                decimal Longitude = Convert.ToDecimal(uxAddLongCI.Value);
                data.LONGITUDE = Longitude;
            }               
                           
            catch (FormatException)
            {
                data.LONGITUDE = null;
            }
            try
            {
                decimal NE = Convert.ToDecimal(uxAddNECI.Value);
                data.ROWNE = NE;
            }
            catch (FormatException)
            {
                data.ROWNE = null;
            }
            try
            {
               decimal NEext = Convert.ToDecimal(uxAddNEextCI.Value);
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
                 decimal NW = Convert.ToDecimal(uxAddNWCI.Value);
                data.ROWNW = NW;
            }
            catch (Exception)
            {
                data.ROWNW = null;
            }
              try
            {
                 decimal NWext = Convert.ToDecimal(uxAddNWextCI.Value);
                 data.EXTNW = NWext;
            }
            catch (Exception)
            {
                data.EXTNW = null;
            }
              try
            {
                string PropertyType = uxAddPropertyTypeCI.Value.ToString();
                data.PROPERTY_TYPE = PropertyType;
            }
            catch (Exception)
            {
                data.PROPERTY_TYPE = null;
            }
              try
            {
                 decimal SE = Convert.ToDecimal(uxAddSECI.Value);
                      data.ROWSE = SE;
            }
            catch (Exception)
            {
                data.ROWSE = null;
            }
              try
            {
               decimal SEext = Convert.ToDecimal(uxAddSEextCI.Value);
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
                decimal SW = Convert.ToDecimal(uxAddSWCI.Value);
                    data.ROWSW = SW;
            }
            catch (Exception)
            {
                data.ROWSW = null;
            }
            try
            {
                 decimal SWext = Convert.ToDecimal(uxAddSWextCI.Value);
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
                 long MainTracks = Convert.ToInt64(uxAddMainTracksCI.Value);    
                     data.MAIN_TRACKS = MainTracks;
            }
            catch (Exception)
            {
                data.MAIN_TRACKS = null;
            }
            try
            {
                  long OtherTracks = Convert.ToInt64(uxAddOtherTracksCI.Value);  
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
                                 d
                              
                            }).SingleOrDefault();
                try
                {
                    long ContactId = long.Parse(data.d.CONTACT_ID.ToString());
                    var contactdata = (from c in _context.CROSSING_CONTACTS
                                       where c.CONTACT_ID == ContactId

                                       select c.CONTACT_NAME).Single();
                    uxEditManagerCI.SetValue(data.d.CONTACT_ID.ToString(), contactdata);
                }
                catch (Exception) 
                {
                    uxEditManagerCI.Value = string.Empty;
                }
               
                uxEditCrossingNumCI.SetValue(data.d.CROSSING_NUMBER);
                uxEditRouteCI.SetValue(data.d.ROUTE);
                uxEditDotCI.SetValue(data.d.DOT);
                uxEditStreetCI.SetValue(data.d.STREET);
                uxEditMPCI.SetValue(data.d.MILE_POST);
                uxEditStateComboBox.SetValue(data.d.STATE);
                uxEditCityCI.SetValue(data.d.CITY);
                uxEditLatCI.SetValue(data.d.LATITUDE);
                uxEditSubDivCIBox.SetValueAndFireSelect(data.d.SUB_DIVISION);
                uxEditCountyCI.SetValue(data.d.COUNTY);
                uxEditLongCI.SetValue(data.d.LONGITUDE);
                uxEditNECI.SetValue(data.d.ROWNE);
                uxEditNEextCI.SetValue(data.d.EXTNE);
                uxEditRowWidthCI.SetValue(data.d.ROW_WIDTH);
                uxEditNWCI.SetValue(data.d.ROWNW);
                uxEditNWextCI.SetValue(data.d.EXTNW);
                uxEditPropertyTypeCI.SetValue(data.d.PROPERTY_TYPE);
                uxEditSECI.SetValue(data.d.ROWSE);
                uxEditSEextCI.SetValue(data.d.EXTSE);
                uxEditSurfaceCI.SetValue(data.d.SURFACE);
                uxEditSWCI.SetValue(data.d.ROWSW);
                uxEditSWextCI.SetValue(data.d.EXTSW);
                uxEditWarningDeviceCI.SetValue(data.d.WARNING_DEVICE);
                uxEditMainTracksCI.SetValue(data.d.MAIN_TRACKS);
                uxEditOtherTracksCI.SetValue(data.d.OTHER_TRACKS);
                uxEditMaxSpeedCINumberField.SetValue(data.d.MAX_SPEED);
                uxEditSpecialInstructCI.SetValue(data.d.SPECIAL_INSTRUCTIONS);
                uxEditSubConCI.SetValue(data.d.SUB_CONTRACTED);
                uxEditRestrictedCI.SetValue(data.d.RESTRICTED_COUNTY);
                uxEditFenceEnchroachCI.SetValue(data.d.FENCE_ENCROACHMENT);
                uxEditOnSpurCI.SetValue(data.d.ON_SPUR);
                uxEditRRCI.SetValueAndFireSelect(data.d.RAILROAD);
                uxEditServiceUnitCI.SetValueAndFireSelect(data.d.SERVICE_UNIT);

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
            string DotNum = uxEditDotCI.Value.ToString();
            string Street = uxEditStreetCI.Value.ToString();
            decimal MP = Convert.ToDecimal(uxEditMPCI.Value);
            string State = uxEditStateComboBox.Value.ToString();
            string City = uxEditCityCI.Value.ToString();
            string County = uxEditCountyCI.Value.ToString();
            string Sub_divisions = uxEditSubDivCIBox.Value.ToString();
            string Sub_contracted = uxEditSubConCI.Value.ToString();
            string Restricted = uxEditRestrictedCI.Value.ToString();
            string FenceEncroach = uxEditFenceEnchroachCI.Value.ToString();
            string OnSpur = uxEditSubConCI.Value.ToString();
            string RailRoad = uxEditRRCI.Value.ToString();
            string ServiceUnit = uxEditServiceUnitCI.Value.ToString();

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
            if (uxEditManagerCI.Text != "")
            {
                long ContactName = Convert.ToInt64(uxEditManagerCI.Value.ToString());
                data.CONTACT_ID = ContactName;
              
            }
            else
            {
                data.CONTACT_ID = null;
            }    

                data.CROSSING_NUMBER = CrossingNum;
                data.MILE_POST = MP;
                data.SUB_DIVISION = Sub_divisions;
                data.CITY = City;
                data.STREET = Street;
                data.STATE = State;
                data.COUNTY = County;
                data.ROUTE = Route;
                data.DOT = DotNum;
                data.SUB_CONTRACTED = Sub_contracted;
                data.RESTRICTED_COUNTY = Restricted;
                data.FENCE_ENCROACHMENT = FenceEncroach;
                data.ON_SPUR = OnSpur;
                data.RAILROAD = RailRoad;
                data.SERVICE_UNIT = ServiceUnit;
                try
                {
                    decimal Latitude = decimal.Parse(uxEditLatCI.Value.ToString());
                    data.LATITUDE = Latitude;
                }
                catch (Exception)
                {
                    data.LATITUDE = null;
                }
                try
                {
                    decimal Longitude = Convert.ToDecimal(uxEditLongCI.Value);
                    data.LONGITUDE = Longitude;
                }
                catch (Exception)
                {
                    data.LONGITUDE = null;
                }
                try
                {
                decimal NE = Convert.ToDecimal(uxEditNECI.Value);
                data.ROWNE = NE;
                }
             catch (Exception)
                {
                 data.ROWNE = null;
                }
             try
                {
                decimal NEext = Convert.ToDecimal(uxEditNEextCI.Value);
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
                decimal NW = Convert.ToDecimal(uxEditNWCI.Value);
                data.ROWNW = NW;
                }
                catch (Exception)
                {
                    data.ROWNW = null;
                }
             try
                {
                decimal NWext = Convert.ToDecimal(uxEditNWextCI.Value);
                data.EXTNW = NWext;
                }
                catch (Exception)
                {
                    data.EXTNW = null;
                }
             try
                {
                string PropertyType = uxEditPropertyTypeCI.Value.ToString();
                data.PROPERTY_TYPE = PropertyType;
                }
                  catch (Exception)
                {
                      data.PROPERTY_TYPE = null;
                }
             try
                {
                decimal SE = Convert.ToDecimal(uxEditSECI.Value);
                data.ROWSE = SE;
                }
                catch (Exception)
                {
                    data.ROWSE = null;
                }
             try
                {
                decimal SEext = Convert.ToDecimal(uxEditSEextCI.Value);
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
                decimal SW = Convert.ToDecimal(uxEditSWCI.Value);
                data.ROWSW = SW;
                }
                 catch (Exception)
                {
                     data.ROWSW = null;
                }
             try
                {
                decimal SWext = Convert.ToDecimal(uxEditSWextCI.Value);
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
                long MainTracks = Convert.ToInt64(uxEditMainTracksCI.Value);
                data.MAIN_TRACKS = MainTracks;
                }
                 catch (Exception)
                {
                     data.MAIN_TRACKS = null;
                }
             try
                {
                long OtherTracks = Convert.ToInt64(uxEditOtherTracksCI.Value);
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

        protected void deLoadType(string rrType)
        {
            
            List<ServiceUnitResponse> types = ServiceUnitData.ServiceUnitTypes().ToList();
            if (rrType == "Add")
            {
                uxAddRailRoadStore.DataSource = types;
                uxAddRailRoadStore.DataBind();
            }
            else
            {
                uxEditRRStore.DataSource = types;
                uxEditRRStore.DataBind();
            }
                
        }
        protected void deLoadUnit(object sender, DirectEventArgs e)
       {
           if (e.ExtraParams["Type"] == "Add")
           {
               List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(uxAddRailRoadCI.SelectedItem.Value).ToList();
               uxAddServiceUnitCI.Clear();
               uxAddSubDivCI.Clear();
               uxAddServiceUnitStore.DataSource = units;
               uxAddServiceUnitStore.DataBind();
           }
           else
           {
               List<ServiceUnitResponse> units = ServiceUnitData.ServiceUnitUnits(uxEditRRCI.SelectedItem.Value).ToList();
               uxEditServiceUnitStore.DataSource = units;
               uxEditServiceUnitStore.DataBind();

           }
           
       }
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
        
           
        protected void deAddManagerGrid(object sender, StoreReadDataEventArgs e)
        {

            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                data = (from d in _context.CROSSING_CONTACTS
  
                        select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER }).ToList<object>();
                int count;
                uxAddManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
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
        protected void deStoreAddManagerValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "AddManager":
                    uxAddManagerCIDropDownField.SetValue(e.ExtraParams["ContactId"], e.ExtraParams["ContactName"]);
                    uxAddManagerFilter.ClearFilter();
                    break;

            }
        }
        protected void deStoreEditManagerValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "EditManager":
                    uxEditManagerCI.SetValue(e.ExtraParams["ContactId"], e.ExtraParams["ContactName"]);
                    uxEditManagerFilter.ClearFilter();
                    break;

            }
        }
    }
}
    

            
        
    
