using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncAcademy.Pages
{
    public class ImageModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;
        private int _id;

        [BindProperty]
        public User? Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == Id);

            if (Account == null)
            {
                return NotFound();
            }

            _id = Account.Id;

            return Page();
        }

        public int GetUserID()
        {
            return Account.Id;
        }

    }
}
