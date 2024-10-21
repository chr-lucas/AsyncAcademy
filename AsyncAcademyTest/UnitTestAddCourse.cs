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
        private DbContextOptions<AsyncAcademyContext> _dbContextOptions;

        [TestInitialize]
        public async Task Setup()
        {
            // Configure an in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseInMemoryDatabase(databaseName: "AsyncAcademyTestDb_AddCourse")
                .Options;

            // Clear any existing data from the in-memory database (for a clean start in tests)
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                context.Course.RemoveRange(context.Course); // Remove all courses
                await context.SaveChangesAsync(); // Save changes to the database
            }
        }

        // Helper method to seed a test course into the database
        private async Task<int> SeedCourse(AsyncAcademyContext context)
        {
            var testCourse = new Course
            {
                InstructorId = 1, // Assuming instructor ID 1
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

            // Add the course to the database and save it
            context.Course.Add(testCourse);
            await context.SaveChangesAsync();
            return testCourse.Id; // Return the ID of the created course for testing
        }

        [TestMethod]
        public async Task TestAddCourse()
        {
            using (var context = new AsyncAcademyContext(_dbContextOptions))
            {
                // Seed a course into the database
                int courseId = await SeedCourse(context);

                // Verify the course was added successfully
                var course = await context.Course.FindAsync(courseId);
                Assert.IsNotNull(course, "Course should be successfully added to the catalog.");
                Assert.AreEqual("Introduction to Algebra", course.Name, "Course name should match.");
                Assert.AreEqual("Math", course.Department, "Course department should match.");
                Assert.AreEqual("An introductory course to algebra.", course.Description, "Course description should match.");
            }
        }
    }
}
