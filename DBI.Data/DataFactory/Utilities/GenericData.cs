using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ext.Net;

namespace DBI.Data.DataFactory.Utilities
{
    public static class GenericData
    {

        public static void Insert<T>(T entity) where T : class
        {
            using (Entities _context = new Entities())
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
        }


        public static void Delete<T>(T entity) where T : class
        {
            using (Entities _context = new Entities())
            {
                _context.Set<T>().Attach(entity);
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }


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
            Entities _context = new Entities();
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

            return rangeData.AsEnumerable<T>();
        }





    }
}


