using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Course_Pages
{
    public class StudentIndexModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        public StudentIndexModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        // List of courses user is enrolled in
        public IList<Course> EnrolledCourses { get; set; } = new List<Course>();

        // List of courses user is not enrolled in
        public IList<Course> AvailableCourses { get; set; } = new List<Course>();

        public User Account { get; set; } = default!;

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

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

            if (Account.IsProfessor) 
            {
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
            }

            // Fetch courses the user is enrolled in
            var enrolledCoursesQuery = from enrollment in _context.Enrollments
                                       join course in _context.Course
                                       on enrollment.CourseId equals course.Id
                                       where enrollment.UserId == currentUserID
                                       select course;

            // Store enrolled courses in a list
            EnrolledCourses = await enrolledCoursesQuery.ToListAsync();

            // Fetch all available courses for the second table
            AvailableCourses = await _context.Course
                                              .Where(c => !EnrolledCourses.Select(ec => ec.Id).Contains(c.Id))
                                              .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int courseId)
        {

            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }
            var enrollmentToDelete = await _context.Enrollments.    
                FirstOrDefaultAsync(enrollment => enrollment.CourseId == courseId && enrollment.UserId == currentUserID);

            // Check if the enrollment exists
            if (enrollmentToDelete != null)
            {
                // Remove the enrollment record
                _context.Enrollments.Remove(enrollmentToDelete);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            // Fetch courses the user is enrolled in
            var enrolledCoursesQuery = from enrollment in _context.Enrollments
                                       join course in _context.Course
                                       on enrollment.CourseId equals course.Id
                                       where enrollment.UserId == currentUserID
                                       select course;



            return RedirectToPage(); // Refresh the page to show updated lists

        }
    }



}
