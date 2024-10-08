using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Models;

namespace AsyncAcademy.Data
{
    public class AsyncAcademyContext : DbContext
    {
        public AsyncAcademyContext (DbContextOptions<AsyncAcademyContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Enrollment> Enrollments { get; set; } = default!;
        public DbSet<Assignment> Assignment { get; set; } = default!;
        public DbSet<Course> Course { get; set; } = default!;
        public DbSet<Department> Department { get; set; } = default!;
        public DbSet<Submission> Submissions { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
    }
}
