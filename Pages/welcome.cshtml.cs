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
using System.Diagnostics;

namespace AsyncAcademy.Pages//.Accounts
{
    public class WelcomeModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;

        [BindProperty]
        public User? Account { get; set; }

        [ViewData]
        public string WelcomeText { get; set; }

        public List<Course> EnrolledCourses = [];
        public List<Section> EnrolledSections = [];

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

            var firstname = Account.FirstName;
            var lastname = Account.LastName;
            if (Account.IsProfessor) 
            {
                WelcomeText = $"Welcome, Professor {firstname} {lastname}";
            }
            else
            {
                WelcomeText = $"Welcome, {firstname} {lastname}";
            }

            // Get all corresponding classes
            var Enrollments = _context.Enrollments.ToList();
            foreach (Enrollment e in Enrollments) 
            {
                if (e.UserId == currentUserID) 
                {
                    Section? correspondingSection = await _context.Sections.FirstOrDefaultAsync(a => a.Id == e.SectionId);
                    if (correspondingSection == null)
                    {
                        return BadRequest();
                    }

                    //Course correspondingCourse = await _context.Course.FirstOrDefaultAsync(a => a.CourseId == correspondingSection.CourseID);
                    //EnrolledCourses.Add(correspondingCourse);
                    EnrolledSections.Add(correspondingSection);
                }
            }

            return Page();
        }

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{

        //    //Get User ID from current session (aka, the logged in user's ID):
        //    int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

        //    if (currentUserID == null)
        //    {
        //        return NotFound();
        //    }

        //    Debug.WriteLine("The current suer ID is: " + id);
        //    Debug.WriteLine("The ACTUAL ID of logged in user is: " + currentUserID);

        //    // If the current session ID does not match the URL ID:
        //    if (id == null || currentUserID != id)
        //    {
        //        Debug.WriteLine("Mismatch between session ID and URL ID.");
        //        Debug.WriteLine("The CORRECT ID ID: " + currentUserID);
        //        // RedirectToPage("./welcome", new { id = currentUserID });
        //        //RedirectToPage("./welcome/"+currentUserID);
        //        return NotFound();
        //    }

        //    Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

        //    if (Account == null)
        //    {
        //        return NotFound();
        //    }

        //    var firstname = Account.FirstName;
        //    var lastname = Account.LastName;
        //    if (Account.IsProfessor)
        //    {
        //        WelcomeText = $"Welcome, Professor {firstname} {lastname}";
        //    }
        //    else
        //    {
        //        WelcomeText = $"Welcome, {firstname} {lastname}";
        //    }

        //    // Get all corresponding classes
        //    var Enrollments = _context.Enrollments.ToList();
        //    foreach (Enrollment e in Enrollments)
        //    {
        //        if (e.UserId == id)
        //        {
        //            Section? correspondingSection = await _context.Sections.FirstOrDefaultAsync(a => a.Id == e.SectionId);
        //            if (correspondingSection == null)
        //            {
        //                return BadRequest();
        //            }

        //            //Course correspondingCourse = await _context.Course.FirstOrDefaultAsync(a => a.CourseId == correspondingSection.CourseID);
        //            //EnrolledCourses.Add(correspondingCourse);
        //            EnrolledSections.Add(correspondingSection);
        //        }
        //    }

        //    return Page();
        //}

    }
}
