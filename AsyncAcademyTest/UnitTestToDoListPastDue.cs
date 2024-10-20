using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Pages;
using AsyncAcademy.Pages.Assignments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAcademyTest
{
    //Test class to ensure that assignments are removed from To-do list if due date has passed - Hanna W
    [TestClass]
    public class UnitTestToDoListPastDue
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
            // Configure an in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb") // Test database name
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

        //Creates a seed assignment
        private async Task<int> SeedAssignment(AsyncAcademyContext context)
        {
            var testAssignment = new Assignment
            {
                Id = 1, //place holder ID
                CourseId = 1, //place holder ID 
                Title = "Test Assignment",
                Description = "An assignment to test submissions",
                MaxPoints = 10,
                Due = DateTime.Now.AddDays(3), //Due date is three days in the future to ensure that assignment is added to list
                Type = "Text Entry"
            };

            context.Assignment.Add(testAssignment);
            await context.SaveChangesAsync();
            return testAssignment.Id;
        }

        private async Task<int> SeedEnrollment(AsyncAcademyContext context)
        {
            var testEnrollment = new Enrollment
            {
                UserId = 1,
                CourseId = 1
            };

            context.Enrollments.Add(testEnrollment);
            await context.SaveChangesAsync();

            return testEnrollment.Id;
        }

        private async Task<int> SeedUser(AsyncAcademyContext context)
        {
            var testUser = new User
            {
                Id = 1,
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

            context.Users.Add(testUser);
            await context.SaveChangesAsync();

            return testUser.Id;
        }

        private WelcomeModel CreateWelcomeModel(AsyncAcademyContext context)
        {
            return new WelcomeModel(context)
            {
                PageContext = _mockPageContext.Object,
                EnrolledCourses = [new Course
                {
                    Id = 1,
                    InstructorId = 0, // This will be overwritten
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
                }],
                Account = context.Users.FirstOrDefault(a => a.Id == 1)


            };
        }

        private async Task VerifyToDoPastDue(AsyncAcademyContext context)
        {
            //creates welcomeModel
            var welcomeModel = CreateWelcomeModel(context);
            //Runs onGetAsync method, where todo list is built
            var setup = await welcomeModel.OnGetAsync();
            //counts todo list items
            int initialCount = welcomeModel.ToDoList.Count;

            //Changing due date on existing assignment so that it is removed from the todo list
            foreach (var a in context.Assignment)
            {
                if (a.Id == 1)
                {
                    a.Due = DateTime.Now.AddDays(-2);
                    context.Assignment.Update(a);

                }
            }
            //save changes to assignment due date
            await context.SaveChangesAsync();


            //rebuilding todo list
            var result = await welcomeModel.OnGetAsync();
            //recount todo list by running ongetasync again and 
            int finalCount = welcomeModel.ToDoList.Count;

            //test if todo list was updated to remove past due assignment
            Assert.AreEqual(initialCount - 1 , finalCount, $"To-Do list should only include assignments prior to assignment due date.");
        }

        [TestMethod]
        public async Task NewAssignmentPastDueToDoListTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                //seeds assignment 
                int assignmentId = await SeedAssignment(context);
                //seeds enrollment
                int enrollmentID = await SeedEnrollment(context);
                //seeds user
                int userID = await SeedUser(context);
                //adds user ID and course ID values to session
                MockSessionValues(userID, 1);
                //run test
                await VerifyToDoPastDue(context);
            }
        }




    }
}
