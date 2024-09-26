using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Course_Pages
{
    public class DeleteModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public DeleteModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            else
            {
                Course = course;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                Course = course;
                _context.Course.Remove(Course);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
