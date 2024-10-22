using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Pages.Assignments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestInstructorGradesSubmission
    {
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;
        private Mock<HttpContext> _mockHttpContext;
        private Mock<Assignment> _mockAssignment;
        private Mock<Submission> _mockSubmission;
        private Mock<ISession> _mockSession;
        private Mock<PageContext> _mockPageContext;

        private Submission testingSubmission;


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
            _mockSubmission = new Mock<Submission>();
            _mockSession = new Mock<ISession>();
            _mockHttpContext.Setup(m => m.Session).Returns(_mockSession.Object);

            _mockPageContext = new Mock<PageContext>();
            _mockPageContext.Object.HttpContext = _mockHttpContext.Object;

            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                _context.Users.RemoveRange(_context.Users); // Clear existing data
                _context.Course.RemoveRange(_context.Course);
                _context.Assignment.RemoveRange(_context.Assignment);
                _context.Submissions.RemoveRange(_context.Submissions);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<int> SeedSubmission(AsyncAcademyContext context)
        {
            var testSubmission = new Submission
            {
                Content = "A well thought out answer",
                AssignmentId = 10,
                UserId = 4,
                PointsGraded = null,
                Timestamp = DateTime.Now
            };

            context.Submissions.Add(testSubmission);
            await context.SaveChangesAsync();
            testingSubmission = testSubmission;
            return testSubmission.Id;
        }

        private SubmissionDetailsModel CreateSubmissionDetailsModel(AsyncAcademyContext context, int submissionId)
        {
            return new SubmissionDetailsModel(context)
            {
                PageContext = _mockPageContext.Object,
                Submission = testingSubmission
            };
        }

        private void MockSessionValues(int userId, int submissionId)
        {
            byte[] userIdBytes = BitConverter.GetBytes(userId);
            if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
            _mockSession.Setup(s => s.TryGetValue("CurrentUserId", out userIdBytes)).Returns(true);

            byte[] submissionIdBytes = BitConverter.GetBytes(submissionId);
            if (BitConverter.IsLittleEndian) Array.Reverse(submissionIdBytes);
            _mockSession.Setup(s => s.TryGetValue("SubmissionId", out submissionIdBytes)).Returns(true);
        }

        private async Task VerifyGradeCreation(AsyncAcademyContext context, int submissionId)
        {
            int? startGrade = context.Submissions.Where(s => s.Id == submissionId).FirstOrDefault().PointsGraded;

            var pageModel = CreateSubmissionDetailsModel(context, submissionId);
            var setup = await pageModel.OnGetAsync(submissionId);

            pageModel.Submission.PointsGraded = 90; // Update the pageModel's submission attributes
            var result = await pageModel.OnPostAsync();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), $"Expected a redirect after grading submission.");

            pageModel.Submission.PointsGraded = 10; // Change pageModel again to ensure our test value is coming exclusively from DB
            context.ChangeTracker.Clear(); // If DB has not saved, revert changes to the db
            int endGrade = (int)context.Submissions.Where(s => s.Id == submissionId).FirstOrDefault().PointsGraded; // Get grade value after OnPost method

            Assert.AreEqual(startGrade, null, $"Assignment initial grade expected to be null.");
            Assert.AreNotEqual(startGrade, endGrade, $"Assignment final grade expected to be not null."); // Should fail if db never saved
            Assert.AreNotEqual(pageModel.Submission.PointsGraded, endGrade, $"Assignment final grade expected to be not null."); // Should fail if db never saved
            Assert.AreEqual(endGrade, 90, $"Assignment final grade expected to be 90."); // Should fail if db never saved
        }

        [TestMethod]
        public async Task InstructorCanGradeTextEntryTest()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                int submissionId = await SeedSubmission(context);
                MockSessionValues(1, submissionId);
                await VerifyGradeCreation(context, submissionId);
            }
        }
    }
}
