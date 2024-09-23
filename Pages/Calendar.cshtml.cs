using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;
using AsyncAcademy.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace AsyncAcademy.Pages
{
    public class CalendarModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;

        [BindProperty]
        public User? Account { get; set; }

        [ViewData]
        public string NavBarLink { get; set; } = "/CreateSection";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

         public List<Course> EnrolledCourses = [];
        public List<Section> EnrolledSections = [];
        public List<CalendarEvent> CalendarEvents = [];

        public JsonResult result { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            //assigns user - Assisted by Chris L.
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

            if (Account.IsProfessor)
            {
                NavBarLink = "/SectionSignup";
                NavBarText = "Classes";


            }

            if (!Account.IsProfessor)
            {
                GetStudentEvents(currentUserID);
            }

            return Page();
        }

        private void GetStudentEvents(int? currentUserID)
        {
            // Get all corresponding classes for signed in user - Borrowed logic from Bash
            var Enrollments = _context.Enrollments.ToList();
            foreach (Enrollment e in Enrollments)
            {
                if (e.UserId == currentUserID) //fills list with sections signed in user is enrolled in
                {
                    Section? correspondingSection = _context.Sections.FirstOrDefault(a => a.SectionId == e.SectionId);
                    if (correspondingSection == null)
                    {
                        BadRequest();
                    }
                    EnrolledSections.Add(correspondingSection);
                }
            }

            //create calendar events for each section

            foreach (Section s in EnrolledSections)
            {
                //Determines how many classes the user has per day
                int classesPerDay = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (s.MeetingTimeInfo.Contains("Monday"))
                    {
                        classesPerDay++;
                    }
                    if (s.MeetingTimeInfo.Contains("Tuesday"))
                    {
                        classesPerDay++;
                    }
                    if (s.MeetingTimeInfo.Contains("Wednesday"))
                    {
                        classesPerDay++;
                    }
                    if (s.MeetingTimeInfo.Contains("Thursday"))
                    {
                        classesPerDay++;
                    }
                    if (s.MeetingTimeInfo.Contains("Friday"))
                    {
                        classesPerDay++;
                    }
                    break;

                }


                //Creates the calendar events for each section
                CalendarEvent NewEvent = new CalendarEvent();
                NewEvent.title = "Class " + s.SectionId; // need to pull specific title
                NewEvent.startRecur = s.StartDate;
                NewEvent.endRecur = s.EndDate;
                NewEvent.startTime = s.StartTime;
                NewEvent.endTime = s.EndTime;
                NewEvent.daysOfWeek = new int[classesPerDay];

                //determines which day each event occurs - Monday - Friday as classes do not occur on the weekends
                for (int i = 0; i < 5; i++)
                {
                    if (s.MeetingTimeInfo.Contains("Monday"))
                    {
                        NewEvent.daysOfWeek[i] = 1;
                        i++;
                    }
                    if (s.MeetingTimeInfo.Contains("Tuesday"))
                    {
                        NewEvent.daysOfWeek[i] = 2;
                        i++;
                    }
                    if (s.MeetingTimeInfo.Contains("Wednesday"))
                    {
                        NewEvent.daysOfWeek[i] = 3;
                        i++;
                    }
                    if (s.MeetingTimeInfo.Contains("Thursday"))
                    {
                        NewEvent.daysOfWeek[i] = 4;
                        i++;
                    }
                    if (s.MeetingTimeInfo.Contains("Friday"))
                    {
                        NewEvent.daysOfWeek[i] = 5;
                        i++;
                    }
                    break;

                }

                CalendarEvents.Add(NewEvent);
            }
        }
    }
}
