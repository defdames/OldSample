using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ext.Net;
using System.Data.Entity;

namespace DBI.Data
{
    public partial class SYS_LOG
    {

        /// <summary>
        /// Logs data to the database for errors
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public static void LogToDatabase(Exception ex, out string ErrorID,string username = null)
        {
            using (Entities _context = new Entities())
            {
                SYS_LOG _log = new SYS_LOG();
                HttpContext _httpcontext = HttpContext.Current;
                CultureInfo InvC = new CultureInfo("");

                //Generate a new GUID to track this error
                ErrorID = Guid.NewGuid().ToString();
                _log.GUID = ErrorID;

                _log.USER_ID = (username != null) ? SYS_USER_INFORMATION.UserID(username) : SYS_USER_INFORMATION.UserID(_httpcontext.User.Identity.Name);
                _log.MESSAGE = ex.Message;
                _log.INNER_EXCEPTION = ex.InnerException.Message;
                _log.SOURCE = ex.Source;
                _log.STACKTRACE = ex.StackTrace;
                _log.USER_CULTURE = CultureInfo.CurrentCulture.Name;
                _log.CREATED_DATE = DBI.Core.DateTime_xm.InvariantCulture(DateTime.Now);

                // Log as debug mode issue
                #if (DEBUG)
                _log.DEBUG = "Y";
                #endif

                GenericData.Insert<SYS_LOG>(_log);

            }

        }

        /// <summary>
        /// Returns a list of log information from oracle
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<SYS_LOG_CT> Logs(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            try
            {
                List<SYS_LOG_CT> data = new List<SYS_LOG_CT>();

                using (Entities _context = new Entities())
                {
                    //Get a list of information we need
                    List<SYS_LOG> results = _context.SYS_LOG.Include(a => a.SYS_USERS).ToList();

                    foreach (SYS_LOG log in results)
                    {
                        // Get User Information
                        SYS_USER_INFORMATION user = SYS_USER_INFORMATION.UserByID(log.USER_ID);

                        if (user != null)
                        {
                            SYS_LOG_CT _log = new SYS_LOG_CT();
                            _log.ID = log.ID;
                            _log.USER_NAME = user.USER_NAME;
                            _log.EMPLOYEE_NAME = user.EMPLOYEE_NAME;
                            _log.GUID = log.GUID;
                            _log.MESSAGE = log.MESSAGE;
                            _log.INNER_EXCEPTION = log.INNER_EXCEPTION;
                            _log.SOURCE = log.SOURCE;
                            _log.STACKTRACE = log.STACKTRACE;
                            _log.USER_CULTURE = log.USER_CULTURE;
                            _log.DEBUG = log.DEBUG;
                            _log.CREATED_DATE = log.CREATED_DATE;
                            data.Add(_log);
                        }
                    }

                }

                return GenericData.EnumerableFilter<SYS_LOG_CT>(start, limit, sort, filter, data, out count);
            }
            catch (Exception e)
            {
                throw(e);
            }
            
        }

    }
}
