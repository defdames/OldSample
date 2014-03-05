using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class Utility
    {
        public static string requestHeaderByValue(string headerID, HttpRequestMessage message)
        {
            IEnumerable<string> headerValues = message.Headers.GetValues(headerID);
            var id = headerValues.FirstOrDefault();
            return id.ToString();
        }

        public static void registerDeviceForNotifications(string deviceID)
        {

            if(!string.IsNullOrEmpty(deviceID))
            {
                 //Check the system for the registered device and if it's not saved to the database
                using (Entities _context = new Entities())
                {
                    //search the table for the device if not found add it.
                    SYS_MOBILE_DEVICES device = _context.SYS_MOBILE_DEVICES.Where(d => d.DEVICE_ID == deviceID).SingleOrDefault();

                    //Device was found update the last activity date
                    if (device != null)
                    {
                        device.LAST_ACTIVITY_DATE = DateTime.Now;
                        GenericData.Update<SYS_MOBILE_DEVICES>(device);
                    }
                    else
                    {
                        SYS_MOBILE_DEVICES newDevice = new SYS_MOBILE_DEVICES();
                        newDevice.DEVICE_ID = deviceID;
                        newDevice.DATE_CREATED = DateTime.Now;
                        newDevice.LAST_ACTIVITY_DATE = newDevice.DATE_CREATED;
                        GenericData.Insert<SYS_MOBILE_DEVICES>(newDevice);
                    }
  
                }

            }

        }

    }
}