using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Core
{
    public static class IQueryable_xm
    {
            //public static IQueryable<T> AddEqualityCondition<T, V>(this IQueryable<T> queryable, string propertyName, V propertyValue)
            //{
            //    ParameterExpression pe = Expression.Parameter(typeof(T), "p");

            //    IQueryable<T> x = queryable.Where<T>(
            //        Expression.Lambda<Func<T, bool>>(
            //        Expression.Equal(Expression.Property(pe, typeof(T).GetProperty(propertyName)),
            //            Expression.Constant(propertyValue, typeof(V)), false, typeof(T).GetMethod("op_Equality")), new ParameterExpression[] { pe }));

            //    return (x);
            //}

            public static IQueryable<T> AddEqualCondition<T, V>(this IQueryable<T> queryable, string propertyName, V propertyValue)
            {
                try
                {
                    ParameterExpression pe = Expression.Parameter(typeof(T), "p");

                    var me = Expression.Property(pe, typeof(T).GetProperty(propertyName));
                    var ce = Expression.Constant(propertyValue, typeof(string));
                    MethodInfo method = typeof(string).GetMethod("Equal", new[] { typeof(string) });
                    var call = Expression.Call(ce, method, me);
                    var lambda = Expression.Lambda<Func<T, bool>>(call, pe);

                    IQueryable<T> x = queryable.Where<T>(lambda);
                    return (x);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static IQueryable<T> AddContainsCondition<T, V>(this IQueryable<T> queryable, string propertyName, V propertyValue)
            {
                    ParameterExpression _pe = Expression.Parameter(typeof(T), "p");
                    var _columnExpression = Expression.Property(_pe, typeof(T).GetProperty(propertyName));
                    var _valueExpression = Expression.Constant(propertyValue.ToString().ToLower(), typeof(string));
                    MethodInfo _methodToLower = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
                    Expression _firstCall = Expression.Call(_columnExpression, _methodToLower);
                    MethodInfo _methodContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    Expression _secondCall = Expression.Call(_firstCall, _methodContains, _valueExpression);
                    
                    var _lambda = Expression.Lambda<Func<T, bool>>(_secondCall, _pe);
                    IQueryable<T> x = queryable.Where<T>(_lambda);
                    return (x);
            }

        }
    }
