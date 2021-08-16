using Eindproject.Data;
using Eindproject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class NotificationController : Controller
    {
        // Ophalen van data
        // Zien of ze vrienden zijn 
        private ApplicationDbContext applicationDbContext;
        private List<NotificationViewModel> NotificationViews;
        public NotificationController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;

        }


        public IActionResult GetAllNotifications()
        {
            // Ga kijken in de database van de ToUserId en als als id false is 
            NotificationViews = applicationDbContext.Notifications.Select(n => new NotificationViewModel
            {

            }); 

            // Als ToUserId en isRead  = true dan gewoon checken of ouder dan een week
            // Dan verwijderen
            // Dan alle alle notifications and set them in a ViewModel 
            // Return dat ViewModel 
        }

    }
}
