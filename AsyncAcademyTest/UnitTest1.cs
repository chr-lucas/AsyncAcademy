using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly string _connectionString = "OurConnectionStringHere"; // Replace with actual connection string

        [TestMethod]
        public void InstructorCanCreateCourseTest()
        {
            // Setup DbContextOptions for database
            var options = new DbContextOptionsBuilder<AsyncAcademyContext>()
                .UseSqlServer(_connectionString)  // Use the provided connection string
                .Options;

            // Create the context
            using (var _context = new AsyncAcademyContext(options))
            {
                // Start with known instructor user id
                int userId = 27;

                // Count how many courses the instructor is teaching initially
                int initialCourseCount = _context.Course.Count(c => c.InstructorId == userId);

                // Create a new course for the instructor
                var course = new Course
                {
                    CourseNumber = "101A",
                    Department = "CS",
                    Name = "Introduction to Programming",
                    Description = "An introductory course to programming with C#.",
                    CreditHours = 3,
                    InstructorId = userId,  // Associate the course with the instructor
                    StartTime = DateTime.Parse("09:00:00"),
                    EndTime = DateTime.Parse("11:00:00"),
                    Location = "Room 101",
                    StudentCapacity = 30,
                    StudentsEnrolled = 0,
                    MeetingTimeInfo = "MWF 9:00AM - 11:00AM",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(4)
                };

                _context.Course.Add(course);  // Add the course to the database
                _context.SaveChanges();       // Save the course to the database

                // Count how many courses the professor is teaching after adding the course
                int finalCourseCount = _context.Course.Count(c => c.InstructorId == userId);

                // Assert that the number of courses has increased by one
                Assert.AreEqual(initialCourseCount + 1, finalCourseCount);

                //Remove the newly created course after the assertion
                _context.Course.Remove(course);  // Delete the course from the database
                _context.SaveChanges();          // Save the changes to apply the deletion

            }
        }
    }
}
