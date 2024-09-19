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
        private readonly UserManager<User> _userManager; // Inject UserManager to fetch user data
        private readonly SignInManager<User> _signInManager; // Optional: Check if user is signed in

        public SectionSignupModel(AsyncAcademyContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

            // Fetch the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // If no user is logged in, redirect to login page
                return RedirectToPage("/Account/Login");
            }

            // Set the InstructorId to the logged-in user's Id
            NewSection.InstructorId = user.Id;

            // Add the new section to the database
            _context.Sections.Add(NewSection);
            await _context.SaveChangesAsync();

            // Check if the user is already logged in
            if (!_signInManager.IsSignedIn(User))
            {
                // If the user is not logged in, log them in
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            // Redirect to the welcome page, keeping the user logged in
            return RedirectToPage("./Welcome", new { id = user.Id });
        }
    }
}
