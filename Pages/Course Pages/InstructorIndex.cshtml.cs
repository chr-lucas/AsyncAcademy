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
    public class InstructorIndexModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public InstructorIndexModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get; set; } = default!;
        public User Account { get; set; } = default!;

        
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

            Course = await _context.Course.ToListAsync();

            return Page();
        }
    }
}
