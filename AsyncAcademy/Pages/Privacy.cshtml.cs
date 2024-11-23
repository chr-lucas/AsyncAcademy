using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Utils;
using System.Threading.Tasks;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        [ViewData]
        public string NavBarLink { get; set; } = "Course Pages/StudentIndex";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        [ViewData]
        public List<object> notos { get; set; }

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
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }
            else
            {
                WelcomeText = $"Welcome, {firstname} {lastname}";

                notos = new List<object>();
                List<Submission> notifications = await _context.Submissions
                    .Where(e => e.UserId == currentUserID)
                    .Where(n => n.IsNew == true)
                    .ToListAsync();

                if (notifications.Count > 0)
                {
                    noto notoController = new noto();
                    notoController.SetViewData(ViewData, notifications.Count);
                    foreach (Submission notification in notifications)
                    {
                        List<object> result = notoController.NotoData(_context, notification);
                        notos.Add(result);
                    }
                }
            }

            return Page();
        }
    }
}
