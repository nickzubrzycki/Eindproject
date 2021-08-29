using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Eindproject.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Eindproject.Data;

namespace Eindproject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _data;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext data)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _data = data;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [Remote("DoesUserEmailExist", "Home", HttpMethod = "POST", ErrorMessage = "Email address already exists. Please enter a different Email address.")]
            public string Email { get; set; }
            [Required]           
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {                
                var user = new ApplicationUser { UserName = Input.UserName, Email = Input.Email, Voornaam = Input.FirstName, Achternaam = Input.LastName, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    //Hier stond de Code voor email comfirm aangezien we geen mailserver gaan gebruiken mag dit weg                  
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    _data.Lijsts.Add(new Lijst { UserId = user.Id });
                    _data.SaveChanges();
                    StatusMessage = "";
                    return RedirectToAction("Index", "Home");
                }                                      
            }
            // If we got this far, something failed, redisplay form
            StatusMessage = "Error: Email address already exists. Please enter a different email or log in.";
            return Page();
        }
    }
}
