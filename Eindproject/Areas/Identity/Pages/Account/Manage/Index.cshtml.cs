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

namespace Eindproject.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _data;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext data)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _data = data;
        }

        public string Username { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Voornaam { get; set; }
            public string Achternaam { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var temp = await _userManager.GetUserAsync(User);


            Input = new InputModel
            {
                PhoneNumber = temp.PhoneNumber,
                UserName = temp.UserName,
                Achternaam = temp.Achternaam,
                Voornaam = temp.Voornaam,
                Email = temp.Email,

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            _data.Users.FirstOrDefault(u => u.Id == user.Id).UserName = Input.UserName;
            _data.Users.FirstOrDefault(u => u.Id == user.Id).Voornaam = Input.Voornaam;
            _data.Users.FirstOrDefault(u => u.Id == user.Id).Achternaam = Input.Achternaam;
            _data.Users.FirstOrDefault(u => u.Id == user.Id).Email = Input.Email;
            _data.Users.FirstOrDefault(u => u.Id == user.Id).PhoneNumber = Input.PhoneNumber;
            _data.SaveChanges();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
