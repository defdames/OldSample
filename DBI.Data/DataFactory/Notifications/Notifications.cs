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
                List<SYS_MOBILE_NOTIFICATIONS> notifications = _context.SYS_MOBILE_NOTIFICATIONS.Where(n => !n.PROCESSED_DATE.HasValue).OrderBy(o =>o.DEVICE_ID).ToList();
                return notifications;
            }
        }


    }
}
