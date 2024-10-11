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
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;
        private Mock<HttpContext> _mockHttpContext;
        private Mock<ISession> _mockSession;
        private Mock<PageContext> _mockPageContext;

        
        [TestInitialize]
        public async Task Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;

            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new Mock<ISession>();
            _mockHttpContext.Setup(m => m.Session).Returns(_mockSession.Object);

            _mockPageContext = new Mock<PageContext>();
            _mockPageContext.Object.HttpContext = _mockHttpContext.Object;

            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                _context.Users.RemoveRange(_context.Users); // Clear existing data
                await _context.SaveChangesAsync();
            }
        }


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

            context.Course.Add(testCourse);
            await context.SaveChangesAsync();
            return testCourse.Id;
        }

        private CreateModel CreateAssignmentModel(AsyncAcademyContext context, int courseId, string assignmentType)
        {
            return new CreateModel(context)
            {
                PageContext = _mockPageContext.Object,
                Assignment = new Assignment
                {
                    Title = $"Test{assignmentType}",
                    MaxPoints = 10,
                    Description = "Test",
                    Due = DateTime.Parse("2023-10-08 09:00:00"),
                    Type = assignmentType,
                    CourseId = courseId
                }
            };
        }

        private void MockSessionValues(int userId, int courseId)
        {
            byte[] userIdBytes = BitConverter.GetBytes(userId);
            if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
            _mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdBytes)).Returns(true);

            byte[] courseIdBytes = BitConverter.GetBytes(courseId);
            if (BitConverter.IsLittleEndian) Array.Reverse(courseIdBytes);
            _mockSession.Setup(s => s.TryGetValue("CourseId", out courseIdBytes)).Returns(true);
        }

        private async Task VerifyAssignmentCreation(AsyncAcademyContext context, int courseId, string assignmentType)
        {
            int initialCount = context.Assignment.Count(a => a.CourseId == courseId);

            var pageModel = CreateAssignmentModel(context, courseId, assignmentType);
            var result = await pageModel.OnPostAsync();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), $"Expected a redirect after creating {assignmentType} assignment.");
            int finalCount = context.Assignment.Count(a => a.CourseId == courseId);

            Assert.AreEqual(initialCount + 1, finalCount, $"Assignment count should increase by one after creating {assignmentType} assignment.");
        }

        [TestMethod]
        public async Task InstructorCanCreateTextAssignmentTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int courseId = await SeedCourse(context);
                MockSessionValues(1, courseId);
                await VerifyAssignmentCreation(context, courseId, "Text Entry");
            }
        }

        [TestMethod]
        public async Task InstructorCanCreateFileAssignmentTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int courseId = await SeedCourse(context);
                MockSessionValues(1, courseId);
                await VerifyAssignmentCreation(context, courseId, "File Submission");
            }
        }

        [TestMethod]
        public async Task InstructorCanCreateURLAssignmentTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int courseId = await SeedCourse(context);
                MockSessionValues(1, courseId);
                await VerifyAssignmentCreation(context, courseId, "URL");
            }
        }
    }
}
