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

        private Enrollment generatePlaceholderCardEnrollment(int sectionId, int userId)
        {
            Enrollment newEnrollment = new Enrollment();
            newEnrollment.SectionId = sectionId;
            newEnrollment.UserId = userId;
            return newEnrollment;
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Part of stopgap solution for implementing different user groups
            Account.IsProfessor = (accountType == "professor"); 

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

            // Fill in placeholder class cards
            var existingUsers = _context.Users.ToList();
            var user = (from row in _context.Users where row.Username == Account.Username select row).FirstOrDefault();
            int accountId = user.Id;
            for (int i = 1; i <= 4; i++) 
            {
                _context.Enrollments.Add(generatePlaceholderCardEnrollment(i, accountId));
            }

            _context.SaveChanges();

            return RedirectToPage("./welcome", new { id = Account.Id });
        }
    }
}

//TEST COMMENT