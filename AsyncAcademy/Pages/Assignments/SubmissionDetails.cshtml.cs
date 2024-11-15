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
        public string submissionFilePath;

        public Notification notification = new Notification();

        public async Task<IActionResult> OnGetAsync(int? id) // Check to see what this id corresponds to
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Course course = await _context.Course.FirstOrDefaultAsync(a => (a.id == id));
            Submission = await _context.Submissions.FirstOrDefaultAsync(a => (a.id == id));
            
            if (Submission == null)
            {
                return NotFound();
            }

            Student = _context.Users.FirstOrDefault(u => (u.Id == Submission.UserId));
            Assignment = _context.Assignment.FirstOrDefault(a => a.Id == Submission.AssignmentId);
            submissionFilePath = "." + Submission.Content.ToString();

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
                notification.UserId = Submission.UserId;
                notification.AssignmentId = Submission.AssignmentId;
                notification.Content = "Your instructor has updated a grade for " + Assignment.Title;
                _context.Update(Submission);
                _context.Notification.Add(notification);
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
