using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.DataFactory.Utilities
{
    public static class GenericData
    {
        public static void Insert<T>(T entity) where T : class
        {
            using (Entities context = new Entities())
            {
                DbTransaction transaction = null;
                try
                {
                    context.Database.Connection.Open();
                    transaction = context.Database.Connection.BeginTransaction();
                    context.Set<T>().Add(entity);
                    transaction.Commit();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    context.Database.Connection.Close();
                    transaction = null;
                }
            }
        }

        public static void Delete<T>(T entity) where T : class
        {
            using (Entities context = new Entities())
            {
                try
                {
                    context.Database.Connection.Open();
                    context.Set<T>().Remove(entity);
                    context.SaveChanges();
                }
                finally
                {
                    context.Database.Connection.Close();
                }
            }
        }

    }
}
