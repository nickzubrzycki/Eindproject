using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class NotificationController : Controller
    {
        // Ophalen van IdentityUser voor de User Id 
        private NotificationRepository NotificationRepository ;
        private IEnumerable<NotificationViewModel> NotificationViews;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationController(NotificationRepository notificationRepository,
            UserManager<ApplicationUser> userManager)
        {
            NotificationRepository = notificationRepository;
            _userManager = userManager;

        }


        public IActionResult GetAllNotifications()
        {
            
            var id = GetUserId();
            
            // Ga kijken in de database van de ToUserId en als als id false is 
            NotificationViews = NotificationRepository.GetNotifications().Where(n => id == n.ToUserId && n.IsRead == false).Select(n => new NotificationViewModel
            {
                 CreatedDate = n.CreatedDate,
                 BodyMessage = n.BodyMessage,
                 HeaderMessage = n.HeaderMessage, 
                 FromUserId = n.FromUserId,
                 ToUserId = n.ToUserId, 
                 Id = n.NotificationId, 
                 IsRead = n.IsRead, 
                 Url = n.Url

            });
            DeleteAllReadNotifications(NotificationViews, id);
            // Als ToUserId en isRead  = true dan gewoon checken of ouder dan een week

            return View(NotificationViews);
        }


        public string  GetUserId()
        {
            return _userManager.GetUserId(HttpContext.User);


        }

        public void DeleteAllReadNotifications(IEnumerable<NotificationViewModel> notificationViewModels,
            string id)
        {
            DateTime today = DateTime.Now;
            // Delete all the notifications that are older than 3 weeks 
            // The notifications need to be isRead == true 
            foreach(var notification in NotificationViews)
            {
                if(notification.IsRead == true 
                    && notification.ToUserId == id
                    && (today.Date.Day - notification.CreatedDate.Date.Day) > 7)
                {
                    NotificationRepository.DeleteNotification(notification.Id);
                }
            }
        }

        // Laten tonen van alle gelezen van user 
        public IActionResult ShowAllReadNotifications()
        {
            var id = GetUserId();
            var NotificationViews = NotificationRepository.GetNotifications().Where(n => id == n.ToUserId && n.IsRead == true).Select(n => new NotificationViewModel
            {
                CreatedDate = n.CreatedDate,
                BodyMessage = n.BodyMessage,
                HeaderMessage = n.HeaderMessage,
                FromUserId = n.FromUserId,
                ToUserId = n.ToUserId,
                Id = n.NotificationId,
                IsRead = n.IsRead,
                Url = n.Url

            });

            return View(NotificationViews);
        }

        
    }
}
