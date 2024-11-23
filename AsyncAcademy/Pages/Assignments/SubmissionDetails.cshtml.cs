using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Models;
using AsyncAcademy.Utils;
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

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        [ViewData]
        public List<object> notos { get; set; }

        public Assignment Assignment { get; set; }
        public User Student { get; set; }
        public string submissionFilePath;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            var Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);


            if (Account.IsProfessor)
            {
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }
            else
            {
                NavBarLink = "Course Pages/StudentIndex";
                NavBarText = "Register";
                NavBarAccountTabLink = "/Account";
                NavBarAccountText = "Account";

                notos = new List<object>();
                List<Submission> notifications = await _context.Submissions
                    .Where(e => e.UserId == currentUserID)
                    .Where(n => n.IsNew == true)
                    .ToListAsync();

                if (notifications.Count > 0)
                {
                    Noto notoController = new Noto();
                    notoController.SetViewData(ViewData, notifications.Count);
                    foreach (Submission notification in notifications)
                    {
                        List<object> result = notoController.NotoData(_context, notification);
                        notos.Add(result);
                    }
                }

            }

            Submission = await _context.Submissions.FirstOrDefaultAsync(a => (a.Id == id));
            
            if (Submission == null)
            {
                return NotFound();
            }

            Student = await _context.Users.FirstOrDefaultAsync(u => (u.Id == Submission.UserId));
            Assignment = await _context.Assignment.FirstOrDefaultAsync(a => a.Id == Submission.AssignmentId);
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
                Submission.IsNew = true; // Flag submission for notification
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
