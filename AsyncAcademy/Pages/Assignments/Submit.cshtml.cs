using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmitModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public SubmitModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Assignment Assignment { get; set; } = default!;

        [BindProperty]
        public Submission Submission { get; set; } = new Submission();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Assignment = await _context.Assignment.FirstOrDefaultAsync(a => a.Id == id);
            if (Assignment == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // <----- Never gets beyond this check
            {
                return Page();
            }

            Submission.AssignmentId = Assignment.Id; // Assuming you have an AssignmentId in your Submission model
            //New Submission fields added by Chris
            if (HttpContext.Session.GetInt32("CurrentUserId") != null)
            {
                Submission.UserId = (int)HttpContext.Session.GetInt32("CurrentUserId");
            }
            Submission.Timestamp = DateTime.Now;
            //
            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Assignments/Details", new { id = Assignment.Id });
        }
    }
}
