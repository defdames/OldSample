using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umEquipmentTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetCurrentEquipment();
        }
        
        /// <summary>
        /// Gets Header's current equipment for gridpanel
        /// </summary>
        protected void GetCurrentEquipment()
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(Request.QueryString["headerId"]);
                var data = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
                            join p in _context.PROJECTS_V on e.PROJECT_ID equals p.PROJECT_ID
                            where e.HEADER_ID == HeaderId
                            select new { p.CLASS_CODE, p.ORGANIZATION_NAME, e.ODOMETER_START, e.ODOMETER_END, e.PROJECT_ID, e.EQUIPMENT_ID, p.NAME, e.HEADER_ID }).ToList();
                uxCurrentEquipmentStore.DataSource = data;
            }
        }

        /// <summary>
        /// Filter Equipment list for add/edit drop-down grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReadGrid(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_EQUIPMENT_V> data = new List<WEB_EQUIPMENT_V>();

            if (e.Parameters["Form"] == "Add")
            {
                if (uxAddEquipmentToggleOrg.Pressed)
                {
                    //Get All Projects
                    data = WEB_EQUIPMENT_V.ListEquipment();
                }
                else
                {
                    var MyAuth = new Authentication();
                    int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                    //Get projects for my org only
                    data = WEB_EQUIPMENT_V.ListEquipment(CurrentOrg);
                }
            }
            else
            {
                if (uxEditRegion.Pressed)
                {
                    //Get All Projects
                    data = WEB_EQUIPMENT_V.ListEquipment();
                }
                else
                {
                    var MyAuth = new Authentication();
                    int CurrentOrg = Convert.ToInt32(MyAuth.GetClaimValue("CurrentOrgId", User as ClaimsPrincipal));
                    //Get projects for my org only
                    data = WEB_EQUIPMENT_V.ListEquipment(CurrentOrg);
                }
            }

            //-- start filtering -----------------------------------------------------------
            FilterHeaderConditions fhc = new FilterHeaderConditions(e.Parameters["filterheader"]);

            foreach (FilterHeaderCondition condition in fhc.Conditions)
            {
                string dataIndex = condition.DataIndex;
                FilterType type = condition.Type;
                string op = condition.Operator;
                object value = null;

                switch (condition.Type)
                {
                    case FilterType.Boolean:
                        value = condition.Value<bool>();
                        break;

                    case FilterType.Date:
                        switch (condition.Operator)
                        {
                            case "=":
                                value = condition.Value<DateTime>();
                                break;

                            case "compare":
                                value = FilterHeaderComparator<DateTime>.Parse(condition.JsonValue);
                                break;
                        }
                        break;

                    case FilterType.Numeric:
                        bool isInt = data.Count > 0 && data[0].GetType().GetProperty(dataIndex).PropertyType == typeof(int);
                        switch (condition.Operator)
                        {
                            case "=":
                                if (isInt)
                                {
                                    value = condition.Value<int>();
                                }
                                else
                                {
                                    value = condition.Value<double>();
                                }
                                break;

                            case "compare":
                                if (isInt)
                                {
                                    value = FilterHeaderComparator<int>.Parse(condition.JsonValue);
                                }
                                else
                                {
                                    value = FilterHeaderComparator<double>.Parse(condition.JsonValue);
                                }

                                break;
                        }

                        break;
                    case FilterType.String:
                        value = condition.Value<string>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                data.RemoveAll(item =>
                {
                    object oValue = item.GetType().GetProperty(dataIndex).GetValue(item, null);
                    string matchValue = null;
                    string itemValue = null;

                    if (type == FilterType.String)
                    {
                        matchValue = (string)value;
                        matchValue = matchValue.ToLower();
                        itemValue = oValue as string;
                        itemValue = itemValue.ToLower();
                    }
                       return itemValue == null || itemValue.IndexOf(matchValue) < 0;
                });
            }
            //-- end filtering ------------------------------------------------------------


            //-- start sorting ------------------------------------------------------------
            if (e.Sort.Length > 0)
            {
                data.Sort(delegate(WEB_EQUIPMENT_V x, WEB_EQUIPMENT_V y)
                {
                    object a;
                    object b;

                    int direction = e.Sort[0].Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                    a = x.GetType().GetProperty(e.Sort[0].Property).GetValue(x, null);
                    b = y.GetType().GetProperty(e.Sort[0].Property).GetValue(y, null);
                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }
            //-- end sorting ------------------------------------------------------------


            //-- start paging ------------------------------------------------------------
            int limit = e.Limit;

            if ((e.Start + e.Limit) > data.Count)
            {
                limit = data.Count - e.Start;
            }

            List<WEB_EQUIPMENT_V> rangeData = (e.Start < 0 || limit < 0) ? data : data.GetRange(e.Start, limit);
            //-- end paging ------------------------------------------------------------

            e.Total = data.Count;
            if (e.Parameters["Form"] == "Add")
            {
                uxEquipmentStore.DataSource = rangeData;
                uxEquipmentStore.DataBind();
            }
            else
            {
                uxEditEquipmentProjectStore.DataSource = rangeData;
                uxEditEquipmentProjectStore.DataBind();
            }
        }

        /// <summary>
        /// Add equipment to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deAddEquipment(object sender, DirectEventArgs e)
        {
            long headerId = long.Parse(Request.QueryString["headerId"]);
            long projectId = long.Parse(uxAddEquipmentDropDown.Value.ToString());
            long odStart = long.Parse(uxAddEquipmentStart.Value.ToString());
            long odEnd = long.Parse(uxAddEquipmentEnd.Value.ToString());
            var MyAuth = new Authentication();
            var icp = User as ClaimsPrincipal;
            var AddingUser = MyAuth.GetClaimValue(ClaimTypes.Name, icp);

            DAILY_ACTIVITY_EQUIPMENT added = new DAILY_ACTIVITY_EQUIPMENT()
            {
                HEADER_ID = headerId,
                PROJECT_ID = projectId,
                ODOMETER_START = odStart,
                ODOMETER_END = odEnd,
                CREATE_DATE = DateTime.Now,
                MODIFY_DATE = DateTime.Now,
                CREATED_BY = AddingUser,
                MODIFIED_BY = AddingUser
            };

            //Write Data to DB
            GenericData.Insert<DAILY_ACTIVITY_EQUIPMENT>(added);
            uxAddEquipmentWindow.Hide();
            uxCurrentEquipmentStore.Reload();
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Equipment Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Load edit equipment form and populate fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditEquipmentForm(object sender, DirectEventArgs e)
        {
            //JSON Decode Row and assign to variables
            string JsonValues = e.ExtraParams["EquipmentDetails"];
            Dictionary<string, string>[] EquipmentDetails = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);
            
            //Populate form with existing data
            foreach (Dictionary<string, string> Detail in EquipmentDetails)
            {
                uxEditEquipmentProject.SetValue(Detail["PROJECT_ID"], Detail["NAME"]);
                uxEditEquipmentStart.SetValue(Detail["ODOMETER_START"]);
                uxEditEquipmentEnd.SetValue(Detail["ODOMETER_END"]);                              
            }
        }

        /// <summary>
        /// Store edits to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deEditEquipment(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_EQUIPMENT data;
            using (Entities _context = new Entities())
            {
                long EquipmentId = long.Parse(e.ExtraParams["EquipmentId"]);
                //Get Current Record
                data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                        where d.EQUIPMENT_ID == EquipmentId
                        select d).Single();

                long ProjectId = long.Parse(uxEditEquipmentProject.Value.ToString());
                long OdStart = long.Parse(uxEditEquipmentStart.Value.ToString());
                long OdEnd = long.Parse(uxEditEquipmentEnd.Value.ToString());

                //Update Entity
                data.PROJECT_ID = ProjectId;
                data.ODOMETER_START = OdStart;
                data.ODOMETER_END = OdEnd;
                data.MODIFIED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;
            }
            //Save to DB
            GenericData.Update<DAILY_ACTIVITY_EQUIPMENT>(data);
            uxCurrentEquipmentStore.Reload();
            uxEditEquipmentWindow.Hide();
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Equipment Edited Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
            
        }

        /// <summary>
        /// Remove equipment from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveEquipment(object sender, DirectEventArgs e)
        {
            //Convert EquipmentId to long
            long EquipmentId = long.Parse(e.ExtraParams["EquipmentId"]);
            DAILY_ACTIVITY_EQUIPMENT data;
            using (Entities _context = new Entities())
            {
                data = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                        where d.EQUIPMENT_ID == EquipmentId
                        select d).Single();
            }
            GenericData.Delete<DAILY_ACTIVITY_EQUIPMENT>(data);
            uxCurrentEquipmentStore.Reload();
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Equipment Removed Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
        }

        /// <summary>
        /// Update toggle button and reload store
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deReloadStore(object sender, DirectEventArgs e)
        {
            string type = e.ExtraParams["Type"];
            if (type == "Equipment")
            {
                uxEquipmentStore.Reload();
                if (uxAddEquipmentToggleOrg.Pressed)
                {
                    uxAddEquipmentToggleOrg.Text = "My Region";
                }
                else
                {
                    uxAddEquipmentToggleOrg.Text = "All Regions";
                }
            }
            if (type == "Edit")
            {
                uxEditEquipmentProjectStore.Reload();
                if (uxEditRegion.Pressed)
                {
                    uxEditRegion.Text = "My Region";
                }
                else
                {
                    uxEditRegion.Text = "All Regions";
                }
            }
        }

        /// <summary>
        /// Put selected grid value into drop-down field and clear filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreGridValue(object sender, DirectEventArgs e)
        {
            if (e.ExtraParams["Form"] == "Add")
            {
                //Set value and text for equipment
                uxAddEquipmentDropDown.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["EquipmentName"]);

                //Clear existing filters
                uxAddEquipmentFilter.ClearFilter();
            }
            else
            {
                //Set value and text for equipment
                uxEditEquipmentProject.SetValue(e.ExtraParams["ProjectId"], e.ExtraParams["EquipmentName"]);

                //Clear existing filters
                uxEditEquipmentFilter.ClearFilter();
            }
        }
    }
}