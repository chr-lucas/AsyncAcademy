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
        public Section NewSection { get; set; } = default!;

        [ViewData]
        public string NavBarLink { get; set; } = "/SectionSignup";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        public IActionResult OnGet()
        {
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

            if (account.IsProfessor)
            {
                // Set ViewData variables for instructors
                ViewData["NavBarLink"] = "/CreateSection";
                ViewData["NavBarText"] = "Classes";
            }

            NewSection.InstructorId = userId.Value; // Set the instructor ID to the current user's ID

            _context.Sections.Add(NewSection);
            await _context.SaveChangesAsync();

            // Fill in placeholder class cards
            for (int i = 1; i <= 4; i++) 
            {
                _context.Enrollments.Add(new Enrollment { SectionId = NewSection.CourseId, UserId = userId.Value });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Welcome", new { id = userId }); // Redirect to the welcome page
        }
    }
}
