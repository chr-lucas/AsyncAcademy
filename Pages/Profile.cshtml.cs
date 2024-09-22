using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsyncAcademy.Pages
{
    public class ProfileModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;
        public string accountType = "Student";
        public DateTime birthday;

        [BindProperty]
        public User? Account { get; set; }

        [ViewData]
        public string NavBarLink { get; set; } = "/SectionSignup";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        public async Task<IActionResult> OnGetAsync()
        {
            //// Sample data - replace with actual user data retrieval logic
            //UserName = "John Doe";
            //Email = "john.doe@example.com";
            //JoinDate = new DateTime(2023, 1, 15);
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

            if (Account.IsProfessor == true)
            {
                accountType = "Professor";
                NavBarLink = "/CreateSection";
                NavBarText = "Classes";
            }

            return Page();
        }
    }
}
