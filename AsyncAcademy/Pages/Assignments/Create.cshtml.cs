using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncAcademy.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public CreateModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public User Account { get; set; }
     
        public int? CourseId { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        [BindProperty]
        public Assignment Assignment { get; set; }

        public IActionResult OnGet(int? courseId)
        {
            // Assign user
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account == null)
            {
                return NotFound();
            }

            // Set navbar
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

            // Ensure courseId is passed in URL
            if (courseId == null)
            {
                return NotFound(); // Handle missing courseId
            }

            // Store courseId in ViewData for later use in form submission
            ViewData["CourseId"] = courseId.Value;

            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure the CourseId is assigned to the new assignment
            if (ViewData["CourseId"] != null)
            {
                Assignment.CourseId = (int)ViewData["CourseId"];
            }

            // Add the assignment to the context and save
            _context.Assignment.Add(Assignment);
            await _context.SaveChangesAsync();

            // Redirect to the Assignment Details page after creating it
            return RedirectToPage("/Assignments/Details", new { id = Assignment.Id });
        }
    }
}
