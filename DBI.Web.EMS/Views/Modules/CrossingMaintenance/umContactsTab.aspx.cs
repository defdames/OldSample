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
                         join c in _context.CROSSINGS
                         on d.CROSSING_ID equals c.CROSSING_ID
                           
                            select new
                            { d.CROSSING_ID, d.CONTACT_ID, d.CONTACT_NAME  }).ToList<object>();
                int count;
             uxCurrentContactStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
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
                            { d.CONTACT_NAME, d.RAIL_ROAD, d.ADDRESS_1, d.ADDRESS_2, d.CITY, d.STATE, d.ZIP_CODE, d.CELL_NUMBER, d.WORK_NUMBER,
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
            //uxAddContactForm.Reset();
            //uxCurrentContactStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Inventory Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deRemoveContact(object sender, EventArgs e)
        {

        }
   
    }
}