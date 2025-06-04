using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq; // Moq is used for mocking objects in the unit tests
using System;
using System.Linq;
using System.Threading.Tasks;
using AsyncAcademy.Pages.Course_Pages; // Razor Page for creating assignments


namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestStudentEnrollment
    {
        // Private fields to hold test-specific configurations and mock objects
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions; // Options for configuring in-memory test DB
        private Mock<HttpContext> _mockHttpContext; // Mocking HttpContext for session access
        private Mock<ISession> _mockSession; // Mocking session to simulate session variables
        private Mock<PageContext> _mockPageContext; // Mocking PageContext for Razor Page interactions

        //Setup method to initialize mocks and prepare the in-memory database:
        [TestInitialize]
        public async Task SetUp()
        {
            // Generate a unique database name using a GUID
            string uniqueDbName = "AsyncAcademyTestDb_" + Guid.NewGuid().ToString();

            // Configure an in-memory database for this specific test
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName) // Specific database name for this test
                .Options;


            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new Mock<ISession>();
            _mockHttpContext.Setup(m => m.Session).Returns(_mockSession.Object); // Linking HttpContext with the mocked session

            //Setting up PageContext to use the mocked HttpContext:
            _mockPageContext = new Mock<PageContext>();
            _mockPageContext.Object.HttpContext = _mockHttpContext.Object;


            //Clear any existing data from the in-memory database (for a clean start in tests)
            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                _context.Enrollments.RemoveRange(_context.Enrollments);
                await _context.SaveChangesAsync();
            }
        }





        //Helper method to seed the db:
        private async Task<int> SeedCourse(AsyncAcademyContext context)
        {
            var testCourse = new Course
            {
                InstructorId = 1, 
                CourseNumber = "101A",
                Department = "CS", 
                Name = "Introduction to Programming", 
                Description = "An introductory course to programming with C#.",
                CreditHours = 3,
                StartTime = DateTime.Parse("09:00:00"),
                EndTime = DateTime.Parse("11:00:00"),
                Location = "Room 101",
                StudentCapacity = 30, 
                StudentsEnrolled = 0,
                MeetingTimeInfo = "MWF 9:00AM - 11:00AM", 
                StartDate = DateTime.Now, 
                EndDate = DateTime.Now.AddMonths(4) 
            };



            //Add the course and the student to the db and save it:
            context.Course.Add(testCourse);
            await context.SaveChangesAsync();

            return testCourse.Id;

        }


        //Helper method to seed the student that wil enroll in the above course:
        private async Task<int> SeedStudent(AsyncAcademyContext context)
        {
            var testStudent = new User
            {
                Id = 1,
                Username = "testUser",
                FirstName = "testUser",
                LastName = "tester",
                Mail = "test@gmail.com",
                Pass = "Password",
                ConfirmPass = "Password",
                Birthday = new DateTime(1980, 5, 20),
                IsProfessor = false,
                Addr_City = "Test City",
                Addr_State = "Test State",
                Addr_Zip = "12345",
                Addr_Street = "123 Test St",
                Phone = "123-456-7890"

            };

            context.Users.Add(testStudent);
            await context.SaveChangesAsync();

            return testStudent.Id;

        }

        private EnrollModel CreateTestEnrollment(AsyncAcademyContext context, int courseId, int userId)
        {
            // Return the EnrollModel with the mock PageContext
            return new EnrollModel(context)
            {
                PageContext = _mockPageContext.Object
            };
        }

        // Helper method to simulate session variables (like User ID and Course ID)
        private void MockSessionValues(int userId, int courseId)
        {
            // Simulate storing the user ID in the session:
            byte[] userIdBytes = BitConverter.GetBytes(userId);
            if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes); // Handle endianness
            _mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdBytes)).Returns(true); // Mock session value

            // Simulate storing the course ID in the session:
            byte[] courseIdBytes = BitConverter.GetBytes(courseId);
            if (BitConverter.IsLittleEndian) Array.Reverse(courseIdBytes);
            _mockSession.Setup(s => s.TryGetValue("CourseId", out courseIdBytes)).Returns(true);
        }



        //Method to verify that student was successfully enrolled in the course:
        private async Task VerifyEnrollment(AsyncAcademyContext context, int userId, int courseId)
        {
            var enrollment = await context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            Assert.IsNotNull(enrollment, "Enrollment should exist after successful enrollment.");
            Assert.AreEqual(userId, enrollment.UserId, "The user ID should match the enrolled student's ID.");
            Assert.AreEqual(courseId, enrollment.CourseId, "The course ID should match the enrolled course.");
        }


        [TestMethod]
        public async Task StudentCanEnrollInCourseTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                //We create a course and a student thtough our seed method:
                int courseId = await SeedCourse(context);
                int studentId = await SeedStudent(context);

                // Mock the session values for the current student
                MockSessionValues(studentId, courseId);

                //We create the enrollment model and simulate the enrollment process:
                var pageModel = CreateTestEnrollment(context, courseId, studentId);
                var result = await pageModel.OnPostAsync(courseId);//Here's where the actual enrollment happens

                //Verify that the enrollment was successful:
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), "Enrollment should redirect on success");
                await VerifyEnrollment(context, studentId, courseId);
            }
        }

    }
}
