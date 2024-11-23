using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

//Edit assignment page for a course, only accessible by instrcutor
//Code below written by Hanna Whitney unless notated otherwise

namespace AsyncAcademy.Pages.Assignments
{
    public class EditModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public EditModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        [BindProperty]
        public Assignment Assignment { get; set; } = default!;

        public User? Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            //sets navbar
            if (Account.IsProfessor)
            {

                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }
            else
            {
                NavBarLink = "Course Pages/StudentIndex";
                NavBarText = "Register";
                NavBarAccountTabLink = "/Account";
                NavBarAccountText = "Account";
                var notifications = await _context.Submissions
                    .Where(e => e.UserId == currentUserID)
                    .Where(n => n.IsNew == true)
                    .ToListAsync();

                if (notifications.Count > 0)
                {
                    ViewData["BellIcon"] = "fa-solid fa-bell";
                    ViewData["BellNum"] = notifications.Count.ToString();
                }
                else
                {
                    ViewData["BellIcon"] = "fa-regular fa-bell";
                    ViewData["BellNum"] = String.Empty;
                }

            }

            var assignment =  await _context.Assignment.FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }
            Assignment = assignment;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Assignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(Assignment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Assignments/Details", new { id = Assignment.Id });
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.Id == id);
        }
    }
}
