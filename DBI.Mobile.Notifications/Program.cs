using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;


namespace DBI.Mobile.Notifications
{
    class Program
    {
       static void Main(string[] args)
		{		
			//Create our push services broker
			var push = new PushBroker();

			//Wire up the events for all the services that the broker registers
			push.OnNotificationSent += NotificationSent;
			push.OnChannelException += ChannelException;
			push.OnServiceException += ServiceException;
			push.OnNotificationFailed += NotificationFailed;
			push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
			push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
			push.OnChannelCreated += ChannelCreated;
			push.OnChannelDestroyed += ChannelDestroyed;

			var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PushSharp.PushCert.Production.p12"));
            
            push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, "Dbi18201")); //Extension method


            List<SYS_MOBILE_NOTIFICATIONS> notifications = DBI.Data.SYS_MOBILE_NOTIFICATIONS.unprocessedNotifications();

            foreach (SYS_MOBILE_NOTIFICATIONS notification in notifications)
            {
                push.QueueNotification(new AppleNotification()
                           .ForDeviceToken(notification.DEVICE_ID)
                           .WithAlert(notification.MESSAGE)
                           .WithSound(notification.SOUND)
                           .WithTag(notification.NOTIFICATION_ID));
            }

			Console.WriteLine("Waiting for Queue to Finish...");

			//Stop and wait for the queues to drains
			push.StopAllServices();
		}


		static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
		{
			//Currently this event will only ever happen for Android GCM
			Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
		}

		static void NotificationSent(object sender, INotification notification)
		{
			Console.WriteLine("Sent: " + sender + " -> " + notification);

            int token = Convert.ToInt16(notification.Tag.ToString().Trim());
            DBI.Data.SYS_MOBILE_NOTIFICATIONS.UpdateNotificationToProcessed(token);
  
		}

		static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
            int token = Convert.ToInt16(notification.Tag.ToString().Trim());
            DBI.Data.SYS_MOBILE_NOTIFICATIONS.UpdateNotificationToProcessed(token, notificationFailureException.Message);
		}

		static void ChannelException(object sender, IPushChannel channel, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void ServiceException(object sender, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
		{
            Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);

		}

		static void ChannelDestroyed(object sender)
		{
			Console.WriteLine("Channel Destroyed for: " + sender);
		}

		static void ChannelCreated(object sender, IPushChannel pushChannel)
		{
			Console.WriteLine("Channel Created for: " + sender);
		}
	}

}
