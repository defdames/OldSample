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
    public partial class umSubDivisionsTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
          protected void deSubDivMainGrid(object sender, StoreReadDataEventArgs e)
        {
          
            //Get Contacts
            using (Entities _context = new Entities())
            {
                List<object> data;
                 data = (from d in _context.CROSSING_SUB_DIVISION
                                      
                            select new
                            {  d.SUB_DIVISION_ID, d.SUB_DIVISION_NAME, d.STATE }).ToList<object>();
                int count;
             uxCurrentSubDivStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
        protected void GetSubDivGridData(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                long SubDivId = long.Parse(e.ExtraParams["SubDivId"]);
                var data = (from d in _context.CROSSING_SUB_DIVISION
                            where d.SUB_DIVISION_ID == SubDivId
                            select new
                            { d.SUB_DIVISION_ID, d.SUB_DIVISION_NAME, d.STATE, d.CITY, d.COUNTY, d.STREET, d.ROUTE, d.MILE_POST, d.SERVICE_TYPE, d.DOT, d.REMARKS
                          }).SingleOrDefault();
                uxSubDivisionSDTextField.SetValue(data.SUB_DIVISION_NAME);
                uxRouteSD.SetValue(data.ROUTE);
                uxStateSD.SetValue(data.STATE);
                uxDotNumSD.SetValue(data.DOT);
                uxStreetSD.SetValue(data.STREET);
                uxCitySD.SetValue(data.CITY);
                uxMPSD.SetValue(data.MILE_POST);
                uxCountySD.SetValue(data.COUNTY);
                uxServiceTypeSD.SetValue(data.SERVICE_TYPE);
                uxSubDivRemarks.SetValue(data.REMARKS);
            }
        }
        protected void deAddSubDiv(object sender, DirectEventArgs e)
        {
            CROSSING_SUB_DIVISION data;
           
            //do type conversions
            string SubDivName = uxAddNewSubDivSD.Value.ToString();
            string Route = uxAddNewRouteSD.Value.ToString();
            string State = uxAddNewStateSDTextField.Value.ToString();
            string Dot = uxAddNewDOTNumSD.Value.ToString();
            string City = uxAddNewCitySD.Value.ToString();
            string Street = uxAddNewStreetSD.Value.ToString();
            decimal MP = Convert.ToDecimal(uxAddNewMPSD.Value);
            string County = uxAddNewCountySD.Value.ToString();
            string ServiceType = uxAddNewServiceTypeSD.Value.ToString();
            string Remarks = uxAddNewRemarksSD.Value.ToString();
            
            //Add to Db
            using (Entities _context = new Entities())
            {
                data = new CROSSING_SUB_DIVISION()
                {
                   
                    SUB_DIVISION_NAME = SubDivName,
                    ROUTE = Route,
                    DOT = Dot,
                    CITY = City,
                    STREET = Street,
                    MILE_POST = MP,
                    COUNTY = County,
                    SERVICE_TYPE = ServiceType,
                    REMARKS = Remarks,
                    STATE = State,
                  
                };
            }
           
            //Process addition
            GenericData.Insert<CROSSING_SUB_DIVISION>(data);

            uxAddSubdivisionWindow.Hide();
            uxSubDivForm.Reset();
            uxCurrentSubDivStore.Reload();
          

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Contact Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        protected void deEditSubDivForm(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                long SubDivId = long.Parse(e.ExtraParams["SubDivId"]);
                var data = (from d in _context.CROSSING_SUB_DIVISION
                            where d.SUB_DIVISION_ID == SubDivId
                            select new
                             { d.SUB_DIVISION_ID, d.SUB_DIVISION_NAME, d.STATE, d.CITY, d.COUNTY, d.STREET, d.ROUTE, d.MILE_POST, d.SERVICE_TYPE, d.DOT, d.REMARKS
                          }).SingleOrDefault();
                uxEditSubDivSD.SetValue(data.SUB_DIVISION_NAME);
                uxEditRouteSD.SetValue(data.ROUTE);
                uxEditStateSDTextField.SetValue(data.STATE);
                uxEditDOTNumSD.SetValue(data.DOT);
                uxEditStreetSD.SetValue(data.STREET);
                uxEditCitySD.SetValue(data.CITY);
                uxEditMPSD.SetValue(data.MILE_POST);
                uxEditCountySD.SetValue(data.COUNTY);
                uxEditServiceTypeSD.SetValue(data.SERVICE_TYPE);
                uxEditRemarksSD.SetValue(data.REMARKS);
            }
        }
        

        /// <summary>
        /// Store edit changes to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditSubDivs(object sender, DirectEventArgs e)
        {
            CROSSING_SUB_DIVISION data;

            //do type conversions
            string SubDivName = uxEditSubDivSD.Value.ToString();
            string Route = uxEditRouteSD.Value.ToString();
            string State = uxEditStateSDTextField.Value.ToString();
            string Dot = uxEditDOTNumSD.Value.ToString();
            string City = uxEditCitySD.Value.ToString();
            string Street = uxEditStreetSD.Value.ToString();
            decimal MP = Convert.ToDecimal(uxEditMPSD.Value);
            string County = uxEditCountySD.Value.ToString();
            string ServiceType = uxEditServiceTypeSD.Value.ToString();
            string Remarks = uxEditRemarksSD.Value.ToString();

            //Get record to be edited
            using (Entities _context = new Entities())
            {
                var SubDivId = long.Parse(e.ExtraParams["SubDivId"]);
                data = (from d in _context.CROSSING_SUB_DIVISION
                        where d.SUB_DIVISION_ID == SubDivId
                        select d).Single();
            }

            data.SUB_DIVISION_NAME = SubDivName;
            data.ROUTE = Route;
            data.STATE = State;
            data.DOT = Dot;
            data.CITY = City;
            data.MILE_POST = MP;
            data.STATE = State;
            data.COUNTY = County;
            data.SERVICE_TYPE = ServiceType;
            data.REMARKS = Remarks;


            GenericData.Update<CROSSING_SUB_DIVISION>(data);

            uxEditSubdivisionWindow.Hide();
            uxSubDivForm.Reset();
            uxCurrentSubDivStore.Reload();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }
        protected void deRemoveSubdivision(object sender, DirectEventArgs e)
        {
            long SubDivId = long.Parse(e.ExtraParams["SubDivId"]);
            CROSSING_SUB_DIVISION data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.CROSSING_SUB_DIVISION
                        where d.SUB_DIVISION_ID == SubDivId
                        select d).Single();
            }
            GenericData.Delete<CROSSING_SUB_DIVISION>(data);

            uxCurrentSubDivStore.Reload();
            uxSubDivForm.Reset();

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Crossing Removed Successfully",
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