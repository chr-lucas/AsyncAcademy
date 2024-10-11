using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmitFileModel(AsyncAcademy.Data.AsyncAcademyContext context, IWebHostEnvironment environment) : PageModel
    {

        private readonly AsyncAcademyContext _context = context;
        private IWebHostEnvironment _environment = environment;

        public User Account { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        public Assignment Assignment { get; set; }

        public Submission Submission { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Assign user
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            if (FileSubmission == null || FileSubmission.Length == 0)
            {
                ModelState.AddModelError("ImageError", "No image uploaded.");
                return Page();
            }

            Assignment = await _context.Assignment.FirstOrDefaultAsync(a => a.Id == id);

            //create new submission
            Submission = new Submission();
            Submission.AssignmentId = id;
            Submission.UserId = (int)currentUserID;
            Submission.Timestamp = DateTime.Now;

            //save file to wwwroot/Submissions
            if (FileSubmission != null)
            {
                //saves filename as userID_CourseID_AssignmentID_FileName to keep files unique to student, course, and assignment
                FileName = currentUserID.ToString() + "_" + Assignment.CourseId + "_" + Assignment.Id + "_" + FileSubmission.FileName;
                string dbPath = "/submissions/" + FileName;

                //saves to wwwroot/submissions
                string filePath = _environment.WebRootPath + dbPath;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FileSubmission.CopyToAsync(fileStream);
                }

                //save file path as submission content
                Submission.Content = filePath;
            }

            //Add new submission to database
            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Assignments/Details", new { id = id });

        }



    }
}
