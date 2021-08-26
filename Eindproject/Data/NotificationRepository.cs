using Eindproject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public NotificationRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public void AddNotification(Notification notification)
        {
            applicationDbContext.Notifications.Add(notification); 
            applicationDbContext.SaveChanges();
        }

        public void DeleteNotification(int Id)
        {
            var notification = applicationDbContext.Notifications.SingleOrDefault(m => m.NotificationId == Id);

            if (notification != null)
            {
                applicationDbContext.Notifications.Remove(notification);
                applicationDbContext.SaveChanges();
            }
        }

        public Notification GetNotification(int Id)
        {
            return applicationDbContext.Notifications.SingleOrDefault(n => n.NotificationId == Id);
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return applicationDbContext.Notifications;
        }

        public void UpdateNotification(int Id, Notification notification)
        {
            var notificationDb = applicationDbContext.Notifications.SingleOrDefault(n => n.NotificationId == Id);

            if(notificationDb != null)
            {
                notificationDb.FromUserId = notification.FromUserId;
                notificationDb.ToUserId = notification.ToUserId;
                notificationDb.HeaderMessage = notification.HeaderMessage;
                notificationDb.BodyMessage = notification.BodyMessage;
                notificationDb.Url = notificationDb.Url;
                notificationDb.IsRead = notification.IsRead;
                notificationDb.Url = notification.Url;
                notificationDb.CreatedDate = notification.CreatedDate;
                applicationDbContext.SaveChanges();

            }
        }
    }

    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNotifications();

        Notification GetNotification(int Id);

        void AddNotification(Notification notification);

        void UpdateNotification(int Id, Notification notification);

        void DeleteNotification(int Id);
    }
}
