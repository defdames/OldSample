using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umAddEditEquipment : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {
                if (Request.QueryString["type"] == "Edit")
                {
                    uxFormType.Value = "Edit";
                    LoadEditEquipmentForm();
                }
                else
                {
                    uxFormType.Value = "Add";
                }
            }
        }

        /// <summary>
        /// Filter Equipment list for add/edit drop-down grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadGrid(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_EQUIPMENT_V> dataIn;

            if (uxAddEquipmentToggleOrg.Pressed)
            {
                //Get All Projects
                dataIn = WEB_EQUIPMENT_V.ListEquipment();
            }
            else
            {
                int CurrentOrg = Convert.ToInt32(Authentication.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                //Get projects for my org only
                dataIn = WEB_EQUIPMENT_V.ListEquipment(CurrentOrg);
            }

            int count;

            //Get paged, filterable list of Equipment
            List<WEB_EQUIPMENT_V> data = GenericData.EnumerableFilterHeader<WEB_EQUIPMENT_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxEquipmentStore.DataSource = data;
        }

        protected void deProcessForm(object sender, DirectEventArgs e)
        {
            if (uxFormType.Value.ToString() == "Add")
            {
                deAddEquipment(sender, e);
            }
            else
            {
                deEditEquipment(sender, e);
            }
        }

        /// <summary>
        /// Add equipment to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddEquipment(object sender, DirectEventArgs e)
        {
            long headerId = long.Parse(Request.QueryString["headerId"]);
            long projectId = long.Parse(uxAddEquipmentDropDown.Value.ToString());
            long odStart;
            long odEnd;

            var icp = User as ClaimsPrincipal;
            var AddingUser = Authentication.GetClaimValue(ClaimTypes.Name, icp);

            DAILY_ACTIVITY_EQUIPMENT added = new DAILY_ACTIVITY_EQUIPMENT()
            {
                HEADER_ID = headerId,
                PROJECT_ID = projectId,
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = AddingUser,
                MODIFIED_BY = AddingUser
            };

            //Check for Odometer Start
            if (long.TryParse(uxAddEquipmentStart.Text, out odStart))
            {
                added.ODOMETER_START = odStart;
            }
            else
            {
                added.ODOMETER_START = null;
            }

            //Check for Odometer End
            if(long.TryParse(uxAddEquipmentEnd.Text, out odEnd))
            {
                odEnd = long.Parse(uxAddEquipmentEnd.Value.ToString());
                added.ODOMETER_END = odEnd;
            }
            else
            {
                added.ODOMETER_END = null;
            }

            //Write Data to DB
            GenericData.Insert<DAILY_ACTIVITY_EQUIPMENT>(added);

            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");

        }

        /// <summary>
        /// Load edit equipment form and populate fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadEditEquipmentForm()
        {
            long EquipmentId = long.Parse(Request.QueryString["EquipmentId"]);
            using (Entities _context = new Entities()){
                var Equipment = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                                 join p in _context.CLASS_CODES_V on d.PROJECT_ID equals p.PROJECT_ID
                                     where d.EQUIPMENT_ID == EquipmentId
                                     select new{d, p.NAME}).Single();
                uxAddEquipmentDropDown.SetValue(Equipment.d.PROJECT_ID.ToString(), Equipment.NAME);
                uxAddEquipmentStart.SetValue(Equipment.d.ODOMETER_START);
                uxAddEquipmentEnd.SetValue(Equipment.d.ODOMETER_END);
            }
        }

        /// <summary>
        /// Store edits to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditEquipment(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_EQUIPMENT data;

            //Get record to be edited
            using (Entities _context = new Entities())
            {
                long EquipmentId = long.Parse(Request.QueryString["EquipmentId"]);

                data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                        where d.EQUIPMENT_ID == EquipmentId
                        select d).Single();

                long ProjectId = long.Parse(uxAddEquipmentDropDown.Value.ToString());
                long odStart;
                long odEnd;

                //Check for odometer start
                if (long.TryParse(uxAddEquipmentStart.Text, out odStart))
                {
                    data.ODOMETER_START = odStart;
                }
                else
                {
                    data.ODOMETER_START = null;
                }

                //Check for Odometer End
                if (long.TryParse(uxAddEquipmentEnd.Text, out odEnd))
                {
                    odEnd = long.Parse(uxAddEquipmentEnd.Value.ToString());
                    data.ODOMETER_END = odEnd;
                }
                else
                {
                    data.ODOMETER_END = null;
                }

                //Update Entity
                data.PROJECT_ID = ProjectId;
                data.MODIFIED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;
            }

            //Save to DB
            GenericData.Update<DAILY_ACTIVITY_EQUIPMENT>(data);

            X.Js.Call("parent.App.uxDetailsPanel.reload(); parent.App.uxPlaceholderWindow.close()");
        }

          /// <summary>
        /// Update toggle button and reload store
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReloadStore(object sender, DirectEventArgs e)
        {
            uxEquipmentStore.Reload();
            if (uxAddEquipmentToggleOrg.Pressed)
            {
                uxAddEquipmentToggleOrg.Text = "My Region";
            }
            else
            {
                uxAddEquipmentToggleOrg.Text = "All Regions";
            }
        }

        /// <summary>
        /// Put selected grid value into drop-down field and clear filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
                //Set value and text for equipment
                uxAddEquipmentDropDown.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["EquipmentName"]);

                //Clear existing filters
                uxAddEquipmentFilter.ClearFilter();
        }

        
    }
}