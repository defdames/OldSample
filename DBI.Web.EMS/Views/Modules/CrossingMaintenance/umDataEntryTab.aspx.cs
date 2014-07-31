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
                CheckboxSelectionModel sm = CheckboxSelectionModel1;
                sm.ClearSelection();
            }
          
        }
        protected void deApplicationGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                if (validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
                {
                    long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));

                    List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                    IQueryable<CROSSING_MAINTENANCE.CrossingData> data = CROSSING_MAINTENANCE.GetCrossingList(RailroadId, _context).Where(v => v.PROJECT_TYPE == "CUSTOMER BILLING" && v.TEMPLATE_FLAG == "N" && v.PROJECT_STATUS_CODE == "APPROVED" && v.ORGANIZATION_NAME.Contains(" RR") && OrgsList.Contains(v.CARRYING_OUT_ORGANIZATION_ID));

                    int count;
                    uxAppEntryCrossingStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.CrossingData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }

        protected void deLoadData(object sender, DirectEventArgs e)
        {
            uxApplicationStore.Reload();
            uxAddAppButton.Enable();
        }

        protected void GetApplicationGridData(object sender, StoreReadDataEventArgs e)
        {

            //Get application data and set datasource
            CheckboxSelectionModel sm = CheckboxSelectionModel1;
           
            List<long> crossingIdList = new List<long>();
          
            foreach (SelectedRow sr in sm.SelectedRows)
            {
              crossingIdList.Add(long.Parse(sr.RecordID));
            }
                using (Entities _context = new Entities())
                {
                    IQueryable<CROSSING_MAINTENANCE.ApplicationList> data = CROSSING_MAINTENANCE.GetApplications(_context).Where(s => crossingIdList.Contains(s.CROSSING_ID));
                   
                    int count;
                    uxApplicationStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.ApplicationList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;               

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
            CheckboxSelectionModel sm = CheckboxSelectionModel1;

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

                foreach (SelectedRow sr in sm.SelectedRows)
                {   
                    //check for if application requested has been duplicated in the same fiscal year.

                    DateTime AppDate = uxAddEntryDate.SelectedDate;
                    DateTime Start = new DateTime(2013, 11, 1); //this pulls in november 1st of year for fiscal yr
                    DateTime End = new DateTime(2014, 10, 31); //this pulls in oct 31st of next year for fiscal yr
                    string app = (e.ExtraParams["appList"]);
                    List<AppNumber> appList = JSON.Deserialize<List<AppNumber>>(app);

                    List<object> checkApp;
                    using (Entities _context = new Entities())
                    {
                        checkApp = (from a in _context.CROSSING_APPLICATION
                                    where a.APPLICATION_REQUESTED == AppRequested
                                    select new { a.APPLICATION_REQUESTED, a.APPLICATION_ID }).ToList<object>();



                        if (AppDate >= Start.Date && AppDate <= End.Date && appList.Where(x => x.APPLICATION_REQUESTED == AppRequested).Count() > 0)
                        {

                            X.Msg.Alert("Warning", "Application exists for this fiscal year").Show();

                        }

                        else
                        {
                            data = new CROSSING_APPLICATION();
                            data.APPLICATION_DATE = Date;
                            data.APPLICATION_REQUESTED = AppRequested;
                            data.TRUCK_NUMBER = TruckNumber;
                            data.SPRAY = Spray;
                            data.CUT = Cut;
                            data.INSPECT = Inspect;
                            data.CROSSING_ID = long.Parse(sr.RecordID);
                            data.CREATE_DATE = DateTime.Now;
                            data.MODIFY_DATE = DateTime.Now;
                            data.CREATED_BY = User.Identity.Name;
                            data.MODIFIED_BY = User.Identity.Name;

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

                            uxAddNewApplicationEntryWindow.Hide();
                            uxApplicationStore.Reload();
                            uxAddApplicationForm.Reset();


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
            }
            
        
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Form"] == "Add")
            {
                //Set value and text for equipment
                uxAddEquipmentDropDown.SetValue(e.ExtraParams["EquipmentName"], e.ExtraParams["EquipmentName"]);

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
        public class AppNumber
        {
            public string APPLICATION_REQUESTED { get; set; }
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
        public class CrossingData
        {
            public long CROSSING_ID { get; set; }
            public string CROSSING_NUMBER { get; set; }
            public string SERVICE_UNIT { get; set; }
            public string SUB_DIVISION { get; set; }
            public string DOT { get; set; }
            public long? PROJECT_ID { get; set; }
            public string STATE { get; set; }
            public decimal? CONTACT_ID { get; set; }
            public string CONTACT_NAME { get; set; }
            public decimal? RAILROAD_ID { get; set; }
        }
    }
}
