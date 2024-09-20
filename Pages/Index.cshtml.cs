using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AsyncAcademy.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public LoginModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User Account { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Account.Username) || string.IsNullOrEmpty(Account.Pass))
            {
                ErrorMessage = "Username and password are required to login.";
                return Page();
            }

            string enteredUsername = Account.Username;
            string enteredPassword = Account.Pass;
            var existingUsers = _context.Users.ToList();

            var user = (from row in _context.Users where row.Username == enteredUsername select row).FirstOrDefault();

            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Pass, enteredPassword);

            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            HttpContext.Session.SetInt32("CurrentUserId", user.Id);
            return RedirectToPage("./welcome");
        }
    }
}
