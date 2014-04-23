using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using DBI.Data.DataFactory;
using DBI.Data.GMS;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umContactsTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                deLoadRRType("Add");
                deLoadRRType("Edit");
                uxAddStateList.Data = StaticLists.StateList;
                uxEditStateList.Data = StaticLists.StateList;
            }
        }

        protected void deContactMainGrid(object sender, StoreReadDataEventArgs e)
        {

            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                data = (from d in _context.CROSSING_CONTACTS

                        select new { d.CONTACT_ID, d.CONTACT_NAME, d.WORK_NUMBER, d.CELL_NUMBER, d.RAILROAD }).ToList<object>();
                int count;
                uxCurrentContactStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void GetContactGridData(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                long ContactId = long.Parse(e.ExtraParams["ContactId"]);
                var data = (from d in _context.CROSSING_CONTACTS
                            where d.CONTACT_ID == ContactId
                            select new
                            {
                                d.CONTACT_ID,
                                d.CONTACT_NAME,
                                d.RAILROAD,
                                d.ADDRESS_1,
                                d.ADDRESS_2,
                                d.CITY,
                                d.STATE,
                                d.ZIP_CODE,
                                d.CELL_NUMBER,
                                d.WORK_NUMBER,
                            }).SingleOrDefault();
                uxContactManagerName.SetValue(data.CONTACT_NAME);
                uxContactRR.SetValue(data.RAILROAD);
                uxContactAddress1.SetValue(data.ADDRESS_1);
                uxContactAddress2.SetValue(data.ADDRESS_2);
                uxContactCity.SetValue(data.CITY);
                uxContactState.SetValue(data.STATE);
                uxContactZip.SetValue(data.ZIP_CODE);
                uxContactCell.SetValue(data.CELL_NUMBER);
                uxContactOffice.SetValue(data.WORK_NUMBER);

            }
        }
        protected void deAddContact(object sender, DirectEventArgs e)
        {
            CROSSING_CONTACTS data;

            //do type conversions
            string ContactName = uxAddNewManagerName.Value.ToString();
            string RailRoad = uxAddRRType.Value.ToString();
           
            string State = uxAddContactStateComboBox.Value.ToString();
           

            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new CROSSING_CONTACTS()
                {

                    CONTACT_NAME = ContactName,
                    RAILROAD = RailRoad.ToString(),
                   
                    STATE = State,
                   
                };
            }
            try
            {
             string Address1 = uxAddNewAddress1.Value.ToString();
             data.ADDRESS_1 = Address1;
            }
            catch(Exception) 
            {
                data.ADDRESS_1 = null;
            }
            try
            {
            string Address2 = uxAddNewAddress2.Value.ToString();
             data.ADDRESS_2 = Address2;
            }
            catch(Exception) 
            {
                data.ADDRESS_2 = null;
            }
            try
            {
            string City = uxAddNewContactCityTextField.Value.ToString();
            data.CITY = City;
            }
            catch(Exception)
            {
                data.CITY = null;
            }
            try
            {
            string Zip = uxAddNewContactZip.Value.ToString();
             data.ZIP_CODE = Zip;
            }
            catch 
                (Exception)
            {
                data.ZIP_CODE = null;
            }
            try
            {
             string Cell = uxAddNewContactCell.Value.ToString();
                  data.CELL_NUMBER = Cell;
            }
            catch(Exception)
            {
                data.CELL_NUMBER = null;
            }
            try
            {
            string Office = uxAddNewContactOffice.Value.ToString();
            data.WORK_NUMBER = Office;
            }
            catch
            {
                data.WORK_NUMBER = null;
            }

            GenericData.Insert<CROSSING_CONTACTS>(data);

            uxAddContactWindow.Hide();
            uxAddContactForm.Reset();
            uxCurrentContactStore.Reload();


            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Contact Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deEditContactsForm(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                long ContactId = long.Parse(e.ExtraParams["ContactId"]);
                var data = (from d in _context.CROSSING_CONTACTS
                            where d.CONTACT_ID == ContactId
                            select new
                            {
                                d.CONTACT_ID,
                                d.CONTACT_NAME,
                                d.RAILROAD,
                                d.ADDRESS_1,
                                d.ADDRESS_2,
                                d.CITY,
                                d.STATE,
                                d.ZIP_CODE,
                                d.CELL_NUMBER,
                                d.WORK_NUMBER,
                            }).SingleOrDefault();
                uxEditManagerName.SetValue(data.CONTACT_NAME);
                uxEditRRType.SetValue(data.RAILROAD);
                uxEditContactAdd1.SetValue(data.ADDRESS_1);
                uxEditContactAdd2.SetValue(data.ADDRESS_2);
                uxEditContactCity.SetValue(data.CITY);
                uxEditContactState.SetValue(data.STATE);
                uxEditContactZip.SetValue(data.ZIP_CODE);
                uxEditContactCellNum.SetValue(data.CELL_NUMBER);
                uxEditContactPhoneNum.SetValue(data.WORK_NUMBER);
            }
        }




        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditContacts(object sender, DirectEventArgs e)
        {
            CROSSING_CONTACTS data;

            //do type conversions
            string ContactName = uxEditManagerName.Value.ToString();
            string RailRoad = uxEditRRType.Value.ToString();
         
            string State = uxEditContactState.Value.ToString();
          
            //Get record to be edited
            using (Entities _context = new Entities())
            {
                var ContactId = long.Parse(e.ExtraParams["ContactId"]);
                data = (from d in _context.CROSSING_CONTACTS
                        where d.CONTACT_ID == ContactId
                        select d).Single();
            }

            data.CONTACT_NAME = ContactName;
            data.RAILROAD = RailRoad;
            data.STATE = State;

            try
            {
                string Address1 = uxEditContactAdd1.Value.ToString();
                data.ADDRESS_1 = Address1;
            }
            catch (Exception)
            {
                data.ADDRESS_1 = null;
            }
            try
            {
                string Address2 = uxEditContactAdd2.Value.ToString();
                data.ADDRESS_2 = Address2;
            }
            catch (Exception)
            {
                data.ADDRESS_2 = null;
            }
            try
            {
                string City = uxEditContactCity.Value.ToString();
                data.CITY = City;
            }
            catch (Exception)
            {
                data.CITY = null;
            }
            try
            {
                string Zip = uxEditContactZip.Value.ToString();
                data.ZIP_CODE = Zip;
            }
            catch (Exception)
            {
                data.ZIP_CODE = null;
            }
            try
            {
                string Cell = uxEditContactCellNum.Value.ToString();
                data.CELL_NUMBER = Cell;
            }
            catch (Exception)
            {
                data.CELL_NUMBER = null;
            }
            try
            {
                string Office = uxEditContactPhoneNum.Value.ToString();
                data.WORK_NUMBER = Office;
            }
            catch (Exception)
            {
                data.WORK_NUMBER = null;
            }
        
     
         


            GenericData.Update<CROSSING_CONTACTS>(data);

            uxEditContactWindow.Hide();
            uxContactFormPanel.Reset();
            uxCurrentContactStore.Reload();

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
        protected void deRemoveContact(object sender, DirectEventArgs e)
        {
            long ContactId = long.Parse(e.ExtraParams["ContactId"]);
            CROSSING_CONTACTS data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSING_CONTACTS
                        where d.CONTACT_ID == ContactId
                        select d).Single();
            }
            GenericData.Delete<CROSSING_CONTACTS>(data);

            uxCurrentContactStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Contact Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deAssignContactManagerGrid(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new contacts

                data = (from d in _context.CROSSING_CONTACTS
                        select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER, d.RAILROAD }).ToList<object>();

                int count;
                uxAssignContactManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void deAssignContactCrossingGrid(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new crossings

                data = (from d in _context.CROSSINGS
                        where d.CONTACT_ID == null
                        select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.RAILROAD, d.SUB_DIVISION }).ToList<object>();

                int count;
                uxAssignContactCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
        protected void deAssignCrossingtoContact(object sender, DirectEventArgs e)
        {
            CROSSING data;

            //do type conversions

            RowSelectionModel sm = uxAssignContactGrid.GetSelectionModel() as RowSelectionModel;
            long ContactId = long.Parse(e.ExtraParams["contactId"]);


            string json = (e.ExtraParams["selectedCrossings"]);
            List<CrossingDetails> crossingList = JSON.Deserialize<List<CrossingDetails>>(json);
            foreach (CrossingDetails crossing in crossingList)
            {
                //Get record to be edited
                using (Entities _context = new Entities())
                {

                    data = (from d in _context.CROSSINGS
                            where d.CROSSING_ID == crossing.CROSSING_ID
                            select d).Single();
                    data.CONTACT_ID = ContactId;
                }
                GenericData.Update<CROSSING>(data);
            }

            uxAssignCrossingWindow.Hide();
            uxAssignContactManagerStore.Reload();
            uxAssignContactCrossingStore.Reload();
            uxContactFormPanel.Reset();



            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing to Contact Updated Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deCurrentManagerGrid(object sender, StoreReadDataEventArgs e)
        {

            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                data = (from d in _context.CROSSING_CONTACTS
                        select new { d.CONTACT_ID, d.CONTACT_NAME, }).ToList<object>();
                int count;
                uxCurrentManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void deNewManagerGrid(object sender, StoreReadDataEventArgs e)
        {

            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                data = (from d in _context.CROSSING_CONTACTS
                        select new { d.CONTACT_ID, d.CONTACT_NAME }).ToList<object>();
                int count;
                uxNewManagerStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void deStoreCurrentManagerValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "CurrentManager":
                    uxUpdateContactCurrentManager.SetValue(e.ExtraParams["ContactId"], e.ExtraParams["ContactName"]);
                    uxAddCurrentManagerFilter.ClearFilter();
                    break;

            }
        }
        protected void deStoreNewManagerValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "NewManager":
                    uxUpdateContactNewManager.SetValue(e.ExtraParams["ContactId"], e.ExtraParams["ContactName"]);
                    uxNewManagerFilter.ClearFilter();
                    break;
            }
        }
       
        protected void deShowGrid(object sender, DirectEventArgs e)
        {
                long ContactId = long.Parse(uxUpdateContactCurrentManager.Value.ToString());
                             
                    using (Entities _context = new Entities())
                    {
                        List<object> list;
                        list = (from d in _context.CROSSINGS
                                where d.CONTACT_ID == ContactId
                                select d).ToList<object>();

                        uxCurrentManagerCrossingStore.DataSource = list;
                        uxCurrentManagerCrossingStore.DataBind();

                        uxTransferCrossingWindow.Show();
                    }      
        }
        protected void AssociateTransfer(object sender, DirectEventArgs e)
        {
            CROSSING data;

            //do type conversions

            long ContactId = long.Parse(uxUpdateContactNewManager.Value.ToString());

            string json = (e.ExtraParams["selectedCrossings"]);
            List<CrossingDetails> crossingList = JSON.Deserialize<List<CrossingDetails>>(json);
            foreach (CrossingDetails crossing in crossingList)
            {
                //Get record to be edited
                using (Entities _context = new Entities())
                {
                   
                        data = (from d in _context.CROSSINGS
                                where d.CROSSING_ID == crossing.CROSSING_ID
                                select d).SingleOrDefault();


                        data.CONTACT_ID = ContactId;
                    
                }
                GenericData.Update<CROSSING>(data);
            }

            uxTransferCrossingWindow.Hide();
            uxUpdateContactWindow.Hide();
            uxCurrentManagerCrossingStore.Reload();
            uxTransferCrossingsNewManagerStore.Reload();
            uxUpdateContactForm.Reset();
            uxContactFormPanel.Reset();



            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing(s) Transferred Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deLoadRRType(string rrType)
        {

            List<ServiceUnitResponse> types = ServiceUnitData.ServiceUnitTypes().ToList();
            if (rrType == "Add")
            {
                uxAddContactRRStore.DataSource = types;
                uxAddContactRRStore.DataBind();
            }
            else
            {
                uxEditContactRRStore.DataSource = types;
                uxEditContactRRStore.DataBind();
            }

        }
        public class CrossingDetails
        {
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string RAILROAD { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
        }
    }
}
    
