using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using AsyncAcademy.Pages;
using Microsoft.AspNetCore.Identity;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestSignup
    {
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;
        private Mock<HttpContext> _mockHttpContext;
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


        private SignupModel CreateSignupModel(AsyncAcademyContext context, User user)
        {
            return new SignupModel(context)
            {
                PageContext = _mockPageContext.Object,
                Account = user
            };
        }

        private User CreateTestUser(bool isProfessor)
        {
            return new User
            {
                Username = "Test",
                FirstName = "Test",
                LastName = "Test",
                Mail = "test@test.com",
                Pass = "TestPass123",
                ConfirmPass = "TestPass123",
                Birthday = new DateTime(1980, 5, 20),
                IsProfessor = isProfessor,
                Addr_City = "Test City",
                Addr_State = "Test State",
                Addr_Zip = "12345",
                Addr_Street = "123 Test St",
                Phone = "123-456-7890"
            };
        }

        private async Task VerifyUserInDb(AsyncAcademyContext context, bool isProfessor)
        {
            // Verify that the new user was added to the database
            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Username == "Test");
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("Test", userInDb.Username);
            Assert.AreEqual(isProfessor, userInDb.IsProfessor); // Ensure correct user role

            // Verify that the password is hashed
            var passwordHasher = new PasswordHasher<User>();
            var resultCheck = passwordHasher.VerifyHashedPassword(userInDb, userInDb.Pass, "TestPass123");
            Assert.AreEqual(PasswordVerificationResult.Success, resultCheck);
        }

        [TestMethod]
        public async Task UserSignupTestStudent()
        {
            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                var pageModel = CreateSignupModel(_context, CreateTestUser(false));

                // Act - Call the method that handles sign-up
                var result = await pageModel.OnPostAsync();

                // Assert - Check if the sign-up was successful
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult)); // Assuming success redirects
                var redirectResult = result as RedirectToPageResult;
                Assert.AreEqual("./welcome", redirectResult.PageName); // Ensure redirection to "welcome" page

                // Verify that the user was added to the database
                await VerifyUserInDb(_context, false); // false = student
            }
        }

        [TestMethod]
        public async Task UserSignupTestTeacher()
        {
            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                var pageModel = CreateSignupModel(_context, CreateTestUser(true));

                

                // Act - Call the method that handles sign-up
                var result = await pageModel.OnPostAsync();

                // Assert - Check if the sign-up was successful
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult)); // Assuming success redirects
                var redirectResult = result as RedirectToPageResult;
                Assert.AreEqual("./welcome", redirectResult.PageName); // Ensure redirection to "welcome" page

                // Verify that the user was added to the database
                await VerifyUserInDb(_context, true); // true = professor
            }
        }
    }
}
