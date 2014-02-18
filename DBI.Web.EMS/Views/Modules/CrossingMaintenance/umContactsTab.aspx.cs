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
    public partial class umContactsTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
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
            string RailRoad = uxAddNewRRTextField.Value.ToString();
            string Address1 = uxAddNewAddress1.Value.ToString();
            string Address2 = uxAddNewAddress2.Value.ToString();
            string City = uxAddNewContactCityTextField.Value.ToString();
            string Zip = uxAddNewContactZip.Value.ToString();
            string State = uxAddNewContactStateTextField.Value.ToString();
            string Cell = uxAddNewContactCell.Value.ToString();
            string Office = uxAddNewContactOffice.Value.ToString();

            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new CROSSING_CONTACTS()
                {

                    CONTACT_NAME = ContactName,
                    RAILROAD = RailRoad.ToString(),
                    ADDRESS_1 = Address1,
                    ADDRESS_2 = Address2,
                    CITY = City,
                    ZIP_CODE = Zip,
                    STATE = State,
                    CELL_NUMBER = Cell,
                    WORK_NUMBER = Office,

                };
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
                uxEditRRTextField.SetValue(data.RAILROAD);
                uxEditContactAdd1.SetValue(data.ADDRESS_1);
                uxEditContactAdd2.SetValue(data.ADDRESS_2);
                uxEditContactCity.SetValue(data.CITY);
                uxEditContactStateTextField.SetValue(data.STATE);
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
            string RailRoad = uxEditRRTextField.Value.ToString();
            string Address1 = uxEditContactAdd1.Value.ToString();
            string Address2 = uxEditContactAdd2.Value.ToString();
            string City = uxEditContactCity.Value.ToString();
            string Zip = uxEditContactZip.Value.ToString();
            string State = uxEditContactStateTextField.Value.ToString();
            string Cell = uxEditContactCellNum.Value.ToString();
            string Office = uxEditContactPhoneNum.Value.ToString();

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
            data.ADDRESS_1 = Address1;
            data.ADDRESS_2 = Address2;
            data.CITY = City;
            data.ZIP_CODE = Zip;
            data.STATE = State;
            data.CELL_NUMBER = Cell;
            data.WORK_NUMBER = Office;


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
                        select new { d.CONTACT_ID, d.CONTACT_NAME, d.CELL_NUMBER, d.WORK_NUMBER }).ToList<object>();

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
                            select d).Single();
                    data.CONTACT_ID = ContactId;
                }
                GenericData.Update<CROSSING>(data);
            }

            uxTransferCrossingWindow.Hide();
            uxUpdateContactWindow.Hide();
            uxUpdateContactWindow.ClearContent();
            uxCurrentManagerCrossingStore.Reload();
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
    
