﻿using System;
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
    public partial class umCrossingSecurity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deSecurityProjectGrid(object sender, StoreReadDataEventArgs e)
        {
            {
            List<WEB_PROJECTS_V> dataIn;
           
                dataIn = WEB_PROJECTS_V.ProjectList();
            
            int count;
            //Get paged, filterable list of data
            List<WEB_PROJECTS_V> data = GenericData.EnumerableFilterHeader<WEB_PROJECTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxCurrentSecurityProjectStore.DataSource = data;
            uxCurrentSecurityProjectStore.DataBind();
            }
        }
     
        protected void deSecurityCrossingGridData(object sender, DirectEventArgs e)
        {
              long ProjectId = long.Parse(e.ExtraParams["ProjectId"]);
              List<object> data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSINGS
                        where !(from r in _context.CROSSING_RELATIONSHIP 
                                where d.CROSSING_ID == r.CROSSING_ID && r.PROJECT_ID == ProjectId
                                 select r.CROSSING_ID)
                                 .Contains(d.CROSSING_ID)
                     select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.RAILROAD, d.SERVICE_UNIT, d.PROJECT_ID, d.SUB_DIVISION }).ToList<object>();
            uxCurrentSecurityCrossingStore.DataSource = data;
            uxCurrentSecurityCrossingStore.DataBind();

              
            }
        }
        protected void GetCrossingsGridData(object sender, DirectEventArgs e)
        {
            
            using (Entities _context = new Entities())
            {
                long ProjectId = long.Parse(e.ExtraParams["ProjectId"]);
                var data = (from r in _context.CROSSING_RELATIONSHIP
                            join d in _context.CROSSINGS on r.CROSSING_ID equals d.CROSSING_ID
                            where r.PROJECT_ID == ProjectId 

                            select new { r.CROSSING_ID, r.PROJECT_ID, d.CROSSING_NUMBER, d.RAILROAD, d.SERVICE_UNIT, d.SUB_DIVISION }).ToList<object>();


                uxAssignedCrossingGrid.Store.Primary.DataSource = data;
                uxAssignedCrossingGrid.Store.Primary.DataBind();


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
            uxCurrentSecurityProjectStore.Reload();
            uxCurrentSecurityCrossingStore.Reload();
            uxAssignedCrossingStore.Reload();
          
          
           
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
                    }
                uxCurrentSecurityProjectStore.Reload();
                uxAssignedCrossingStore.Reload();

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
                public string CROSSING_ID { get; set; }
            }
         public class GetSelectedCrossings
         {
             public long? PROJECT_ID { get; set; }
             public long? CROSSING_ID { get; set; }
             public decimal RELATIONSHIP_ID { get; set; }
             public string CROSSING_NUMBER { get; set; }
             public string SERVICE_UNIT { get; set; }
             public string SUB_DIVISION { get; set; }
             public long? CONTACT_ID { get; set; }
         }

    }
         

