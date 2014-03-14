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

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umSupplemental : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
              
                    uxAddServiceTypeStore.Data = StaticLists.ServiceTypes;
                    ReadInTruckNumber("Add");               
            }
        }
        protected void deSupplementalGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers

                data = (from d in _context.CROSSINGS
                        join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID into pn
                        from proj in pn.DefaultIfEmpty()
                        select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID, proj.LONG_NAME }).ToList<object>();


                int count;
                uxSupplementalCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void GetSupplementalGridData(object sender, DirectEventArgs e)
        {
            //Get Supplemental data and set datasource
            using (Entities _context = new Entities())
            {
                long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                var data = (from s in _context.CROSSING_SUPPLEMENTAL
                            join c in _context.CROSSINGS on s.CROSSING_ID equals c.CROSSING_ID
                            where s.CROSSING_ID == CrossingId

                            select new { s.CROSSING_ID, s.SUPPLEMENTAL_ID, s.APPROVED_DATE, s.COMPLETED_DATE, s.SERVICE_TYPE, s.INSPECT_START, s.INSPECT_END, s.SQUARE_FEET, s.TRUCK_NUMBER, s.SPRAY, s.CUT, s.INSPECT, s.MAINTAIN, s.RECURRING, s.REMARKS }).ToList<object>();


                uxSupplementalGrid.Store.Primary.DataSource = data;
                uxSupplementalGrid.Store.Primary.DataBind();


            }
        }
        protected void deAddSupplemental(object sender, DirectEventArgs e)
        {
            CROSSING_SUPPLEMENTAL data;

            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
           
            //do type conversions
            DateTime ApprovedDate = (DateTime)uxAddApprovedDateField.Value;
            decimal SquareFeet = Convert.ToDecimal(uxAddSquareFeet.Value);
            string TruckNumber = uxAddTruckComboBox.Value.ToString();
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
                    //COMPLETED_DATE = CompletedDate,
                    SERVICE_TYPE = ServiceType,
                    TRUCK_NUMBER = TruckNumber,
                    SQUARE_FEET = SquareFeet,
                    RECURRING = Recurring,
                   
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

        protected void ReadInTruckNumber(string truckType)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers

                data = (from p in _context.PROJECTS_V
                        where p.PROJECT_TYPE == "TRUCK & EQUIPMENT"
                       select new { p.PROJECT_ID, p.PROJECT_TYPE, p.NAME }).ToList<object>();


                if (truckType == "Add")
                {
                    uxAddTruckStore.DataSource = data;
                }
                
               
                
            }
        }
        
        
        public class SupplementalDetails
        {
            public long SUPPLEMENTAL_ID { get; set; }
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


        
    
