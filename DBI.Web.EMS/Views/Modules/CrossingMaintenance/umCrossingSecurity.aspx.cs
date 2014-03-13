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
        protected void deSecurityCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers

                data = (from d in _context.CROSSINGS
                        join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID into pn
                        from projects in pn.DefaultIfEmpty()
                        select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.RAILROAD, d.SERVICE_UNIT, d.PROJECT_ID, d.SUB_DIVISION, projects.LONG_NAME}).ToList<object>();
                

                int count;
                uxCurrentSecurityCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
        protected void deAssociateCrossings(object sender, DirectEventArgs e)
            {
                CROSSING data;

                //do type conversions

                RowSelectionModel project = uxProjectGrid.GetSelectionModel() as RowSelectionModel;
                long ProjectId = long.Parse(e.ExtraParams["projectId"]);
                             
                string json = (e.ExtraParams["selectedCrossings"]);
                List<ProjectDetails> projectList = JSON.Deserialize<List<ProjectDetails>>(json);
                foreach (ProjectDetails crossing in projectList)
                {
                    //Get record to be edited
                    using (Entities _context = new Entities())
                    {
                        data = (from d in _context.CROSSINGS
                                where d.CROSSING_ID == crossing.CROSSING_ID
                                select d).Single();
                        data.PROJECT_ID = ProjectId;
                      
                    }
                    GenericData.Update<CROSSING>(data);
                }
             
                uxCurrentSecurityCrossingStore.Reload();
                uxCurrentSecurityProjectStore.Reload();
                
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
            }
       //protected void ButtonValidityCheck(object sender, DirectEventArgs e)
            //{
                
            //    if ()
            //    {
            //        uxApplyButtonCS.Enable();
            //    }
            //    else
            //    {
            //        uxApplyButtonCS.Disabled();
            //    }
            //}
        public class ProjectDetails
            {
                public long CROSSING_ID { get; set; }
                public string CROSSING_NUMBER { get; set; }              
                public string SERVICE_UNIT { get; set; }
                public string SUB_DIVISION { get; set; }
                public string CONTACT_ID { get; set; }
            }
    }
         
}
