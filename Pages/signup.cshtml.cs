using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstLastApp.Data;
using FirstLastApp.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FirstLastApp.Pages
{
    public class SignupModel(FirstLastAppContext context) : PageModel
    {
        private readonly FirstLastAppContext _context = context;

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User Account { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
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
