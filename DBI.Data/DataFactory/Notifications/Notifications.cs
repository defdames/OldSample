using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_MOBILE_NOTIFICATIONS
    {

        public static List<SYS_MOBILE_NOTIFICATIONS> unprocessedNotifications()
        {
            using (Entities _context = new Entities())
            {
                // Get a list of unprocessed notifications sort by device id
                List<SYS_MOBILE_NOTIFICATIONS> notifications = _context.SYS_MOBILE_NOTIFICATIONS.Where(n => !n.PROCESSED_DATE.HasValue).OrderBy(o => o.DEVICE_ID).ToList();
                return notifications;
            }
        }

        public static void UpdateNotificationToProcessed(int id, string error = null)
        {

            using (Entities _context = new Entities())
            {
                SYS_MOBILE_NOTIFICATIONS notification = _context.SYS_MOBILE_NOTIFICATIONS.Where(n => n.NOTIFICATION_ID == id).Single();

                if (notification != null)
                {
                    //update the notification to processed
                    notification.PROCESSED_DATE = DateTime.Now;
                    notification.MESSAGE = error;
                    GenericData.Update<SYS_MOBILE_NOTIFICATIONS>(notification);
                }
            }
        }

    }
       
}
