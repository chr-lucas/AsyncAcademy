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
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Check if username or password is empty
            if (string.IsNullOrEmpty(Account.Username) || string.IsNullOrEmpty(Account.Pass))
            {
                ModelState.AddModelError(string.Empty, "Username and password are required to login.");
                return Page();
            }

            string enteredUsername = Account.Username;
            string enteredPassword = Account.Pass;
            var existingUsers = _context.Users.ToList();

            // Searches for a specific user through FirstOrDefault() based on the entered username. Returns null if no matches from query:
            var user = (from row in _context.Users where row.Username == enteredUsername select row).FirstOrDefault();

            // If user is not found, reload page with error message
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            // Takes the specific user, that user's password from db, and compares to entered password:
            var passwordHasher = new PasswordHasher<User>();
            var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Pass, enteredPassword);

            // If verification fails, reload page with error message
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            // If verification succeeds, redirect to Welcome page:
            HttpContext.Session.SetInt32("CurrentUserId", user.Id);
            return RedirectToPage("./welcome", new { id = user.Id });
        }
    }
}
