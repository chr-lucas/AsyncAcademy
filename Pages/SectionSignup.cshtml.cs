using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages
{
    public class SectionSignupModel : PageModel
    {
        private readonly AsyncAcademyContext _context;
        private readonly SignInManager<User> _signInManager; // Inject SignInManager
        private readonly UserManager<User> _userManager; // Inject UserManager

        public SectionSignupModel(AsyncAcademyContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

            // Initialize NewSection with default values for required fields, if necessary
            NewSection = new Section
            {
                StartTime = DateTime.Now, // Example default value
                EndTime = DateTime.Now.AddHours(1), // Example default value
                InstructorId = 1, // Replace this with a valid InstructorId
                Location = "Default Location",
                StudentCapacity = 30,
                StudentsEnrolled = 0,
                MeetingTimeInfo = "Weekly on Monday"
            };
        }

        // Bind the Section model to the form data
        [BindProperty]
        public Section NewSection { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add the new section to the database context
            _context.Sections.Add(NewSection);
            await _context.SaveChangesAsync();

            // Ensure the user is logged in
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user

            if (user != null)
            {
                // Check if the user is already logged in
                var isSignedIn = _signInManager.IsSignedIn(User);

                if (!isSignedIn)
                {
                    // If the user is not logged in, log them in
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            else
            {
                // If no user is found, redirect to login page
                return RedirectToPage("/Account/Login");
            }

            // Redirect to the welcome page, while keeping the user logged in
            return RedirectToPage("./Welcome", new { id = user.Id });
        }
    }
}
