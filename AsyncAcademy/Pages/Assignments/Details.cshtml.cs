using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Utils;
using AsyncAcademy.Models;

//Page to display details of an assignment for a course
//Code below written by Hanna Whitney unless notated otherwise

namespace AsyncAcademy.Pages.Assignments
{
    public class DetailsModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public DetailsModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        public Assignment Assignment { get; set; } = default!;

        public User? Account { get; set; }

        public Course Course { get; set; }

        public List<Submission> Submissions = new List<Submission>();

        public List<Assignment> CorrespondingAssignment = new List<Assignment>();

        public List<String> Names = new List<String>();
        public List<int?> Grades = new List<int?>(); 

        // Counter variables for chart data
        public int numA = 0;
        public int numB = 0;
        public int numC = 0;
        public int numD = 0;
        public int numF = 0;
        public int numUG = 0;
        public int numSub = 0;
        public int numNotSub = 0;
        public int maxPoints = 0;

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

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            //assigns user - Assisted by Chris L.
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (Account == null) {
                return NotFound();
            }

            // Pull all submissions for assignment
            Submissions = _context.Submissions.Where(a => (a.UserId == currentUserID || Account.IsProfessor) && a.AssignmentId == id).ToList();
            foreach (var submission in Submissions) {
                CorrespondingAssignment.Add(_context.Assignment.First(a => a.Id == submission.AssignmentId));
                User user = _context.Users.First(a => a.Id == submission.UserId);
                Names.Add(user.LastName + "," + user.FirstName);
                Grades.Add(submission.PointsGraded);
            }

            if (Account == null)
            {
                return NotFound();
            }

            //sets navbar
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

            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }
            else
            {
                Assignment = assignment;
                maxPoints = Assignment.MaxPoints;
            }

            //assigns course - Assisted by Kevin B.
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);

            if (Course == null)
            {
                NotFound();
            }

            // Get details about submissions for the current assignment
            // currently hardcoded to test case
            foreach (Submission s in Submissions)
            {
                numSub++; // Count number of submissions
                // Sort data by grade
                if (s.PointsGraded.HasValue) // Only counts graded submissions
                {
                    double gradePercent = (double)s.PointsGraded / maxPoints;
                    if (gradePercent >= .90) { numA++; }
                    else if (gradePercent >= .80) { numB++; }
                    else if (gradePercent >= .70) { numC++; }
                    else if (gradePercent >= .60) { numD++; }
                    else if (gradePercent < .60) { numF++; }
                }
                else
                {
                    numUG++;
                }
            }

            // Calculate number of students without a submission for this assignment
            numNotSub = Course.StudentsEnrolled - numSub;

            return Page();
        }

        public string GetGradeStatus(int? grade)
        {
            var gradeStatus = "";
            if (grade == null) gradeStatus = "-/" + Assignment.MaxPoints.ToString();
            else gradeStatus = grade.ToString() + "/" + Assignment.MaxPoints.ToString();

            return gradeStatus;
        }
    }
}
