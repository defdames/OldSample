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
    public partial class umDataEntryTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxAddAppRequestedStore.Data = StaticLists.ApplicationRequested;
                ReadInTruckNumberForApplication("Add");
              
            }
        }
        protected void deApplicationGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                data = (from d in _context.CROSSINGS
                        join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID into pn
                        from proj in pn.DefaultIfEmpty()
                        select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID, proj.LONG_NAME }).ToList<object>();


                int count;
                uxAppEntryCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        //   protected void GetApplicationGridData(object sender, DirectEventArgs e)
        //{
        //    //Get Supplemental data and set datasource
        //    using (Entities _context = new Entities())
        //    {
        //        long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
        //        var data = (from a in _context.CROSSING_APPLICATION
        //                    join c in _context.CROSSINGS on a.CROSSING_ID equals c.CROSSING_ID
        //                    where a.CROSSING_ID == CrossingId

        //                    select new { a.CROSSING_ID, a.APPLICATION_ID, a.APPLICATION_NUMBER, a.APPLICATION_REQUESTED, a.APPLICATION_DATE, a.TRUCK_NUMBER, a.SPRAY, a.CUT, a.INSPECT, a.REMARKS }).ToList<object>();


        //        uxApplicationEntryGrid.Store.Primary.DataSource = data;
        //        uxApplicationEntryGrid.Store.Primary.DataBind();


        //    }
        //}
        protected void deAddApplication(object sender, DirectEventArgs e)
        {
            CROSSING_APPLICATION data;

            //long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
            

            //do type conversions
            DateTime Date = (DateTime)uxAddEntryDate.Value;
            string AppRequested = uxAddAppReqeusted.Value.ToString();
            string TruckNumber = uxAddApplicationTruckComboBox.Value.ToString();
            string Spray = uxAddEntrySprayBox.Value.ToString();
            //DateTime FiscalYear = uxAddFiscalYear.Value.ToString();
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
               



                //Add to Db
                using (Entities _context = new Entities())
                {
                    data = new CROSSING_APPLICATION();

                        data.APPLICATION_DATE = Date;
                        data.APPLICATION_REQUESTED = AppRequested;
                        data.TRUCK_NUMBER = TruckNumber;
                        data.SPRAY = Spray;
                        //data.FISCAL_YEAR = FiscalYear;
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
                uxAddApplicationForm.Reset();
                uxAppEntryCrossingStore.Reload();



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
        //protected void deEditApplicationForm(object sender, DirectEventArgs e)
        //{
        //    string json = e.ExtraParams["ApplicationInfo"];
        //    List<ApplicationDetails> ApplicationList = JSON.Deserialize<List<ApplicationDetails>>(json);
        //    foreach (ApplicationDetails Application in ApplicationList)
        //    {


        //        uxEditEntryNumber.SetValue(Application.APPLICATION_NUMBER);
        //        uxEditApplicationTruckNumber.SetValue(Application.TRUCK_NUMBER);
        //        uxEditAppRequested.SetValue(Application.APPLICATION_REQUESTED);
        //        uxEditEntryDate.SetValue(Application.APPLICATION_DATE);
        //        uxEditEntrySprayBox.SetValue(Application.SPRAY);
        //        uxEditEntryCutBox.SetValue(Application.CUT);
        //        uxEditEntryInspectBox.SetValue(Application.INSPECT);
        //        uxEditEntryRemarks.SetValue(Application.REMARKS);

        //        if (Application.SPRAY == "Y")
        //        {
        //            uxEditEntrySprayBox.Checked = true;
        //        }
        //        if (Application.CUT == "Y")
        //        {
        //            uxEditEntryCutBox.Checked = true;
        //        }

        //        if (Application.INSPECT == "Y")
        //        {
        //            uxEditEntryInspectBox.Checked = true;
        //        }

        //    }

        //}


        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void deEditApplication(object sender, DirectEventArgs e)
        //{
        //    CROSSING_APPLICATION data;

        //    //Do type conversions

        //    long ApplicationNumber = Convert.ToInt64(uxEditEntryNumber.Value);
        //    DateTime Date = (DateTime)uxEditEntryDate.Value;
        //    string TruckNumber = uxEditApplicationTruckNumber.Value.ToString();
        //    string AppRequested = uxEditAppRequested.Value.ToString();
        //    string Spray = uxEditEntrySprayBox.Value.ToString();
        //    string Cut = uxEditEntryCutBox.Value.ToString();
        //    string Inspect = uxEditEntryInspectBox.Value.ToString();
        //    string Remarks = uxEditEntryRemarks.Value.ToString();
        //    if (uxEditEntrySprayBox.Checked)
        //    {
        //        Spray = "Y";
        //    }
        //    else
        //    {
        //        Spray = "N";
        //    }

        //    if (uxEditEntryCutBox.Checked)
        //    {
        //        Cut = "Y";
        //    }
        //    else
        //    {
        //        Cut = "N";
        //    }
        //    if (uxEditEntryInspectBox.Checked)
        //    {
        //        Inspect = "Y";
        //    }
        //    else
        //    {
        //        Inspect = "N";
        //    }



        //    //Get record to be edited
        //    using (Entities _context = new Entities())
        //    {
        //        var ApplicationId = long.Parse(e.ExtraParams["ApplicationId"]);
        //        data = (from d in _context.CROSSING_APPLICATION
        //                where d.APPLICATION_ID == ApplicationId

        //                select d).Single();

        //    }



        //    data.APPLICATION_DATE = Date;
        //    data.APPLICATION_NUMBER = ApplicationNumber;
        //    data.APPLICATION_REQUESTED = AppRequested;
        //    data.TRUCK_NUMBER = TruckNumber;
        //    data.SPRAY = Spray;
        //    data.CUT = Cut;
        //    data.INSPECT = Inspect;
        //    data.REMARKS = Remarks;
        //    //data.CROSSING_ID = CrossingId;

        //    //Write to DB
        //    GenericData.Update<CROSSING_APPLICATION>(data);

        //    uxEditApplicationEntryWindow.Hide();
        //    uxEditApplicationForm.Reset();
           

        //    Notification.Show(new NotificationConfig()
        //    {
        //        Title = "Success",
        //        Html = "Application Edited Successfully",
        //        HideDelay = 1000,
        //        AlignCfg = new NotificationAlignConfig
        //        {
        //            ElementAnchor = AnchorPoint.Center,
        //            TargetAnchor = AnchorPoint.Center
        //        }
        //    });
        //}
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
        protected void ReadInTruckNumberForApplication(string truckType)
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
                    uxAddApplicationTruckStore.DataSource = data;
                }
               
            }
        }
          public class CrossingForApplicationDetails
            {
                public long CROSSING_ID { get; set; }
                public string CROSSING_NUMBER { get; set; }              
                public string SERVICE_UNIT { get; set; }
                public string SUB_DIVISION { get; set; }
                public string DOT { get; set; }
                public string MILE_POST{ get; set; }
                public string STATE { get; set; }
                public string CONTACT_ID { get; set; }
            }
        //protected void deRemoveApplicationEntry(object sender, DirectEventArgs e)
        //{
        //      CROSSING_APPLICATION data;
        //    string json = e.ExtraParams["ApplicationInfo"];

        //    List<ApplicationDetails> ApplicationList = JSON.Deserialize<List<ApplicationDetails>>(json);
        //    foreach (ApplicationDetails Application in ApplicationList)
        //    {
        //        using (Entities _context = new Entities())
        //        {
        //            data = (from d in _context.CROSSING_APPLICATION
        //                    where d.APPLICATION_ID == Application.APPLICATION_ID
        //                    select d).Single();

        //        }
        //        GenericData.Delete<CROSSING_APPLICATION>(data);

              

        //        Notification.Show(new NotificationConfig()
        //        {
        //            Title = "Success",
        //            Html = "Application Removed Successfully",
        //            HideDelay = 1000,
        //            AlignCfg = new NotificationAlignConfig
        //            {
        //                ElementAnchor = AnchorPoint.Center,
        //                TargetAnchor = AnchorPoint.Center
        //            }
        //        });
         
        
        }
     
    }
