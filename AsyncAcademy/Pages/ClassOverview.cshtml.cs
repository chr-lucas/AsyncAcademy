// All Kevins code. If you change code, please comment

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Utils;

namespace AsyncAcademy.Pages
{
    public class ClassOverviewModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public ClassOverviewModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        // Counter variables for chart data
        public int numA = 0;
        public int numAm = 0;
        public int numBp = 0;
        public int numB = 0;
        public int numBm = 0;
        public int numCp = 0;
        public int numC = 0;
        public int numCm = 0;
        public int numDp = 0;
        public int numD = 0;
        public int numDm = 0;
        public int numE = 0;

        public bool isProfessor = false;

        public Course Course { get; set; }

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        [ViewData]
        public List<object> notos {  get; set; }

        public User Account { get; set; } = default!;

        public List<Submission> Submissions;

        public List<User> Students = new List<User>();

        public List<Enrollment> EnrollmentsForClass;

        public List<Assignment> AssignmentsForClass;

        public List<int> AssignmentIdsForClass = new List<int>();

        public List<float> AverageGrades = new List<float>();

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);

            if (Course == null)
            {
                return NotFound();
            }

            EnrollmentsForClass = _context.Enrollments.Where(a => (a.CourseId == courseId)).ToList();
            foreach (Enrollment e in EnrollmentsForClass) 
            {
                Students.Add(_context.Users.First<User>(a => (a.Id == e.UserId)));
            }
            AssignmentsForClass = _context.Assignment.Where(a => (a.CourseId == courseId)).ToList();
            foreach (Assignment a in AssignmentsForClass)
            {
                AssignmentIdsForClass.Add(a.Id);
            }
            Submissions = _context.Submissions.Where(a => (AssignmentIdsForClass.Contains(a.AssignmentId))).ToList();

            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (Account == null)
            {
                return NotFound();
            }

             if (Account.IsProfessor) 
            {
                isProfessor = true;
                //WelcomeText = $"Welcome, Professor {firstname} {lastname}";
                // Set ViewData variables for instructors
                NavBarLink = "Course Pages/InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }
            else
            {
                //WelcomeText = $"Welcome, {firstname} {lastname}";
                NavBarLink = "Course Pages/StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
                NavBarAccountTabLink = "/Account";
                NavBarAccountText = "Account";

                notos = new List<object>();
                List<Submission> notifications = await _context.Submissions
                    .Where(e => e.UserId == currentUserID)
                    .Where(n => n.IsNew == true)
                    .ToListAsync();

                Noto notoController = new Noto();
                notoController.SetViewData(ViewData, notifications.Count);

                if (notifications.Count > 0)
                {
                    foreach (Submission notification in notifications)
                    {
                        List<object> result = notoController.NotoData(_context, notification);
                        notos.Add(result);
                    }
                }

            }

            foreach (User stu in Students) // Not very efficient, could probably be optimized
            {
                int? scores = 0;
                int? numsubs = 0;
                int? totals = 0;
                float scoreForStudent = 0;
                foreach (Submission sub in Submissions)
                {
                    if (sub.UserId == stu.Id) {
                        if (sub.PointsGraded.HasValue) // Only counts graded submissions
                        {
                            numsubs += 1; // Probably needs a better name
                            scores += sub.PointsGraded;
                            totals += _context.Assignment.First(a => (a.Id == sub.AssignmentId)).MaxPoints; // This is gonna require a lot of queries, so not efficient at all
                        }
                    }
                }
                if (numsubs > 0)
                {
                    scoreForStudent = ((float)scores / (float)totals) * 100;
                }
                else 
                {
                    scoreForStudent = -1; // Negative grade means no assignments have been graded yet
                }
                AverageGrades.Add(scoreForStudent);
            }

            foreach (float? grade in AverageGrades) {
                if (grade >= 94) { numA++; }
                else if (grade >= 90) { numAm++; }
                else if (grade >= 87) { numBp++; }
                else if (grade >= 84) { numB++; }
                else if (grade >= 80) { numBm++; }
                else if (grade >= 77) { numCp++; }
                else if (grade >= 74) { numC++; }
                else if (grade >= 70) { numCm++; }
                else if (grade >= 67) { numDp++; }
                else if (grade >= 64) { numD++; }
                else if (grade >= 60) { numDm++; }
                else if (grade < 60) { numE++; }
            }

            return Page();
        }
    }
}
