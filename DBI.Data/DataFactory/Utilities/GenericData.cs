using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ext.Net;

namespace DBI.Data
{
    public static class GenericData
    {
        /// <summary>
        /// Checks to see if we can make a connection to Oracle
        /// </summary>
        /// <returns></returns>
        public static bool IsContextValid()
        {
            bool isValid = false;

            try
            {
                using (Entities _context = new Entities())
                {
                    _context.SYS_ACTIVITY.Count();
                }
                isValid = true;
            }
            catch
            {
                isValid = false;
            }

            return isValid;

        }


        /// <summary>
        /// This inserts data into any entity object using sqlcommand
        /// </summary>
        /// <typeparam name="T">Entity object type</typeparam>
        /// <param name="entity">Entity object you want to insert</param>
        public static void Insert<T>(T entity, String[] includedColumns, string tableName) where T : class
        {

            using (Entities _context = new Entities())
            {

                var colNames = typeof(T).GetProperties().Select(a => a.Name).ToList();

                StringBuilder sqlColumns = new StringBuilder();
                StringBuilder sqlValues = new StringBuilder();
                StringBuilder sqlInsert = new StringBuilder();

                int count = 0;
                string sql = "INSERT INTO " + tableName;

                foreach (string column in includedColumns.ToList())
                {
                    PropertyInfo propInfo = entity.GetType().GetProperty(column);
                    object columnValue = propInfo.GetValue(entity,null);

                        count = count + 1;
                        sqlColumns.Append(column);

                        if (count < includedColumns.Count())
                        {
                            sqlColumns.Append(",");
                        }

                        if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?))
                        {
                            DateTime dateValue = (DateTime)columnValue;

                            sqlValues.Append("TO_DATE('" + dateValue.ToString("MM/dd/yyyy HH:mm:ss") + "','MM/DD/YYYY HH24:MI:SS')");
                        }
                        else
                        {
                            sqlValues.Append("'" + columnValue + "'");
                        }

                        if (count < includedColumns.Count())
                        {
                            sqlValues.Append(",");
                        }
                }

