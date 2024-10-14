using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Course_Pages
{
    public class EnrollModel : PageModel
    {
        private readonly AsyncAcademyContext _context;

        public EnrollModel(AsyncAcademyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(int courseId)
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            // Check if enrollment already exists
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == currentUserID && e.CourseId == courseId);
            if (existingEnrollment != null)
            {
                return RedirectToPage("./StudentIndex");
            }

            // Create new enrollment
            var enrollment = new Enrollment
            {
                UserId = currentUserID.Value,
                CourseId = courseId
            };

            _context.Enrollments.Add(enrollment);


            // Get course details to calculate StudentPayment
            var course = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }

            //decimal courseCost = course.CreditHours * 100; // Assuming 100 USD per credit hour

            ////Check if student already has a StudentPayment record
            //var StudentPaymentnRecord = await _context.Payments
            //    .FirstOrDefaultAsync(t => t.UserId == currentUserID);

            //if (StudentPaymentnRecord != null)
            //{
            //    // Update existing StudentPayment record
            //    StudentPaymentnRecord.AmountPaid += courseCost;
            //    StudentPaymentnRecord.Timestamp = DateTime.Now;
            //    _context.Payments.Update(StudentPaymentnRecord);
            //}
            //else
            //{
            //    // Create new StudentPayment record
            //    var newStudentPayment = new Payment
            //    {
            //        UserId = currentUserID.Value,
            //        AmountPaid = courseCost,
            //        Timestamp = DateTime.Now
            //    };
            //    _context.Payments.Add(newStudentPayment);
            //}




            await _context.SaveChangesAsync();

            return RedirectToPage("./StudentIndex");
        }
    }
}
