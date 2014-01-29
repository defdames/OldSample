using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ext.Net;
using System.Data.Entity;
using DBI.Core;
using DBI.Data.DataFactory.Messages;
using DBI.Core.Web;
using System.Globalization;

namespace DBI.Data
{
    public partial class SYS_ACTIVITY
    {
        /// <summary>
        /// Returns a list of user activities that have not been assigned to a user are free to use
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<SYS_ACTIVITY> UnassignedActivities(long userID)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<SYS_ACTIVITY> activity = _context.Set<SYS_ACTIVITY>().ToList();
                    List<SYS_USER_ACTIVITY> userActivity = DBI.Data.SYS_USER_ACTIVITY.ActivityByUserID(userID);
                    activity.RemoveAll(i => userActivity.Select(s => s.ACTIVITY_ID).Contains(i.ACTIVITY_ID));
                    return activity;
                }
            }
            catch (Exception e)
            {
                throw (e);
                //string ErrorID;
                //DBI.Data.SYS_LOG.LogToDatabase(e, "GetOpenActivities", out ErrorID);
                //throw new Exception(string.Format(Resource.GetOpenActivities + " " + e.Message + "  {0}", ErrorID));
            }
          
        }

        /// <summary>
        /// Returns a list of Activities that are assigned to the system
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<SYS_ACTIVITY> Activities(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            try
            {
                return GenericData.EnumerableFilter<SYS_ACTIVITY>(start, limit, sort, filter, out count);
            }
            catch (Exception e)
            {
                throw (e);
                //string ErrorID;
                //DBI.Data.SYS_LOG.LogToDatabase(e, "GetActivities", out ErrorID);
                //throw new Exception(string.Format(Resource.GetActivities + " " + e.Message + " {0}", ErrorID));
            }
          
        }

        /// <summary>
        /// Returns an activity by activity id, also checks to see if the activity is new or not
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public static SYS_ACTIVITY Activity(long activityID, out bool isNew)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    SYS_ACTIVITY activity = _context.SYS_ACTIVITY.Where(a => a.ACTIVITY_ID == activityID).SingleOrDefault();
                    isNew = (activity == null);
                    if (isNew)
                    {
                        activity = new SYS_ACTIVITY();
                    }
                    return activity;
                }
            }
            catch (Exception e)
            {
                throw (e);
                //string ErrorID;
                //DBI.Data.SYS_LOG.LogToDatabase(e, "GetActivity", out ErrorID);
                //throw new Exception(string.Format(Resource.GetActivity + " " + e.Message + "  {0}", ErrorID));
            }
        }

        /// <summary>
        /// Returns a list of activity permissions that can be used for the employee
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Claim> Claims(string username)
        {
                using (Entities _context = new Entities())
                {

                    List<SYS_USER_ACTIVITY> roles = new List<SYS_USER_ACTIVITY>();

                    SYS_USER_INFORMATION userInfo = _context.Set<SYS_USER_INFORMATION>().Where(u => u.USER_NAME.Equals(username.ToUpper())).SingleOrDefault();

                    if (userInfo != null)
                    {
                        roles = _context.SYS_USER_ACTIVITY.Include(r => r.SYS_ACTIVITY).Where(a => a.USER_ID.Equals(userInfo.USER_ID)).ToList();
                    }

                    List<Claim> claims = new List<Claim>();

                    foreach (SYS_USER_ACTIVITY srole in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, srole.SYS_ACTIVITY.NAME));

                    }

                    // Add a claim for the username
                    claims.Add(new Claim(ClaimTypes.Name, username.ToUpper()));

                    // Add full name of user to the claims 
                    claims.Add(new Claim("EmployeeName", userInfo.EMPLOYEE_NAME));
                    
                    //Add current organization ID to claims
                    claims.Add(new Claim("CurrentOrgId", userInfo.CURRENT_ORG_ID.ToString()));

                    return claims;
                }   
            
        } 

        /// <summary>
        /// Deletes the activity by activity id
        /// </summary>
        /// <param name="activityID"></param>
        public static void Delete(long activityID)
        {
            try
            {
                bool isNew;
                bool isUserNew;
                SYS_ACTIVITY activity = SYS_ACTIVITY.Activity(activityID, out isNew);
                SYS_USER_ACTIVITY userActivity = DBI.Data.SYS_USER_ACTIVITY.UserActivity(activityID, out isUserNew);
                if (!isNew)
                {
                    if (isUserNew)
                    {
                        GenericData.Delete<SYS_ACTIVITY>(activity);
                    }
                    else
                    {
                        Ext.Net.X.Msg.Alert(Resource.DeleteActivity, Resource.DeleteActivityAssignedToUser).Show();
                    }
                }
                else
                {
                    Ext.Net.X.Msg.Alert(Resource.DeleteActivity, Resource.DeleteActivityDoesNotExist).Show();
                }
            }
            catch (Exception e)
            {
                //string ErrorID;
                //DBI.Data.SYS_LOG.LogToDatabase(e, "DeleteActivity", out ErrorID);
                //throw new Exception(string.Format(Resource.DeleteActivity + " " + e.Message + " {0}", ErrorID));
            }
           
        }

        /// <summary>
        /// Save the activity that is edited or created.
        /// </summary>
        /// <param name="activity"></param>
        public static void Save(SYS_ACTIVITY activity)
        {
            try
            {
                if (activity.ACTIVITY_ID == 0)
                {
                    GenericData.Insert<SYS_ACTIVITY>(activity);
                }
                else
                {
                    GenericData.Update<SYS_ACTIVITY>(activity);
                }
            }
            catch (Exception e)
            {
                //string ErrorID;
                //DBI.Data.SYS_LOG.LogToDatabase(e, "SaveActivity", out ErrorID);
                //throw new Exception(string.Format(Resource.SaveActivity + " " + e.Message + " {0}", ErrorID));
            }
            

        }

        

    }
}
