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
    public partial class umRailRoadProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //protected void deSecurityProjectGrid(object sender, StoreReadDataEventArgs e)
        //{
        //    {
        //        List<WEB_PROJECTS_V> dataIn;

        //        dataIn = WEB_PROJECTS_V.ProjectList();

        //        int count;
        //        //Get paged, filterable list of data
        //        List<WEB_PROJECTS_V> data = GenericData.EnumerableFilterHeader<WEB_PROJECTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

        //        e.Total = count;
        //        uxCurrentSecurityProjectStore.DataSource = data;
        //        uxCurrentSecurityProjectStore.DataBind();
        //    }
        //}
        protected void deSecurityProjectGrid(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long RailRoadId = long.Parse(e.Parameters["RailroadId"]);
                var data = (from v in _context.PROJECTS_V
                            where !(from p in _context.CROSSING_PROJECT
                                    where v.PROJECT_ID == p.PROJECT_ID && p.RAILROAD_ID == RailRoadId
                                    select p.PROJECT_ID)
                                .Contains(v.PROJECT_ID)
                            where v.PROJECT_TYPE == "CUSTOMER BILLING" && v.TEMPLATE_FLAG == "N" && v.PROJECT_STATUS_CODE == "APPROVED"
                            select new { v.PROJECT_ID, v.LONG_NAME, v.ORGANIZATION_NAME, v.SEGMENT1 }).ToList<object>();


                uxProjectGrid.Store.Primary.DataSource = data;
            }
               
        }
        protected void deReadRailRoad(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<CROSSING_RAILROAD> RRData = _context.CROSSING_RAILROAD.ToList();


                uxRailRoadStore.DataSource = RRData;         
              
            }
            
        }
        protected void deSaveRailRoad(object sender, DirectEventArgs e)
        {
            ChangeRecords<AddRailRoad> data = new StoreDataHandler(e.ExtraParams["rrdata"]).BatchObjectData<AddRailRoad>();
            foreach (AddRailRoad CreatedRailRoad in data.Created)
            {
                CROSSING_RAILROAD NewRailRoad = new CROSSING_RAILROAD();
                NewRailRoad.RAILROAD = CreatedRailRoad.RAILROAD;

                GenericData.Insert<CROSSING_RAILROAD>(NewRailRoad);
                uxRailRoadStore.Reload();
            }
        }
        protected void deAssociateProject(object sender, DirectEventArgs e)
        {
            long RailroadId = long.Parse(e.ExtraParams["RailroadId"]);

            string json = (e.ExtraParams["selectedProjects"]);
            List<ProjectDetails> projectList = JSON.Deserialize<List<ProjectDetails>>(json);
            foreach (ProjectDetails project in projectList)
            {
                CROSSING_PROJECT ProjectToAdd = new CROSSING_PROJECT
                {
                    PROJECT_ID = project.PROJECT_ID,
                    RAILROAD_ID = RailroadId,
                };

                GenericData.Insert<CROSSING_PROJECT>(ProjectToAdd);
                uxCurrentSecurityProjectStore.Reload();
                Store2.Reload();
            }
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Project to Railroad Updated Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
            protected void GetProjectsGridData(object sender, StoreReadDataEventArgs e)
        {
            
            using (Entities _context = new Entities())
            {
                long RailroadId = long.Parse(e.Parameters["RailroadId"]);
                var data = (from r in _context.CROSSING_PROJECT
                            join p in _context.CROSSING_RAILROAD on r.RAILROAD_ID equals p.RAILROAD_ID
                            join v in _context.PROJECTS_V on r.PROJECT_ID equals v.PROJECT_ID
                            where r.RAILROAD_ID == RailroadId 

                            select new { r.RAILROAD_ID, r.PROJECT_ID, v.LONG_NAME, v.ORGANIZATION_NAME, v.SEGMENT1}).ToList<object>();


                uxAssignedProjectGrid.Store.Primary.DataSource = data;
               


            }
        }
          protected void deRemoveProjectFromRailroad(object sender, DirectEventArgs e)
        {


            long RailroadId = long.Parse(e.ExtraParams["RailroadId"]);
            string json = e.ExtraParams["ProjectsAssigned"];

            List<Project> ProjectList = JSON.Deserialize<List<Project>>(json);
            foreach (Project projects in ProjectList)
            {
                CROSSING_PROJECT Delete;
                        using (Entities _context = new Entities())
                        {
                            Delete = _context.CROSSING_PROJECT.Where(x => (x.PROJECT_ID == projects.PROJECT_ID) && x.RAILROAD_ID == RailroadId).First();
                        }
                        GenericData.Delete<CROSSING_PROJECT>(Delete);
                    }
                uxCurrentSecurityProjectStore.Reload();
                Store2.Reload();

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Project Removed From Railroad Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });
            }
        
            protected void deLoadStores(object sender, DirectEventArgs e)
            {
                Store2.Reload();
                uxCurrentSecurityProjectStore.Reload();
            }


        }
        public class AddRailRoad
        {
            public int RAILROAD_ID { get; set; }
            public string RAILROAD { get; set; }
        }
        public class Project
        {
            public int PROJECT_ID { get; set; }
            public int RAILROAD_ID { get; set; }
            public string LONG_NAME { get; set; }
            public string SEGMENT1 { get; set; }
            public string ORGANIZATION_NAME { get; set; }
        }
    }
