using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmissionDetailsModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public SubmissionDetailsModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        public Submission Submission { get; set; }
        public Assignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Submission = await _context.Submissions.FirstOrDefaultAsync(a => (a.Id == id));
            
            if (Submission == null)
            {
                return NotFound();
            }

            Assignment = await _context.Assignment.FirstOrDefaultAsync(a => a.Id == Submission.AssignmentId);

            return Page();
        }
    }
}
