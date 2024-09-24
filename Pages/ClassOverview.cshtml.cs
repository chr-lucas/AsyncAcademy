<!-- All Kevins code. If you change code, please comment -->

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages
{
    public class ClassOverviewModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public ClassOverviewModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
