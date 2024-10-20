using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace AsyncAcademy.Pages
{
    public class CourseGradesModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public CourseGradesModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public List<Submission> Submissions = new List<Submission>();
        public List<Assignment> CorrespondingAssignments = new List<Assignment>();

        public Course Course { get; set; }

        public double OverallGrade = 0;
        public double overallGradePercentage; //place holder to ensure that zeros dont cause a 0% overall - Hanna W

        public double OverallMaximumPoints = 0; 

        //gather class data for chart - Hanna W
        public double? averageStudentScore = 0;
        public double? topStudentScore = 0;
        public double? bottomStudentScore = 0;

        public async Task<IActionResult> OnGetAsync(int courseId) {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            Submissions = _context.Submissions.Where(a => a.UserId == currentUserID).ToList();

            foreach (var submission in Submissions) {
                CorrespondingAssignments.Add(await _context.Assignment.FirstOrDefaultAsync(a => a.Id == submission.AssignmentId));
            }

            for (int i = 0; i < Submissions.Count; i++) {
                Submission submission = Submissions[i];
                Assignment assignment = CorrespondingAssignments[i];
                if (submission.PointsGraded == null)
                {
                    continue;
                }
                overallGradePercentage += (double) submission.PointsGraded; //changed to account for grade being a percentage - Hanna W
                OverallMaximumPoints += assignment.MaxPoints; //changed to account for grade being a percentage - Hanna W
            }

            if (OverallMaximumPoints == 0)
            {
                OverallGrade = -1;
            }
            else
            {
                overallGradePercentage /= OverallMaximumPoints;
                OverallGrade = (overallGradePercentage * 100);
            }

            //Chart data - Hanna W
            //gathers all assignments associated with course
            var allAssignments = _context.Assignment.Where(a => a.CourseId == courseId).ToList();
            //gathers all students enrolled in the course
            var allStudents = _context.Enrollments.Where(a => a.CourseId == courseId).Select(a => a.UserId).ToList();
            //a list to store all scores from al students for this course
            List<double?> allScores = new List<double?>();
            allScores.Add(OverallGrade);

            //stores grade value
            double? studentScore = 0;
            double? finalScore = 0;

            //gathers grade for each student
            foreach (var s in allStudents)
            {
                //resets tracking values for each student
                studentScore = 0;
                finalScore = 0;

                //pulls all submissions for student
                var studentSubmissions = _context.Submissions.Where(a => a.UserId == s).ToList();

                //checks that each submissions correlates to assignments for this course
                foreach (var sub in studentSubmissions)
                {
                    foreach (var a in allAssignments)
                    {
                        if (sub.AssignmentId == a.Id)
                        {
                            //if assignment is not graded, skip
                            if (sub.PointsGraded == null)
                            {
                                break;
                            }
                            //adds submissions score to total student score
                            else
                            {
                                studentScore += sub.PointsGraded;
                            }
                        } else
                        {
                            break;
                        }
                    }
                }
                //calculate total class grade for student and store in list
                finalScore += (studentScore / OverallMaximumPoints);
                studentScore =  finalScore * 100;
                allScores.Add(studentScore);
            }

            //gets bottom score from all scores
            bottomStudentScore = allScores.Min();
            //gets top score from all scores
            topStudentScore = allScores.Max();
            //gets average of all scores
            averageStudentScore = allScores.Average();

            return Page();
        }
    }
}
