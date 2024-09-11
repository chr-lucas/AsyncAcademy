using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstLastApp.Data;
using FirstLastApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstLastApp.Pages.Accounts
{
    public class WelcomeModel(FirstLastApp.Data.FirstLastAppContext context) : PageModel
    {
        private FirstLastApp.Data.FirstLastAppContext _context = context;
        private int _id;

        [BindProperty]
        public User? Account { get; set; }

        [ViewData]
        public string WelcomeText { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _context.Account.FirstOrDefaultAsync(a => a.Id == id);

            if (Account == null)
            {
                return NotFound();
            }

            var firstname = Account.FirstName;
            var lastname = Account.LastName;
            WelcomeText = $"Welcome, {firstname} {lastname}";

            return Page();
        }

    }
}
