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
        public double overallGradePercentage; // Placeholder to ensure that zeros don't cause a 0% overall

        public double OverallMaximumPoints = 0;

        // Additional properties for letter grade
        public string LetterGrade { get; set; }  // To store the final letter grade

        // Gather class data for chart
        public double? averageStudentScore = 0;
        public double? topStudentScore = 0;
        public double? bottomStudentScore = 0;

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            Course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            Submissions = _context.Submissions.Where(a => a.UserId == currentUserID).ToList();

            foreach (var submission in Submissions)
            {
                CorrespondingAssignments.Add(await _context.Assignment.FirstOrDefaultAsync(a => a.Id == submission.AssignmentId));
            }

            for (int i = 0; i < Submissions.Count; i++)
            {
                Submission submission = Submissions[i];
                Assignment assignment = CorrespondingAssignments[i];
                if (submission.PointsGraded == null)
                {
                    continue;
                }
                overallGradePercentage += (double)submission.PointsGraded;
                OverallMaximumPoints += assignment.MaxPoints;
            }

            if (OverallMaximumPoints == 0)
            {
                OverallGrade = -1;
                LetterGrade = "N/A"; // No letter grade when no assignments are graded
            }
            else
            {
                overallGradePercentage /= OverallMaximumPoints;
                OverallGrade = (overallGradePercentage * 100);
                LetterGrade = GetLetterGrade(OverallGrade);  // Calculate letter grade based on percentage
            }

            // Chart data
            var allAssignments = _context.Assignment.Where(a => a.CourseId == courseId).ToList();
            var allStudents = _context.Enrollments.Where(a => a.CourseId == courseId).Select(a => a.UserId).ToList();
            List<double?> allScores = new List<double?> { OverallGrade };

            double? studentScore = 0;
            double? finalScore = 0;

            foreach (var s in allStudents)
            {
                studentScore = 0;
                finalScore = 0;

                var studentSubmissions = _context.Submissions.Where(a => a.UserId == s).ToList();

                foreach (var sub in studentSubmissions)
                {
                    foreach (var a in allAssignments)
                    {
                        if (sub.AssignmentId == a.Id)
                        {
                            if (sub.PointsGraded == null)
                            {
                                break;
                            }
                            else
                            {
                                studentScore += sub.PointsGraded;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                finalScore += (studentScore / OverallMaximumPoints);
                studentScore = finalScore * 100;
                allScores.Add(studentScore);
            }

            bottomStudentScore = allScores.Min();
            topStudentScore = allScores.Max();
            averageStudentScore = allScores.Average();

            return Page();
        }

        // Method to calculate letter grade based on overall percentage
        public string GetLetterGrade(double overallGradePercentage)
        {
            if (overallGradePercentage >= 93)
                return "A";
            else if (overallGradePercentage >= 90)
                return "A-";
            else if (overallGradePercentage >= 87)
                return "B+";
            else if (overallGradePercentage >= 83)
                return "B";
            else if (overallGradePercentage >= 80)
                return "B-";
            else if (overallGradePercentage >= 77)
                return "C+";
            else if (overallGradePercentage >= 73)
                return "C";
            else if (overallGradePercentage >= 70)
                return "C-";
            else if (overallGradePercentage >= 67)
                return "D+";
            else if (overallGradePercentage >= 63)
                return "D";
            else if (overallGradePercentage >= 60)
                return "D-";
            else
                return "F";
        }
    }
}
