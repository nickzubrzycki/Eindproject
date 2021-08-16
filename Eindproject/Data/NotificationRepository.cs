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
            throw new NotImplementedException();
        }

        public void DeleteNotification(int Id)
        {
            throw new NotImplementedException();
        }

        public Notification GetNotification(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Notification> GetNotifications()
        {
            throw new NotImplementedException();
        }

        public void UpdateNotification(int Id, Notification notification)
        {
            throw new NotImplementedException();
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
