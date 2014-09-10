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
            if (!validateComponentSecurity("SYS.CrossingMaintenance.DataEntryView") && !validateComponentSecurity("SYS.DailyActivity.IncidentView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }
        protected void deCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                //List<object> data;
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                IQueryable<CROSSING_MAINTENANCE.CrossingList> data = CROSSING_MAINTENANCE.GetCrossingProjectListIncidents(RailroadId, _context);
              
                int count;
                uxCurrentCrossingStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.CrossingList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
              
            }
        }
        protected void GetIncidentGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                //long CrossingId = long.Parse(e.Parameters["CrossingId"]);
                IQueryable<CROSSING_MAINTENANCE.IncidentList> data;
                if (uxToggleClosed.Checked)
                {
                    data = CROSSING_MAINTENANCE.GetIncidents(_context);
                }
                else
                {
                    data = CROSSING_MAINTENANCE.GetIncidents(_context).Where(i => i.DATE_CLOSED == null);
                }
         
                int count;
                uxIncidentStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.IncidentList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
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
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    CREATED_BY = User.Identity.Name,
                    MODIFIED_BY = User.Identity.Name,
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
    

