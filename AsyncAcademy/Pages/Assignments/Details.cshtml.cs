using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

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

        public User Account { get; set; }

        public Course Course { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            //assigns user - Assisted by Chris L.
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

            //sets navbar
            if (Account.IsProfessor)
            {
                //WelcomeText = $"Welcome, Professor {firstname} {lastname}";
                // Set ViewData variables for instructors
                NavBarLink = "Course Pages/InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
            }
            else
            {
                //WelcomeText = $"Welcome, {firstname} {lastname}";
                NavBarLink = "Course Pages/StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
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
            }

            //assigns course - Assisted by Kevin B.
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);

            if (Course == null)
            {
                NotFound();
            }
            return Page();
        }
    }
}
