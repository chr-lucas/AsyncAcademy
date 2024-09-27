using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    public class WelcomeModel : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context;

        [BindProperty]
        public User? Account { get; set; }

        [ViewData]
        public string WelcomeText { get; set; }

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        public List<Course> EnrolledCourses = new List<Course>(); // Initialize as a new list

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
                // Set ViewData variables for instructors
                NavBarLink = "Course Pages/InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
            }
            else
            {
                WelcomeText = $"Welcome, {firstname} {lastname}";
                NavBarLink = "Course Pages/StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
            }

            // Get all corresponding classes
            var Enrollments = await _context.Enrollments.ToListAsync(); // Use async to avoid blocking
            foreach (Enrollment e in Enrollments) 
            {
                if (e.UserId == currentUserID) 
                {
                    Course? correspondingCourse = await _context.Course.FirstOrDefaultAsync(a => a.Id == e.CourseId);
                    if (correspondingCourse == null)
                    {
                        return BadRequest();
                    }

                    EnrolledCourses.Add(correspondingCourse);
                }
            }

            // Sample data for ToDoList
        ToDoList = new List<ToDoItem>
        {
            new ToDoItem { Course = "CS 3750", Assignment = "Assignment 1", DueDate = new DateTime(2024, 9, 16, 23, 59, 0) },
            new ToDoItem { Course = "CS 3750", Assignment = "Assignment 2", DueDate = new DateTime(2024, 9, 17, 23, 59, 0) },
            new ToDoItem { Course = "CS 3750", Assignment = "Assignment 3", DueDate = new DateTime(2024, 9, 18, 23, 59, 0) },
            new ToDoItem { Course = "CS 3750", Assignment = "Assignment 4", DueDate = new DateTime(2024, 9, 19, 23, 59, 0) },
            new ToDoItem { Course = "CS 3750", Assignment = "Assignment 5", DueDate = new DateTime(2024, 9, 20, 23, 59, 0) }
        };

            return Page();
        }
    }
}
