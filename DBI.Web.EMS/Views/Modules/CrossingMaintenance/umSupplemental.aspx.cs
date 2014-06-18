﻿ using System;
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

                uxAddServiceTypeStore.Data = StaticLists.ServiceTypes;
                //ReadInTruckNumber("Add");
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
        protected void deSupplementalGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<CrossingData> data;
                if (validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
                {
                    long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                    List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                    data = (from d in _context.CROSSINGS
                            join r in _context.CROSSING_RELATIONSHIP on d.CROSSING_ID equals r.CROSSING_ID
                            join p in _context.PROJECTS_V on r.PROJECT_ID equals p.PROJECT_ID
                            where p.PROJECT_TYPE == "CUSTOMER BILLING" && p.TEMPLATE_FLAG == "N" && p.PROJECT_STATUS_CODE == "APPROVED" && OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID) && d.RAILROAD_ID == RailroadId
                            select new CrossingData { CONTACT_ID = d.CONTACT_ID,CROSSING_ID = d.CROSSING_ID, CROSSING_NUMBER = d.CROSSING_NUMBER, SERVICE_UNIT = d.SERVICE_UNIT, SUB_DIVISION = d.SUB_DIVISION, CONTACT_NAME = d.CROSSING_CONTACTS.CONTACT_NAME }).Distinct().ToList();


                    int count;
                    uxSupplementalCrossingStore.DataSource = GenericData.EnumerableFilterHeader<CrossingData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }
        //protected void GetSupplementalGridData(object sender, DirectEventArgs e)
        //{
        //    //Get Supplemental data and set datasource
        //    List<object> data;

        //    string json = (e.ExtraParams["crossingId"]);
        //    List<CrossingForSupplementalDetails> crossingList = JSON.Deserialize<List<CrossingForSupplementalDetails>>(json);
        //    List<long> crossingIdList = new List<long>();
        //    foreach (CrossingForSupplementalDetails crossing in crossingList)
        //    {
        //        crossingIdList.Add(crossing.CROSSING_ID);

        //    }        
        //    using (Entities _context = new Entities())
        //    {
        //        //long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
        //        data = (from s in _context.CROSSING_SUPPLEMENTAL
        //                    join c in _context.CROSSINGS on s.CROSSING_ID equals c.CROSSING_ID
        //                    //where s.CROSSING_ID == CrossingId
        //                    where crossingIdList.Contains(s.CROSSING_ID)
        //                    select new {c.CROSSING_NUMBER, s.CROSSING_ID,s.SUPPLEMENTAL_ID, s.APPROVED_DATE, s.COMPLETED_DATE, s.SERVICE_TYPE, s.INSPECT_START, s.INSPECT_END, s.SQUARE_FEET, s.TRUCK_NUMBER, s.SPRAY, s.CUT, s.INSPECT, s.MAINTAIN, s.RECURRING, s.REMARKS }).ToList<object>();


        //        uxSupplementalGrid.Store.Primary.DataSource = data;
        //        uxSupplementalGrid.Store.Primary.DataBind();


        //    }
        //}
        protected void GetSupplementalGridData(object sender, StoreReadDataEventArgs e)
        {
            //Get Supplemental data and set datasource
            List<object> data;

            string json = (e.Parameters["crossingId"]);
            List<CrossingForSupplementalDetails> crossingList = JSON.Deserialize<List<CrossingForSupplementalDetails>>(json);
            List<long> crossingIdList = new List<long>();
            foreach (CrossingForSupplementalDetails crossing in crossingList)
            {
                crossingIdList.Add(crossing.CROSSING_ID);

            }
            using (Entities _context = new Entities())
            {
                //long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                data = (from s in _context.CROSSING_SUPPLEMENTAL
                        join c in _context.CROSSINGS on s.CROSSING_ID equals c.CROSSING_ID
                        //where s.CROSSING_ID == CrossingId
                        where crossingIdList.Contains(s.CROSSING_ID)
                        select new { c.CROSSING_NUMBER, s.CROSSING_ID, s.SUPPLEMENTAL_ID, s.APPROVED_DATE, s.COMPLETED_DATE, s.SERVICE_TYPE, s.INSPECT_START, s.INSPECT_END, s.SQUARE_FEET, s.TRUCK_NUMBER, s.SPRAY, s.CUT, s.INSPECT, s.MAINTAIN, s.RECURRING, s.REMARKS }).ToList<object>();


                int count;
                uxSupplementalStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
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
        protected void deAddSupplemental(object sender, DirectEventArgs e)
        {
            CROSSING_SUPPLEMENTAL data;

            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            //do type conversions
            DateTime ApprovedDate = (DateTime)uxAddApprovedDateField.Value;
            decimal SquareFeet = Convert.ToDecimal(uxAddSquareFeet.Value);
            string TruckNumber = uxAddEquipmentDropDown.Value.ToString();
            string ServiceType = uxAddServiceType.Value.ToString();
            string Recurring = uxAddRecurringBox.Value.ToString();

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
                    TRUCK_NUMBER = TruckNumber,
                    SQUARE_FEET = SquareFeet,
                    RECURRING = Recurring,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name,
                    CROSSING_ID = CrossingId,
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
        //protected void deEditSupplementalForm(object sender, DirectEventArgs e)
        //{
        //    string json = e.ExtraParams["SupplementalInfo"];
        //    List<SupplementalDetails> SupplementalList = JSON.Deserialize<List<SupplementalDetails>>(json);
        //    foreach (SupplementalDetails Supplemental in SupplementalList)
        //    {

        //        uxEditApprovedDateField.SetValue(Supplemental.APPROVED_DATE);
        //        uxEditSquareFeet.SetValue(Supplemental.SQUARE_FEET);
        //        uxEditServiceTypes.SetValue(Supplemental.SERVICE_TYPE);
        //        uxEditTruckNumber.SetValue(Supplemental.TRUCK_NUMBER);
        //        uxEditRecurringBox.SetValue(Supplemental.RECURRING);
        //        uxEditRemarks.SetValue(Supplemental.REMARKS);


        //        if (Supplemental.RECURRING == "Y")
        //        {
        //            uxEditRecurringBox.Checked = true;
        //        }
        //    }

        //}





        //protected void deEditSupplemental(object sender, DirectEventArgs e)
        //{
        //    CROSSING_SUPPLEMENTAL data;

        //    //Do type conversions
        //    DateTime ApprovedDate = (DateTime)uxEditApprovedDateField.Value;
        //    decimal SquareFeet = Convert.ToDecimal(uxEditSquareFeet.Value);
        //    string ServiceType = uxEditServiceTypes.Value.ToString();
        //    string TruckNumber = uxEditTruckNumber.Value.ToString();
        //    string Recurring = uxEditRecurringBox.Value.ToString();
        //    string Remarks = uxEditRemarks.Value.ToString();

        //    if (uxEditRecurringBox.Checked)
        //    {
        //        Recurring = "Y";
        //    }
        //    else
        //    {
        //        Recurring = "N";
        //    }


        //    //Get record to be edited
        //    using (Entities _context = new Entities())
        //    {
        //        var SupplementalId = long.Parse(e.ExtraParams["SupplementalId"]);
        //        data = (from d in _context.CROSSING_SUPPLEMENTAL                   
        //                where d.SUPPLEMENTAL_ID == SupplementalId

        //                select d).Single();
        //    }

        //            data.APPROVED_DATE = ApprovedDate;
        //            //data.COMPLETED_DATE = CompletedDate;
        //            data.SQUARE_FEET = SquareFeet;
        //            data.TRUCK_NUMBER = TruckNumber;
        //            data.SERVICE_TYPE = ServiceType;
        //            data.RECURRING = Recurring;
        //            data.REMARKS = Remarks;

        //        //Write to DB
        //        GenericData.Update<CROSSING_SUPPLEMENTAL>(data);


        //        uxEditSupplementalWindow.Hide();
        //        uxEditSupplementalForm.Reset();
        //        uxSupplementalStore.Reload();

        //        Notification.Show(new NotificationConfig()
        //        {
        //            Title = "Success",
        //            Html = "Supplemental Edited Successfully",
        //            HideDelay = 1000,
        //            AlignCfg = new NotificationAlignConfig
        //            {
        //                ElementAnchor = AnchorPoint.Center,
        //                TargetAnchor = AnchorPoint.Center
        //            }
        //        });
        //    }
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
        //protected void ReadInTruckNumber(string truckType)
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
        //            uxEquipmentStoreDataSource = data;
        //        }



        //    }
        //}
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



        
    
