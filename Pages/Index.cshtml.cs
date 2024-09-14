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
    public class LoginModel : PageModel
    {
        private readonly FirstLastApp.Data.FirstLastAppContext _context;

        public LoginModel(FirstLastApp.Data.FirstLastAppContext context)
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
            string user = Account.Username;
            string pass = Account.Pass;
            var existingUsers = _context.Account.ToList();
            foreach (User a in existingUsers)
            {
                if (!(a.Username == user)) { continue; }
                else
                {
                    if (a.Pass == pass)
                    {
                        return RedirectToPage("./welcome", new { id = a.Id });
                    }
                }
            }

            return RedirectToPage();
        }
    }
}
