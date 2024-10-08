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
using AsyncAcademy.Pages.Course_Pages;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task InstructorCanCreateCourseTest()
        {
            // Use InMemory database instead of a real SQL Server database
            var options = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;

            // Initialize the InMemory database context
            using (var _context = new AsyncAcademyContext(options))
            {
                // Seed the in-memory database with a mock instructor
                var instructor = new User
                {
                    Id = 1,
                    Username = "professor123",
                    FirstName = "John",
                    LastName = "Doe",
                    Mail = "john.doe@example.com",
                    Pass = "ValidPassword1",
                    ConfirmPass = "ValidPassword1", // Password confirmation must match
                    Birthday = new DateTime(1980, 5, 20), // Must pass custom validation logic
                    IsProfessor = true,
                    Addr_Street = "123 Main St",
                    Addr_City = "Springfield",
                    Addr_State = "UT",
                    Addr_Zip = "12345",
                    Phone = "0123456789",
                    ProfilePath = "/images/default_pfp.png"
                };
                _context.Users.Add(instructor);
                await _context.SaveChangesAsync();

                // Store the instructor's userId
                int userId = instructor.Id;

                // Mock HttpContext and ISession
                var mockHttpContext = new Mock<HttpContext>();
                var mockSession = new Mock<ISession>();

                // Simulate the session returning the instructor's userId when "CurrentUserId" is accessed
                byte[] userIdBytes = BitConverter.GetBytes(userId);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(userIdBytes);
                }
                byte[] idResult = userIdBytes;
                mockSession.Setup(s => s.TryGetValue("CurrentUserId", out idResult)).Returns(true);
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

                // Create a mock PageContext
                var mockPageContext = new Mock<PageContext>();
                mockPageContext.Object.HttpContext = mockHttpContext.Object;

                // Create the PageModel instance
                var pageModel = new CreateModel(_context)
                {
                    PageContext = mockPageContext.Object,
                    Course = new Course
                    {
                        // Set placeholder values, OnPostAsync should overwrite the InstructorId based on session
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
                };

                // Count how many courses the instructor is teaching initially
                int initialCourseCount = _context.Course.Count(c => c.InstructorId == userId);

                // Call OnPostAsync to simulate form submission
                var result = await pageModel.OnPostAsync();

                // Count how many courses the instructor is teaching after adding the course
                int finalCourseCount = _context.Course.Count(c => c.InstructorId == userId);

                // Assert that the number of courses has increased by one
                Assert.AreEqual(initialCourseCount + 1, finalCourseCount);

                // Optional: Clean up by removing the newly created course
                var createdCourse = _context.Course.FirstOrDefault(c => c.CourseNumber == "101A" && c.InstructorId == userId);
                if (createdCourse != null)
                {
                    _context.Course.Remove(createdCourse);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
