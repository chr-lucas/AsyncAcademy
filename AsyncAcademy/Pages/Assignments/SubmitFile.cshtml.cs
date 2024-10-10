using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmitFileModel : PageModel
    {

        private readonly AsyncAcademyContext _context;


        public SubmitFileModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public User Account { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        public Assignment Assignment { get; set; }

        public Submission Submission { get; set; } = new Submission();

        public IFormFile FileSubmission { get; set; }

        public string FileName { get; set; }


        public void OnGet(int id)
        {
            // Assign user
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                NotFound();
            }

            Account = _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account == null)
            {
                NotFound();
            }

            // Set navbar
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

            Assignment = _context.Assignment.FirstOrDefault(a => a.Id == id);

            if (Assignment == null)
            {
                NotFound();
            }

        }


    }
}
