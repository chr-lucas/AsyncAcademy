using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using AsyncAcademy.Pages.Assignments;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestAssignments
    {
        [TestMethod]
        public async Task InstructorCanCreateTextAssignmentTest()
        {
            // Use InMemory database instead of a real SQL Server database
            var options = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;

            // Initialize the InMemory database context
            using (var _context = new AsyncAcademyContext(options))
            {
                // Seed the in-memory database with a mock Course
                var testCourse = new Course
                {
                    InstructorId = 1, // Mock InstructorId (e.g., the instructor's UserId)
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

                _context.Course.Add(testCourse);
                await _context.SaveChangesAsync();

                // Store the course Id
                int testCourseId = testCourse.Id;

                // Mock HttpContext and ISession
                var mockHttpContext = new Mock<HttpContext>();
                var mockSession = new Mock<ISession>();

                var userId = 1; // Mock instructor's userId

                // Convert the userId to a byte array for session mocking
                byte[] userIdBytes = BitConverter.GetBytes(userId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(userIdBytes);
                }
                byte[] userIdResult = userIdBytes;

                // Mock TryGetValue for "CurrentUserId" to return the instructor's UserId
                mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdResult)).Returns(true);

                // Simulate session for courseId
                byte[] courseIdBytes = BitConverter.GetBytes(testCourseId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(courseIdBytes);
                }
                byte[] courseIdResult = courseIdBytes;
                mockSession.Setup(s => s.TryGetValue("CourseId", out courseIdResult)).Returns(true);

                // Assign the mocked session to the mocked HttpContext
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

                // Create a mock PageContext
                var mockPageContext = new Mock<PageContext>();
                mockPageContext.Object.HttpContext = mockHttpContext.Object;

                // Create the PageModel instance
                var pageModel = new CreateModel(_context)
                {
                    PageContext = mockPageContext.Object,
                    Assignment = new Assignment
                    {
                        Title = "TestText",
                        MaxPoints = 10,
                        Description = "Test",
                        Due = DateTime.Parse("2023-10-08 09:00:00"),
                        Type = "Text Entry",
                        CourseId = testCourseId // Link to the mock course
                    }
                };

                // Count how many assignments the course has initially
                int initialAssignmentCount = _context.Assignment.Count(a => a.CourseId == testCourseId);

                // Call OnPostAsync to simulate form submission
                var result = await pageModel.OnPostAsync();

                // Ensure the result is a RedirectToPageResult
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), "Expected a redirect result after creating an assignment.");

                // Count how many assignments the course has after adding the assignment
                int finalAssignmentCount = _context.Assignment.Count(a => a.CourseId == testCourseId);

                // Assert that the number of assignments has increased by one
                Assert.AreEqual(initialAssignmentCount + 1, finalAssignmentCount, "The assignment count should increase by one after creation.");
            }
        }

        [TestMethod]
        public async Task InstructorCanCreateFileAssignmentTest()
        {
            // Use InMemory database instead of a real SQL Server database
            var options = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;

            // Initialize the InMemory database context
            using (var _context = new AsyncAcademyContext(options))
            {
                // Seed the in-memory database with a mock Course
                var testCourse = new Course
                {
                    InstructorId = 1, // Mock InstructorId (e.g., the instructor's UserId)
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

                _context.Course.Add(testCourse);
                await _context.SaveChangesAsync();

                // Store the course Id
                int testCourseId = testCourse.Id;

                // Mock HttpContext and ISession
                var mockHttpContext = new Mock<HttpContext>();
                var mockSession = new Mock<ISession>();

                var userId = 1; // Mock instructor's userId

                // Convert the userId to a byte array for session mocking
                byte[] userIdBytes = BitConverter.GetBytes(userId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(userIdBytes);
                }
                byte[] userIdResult = userIdBytes;

                // Mock TryGetValue for "CurrentUserId" to return the instructor's UserId
                mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdResult)).Returns(true);

                // Simulate session for courseId
                byte[] courseIdBytes = BitConverter.GetBytes(testCourseId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(courseIdBytes);
                }
                byte[] courseIdResult = courseIdBytes;
                mockSession.Setup(s => s.TryGetValue("CourseId", out courseIdResult)).Returns(true);

                // Assign the mocked session to the mocked HttpContext
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

                // Create a mock PageContext
                var mockPageContext = new Mock<PageContext>();
                mockPageContext.Object.HttpContext = mockHttpContext.Object;

                // Create the PageModel instance
                var pageModel = new CreateModel(_context)
                {
                    PageContext = mockPageContext.Object,
                    Assignment = new Assignment
                    {
                        Title = "TestFile",
                        MaxPoints = 10,
                        Description = "Test",
                        Due = DateTime.Parse("2023-10-08 09:00:00"),
                        Type = "File Submission",
                        CourseId = testCourseId // Link to the mock course
                    }
                };

                // Count how many assignments the course has initially
                int initialAssignmentCount = _context.Assignment.Count(a => a.CourseId == testCourseId);

                // Call OnPostAsync to simulate form submission
                var result = await pageModel.OnPostAsync();

                // Ensure the result is a RedirectToPageResult
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), "Expected a redirect result after creating an assignment.");

                // Count how many assignments the course has after adding the assignment
                int finalAssignmentCount = _context.Assignment.Count(a => a.CourseId == testCourseId);

                // Assert that the number of assignments has increased by one
                Assert.AreEqual(initialAssignmentCount + 1, finalAssignmentCount, "The assignment count should increase by one after creation.");
            }
        }

        [TestMethod]
        public async Task InstructorCanCreateURltAssignmentTest()
        {
            // Use InMemory database instead of a real SQL Server database
            var options = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;

            // Initialize the InMemory database context
            using (var _context = new AsyncAcademyContext(options))
            {
                // Seed the in-memory database with a mock Course
                var testCourse = new Course
                {
                    InstructorId = 1, // Mock InstructorId (e.g., the instructor's UserId)
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

                _context.Course.Add(testCourse);
                await _context.SaveChangesAsync();

                // Store the course Id
                int testCourseId = testCourse.Id;

                // Mock HttpContext and ISession
                var mockHttpContext = new Mock<HttpContext>();
                var mockSession = new Mock<ISession>();

                var userId = 1; // Mock instructor's userId

                // Convert the userId to a byte array for session mocking
                byte[] userIdBytes = BitConverter.GetBytes(userId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(userIdBytes);
                }
                byte[] userIdResult = userIdBytes;

                // Mock TryGetValue for "CurrentUserId" to return the instructor's UserId
                mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdResult)).Returns(true);

                // Simulate session for courseId
                byte[] courseIdBytes = BitConverter.GetBytes(testCourseId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(courseIdBytes);
                }
                byte[] courseIdResult = courseIdBytes;
                mockSession.Setup(s => s.TryGetValue("CourseId", out courseIdResult)).Returns(true);

                // Assign the mocked session to the mocked HttpContext
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

                // Create a mock PageContext
                var mockPageContext = new Mock<PageContext>();
                mockPageContext.Object.HttpContext = mockHttpContext.Object;

                // Create the PageModel instance
                var pageModel = new CreateModel(_context)
                {
                    PageContext = mockPageContext.Object,
                    Assignment = new Assignment
                    {
                        Title = "TestURL",
                        MaxPoints = 10,
                        Description = "Test",
                        Due = DateTime.Parse("2023-10-08 09:00:00"),
                        Type = "URL",
                        CourseId = testCourseId // Link to the mock course
                    }
                };

                // Count how many assignments the course has initially
                int initialAssignmentCount = _context.Assignment.Count(a => a.CourseId == testCourseId);

                // Call OnPostAsync to simulate form submission
                var result = await pageModel.OnPostAsync();

                // Ensure the result is a RedirectToPageResult
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), "Expected a redirect result after creating an assignment.");

                // Count how many assignments the course has after adding the assignment
                int finalAssignmentCount = _context.Assignment.Count(a => a.CourseId == testCourseId);

                // Assert that the number of assignments has increased by one
                Assert.AreEqual(initialAssignmentCount + 1, finalAssignmentCount, "The assignment count should increase by one after creation.");
            }
        }
    }
}
