using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using AsyncAcademy.Pages.Course_Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AsyncAcademy.Models
{
    public class SeedData
    {
        public User Student { get; set; }
        public User Instructor { get; set; }

        public void Initialize(IServiceProvider serviceProvider)
        {
            var passwordHasher = new PasswordHasher<User>();

            using (var context = new AsyncAcademyContext(serviceProvider.GetRequiredService<DbContextOptions<AsyncAcademyContext>>()))
            {
                if (context == null || context.Course == null || context.Users == null || context.Department == null)
                {
                    throw new ArgumentNullException("Null AsyncAcademyContext");
                }

                // Look for any courses.
                if (context.Course.Any() || context.Users.Any() || context.Department.Any())
                {
                    return;   // DB already has data
                }

                context.Course.AddRange(
                    new Course
                    {
                        Id = 1,
                        CourseNumber = "3750",
                        Department = "CS",
                        Name = "Software Engineering 2",
                        Description = "Engineer Software like a pro",
                        CreditHours = 4,
                        InstructorId = 1,
                        StartTime = DateTime.Parse("9/1/2024 8:00:00 AM"),
                        EndTime = DateTime.Parse("9/1/2024 10:00:00 AM"),
                        Location = "WSU Ogden Campus",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Tuesday, Thursday",
                        StartDate = DateTime.Parse("9/1/2024 8:00:00 AM"),
                        EndDate = DateTime.Parse("12/15/2024 10:00:00 AM")
                    },

                    new Course
                    {
                        Id = 2,
                        CourseNumber = "3100",
                        Department = "CS",
                        Name = "Operating Systems",
                        Description = "Operate systems like a pro",
                        CreditHours = 4,
                        InstructorId = 1,
                        StartTime = DateTime.Parse("9/1/2024 11:30:00 AM"),
                        EndTime = DateTime.Parse("9/1/2024 12:50:00 PM"),
                        Location = "WSU Ogden Campus",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Monday, Wednesday",
                        StartDate = DateTime.Parse("9/1/2024 11:30:00 AM"),
                        EndDate = DateTime.Parse("12/15/2024 12:50:00 PM")
                    },

                    new Course
                    {
                        Id = 3,
                        CourseNumber = "1010",
                        Department = "CS",
                        Name = "Intro to Interact Entertainment",
                        Description = "Develop games like a pro",
                        CreditHours = 3,
                        InstructorId = 1,
                        StartTime = DateTime.Parse("9/1/2024 10:30:00 AM"),
                        EndTime = DateTime.Parse("9/1/2024 11:30:00 AM"),
                        Location = "WSU Davis",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Monday, Wednesday, Friday",
                        StartDate = DateTime.Parse("9/1/2024 10:30:00 AM"),
                        EndDate = DateTime.Parse("12/15/2024 11:30:00 AM")
                    },

                    new Course
                    {
                        Id = 4,
                        CourseNumber = "1030",
                        Department = "CS",
                        Name = "Fundamentals of CS",
                        Description = "Understand basic concepts relating to computer science like a pro",
                        CreditHours = 3,
                        InstructorId = 1,
                        StartTime = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndTime = DateTime.Parse("9/1/2024 4:30:00 PM"),
                        Location = "Online",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Monday, Tuesday, Wednesday, Thursday, Friday",
                        StartDate = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndDate = DateTime.Parse("12/15/2024 4:30:00 PM")
                    }
                );

                context.Department.AddRange(
                    new Department
                    {
                        Id = 1,
                        NameShort = "CS",
                        NameLong = "Computer Science"
                    },

                    new Department
                    {
                        Id = 2,
                        NameShort = "MATH",
                        NameLong = "Math"
                    },

                    new Department
                    {
                        Id = 3,
                        NameShort = "NET",
                        NameLong = "Network Management"
                    },

                    new Department
                    {
                        Id = 4,
                        NameShort = "MUSC",
                        NameLong = "Music"
                    },

                    new Department
                    {
                        Id = 5,
                        NameShort = "SE",
                        NameLong = "Systems Engineering"
                    },

                    new Department
                    {
                        Id = 6,
                        NameShort = "ENG",
                        NameLong = "Engineering"
                    }

                );

                CreateTestInstructor();
                CreateTestStudent();
                // Hash passwords for seed users prior to insert
                Instructor.Pass = passwordHasher.HashPassword(Instructor, Instructor.Pass);
                Student.Pass = passwordHasher.HashPassword(Student, Student.Pass);

                context.Users.AddRange(
                    Instructor,
                    Student
                );

                context.SaveChanges();
            }
        }

        public void CreateTestInstructor()
        { 
            Instructor.Id = 1;
            Instructor.Username = "instructortest";
            Instructor.FirstName = "Test";
            Instructor.LastName = "Instructor";
            Instructor.Mail = "instructor@test.com";
            Instructor.Pass = "Pass1234";
            Instructor.ConfirmPass = "Pass1234";
            Instructor.Birthday = DateTime.Parse("January 1, 1980");
            Instructor.IsProfessor = true;
            Instructor.ProfilePath = "/images/default_pfp.png";
            Instructor.Addr_Street = null;
            Instructor.Addr_City = null;
            Instructor.Addr_State = null;
            Instructor.Addr_Zip = null;
            Instructor.Phone = null;
        }

        public void CreateTestStudent()
        {
            Student.Id = 2;
            Student.Username = "studenttest";
            Student.FirstName = "Test";
            Student.LastName = "Student";
            Student.Mail = "student@test.com";
            Student.Pass = "Pass1234";
            Student.ConfirmPass = "Pass1234";
            Student.Birthday = DateTime.Parse("December 31, 2000");
            Student.IsProfessor = false;
            Student.ProfilePath = "/images/default_pfp.png";
            Student.Addr_Street = null;
            Student.Addr_City = null;
            Student.Addr_State = null;
            Student.Addr_Zip = null;
            Student.Phone = null;
        }
    }
}
