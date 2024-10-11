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
        public List<Submission> previousSubmissions; //tracks previous submissions of given assignment - Hanna w
        public int? currentGrade; //tracks current grade of given assignment  - Hanna w

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

            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                NotFound();
            }

            //gathers any previous submissions for the user - Hanna w
            previousSubmissions = [];
            var submittedAssignments = _context.Submissions.Where(a => a.UserId == currentUserID);

            //searches through user submissions for assignment being viewed - Hanna w
            foreach (var s in submittedAssignments)
            {
                if (s.AssignmentId == Assignment.Id)
                {
                    previousSubmissions.Add(s);

                    //checks if assignment has already been graded - Hanna w
                    if (s.PointsGraded >= 0 && currentGrade == null)
                    {
                        //updates current grade - Hanna w
                        currentGrade = s.PointsGraded;
                        break;
                    }
                    if (s.PointsGraded >= 0 && currentGrade < s.PointsGraded)
                    {
                        currentGrade = s.PointsGraded;
                        break;
                    }
                }
            }

            //gather chart data - Hanna w
            var allSubmissions = _context.Submissions.Where(a => a.AssignmentId == id);
            int? min = allSubmissions.Min(a => a.PointsGraded);
            int? max = allSubmissions.Max(a => a.PointsGraded);
            int? average = (int?)allSubmissions.Average(a => a.PointsGraded);

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
