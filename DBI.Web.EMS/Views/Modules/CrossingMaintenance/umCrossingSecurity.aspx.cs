using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umCrossingSecurity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CrossingMaintenance.InformationView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }
        protected void deSecurityProjectGrid(object sender, StoreReadDataEventArgs e)
        {
            {
                if (validateComponentSecurity("SYS.CrossingMaintenance.InformationView"))
                {
                    long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                    List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                    using (Entities _context = new Entities())
                    {
                        IQueryable<CROSSING_MAINTENANCE.ProjectList> data = CROSSING_MAINTENANCE.GetCrossingSecurityProjectList(RailroadId, _context).Where(v => v.PROJECT_TYPE == "CUSTOMER BILLING" && v.TEMPLATE_FLAG == "N" && v.PROJECT_STATUS_CODE == "APPROVED" && v.ORGANIZATION_NAME.Contains(" RR") && OrgsList.Contains(v.CARRYING_OUT_ORGANIZATION_ID) && RailroadId != null);

                        int count;
                        uxCurrentSecurityProjectStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.ProjectList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                        e.Total = count;
                        
                    }
                 
                }
            }
        }
        protected void deReadSubDiv(object sender, StoreReadDataEventArgs e)
        {
         
             long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
             using (Entities _context = new Entities())
             {
                 IQueryable<CROSSING_MAINTENANCE.StateList> data = CROSSING_MAINTENANCE.GetState(RailroadId, _context);
               
                 int count;
                 uxSubDivStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.StateList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                 e.Total = count;
                        
             }
        }
        protected void deSecurityCrossingGridData(object sender, StoreReadDataEventArgs e)
        {
            
              //long RailroadId = long.Parse(Session["rrType"].ToString());
              long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
              long ProjectId = long.Parse(e.Parameters["ProjectId"]);
              string State = (e.Parameters["State"]);
              List<object> data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS
                        join i in _context.CROSSING_RAILROAD on d.RAILROAD_ID equals i.RAILROAD_ID
                        where !(from r in _context.CROSSING_RELATIONSHIP 
                                where d.CROSSING_ID == r.CROSSING_ID && r.PROJECT_ID == ProjectId
                                 select r.CROSSING_ID)
                                 .Contains(d.CROSSING_ID)
                                 where d.RAILROAD_ID == RailroadId && d.STATE == State
                     select new { d.CROSSING_ID, d.CROSSING_NUMBER, i.RAILROAD, d.SERVICE_UNIT, d.PROJECT_ID, d.SUB_DIVISION }).ToList<object>();
                uxCurrentSecurityCrossingStore.DataSource = data;
            //int count;
            //uxCurrentSecurityCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
          

              
            }
        }
        protected void GetCrossingsGridData(object sender, StoreReadDataEventArgs e)
        {
            
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                long ProjectId = long.Parse(e.Parameters["ProjectId"]);
                IQueryable<CROSSING_MAINTENANCE.CrossingAssignedList> data = CROSSING_MAINTENANCE.GetCrossingsAssigned(RailroadId, _context).Where(x => x.PROJECT_ID == ProjectId);

                int count;
                uxAssignedCrossingGrid.Store.Primary.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.CrossingAssignedList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                 e.Total = count;
             
            }
        }
      
        protected void deAssociateCrossings(object sender, DirectEventArgs e)
        {       
            long ProjectId = long.Parse(e.ExtraParams["projectId"]);

            string json = (e.ExtraParams["selectedCrossings"]);
            List<CrossingDetails> crossingList = JSON.Deserialize<List<CrossingDetails>>(json);
            foreach (CrossingDetails crossing in crossingList)
            {
                 CROSSING_RELATIONSHIP CrossingToAdd = new CROSSING_RELATIONSHIP
                 {
                  CROSSING_ID = crossing.CROSSING_ID,
                  PROJECT_ID = ProjectId,
                 };
            
                GenericData.Insert<CROSSING_RELATIONSHIP>(CrossingToAdd);
            }
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing to Project Updated Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
            uxAssignCrossingWindow.Close();
            //uxCurrentSecurityProjectStore.Reload();
            uxCurrentSecurityCrossingStore.RemoveAll();
            uxSubDivStore.Reload();
            uxAssignedCrossingStore.Reload();
            
           
        }
        protected void deLoadStore(object sender, DirectEventArgs e)
        {
            uxAssignedCrossingStore.Reload();
        }
        protected void deCloseAssignScreen(object sender, DirectEventArgs e)
        {
            uxAssignCrossingWindow.Close();
            uxSubDivStore.Reload();
            uxCurrentSecurityCrossingStore.RemoveAll();
        }
        protected void deRemoveCrossingFromProject(object sender, DirectEventArgs e)
        {

           
               long ProjectId = long.Parse(e.ExtraParams["projectId"]);
            string json = e.ExtraParams["CrossingsAssigned"];

            List<CrossingDetails> CrossingList = JSON.Deserialize<List<CrossingDetails>>(json);
            foreach (CrossingDetails crossing in CrossingList)
            {
                CROSSING_RELATIONSHIP Delete;
                        using (Entities _context = new Entities())
                        {
                            Delete = _context.CROSSING_RELATIONSHIP.Where(x => (x.CROSSING_ID == crossing.CROSSING_ID) && x.PROJECT_ID == ProjectId).First();
                        }
                        GenericData.Delete<CROSSING_RELATIONSHIP>(Delete);
                        uxAssignedCrossingStore.Reload();
                    }
               
               

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Crossing Removed From Project Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });
            }
        }
  
            public class CrossingDetails
            {
                public long CROSSING_ID { get; set; }
                public string CROSSING_NUMBER { get; set; }              
                public string SERVICE_UNIT { get; set; }
                public string SUB_DIVISION { get; set; }
                public string CONTACT_ID { get; set; }
            }
       
            public class ProjectDetails
            {
                public long PROJECT_ID { get; set; }
                public string SEGMENT1 { get; set; }              
                public string ORANGANIZATION_NAME { get; set; }
                public string LONG_NAME { get; set; }
                public long CROSSING_ID { get; set; }
            }
   
    }
         

