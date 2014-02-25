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

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umDataEntryTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deApplicationGridData(object sender, StoreReadDataEventArgs e)
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
                uxAppEntryCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
           protected void GetApplicationGridData(object sender, DirectEventArgs e)
        {
            //Get Supplemental data and set datasource
            using (Entities _context = new Entities())
            {
                long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                var data = (from a in _context.CROSSING_APPLICATION
                            join c in _context.CROSSINGS on a.CROSSING_ID equals c.CROSSING_ID
                            where a.CROSSING_ID == CrossingId

                            select new { a.CROSSING_ID, a.APPLICATION_ID, a.APPLICATION_NUMBER, a.APPLICATION_DATE, a.TRUCK_NUMBER, a.SPRAY, a.CUT, a.INSPECT, a.REMARKS }).ToList<object>();


                uxApplicationEntryGrid.Store.Primary.DataSource = data;
                uxApplicationEntryGrid.Store.Primary.DataBind();


            }
        }
        protected void deAddApplication(object sender, DirectEventArgs e)
        {
            CROSSING_APPLICATION data;

            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            //do type conversions


            long ApplicationNumber = Convert.ToInt64(uxAddEntryNumber.Value);
            DateTime Date = (DateTime)uxAddEntryDate.Value;
            long TruckNumber = Convert.ToInt64(uxAddEntryTruckNum.Value);
            string Spray = uxAddEntrySprayBox.Value.ToString();
            string Cut = uxAddEntryCutBox.Value.ToString();
            string Inspect = uxAddEntryInspectBox.Value.ToString();
            string Remarks = uxAddEntryRemarks.Value.ToString();
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


            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new CROSSING_APPLICATION()
                {
                    APPLICATION_NUMBER = ApplicationNumber,
                    APPLICATION_DATE = Date,
                    TRUCK_NUMBER = TruckNumber,
                    SPRAY = Spray,
                    CUT = Cut,
                    INSPECT = Inspect,
                    REMARKS = Remarks,
                    CROSSING_ID = CrossingId,
                };
            }


            GenericData.Insert<CROSSING_APPLICATION>(data);

            uxAddNewApplicationEntryWindow.Hide();
            uxAddApplicationForm.Reset();
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
        protected void deEditApplicationForm(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["ApplicationInfo"];
            List<ApplicationDetails> ApplicationList = JSON.Deserialize<List<ApplicationDetails>>(json);
            foreach (ApplicationDetails Application in ApplicationList)
            {


                uxEditEntryNumber.SetValue(Application.APPLICATION_NUMBER);
                uxEditEntryTruckNum.SetValue(Application.TRUCK_NUMBER);
                uxEditEntryDate.SetValue(Application.APPLICATION_DATE);
                uxEditEntrySprayBox.SetValue(Application.SPRAY);
                uxEditEntryCutBox.SetValue(Application.CUT);
                uxEditEntryInspectBox.SetValue(Application.INSPECT);
                uxEditEntryRemarks.SetValue(Application.REMARKS);

                if (Application.SPRAY == "Y")
                {
                    uxEditEntrySprayBox.Checked = true;
                }
                if (Application.CUT == "Y")
                {
                    uxEditEntryCutBox.Checked = true;
                }

                if (Application.INSPECT == "Y")
                {
                    uxEditEntryInspectBox.Checked = true;
                }

            }

        }


        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditApplication(object sender, DirectEventArgs e)
        {
            CROSSING_APPLICATION data;

            //Do type conversions

            long ApplicationNumber = Convert.ToInt64(uxEditEntryNumber.Value);
            DateTime Date = (DateTime)uxEditEntryDate.Value;
            long TruckNumber = Convert.ToInt64(uxEditEntryTruckNum.Value);
            string Spray = uxEditEntrySprayBox.Value.ToString();
            string Cut = uxEditEntryCutBox.Value.ToString();
            string Inspect = uxEditEntryInspectBox.Value.ToString();
            string Remarks = uxEditEntryRemarks.Value.ToString();
            if (uxEditEntrySprayBox.Checked)
            {
                Spray = "Y";
            }
            else
            {
                Spray = "N";
            }

            if (uxEditEntryCutBox.Checked)
            {
                Cut = "Y";
            }
            else
            {
                Cut = "N";
            }
            if (uxEditEntryInspectBox.Checked)
            {
                Inspect = "Y";
            }
            else
            {
                Inspect = "N";
            }



            //Get record to be edited
            using (Entities _context = new Entities())
            {
                var CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                data = (from d in _context.CROSSING_APPLICATION
                        join c in _context.CROSSINGS on d.CROSSING_ID equals c.CROSSING_ID
                        where d.CROSSING_ID == CrossingId

                        select d).Single();

            }



            data.APPLICATION_DATE = Date;
            data.APPLICATION_NUMBER = ApplicationNumber;
            data.TRUCK_NUMBER = TruckNumber;
            data.SPRAY = Spray;
            data.CUT = Cut;
            data.INSPECT = Inspect;
            data.REMARKS = Remarks;
            //data.CROSSING_ID = CrossingId;

            //Write to DB
            GenericData.Update<CROSSING_APPLICATION>(data);

            uxEditApplicationEntryWindow.Hide();
            uxEditApplicationForm.Reset();
            uxApplicationStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Application Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        public class ApplicationDetails
        {
            public long APPLICATION_ID { get; set; }
            public Int64 APPLICATION_NUMBER { get; set; }
            public DateTime APPLICATION_DATE { get; set; }
            public Int64 TRUCK_NUMBER { get; set; }           
            public string SPRAY { get; set; }
            public string CUT { get; set; }        
            public string INSPECT { get; set; }
            public string REMARKS { get; set; }

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

                }
                GenericData.Delete<CROSSING_APPLICATION>(data);

                uxApplicationStore.Reload();

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
