using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    [TestClass]
    public class UnitTestStudentEnrollment
    {
        // Private fields to hold test-specific configurations and mock objects
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions; // Options for configuring in-memory test DB
        private Mock<HttpContext> _mockHttpContext; // Mocking HttpContext for session access
        private Mock<ISession> _mockSession; // Mocking session to simulate session variables
        private Mock<PageContext> _mockPageContext; // Mocking PageContext for Razor Page interactions

        //Setup method to initialize mocks and prepare the in-memory database:
        [TestInitialize]
        public async Task SetUp()
        {
            //We configure an in-memory database for testing:
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb")
                .Options;
            
            
            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new Mock<ISession>();
            _mockHttpContext.Setup(m => m.Session).Returns(_mockSession.Object); // Linking HttpContext with the mocked session

            //Setting up PageContext to use the mocked HttpContext:
            _mockPageContext = new Mock<PageContext>();
            _mockPageContext.Object.HttpContext = _mockHttpContext.Object;


            //Clear any existing data from the in-memory database (for a clean start in tests)
            using (var _context = new AsyncAcademyContext(_dbContextOptions))
            {
                _context.Enrollments.RemoveRange(_context.Enrollments);
                await _context.SaveChangesAsync();
            }
        }
    }
}
