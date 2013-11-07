using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data.DataFactory;
using DBI.Data;
using Ext.Net;
using System.Text;
using System.Collections;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umDailyActivity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadData(object sender, StoreReadDataEventArgs e)
        {
            List<WEB_PROJECTS_V> data = WEB_PROJECTS_V.ProjectList();

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
                        itemValue = oValue as string;
                    }

                    switch (op)
                    {
                        case "=":
                            return oValue == null || !oValue.Equals(value);
                        case "compare":
                            return !((IEquatable<IComparable>)value).Equals((IComparable)oValue);
                        case "+":
                            return itemValue == null || !itemValue.StartsWith(matchValue);
                        case "-":
                            return itemValue == null || !itemValue.EndsWith(matchValue);
                        case "!":
                            return itemValue == null || itemValue.IndexOf(matchValue) >= 0;
                        case "*":
                            return itemValue == null || itemValue.IndexOf(matchValue) < 0;
                        default:
                            throw new Exception("Not supported operator");
                    }
                });
            }
            //-- end filtering ------------------------------------------------------------


            //-- start sorting ------------------------------------------------------------
            if (e.Sort.Length > 0)
            {
                data.Sort(delegate(WEB_PROJECTS_V x, WEB_PROJECTS_V y)
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

            List<WEB_PROJECTS_V> rangeData = (e.Start < 0 || limit < 0) ? data : data.GetRange(e.Start, limit);
            //-- end paging ------------------------------------------------------------

            e.Total = data.Count;
            uxFormProjectStore.DataSource = rangeData;
            uxFormProjectStore.DataBind();
        }

        protected void deStoreValue(object sender, DirectEventArgs e)
        {
            uxFormProject.SetValue(e.ExtraParams["Segment"]);
            uxFormProjectFilter.ClearFilter();
        }
        //protected void deReadData(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    Entities _context = new Entities();
        //    var data = WEB_PROJECTS_V.ProjectLookup(e.Start, e.Limit, e.Sort, "DESC", e.Parameters["filter"], "ORGANIZATION_NAME");
        //    uxFormProject.GetStore().DataSource = data;
        //    uxFormProject.GetStore().DataBind();
        //}

        //todo Finish DirectEvent and uncomment Comboboxes
        /// <summary>
        /// Direct Event that stores the Daily Activity form data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreActivity(object sender, Ext.Net.DirectEventArgs e)
        {

        }

        protected Field OnCreateFilterableField(object sender, ColumnBase column, Field defaultField)
        {
            if (column.DataIndex == "SEGMENT1")
            {
                ((TextField)defaultField).Icon = Icon.Magnifier;
            }

            return defaultField;
        }
        protected object StateList
        {
            get
            {
                return new object[]
            {
                new object[] { "AL", "Alabama" },
                new object[] { "AK", "Alaska" },
                new object[] { "AZ", "Arizona" },
                new object[] { "AR", "Arkansas" },
                new object[] { "CA", "California" },
                new object[] { "CO", "Colorado" },
                new object[] { "CT", "Connecticut" },
                new object[] { "DE", "Delaware" },
                new object[] { "DC", "District of Columbia" },
                new object[] { "FL", "Florida" },
                new object[] { "GA", "Georgia" },
                new object[] { "HI", "Hawaii" },
                new object[] { "ID", "Idaho" },
                new object[] { "IL", "Illinois" },
                new object[] { "IN", "Indiana" },
                new object[] { "IA", "Iowa" },
                new object[] { "KS", "Kansas" },
                new object[] { "KY", "Kentucky" },
                new object[] { "LA", "Louisiana" },
                new object[] { "ME", "Maine" },
                new object[] { "MD", "Maryland" },
                new object[] { "MA", "Massachusetts" },
                new object[] { "MI", "Michigan" },
                new object[] { "MN", "Minnesota" },
                new object[] { "MS", "Mississippi" },
                new object[] { "MO", "Missouri" },
                new object[] { "MT", "Montana" },
                new object[] { "NE", "Nebraska" },
                new object[] { "NV", "Nevada" },
                new object[] { "NH", "New Hampshire" },
                new object[] { "NJ", "New Jersey" },
                new object[] { "NM", "New Mexico" },
                new object[] { "NY", "New York" },
                new object[] { "NC", "North Carolina" },
                new object[] { "ND", "North Dakota" },
                new object[] { "OH", "Ohio" },
                new object[] { "OK", "Oklahoma" },
                new object[] { "OR", "Oregon" },
                new object[] { "PA", "Pennsylvania" },
                new object[] { "RI", "Rhode Island" },
                new object[] { "SC", "South Carolina" },
                new object[] { "SD", "South Dakota" },
                new object[] { "TN", "Tennessee" },
                new object[] { "TX", "Texas" },
                new object[] { "UT", "Utah" },
                new object[] { "VT", "Vermont" },
                new object[] { "VA", "Virginia" },
                new object[] { "WA", "Washington" },
                new object[] { "WV", "West Virginia" },
                new object[] { "WI", "Wisconsin" },
                new object[] { "WY", "Wyoming" } 
            };
            }
        }

    }
}