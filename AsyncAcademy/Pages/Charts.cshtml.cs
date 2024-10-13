using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace AsyncAcademy.Pages
{
    public class ChartsModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public ChartsModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [ViewData]
        public string NavBarLink { get; set;}

        [ViewData]
        public string NavBarText { get; set;}

        public User Account { get; set; }
        public Assignment Assignment { get; set; } // Get via OnGet()
        public Course Course { get; set; }

        public List<Submission> Submissions = []; // temp store for DB querry
        
        // Counter variables for chart data
        public int numA = 0;
        public int numB = 0;
        public int numC = 0;
        public int numD = 0;
        public int numF = 0;
        public int numUG = 0;
        public int numSub = 0;
        public int numNotSub = 2; // Simulated for testing since I can't look up a course

        public void OnGet()
        {
            //assigns user - Assisted by Chris L.
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            Account = _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account == null)
            {
                NotFound();
            }

            //set navbar
            if (Account.IsProfessor)
            {
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
            }
            else
            {
                NavBarLink = "Course Pages/StudentIndex";
                NavBarText = "Register";
            }

            // Get details about the current assignment
            // currently hardcoded to test case, so this logic is not used
            // Assignment = _context.Assignment.FirstOrDefault(a => a.Id == passedAssignmentId);
            // Course = _context.Course.FirstOrDefault(c => c.Id == Assignment.CourseId);


            // Get details about submissions for the current assignment
            // currently hardcoded to test case
            foreach (Submission s in _context.Submissions)
            {
                if (s.AssignmentId == 77)
                {
                    numSub++; // Count number of submissions
                    // Sort data by grade
                    if (s.PointsGraded.HasValue) // Only counts graded submissions
                    {
                        if (s.PointsGraded >= 90) { numA++;}
                        else if (s.PointsGraded >= 80) { numB++;}
                        else if (s.PointsGraded >= 70) { numC++;}
                        else if (s.PointsGraded >= 60) { numD++;}
                        else if (s.PointsGraded < 60) { numF++;}
                    }
                    else
                    {
                        numUG++;
                    }
                }
            }

            // Calculate number of students without a submission for this assignment
            //numNotSub = Course.StudentsEnrolled - numSub;
        }
    }
}
