using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;
using AsyncAcademy.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace AsyncAcademy.Pages
{
    public class CalendarModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;

        [BindProperty]
        public User? Account { get; set; }
         public List<Course> EnrolledCourses = [];
        public List<Section> EnrolledSections = [];
        public List<CalendarEvent> CalendarEvents = [];

        public JsonResult result { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            //asigns user
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

            // Get all corresponding classes for signed in user - Borrowed logic from Bash
            var Enrollments = _context.Enrollments.ToList();
            foreach (Enrollment e in Enrollments)
            {
                if (e.UserId == currentUserID) //fills list with sections signed in user is enrolled in
                {
                    Section? correspondingSection = _context.Sections.FirstOrDefault(a => a.Id == e.SectionId);
                    if (correspondingSection == null)
                    {
                        BadRequest();
                    }
                    EnrolledSections.Add(correspondingSection);
                }
            }

            //create calendar events for each section
            //TO DO - NEED TO REFINE HOW CALENDAREVENT DATA IS COLLECTED TO MEET INFO REQUIREMENTS

            foreach (Section s in EnrolledSections)
            {
                CalendarEvent NewEvent = new CalendarEvent();
                NewEvent.Title = "Class"; // need to pull specific title
                NewEvent.StartTime = s.StartTime;
                NewEvent.EndTime = s.EndTime;
                CalendarEvents.Add(NewEvent);
            }


            return Page();
        }
    }
}
