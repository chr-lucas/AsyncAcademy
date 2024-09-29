using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Elfie.Serialization;
using System.Runtime.ConstrainedExecution;

namespace AsyncAcademy.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public CreateModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }
        public User Account { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        public IActionResult OnGet()
        {

            //assigns user - Assisted by Chris L.
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account =  _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account == null)
            {
                return NotFound();
            }

            //sets navbar
            if (Account.IsProfessor)
            {
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes"; 
            }
            else
            {
                NavBarLink = "Course Pages/StudentIndex"; 
                NavBarText = "Register"; 
            }

            return Page();

        }

        [BindProperty]
        public Assignment Assignment { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Assignment.Add(Assignment);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Assignments/Details", new {id = Assignment.Id });


        }
    }
}
