using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AsyncAcademy.Pages
{
    public class CourseGradesModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public CourseGradesModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public List<Submission> Submissions = new List<Submission>();
        public List<Assignment> CorrespondingAssignments = new List<Assignment>();

        public Course Course { get; set; }

        public int OverallGrade = 0;

        public async Task<IActionResult> OnGetAsync(int courseId) {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Submissions = _context.Submissions.Where(a => a.UserId == currentUserID).ToList();
            foreach (var submission in Submissions) {
                CorrespondingAssignments.Add(await _context.Assignment.FirstOrDefaultAsync(a => a.Id == submission.AssignmentId));
            }
            int i = 0;
            foreach (var submission in Submissions) {
                if (submission.PointsGraded == null)
                {
                    continue;
                }
                OverallGrade += (int)submission.PointsGraded;
                i++;
            }
            if (i == 0)
            {
                OverallGrade = -1;
            }
            else
            {
                OverallGrade /= i;
            }

            return Page();
        }
    }
}
