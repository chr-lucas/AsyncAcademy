using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

//Submit assignment page for File Submission assignments, student view
//Code below written by Hanna Whitney unless notated otherwise

namespace AsyncAcademy.Pages.Assignments
{
    public class SubmitFileModel(AsyncAcademy.Data.AsyncAcademyContext context, IWebHostEnvironment environment) : PageModel
    {

        private readonly AsyncAcademyContext _context = context;
        private IWebHostEnvironment _environment = environment;
        public List<Submission> previousSubmissions;
        public int? currentGrade;
        public int? minGrade;
        public int? maxGrade;
        public int? averageGrade;

        public User Account { get; set; }

        [ViewData]
        public string NavBarLink { get; set; }

        [ViewData]
        public string NavBarText { get; set; }

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public List<object> notos { get; set; }

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        public Assignment Assignment { get; set; }

        public Submission Submission { get; set; } = new Submission();


        public IFormFile FileSubmission { get; set; }

        public string FileName { get; set; }

        public bool fileSubmitted = false;
        public Submission previousSubmission;
        public string prevSubFilePath;



        public async void OnGetAsync(int id)
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

                Noto notoController = new Noto();
                notoController.SetViewData(ViewData, notifications.Count);

                if (notifications.Count > 0)
                {
                    foreach (Submission notification in notifications)
                    {
                        List<object> result = notoController.NotoData(_context, notification);
                        notos.Add(result);
                    }
                }

            }

            Assignment = _context.Assignment.FirstOrDefault(a => a.Id == id);

            if (Assignment == null)
            {
                NotFound();
            }

            Submission userSub = await _context.Submissions.Where(a => a.UserId == currentUserID).Where(s => s.AssignmentId == Assignment.Id).FirstAsync();
            if (userSub != null)
            {
                if (userSub.IsNew == true)
                {
                    userSub.IsNew = false;
                    _context.Submissions.Update(userSub);
                    await _context.SaveChangesAsync();
                }
            }

            //gathers any previous submissions for the user
            previousSubmissions = [];
            var submittedAssignments = _context.Submissions.Where(a => a.UserId == currentUserID);

            foreach (var s in submittedAssignments)
            {
                if (s.AssignmentId == Assignment.Id && s.IsNew == true)
                {
                    s.IsNew = false;
                    _context.SaveChanges();
                }
            }
            
            //Gather grade
            GetUserGrade(submittedAssignments);

            //gather chart data
            var allSubmissions = _context.Submissions.Where(a => a.AssignmentId == id);
            minGrade = allSubmissions.Min(a => a.PointsGraded);
            maxGrade = allSubmissions.Max(a => a.PointsGraded);
            averageGrade = (int?)allSubmissions.Average(a => a.PointsGraded);

        }

        //gathers grades for submitted assignments
        private void GetUserGrade(IQueryable<Submission> submittedAssignments)
        {
            //searches through user submissions for assignment being viewed
            foreach (var s in submittedAssignments)
            {
                if (s.AssignmentId == Assignment.Id)
                {
                    previousSubmissions.Add(s);

                    //checks if assignment has already been graded
                    if (s.PointsGraded >= 0 && currentGrade == null)
                    {
                        //updates current grade
                        currentGrade = s.PointsGraded;
                        break;
                    }
                    if (s.PointsGraded >= 0 && currentGrade < s.PointsGraded)
                    {
                        currentGrade = s.PointsGraded;
                        break;
                    }

                    // Set flag to show previous submission on page
                    fileSubmitted = true;
                    previousSubmission = s;
                    prevSubFilePath = "." + s.Content.ToString();
                }
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
            Submission.AssignmentId = id;
            Submission.UserId = (int)currentUserID;
            Submission.Timestamp = DateTime.Now;
            Submission.IsNew = false;

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

                //save dbPath as submission content
                // This is the relative file path from the webroot
                // To access elsewhere, preceed database query with ~
                Submission.Content = dbPath;
            }

            //Add new submission to database
            _context.Submissions.Add(Submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Assignments/SubmitFile", new { id = Assignment.Id });

        }



    }
}
