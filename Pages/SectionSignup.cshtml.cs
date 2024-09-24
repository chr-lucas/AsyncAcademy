using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AsyncAcademy.Pages
{
    public class SectionSignupModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public SectionSignupModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course NewCourse { get; set; } = default!;

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        public async Task<IActionResult> OnGetAsync()
        {
            // Assuming you can get the user ID from the session or other mechanism
            int? userId = HttpContext.Session.GetInt32("CurrentUserId");

            if (userId == null)
            {
                return NotFound();
            }

            // Retrieve the current user's account
            var account = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);

            if (account == null)
            {
                return NotFound();
            }

            // Set ViewData variables based on user role
            if (account.IsProfessor)
            {
                NavBarLink = "/CreateSection"; // Set NavBarLink for professors
                NavBarText = "Classes"; // Set NavBarText for professors
            }
            else
            {
                NavBarLink = "/SectionSignup"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Assuming you can get the user ID from the session or other mechanism
            int? userId = HttpContext.Session.GetInt32("CurrentUserId");

            if (userId == null)
            {
                return NotFound();
            }

            // Retrieve the current user's account
            var account = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);

            if (account == null)
            {
                return NotFound();
            }

            NewCourse.InstructorId = userId.Value; // Set the instructor ID to the current user's ID

            _context.Course.Add(NewCourse);
            await _context.SaveChangesAsync();

            // Fill in placeholder class cards
            for (int i = 1; i <= 4; i++) 
            {
                _context.Enrollments.Add(new Enrollment { CourseId = NewCourse.Id, UserId = userId.Value });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Welcome", new { id = userId }); // Redirect to the welcome page
        }
    }
}
