using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public string NavBarLink { get; set; } = "Course Pages/StudentIndex";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        public List<Course> EnrolledSections = [];
        public List<CalendarEvent> CalendarEvents = [];
        public List<Assignment> UpcomingAssignments = [];
        public List<Course> InstructorSections = [];
        public List<string> colorWheel = ["PaleTurquoise","Gold","LightPink","PaleGreen","Plum"];
        public int colorIndex = 0;


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
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
                GetInstructorEvents(currentUserID);
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }

            if (!Account.IsProfessor)
            {
                GetStudentEvents(currentUserID);

            }

            return Page();
        }

        private void GetInstructorEvents(int? currentUserID)
        {
            foreach (Course s in _context.Course)
            {
                if (s.InstructorId == currentUserID)
                {
                    InstructorSections.Add(s);
                }
            }

            foreach (Course s in InstructorSections)
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
                NewEvent.title = s.Name; // need to pull specific title
                NewEvent.startRecur = s.StartDate;
                NewEvent.endRecur = s.EndDate;
                NewEvent.startTime = s.StartTime.ToString("HH:mm:ss");
                NewEvent.endTime = s.EndTime.ToString("HH:mm:ss");
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

        private void GetStudentEvents(int? currentUserID)
        {
            // Get all corresponding classes for signed in user - Borrowed logic from Bash
            var Enrollments = _context.Enrollments.ToList();
            foreach (Enrollment e in Enrollments)
            {
                if (e.UserId == currentUserID) //fills list with sections signed in user is enrolled in
                {
                    Course? correspondingSection = _context.Course.FirstOrDefault(a => a.Id == e.CourseId);
                    if (correspondingSection == null)
                    {
                        BadRequest();
                    }
                    EnrolledSections.Add(correspondingSection);

                    // Get all assignments for current course
                    // Add .Where(a => a.Due > DateTime.Now) to filter our past events
                    // Filter out events that already have a submission?
                    var upcomingAssignments = _context.Assignment.Where(a => a.CourseId == correspondingSection.Id).ToList();
                    UpcomingAssignments.AddRange(upcomingAssignments);
                    
                    foreach (Assignment a in UpcomingAssignments)
                    {
                        //Creates the calendar events for each assignment
                        CalendarEvent FutureAssignment = new CalendarEvent();
                        FutureAssignment.title = a.Title;
                        FutureAssignment.start = a.Due;
                        FutureAssignment.end = a.Due.AddSeconds(1);
                        FutureAssignment.url = "/Assignments/Submit?id=" + a.Id.ToString();
                        FutureAssignment.display = "list-item";
                        FutureAssignment.backgroundColor = colorWheel[colorIndex];
                        FutureAssignment.borderColor = colorWheel[colorIndex];
                        CalendarEvents.Add(FutureAssignment);
                    }
                    colorIndex++; // increment colors for next set of assignments
                }
            }

            colorIndex = 0; // reset color wheel for courses

            //create calendar events for each section
            foreach (Course s in EnrolledSections)
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
                NewEvent.title = s.Name; // need to pull specific title
                NewEvent.startRecur = s.StartDate;
                NewEvent.endRecur = s.EndDate;
                NewEvent.startTime = s.StartTime.ToString("HH:mm:ss");
                NewEvent.endTime = s.EndTime.ToString("HH:mm:ss");
                NewEvent.display = "block";
                NewEvent.backgroundColor = colorWheel[colorIndex];
                NewEvent.borderColor = colorWheel[colorIndex];
                NewEvent.textColor = "black";
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
                colorIndex++;

                CalendarEvents.Add(NewEvent);
            }

            //foreach(Assignment a in UpcomingAssignments)
            //{
            //    //Creates the calendar events for each assignment
            //    CalendarEvent FutureAssignment = new CalendarEvent();
            //    FutureAssignment.title = a.Title;
            //    FutureAssignment.start = a.Due;
            //    FutureAssignment.end = a.Due.AddSeconds(1);
            //    FutureAssignment.url = "/Assignments/Submit?id=" + a.Id.ToString();
            //    FutureAssignment.display = "list-item";
            //    FutureAssignment.backgroundColor = "PaleVioletRed";
            //    FutureAssignment.borderColor = "PaleVioletRed";
            //    CalendarEvents.Add(FutureAssignment);
            //}
        }
    }
}
