using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using System.Threading.Tasks;

namespace AsyncAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AsyncAcademyContext _context;

        public CourseController(AsyncAcademyContext context)
        {
            _context = context;
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollInCourse([FromBody] Enrollment enrollment)
        {
            if (enrollment == null)
            {
                return BadRequest("Enrollment data is required.");
            }

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            // Optionally, you can call a method to run queries or update user data here
            await RunQueriesForUser(enrollment.UserId);

            return Ok(new { message = "Successfully enrolled in the course." });
        }

        [HttpPost("drop")]
        public async Task<IActionResult> DropCourse([FromBody] DropCourseRequest request)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == request.UserId && e.CourseId == request.CourseId);

            if (enrollment == null)
            {
                return NotFound("Enrollment not found.");
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            // Optionally, you can call a method to run queries or update user data here
            await RunQueriesForUser(request.UserId);

            return Ok(new { message = "Successfully dropped the course." });
        }

        private async Task RunQueriesForUser(int userId)
        {
            // Implement your logic to run queries related to the user
        }
    }

    public class DropCourseRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
