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
        /// <summary>
        /// 
        /// </summary>
        protected void deContactMainGrid(object sender, StoreReadDataEventArgs e)
        {
          
            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                 data = (from d in _context.CROSSING_CONTACTS
                         //join c in _context.CROSSINGS
                         //on d.CROSSING_ID equals c.CROSSING_ID                 
                            select new
                            {  d.CONTACT_ID, d.CONTACT_NAME, d.WORK_NUMBER, d.CELL_NUMBER, d.RAIL_ROAD  }).ToList<object>();
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
                            { d.CONTACT_ID, d.CONTACT_NAME, d.RAIL_ROAD, d.ADDRESS_1, d.ADDRESS_2, d.CITY, d.STATE, d.ZIP_CODE, d.CELL_NUMBER, d.WORK_NUMBER,
                          }).SingleOrDefault();
                uxContactManagerName.SetValue(data.CONTACT_NAME);
                uxContactRR.SetValue(data.RAIL_ROAD);
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
                    RAIL_ROAD = RailRoad.ToString(),
                    ADDRESS_1 = Address1,
                    ADDRESS_2 = Address2,
                    CITY = City,
                    ZIP_CODE = Zip,
                    STATE = State,
                    CELL_NUMBER = Cell,
                    WORK_NUMBER = Office,
              
                };
            }
           
            //Process addition
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
                                d.RAIL_ROAD,
                                d.ADDRESS_1,
                                d.ADDRESS_2,
                                d.CITY,
                                d.STATE,
                                d.ZIP_CODE,
                                d.CELL_NUMBER,
                                d.WORK_NUMBER,
                            }).SingleOrDefault();
                uxEditManagerName.SetValue(data.CONTACT_NAME);
                uxEditRRTextField.SetValue(data.RAIL_ROAD);
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
                    data.RAIL_ROAD = RailRoad;
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

                //Get List of all new headers

                data = (from d in _context.CROSSING_CONTACTS
                        select new { d.CONTACT_NAME, }).ToList<object>();

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

                //Get List of all new headers

                data = (from d in _context.CROSSINGS
                        select new { d.CROSSING_NUMBER,  }).ToList<object>();

                int count;
                uxAssignContactCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }

        }
    }
}