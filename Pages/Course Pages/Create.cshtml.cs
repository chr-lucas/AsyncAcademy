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

namespace AsyncAcademy.Pages.Course_Pages
{
    public class CreateModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public CreateModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        public IActionResult OnGet()
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account == null)
            {
                return NotFound();
            }

            SetNavBarProperties();

            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;
        public User Account { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (Account == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            SetNavBarProperties();

            Course.InstructorId = Account.Id;

            _context.Course.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("InstructorIndex");
        }

        private void SetNavBarProperties()
        {
            if (Account.IsProfessor) 
            {
                NavBarLink = "InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
            }
            else
            {
                NavBarLink = "StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
            }
        }
    }
}
