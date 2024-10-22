using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTestAddCourse
    {
        // Private variable to store the DbContext options for the in-memory database
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;

        // This method is run before each test to set up the in-memory database
        [TestInitialize]
        public async Task Setup()
        {
            // Generate a unique database name using a GUID
            string uniqueDbName = "AsyncAcademyTestDb_" + Guid.NewGuid().ToString();

            // Configure an in-memory database for this specific test
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName) // Specific database name for this test
                .Options;

            // Clear any existing data from the in-memory database before each test
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                context.Course.RemoveRange(context.Course); // Remove all existing courses
                await context.SaveChangesAsync(); // Save changes to ensure the database is clean
            }
        }

        // Helper method to add a test course to the database
        private async Task<int> SeedCourse(AsyncAcademyContext context)
        {
            // Create a new Course object with predefined properties
            var testCourse = new Course
            {
                InstructorId = 1, // Assuming there is an instructor with ID 1
                CourseNumber = "101B",
                Department = "Math",
                Name = "Introduction to Algebra",
                Description = "An introductory course to algebra.",
                CreditHours = 3,
                StartTime = DateTime.Parse("09:00:00"),
                EndTime = DateTime.Parse("11:00:00"),
                Location = "Room202",
                StudentCapacity = 30,
                StudentsEnrolled = 0,
                MeetingTimeInfo = "TTh 9:00AM-11:00AM",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(4)
            };

            // Add the new course to the context and save it to the in-memory database
            context.Course.Add(testCourse);
            await context.SaveChangesAsync(); // Commit the changes to the database
            return testCourse.Id; // Return the ID of the newly added course
        }

        // Test method to verify that a course can be successfully added to the database
        [TestMethod]
        public async Task TestAddCourse()
        {
            // Create a new context for the in-memory database
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                // Seed the test course into the database and capture its ID
                int courseId = await SeedCourse(context);

                // Fetch the added course using its ID and check if it exists
                var course = await context.Course.FindAsync(courseId);

                // Perform assertions to verify that the course was added correctly
                Assert.IsNotNull(course, "Course should be successfully added to the catalog.");
                Assert.AreEqual("Introduction to Algebra", course.Name, "Course name should match.");
                Assert.AreEqual("Math", course.Department, "Course department should match.");
                Assert.AreEqual("An introductory course to algebra.", course.Description, "Course description should match.");
            }
        }
    }
}
