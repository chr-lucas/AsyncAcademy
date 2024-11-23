using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Utils;

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmitModel : PageModel
    {
        private readonly AsyncAcademyContext _context;
        public List<Submission> previousSubmissions; //tracks previous submissions of given assignment - Hanna w
        public int? currentGrade; //tracks current grade of given assignment  - Hanna w
        public int? minGrade; //tracks minimum grade for chart data - Hanna w
        public int? maxGrade; //tracks max grade for chart data - Hanna w
        public int? averageGrade; //tracks average grade for chart data - Hanna w

        public SubmitModel(AsyncAcademyContext context)
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

        [ViewData]
        public List<object> notos { get; set; }

        public User? Account { get; set; }

        [BindProperty]
        public Assignment Assignment { get; set; } = default!;

        [BindProperty]
        public Submission Submission { get; set; } = new Submission();

        public bool inputReadOnly = false;

        public async Task<IActionResult> OnGetAsync(int id)
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

            Assignment = await _context.Assignment.FirstOrDefaultAsync(a => a.Id == id);
            if (Assignment == null)
            {
                return NotFound();
            }

            //gathers any previous submissions for the user - Hanna w
            previousSubmissions = [];
            var submittedAssignments = _context.Submissions.Where(a => a.UserId == currentUserID);

            //searches through user submissions for assignment being viewed - Hanna w
            foreach (var s in submittedAssignments)
            {
                if (s.AssignmentId == Assignment.Id)
                {
                    previousSubmissions.Add(s);
                    
                    // Populate previous answer in content field
                    // Make answer uneditable and hide submit button
                    Submission.Content = s.Content;
                    inputReadOnly = true;


                    //checks if assignment has already been graded - Hanna w
                    if (s.PointsGraded >= 0 && currentGrade == null)
                    {
                        //updates current grade - Hanna w
                        currentGrade = s.PointsGraded;
                        break;
                    }
                    if (s.PointsGraded >= 0 && currentGrade < s.PointsGraded)
                    {
                        currentGrade = s.PointsGraded;
                        break;
                    }
                }
            }

            //gather chart data - Hanna w
            var allSubmissions = _context.Submissions.Where(a => a.AssignmentId == id);
            minGrade = allSubmissions.Min(a => a.PointsGraded);
            maxGrade = allSubmissions.Max(a => a.PointsGraded);
            averageGrade = (int?)allSubmissions.Average(a => a.PointsGraded);

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

                notos = new List<object>();
                List<Submission> notifications = await _context.Submissions
                    .Where(e => e.UserId == currentUserID)
                    .Where(n => n.IsNew == true)
                    .ToListAsync();

                if (notifications.Count > 0)
                {
                    Noto notoController = new Noto();
                    notoController.SetViewData(ViewData, notifications.Count);
                    foreach (Submission notification in notifications)
                    {
                        List<object> result = notoController.NotoData(_context, notification);
                        notos.Add(result);
                    }
                }

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // <----- Never gets beyond this check
            {
                return Page();
            }

            Submission.AssignmentId = Assignment.Id; // Assuming you have an AssignmentId in your Submission model
            //New Submission fields added by Chris
            if (HttpContext.Session.GetInt32("CurrentUserId") != null)
            {
                Submission.UserId = (int)HttpContext.Session.GetInt32("CurrentUserId");
            }
            Submission.Timestamp = DateTime.Now;
            Submission.IsNew = false;
            //
            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            var courseID = Assignment.CourseId;

            return RedirectToPage("/Assignments/Submit", new { id = Assignment.Id });
        }
    }
}
