using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json; // For JSON serialization
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages
{
    public class ToDoItem
    {
        public string Course { get; set; }
        public string Assignment { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignmentID { get; set; } // For to-do list clickability - Hanna W
        public string SubmissionType { get; set; } // For to-do list clickability - Hanna W
    }

    public class WelcomeModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        [BindProperty]
        public User? Account { get; set; }

        [ViewData]
        public string WelcomeText { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        public List<Course> EnrolledCourses { get; set; } = new List<Course>();

        public List<ToDoItem> ToDoList { get; set; } = new List<ToDoItem>();

        public WelcomeModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            // Check if user data is already in session
            if (HttpContext.Session.TryGetValue("UserAccount", out var accountData) &&
                HttpContext.Session.TryGetValue("EnrolledCourses", out var coursesData) &&
                HttpContext.Session.TryGetValue("ToDoList", out var todoData))
            {
                // Deserialize data from session
                Account = JsonSerializer.Deserialize<User>(accountData);
                EnrolledCourses = JsonSerializer.Deserialize<List<Course>>(coursesData);
                ToDoList = JsonSerializer.Deserialize<List<ToDoItem>>(todoData);

                // Set ViewData properties here
                var firstname = Account.FirstName;
                var lastname = Account.LastName;

                if (Account.IsProfessor)
                {
                    WelcomeText = $"Welcome, Professor {firstname} {lastname}";
                    NavBarLink = "Course Pages/InstructorIndex";
                    NavBarText = "Classes";
                    NavBarAccountTabLink = "";
                    NavBarAccountText = "";
                }
                else
                {
                    WelcomeText = $"Welcome, {firstname} {lastname}";
                    NavBarLink = "Course Pages/StudentIndex";
                    NavBarText = "Register";
                    NavBarAccountTabLink = "/Account";
                    NavBarAccountText = "Account";
                }
            }
            else
            {
                // If not in session, query the database
                Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

                if (Account == null)
                {
                    return NotFound();
                }

                var firstname = Account.FirstName;
                var lastname = Account.LastName;

                if (Account.IsProfessor)
                {
                    WelcomeText = $"Welcome, Professor {firstname} {lastname}";
                    NavBarLink = "Course Pages/InstructorIndex";
                    NavBarText = "Classes";
                    NavBarAccountTabLink = "";
                    NavBarAccountText = "";
                }
                else
                {
                    WelcomeText = $"Welcome, {firstname} {lastname}";
                    NavBarLink = "Course Pages/StudentIndex";
                    NavBarText = "Register";
                    NavBarAccountTabLink = "/Account";
                    NavBarAccountText = "Account";
                }

                // Get all enrolled courses for the current student
                var enrollments = await _context.Enrollments
                    .Where(e => e.UserId == currentUserID)
                    .ToListAsync();

                var enrolledCourseIds = enrollments.Select(e => e.CourseId).ToList();

                EnrolledCourses = await _context.Course
                    .Where(c => enrolledCourseIds.Contains(c.Id))
                    .ToListAsync();

                var submissions = await _context.Submissions
                    .Where(a => a.UserId == currentUserID)
                    .Select(a => a.AssignmentId)
                    .ToListAsync();

                // Get upcoming assignments
                DateTime now = DateTime.Now;
                ToDoList = await _context.Assignment
                    .Where(a => a.Due > now && enrolledCourseIds.Contains(a.CourseId) && !submissions.Contains(a.Id))
                    .OrderBy(a => a.Due)
                    .Take(5)
                    .Select(a => new ToDoItem
                    {
                        Course = _context.Course
                            .Where(c => c.Id == a.CourseId)
                            .Select(c => c.Department + " " + c.CourseNumber)
                            .FirstOrDefault(),
                        Assignment = a.Title,
                        DueDate = a.Due,
                        AssignmentID = a.Id,
                        SubmissionType = a.Type
                    })
                    .ToListAsync();

                // Store data in session
                HttpContext.Session.SetString("UserAccount", JsonSerializer.Serialize(Account));
                HttpContext.Session.SetString("EnrolledCourses", JsonSerializer.Serialize(EnrolledCourses));
                HttpContext.Session.SetString("ToDoList", JsonSerializer.Serialize(ToDoList));
            }

            return Page();
        }
    }
}
