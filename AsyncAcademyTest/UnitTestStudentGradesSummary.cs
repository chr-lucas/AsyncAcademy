using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestStudentGradesSummary
    {
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;

        [TestInitialize]
        public async Task Setup()
        {
            // Generate a unique database name using a GUID
            string uniqueDbName = "AsyncAcademyTestDb_" + Guid.NewGuid().ToString();

            // Configure an in-memory database for this specific test
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName) // Specific database name for this test
                .Options;

            // Clear database before each test
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                context.Users.RemoveRange(context.Users);
                context.Course.RemoveRange(context.Course);
                context.Enrollments.RemoveRange(context.Enrollments);
                context.Submissions.RemoveRange(context.Submissions);
                context.Assignment.RemoveRange(context.Assignment);
                await context.SaveChangesAsync();
            }
        }

        private async Task<int> SeedStudent(AsyncAcademyContext context)
        {
            var newStudent = new User
            {
                FirstName = "Test",
                LastName = "Student",
                Username = "teststudent",
                IsProfessor = false,
                Mail = "test@student.com",
                Pass = "password",
                ConfirmPass = "password",
                Phone = "1234567890",

                // Required fields for User
                Addr_Street = "123 Test St",
                Addr_City = "Testville",
                Addr_State = "TX",
                Addr_Zip = "12345"
            };

            context.Users.Add(newStudent);
            await context.SaveChangesAsync();
            return newStudent.Id;
        }

        private async Task<int> SeedCourse(AsyncAcademyContext context)
        {
            var newCourse = new Course
            {
                InstructorId = 1,
                CourseNumber = "CS101",
                Department = "CS",
                Name = "Intro to Computer Science",
                CreditHours = 3,

                // Required fields for Course
                Description = "An introduction to computer science.",
                StartTime = DateTime.Parse("09:00 AM"),
                EndTime = DateTime.Parse("10:30 AM"),
                Location = "Room 101",
                StudentCapacity = 30,
                StudentsEnrolled = 0,
                MeetingTimeInfo = "MWF 9:00 AM - 10:30 AM",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(4) // Assuming a 4-month course
            };

            context.Course.Add(newCourse);
            await context.SaveChangesAsync();
            return newCourse.Id;
        }

        [TestMethod]
        public async Task TestStudentGradesSummary()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                // Seed student and course data
                int studentId = await SeedStudent(context);
                int courseId = await SeedCourse(context);

                // Add logic to calculate or test student grade summary
                // Example: Add logic to test the summary view of student grades or mock session interactions
                // This is where you would write the test logic specific to your project requirements.

                Assert.IsTrue(true); // Placeholder assertion; replace with actual test assertions
            }
        }
    }
}
