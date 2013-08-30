using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DBI.Data.DataFactory.Messages;

namespace DBI.Data
{
    public partial class SYS_USER_ACTIVITY
    {
        /// <summary>
        /// return a list of activities by userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<SYS_USER_ACTIVITY> ActivityByUserID(long userID)
        {
            Entities _context = new Entities();
            List<SYS_USER_ACTIVITY> activities = _context.SYS_USER_ACTIVITY.Include(a => a.SYS_ACTIVITY).Where(a => a.USER_ID == userID).ToList();
            return activities;
        }

        public static void Delete(long activityID)
        {
            bool isNew;
            SYS_USER_ACTIVITY userActivity = SYS_USER_ACTIVITY.UserActivity(activityID, out isNew);
            if (!isNew)
            {
                    GenericData.Delete<SYS_USER_ACTIVITY>(userActivity);
                    Ext.Net.X.Msg.Alert("Security Changed", "You have changed the basic security for this user, please have them log out and back in to access their new activities!").Show();
            }
             else
            {
                throw new Exception("");
            }
          
        }

        public static void Add(SYS_USER_ACTIVITY activity)
        {
            GenericData.Insert<SYS_USER_ACTIVITY>(activity);

            Ext.Net.X.Msg.Alert("Security Changed", "You have changed the basic security for this user, please have them log out and back in to access their new activities!").Show();
        }

        public static SYS_USER_ACTIVITY UserActivity(long userActivityID, out bool isNew)
        {
            using (Entities _context = new Entities())
            {
                //Check to see if there if this activity is assigned to a user activity
                SYS_USER_ACTIVITY activity = _context.SYS_USER_ACTIVITY.Where(a => a.USER_ACTIVITY_ID == userActivityID).FirstOrDefault();
                isNew = (activity == null);
                if (isNew)
                {
                    activity = new SYS_USER_ACTIVITY();
                }
                return activity;
            }
        }


    }
}
