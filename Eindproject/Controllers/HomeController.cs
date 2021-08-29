using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _data; 
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext data, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _data = data;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Profiel([FromRoute] string id)
        {
            var persoon = _data.Users.FirstOrDefault(u => u.Id == id);
            var vm = new ProfielViewModel
            {
                Voornaam = persoon.Voornaam,
                Achternaam = persoon.Achternaam,
                Email = persoon.Email
            };

            return View(vm);

            
        }
       
        public IActionResult UserSearch(string searchString)
        {
            // remove self from search 
            string uid = _userManager.GetUserId(HttpContext.User);

            if (String.IsNullOrEmpty(searchString))
            {
                return View();
            }

            var data = _data.Users.Where(u => u.UserName.Contains(searchString) && u.Id != uid);
            var vm = data.Select(u => new UserListViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                
            });
            var users = _data.Users.Where(u => u.UserName.Contains(searchString));
          

            return View(vm);
        }
        public IActionResult AddFriend(string bevriendid)
        {
            ApplicationUser vriend = _data.Users.FirstOrDefault(u => u.Id == bevriendid);

            ApplicationUser user = _data.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(HttpContext.User));

            _data.Vriend.Add(new Vriend { User = user, Bevriend = vriend, Accepted = false});
            _data.SaveChanges();
            return RedirectToAction(nameof(UserSearch));
        }
        public IActionResult ComfirmFriend(int vriendId)
        {
            
            _data.Vriend.FirstOrDefault(v => v.VriendId == vriendId).Accepted = true;
            _data.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFriend(int vriendId)
        {

            _data.Vriend.Remove(_data.Vriend.FirstOrDefault(f => f.VriendId == vriendId));
            _data.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult DoesUserEmailExist(string email)
        {

            var user = _data.Users.FirstOrDefault(u => u.Email == email);

            return Json(user == null);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
