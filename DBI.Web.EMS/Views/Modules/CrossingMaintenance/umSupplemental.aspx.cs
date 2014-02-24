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
    public partial class umSupplemental : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                       
                            select new { s.CROSSING_ID, s.SUPPLEMENTAL_ID, s.APPROVED_DATE, s.COMPLETED_DATE, s.SERVICE_TYPE, s.INSPECT_START, s.INSPECT_END, s.TRUCK_NUMBER, s.SPRAY, s.CUT, s.INSPECT, s.MAINTAIN, s.RECURRING, s.REMARKS}).ToList<object>();
                          
                          
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
                DateTime CompletedDate = (DateTime)uxAddCompleteDateField.Value;
                string ServiceType = uxAddServiceType.Value.ToString();
                DateTime InspectionStart = (DateTime)uxAddInspectStartDateField.Value;
                DateTime InspectionEnd = (DateTime)uxAddInspectEndDateField.Value;
                long TruckNumber = Convert.ToInt64(uxAddTruck.Value);
                string Spray = uxAddSpray.Value.ToString();
                string Cut = uxAddCut.Value.ToString();
                string Maintain = uxAddMaintainBox.Value.ToString();
                string Inspect = uxAddInspect.Value.ToString();
                string Recurring = uxAddRecurringBox.Value.ToString();
                string Remarks = uxAddRemarks.Value.ToString();
                if (uxAddSpray.Checked)
                {
                    Spray = "Y";
                }
                else
                {
                    Spray = "N";
                }

                if (uxAddCut.Checked)
                {
                    Cut = "Y";
                }
                else
                {
                    Cut = "N";
                }
                if (uxAddMaintainBox.Checked)
                {
                    Maintain = "Y";
                }
                else
                {
                    Maintain = "N";
                }
                if (uxAddInspect.Checked)
                {
                    Inspect = "Y";
                }
                else
                {
                    Inspect = "N";
                }
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
                        COMPLETED_DATE = CompletedDate,
                        SERVICE_TYPE = ServiceType,
                        INSPECT_START = InspectionStart,
                        INSPECT_END = InspectionEnd,
                        TRUCK_NUMBER = TruckNumber,
                        SPRAY = Spray, 
                        CUT = Cut,
                        MAINTAIN = Maintain,
                        INSPECT = Inspect,
                        RECURRING = Recurring,
                        REMARKS = Remarks,
                        CROSSING_ID = CrossingId,
                    };
                }


                GenericData.Insert<CROSSING_SUPPLEMENTAL>(data);

                uxAddNewSupplementalWindow.Hide();
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
            protected void deEditSupplementalForm(object sender, DirectEventArgs e)
            {
                string json = e.ExtraParams["SupplementalInfo"];
                List<SupplementalDetails> SupplementalList = JSON.Deserialize<List<SupplementalDetails>>(json);
                foreach (SupplementalDetails Supplemental in SupplementalList)
                {
                    
                    uxEditApprovedDateField.SetValue(Supplemental.APPROVED_DATE);
                    uxEditCompletedDateField.SetValue(Supplemental.COMPLETED_DATE);
                    uxEditServiceType.SetValue(Supplemental.SERVICE_TYPE);
                    uxEditTruckNumber.SetValue(Supplemental.TRUCK_NUMBER);
                    uxEditInspectStartDateField.SetValue(Supplemental.INSPECT_START);
                    uxEditInspectEndDateField.SetValue(Supplemental.INSPECT_END);
                    uxEditSprayBox.SetValue(Supplemental.SPRAY);
                    uxEditCut.SetValue(Supplemental.CUT);
                    uxEditInspectBox.SetValue(Supplemental.MAINTAIN);
                    uxEditMaintain.SetValue(Supplemental.INSPECT);
                    uxEditRecurringBox.SetValue(Supplemental.RECURRING);
                    uxEditRemarks.SetValue(Supplemental.REMARKS);

                    if (Supplemental.SPRAY == "Y")
                    {
                        uxEditSprayBox.Checked = true;
                    }
                    if (Supplemental.CUT == "Y")
                    {
                        uxEditCut.Checked = true;
                    }
                    if (Supplemental.MAINTAIN == "Y")
                    {
                        uxEditMaintain.Checked = true;
                    }
                    if (Supplemental.INSPECT == "Y")
                    {
                        uxEditInspectBox.Checked = true;
                    }
                    if (Supplemental.RECURRING == "Y")
                    {
                        uxEditRecurringBox.Checked = true;
                    }
                }
            
        }
               
                   


            /// <summary>
            /// Store edit changes to database
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected void deEditSupplemental(object sender, DirectEventArgs e)
            {
                CROSSING_SUPPLEMENTAL data;

                //Do type conversions
                DateTime ApprovedDate = (DateTime)uxEditApprovedDateField.Value;
                DateTime CompletedDate = (DateTime)uxEditCompletedDateField.Value;
                string ServiceType = uxEditServiceType.Value.ToString();
                DateTime InspectionStart = (DateTime)uxEditInspectStartDateField.Value;
                DateTime InspectionEnd = (DateTime)uxEditInspectEndDateField.Value;
                long TruckNumber = Convert.ToInt64(uxEditTruckNumber.Value);
                string Spray = uxEditSprayBox.Value.ToString();
                string Cut = uxEditCut.Value.ToString();
                string Maintain = uxEditMaintain.Value.ToString();
                string Inspect = uxEditInspectBox.Value.ToString();
                string Recurring = uxEditRecurringBox.Value.ToString();
                string Remarks = uxEditRemarks.Value.ToString();
                if (uxEditSprayBox.Checked)
                {
                    Spray = "Y";
                }
                else
                {
                    Spray = "N";
                }

                if (uxEditCut.Checked)
                {
                    Cut = "Y";
                }
                else
                {
                    Cut = "N";
                }
                if (uxEditMaintain.Checked)
                {
                    Maintain = "Y";
                }
                else
                {
                    Maintain = "N";
                }
                if (uxEditInspectBox.Checked)
                {
                    Inspect = "Y";
                }
                else
                {
                    Inspect = "N";
                }
                if (uxEditRecurringBox.Checked)
                {
                    Recurring = "Y";
                }
                else
                {
                    Recurring = "N";
                }
     

                //Get record to be edited
                using (Entities _context = new Entities())
                {
                    var CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
                    data = (from d in _context.CROSSING_SUPPLEMENTAL
                                join c in _context.CROSSINGS on d.CROSSING_ID equals c.CROSSING_ID
                                where d.CROSSING_ID == CrossingId

                                select d).Single();
                    
                }
                
                        data.APPROVED_DATE = ApprovedDate;
                        data.COMPLETED_DATE = CompletedDate;
                        data.SERVICE_TYPE = ServiceType;
                        data.INSPECT_START = InspectionStart;
                        data.INSPECT_END = InspectionEnd;
                        data.TRUCK_NUMBER = TruckNumber;
                        data.SPRAY = Spray; 
                        data.CUT = Cut;
                        data.MAINTAIN = Maintain;
                        data.INSPECT = Inspect;
                        data.RECURRING = Recurring;
                        data.REMARKS = Remarks;
                        //data.CROSSING_ID = CrossingId;

                //Write to DB
                GenericData.Update<CROSSING_SUPPLEMENTAL>(data);

                uxEditSupplementalWindow.Hide();
               
                uxSupplementalStore.Reload();

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Supplemental Edited Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });
            }
            public class SupplementalDetails
            {
                public long SUPPLEMENTAL_ID { get; set; }
                public DateTime APPROVED_DATE { get; set; }
                public DateTime COMPLETED_DATE { get; set; }
                public string SERVICE_TYPE { get; set; }
                public Int64 TRUCK_NUMBER { get; set; }
                public DateTime INSPECT_START { get; set; }
                public DateTime INSPECT_END { get; set; }
                public string SPRAY { get; set; }
                public string CUT { get; set; }
                public string MAINTAIN { get; set; }
                public string INSPECT { get; set; }
                public string RECURRING { get; set; }
                public string REMARKS { get; set; }
               
            }
            //protected void deRemoveSupplemental(object sender, DirectEventArgs e)
            //{

            //    long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);
            //    CROSSING_SUPPLEMENTAL data;
            //    using (Entities _context = new Entities())
            //    {
            //        data = (from d in _context.CROSSING_SUPPLEMENTAL
            //                join c in _context.CROSSINGS on d.CROSSING_ID equals c.CROSSING_ID
            //                where d.CROSSING_ID == CrossingId
            //                select d).Single();
            //    }
            //    GenericData.Delete<CROSSING_SUPPLEMENTAL>(data);

            //    uxSupplementalStore.Reload();

            //    Notification.Show(new NotificationConfig()
            //    {
            //        Title = "Success",
            //        Html = "Supplemental Removed Successfully",
            //        HideDelay = 1000,
            //        AlignCfg = new NotificationAlignConfig
            //        {
            //            ElementAnchor = AnchorPoint.Center,
            //            TargetAnchor = AnchorPoint.Center
            //        }
            //    });
            //}

        }
    }
