using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

//Main assignment display page for a couse
//Code below written by Hanna Whitney unless notated otherwise

namespace AsyncAcademy.Pages.Assignments
{
    public class IndexModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public IndexModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }

        [ViewData]
        public string NavBarLink { get; set; } // Removed default initialization

        [ViewData]
        public string NavBarText { get; set; } // Removed default initialization

        public User Account { get; set; }

        public IList<Assignment> Assignment { get;set; }


        public async Task OnGetAsync(int courseId)
        {
            //assigns course - Assisted by Kevin B.
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);

            if (Course == null)
            {
                NotFound();
            }

            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                NotFound();
            }
            //assigns user - Assisted by Chris L.
            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (Account == null)
            {
                NotFound();
            }

            //set navbar
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

            //Displays the assignments related to the course the user is viewing
            var assignments = from a in _context.Assignment
                              select a;
            assignments = assignments.Where(a => a.CourseId == Course.Id);
            Assignment = await assignments.ToListAsync();
        }
    }
}
