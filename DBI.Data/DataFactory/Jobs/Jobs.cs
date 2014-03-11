using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;

namespace DBI.Data
{
    public sealed class Jobs
    {

        public static void ProcessMobileNotifications(object state)
        {
            PushBroker push = new PushBroker();

           //Wire up the events for all the services that the broker registers
           push.OnNotificationSent += NotificationSent;
           push.OnChannelException += ChannelException;
           push.OnServiceException += ServiceException;
           push.OnNotificationFailed += NotificationFailed;
           push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
           push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
           push.OnChannelCreated += ChannelCreated;
           push.OnChannelDestroyed += ChannelDestroyed;

           MemoryStream ms = new MemoryStream();
           foreach (string resource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
           {
               if (resource.EndsWith("PushSharp.PushCert.Production.p12"))
               {
                   Stream ProductionCertificate = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                   ProductionCertificate.CopyTo(ms);
               }
           }

           var appleCert = ms.ToArray();
           push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, "Dbi18201")); //Extension method


           List<SYS_MOBILE_NOTIFICATIONS> notifications = DBI.Data.SYS_MOBILE_NOTIFICATIONS.unprocessedNotifications();

           if (notifications.Count > 0)
           {

               foreach (SYS_MOBILE_NOTIFICATIONS notification in notifications)
               {
                   push.QueueNotification(new AppleNotification()
                              .ForDeviceToken(notification.DEVICE_ID)
                              .WithAlert(notification.MESSAGE)
                              .WithSound(notification.SOUND)
                              .WithTag(notification.NOTIFICATION_ID));
               }

           }

           //push.StopAllServices(true);

       }

		static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
		{
			//Currently this event will only ever happen for Android GCM
			//Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
		}

		static void NotificationSent(object sender, INotification notification)
		{
            int token = Convert.ToInt16(notification.Tag.ToString().Trim());
            DBI.Data.SYS_MOBILE_NOTIFICATIONS.UpdateNotificationToProcessed(token);
			//Console.WriteLine("Sent: " + sender + " -> " + notification);  
		}

		static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
		{
			//Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
            int token = Convert.ToInt16(notification.Tag.ToString().Trim());
            DBI.Data.SYS_MOBILE_NOTIFICATIONS.UpdateNotificationToProcessed(token, notificationFailureException.Message);
		}

		static void ChannelException(object sender, IPushChannel channel, Exception exception)
		{
			//Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void ServiceException(object sender, Exception exception)
		{
			//Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
		{
            //Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);

		}

		static void ChannelDestroyed(object sender)
		{
			//Console.WriteLine("Channel Destroyed for: " + sender);
		}

		static void ChannelCreated(object sender, IPushChannel pushChannel)
		{
			//Console.WriteLine("Channel Created for: " + sender);
		}
        }

    }
