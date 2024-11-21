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

                // Look for any courses, users.
                if (context.Course.Any() || context.Users.Any())
                {
                    return;   // DB already has data
                }

                // Create 1 test instructor (instructortest) and 1 test student (studenttest)
                // Password for both is Pass1234
                // User 1 is the Test Instructor (instructortest)
                // User 2 is the Test Student (studenttest)
                // Users 3 - 28 are generic students with pre-made data to populate course graphs
                // Users 29, 30 are generic instructors with pre-made courses to populate the course catalog
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
                        },
                        new User
                        {
                            Id = 3,
                            Username = "aalexson",
                            FirstName = "Alex",
                            LastName = "Alexson",
                            Mail = "aalexson@test.com",
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
                        },
                        new User
                        {
                            Id = 4,
                            Username = "bbattle",
                            FirstName = "Brit",
                            LastName = "Battle",
                            Mail = "bbattle@test.com",
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
                        },
                        new User
                        {
                            Id = 5,
                            Username = "cclark",
                            FirstName = "Charlie",
                            LastName = "Clark",
                            Mail = "cclark@test.com",
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
                        },
                        new User
                        {
                            Id = 6,
                            Username = "ddawes",
                            FirstName = "Daisy",
                            LastName = "Dawes",
                            Mail = "ddawes@test.com",
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
                        },
                        new User
                        {
                            Id = 7,
                            Username = "eericson",
                            FirstName = "Emily",
                            LastName = "Ericson",
                            Mail = "eericson@test.com",
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
                        },
                        new User
                        {
                            Id = 8,
                            Username = "ffont",
                            FirstName = "Frank",
                            LastName = "Font",
                            Mail = "ffont@test.com",
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
                        },
                        new User
                        {
                            Id = 9,
                            Username = "ggarvey",
                            FirstName = "Grant",
                            LastName = "Garvey",
                            Mail = "ggarvey@test.com",
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
                        },
                        new User
                        {
                            Id = 10,
                            Username = "hhall",
                            FirstName = "Helena",
                            LastName = "Hall",
                            Mail = "hhall@test.com",
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
                        },
                        new User
                        {
                            Id = 11,
                            Username = "iichiba",
                            FirstName = "Ito",
                            LastName = "Ichiba",
                            Mail = "iichiba@test.com",
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
                        },
                        new User
                        {
                            Id = 12,
                            Username = "jjones",
                            FirstName = "Jamie",
                            LastName = "Jones",
                            Mail = "jjones@test.com",
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
                        },
                        new User
                        {
                            Id = 13,
                            Username = "kkates",
                            FirstName = "Karen",
                            LastName = "Kates",
                            Mail = "kkates@test.com",
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
                        },
                        new User
                        {
                            Id = 14,
                            Username = "lloggins",
                            FirstName = "Lucien",
                            LastName = "Loggins",
                            Mail = "lloggins@test.com",
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
                        },
                        new User
                        {
                            Id = 15,
                            Username = "mmahoney",
                            FirstName = "Maple",
                            LastName = "Mahoney",
                            Mail = "mmahoney@test.com",
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
                        },
                        new User
                        {
                            Id = 16,
                            Username = "nnorth",
                            FirstName = "Norma",
                            LastName = "North",
                            Mail = "nnorth@test.com",
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
                        },
                        new User
                        {
                            Id = 17,
                            Username = "ooconnor",
                            FirstName = "Ophelia",
                            LastName = "O'Connor",
                            Mail = "ooconnor@test.com",
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
                        },
                        new User
                        {
                            Id = 18,
                            Username = "ppeters",
                            FirstName = "Parker",
                            LastName = "Peters",
                            Mail = "ppeters@test.com",
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
                        },
                        new User
                        {
                            Id = 19,
                            Username = "qqueen",
                            FirstName = "Quinn",
                            LastName = "Queen",
                            Mail = "qqueen@test.com",
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
                        },
                        new User
                        {
                            Id = 20,
                            Username = "rralston",
                            FirstName = "Rachael",
                            LastName = "Ralston",
                            Mail = "rralston@test.com",
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
                        },
                        new User
                        {
                            Id = 21,
                            Username = "ssmith",
                            FirstName = "Samantha",
                            LastName = "Smith",
                            Mail = "ssmith@test.com",
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
                        },
                        new User
                        {
                            Id = 22,
                            Username = "tthompson",
                            FirstName = "Terry",
                            LastName = "Thompson",
                            Mail = "tthompson@test.com",
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
                        },
                        new User
                        {
                            Id = 23,
                            Username = "uusman",
                            FirstName = "Uzair",
                            LastName = "Usman",
                            Mail = "uusman@test.com",
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
                        },
                        new User
                        {
                            Id = 24,
                            Username = "vvickers",
                            FirstName = "Vernon",
                            LastName = "Vickers",
                            Mail = "vvickers@test.com",
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
                        },
                        new User
                        {
                            Id = 25,
                            Username = "wwoods",
                            FirstName = "Wilson",
                            LastName = "Woods",
                            Mail = "wwoods@test.com",
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
                        },
                        new User
                        {
                            Id = 26,
                            Username = "xxander",
                            FirstName = "Xavier",
                            LastName = "Xander",
                            Mail = "xxander@test.com",
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
                        },
                        new User
                        {
                            Id = 27,
                            Username = "yyoung",
                            FirstName = "Yancy",
                            LastName = "Young",
                            Mail = "yyoung@test.com",
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
                        },
                        new User
                        {
                            Id = 28,
                            Username = "zzane",
                            FirstName = "Zeza",
                            LastName = "Zane",
                            Mail = "zzane@test.com",
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
                        },
                        new User
                        {
                            Id = 29,
                            Username = "cacademia",
                            FirstName = "Clive",
                            LastName = "Academia",
                            Mail = "instructorclive@test.com",
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
                            Id = 30,
                            Username = "ateacher",
                            FirstName = "Amanda",
                            LastName = "Teacher",
                            Mail = "instructoramanda@test.com",
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
                        });
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Users ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Users OFF");
                    //
                    transaction.Commit();
                }

                // Test instructor creates 4 test courses (Should add a fifth as part of the demo)
                // Generic instructors create 2 courses each to fill out course catalog
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
                        StudentsEnrolled = 27,
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
                        StudentsEnrolled = 1,
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
                        StudentsEnrolled = 1,
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
                        StudentsEnrolled = 1,
                        MeetingTimeInfo = "Monday, Tuesday, Wednesday, Thursday, Friday",
                        StartDate = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndDate = DateTime.Parse("12/15/2024 4:30:00 PM")
                    },
                    new Course
                    {
                        Id = 5,
                        CourseNumber = "1060",
                        Department = "MATH",
                        Name = "Trigonometry",
                        Description = "Foundations of trigonometry and related methods",
                        CreditHours = 3,
                        InstructorId = 29,
                        StartTime = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndTime = DateTime.Parse("9/1/2024 4:30:00 PM"),
                        Location = "Online",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Tuesday, Thursday",
                        StartDate = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndDate = DateTime.Parse("12/15/2024 4:30:00 PM")
                    },
                    new Course
                    {
                        Id = 6,
                        CourseNumber = "1210",
                        Department = "MATH",
                        Name = "Calculus I",
                        Description = "The foundations of calculus and related methods",
                        CreditHours = 3,
                        InstructorId = 29,
                        StartTime = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndTime = DateTime.Parse("9/1/2024 4:30:00 PM"),
                        Location = "Online",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Monday, Tuesday, Wednesday, Thursday, Friday",
                        StartDate = DateTime.Parse("9/1/2024 2:00:00 PM"),
                        EndDate = DateTime.Parse("12/15/2024 4:30:00 PM")
                    },
                    new Course
                    {
                        Id = 7,
                        CourseNumber = "1040",
                        Department = "MUSC",
                        Name = "Music of World Cultures",
                        Description = "An introduction to the music of cultures around the world.",
                        CreditHours = 3,
                        InstructorId = 30,
                        StartTime = DateTime.Parse("9/1/2024 1:00:00 PM"),
                        EndTime = DateTime.Parse("9/1/2024 2:30:00 PM"),
                        Location = "Online",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Monday, Wednesday, Friday",
                        StartDate = DateTime.Parse("9/1/2024 1:00:00 PM"),
                        EndDate = DateTime.Parse("12/15/2024 2:30:00 PM")
                    },
                    new Course
                    {
                        Id = 8,
                        CourseNumber = "1030",
                        Department = "MUSC",
                        Name = "Introduction to Jazz",
                        Description = "A survey of jazz in America, including blues, ragtime, and traditional jazz.",
                        CreditHours = 3,
                        InstructorId = 30,
                        StartTime = DateTime.Parse("9/1/2024 5:00:00 PM"),
                        EndTime = DateTime.Parse("9/1/2024 6:30:00 PM"),
                        Location = "Online",
                        StudentCapacity = 50,
                        StudentsEnrolled = 0,
                        MeetingTimeInfo = "Monday, Tuesday, Wednesday, Thursday, Friday",
                        StartDate = DateTime.Parse("9/1/2024 5:00:00 PM"),
                        EndDate = DateTime.Parse("12/15/2024 6:30:00 PM")
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


                // Test student account enrolls in all 4 test courses from the Test Instructor
                // Generic Student accounts all enroll in CourseID 1 (Software Eng II)
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
                        },
                        new Enrollment
                        {
                            Id = 5,
                            UserId = 3,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 6,
                            UserId = 4,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 7,
                            UserId = 5,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 8,
                            UserId = 6,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 9,
                            UserId = 7,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 10,
                            UserId = 8,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 11,
                            UserId = 9,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 12,
                            UserId = 10,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 13,
                            UserId = 11,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 14,
                            UserId = 12,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 15,
                            UserId = 13,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 16,
                            UserId = 14,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 17,
                            UserId = 15,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 18,
                            UserId = 16,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 19,
                            UserId = 17,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 20,
                            UserId = 18,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 21,
                            UserId = 19,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 22,
                            UserId = 20,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 23,
                            UserId = 21,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 24,
                            UserId = 22,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 25,
                            UserId = 23,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 26,
                            UserId = 24,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 27,
                            UserId = 25,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 28,
                            UserId = 26,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 29,
                            UserId = 27,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 30,
                            UserId = 28,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 31,
                            UserId = 1,
                            CourseId = 1
                        },
                        new Enrollment
                        {
                            Id = 32,
                            UserId = 1,
                            CourseId = 2
                        },
                        new Enrollment
                        {
                            Id = 33,
                            UserId = 1,
                            CourseId = 3
                        },
                        new Enrollment
                        {
                            Id = 34,
                            UserId = 1,
                            CourseId = 4
                        },
                        new Enrollment
                        {
                            Id = 35,
                            UserId = 29,
                            CourseId = 5
                        },
                        new Enrollment
                        {
                            Id = 36,
                            UserId = 29,
                            CourseId = 6
                        },
                        new Enrollment
                        {
                            Id = 37,
                            UserId = 30,
                            CourseId = 7
                        },
                        new Enrollment
                        {
                            Id = 38,
                            UserId = 30,
                            CourseId = 8
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

                // Add test submissions for testing grading chart.
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Submissions.AddRange(
                        new Submission
                        {
                            Id = 1,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 3,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 2,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 4,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 3,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 5,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 4,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 6,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 5,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 7,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 6,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 8,
                            Timestamp = DateTime.Now,
                            PointsGraded = 17,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 7,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 9,
                            Timestamp = DateTime.Now,
                            PointsGraded = 15,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 8,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 10,
                            Timestamp = DateTime.Now,
                            PointsGraded = 18,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 9,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 11,
                            Timestamp = DateTime.Now,
                            PointsGraded = 20,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 10,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 12,
                            Timestamp = DateTime.Now,
                            PointsGraded = 20,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 11,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 13,
                            Timestamp = DateTime.Now,
                            PointsGraded = 17,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 12,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 14,
                            Timestamp = DateTime.Now,
                            PointsGraded = 11,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 13,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 15,
                            Timestamp = DateTime.Now,
                            PointsGraded = 14,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 14,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 16,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 15,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 17,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 16,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 18,
                            Timestamp = DateTime.Now,
                            PointsGraded = 18,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 17,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 19,
                            Timestamp = DateTime.Now,
                            PointsGraded = 18,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 18,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 20,
                            Timestamp = DateTime.Now,
                            PointsGraded = 18,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 19,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 21,
                            Timestamp = DateTime.Now,
                            PointsGraded = 18,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 20,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 22,
                            Timestamp = DateTime.Now,
                            PointsGraded = 16,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 21,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 23,
                            Timestamp = DateTime.Now,
                            PointsGraded = 17,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 22,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 24,
                            Timestamp = DateTime.Now,
                            PointsGraded = 6,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 23,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 25,
                            Timestamp = DateTime.Now,
                            PointsGraded = 19,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 24,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 26,
                            Timestamp = DateTime.Now,
                            PointsGraded = 18,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 25,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 27,
                            Timestamp = DateTime.Now,
                            PointsGraded = 17,
                            IsNew = false
                        },
                        new Submission
                        {
                            Id = 26,
                            Content = "Hi there!",
                            AssignmentId = 1,
                            UserId = 28,
                            Timestamp = DateTime.Now,
                            PointsGraded = 17,
                            IsNew = false
                        });
                    // Temporarily override DB controlled primary key
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Submissions ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Submissions OFF");
                    //
                    transaction.Commit();
                }

                        // Old Submission generation code
                        // Unsure if it is still needed
                        //new Submission
                        //{
                        //    Id = 1,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 20,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 94
                        //},
                        //new Submission
                        //{
                        //    Id = 2,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 21,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 90
                        //},
                        //new Submission
                        //{
                        //    Id = 3,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 22,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 83
                        //},
                        //new Submission
                        //{
                        //    Id = 4,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 23,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 72
                        //},
                        //new Submission
                        //{
                        //    Id = 5,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 24,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 61
                        //},
                        //new Submission
                        //{
                        //    Id = 6,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 25,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 99
                        //},
                        //new Submission
                        //{
                        //    Id = 7,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 26,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 0
                        //},
                        //new Submission
                        //{
                        //    Id = 8,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 27,
                        //    Timestamp = DateTime.Now,
                        //    PointsGraded = 30
                        //},
                        //new Submission
                        //{
                        //    Id = 9,
                        //    Content = "Some Text",
                        //    AssignmentId = 77,
                        //    UserId = 28,
                        //    Timestamp = DateTime.Now
                        //});

                context.SaveChanges(); // Save all transactions to DB.
            }
        }
    }

}