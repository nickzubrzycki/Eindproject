using Eindproject.Data;
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
        private readonly UserManager<IdentityUser> _userManager;
        public NotificationController(NotificationRepository notificationRepository,
            UserManager<IdentityUser> userManager)
        {
            NotificationRepository = notificationRepository;
            _userManager = userManager;

        }


        public IActionResult GetAllNotifications()
        {
            
            var id = int.Parse(GetUserId().Result);
            
            // Ga kijken in de database van de ToUserId en als als id false is 
            NotificationViews = NotificationRepository.GetNotifications().Where(n => id == n.Id && n.IsRead == false).Select(n => new NotificationViewModel
            {
                 CreatedDate = n.CreatedDate,
                 BodyMessage = n.BodyMessage,
                 HeaderMessage = n.HeaderMessage, 
                 FromUserId = n.FromUserId,
                 ToUserId = n.ToUserId, 
                 Id = n.Id, 
                 IsRead = n.IsRead, 
                 Url = n.Url

            });
            DeleteAllReadNotifications(NotificationViews, id);
            // Als ToUserId en isRead  = true dan gewoon checken of ouder dan een week

            return View(NotificationViews);
        }


        public async Task<string> GetUserId()
        {
           var user =  await _userManager.GetUserAsync(User);
           return user.Id;

        }

        public void DeleteAllReadNotifications(IEnumerable<NotificationViewModel> notificationViewModels,
            int Id)
        {
            DateTime today = DateTime.Now;
            // Delete all the notifications that are older than 3 weeks 
            // The notifications need to be isRead == true 
            foreach(var notification in NotificationViews)
            {
                if(notification.IsRead == true 
                    && notification.ToUserId == Id 
                    && (today.Date.Day - notification.CreatedDate.Date.Day) > 7)
                {
                    NotificationRepository.DeleteNotification(notification.Id);
                }
            }
        }

        // Laten tonen van alle gelezen van user 
        public IActionResult ShowAllReadNotifications()
        {
            var id = int.Parse(GetUserId().Result);
            var NotificationViews = NotificationRepository.GetNotifications().Where(n => id == n.Id && n.IsRead == true).Select(n => new NotificationViewModel
            {
                CreatedDate = n.CreatedDate,
                BodyMessage = n.BodyMessage,
                HeaderMessage = n.HeaderMessage,
                FromUserId = n.FromUserId,
                ToUserId = n.ToUserId,
                Id = n.Id,
                IsRead = n.IsRead,
                Url = n.Url

            });

            return View(NotificationViews);
        }

        
    }
}
