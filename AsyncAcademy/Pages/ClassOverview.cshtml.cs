// All Kevins code. If you change code, please comment

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages
{
    public class ClassOverviewModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public ClassOverviewModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        public User Account { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);

            if (Course == null)
            {
                return NotFound();
            }

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

             if (Account.IsProfessor) 
            {
                //WelcomeText = $"Welcome, Professor {firstname} {lastname}";
                // Set ViewData variables for instructors
                NavBarLink = "Course Pages/InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
            }
            else
            {
                //WelcomeText = $"Welcome, {firstname} {lastname}";
                NavBarLink = "Course Pages/StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
            }

            return Page();
        }
    }
}
