using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Course_Pages
{
    public class EnrollModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public EnrollModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(int courseId)
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            // Check if enrollment already exists
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == currentUserID && e.CourseId == courseId);
            if (existingEnrollment != null)
            {
                return RedirectToPage("./StudentIndex");
            }

            // Create new enrollment
            var enrollment = new Enrollment
            {
                UserId = currentUserID.Value,
                CourseId = courseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./StudentIndex");
        }
    }
}
