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
    public partial class InspectionEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {

                ReadInTruckNumberForInspection("Add");
                ReadInTruckNumberForInspection("Edit");
            }
        }
        protected void deInspectionGridData(object sender, StoreReadDataEventArgs e)
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
                uxInspectionEntryCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void GetInspectionGridData(object sender, DirectEventArgs e)
        {
            //Get Supplemental data and set datasource
            using (Entities _context = new Entities())
            {
                long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                var data = (from s in _context.CROSSING_INSPECTION
                            join c in _context.CROSSINGS on s.CROSSING_ID equals c.CROSSING_ID
                            where s.CROSSING_ID == CrossingId

                            select new { s.CROSSING_ID, s.INSPECTION_ID, s.INSPECTION_NUMBER, s.INSPECTION_DATE, s.TRUCK_NUMBER, s.SPRAY, s.CUT, s.INSPECT, s.REMARKS }).ToList<object>();


                uxInspectionEntryGrid.Store.Primary.DataSource = data;
                uxInspectionEntryGrid.Store.Primary.DataBind();


            }
        }
        protected void deAddInspection(object sender, DirectEventArgs e)
        {
            CROSSING_INSPECTION data;

            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            //do type conversions


            long InspectionNumber = Convert.ToInt64(uxAddInspectEntryNumber.Value);
            DateTime Date = (DateTime)uxAddInspectEntryDate.Value;
            string TruckNumber = uxAddInspectionTruckComboBox.Value.ToString();
            string Spray = uxAddInspectEntrySprayBox.Value.ToString();
            string Cut = uxAddInspectEntryCutBox.Value.ToString();
            string Inspect = uxAddInspectEntryInspectBox.Value.ToString();
            string Remarks = uxAddInspectEntryRemarks.Value.ToString();
            if (uxAddInspectEntrySprayBox.Checked)
            {
                Spray = "Y";
            }
            else
            {
                Spray = "N";
            }

            if (uxAddInspectEntryCutBox.Checked)
            {
                Cut = "Y";
            }
            else
            {
                Cut = "N";
            }

            if (uxAddInspectEntryInspectBox.Checked)
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
                data = new CROSSING_INSPECTION()
                {
                    INSPECTION_NUMBER = InspectionNumber,
                    INSPECTION_DATE = Date,
                    //TRUCK_NUMBER = TruckNumber,
                    SPRAY = Spray,
                    CUT = Cut,
                    INSPECT = Inspect,
                    REMARKS = Remarks,
                    CROSSING_ID = CrossingId,
                };
            }


            GenericData.Insert<CROSSING_INSPECTION>(data);

            uxAddInspectionEntryWindow.Hide();
            uxAddInspectionForm.Reset();
            uxInspectionStore.Reload();


            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Inspection Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deEditInspectionForm(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["InspectionInfo"];
            List<InspectDetails> InspectionList = JSON.Deserialize<List<InspectDetails>>(json);
            foreach (InspectDetails Inspection in InspectionList)
            {


                uxEditInspectEntryNumber.SetValue(Inspection.INSPECTION_NUMBER);
                uxEditInspectionTruckNumber.SetValue(Inspection.TRUCK_NUMBER);
                uxEditInspectEntryDate.SetValue(Inspection.INSPECTION_DATE);
                uxEditInspectEntrySprayBox.SetValue(Inspection.SPRAY);
                uxEditInspectEntryCutBox.SetValue(Inspection.CUT);
                uxEditInspectEntryInspectBox.SetValue(Inspection.INSPECT);
                uxEditInspectEntryRemarks.SetValue(Inspection.REMARKS);

                if (Inspection.SPRAY == "Y")
                {
                    uxEditInspectEntrySprayBox.Checked = true;
                }
                if (Inspection.CUT == "Y")
                {
                    uxEditInspectEntryCutBox.Checked = true;
                }

                if (Inspection.INSPECT == "Y")
                {
                    uxEditInspectEntryInspectBox.Checked = true;
                }

            }

        }


        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditInspection(object sender, DirectEventArgs e)
        {
            CROSSING_INSPECTION data;

            //Do type conversions

            long InspectionNumber = Convert.ToInt64(uxEditInspectEntryNumber.Value);
            DateTime Date = (DateTime)uxEditInspectEntryDate.Value;
            string TruckNumber = uxEditInspectionTruckNumber.Value.ToString();
            string Spray = uxEditInspectEntrySprayBox.Value.ToString();
            string Cut = uxEditInspectEntryCutBox.Value.ToString();
            string Inspect = uxEditInspectEntryInspectBox.Value.ToString();
            string Remarks = uxEditInspectEntryRemarks.Value.ToString();
            if (uxEditInspectEntrySprayBox.Checked)
            {
                Spray = "Y";
            }
            else
            {
                Spray = "N";
            }

            if (uxEditInspectEntryCutBox.Checked)
            {
                Cut = "Y";
            }
            else
            {
                Cut = "N";
            }
            if (uxEditInspectEntryInspectBox.Checked)
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
                var InspectionId = long.Parse(e.ExtraParams["InspectionId"]);
                data = (from d in _context.CROSSING_INSPECTION
                        where d.INSPECTION_ID == InspectionId

                        select d).Single();

            }

            data.INSPECTION_DATE = Date;
            data.INSPECTION_NUMBER = InspectionNumber;
            //data.TRUCK_NUMBER = TruckNumber;
            data.SPRAY = Spray;
            data.CUT = Cut;
            data.INSPECT = Inspect;
            data.REMARKS = Remarks;
            //data.CROSSING_ID = CrossingId;

            //Write to DB
            GenericData.Update<CROSSING_INSPECTION>(data);

            uxEditInspectionEntryWindow.Hide();
            uxEditInspectionForm.Reset();
            uxInspectionStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Inspection Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deRemoveInspectionEntry(object sender, DirectEventArgs e)
        {
            CROSSING_INSPECTION data;
            string json = e.ExtraParams["InspectionInfo"];

            List<InspectDetails> InspectionList = JSON.Deserialize<List<InspectDetails>>(json);
            foreach (InspectDetails Inspection in InspectionList)
            {
                using (Entities _context = new Entities())
                {
                    data = (from d in _context.CROSSING_INSPECTION
                            where d.INSPECTION_ID == Inspection.INSPECTION_ID
                            select d).Single();

                }
                GenericData.Delete<CROSSING_INSPECTION>(data);

                uxInspectionStore.Reload();

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Inspection Removed Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });
            }
        }
        protected void ReadInTruckNumberForInspection(string truckType)
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
                    uxAddInspectionTruckStore.DataSource = data;
                }
                else
                {
                    uxEditInspectionTruckStore.DataSource = data;
                }



            }
        }
        
        public class InspectDetails
        {
            public long INSPECTION_ID { get; set; }
            public Int64 INSPECTION_NUMBER { get; set; }
            public DateTime INSPECTION_DATE { get; set; }
            public string TRUCK_NUMBER { get; set; }
            public string SPRAY { get; set; }
            public string CUT { get; set; }
            public string INSPECT { get; set; }
            public string REMARKS { get; set; }

        }
    }
}