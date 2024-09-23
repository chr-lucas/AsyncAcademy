using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AsyncAcademy.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        [ViewData]
        public string NavBarLink { get; set; } = "/SectionSignup";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        [ViewData]
        public string WelcomeText { get; set; }

        public PrivacyModel(ILogger<PrivacyModel> logger, AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            var account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (account == null)
            {
                return NotFound();
            }

            var firstname = account.FirstName;
            var lastname = account.LastName;
            if (account.IsProfessor)
            {
                WelcomeText = $"Welcome, Professor {firstname} {lastname}";
                NavBarLink = "/CreateSection";
                NavBarText = "Classes";
            }
            else
            {
                WelcomeText = $"Welcome, {firstname} {lastname}";
            }

            return Page();
        }
    }
}
