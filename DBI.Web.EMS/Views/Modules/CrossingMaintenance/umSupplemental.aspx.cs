 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;
using DBI.Data.GMS;
using DBI.Data.DataFactory;
using System.Security.Claims;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umSupplemental : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {

                //uxAddServiceTypeStore.Data = StaticLists.ServiceTypes;
      
            }
        }

        protected void deSupplementalGridData(object sender, StoreReadDataEventArgs e)
        {

            
            long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
            //List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
            decimal UserId = SYS_USER_INFORMATION.UserID(User.Identity.Name);

            List<CROSSING_MAINTENANCE.CrossingData1> dataSource = CROSSING_MAINTENANCE.GetSuppCrossingList(RailroadId, UserId).ToList();

            List<object> _data = dataSource.ToList<object>();
            int count = 0;
            uxSupplementalCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
            e.Total = count;
        }
                   
        protected void GetSupplementalGridData(object sender, StoreReadDataEventArgs e)
        {
            //Get Supplemental data and set datasource
            string json = (e.Parameters["crossingId"]);
            List<CrossingForSupplementalDetails> crossingList = JSON.Deserialize<List<CrossingForSupplementalDetails>>(json);
            List<long> crossingIdList = new List<long>();
            foreach (CrossingForSupplementalDetails crossing in crossingList)
            {
                crossingIdList.Add(crossing.CROSSING_ID);

            }
            using (Entities _context = new Entities())
            {
                IQueryable<CROSSING_MAINTENANCE.SupplementalList> data = CROSSING_MAINTENANCE.GetSupplementals(_context).Where(s => crossingIdList.Contains(s.CROSSING_ID));

                int count;
                uxSupplementalStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.SupplementalList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
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
        //protected void deReloadStore(object sender, DirectEventArgs e)
        //{
        //    string type = e.ExtraParams["Type"];
        //    if (type == "Equipment")
        //    {
        //        uxEquipmentStore.Reload();
        //        if (uxAddEquipmentToggleOrg.Pressed)
        //        {
        //            uxAddEquipmentToggleOrg.Text = "My Region";
        //        }
        //        else
        //        {
        //            uxAddEquipmentToggleOrg.Text = "All Regions";
        //        }
        //    }
        //}
        protected void deAddSupplemental(object sender, DirectEventArgs e)
        {
            CROSSING_SUPPLEMENTAL data;

            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            //do type conversions
            DateTime ApprovedDate = (DateTime)uxAddApprovedDateField.Value;
            DateTime CutDate = (DateTime)uxAddCutDateField.Value;
            decimal SquareFeet = Convert.ToDecimal(uxAddSquareFeet.Value);
            string ServiceType = uxAddPricingGrid.Value.ToString();
            string Recurring = uxAddRecurringBox.Value.ToString();
            long ProjectName = Convert.ToInt64(uxAddProjectDropDownField.Value);


            if (uxAddRecurringBox.Checked)
            {
                Recurring = "Y";
            }
            else
            {
                Recurring = "N";
            }

            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new CROSSING_SUPPLEMENTAL()
                {

                    APPROVED_DATE = ApprovedDate,
                    SERVICE_TYPE = ServiceType,
                    SQUARE_FEET = SquareFeet,
                    RECURRING = Recurring,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name,
                    CROSSING_ID = CrossingId,
                    CUT_TIME = CutDate,
                    PROJECT_ID = ProjectName,
                };
            }
            try
            {
                string Remarks = uxAddRemarks.Value.ToString();
                data.REMARKS = Remarks;
            }
            catch (Exception)
            {
                data.REMARKS = null;
            }

            GenericData.Insert<CROSSING_SUPPLEMENTAL>(data);

            uxAddNewSupplementalWindow.Hide();
            uxAddSupplementalForm.Reset();
            uxSupplementalStore.Reload();
         


            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Supplemental Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }


        protected void deAddProjectValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "Add":
                    uxAddProjectDropDownField.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["ProjectName"]);
                    uxAddProjectFilter.ClearFilter();
                    break;

            }
        }
        protected void deAddProjectGrid(object sender, StoreReadDataEventArgs e)
        {

            long CrossingId = long.Parse(e.Parameters["CrossingId"]);
            //long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
           // List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
            using (Entities _context = new Entities())
            {
                List<CROSSING_MAINTENANCE.CrossingProject> data = CROSSING_MAINTENANCE.CrossingsProjectList(CrossingId);//.Where(v => v.PROJECT_TYPE == "CUSTOMER BILLING" && v.TEMPLATE_FLAG == "N" && v.PROJECT_STATUS_CODE == "APPROVED" && v.ORGANIZATION_NAME.Contains(" RR") && OrgsList.Contains(v.CARRYING_OUT_ORGANIZATION_ID) && RailroadId != null);

                int count;
                uxSupplementalProjectStore.DataSource = GenericData.EnumerableFilterHeader<CROSSING_MAINTENANCE.CrossingProject>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;

            }
        }
        protected void deAddPricingValue(object sender, DirectEventArgs e)
        {
            switch (e.ExtraParams["Type"])
            {
                case "Add":
                    uxAddPricingGrid.SetValue(e.ExtraParams["ServiceCategory"], e.ExtraParams["ServiceCategory"]);
                    uxAddPricingFilter.ClearFilter();
                    break;

            }
        }
        protected void deAddPricingGrid(object sender, StoreReadDataEventArgs e)
        {

            
       
            using (Entities _context = new Entities())
            {
                List<CROSSING_MAINTENANCE.CrossingPricing> data = CROSSING_MAINTENANCE.CrossingsPricingList();
                int count;
                uxSupplementalPricingStore.DataSource = GenericData.EnumerableFilterHeader<CROSSING_MAINTENANCE.CrossingPricing>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;

            }
        }
               
       
        public class CrossingForSupplementalDetails
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

        public class SupplementalDetails
        {
            public long SUPPLEMENTAL_ID { get; set; }
            public long CROSSING_ID { get; set; }
            public DateTime APPROVED_DATE { get; set; }
            public DateTime COMPLETED_DATE { get; set; }
            public string SERVICE_TYPE { get; set; }
            public string TRUCK_NUMBER { get; set; }
            public DateTime INSPECT_START { get; set; }
            public DateTime INSPECT_END { get; set; }
            public string SPRAY { get; set; }
            public long SQUARE_FEET { get; set; }
            public string CUT { get; set; }
            public string MAINTAIN { get; set; }
            public string INSPECT { get; set; }
            public string RECURRING { get; set; }
            public string REMARKS { get; set; }

        }

        protected void deRemoveSupplemental(object sender, DirectEventArgs e)
        {

            CROSSING_SUPPLEMENTAL data;
            string json = e.ExtraParams["SupplementalInfo"];
            //long SupplementalId = long.Parse(e.ExtraParams["SupplementalId"]);
            List<SupplementalDetails> SupplementalList = JSON.Deserialize<List<SupplementalDetails>>(json);
            foreach (SupplementalDetails Supplemental in SupplementalList)
            {
                using (Entities _context = new Entities())
                {
                    data = (from d in _context.CROSSING_SUPPLEMENTAL
                            where d.SUPPLEMENTAL_ID == Supplemental.SUPPLEMENTAL_ID
                            select d).Single();

                }
                GenericData.Delete<CROSSING_SUPPLEMENTAL>(data);

                uxSupplementalStore.Reload();

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Supplemental Removed Successfully",
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



        
    