                sqlInsert.Append(sql);
                sqlInsert.Append(" (");
                sqlInsert.Append(sqlColumns.ToString());
                sqlInsert.Append(") VALUES(");
                sqlInsert.Append(sqlValues.ToString());
                sqlInsert.Append(")");
                _context.Database.ExecuteSqlCommand(sqlInsert.ToString());
            }
        }



        /// <summary>
        /// This inserts data into any entity object
        /// </summary>
        /// <typeparam name="T">Entity object type</typeparam>
        /// <param name="entity">Entity object you want to insert</param>
        public static void Insert<T>(T entity) where T : class
        {
            using (Entities _context = new Entities())
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// This deletes data into any entity object
        /// </summary>
        /// <typeparam name="T">Entity object type</typeparam>
        /// <param name="entity">Entity object you want to delete</param>
        public static void Delete<T>(T entity) where T : class
        {
            using (Entities _context = new Entities())
            {
                _context.Set<T>().Attach(entity);
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// This updates data into any entity object
        /// </summary>
        /// <typeparam name="T">Entity object type</typeparam>
        /// <param name="entity">Entity object you want to update</param>
        public static void Update<T>(T entity) where T : class
        {
            using (Entities _context = new Entities())
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = System.Data.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public static IEnumerable<T> EnumerableFilter<T>(int start, int limit, DataSorter[] sort, string filter, out int count) where T : class
        {
            using (Entities _context = new Entities())
            {
                //-- return the list but do not track the information ---------------------------
                List<T> data = _context.Set<T>().ToList();

                //-- start filtering ------------------------------------------------------------
                if (!string.IsNullOrEmpty(filter))
                {
                    FilterConditions fc = new FilterConditions(filter);

                    foreach (FilterCondition condition in fc.Conditions)
                    {
                        Comparison comparison = condition.Comparison;
                        string field = condition.Field;
                        FilterType type = condition.Type;

                        object value;
                        switch (condition.Type)
                        {
                            case FilterType.Boolean:
                                value = condition.Value<bool>();
                                break;
                            case FilterType.Date:
                                value = condition.Value<DateTime>();
                                break;
                            case FilterType.List:
                                value = condition.List;
                                break;
                            case FilterType.Numeric:
                                if (data.Count() > 0 && data[0].GetType().GetProperty(field).PropertyType == typeof(int))
                                {
                                    value = condition.Value<int>();
                                }
                                else
                                {
                                    value = condition.Value<double>();
                                }

                                break;
                            case FilterType.String:
                                value = condition.Value<string>();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        data.RemoveAll(
                            item =>
                            {
                                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                IComparable cItem = oValue as IComparable;

                                switch (comparison)
                                {
                                    case Comparison.Eq:

                                        switch (type)
                                        {
                                            case FilterType.List:
                                                return !(value as List<string>).Contains(oValue.ToString());
                                            case FilterType.String:
                                                return (oValue != null) ? !oValue.ToString().ToLower().Contains(value.ToString().ToLower()) : true;
                                            default:
                                                return !cItem.Equals(value);
                                        }

                                    case Comparison.Gt:
                                        return cItem.CompareTo(value) < 1;
                                    case Comparison.Lt:
                                        return cItem.CompareTo(value) > -1;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        );
                    }
                }

                //-- end filtering ------------------------------------------------------------

                //-- start sorting ------------------------------------------------------------
                if (sort.Length > 0)
                {
                    data.Sort(delegate(T x, T y)
                    {
                        object a;
                        object b;

                        int direction = sort[0].Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                        a = x.GetType().GetProperty(sort[0].Property).GetValue(x, null);
                        b = y.GetType().GetProperty(sort[0].Property).GetValue(y, null);
                        return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                    });
                }
                //-- end sorting ------------------------------------------------------------


                //-- start paging -----------------------------------------------------------

                if ((start + limit) > data.Count)
                {
                    limit = data.Count - start;
                }

                List<T> rangeData = (start < 0 || limit < 0) ? data : data.GetRange(start, limit);
                //-- end paging ------------------------------------------------------------

                count = data.Count;
                return rangeData;

            }
        }

        /// <summary>
        /// This returns a list of data used mostly for complex types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="dataIn"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerableFilter<T>(int start, int limit, DataSorter[] sort, string filter, List<T> dataIn, out int count) where T : class
        {
                //-- data is copied from entry, so ignore pull from database.
                List<T> data = dataIn;

                //-- start filtering ------------------------------------------------------------
                if (!string.IsNullOrEmpty(filter))
                {
                    FilterConditions fc = new FilterConditions(filter);

                    foreach (FilterCondition condition in fc.Conditions)
                    {
                        Comparison comparison = condition.Comparison;
                        string field = condition.Field;
                        FilterType type = condition.Type;

                        object value;
                        switch (condition.Type)
                        {
                            case FilterType.Boolean:
                                value = condition.Value<bool>();
                                break;
                            case FilterType.Date:
                                value = condition.Value<DateTime>();
                                break;
                            case FilterType.List:
                                value = condition.List;
                                break;
                            case FilterType.Numeric:
                                if (data.Count() > 0 && data[0].GetType().GetProperty(field).PropertyType == typeof(int))
                                {
                                    value = condition.Value<int>();
                                }
                                else
                                {
                                    value = condition.Value<double>();
                                }

                                break;
                            case FilterType.String:
                                value = condition.Value<string>();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        data.RemoveAll(
                            item =>
                            {
                                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                IComparable cItem = oValue as IComparable;

                                switch (comparison)
                                {
                                    case Comparison.Eq:

                                        switch (type)
                                        {
                                            case FilterType.List:
                                                return !(value as List<string>).Contains(oValue.ToString());
                                            case FilterType.String:
                                                return (oValue != null) ? !oValue.ToString().ToLower().Contains(value.ToString().ToLower()) : true;
                                            default:
                                                return !cItem.Equals(value);
                                        }

                                    case Comparison.Gt:
                                        return cItem.CompareTo(value) < 1;
                                    case Comparison.Lt:
                                        return cItem.CompareTo(value) > -1;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        );
                    }
                }

                //-- end filtering ------------------------------------------------------------

                //-- start sorting ------------------------------------------------------------
                if (sort.Length > 0)
                {
                    data.Sort(delegate(T x, T y)
                    {
                        object a;
                        object b;

                        int direction = sort[0].Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                        a = x.GetType().GetProperty(sort[0].Property).GetValue(x, null);
                        b = y.GetType().GetProperty(sort[0].Property).GetValue(y, null);
                        return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                    });
                }
                //-- end sorting ------------------------------------------------------------


                //-- start paging -----------------------------------------------------------

                if ((start + limit) > data.Count)
                {
                    limit = data.Count - start;
                }

                List<T> rangeData = (start < 0 || limit < 0) ? data : data.GetRange(start, limit);
                //-- end paging ------------------------------------------------------------

                count = data.Count;
                return rangeData;
        }

        /// <summary>
        /// This returns a list of data used mostly for complex types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="dataIn"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerableFilterHeader<T>(int start, int limit, DataSorter[] sort, string filter, List<T> dataIn, out int count) where T : class
        {
            //-- data is copied from entry, so ignore pull from database.
            List<T> data = dataIn;

            //-- start filtering -----------------------------------------------------------
            FilterHeaderConditions fhc = new FilterHeaderConditions(filter);

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

                    return (oValue != null) ? !oValue.ToString().ToLower().Contains(value.ToString().ToLower()) : true;
                   
                });
            }
            //-- end filtering ------------------------------------------------------------


            //-- start sorting ------------------------------------------------------------
            if (sort.Length > 0)
            {
                data.Sort(delegate(T x, T y)
                {
                    object a;
                    object b;

                    int direction = sort[0].Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                    a = x.GetType().GetProperty(sort[0].Property).GetValue(x, null);
                    b = y.GetType().GetProperty(sort[0].Property).GetValue(y, null);
                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }
            //-- end sorting ------------------------------------------------------------


            //-- start paging ------------------------------------------------------------
            
            if ((start + limit) > data.Count)
            {
                limit = data.Count - start;
            }

            List<T> rangeData = (start < 0 || limit < 0) ? data : data.GetRange(start, limit);
            //-- end paging ------------------------------------------------------------

        

            count = data.Count;
            return rangeData;
        }

        public static Ext.Net.Paging<T> PagingFilter<T>(int start, int limit, string sort, string dir, string filter, string field) where T : class
        {
            using (Entities _context = new Entities())
            {
                //-- return the list but do not track the information ---------------------------
                List<T> data = _context.Set<T>().ToList();

                //-- start filtering ------------------------------------------------------------
                if (!string.IsNullOrEmpty(filter) && filter != "*")
                {
                        data.RemoveAll(
                            item =>
                            {
                                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                IComparable cItem = oValue as IComparable;
                                return (oValue != null) ? !oValue.ToString().ToLower().Contains(filter.ToLower()) : true;
                            }
                        );
                    }

                //-- end filtering ------------------------------------------------------------

                //-- start sorting ------------------------------------------------------------
                if (sort.Length > 0)
                {
                    data.Sort(delegate(T x, T y)
                    {
                        object a;
                        object b;

                        int direction = dir == "DESC" ? -1 : 1;

                        a = x.GetType().GetProperty(sort).GetValue(x, null);
                        b = y.GetType().GetProperty(sort).GetValue(y, null);
                        return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                    });
                }
                //-- end sorting ------------------------------------------------------------


                //-- start paging -----------------------------------------------------------

                if ((start + limit) > data.Count)
                {
                    limit = data.Count - start;
                }

                List<T> rangeData = (start < 0 || limit < 0) ? data : data.GetRange(start, limit);
                //-- end paging ------------------------------------------------------------

                int count = data.Count;
                return new Ext.Net.Paging<T>(rangeData,count);

            }
        }

        public static Ext.Net.Paging<T> PagingFilter<T>(int start, int limit, string sort, string dir, string filter, List<T> dataIn, string field) where T : class
        {
            using (Entities _context = new Entities())
            {
                //-- data is copied from entry, so ignore pull from database.
                List<T> data = dataIn;

                //-- start filtering ------------------------------------------------------------
                if (!string.IsNullOrEmpty(filter) && filter != "*")
                {
                        data.RemoveAll(
                            item =>
                            {
                                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                IComparable cItem = oValue as IComparable;
                                return (oValue != null) ? !oValue.ToString().ToLower().Contains(filter.ToLower()) : true;
                            }
                        );
                    }

                //-- end filtering ------------------------------------------------------------

                //-- start sorting ------------------------------------------------------------
                if (sort.Length > 0)
                {
                    data.Sort(delegate(T x, T y)
                    {
                        object a;
                        object b;

                        int direction = dir == "DESC" ? -1 : 1;

                        a = x.GetType().GetProperty("ORGANIZATION_NAME").GetValue(x, null);
                        b = y.GetType().GetProperty("ORGANIZATION_NAME").GetValue(y, null);
                        return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                    });
                }
                //-- end sorting ------------------------------------------------------------


                //-- start paging -----------------------------------------------------------

                if ((start + limit) > data.Count)
                {
                    limit = data.Count - start;
                }

                List<T> rangeData = (start < 0 || limit < 0) ? data : data.GetRange(start, limit);
                //-- end paging ------------------------------------------------------------

                int count = data.Count;
                return new Ext.Net.Paging<T>(rangeData,count);

            }
        }
        
        /// <summary>
        /// Allows you to test the connection to oracle and if it fails return a message saying it's down.
        /// </summary>
        /// <returns></returns>
        public static bool oracleConnectionTest()
        {
            var db = new Entities();

            try
            {
                db.Database.Connection.Open();   // check the database connection
                return true;
            }
            catch
            {
                return false;
            }

        }



    }
}


