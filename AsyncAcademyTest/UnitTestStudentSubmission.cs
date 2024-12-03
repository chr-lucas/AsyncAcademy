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
using AsyncAcademy.Pages.Assignments;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestStudentTextSubmission
    {
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;
        private Mock<HttpContext> _mockHttpContext;
        private Mock<Assignment> _mockAssignment;
        private Mock<ISession> _mockSession;
        private Mock<PageContext> _mockPageContext;


        [TestInitialize]
        public async Task Setup()
        {
            // Generate a unique database name using a GUID
            string uniqueDbName = "AsyncAcademyTestDb_" + Guid.NewGuid().ToString();

            // Configure an in-memory database for this specific test
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName) // Specific database name for this test
                .Options;

            _mockHttpContext = new Mock<HttpContext>();
            _mockAssignment = new Mock<Assignment>();
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


        private async Task<int> SeedAssignment(AsyncAcademyContext context)
        {
            var testUser = new User
            {
                Id = 1,
                Username = "Test",
                FirstName = "Test",
                LastName = "Test",
                Mail = "test@test.com",
                Pass = "1234ABCD",
                ConfirmPass = "1234ABCD",
                IsProfessor = true,
                Addr_Street = null,
                Addr_City = null,
                Addr_State = null,
                Addr_Zip = null,
                Phone = null
            };

            context.Users.Add(testUser);
            await context.SaveChangesAsync();

            var testAssignment = new Assignment
            {
                CourseId = 1,
                Title = "Test Assignment",
                Description = "An assignment to test submissions",
                MaxPoints = 10,
                Due = DateTime.Now.AddDays(3),
                Type = "Text Entry"
            };

            context.Assignment.Add(testAssignment);
            await context.SaveChangesAsync();
            return testAssignment.Id;
        }

        private SubmitModel CreateSubmissionModel(AsyncAcademyContext context, int assignmentId)
        {
            return new SubmitModel(context)
            {
                PageContext = _mockPageContext.Object,
                Submission = new Submission
                {
                    Content = "This is my test submission",
                    AssignmentId = assignmentId,
                    UserId = 0, // only used to fulfill constructor requirement. Updated later.
                    PointsGraded = null,
                    Timestamp = DateTime.Now
                }
            };
        }

        private void MockSessionValues(int userId)
        {
            byte[] userIdBytes = BitConverter.GetBytes(userId);
            if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
            _mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdBytes)).Returns(true);

            //byte[] assignmentIdBytes = BitConverter.GetBytes(assignmentId);
            //if (BitConverter.IsLittleEndian) Array.Reverse(assignmentIdBytes);
            //_mockSession.Setup(s => s.TryGetValue("CourseId", out assignmentIdBytes)).Returns(true);
        }

        private async Task VerifySubmissionCreation(AsyncAcademyContext context, int assignmentId)
        {
            int initialCount = context.Submissions.Count(s => s.AssignmentId == assignmentId);

            var pageModel = CreateSubmissionModel(context, assignmentId);
            var setup = await pageModel.OnGetAsync(assignmentId);
            var result = await pageModel.OnPostAsync();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), $"Expected a redirect after creating submission.");
            int finalCount = context.Submissions.Count(s => s.AssignmentId == assignmentId);

            Assert.AreEqual(initialCount + 1, finalCount, $"Assignment count should increase by one after creating submission.");
        }

        [TestMethod]
        public async Task StudentCanSubmitTextEntryTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int assignmentId = await SeedAssignment(context);
                MockSessionValues(1);
                await VerifySubmissionCreation(context, assignmentId);
            }
        }
    }
}
