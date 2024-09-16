using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Identity;

namespace AsyncAcademy.Pages
{
    public class SignupModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public SignupModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User Account { get; set; } = default!;

        [BindProperty]
        public string accountType { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Account.IsProfessor = (accountType == "professor"); // Part of stopgap solution for implementing different user groups
            System.Diagnostics.Debug.WriteLine("Account type string is " + accountType);

            //Create passwordHasher variable that will take data type "User"
            var passwordHasher = new PasswordHasher<User>();

            //Hash the actual password (salt already added on the hasher):
            Account.Pass = passwordHasher.HashPassword(Account, Account.Pass);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./welcome", new { id = Account.Id });
        }
    }
}

//TEST COMMENT