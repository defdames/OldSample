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


namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umDataEntryTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {
                uxAddAppRequestedStore.Data = StaticLists.ApplicationRequested;
                //ReadInTruckNumberForApplication("Add");

            }
          
        }
        protected void deApplicationGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                if (validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
                {
                    long RailroadId = long.Parse(Session["rrType"].ToString());

                    List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                    data = (from d in _context.CROSSINGS
                            join r in _context.CROSSING_RELATIONSHIP on d.CROSSING_ID equals r.CROSSING_ID
                            join p in _context.PROJECTS_V on r.PROJECT_ID equals p.PROJECT_ID

                            where p.PROJECT_TYPE == "CUSTOMER BILLING" && p.TEMPLATE_FLAG == "N" && p.PROJECT_STATUS_CODE == "APPROVED" && OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID) && d.RAILROAD_ID == RailroadId
                            select new { d.RAILROAD_ID, d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID, p.LONG_NAME }).ToList<object>();


                    int count;
                    uxAppEntryCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }
        protected void GetApplicationGridData(object sender, DirectEventArgs e)
        {
            //Get application data and set datasource
         
                List<object> data;
                            
                string json = (e.ExtraParams["crossingId"]);
                List<CrossingForApplicationDetails> crossingList = JSON.Deserialize<List<CrossingForApplicationDetails>>(json);
                List<long>crossingIdList = new List<long>();
                foreach (CrossingForApplicationDetails crossing in crossingList)
                {
                    crossingIdList.Add(crossing.CROSSING_ID);
                   
                }        
                        using (Entities _context = new Entities())
                        {

                            data = (from a in _context.CROSSING_APPLICATION
                                    join c in _context.CROSSINGS on a.CROSSING_ID equals c.CROSSING_ID
                                    where  crossingIdList.Contains(a.CROSSING_ID)
                                    select new { c.CROSSING_NUMBER, a.CROSSING_ID, a.APPLICATION_ID, a.APPLICATION_NUMBER, a.APPLICATION_REQUESTED, a.APPLICATION_DATE, a.TRUCK_NUMBER, a.SPRAY, a.CUT, a.INSPECT, a.REMARKS }).ToList<object>();


                            uxApplicationEntryGrid.Store.Primary.DataSource = data;
                            uxApplicationEntryGrid.Store.Primary.DataBind();

                        }
                  
                
            
        }
        protected void deReadGrid(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_EQUIPMENT_V> dataIn;

            if (e.Parameters["Form"] == "Add")
            {
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
                if (e.Parameters["Form"] == "Add")
                {
                    uxEquipmentStore.DataSource = data;
                }

            }
        }
        protected void deAddApplication(object sender, DirectEventArgs e)
        {
            CROSSING_APPLICATION data;

            //do type conversions
            DateTime Date = (DateTime)uxAddEntryDate.Value;
            string AppRequested = uxAddAppReqeusted.Value.ToString();
            string TruckNumber = uxAddEquipmentDropDown.Value.ToString();
            string Spray = uxAddEntrySprayBox.Value.ToString();
            string Cut = uxAddEntryCutBox.Value.ToString();
            string Inspect = uxAddEntryInspectBox.Value.ToString();

            if (uxAddEntrySprayBox.Checked)
            {
                Spray = "Y";
            }
            else
            {
                Spray = "N";
            }

            if (uxAddEntryCutBox.Checked)
            {
                Cut = "Y";
            }
            else
            {
                Cut = "N";
            }

            if (uxAddEntryInspectBox.Checked)
            {
                Inspect = "Y";
            }
            else
            {
                Inspect = "N";
            }

            string json = (e.ExtraParams["selectedCrossings"]);
            List<CrossingForApplicationDetails> crossingList = JSON.Deserialize<List<CrossingForApplicationDetails>>(json);
            foreach (CrossingForApplicationDetails crossing in crossingList)
            {
                //check for if application requested has been duplicated in the same fiscal year.
                //CROSSING_APPLICATION appdatedata;
                DateTime appDate = Date;
                DateTime Start = new DateTime(2014, 11, 1); //this pulls in november 1st of year for fiscal yr
                DateTime End = new DateTime(2015, 10, 31); //this pulls in oct 31st of next year for fiscal yr
                if (appDate >= Start && appDate <= End)
                {
                    using (Entities _context = new Entities())
                    {
                        data = (from a in _context.CROSSING_APPLICATION
                                where a.APPLICATION_REQUESTED == AppRequested && AppRequested.Contains(a.APPLICATION_REQUESTED)
                                select a).Single();
                    }
                X.Msg.Alert("test",  "Application already contains this number for this fiscal year").Show();
                }
                else
                {

                //Add to Db
                using (Entities _context = new Entities())
                {
                    data = new CROSSING_APPLICATION();

                    data.APPLICATION_DATE = Date;
                    data.APPLICATION_REQUESTED = AppRequested;
                    data.TRUCK_NUMBER = TruckNumber;
                    data.SPRAY = Spray;
                    data.CUT = Cut;
                    data.INSPECT = Inspect;
                    data.CROSSING_ID = crossing.CROSSING_ID;
                    try
                    {
                        string Remarks = uxAddEntryRemarks.Value.ToString();
                        data.REMARKS = Remarks;
                    }
                    catch (Exception)
                    {
                        data.REMARKS = null;
                    }
                    GenericData.Insert<CROSSING_APPLICATION>(data);
                }

                uxAddNewApplicationEntryWindow.Hide();
                uxApplicationStore.Reload();



                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Application Added Successfully",
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
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Form"] == "Add")
            {
                //Set value and text for equipment
                uxAddEquipmentDropDown.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["EquipmentName"]);

                //Clear existing filters
                uxAddEquipmentFilter.ClearFilter();
            }
        }
        protected void deReloadStore(object sender, DirectEventArgs e)
        {
            string type = e.ExtraParams["Type"];
            if (type == "Equipment")
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
        }
        
       
        public class ApplicationDetails
        {
            public long APPLICATION_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public Int64 APPLICATION_NUMBER { get; set; }
            public string APPLICATION_REQUESTED { get; set; }
            public DateTime APPLICATION_DATE { get; set; }
            public string TRUCK_NUMBER { get; set; }
            public long FISCAL_YEAR { get; set; }
            public string SPRAY { get; set; }
            public string CUT { get; set; }
            public string INSPECT { get; set; }
            public string REMARKS { get; set; }

        }
        //protected void ReadInTruckNumberForApplication(string truckType)
        //{

        //    using (Entities _context = new Entities())
        //    {
        //        List<object> data;

        //        //Get List of all new headers

        //        data = (from p in _context.PROJECTS_V
        //                where p.PROJECT_TYPE == "TRUCK & EQUIPMENT"
        //                select new { p.PROJECT_ID, p.PROJECT_TYPE, p.NAME }).ToList<object>();


        //        if (truckType == "Add")
        //        {
        //            uxAddApplicationTruckStore.DataSource = data;
        //        }

        //    }
        //}
        public class CrossingForApplicationDetails
        {
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string DOT { get; set; }
            public string MILE_POST { get; set; }
            public string STATE { get; set; }
            public string CONTACT_ID { get; set; }
        }
        protected void deRemoveApplicationEntry(object sender, DirectEventArgs e)
        {
            CROSSING_APPLICATION data;
            string json = e.ExtraParams["ApplicationInfo"];

            List<ApplicationDetails> ApplicationList = JSON.Deserialize<List<ApplicationDetails>>(json);
            foreach (ApplicationDetails Application in ApplicationList)
            {
                using (Entities _context = new Entities())
                {
                    data = (from d in _context.CROSSING_APPLICATION
                            where d.APPLICATION_ID == Application.APPLICATION_ID
                            select d).Single();


                    GenericData.Delete<CROSSING_APPLICATION>(data);
                    uxApplicationStore.Reload();
                }

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Application Removed Successfully",
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
}
