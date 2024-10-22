using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Pages;
using AsyncAcademy.Pages.Assignments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAcademyTest
{
    // Test class to ensure that assignments are added to To-do list if the due date has not passed - Hanna W
    [TestClass]
    public class UnitTestAddToToDoList
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

        // Helper method to seed an assignment for testing
        private async Task<int> SeedAssignment(AsyncAcademyContext context)
        {
            var testAssignment = new Assignment
            {
                Id = 1, // Placeholder ID
                CourseId = 1, // Placeholder ID 
                Title = "Test Assignment",
                Description = "An assignment to test submissions",
                MaxPoints = 10,
                Due = DateTime.Now.AddDays(3), // Due date is three days in the future
                Type = "Text Entry"
            };

            context.Assignment.Add(testAssignment); // Add the assignment to the context
            await context.SaveChangesAsync(); // Save changes to the in-memory DB
            return testAssignment.Id; // Return the ID of the created assignment
        }

        // Helper method to seed an enrollment for testing
        private async Task<int> SeedEnrollment(AsyncAcademyContext context)
        {
            var testEnrollment = new Enrollment
            {
                UserId = 1, // Placeholder user ID
                CourseId = 1 // Placeholder course ID
            };

            context.Enrollments.Add(testEnrollment); // Add the enrollment to the context
            await context.SaveChangesAsync(); // Save changes
            return testEnrollment.Id; // Return the ID of the created enrollment
        }

        // Helper method to seed a user for testing
        private async Task<int> SeedUser(AsyncAcademyContext context)
        {
            var testUser = new User
            {
                Id = 1, // Placeholder ID
                Username = "Test",
                FirstName = "Test",
                LastName = "Test",
                Mail = "test@test.com",
                Pass = "TestPass123",
                ConfirmPass = "TestPass123",
                Birthday = new DateTime(1980, 5, 20),
                IsProfessor = false,
                Addr_City = "Test City",
                Addr_State = "Test State",
                Addr_Zip = "12345",
                Addr_Street = "123 Test St",
                Phone = "123-456-7890"
            };

            context.Users.Add(testUser); // Add the user to the context
            await context.SaveChangesAsync(); // Save changes
            return testUser.Id; // Return the ID of the created user
        }

        // Helper method to create a WelcomeModel for testing Razor Page behavior
        private WelcomeModel CreateWelcomeModel(AsyncAcademyContext context)
        {
            return new WelcomeModel(context)
            {
                PageContext = _mockPageContext.Object, // Mocked page context
                EnrolledCourses = new List<Course>
                {
                    new Course
                    {
                        Id = 1,
                        InstructorId = 0,
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
                    }
                },
                Account = context.Users.FirstOrDefault(a => a.Id == 1) // Retrieve the first user in the database
            };
        }

        // Helper method to verify that an assignment was added to the To-Do list
        private async Task VerifyToDoAddition(AsyncAcademyContext context)
        {
            // Create the welcome model with the context
            var welcomeModel = CreateWelcomeModel(context);
            // Run the OnGetAsync method to populate the To-Do list
            var setup = await welcomeModel.OnGetAsync();
            // Count the initial number of To-Do items
            int initialCount = welcomeModel.ToDoList.Count;

            // Add a new assignment to the context, which should increase the To-Do list count
            Assignment newAssignment = new Assignment
            {
                Id = 2, // Placeholder ID
                CourseId = 1, // Placeholder course ID
                Title = "Test Assignment 2",
                Description = "An assignment to test submissions",
                MaxPoints = 10,
                Due = DateTime.Now.AddDays(3), // Due date is three days in the future
                Type = "Text Entry"
            };
            context.Assignment.Add(newAssignment); // Add the new assignment to the context
            await context.SaveChangesAsync(); // Save changes to the DB

            // Recount the To-Do list after running OnGetAsync again
            var result = await welcomeModel.OnGetAsync();
            int finalCount = welcomeModel.ToDoList.Count;

            // Assert that the To-Do list count has increased by one
            Assert.AreEqual(initialCount + 1, finalCount, "To-Do list should include assignments before their due date.");
        }

        // Test method to ensure that a new assignment is added to the To-Do list
        [TestMethod]
        public async Task NewAssignmentAddedToToDoListTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                // Seed an assignment for testing
                int assignmentId = await SeedAssignment(context);
                // Seed enrollment for the user
                int enrollmentID = await SeedEnrollment(context);
                // Seed the test user
                int userID = await SeedUser(context);
                // Simulate session values for the user and course
                MockSessionValues(userID, 1);
                // Run the verification method to check if the To-Do list is updated
                await VerifyToDoAddition(context);
            }
        }
    }
}
