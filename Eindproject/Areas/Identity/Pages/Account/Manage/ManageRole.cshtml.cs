using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Eindproject.Data;
using Eindproject.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eindproject.Areas.Identity.Pages.Account.Manage
{
    public class ManageRoleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _data;
        private readonly RoleManager<IdentityRole> _roleManager;


        public ManageRoleModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext data, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _data = data;
            _roleManager = roleManager;
            Users = Userlist();
            UserRoles = UserRoleList();
        }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> UserRoles { get; set; }
        public class InputModel
        {

            [Required]
            public string Name { get; set; }
            public string UserId { get; set; }
            public string UserRoleId { get; set; }
            

        }
        private async Task LoadAsync(ApplicationUser user)
        {          
            await _userManager.GetUserAsync(User);          
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == Input.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var roleName = _roleManager.Roles.FirstOrDefault(u => u.Id == Input.UserRoleId).Name;

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                StatusMessage = $"Role {roleName} has been given to {user.UserName}";
                return RedirectToPage();
            }
            return RedirectToPage();
           
        }
        private List<SelectListItem> Userlist()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in _userManager.Users)
            {
                list.Add(new SelectListItem { Value = item.Id, Text = item.UserName });
            }

            return list;
        }
        private List<SelectListItem> UserRoleList()
        {

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in _roleManager.Roles)
            {
                list.Add(new SelectListItem { Value = item.Id, Text = item.Name });
            }

            return list;
        }
    }
}

