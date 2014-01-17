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
        //protected void GetGridData()
        //{
        //    long ContactId = long.Parse(Request.QueryString["ContactId"]);

        //    //Get Inventory for current project
        //    using (Entities _context = new Entities())
        //    {
        //        var data = (from d in _context.CROSSING_CONTACTS                       
        //                    where d.CONTACT_ID == ContactId
        //                    select new {d.CONTACT_ID, d.CONTACT_NAME, d.RAIL_ROAD, d.ADDRESS_1, d.ADDRESS_2, d.CITY, d.ZIP_CODE, d.STATE,
        //                    d.CELL_NUMBER, d.WORK_NUMBER }).ToList();

        //        uxCurrentContactStore.DataSource = data;
        //    }
        //}
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