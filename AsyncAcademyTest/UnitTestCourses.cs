using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http;
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
    public class UnitTestCourses
    {
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;
        }

        private async Task<User> SeedInstructor(AsyncAcademyContext context)
        {
            var instructor = new User
            {
                Username = "professor123",
                FirstName = "John",
                LastName = "Doe",
                Mail = "john.doe@example.com",
                Pass = "ValidPassword1",
                ConfirmPass = "ValidPassword1",
                Birthday = new DateTime(1980, 5, 20),
                IsProfessor = true,
                Addr_Street = "123 Main St",
                Addr_City = "Springfield",
                Addr_State = "UT",
                Addr_Zip = "12345",
                Phone = "0123456789",
                ProfilePath = "/images/default_pfp.png"
            };

            context.Users.Add(instructor);
            await context.SaveChangesAsync();
            return instructor;
        }

        private Mock<HttpContext> SetupHttpContext(int userId)
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();

            byte[] userIdBytes = BitConverter.GetBytes(userId);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(userIdBytes);
            }

            mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdBytes)).Returns(true);
            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

            return mockHttpContext;
        }

        private CreateModel CreatePageModel(AsyncAcademyContext context, Mock<HttpContext> mockHttpContext)
        {
            var mockPageContext = new Mock<PageContext>
            {
                Object = { HttpContext = mockHttpContext.Object }
            };

            return new CreateModel(context)
            {
                PageContext = mockPageContext.Object,
                Course = new Course
                {
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
                }
            };
        }

        private async Task VerifyCourseCreation(AsyncAcademyContext context, int userId, string courseNumber)
        {
            // Assert that the course was created
            var createdCourse = await context.Course.FirstOrDefaultAsync(c => c.CourseNumber == courseNumber && c.InstructorId == userId);
            Assert.IsNotNull(createdCourse);
        }

        [TestMethod]
        public async Task InstructorCanCreateCourseTest()
        {
            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                // Seed the in-memory database with a mock instructor
                var instructor = await SeedInstructor(_context);
                int userId = instructor.Id;

                // Mock HttpContext
                var mockHttpContext = SetupHttpContext(userId);

                // Create the PageModel instance
                var pageModel = CreatePageModel(_context, mockHttpContext);

                // Count how many courses the instructor is teaching initially
                int initialCourseCount = _context.Course.Count(c => c.InstructorId == userId);

                // Call OnPostAsync to simulate form submission
                var result = await pageModel.OnPostAsync();

                // Count how many courses the instructor is teaching after adding the course
                int finalCourseCount = _context.Course.Count(c => c.InstructorId == userId);

                // Assert that the number of courses has increased by one
                Assert.AreEqual(initialCourseCount + 1, finalCourseCount);

                // Verify that the course was created
                await VerifyCourseCreation(_context, userId, "101A");

                // Optional: Clean up by removing the newly created course
                var createdCourse = await _context.Course.FirstOrDefaultAsync(c => c.CourseNumber == "101A" && c.InstructorId == userId);
                if (createdCourse != null)
                {
                    _context.Course.Remove(createdCourse);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
