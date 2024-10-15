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

        public int OverallMaximumPoints = 0;

        public async Task<IActionResult> OnGetAsync(int courseId) {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Submissions = _context.Submissions.Where(a => a.UserId == currentUserID).ToList();
            foreach (var submission in Submissions) {
                CorrespondingAssignments.Add(await _context.Assignment.FirstOrDefaultAsync(a => a.Id == submission.AssignmentId));
            }
            for (int i = 0; i < Submissions.Count; i++) {
                Submission submission = Submissions[i];
                Assignment assignment = CorrespondingAssignments[i];
                if (submission.PointsGraded == null)
                {
                    continue;
                }
                OverallGrade += (int)submission.PointsGraded;
                OverallMaximumPoints += (int)assignment.MaxPoints;
            }
            if (OverallMaximumPoints == 0)
            {
                OverallGrade = -1;
            }
            else
            {
                OverallGrade /= OverallMaximumPoints;
                OverallGrade *= 100;
            }

            return Page();
        }
    }
}
