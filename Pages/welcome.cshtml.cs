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

namespace AsyncAcademy.Pages//.Accounts
{
    public class WelcomeModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;
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

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

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
