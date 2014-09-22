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
    public partial class ManageKCS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deReadServiceUnit(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<CROSSING_SERVICE_UNIT> SUData = _context.CROSSING_SERVICE_UNIT.ToList();

                uxServiceUnitStore.DataSource = SUData;

            }

        }
        protected void deSaveServiceUnit(object sender, DirectEventArgs e)
        {
            //long RailroadId = long.Parse(Session["rrType"].ToString());
            //CROSSING_SERVICE_UNIT rrId = new CROSSING_SERVICE_UNIT();
            //{
                
            //}
            ChangeRecords<AddServiceUnit> data = new StoreDataHandler(e.ExtraParams["sudata"]).BatchObjectData<AddServiceUnit>();
            foreach (AddServiceUnit CreatedServiceUnit in data.Created)
            {
                CROSSING_SERVICE_UNIT NewServiceUnit = new CROSSING_SERVICE_UNIT();
                NewServiceUnit.SERVICE_UNIT_NAME = CreatedServiceUnit.SERVICE_UNIT_NAME;
                NewServiceUnit.RAILROAD_ID = long.Parse(SYS_USER_PROFILE_OPTIONS.UserProfileOption("UserCrossingSelectedValue"));
                NewServiceUnit.CREATE_DATE = DateTime.Now;
                NewServiceUnit.MODIFY_DATE = DateTime.Now;
                NewServiceUnit.CREATED_BY = User.Identity.Name;
                NewServiceUnit.MODIFIED_BY = User.Identity.Name;
                GenericData.Insert<CROSSING_SERVICE_UNIT>(NewServiceUnit);
                uxServiceUnitStore.Reload();
            }
        }
        protected void deReadSubDiv(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long ServiceUnitId = 0;
                try
                {
                     ServiceUnitId = long.Parse(e.Parameters["ServiceUnitId"].ToString());
                }
                catch (NullReferenceException)
                {
                    ServiceUnitId = 0;
                }
                List<CROSSING_SUB_DIVISION> SDData = _context.CROSSING_SUB_DIVISION.Where(x => x.SERVICE_UNIT_ID == ServiceUnitId).ToList();


                int count;
                uxSubDivStore.DataSource = GenericData.EnumerableFilterHeader<CROSSING_SUB_DIVISION>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], SDData, out count);
                e.Total = count;

            }

        }
        protected void deSaveSubDiv(object sender, DirectEventArgs e)
        {
            ChangeRecords<AddSubDiv> data = new StoreDataHandler(e.ExtraParams["subdivdata"]).BatchObjectData<AddSubDiv>();
            foreach (AddSubDiv CreatedSubDiv in data.Created)
            {
                CROSSING_SUB_DIVISION NewSubDiv = new CROSSING_SUB_DIVISION();
                NewSubDiv.SUB_DIVISION_NAME = CreatedSubDiv.SUB_DIVISION_NAME;
                NewSubDiv.SERVICE_UNIT_ID = long.Parse(e.ExtraParams["ServiceUnitId"].ToString());
                NewSubDiv.CREATE_DATE = DateTime.Now;
                NewSubDiv.MODIFY_DATE = DateTime.Now;
                NewSubDiv.CREATED_BY = User.Identity.Name;
                NewSubDiv.MODIFIED_BY = User.Identity.Name;
                GenericData.Insert<CROSSING_SUB_DIVISION>(NewSubDiv);
                uxSubDivStore.Reload();
            }
        }
        protected void deLoadSubDiv(object sender, DirectEventArgs e)
        {
            uxSubDivStore.Reload();
        }
        protected void deLoadServiceUnitStores(object sender, DirectEventArgs e)
        {
            uxCurrentServiceUnitStore.Reload();
            uxNewServiceUnitStore.Reload();
        }
        protected void deCurrentServiceUnitGrid(object sender, StoreReadDataEventArgs e)
        {
            //Get ServiceUnit List
            using (Entities _context = new Entities())
            {
                IQueryable<CROSSING_MAINTENANCE.ServiceUnitList> data = CROSSING_MAINTENANCE.GetKCSServiceUnit(_context);
               
                int count;
                uxCurrentServiceUnitStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.ServiceUnitList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void deNewServiceUnitGrid(object sender, StoreReadDataEventArgs e)
        {
            //Get Service Unit
            using (Entities _context = new Entities())
            {                 
                IQueryable<CROSSING_MAINTENANCE.ServiceUnitList> data = CROSSING_MAINTENANCE.GetKCSServiceUnit(_context);
                int count;
                uxNewServiceUnitStore.DataSource = GenericData.ListFilterHeader<CROSSING_MAINTENANCE.ServiceUnitList>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }
        protected void deShowGrid(object sender, DirectEventArgs e)
        {
            long ServiceUnitId = long.Parse(uxCurrentServiceUnit.Value.ToString());

            using (Entities _context = new Entities())
            {
                List<object> list;
                list = (from d in _context.CROSSING_SUB_DIVISION
                        where d.SERVICE_UNIT_ID == ServiceUnitId
                        select d).ToList<object>();

                uxCurrentSubDivStore.DataSource = list;
                uxCurrentSubDivStore.DataBind();

                uxTransferSubDivWindow.Show();
            }
        }
        protected void AssociateTransfer(object sender, DirectEventArgs e)
        {
            CROSSING_SUB_DIVISION data;

            //do type conversions

            long ServiceUnitId = long.Parse(uxNewServiceUnit.Value.ToString());

            string json = (e.ExtraParams["selectedSubdivs"]);
            List<SelectedSubDiv> subdivList = JSON.Deserialize<List<SelectedSubDiv>>(json);
            foreach (SelectedSubDiv subdiv in subdivList)
            {
                //Get record to be edited
                using (Entities _context = new Entities())
                {
                    
                    data = (from d in _context.CROSSING_SUB_DIVISION
                            where d.SUB_DIVISION_ID == subdiv.SUB_DIVISION_ID 
                            select d).SingleOrDefault();


                    data.SERVICE_UNIT_ID = ServiceUnitId;

                }
                GenericData.Update<CROSSING_SUB_DIVISION>(data);
            }

            uxTransferSubDivWindow.Hide();
            uxUpdateSubDivWindow.Hide();
            uxUpdateSubDivForm.Reset();
            uxCurrentServiceUnitStore.Reload();
            uxTransferNewServiceUnitStore.Reload();         
            uxServiceUnitStore.Reload();
            uxSubDivStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Subdivision(s) Transferred Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
       
        //protected void deRemoveServiceUnit(object sender, DirectEventArgs e)
        //{
        //    CROSSING_SERVICE_UNIT data;
        //    string json = e.ExtraParams["SUInfo"];

        //    List<AddServiceUnit> SUList = JSON.Deserialize<List<AddServiceUnit>>(json);
        //    foreach (AddServiceUnit ServiceUnit in SUList)
        //    {
        //        using (Entities _context = new Entities())
        //        {
        //            data = (from s in _context.CROSSING_SERVICE_UNIT
        //                    where s.SERVICE_UNIT_ID == ServiceUnit.SERVICE_UNIT_ID
        //                    select s).Single();


        //            GenericData.Delete<CROSSING_SERVICE_UNIT>(data);
                   
        //        }

        //        Notification.Show(new NotificationConfig()
        //        {
        //            Title = "Success",
        //            Html = "Service Unit Removed Successfully",
        //            HideDelay = 1000,
        //            AlignCfg = new NotificationAlignConfig
        //            {
        //                ElementAnchor = AnchorPoint.Center,
        //                TargetAnchor = AnchorPoint.Center
        //            }
        //        });


        //    }

        //}
        protected void deRemoveSubDiv(object sender, DirectEventArgs e)
        {

            string json = e.ExtraParams["SDInfo"];

            List<RemoveSubDiv> SDList = JSON.Deserialize<List<RemoveSubDiv>>(json);
            foreach (RemoveSubDiv SubDiv in SDList)
            {

                CROSSING_SUB_DIVISION data = new CROSSING_SUB_DIVISION();

                using (Entities _context = new Entities())
                {
                    data = _context.CROSSING_SUB_DIVISION.Where(x => x.SUB_DIVISION_ID == SubDiv.SUB_DIVISION_ID).SingleOrDefault();

                    uxSubDivStore.Reload();
                }

                GenericData.Delete<CROSSING_SUB_DIVISION>(data);

                Notification.Show(new NotificationConfig()
                {
                    Title = "Success",
                    Html = "Subdivision Removed Successfully",
                    HideDelay = 1000,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.Center,
                        TargetAnchor = AnchorPoint.Center
                    }
                });


            }

        }
        public class AddServiceUnit
        {
            public decimal? RAILROAD_ID { get; set; }
            public int SERVICE_UNIT_ID { get; set; }
            public string SERVICE_UNIT_NAME { get; set; }
        }
        public class AddSubDiv
        {
            public int SUB_DIVISION_ID { get; set; }
            public string SUB_DIVISION_NAME { get; set; }
            public long? SERVICE_UNIT_ID { get; set; }
        }
        public class SelectedSubDiv
        {
            public long SUB_DIVISION_ID { get; set; }
            public string SUB_DIVISION_NAME { get; set; }
            public long SERVICE_UNIT_ID { get; set; }
        }
        public class RemoveSubDiv
        {
            public int SUB_DIVISION_ID { get; set; }
            public string SUB_DIVISION_NAME { get; set; }
            public long? SERVICE_UNIT_ID { get; set; }
        }
    }
}