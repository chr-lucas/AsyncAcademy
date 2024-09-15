using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstLastApp.Data;
using FirstLastApp.Models;
using Microsoft.AspNetCore.Identity;

namespace FirstLastApp.Pages
{
    public class signupModel : PageModel
    {
        private readonly FirstLastAppContext _context;

        public signupModel(FirstLastAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User Account { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            //Create passwordHasher variable that will take data type "User"
            var passwordHasher = new PasswordHasher<User>();

            //Hash the actual password (salt already added on the hasher):
            Account.Pass = passwordHasher.HashPassword(Account, Account.Pass);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Account.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./welcome", new { id = Account.Id });
        }
    }
}

//TEST COMMENT