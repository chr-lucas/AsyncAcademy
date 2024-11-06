using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json; // Make sure to include this for JSON serialization
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
        public int AssignmentID { get; set; } // Added for to-do list clickability - Hanna W
        public string SubmissionType { get; set; } //Added for to-do list clickability - Hanna W
    }

    public class WelcomeModel : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context;

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

        public WelcomeModel(AsyncAcademy.Data.AsyncAcademyContext context)
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
                // Deserialize the data from session
                Account = JsonSerializer.Deserialize<User>(accountData);
                EnrolledCourses = JsonSerializer.Deserialize<List<Course>>(coursesData);
                ToDoList = JsonSerializer.Deserialize<List<ToDoItem>>(todoData);
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
                var Enrollments = await _context.Enrollments
                    .Where(e => e.UserId == currentUserID)
                    .ToListAsync();

                var enrolledCourseIds = Enrollments.Select(e => e.CourseId).ToList();

                EnrolledCourses = await _context.Course
                    .Where(c => enrolledCourseIds.Contains(c.Id))
                    .ToListAsync();

                var submissions = await _context.Submissions.Where(a => a.UserId == currentUserID).Select(a => a.AssignmentId).ToListAsync(); //Used to exclude todo list items with submissions - Hanna W

                // Get the upcoming assignments
                DateTime now = DateTime.Now; // Or use DateTime.UtcNow for consistency
                ToDoList = await _context.Assignment
                    .Where(a => a.Due > now && enrolledCourseIds.Contains(a.CourseId) && !submissions.Contains(a.Id)) //changed to exclude todo list items with submissions - Hanna W
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

                // Store the data in session
                HttpContext.Session.SetString("UserAccount", JsonSerializer.Serialize(Account));
                HttpContext.Session.SetString("EnrolledCourses", JsonSerializer.Serialize(EnrolledCourses));
                HttpContext.Session.SetString("ToDoList", JsonSerializer.Serialize(ToDoList));
            }

            return Page();
        }
    }
} 
