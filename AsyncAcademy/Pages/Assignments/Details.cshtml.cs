using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

//Page to display details of an assignment for a course
//Code below written by Hanna Whitney unless notated otherwise

namespace AsyncAcademy.Pages.Assignments
{
    public class DetailsModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public DetailsModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        public Assignment Assignment { get; set; } = default!;

        public User Account { get; set; }

        public Course Course { get; set; }

        public List<Submission> Submissions = new List<Submission>();

        public List<Assignment> CorrespondingAssignment = new List<Assignment>();

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            //assigns user - Assisted by Chris L.
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            // Pull all submissions for account
            Submissions = _context.Submissions.Where(a => a.UserId == currentUserID).ToList();
            foreach (var submission in Submissions) {
                CorrespondingAssignment.Add(_context.Assignment.First(a => a.Id == submission.AssignmentId));
            }

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

            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }
            else
            {
                Assignment = assignment;
            }

            //assigns course - Assisted by Kevin B.
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);

            if (Course == null)
            {
                NotFound();
            }
            return Page();
        }
    }
}
