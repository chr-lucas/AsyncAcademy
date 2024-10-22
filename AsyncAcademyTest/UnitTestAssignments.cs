// Using necessary namespaces for data access, models, ASP.NET Core, and unit testing frameworks
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
using AsyncAcademy.Pages.Assignments; // Razor Page for creating assignments

namespace AsyncAcademyTest
{
    // Test class for unit testing the Assignments-related functionality
    [TestClass]
    public class UnitTestAssignments
    {
        // Private fields to hold test-specific configurations and mock objects
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions; // Options for configuring in-memory test DB
        private Mock<HttpContext> _mockHttpContext; // Mocking HttpContext for session access
        private Mock<ISession> _mockSession; // Mocking session to simulate session variables
        private Mock<PageContext> _mockPageContext; // Mocking PageContext for Razor Page interactions

        // Setup method to initialize mocks and prepare the in-memory database
        [TestInitialize]
        public async Task Setup()
        {
            // Generate a unique database name using a GUID
            string uniqueDbName = "AsyncAcademyTestDb_" + Guid.NewGuid().ToString();

            // Configure an in-memory database for this specific test
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName) // Specific database name for this test
                .Options;

            // Set up mocked HttpContext and Session
            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new Mock<ISession>();
            _mockHttpContext.Setup(m => m.Session).Returns(_mockSession.Object); // Linking HttpContext with the mocked session

            // Setting up PageContext to use the mocked HttpContext
            _mockPageContext = new Mock<PageContext>();
            _mockPageContext.Object.HttpContext = _mockHttpContext.Object;

            // Clear any existing data from the in-memory database (for a clean start in tests)
            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                _context.Users.RemoveRange(_context.Users); // Remove all users
                await _context.SaveChangesAsync(); // Save changes to the database
            }
        }

        // Helper method to seed a test course into the database
        private async Task<int> SeedCourse(AsyncAcademyContext context)
        {
            var testCourse = new Course
            {
                InstructorId = 1, // Assuming instructor ID 1
                CourseNumber = "101A",
                Department = "CS", // Computer Science department
                Name = "Introduction to Programming", // Course details
                Description = "An introductory course to programming with C#.",
                CreditHours = 3,
                StartTime = DateTime.Parse("09:00:00"),
                EndTime = DateTime.Parse("11:00:00"),
                Location = "Room 101",
                StudentCapacity = 30, // Maximum number of students allowed
                StudentsEnrolled = 0,
                MeetingTimeInfo = "MWF 9:00AM - 11:00AM", // Class schedule
                StartDate = DateTime.Now, // Current date
                EndDate = DateTime.Now.AddMonths(4) // Course ends in 4 months
            };

            // Add the course to the database and save it
            context.Course.Add(testCourse);
            await context.SaveChangesAsync();
            return testCourse.Id; // Return the ID of the created course for testing
        }

        // Helper method to create the Razor Page model for assignment creation
        private CreateModel CreateAssignmentModel(AsyncAcademyContext context, int courseId, string assignmentType)
        {
            return new CreateModel(context)
            {
                // Inject the mocked PageContext and initialize the assignment details
                PageContext = _mockPageContext.Object,
                Assignment = new Assignment
                {
                    Title = $"Test{assignmentType}", // Assignment title based on type
                    MaxPoints = 10, // Maximum points for the assignment
                    Description = "Test", // Description of the assignment
                    Due = DateTime.Parse("2023-10-08 09:00:00"), // Due date for the assignment
                    Type = assignmentType, // Assignment type (Text, File, URL)
                    CourseId = courseId // Course ID associated with the assignment
                }
            };
        }

        // Helper method to simulate session variables (like User ID and Course ID)
        private void MockSessionValues(int userId, int courseId)
        {
            // Simulate storing the user ID in the session
            byte[] userIdBytes = BitConverter.GetBytes(userId);
            if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes); // Handle endianness
            _mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdBytes)).Returns(true); // Mock session value

            // Simulate storing the course ID in the session
            byte[] courseIdBytes = BitConverter.GetBytes(courseId);
            if (BitConverter.IsLittleEndian) Array.Reverse(courseIdBytes);
            _mockSession.Setup(s => s.TryGetValue("CourseId", out courseIdBytes)).Returns(true);
        }

        // Helper method to verify that the assignment creation process works
        private async Task VerifyAssignmentCreation(AsyncAcademyContext context, int courseId, string assignmentType)
        {
            // Count the number of assignments before creation
            int initialCount = context.Assignment.Count(a => a.CourseId == courseId);

            // Create and submit the assignment using the CreateModel
            var pageModel = CreateAssignmentModel(context, courseId, assignmentType);
            var result = await pageModel.OnPostAsync(); // Call the OnPostAsync method to simulate form submission

            // Assert that the result is a redirect (indicating successful assignment creation)
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), $"Expected a redirect after creating {assignmentType} assignment.");

            // Count the number of assignments after creation and assert it has increased by 1
            int finalCount = context.Assignment.Count(a => a.CourseId == courseId);
            Assert.AreEqual(initialCount + 1, finalCount, $"Assignment count should increase by one after creating {assignmentType} assignment.");
        }

        // Test method to verify that an instructor can create a "Text Entry" assignment
        [TestMethod]
        public async Task InstructorCanCreateTextAssignmentTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int courseId = await SeedCourse(context); // Seed a course for testing
                MockSessionValues(1, courseId); // Mock session data for instructor
                await VerifyAssignmentCreation(context, courseId, "Text Entry"); // Verify assignment creation for "Text Entry"
            }
        }

        // Test method to verify that an instructor can create a "File Submission" assignment
        [TestMethod]
        public async Task InstructorCanCreateFileAssignmentTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int courseId = await SeedCourse(context); // Seed a course for testing
                MockSessionValues(1, courseId); // Mock session data for instructor
                await VerifyAssignmentCreation(context, courseId, "File Submission"); // Verify assignment creation for "File Submission"
            }
        }

        // Test method to verify that an instructor can create a "URL" assignment
        [TestMethod]
        public async Task InstructorCanCreateURLAssignmentTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int courseId = await SeedCourse(context); // Seed a course for testing
                MockSessionValues(1, courseId); // Mock session data for instructor
                await VerifyAssignmentCreation(context, courseId, "URL"); // Verify assignment creation for "URL"
            }
        }
    }
}
