using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RoleCreateViewModel
            {
                Users = Userlist(),
                UserRoles = UserRoles()
            };
            return View(vm);
        }

        [HttpPost] 
        public async Task<IActionResult> Create(RoleCreateViewModel vm)
        {
            if (TryValidateModel(vm))
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = vm.Name,

                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("create", "administration");
                }
            }

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserRole(RoleCreateViewModel vm)
        {          
            var user = userManager.Users.FirstOrDefault(u => u.Id == vm.UserId);

            var roleName = roleManager.Roles.FirstOrDefault(u => u.Id == vm.UserRoleId).Name;

            await userManager.AddToRoleAsync(user, roleName);
            
            return RedirectToAction("create", "administration");
        }
        private List<SelectListItem> Userlist()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in userManager.Users)
            {
                list.Add(new SelectListItem { Value = item.Id, Text = item.UserName });
            }

            return list;
        }
        private List<SelectListItem> UserRoles()
        {

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in roleManager.Roles)
            {
                list.Add(new SelectListItem { Value = item.Id, Text = item.Name });
            }

            return list;
        }
    }
}
