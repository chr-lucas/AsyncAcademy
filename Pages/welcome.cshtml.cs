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
        public List<Section> EnrolledSections = new List<Section>(); // Initialize as a new list
        public List<Department> EnrolledCourseDepartments = new List<Department>(); // Initialize as a new list

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
                NavBarLink = "/SectionSignup"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
            }
            else
            {
                WelcomeText = $"Welcome, {firstname} {lastname}";
                NavBarLink = "/CreateSection"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
            }

            // Get all corresponding classes
            var Enrollments = await _context.Enrollments.ToListAsync(); // Use async to avoid blocking
            foreach (Enrollment e in Enrollments) 
            {
                if (e.UserId == currentUserID) 
                {
                    
                    Section? correspondingSection = await _context.Sections.FirstOrDefaultAsync(a => a.SectionId == e.SectionId);
                    Course? correspondingCourse = await _context.Course.FirstOrDefaultAsync(a => a.CourseId == correspondingSection.CourseId);
                    Department? correspondingDepartment = await _context.Department.FirstOrDefaultAsync(a => a.DepartmentId == correspondingCourse.DepartmentId);
                    if (correspondingSection == null)
                    {
                        return BadRequest();
                    }

                    EnrolledSections.Add(correspondingSection);
                    EnrolledCourses.Add(correspondingCourse);
                    EnrolledCourseDepartments.Add(correspondingDepartment);
                }
            }

            return Page();
        }
    }
}
