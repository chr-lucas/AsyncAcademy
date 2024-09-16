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

namespace AsyncAcademy.Pages//.Accounts
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

        public RedirectToPageResult OnPost()
        {
            string enteredUsername = Account.Username;
            string enteredPassword = Account.Pass;
            var existingUsers = _context.Users.ToList();

            //searches for a specific user through firstOrDefault() based on the entered username. Returns null if no matches from query:
            var user = (from row in _context.Users where row.Username == enteredUsername select row).FirstOrDefault();

            //takes the specific user, that user's passd from db, and compares to entered pass:
            var passwordHasher = new PasswordHasher<User>();
            var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Pass, enteredPassword);

            //If verification fails, reload page:
            if (passwordVerification == PasswordVerificationResult.Failed)
                return RedirectToPage();

            //If verification succeeds, redirect to Welcome page:
            else
                return RedirectToPage("./welcome", new { id = user.Id });




            //For-each approach: 

            //foreach (User a in existingUsers)
            //{
            //    if (a.Username != enteredUsername)
            //    {
            //        continue; 
            //    }

            //    // Check if the provided password matches the hashed password
            //    var passwordHasher = new PasswordHasher<User>();
            //    var passwordVerification = passwordHasher.VerifyHashedPassword(a, a.Pass, enteredPassword);

            //    if (passwordVerification == PasswordVerificationResult.Success)
            //    {

            //        return RedirectToPage("./welcome", new { id = a.Id });
            //    }
            //}

            //// If no match, then reload page:
            //return RedirectToPage();


            //TEST COMMENT
        }
    }
}
