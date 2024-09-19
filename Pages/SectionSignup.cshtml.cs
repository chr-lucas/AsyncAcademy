using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using System.Threading.Tasks;
using System.Linq;

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
            // Here is a placeholder example:
            int userId = 1; // Replace with actual logic to get the current user's ID

            NewSection.InstructorId = userId; // Set the instructor ID to the current user's ID

            _context.Sections.Add(NewSection);
            await _context.SaveChangesAsync();

            // Fill in placeholder class cards
            for (int i = 1; i <= 4; i++) 
            {
                _context.Enrollments.Add(new Enrollment { SectionId = NewSection.Id, UserId = userId });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Welcome", new { id = userId }); // Redirect to the welcome page
        }
    }
}
