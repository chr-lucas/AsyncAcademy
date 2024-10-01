using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AsyncAcademy.Models
{
    public class SeedData(AsyncAcademyContext context)
    {
        private readonly AsyncAcademyContext _context = context;
        [BindProperty]
        public User InstructorAccount { get; set; } = default!;
        [BindProperty]
        public User StudentAccount { get; set; } = default!;

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

                using (var transaction = context.Database.BeginTransaction())
                {
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
                    });
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Course ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Course OFF");
                    transaction.Commit();
                }

                using (var transaction = context.Database.BeginTransaction())
                {

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
                        });

                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Department ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Department OFF");
                    transaction.Commit();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    string pass = "Pass1234";
                    context.Users.AddRange(
                        new User
                        {
                            Id = 1,
                            Username = "instructortest",
                            FirstName = "Test",
                            LastName = "Instructor",
                            Mail = "instructor@test.com",
                            Pass = passwordHasher.HashPassword(InstructorAccount, pass),
                            ConfirmPass = "Pass1234",
                            Birthday = DateTime.Parse("January 1, 1980"),
                            IsProfessor = true,
                            ProfilePath = "/images/default_pfp.png",
                            Addr_Street = null,
                            Addr_City = null,
                            Addr_State = null,
                            Addr_Zip = null,
                            Phone = null
                        },
                        new User
                        {
                            Id = 2,
                            Username = "studenttest",
                            FirstName = "Test",
                            LastName = "Student",
                            Mail = "student@test.com",
                            Pass = passwordHasher.HashPassword(StudentAccount, pass),
                            ConfirmPass = "Pass1234",
                            Birthday = DateTime.Parse("December 31, 2000"),
                            IsProfessor = false,
                            ProfilePath = "/images/default_pfp.png",
                            Addr_Street = null,
                            Addr_City = null,
                            Addr_State = null,
                            Addr_Zip = null,
                            Phone = null
                        });

                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Users ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Users OFF");
                    transaction.Commit();
                }

                //// Hash passwords for seed users prior to insert
                //InstructorAccount.Pass = passwordHasher.HashPassword(InstructorAccount, InstructorAccount.Pass);
                //StudentAccount.Pass = passwordHasher.HashPassword(StudentAccount, StudentAccount.Pass);

                context.SaveChanges();
            }
        }
    }

}