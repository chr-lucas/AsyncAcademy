using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Identity.Client;

namespace AsyncAcademy.Pages
{
    public class ImageModel(AsyncAcademy.Data.AsyncAcademyContext context, IWebHostEnvironment environment) : PageModel
    {
        private const int TwoMegaBytes = 2 * 1024 * 1024;
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;
        private IWebHostEnvironment _environment = environment;
        public string profilePath;

        [BindProperty]
        public User? Account { get; set; }
        
        [ViewData]
        public string NavBarLink { get; set; } = "/CreateSection";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        [BindProperty]
        public IFormFile myFile { get; set; }

        public string myFileName { get; set; }

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
                NavBarLink = "/SectionSignup";
                NavBarText = "Classes";
            }
            
            profilePath = Account.ProfilePath;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (myFile == null)
            {
                ModelState.AddModelError("ImageError", "No image uploaded.");
                return Page();
            }

            if (myFile.Length > TwoMegaBytes)
            {
                ModelState.AddModelError("ImageError", "Image too large. Upload a file less than 2MB in size.");
                return Page();
            }
            
            myFileName = Account.Id.ToString() + "_" + myFile.FileName;

            string dbPath = "/images/" + myFileName;

            string filePath = _environment.WebRootPath + dbPath;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await myFile.CopyToAsync(fileStream);
            }

            Account.ProfilePath = dbPath;
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
