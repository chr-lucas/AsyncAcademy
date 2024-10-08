using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAcademy.Models
{
    public class SeedData(AsyncAcademyContext context)
    {
        private readonly AsyncAcademyContext _context = context;
        [BindProperty]
        public User InstructorAccount { get; set; }
        [BindProperty]
        public User StudentAccount { get; set; }

        public void Initialize(IServiceProvider serviceProvider)
        {
            var passwordHasher = new PasswordHasher<User>(); // To hash User passwords during Users seed

            using (var context = new AsyncAcademyContext(serviceProvider.GetRequiredService<DbContextOptions<AsyncAcademyContext>>()))
            {
                if (context == null || context.Course == null || context.Users == null || context.Department == null)
                {
                    throw new ArgumentNullException("Null AsyncAcademyContext"); // DB does not have required tables
                }

                // Look for any courses, users, departments, enrollments, assignments.
                if (context.Course.Any() || context.Users.Any() || context.Department.Any() || context.Enrollments.Any() || context.Assignment.Any())
                {
                    return;   // DB already has data
                }

                // Create 1 test instructor (instructortest) and 1 test student (studenttest)
                // Password for both is Pass1234
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
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Users ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Users OFF");
                    //
                    transaction.Commit();
                }

                // Test instructor creates 4 test courses
                // All 4 Course.Name attributes start with "TEST - " for filtering from live views
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Course.AddRange(
                    new Course
                    {
                        Id = 1,
                        CourseNumber = "3750",
                        Department = "CS",
                        Name = "TEST - Software Engineering 2",
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
                        Name = "TEST - Operating Systems",
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
                        Name = "TEST - Intro to Interact Entertainment",
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
                        Name = "TEST - Fundamentals of CS",
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
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Course ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Course OFF");
                    //
                    transaction.Commit();
                }

                // Seeded Department codes for Course Creation workflow dropdown
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
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Department ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Department OFF");
                    //
                    transaction.Commit();
                }


                // Test student account enrolls in all 4 test courses
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Enrollments.AddRange(
                        new Enrollment
                        {
                            Id = 1,
                            UserId = 2,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 2,
                            UserId = 2,
                            CourseId = 2
                        },
                        new Enrollment
                        {
                            Id = 3,
                            UserId = 2,
                            CourseId = 3
                        },
                        new Enrollment
                        {
                            Id = 4,
                            UserId = 2,
                            CourseId = 4
                        });
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Enrollments ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Enrollments OFF");
                    //
                    transaction.Commit();
                }

                // Populate each test course with 2 assignments each.
                // Due dates are hardcoded and will need to be updated throughout semester
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Id = 1,
                            CourseId = 1,
                            Title = "Test Intro Assignment",
                            Description = "Introduce yourself to the class.",
                            MaxPoints = 20,
                            Due = DateTime.Parse("October 14, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 2,
                            CourseId = 1,
                            Title = "Form teams",
                            Description = "Enter the name of your team and your teamate's names.",
                            MaxPoints = 50,
                            Due = DateTime.Parse("October 21, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 3,
                            CourseId = 2,
                            Title = "Installing an OS",
                            Description = "Install our learning OS on your machine.",
                            MaxPoints = 80,
                            Due = DateTime.Parse("October 16, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 4,
                            CourseId = 2,
                            Title = "Setup your IDE",
                            Description = "Install the software needed for the course.",
                            MaxPoints = 50,
                            Due = DateTime.Parse("October 13, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 5,
                            CourseId = 3,
                            Title = "Icebreaker",
                            Description = "What is your favorite video game?",
                            MaxPoints = 10,
                            Due = DateTime.Parse("October 14, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 6,
                            CourseId = 3,
                            Title = "Design Fundamentals",
                            Description = "Create a design document for the game \"Pong\".",
                            MaxPoints = 50,
                            Due = DateTime.Parse("October 18, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 7,
                            CourseId = 4,
                            Title = "What is Computer Science",
                            Description = "What are your main takeaways from the article?",
                            MaxPoints = 100,
                            Due = DateTime.Parse("October 17, 2024 11:59:00PM"),
                            Type = "textentry"
                        },
                        new Assignment
                        {
                            Id = 8,
                            CourseId = 4,
                            Title = "Hello World",
                            Description = "Complete the 'Hello World' tutorial in Python.",
                            MaxPoints = 100,
                            Due = DateTime.Parse("October 15, 2024 11:59:00PM"),
                            Type = "textentry"
                        });
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Assignment ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Assignment OFF");
                    //
                    transaction.Commit();
                }

                context.SaveChanges(); // Save all transactions to DB.
            }
        }
    }

}