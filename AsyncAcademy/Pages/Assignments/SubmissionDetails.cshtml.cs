using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmissionDetailsModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public SubmissionDetailsModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Submission Submission { get; set; }
      
        public Assignment Assignment { get; set; }
        public User Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Submission = await _context.Submissions.FirstOrDefaultAsync(a => (a.Id == id));
            
            if (Submission == null)
            {
                return NotFound();
            }

            Student = await _context.Users.FirstOrDefaultAsync(u => (u.Id == Submission.UserId));
            Assignment = await _context.Assignment.FirstOrDefaultAsync(a => a.Id == Submission.AssignmentId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _context.Update(Submission);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            // Return to assignment details view
            return RedirectToPage("/Assignments/Details", new { id = Submission.AssignmentId });
        }
    }
}
