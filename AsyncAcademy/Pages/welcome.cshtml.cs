using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Stripe;

namespace AsyncAcademy.Pages
{
    public class ToDoItem
    {
        public string Course { get; set; }
        public string Assignment { get; set; }
        public DateTime DueDate { get; set; }
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
    
    // Get list of course IDs that the student is enrolled in
    var enrolledCourseIds = Enrollments.Select(e => e.CourseId).ToList();

            EnrolledCourses = await _context.Course
                .Where(c => enrolledCourseIds.Contains(c.Id))
                .ToListAsync();

            // Get the upcoming assignments, filtering by the student's enrolled courses and excluding past due assignments
            DateTime now = DateTime.Now; // Or use DateTime.UtcNow for consistency
            ToDoList = await _context.Assignment
                .Where(a => a.Due > now && enrolledCourseIds.Contains(a.CourseId)) // Filter by enrolled courses
                .OrderBy(a => a.Due)
                .Take(5)
                .Select(a => new ToDoItem
                {
                    Course = _context.Course
                        .Where(c => c.Id == a.CourseId)
                        .Select(c => c.Department + " " + c.CourseNumber)
                        .FirstOrDefault(),
                    Assignment = a.Title,
                    DueDate = a.Due
                })
                .ToListAsync();

            // Check for dropped courses
            // var droppedCourses = await _context.DroppedCourses  //we don't currently have a DroppedCourses in the database 
            //   .Where(d => d.UserId == userId)
            // .ToListAsync();

            //if (droppedCourses.Any())
            //{
            // Run queries related to dropped courses
            // For example, log the drop event or update user data
            //}

            return Page();
        }
    }
}
