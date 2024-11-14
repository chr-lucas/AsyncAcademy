using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Course_Pages
{
    public class DeleteModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public DeleteModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            var Account = _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account.IsProfessor)
            {
                NavBarLink = "InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }
            else
            {
                NavBarLink = "StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
                NavBarAccountTabLink = "/Account";
                NavBarAccountText = "Account";
            }

            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            else
            {
                Course = course;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                Course = course;
                _context.Course.Remove(Course);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
