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
    public partial class umIncident : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }
        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                data = (from d in _context.CROSSINGS
                        select new { d.CONTACT_ID, d.CROSSING_ID, d.CROSSING_NUMBER, d.SERVICE_UNIT, d.SUB_DIVISION, d.CROSSING_CONTACTS.CONTACT_NAME, d.PROJECT_ID }).ToList<object>();


                int count;
                uxCurrentCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void GetIncidentGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {

                List<object> data;
                long CrossingId = long.Parse(e.Parameters["CrossingId"]);
                if (uxToggleClosed.Checked)
                {
                    data = (from i in _context.CROSSING_INCIDENT
                            join c in _context.CROSSINGS on i.CROSSING_ID equals c.CROSSING_ID
                            where i.CROSSING_ID == CrossingId

                            select new { i.CROSSING_ID, i.INCIDENT_ID, i.INCIDENT_NUMBER, i.DATE_REPORTED, i.DATE_CLOSED, i.SLOW_ORDER, i.REMARKS }).ToList<object>();


                }
                else
                {
                    data = (from i in _context.CROSSING_INCIDENT
                            join c in _context.CROSSINGS on i.CROSSING_ID equals c.CROSSING_ID
                            where i.CROSSING_ID == CrossingId && i.DATE_CLOSED == null

                            select new { i.CROSSING_ID, i.INCIDENT_ID, i.INCIDENT_NUMBER, i.DATE_REPORTED, i.DATE_CLOSED, i.SLOW_ORDER, i.REMARKS }).ToList<object>();

                }

                uxIncidentStore.DataSource = data;
             
            }
        }
        protected void deAddIncident(object sender, DirectEventArgs e)
        {
            CROSSING_INCIDENT data;

            long CrossingId = long.Parse(e.ExtraParams["CrossingId"]);

            //do type conversions
            long IncidentNumber = Convert.ToInt64(uxIncidentNumber.Value);
            DateTime DateReported = (DateTime)uxIncidentDateReported.Value;

            string SlowOrder = uxIncidentSlowOrder.Value.ToString();

            if (uxIncidentSlowOrder.Checked)
            {
                SlowOrder = "Y";
            }
            else
            {
                SlowOrder = "N";
            }

            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new CROSSING_INCIDENT()
                {

                    INCIDENT_NUMBER = IncidentNumber,
                    DATE_REPORTED = DateReported,
                    SLOW_ORDER = SlowOrder,
                    CROSSING_ID = CrossingId,
                };
            }
        
            try
            {
                string Remarks = uxIncidentRemarks.Value.ToString();
                data.REMARKS = Remarks;
            }
            catch (Exception)
            {
                data.REMARKS = null;
            }

            GenericData.Insert<CROSSING_INCIDENT>(data);

            uxIncidentWindow.Hide();
            uxIncidentFormPanel.Reset();
            uxIncidentStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Incident Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deCloseIncident(object sender, DirectEventArgs e)
        {
            CROSSING_INCIDENT data;
            string json = e.ExtraParams["IncidentInfo"];

            List<IncidentDetails> IncidentList = JSON.Deserialize<List<IncidentDetails>>(json);
            foreach (IncidentDetails incident in IncidentList)
            {
                using (Entities _context = new Entities())
                {
                    data = (from d in _context.CROSSING_INCIDENT
                            where d.INCIDENT_ID == incident.INCIDENT_ID
                            select d).Single();

                }
      
                uxCloseIncidentNum.SetValue(data.INCIDENT_NUMBER);
                uxCloseDateReported.SetValue(data.DATE_REPORTED);
                uxCloseSlowOrder.SetValue(data.SLOW_ORDER);
                uxCloseRemarks.SetValue(data.REMARKS);
                if (data.SLOW_ORDER == "Y")
                {
                    uxCloseSlowOrder.Checked = true;
                }
               

            }
        }
               protected void deCloseIncidentForm(object sender, DirectEventArgs e)
        {
                   CROSSING_INCIDENT data;
                   DateTime DateClosed = (DateTime)uxCloseIncidentDateClosed.Value;
             string json = e.ExtraParams["IncidentInfo"];

            List<IncidentDetails> IncidentList = JSON.Deserialize<List<IncidentDetails>>(json);
            foreach (IncidentDetails incident in IncidentList)
            {
                using (Entities _context = new Entities())
                {
                    data = (from d in _context.CROSSING_INCIDENT
                            where d.INCIDENT_ID == incident.INCIDENT_ID
                            select d).Single();

                }
            
                data.DATE_CLOSED = DateClosed;
                try
            {
                string Remarks = uxCloseRemarks.Value.ToString();
                data.REMARKS = Remarks;
            }
            catch (Exception)
            {
                data.REMARKS = null;
            }
              GenericData.Update<CROSSING_INCIDENT>(data);

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Incident Closed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
          
            uxCloseIncidentWindow.Hide();
            uxIncidentStore.Reload();
            uxToggleClosed.Reset();
            }}
        
              public class IncidentDetails
        {
            public long INCIDENT_ID { get; set; }
            public DateTime DATE_CLOSED { get; set; }
            public string REMARKS {get; set; }
            public long INCIDENT_NUMBER { get; set; }
            public DateTime DATE_REPORTED { get; set; }
            public string SLOW_ORDER { get; set; }

        }
            
            }
        }
    

